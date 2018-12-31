//------------------------------------------------------------------------------
//-------------------------------------------------------------------------------
using System;
using System.Threading;
using System.Collections;
using System.Windows.Forms;
using System.Xml;
using AutoKillerScript;
using AKServer.DLL;

namespace DAoC_Bot
{
	/// <summary>
	/// All Possible actions
	/// </summary>
	public enum BotAction
	{
		/// <summary>Initializes the bot</summary>
		Initialize,
		/// <summary>Finds the closest waypoint and sets the waypoint iterator</summary>
		SetClosestWaypoint,
		/// <summary>Sets waypoint iterator to the next waypoint to fight at</summary>
		SetNextWaypoint,
		/// <summary>Moves to the targetted waypoint</summary>
		MoveToWaypoint,
		/// <summary>Find target</summary>
		FindTarget,
		/// <summary>Checks agro</summary>
		CheckAgro,
		/// <summary>Gets in range</summary>
		GetInRange,
		/// <summary>Starts the ranged fight sequence</summary>
		RangedFight,
		/// <summary>Starts the melee fight sequence</summary>
		MeleeFight,
		/// <summary>Decide if we have to flee</summary>
		CheckFlee,
		/// <summary>Flee</summary>
		Flee,
		/// <summary>Loot all nearby targets</summary>
		Loot,
		/// <summary>Rest</summary>
		Rest,
		/// <summary>Check buffs</summary>
		CheckBuffs,
		/// <summary>Release and run waypoints to patrol area</summary>
		DiedReleaseAndRun,
		/// <summary>Find grave</summary>
		FindGrave,
		/// <summary>Pray</summary>
		Pray,
		/// <summary>Check group members for aggro</summary>
		Protect,
		/// <summary>Paused or Stopped after death</summary>
		Pause
	}

	/// <summary>
	/// The basis for all bots
	/// </summary>
	public abstract class DAoC_BotBase
	{
		/// <summary>
		/// The AutoKillerScript.clsAutoKillerScript object
		/// </summary>
		public AutoKillerScript.clsAutoKillerScript _ak;

		/// <summary>
		/// The profile object
		/// </summary>
		public Profile profile;

		/// <summary>
		/// The PatrolAreas object
		/// </summary>
		public PatrolAreas patrolareas;

		/// <summary>
		/// The localization object
		/// </summary>
		public Localization localization;

		/// <summary>
		/// The cooldown manager
		/// </summary>
		public Cooldowns cooldowns;

		/// <summary>
		/// The bot's current action
		/// </summary>
		public BotAction Action;

		/// <summary>
		/// The bot's next actions
		/// </summary>
		public BotQueue ActionQueue;

		/// <summary>
		/// Random used to randomize timings within the bot
		/// </summary>
		public Random rnd = new Random();
				
		/// <summary>
		/// List of all key words for loot we can trash, like "ragged", etc.
		/// </summary>
		public ArrayList ListOfBadLoot;
		
		/// <summary>
		/// Ignore arrays
		/// </summary>
		public ArrayList IgnoreLootObjects;
		public ArrayList IgnoreSkinnableObjects;
		public ArrayList IgnoreTargetObjects;
		
		/// <summary>
		/// flag to change direction when we start to flee through waypoints
		/// </summary>
		protected bool ChangedDirection = false;

		/// <summary>
		/// Our current PatrolArea
		/// </summary>
		//internal PatrolArea patrolarea = null;

		/// <summary>
		/// Our current corpserun Travel Path
		/// </summary>
		//internal PatrolArea corpserun = null;

		/// <summary>
		/// flag to pause DoAction
		/// </summary>
		internal bool bPaused = false;

		/// <summary>
		/// ID of pet, if any
		/// </summary>
		internal int petID = -1;

		/// <summary>
		/// flag set if we got chat.log message "You can't see", reset in FindTarget
		/// </summary>
		internal bool bNoLineOfSight = false;
		
		/// <summary>
		/// Keys from games settings files
		/// </summary>
		internal AutoKillerScript.UserKeys PlayerKeys; 	
		
											   /// <summary>
		/// Event to send messages to the Statistics form's log tab
		/// </summary>
		public event UpdateStatusDelegate UpdateStatus;
		public delegate void UpdateStatusDelegate( string message);

		internal DateTime last = DateTime.Now ;
		internal DateTime first = DateTime.Now ;

		internal DateTime LastEmote = DateTime.Now ;

		/// <summary>
		/// Used to control DoMoveToWaypoints
		/// true is used while heading to next fight point, to find targets as we run
		/// false is used while returning to a waypoint from which we wandered too far
		/// </summary>
		internal bool CheckForTargets = true;

		private ArrayList LoadNamesXml( string xmlfile, string xpath)
		{
			ArrayList result = new ArrayList();
			XmlDocument doc = new XmlDocument();
			try
			{
				doc.Load( xmlfile);
				foreach( XmlNode node in doc.SelectNodes( xpath))
					result.Add( node.Value);
			}
			catch
			{
			}

			return result;
		}

		private ArrayList LoadIdsXml( string xmlfile, string xpath)
		{
			ArrayList result = new ArrayList();
			XmlDocument doc = new XmlDocument();
			try
			{
				doc.Load( xmlfile);
				foreach( XmlNode node in doc.SelectNodes( xpath))
					result.Add( int.Parse( node.Value));
			}
			catch
			{
			}

			return result;
		}

		/// <summary>
		/// Initiliazes the bot
		/// </summary>
		/// <param name="ak">The AutoKillerScript object</param>
		public DAoC_BotBase( AutoKillerScript.clsAutoKillerScript ak, Profile profile, PatrolAreas patrolareas)
		{
			_ak = ak;
			_ak.OnRegExTrue += new AutoKillerScript.clsAutoKillerScript.OnRegExTrueEventHandler(ParseChat);

			PlayerKeys = new AutoKillerScript.UserKeys(_ak);

			this.profile = profile;
			this.patrolareas = patrolareas;
			this.localization = localization;

			Action = BotAction.Initialize;
			ActionQueue = new BotQueue();

			ListOfBadLoot = new ArrayList ( );

			cooldowns = new Cooldowns();
			cooldowns.DefineCooldown( "Global", 500);
			cooldowns.DefineCooldown( "SkinFlag", 1000);
			cooldowns.DefineCooldown( "LootFlag", 1600);
			cooldowns.DefineCooldown( "WaitRez", 60 * 1000);

			IgnoreLootObjects = new ArrayList();
			IgnoreSkinnableObjects = new ArrayList();
			IgnoreTargetObjects = new ArrayList();

		}

		/// <summary>
		/// Clears an arraylist containing spawnID's of all invalid objects
		/// </summary>
		/// <param name="list">List to clear</param>
		public void ClearObjectIgnoreList( ArrayList list)
		{
			list.Clear();
		}


		BotAction CurrentAction;
		BotAction PreviousAction;
		/// <summary>
		/// The action switch
		/// </summary>
		public virtual void DoAction()
		{
			PreviousAction = CurrentAction;
			CurrentAction = Action;

//			AddMessage("DEBUG doing: " + Action.ToString());

			try
			{
				//check for death before anything else can happen
				if( _ak.IsPlayerDead &&							//if we are dead
					! bPaused &&									//and not paused
					Action != BotAction.DiedReleaseAndRun &&			//and not already doing corpse run
					Action != BotAction.FindGrave &&				//and not already finding body
					Action != BotAction.Pray )					//and not already waiting for Pray
				{													//then start the death sequence
					AddMessage("Oh dear, it appears we died.  Lets fix that.");
					Action = BotAction.DiedReleaseAndRun;
					CurrentAction = Action;
					ActionQueue.Clear();
				}

				//check for bPaused flag
				if(bPaused)
				{
					//save our current action if its not Pause, meaning its our first loop after we got paused
					if(Action != BotAction.Pause)
						ActionQueue.Insert(0, Action);

					//and do the pause
					Action = BotAction.Pause;
					CurrentAction = Action;
				}

				//wow.LogLine( "Current Action: {0}", Action);
				//AddMessage(string.Format("Current Action: {0}", Action));
				switch( Action)
				{
					case BotAction.Initialize:
						DoInitialize();
						break;

					case BotAction.SetClosestWaypoint:
						DoSetClosestWaypoint();
						break;

					case BotAction.SetNextWaypoint:
						DoSetNextWaypoint();
						break;

					case BotAction.MoveToWaypoint:
						DoMoveToWaypoint();
						break;

					case BotAction.FindTarget:
						DoFindTarget();
						break;

					case BotAction.CheckAgro:
						DoCheckAgro();
						break;

					case BotAction.GetInRange:
						DoGetInRange();
						break;

					case BotAction.RangedFight:
						DoRangedFight();
						break;

					case BotAction.MeleeFight:
						DoMeleeFight();
						break;

					case BotAction.CheckFlee:
						DoCheckFlee();
						break;

					case BotAction.Flee:
						DoFlee();
						break;

					case BotAction.Loot:
						DoLoot();
						break;

					case BotAction.Rest:
						DoRest();
						break;

					case BotAction.CheckBuffs:
						DoCheckBuffs();
						break;

					case BotAction.DiedReleaseAndRun:
						DoDiedReleaseAndRun();
						break;

					case BotAction.FindGrave:
						DoDeadFindGrave();
						break;

					case BotAction.Protect:
						DoProtect();
						break;

					case BotAction.Pray:
						DoPray();
						break;

					case BotAction.Pause:
						DoPause();
						break;
				}

			}
			catch( Exception e)
			{
				AddMessage( string.Format("Exception in DoAction. Current action: {0} Previous: {1}. " , CurrentAction, PreviousAction));
				AddMessage( e.Message );

				_ak.StopInit();
				
//				throw e;

			}
		}

		/// <summary>
		/// Initializes the bot, default it'll call the following actions:
		/// CheckAgro
		/// CheckBuffs [CheckAgro]
		/// SetClosestWaypoint
		/// MoveToWaypoint
		/// CheckBuffs [CheckAgro]
		/// FindTarget
		/// </summary>
		public virtual void DoInitialize()
		{
			// Load array of keywords for list we do NOT want to pick up
			ListOfBadLoot.Clear();
			string saveStr = profile.GetString("BadLoot");

			char delimiter = '|';
			string [ ] itemssplit = saveStr.Split ( delimiter );
			foreach (string keyWord in itemssplit) 
			{
				if ( keyWord != "" ) 
				{
					ListOfBadLoot.Add(keyWord);
				}
				
			}

			
			AddMessage("Our thread priority is " + Thread.CurrentThread.Priority.ToString());
			//When we start, there is no "current" waypoint, to avoid errors find the closest one
			PatrolArea patrolarea = GetPatrolArea();
			patrolarea.FindClosest(patrolarea.Direction);

			//Are we dead?
			if(_ak.IsPlayerDead )
			{
				Action = BotAction.DiedReleaseAndRun;
				return;
			}


			// Clear the queue
			ActionQueue.Clear();

			Action = BotAction.Protect;
			ActionQueue.Enqueue( BotAction.CheckAgro );
			ActionQueue.Enqueue( BotAction.CheckBuffs);
			CheckForTargets = true;
			ActionQueue.Enqueue( BotAction.SetClosestWaypoint);
			ActionQueue.Enqueue( BotAction.CheckBuffs);
			ActionQueue.Enqueue( BotAction.FindTarget);
		}

		/// <summary>
		/// Sets the waypoint iterator to the next waypoint in the waypoint direction setting's direction
		/// </summary>
		public virtual void DoSetNextWaypoint()
		{
			Action = BotAction.MoveToWaypoint;

			//this sets the internal waypoint iterator so the next call to 
			//GetNextWaypoint will get the closest waypoint in that direction
			//if it returns null something is wrong, there are no waypoints defined for this patrol area
			PatrolArea patrolarea = GetPatrolArea();
			patrolarea.CycleWaypoints();


			if(patrolarea.GetCurrentWaypoint() == null)
				throw new Exception("No waypoints defined for " + patrolarea.Name);

			AddMessage("Next Waypoint " + patrolarea.Direction.ToString() + " " + patrolarea.iCurrentWaypoint.ToString() );
		}

		/// <summary>
		/// Finds the closest waypoint and sets the waypoint iterator to that waypoint
		/// </summary>
		public virtual void DoSetClosestWaypoint()
		{
			Action = BotAction.MoveToWaypoint;

			//find the closest waypoint to the player in the direction we are heading,
			//this sets the internal waypoint iterator so the next call to 
			//GetNextWaypoint (if moving forward) or GetPreviousWaypoint (if moving backward)
			//will get the closest waypoint in that direction
			//if it returns null something is wrong, there are no waypoints defined for this patrol area
			PatrolArea patrolarea = GetPatrolArea();
			if(patrolarea.FindClosest(patrolarea.Direction) == null)
				throw new Exception("No waypoints defined for " + patrolarea.Name);

		}
		/// <summary>
		/// Moves to the waypoint that the iterator in the waypoint class points to
		/// </summary>
		public virtual void DoMoveToWaypoint()
		{
			// Do our final distance move?
			PatrolArea patrolarea = GetPatrolArea();

			AddMessage("Distance remaining: " + patrolarea.GetCurrentWaypoint().GetDistance().ToString());

			if( patrolarea.GetCurrentWaypoint().GetDistance() <= 200)
			{
				// Do our final move
				patrolarea.GetCurrentWaypoint().MoveTo(200,false,true);

				// One final agro check
				Action = BotAction.Protect;
				ActionQueue.Insert( 0, BotAction.CheckAgro );

			}
			else
			{
				// Walk to our target in 100 unit steps
				float distToWalk = patrolarea.GetCurrentWaypoint().GetDistance() - 100;

				// Test if we should jump, just to look realistic, TODO don't assume "a" is jump key
				if(distToWalk > 300 && rnd.Next(500) < 30)
					_ak.SendString("a");
					
				//Do the movement
				patrolarea.GetCurrentWaypoint().MoveTo(distToWalk,false,true);

				// Setup our bot actions
				Action = BotAction.Protect;

				// insert our MoveToWaypoint at the head of the queue after Protect
				ActionQueue.Insert( 0, BotAction.MoveToWaypoint);
				ActionQueue.Insert( 0, BotAction.CheckAgro );

				// Do we want to fing things to fight as we run? 
				if(CheckForTargets)
					ActionQueue.Insert( 0, BotAction.FindTarget );
			}

			
		}
		/// <summary>
		/// Tests if a target is suitable to fight
		/// </summary>
		public virtual bool TestTarget(int spawnID)
		{
			// a few guards show up as mobs, no idea why
			if(_ak.get_MobName(spawnID)  == "woodsman")
				IgnoreTargetObjects.Add(spawnID);

			// Is it dead?
			if( _ak.get_IsMobDead(spawnID))
				return false;

			// Is it in our ignore list?
			if( IgnoreTargetObjects.Contains( spawnID))
				return false;

			// Check if it has someone or something targeted, and if that someone or something is not us,
			// not our pet, and not our group members
			if( _ak.get_isMobInCombat(spawnID))
			{
				int mobTarget = _ak.get_MobTarget(spawnID);
				AKServer.DLL.DAoCServer.Group groupMembers = _ak.GroupMemberInfo;
				foreach ( AKServer.DLL.DAoCServer.GroupMember  grpMember in  groupMembers.GroupMemberTable.Values   )
				{
					if( grpMember.ID == mobTarget)
						return true;
				}

				//Is it fighting our pet?
				if( petID == mobTarget)
					return true;
			}

			// Is it within level range?
			if( _ak.get_MobLevel(spawnID)  < profile.GetInteger( "MinimumLevel") ||
					_ak.get_MobLevel(spawnID)  > profile.GetInteger( "MaximumLevel"))
				return false; // No :(
//			}

			return true;
		}

		/// <summary>
		/// Finds any group members with agro
		/// </summary>
		public abstract void DoProtect();

		/// <summary>
		/// Finds a target
		/// </summary>
		public virtual void DoFindTarget()
		{
			bNoLineOfSight = false;

			// This is the best time to check if we need a rest
			if( _ak.PlayerHealth  < profile.GetFloat( "RestBelowHealth") ||
				_ak.PlayerMana  < profile.GetFloat( "RestBelowMana"))
			{
				// Stop running
				_ak.StopRunning() ;

				// Switch to rest
				Action = BotAction.Rest;

				// Clear the queue
				ActionQueue.Clear();
				
				return;
			}

			// Calculate min and max levels if automatic is selected.
			// We do this each pull in case we just leveled
			if(profile.GetBool("AutomaticLevelSelect"))
			{
				int Low = 0;;
				int Hi = 0;
				AKServer.DLL.DAoCServer.ConColors.DAOCConRangeDefinition test = AKServer.DLL.DAoCServer.ConColors.CON_RANGES[_ak.PlayerLevel ]; 
				
				//Find lowest level to fight
				if(profile.GetBool("FightGreens"))
					Low = test.GrayMax + 1;
				else if(profile.GetBool("FightBlues"))
					Low = test.GreenMax + 1;
				else if(profile.GetBool("FightYellows"))
					Low = test.BlueMax + 1;
				else if(profile.GetBool("FightOranges"))
					Low = test.YellowMax + 1;
				
				//Find highest level to fight
				if(profile.GetBool("FightOranges"))
					Hi = test.OrangeMax;
				else if(profile.GetBool("FightYellows"))
					Hi = test.YellowMax;
				else if(profile.GetBool("FightBlues"))
					Hi = test.BlueMax;
				else if(profile.GetBool("FightGreens"))
					Hi = test.GreenMax;

				profile.SetValue("MinimumLevel",Low);
				profile.SetValue("MaximumLevel",Hi);
			}

			// Need to set AutoKillerScripts list of names to attack, if any.
			bool bUsingNames = false;
			PatrolArea patrolarea = GetPatrolArea();
			if(patrolarea.NumTargets > 0)
			{
				bUsingNames = true;
				_ak.MobList.Clear();
				// Remember to lock on SyncRoot before enumeration
				lock(patrolarea.Targets.SyncRoot)
				{
					foreach (string target in patrolarea.Targets)
					{
						//we should really find a way to determine if a mob is agro
						//or add some feature to the GUI for it, but for now set all to agro
						_ak.MobList.Add(target, true);
					}
				}
			}

			// Retrieve a list of objects we could attack
			int myX = _ak.gPlayerXCoord;
			int myY = _ak.gPlayerYCoord;
			int wX = (int)patrolarea.GetCurrentWaypoint().X;
			int wY = (int)patrolarea.GetCurrentWaypoint().Y;
			int spawnID = _ak.FindClosestMob((short)profile.GetInteger( "MinimumLevel"),(short)profile.GetInteger( "MaximumLevel"),(int)profile.GetFloat( "SearchDistance"),bUsingNames,(int)patrolarea.GetCurrentWaypoint().X,(int)patrolarea.GetCurrentWaypoint().Y);
			while(spawnID != -1 )
			{
				// Does this object match our fight selections?
				if( ! TestTarget(spawnID) || spawnID == petID )
				{
					spawnID = _ak.FindNextClosestMob((short)profile.GetInteger( "MinimumLevel"),(short)profile.GetInteger( "MaximumLevel"),(int)profile.GetFloat( "SearchDistance"),bUsingNames,(int)patrolarea.GetCurrentWaypoint().X,(int)patrolarea.GetCurrentWaypoint().Y);				
					continue;
				}

				//Must have found a target we like
				// Target it
				_ak.StopRunning();
				_ak.SetTarget(spawnID);

				AddMessage(string.Format("Found target {0} at distance {1:N}",_ak.get_MobName(spawnID),DistanceToMob(spawnID)));

				// Setup the appropriate action
				Action = BotAction.GetInRange;
				ActionQueue.Clear();
				ActionQueue.Enqueue( BotAction.RangedFight);
				return;
			}
			//No mob found if we got to here

			// are we just checking for targets while we run to the next waypoint?
			if( ActionQueue.Count > 0 && ActionQueue.Peek(0) == BotAction.MoveToWaypoint)
			{
				CheckForTargets = true;
				//yes, we are running to the next waypoint and didnt find a target, so just let the queued actions do their stuff
				Action = ActionQueue.Dequeue();
				return;
			}

			AddMessage("Nothing left to fight at this waypoint!");

			//we must be done with all targets at this waypoint, lets move on
			Action = BotAction.Protect;
			
			ActionQueue.Clear();
			ActionQueue.Enqueue( BotAction.CheckAgro );
			ActionQueue.Enqueue( BotAction.SetNextWaypoint );
			ActionQueue.Enqueue( BotAction.FindTarget  );

		}

		/// <summary>
		/// Handles agro, it will continue the ActionQueue if there's no agro,
		/// otherwise it'll call RangedAttack or MeleeAttack (depending on range of agro)
		/// </summary>
		public virtual void DoCheckAgro()
		{
			// Check if we have agro
			ArrayList pets = new ArrayList();
			pets.Add(petID);
			
			int spawnID = _ak.FindClosestMobWithPlayerAsTarget(2000, pets);	//is player targeted by a mob?
			if(spawnID < 1 && petID > 0)
				spawnID = _ak.FindClosestMobWithPlayerAsTarget(2000, petID, pets);	///or is players pet targeted by a mob?
			
			if(spawnID > 0 && ! _ak.get_IsMobDead(spawnID))	
			{
				AddMessage( "We agro'd something! ID " + spawnID.ToString() + " hlth " + _ak.get_MobHealth(spawnID).ToString());

				// Stand up if sitting
				if(_ak.isPlayerSitting)
				{
					PlayerKeys.SitStand(KeyDirection.KeyUpDown);
					Thread.Sleep(1000);
				}

				// Stop running if running
				_ak.StopRunning();

				// Forget whatever we where suppose to do
				ActionQueue.Clear();

				// Get the first monster
				float dist = DistanceToMob(spawnID);

				// Target out attacker
				_ak.SetTarget(spawnID);

				// Check where it is, and what fighting action we should take
				if( dist > profile.GetFloat( "MinimumRangedAttackDistance"))
				{
					// We're farther than minimum ranged distance, are we within max ranged distance?
					if( dist > profile.GetFloat( "MaximumRangedAttackDistance"))
					{	// No, check if we should fight, and if so get in range, and fight
						Action = BotAction.CheckFlee;
						ActionQueue.Enqueue( BotAction.GetInRange);
						ActionQueue.Enqueue( BotAction.RangedFight);
					}
					else// Yes, kill forest, kill! ;)
					{
						Action = BotAction.CheckFlee;
						ActionQueue.Enqueue( BotAction.RangedFight);
					}
				}
				else// We're too close for ranged, do melee
				{
					//stick to whatever it is
					if( _ak.PlayerMana == 0)  
						PlayerKeys.Stick(KeyDirection.KeyUpDown);

					Action = BotAction.CheckFlee;
					ActionQueue.Enqueue( BotAction.MeleeFight);
				}
			}
			else
			{	
				// Should never happen, but lets just check for bad bot code ;)
				if( ActionQueue.Count == 0)
					throw new Exception("No action in ActionQueue");

				Action = ActionQueue.Dequeue();
			}
		}

		/// <summary>
		/// Get in range of our current target
		/// </summary>
		public virtual void DoGetInRange()
		{
			// Do we have a target and is it not fighting someone else?
			if( _ak.TargetIndex < 1 || (_ak.TargetObject != null && _ak.TargetObject.TargetID != 0 && _ak.TargetObject.TargetID != _ak.PlayerID ))
			{	// No? Stop moving
				_ak.StopRunning();

				// Check agro
				Action = BotAction.CheckAgro;

				// If we were in Melee code, maybe it died?
				if(ActionQueue.Count > 0 && ActionQueue.Peek(0) == BotAction.MeleeFight)
				{
					// Let melee code do whatever it does when something dies
					return;
				}
				else
				{
					// Find a new target
					ActionQueue.Clear();
					ActionQueue.Enqueue( BotAction.FindTarget);
				}
				
				return;
			}

			// Get the next action from the queue
			BotAction NextAction = ActionQueue.Dequeue();

			// By default assume a ranged attack distance (move closer then the max, so mobs that run away are caught easier)
			float AttackDistance = profile.GetFloat("MaximumRangedAttackDistance") * 0.95f;

			// Calculate the distance to the target
			float TargetDistance = DistanceToMob(_ak.TargetIndex);

			// Check if we're past the minimum distance, go to melee distance else
			if( TargetDistance < profile.GetFloat("MinimumRangedAttackDistance"))
				NextAction = BotAction.MeleeFight;

			// Should we go to melee attack distance?
			if( NextAction == BotAction.MeleeFight || (ActionQueue.Count > 0 && ActionQueue.Peek(0) == BotAction.MeleeFight ))
				AttackDistance = 200;  //200 should be close enough to /stick to the target

			// TODO: Add (extra) stuck checking here

			float distToWalk = TargetDistance - AttackDistance;

			// Do our final distance move?
			if( _ak.TargetIndex > 0 && distToWalk <= 200)
			{
				// Do our final move
				MoveTo( _ak.TargetIndex, AttackDistance, true);

				// Make sure we are either sticking or facing the target
				if (_ak.TargetIndex > 0)
				{
					//a hack solution, casters dont need to stick
					//instead we should use PlayerClass once it is fixed
					if(AttackDistance <= 200 && _ak.PlayerMana == 0)  
						PlayerKeys.Stick(KeyDirection.KeyUpDown ); // PlayerKeys.Stick(out KeyDirection.KeyUpDown );
					else
						PlayerKeys.Face(KeyDirection.KeyUpDown );
				}

				// One final agro check, might be the same monster though
				Action = BotAction.CheckAgro;

				// Make sure we do the appropriate action
				ActionQueue.Clear();
				ActionQueue.Enqueue( NextAction);
			}
			else
			{
				// Walk to our target in small steps
				if ( _ak.TargetIndex > 0)
					MoveTo( _ak.TargetIndex, TargetDistance - (distToWalk / 4), false);

				// Recheck if our target is still the closest once, only if not incombat
				if( GetAttackerCount() == 0)
				{
					// Setup our bot actions
					Action = BotAction.CheckAgro;
					ActionQueue.Clear();
					ActionQueue.Enqueue( BotAction.Protect );
					ActionQueue.Enqueue( BotAction.FindTarget);
				}
				else
				{	// In combat, just get in range
					Action = BotAction.GetInRange;
					ActionQueue.Clear();
					ActionQueue.Enqueue( NextAction);
				}
			}
		}

		/// <summary>
		/// The Ranged Fight sequence
		/// </summary>
		public abstract void DoRangedFight();

		/// <summary>
		/// The Melee fight sequence
		/// </summary>
		public abstract void DoMeleeFight();

		/// <summary>
		/// Check if we have to flee, might need a change for some classes
		/// </summary>
		public virtual void DoCheckFlee()
		{
			int numAttackers = GetAttackerCount();
			
			// Less then specified hp plus 10%, and more then 1 attacker?
			if( _ak.PlayerHealth < profile.GetInteger("FleeBelowHealth") + 10 && 
				numAttackers > 1)
			{
				// Yes? RUN FOREST RUN !
				// Can we do an instant heal? 
				if (profile.GetBool("FleeUseHeal"))
				{
					AddMessage("Instant Heal as we flee"); 
					Thread.Sleep(500);
				}

				Action = BotAction.Flee;

				ActionQueue.Clear();
				ActionQueue.Enqueue( BotAction.Rest);
				return;
			}

			//Just one attacker, and below health at which we should run away?
			if( _ak.PlayerHealth  < profile.GetInteger("FleeBelowHealth"))
			{
				// Our attacker has more HP then us?
				if(_ak.TargetIndex > 0 && _ak.PlayerHealth < _ak.TargetObject.Health )
				{
					// Yes? RUN FOREST RUN !
					// Can we do an instant heal? 
					if (profile.GetBool("FleeUseHeal"))
					{
						AddMessage("Instant Heal as we flee"); 
						Thread.Sleep(500);
					}

					Action = BotAction.Flee;

					ActionQueue.Clear();
					ActionQueue.Enqueue( BotAction.Rest);
					return;
				}
			}

			// Check if we have a next action
			if( ActionQueue.Count == 0)
				throw new Exception("No action in ActionQueue");

			// Next Action
			Action = ActionQueue.Dequeue();
		}

		/// <summary>
		/// Flee
		/// </summary>
		public virtual void DoFlee()
		{
			int numAttackers = GetAttackerCount();
			PatrolArea patrolarea = GetPatrolArea();

			// Did all attackers give up ?
			if(numAttackers == 0)
			{
				// Yes, stop moving
				_ak.StopRunning();

				// Start the next action
				Action = ActionQueue.Dequeue();

				//reset the flag for next time we flee, and reset our direction
				if(ChangedDirection)
				{
					ChangedDirection = false;
					
					if(patrolarea.Direction == MovementDirection.Forward)
						patrolarea.Direction = MovementDirection.Back;
					else
						patrolarea.Direction = MovementDirection.Forward;
				}

				return;
			}

			//Something still attacking us, keep running if we can
			_ak.StartRunning();

			// if we are just starting to run away, change direction so that
			// we will run back through the area we just fought in
			if( ! ChangedDirection)
			{
				if(patrolarea.Direction == MovementDirection.Forward)
					patrolarea.Direction = MovementDirection.Back;
				else
					patrolarea.Direction = MovementDirection.Forward;

				//dont change direction again until next time we flee
				ChangedDirection = true;
			}

			//run to next waypoint
			Waypoint wpt = patrolarea.CycleWaypoints();
			wpt.MoveTo(50,false,false);
			
			//and then come here again to check if we need to keep running
			Action = BotAction.Flee;
		}

		/// <summary>
		/// This function waits for loot (up to 4s)
		/// </summary>
		/// <returns>Was there loot?</returns>
		public bool WaitForLoot()
		{
			Thread.Sleep(100);
			
			// Wait for a maximum of 1s for the loot window to popup
			for( int sanity = 0; sanity < 10; sanity ++)
			{
				// check for loot near player
				if( _ak.FindClosestObject(0,0,500,_ak.gPlayerXCoord,_ak.gPlayerYCoord) != -1)
					return true;

				//is the loot near our pet maybe?
				if(petID > 0)
					if( _ak.FindClosestObject(0,0,500,_ak.get_MobXCoord(petID),_ak.get_MobYCoord(petID)) != -1)
						return true;

				Thread.Sleep(100);
			}

			return false;
		}


		/// <summary>
		/// Loot all close by objects
		/// </summary>
		public virtual void DoLoot()
		{
			// Do we even want to loot, and is there loot?
			//TODO: Looting settings
			if( !profile.GetBool( "DoLooting") || ! WaitForLoot())
			{
				// Check for agro
				Action = BotAction.Protect ;

				ActionQueue.Clear();
				ActionQueue.Enqueue( BotAction.CheckAgro );
				ActionQueue.Enqueue( BotAction.Rest);

				return;
			}
			
			AddMessage("Looting");

			// Clear the list of invalid objects
			ClearObjectIgnoreList( IgnoreLootObjects);

			int lootID =  _ak.FindClosestObject(0,0,500,_ak.gPlayerXCoord,_ak.gPlayerYCoord);
			if(lootID == -1 && petID > 0)
				lootID =  _ak.FindClosestObject(0,0,500,_ak.get_MobXCoord(petID),_ak.get_MobYCoord(petID));

			while(lootID != -1)
			{
				//Are we being attacked?
				if(GetAttackerCount() > 0)
				{	//Under attack!  Fight them
					// Check for agro
					Action = BotAction.Protect ;

					ActionQueue.Clear();
					ActionQueue.Enqueue( BotAction.CheckAgro );
					ActionQueue.Enqueue( BotAction.Rest);
				}

				// Money only?
				if(profile.GetBool("MoneyOnly"))
				{
					if( _ak.get_MobName(lootID).IndexOf("bag of coin") == -1 &&
						_ak.get_MobName(lootID).IndexOf("large chest") == -1)
					{
						lootID =  _ak.FindNextClosestObject(0,0,500,_ak.gPlayerXCoord,_ak.gPlayerYCoord);
						continue;
					}
				}
				
				// Skip loot in exclude list
				foreach (string badWord in ListOfBadLoot)
				{
					if( _ak.get_MobName(lootID).IndexOf(badWord) > -1)
					{
						lootID =  _ak.FindNextClosestObject(0,0,500,_ak.gPlayerXCoord,_ak.gPlayerYCoord);
						continue;
					}
				}

				// Check if its a valid loot object, maybe it is someone elses
				if( IgnoreLootObjects.IndexOf( lootID) != -1)
				{
					lootID =  _ak.FindNextClosestObject(0,0,500,_ak.gPlayerXCoord,_ak.gPlayerYCoord);
					continue;
				}

				// Don't run around trying to pick up graves, they are very heavy and you could hurt yourself
				if(_ak.get_MobName(lootID).EndsWith("Grave"))
				{
					IgnoreLootObjects.Add( lootID);
					lootID =  _ak.FindNextClosestObject(0,0,500,_ak.gPlayerXCoord,_ak.gPlayerYCoord);
					continue;
				}

				// Are we close enough to pick up the loot?
				if( DistanceToMob(lootID) > 100)
				{
					// Move to the loot object
					MoveTo(lootID,100,true);

					// Recheck agro
					Action = BotAction.Protect ;

					ActionQueue.Clear();
					ActionQueue.Enqueue( BotAction.CheckAgro );
					ActionQueue.Enqueue( BotAction.Loot);
					return;
				}

				// try and grab the loot
				_ak.SetTarget(lootID);
				Thread.Sleep(100);
				PlayerKeys.GetItem(KeyDirection.KeyUpDown );
				Thread.Sleep(100);
				
				// Flag this object as 'looted'
				IgnoreLootObjects.Add( lootID);

				// Recheck agro
				// Check for agro
				Action = BotAction.Protect ;

				ActionQueue.Clear();
				ActionQueue.Enqueue( BotAction.CheckAgro );
				ActionQueue.Enqueue( BotAction.Loot );

				lootID =  _ak.FindNextClosestObject(0,0,500,_ak.gPlayerXCoord,_ak.gPlayerYCoord);

			}
		
			// Done looting, Check for agro
			Action = BotAction.Protect ;

			ActionQueue.Clear();
			ActionQueue.Enqueue( BotAction.CheckAgro );
			ActionQueue.Enqueue( BotAction.Rest);
		}


		protected bool Resting = false;
		/// <summary>
		/// Rest, eat and drink when needed
		/// </summary>
		public virtual void DoRest()
		{
			// Stop running just in case
			if(!_ak.isPlayerSitting )
				_ak.StopRunning();

			// Setup default action
			Action = BotAction.Protect ;

			ActionQueue.Clear();
			ActionQueue.Enqueue( BotAction.CheckAgro );
			ActionQueue.Enqueue( BotAction.Rest);

			// If we're casting something, like a heal, wait till we're done
			if( _ak.isPlayerCasting )
				return;

			// Do we really need to rest?
			if( _ak.PlayerHealth < profile.GetFloat( "RestBelowHealth") ||
				_ak.PlayerMana < profile.GetFloat( "RestBelowMana") ||
				_ak.PlayerStamina < profile.GetFloat( "RestBelowEndurance"))
			{
				//Is this our first time finding we need to rest? If so add message
				if(!Resting)
					AddMessage("Resting");

				// Yes, set the rest flag
				// This will stay set until users full health conditions are met
				Resting = true;
				if(! _ak.isPlayerSitting)
				{
					PlayerKeys.SitStand(KeyDirection.KeyUpDown);
					Thread.Sleep(1000); //takes time to actually sit!
				}
			}

			// Is the RestUntilFullyRecovered bool set to true?
			if(profile.GetBool("RestUntilFullyRecovered"))
			{	//yes, was our health low enough that we are really needing to rest?
				if(Resting)
				{
					// Are we back to full health?
					if(_ak.PlayerHealth > 98 &&
						(_ak.PlayerMana == 0 || _ak.PlayerMana > 98) &&   //zero for non-mana users
					    _ak.PlayerStamina > 98)
					{
						// We are fully recovered, lets move on
						Action = BotAction.Protect ;
						ActionQueue.Clear();
						ActionQueue.Enqueue( BotAction.CheckAgro );

						//check if we are too far from current waypoint
						PatrolArea patrolarea = GetPatrolArea();

						if( patrolarea.GetCurrentWaypoint().GetDistance() > profile.GetFloat( "WanderDistance"))
						{
							//wow.LogLine("Too far from current waypoint, returning");
							AddMessage("Too far from current waypoint, returning " + patrolarea.GetCurrentWaypoint().GetDistance().ToString());
							CheckForTargets = false;  //go back to waypoint without running off after new targets
							ActionQueue.Enqueue( BotAction.MoveToWaypoint);
						}

						// Check our buffs
						ActionQueue.Enqueue( BotAction.CheckBuffs);
						ActionQueue.Enqueue( BotAction.FindTarget);

						//Done resting
						Resting = false;
						if(_ak.isPlayerSitting )
							PlayerKeys.SitStand(KeyDirection.KeyUpDown );
						
						return;
					}
				}
				else
					//Our health didnt get low enough that we need to rest? Then we can move on
				{
					// Check our buffs
					ActionQueue.Clear();

					//check if we are too far from current waypoint
					PatrolArea patrolarea = GetPatrolArea();

					if( patrolarea.GetCurrentWaypoint().GetDistance() > profile.GetFloat( "WanderDistance"))
					{
						//wow.LogLine("Too far from current waypoint, returning");
						AddMessage("Too far from current waypoint, returning " + patrolarea.GetCurrentWaypoint().GetDistance().ToString());
						CheckForTargets = false;  //go back to waypoint without running off after new targets
						ActionQueue.Enqueue( BotAction.MoveToWaypoint);
					}

					ActionQueue.Enqueue( BotAction.CheckBuffs);
					ActionQueue.Enqueue( BotAction.FindTarget);

					if(_ak.isPlayerSitting )
						PlayerKeys.SitStand(KeyDirection.KeyUpDown );

					return;
				}
			}
				//else, "rest until fully recovered" is not set.
			else if( 
					_ak.PlayerHealth > profile.GetFloat( "RestBelowHealth") &&
					_ak.PlayerMana > profile.GetFloat( "RestBelowMana") &&
					_ak.PlayerStamina > profile.GetFloat( "RestBelowEndurance"))
			{	
				// Yes, check our buffs
				Action = BotAction.Protect ;
				ActionQueue.Clear();
				ActionQueue.Enqueue( BotAction.CheckAgro );

				//check if we are too far from current waypoint
				PatrolArea patrolarea = GetPatrolArea();

				if( patrolarea.GetCurrentWaypoint().GetDistance() > profile.GetFloat( "WanderDistance"))
				{
					//wow.LogLine("Too far from current waypoint, returning");
					AddMessage("Too far from current waypoint, returning " + patrolarea.GetCurrentWaypoint().GetDistance().ToString());
					CheckForTargets = false;  //go back to waypoint without running off after new targets
					ActionQueue.Enqueue( BotAction.MoveToWaypoint);
				}
				// Check our buffs
				ActionQueue.Enqueue( BotAction.CheckBuffs);
				ActionQueue.Enqueue( BotAction.FindTarget);

				//Done resting
				Resting = false;
				if(_ak.isPlayerSitting )
					PlayerKeys.SitStand(KeyDirection.KeyUpDown );

				return;
			}

		}


		/// <summary>
		/// Check buffs, more logic can be added from character specific classes
		/// </summary>
		public abstract void DoCheckBuffs();

		/// <summary>
		/// Pray
		/// </summary>
		public virtual void DoPray()
		{
			//are we still within our time waiting for nearby monsters to go away?
			if( ! cooldowns.IsReady("WaitRez"))
			{
				if(-1 != _ak.FindClosestMob((short)profile.GetInteger( "MinimumLevel"),100,500,false,_ak.gPlayerXCoord,_ak.gPlayerYCoord))
					return;  //keep waiting
			}
			
			//time to rez
			AddMessage("Praying");
			
			//just in case
			_ak.StopRunning();

			string graveName = _ak.PlayerName + "'s grave";
			
			_ak.SetTarget(graveName,true);
			Thread.Sleep(1000);
			_ak.SendString("/pray~");
			Thread.Sleep(5000);

			//make sure we are not being attacked
			Action = BotAction.Protect ;

			ActionQueue.Clear();
			ActionQueue.Enqueue( BotAction.CheckAgro );

			//go back to waypoint without running off after new targets
			CheckForTargets = false;  
			ActionQueue.Enqueue( BotAction.MoveToWaypoint);
			//then buff
			ActionQueue.Enqueue( BotAction.CheckBuffs);
			//then fight
			ActionQueue.Enqueue( BotAction.FindTarget);
			
		}
		/// <summary>
		/// Release, run back to patrol area
		/// </summary>
		public virtual void DoDeadFindGrave()
		{
			PatrolArea patrolarea = GetPatrolArea();

			int iterator = 0;

			AddMessage("Doing Patrol points looking for grave in " + patrolarea.Name);
			
			//we finished running waypoints, find patrol point closest to last waypoint
			Waypoint nextWaypoint = patrolarea.FindClosest( MovementDirection.Forward );

			Waypoint graveWP = null;
			int graveID = 0;

			//run all the points while we search for our grave
			while(nextWaypoint != null && iterator <= patrolarea.NumWaypoints)
			{
				if (bPaused)
				{
					_ak.StopRunning();
					return;
				}

				nextWaypoint.MoveTo( 250, false, false);
					
				graveID = FindGrave();


				// are we close enough to have seen our grave?
				if(graveID > 0)
				{
					//find the waypoint closest to the grave
					//we use movementdirection.back because we want the nearest waypoint BEFORE the corpse
					graveWP = patrolarea.FindClosest(_ak.get_MobXCoord(graveID), _ak.get_MobYCoord(graveID), _ak.get_MobZCoord(graveID), MovementDirection.Back);
				}

				//did we just run to the graveWP?
				if(nextWaypoint == graveWP)
					break;

				//either didnt get close enough to see grave yet, or there are closer waypoints, so keep running
				iterator++;
				nextWaypoint = patrolarea.CycleWaypoints();
			}

			//are we at the end of the patrol area and still not at waypoint closest to grave?
			if (iterator >= patrolarea.NumWaypoints && nextWaypoint != graveWP)
			{
				AddMessage("Completed full patrol path and did not find grave waypoint");
				
				_ak.StopRunning();
				//pause all actions
				bPaused = true;
				return;
			}

			//did we stop at the right place, or run to the end of the waypoint list?
			if(nextWaypoint == null && graveID < 1)
			{
				AddMessage("Did not find waypoint closest to corpse, sorry!");
				
				_ak.StopRunning();
				//pause all actions
				bPaused = true;
				return;
			}

			//go to corpse
			MoveTo( graveID, 1800, true);

			//just in case, some people claim it keeps running when it finds corpse
			_ak.StopRunning();

			//set timer used during waiting for Pray
			cooldowns.SetTime("WaitRez");

			//wait up to a minute for any nearby monsters to go away
			AddMessage("Waiting for up to 1 minute for nearby monsters to wander away");

			//ressurect
			Action = BotAction.Pray;

		}
		/// <summary>
		/// Release, run back through waypoints to patrol area
		/// </summary>
		public virtual void DoDiedReleaseAndRun()
		{
			if( _ak.IsPlayerDead )
			{

				//rez to bind point
				_ak.StopRunning();
				_ak.SendString("/release~");

				//wait to be at bind point
				int sanity = 0;
				while(!bPaused && sanity++ < 30 && _ak.IsPlayerDead)
					Thread.Sleep(2000);

				//if there is a selected travel path, use it for corpserun
				if(profile.GetString("CorpseRun") != "")
				{
					AddMessage("Doing waypoints " + profile.GetString("CorpseRun"));

					PatrolArea corpserun = GetCorpseRun();

					//we assume corpse run is set up from bind point forward
					corpserun.Direction = MovementDirection.Forward;
					
					//find closest point to where we are
					Waypoint nextWaypoint = corpserun.FindClosest(MovementDirection.Forward);
					
					//lets see if for some reson the bot was started closer to the patrol area
					//find closest patrol area point
					PatrolArea patrolarea = GetPatrolArea();

					Waypoint patrolareaWaypoint = patrolarea.FindClosest(MovementDirection.Forward);
					//compare corpserun to patrolarea
					if(patrolareaWaypoint.GetDistance() < nextWaypoint.GetDistance())
					{
						//we are closer to the patrol area!
						Action = BotAction.FindGrave;
						return;
					}
					
					//test if this appears to be a valid corpse run
					if(nextWaypoint.GetDistance() > 100)
					{
						AddMessage("The first waypoint in the corpse run is too far away " + corpserun.Name);

						AddMessage("Corpse Run aborted");
						_ak.StopRunning();
						//pause all actions
						bPaused = true;
						return;
					}


					//run all the points to the end
					while( nextWaypoint != null )
					{
						if (bPaused)
						{
							_ak.StopRunning();
							return;
						}

						nextWaypoint.MoveTo( 50, false, false);
						nextWaypoint = corpserun.GetForwardWaypoint();
					}

					Action = BotAction.FindGrave;

				}
				else
				{
					bPaused = true;
					AddMessage("No Waypoints path set!  Use the Waypoints button to select a path");
				}
			}
		}
		/// <summary>
		/// Pause
		/// </summary>
		public virtual void DoPause()
		{
			//if the gui clears the pause flag, check if we can go back to what we were doing
			if( ! bPaused)
			{
				//did the player move while paused so that now the current waypoint is too far away?
				PatrolArea patrolarea = GetPatrolArea();

				if(patrolarea.GetCurrentWaypoint() == null || patrolarea.GetCurrentWaypoint().GetDistance() > 2000)
				{
					//yes, try to find closest patrolpoint forward
					if(patrolarea.FindClosest(MovementDirection.Forward).GetDistance() > 2000)
					{
						//still too far away? yes, try to find closest patrolpoint back
						if(patrolarea.FindClosest(MovementDirection.Back ).GetDistance() > 2000)
						{
							//still too far away? yes, they are all too far away, so give up searching and ask the user
							if(MessageBox.Show("You are far away from this Patrol Area, are you sure you want to resume fighting here?" , "Distance Warning",
								MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No )
							{
								bPaused = true;
								return;
							}
						}
					}
				}

				// we are close enough to patrol area, or user insists its okay, go back to what we were doing
				Action = ActionQueue.Dequeue();
			}
			else
			{	//we just paused
				//in case we are pausing during a moveto
				_ak.StopRunning();
				Thread.Sleep(100);
			}
		}


		/// <summary>
		/// Call from derived class to add any targets we find we cannot fight
		/// </summary>
		/// <param name="badTarget">The Mob to ignore</param>
		public void IgnoreThis(int badTarget)
		{
			IgnoreTargetObjects.Add(badTarget);

			//lets only keep 5 most recent kills, new spawns might have the same number
			while(IgnoreTargetObjects.Count > 5)
				IgnoreTargetObjects.RemoveAt(0);
		}
		
		//the compiler insists I cant call UpdateStatus from a derived class, so we do it from here
		public void AddMessage(string msg)
		{
			UpdateStatus(msg);
		}
		
		internal PatrolArea GetPatrolArea()
		{
			PatrolArea patrolarea = null;
			try
			{
				//get a patrolarea object based on the users selected patrolarea
				if(profile.GetString( "PatrolArea") != "")
					patrolarea = patrolareas.GetPatrolArea( profile.GetString( "PatrolArea"));

				//if the patrol area has changed and we don't have a waypoint, then find closest one.
				if (patrolarea.GetCurrentWaypoint() == null)
					patrolarea.FindClosest(patrolarea.Direction);
			}
			catch ( Exception E)
			{
				AddMessage(E.Message);
				bPaused = true;
			}
			return patrolarea;
		}

		internal PatrolArea GetCorpseRun()
		{
			PatrolArea corpserun = null;
			try
			{
				//get a corpserun object based on the users selected patrolarea
				if(profile.GetString( "CorpseRun") != "")
					corpserun = patrolareas.GetPatrolArea( profile.GetString( "CorpseRun"));

				//if the corpserun has changed and we don't have a waypoint, then find closest one.
				if (corpserun.GetCurrentWaypoint() == null)
					corpserun.FindClosest(corpserun.Direction);
			}
			catch ( Exception E)
			{
				AddMessage(E.Message);
				bPaused = true;
			}
			return corpserun;
		}
		
		internal float DistanceToMob(int spawnID)
		{
			return (float)_ak.ZDistance(_ak.gPlayerXCoord,_ak.gPlayerYCoord,_ak.gPlayerZCoord,_ak.get_MobXCoord(spawnID),_ak.get_MobYCoord(spawnID),_ak.get_MobZCoord(spawnID));
		}
		internal float DistanceToPet(int spawnID)
		{
			if(! _ak.get_DoesObjectExist(petID))
				return 0;

			return (float)_ak.ZDistance(_ak.get_MobXCoord(petID),_ak.get_MobYCoord(petID),_ak.get_MobZCoord(petID),_ak.get_MobXCoord(spawnID),_ak.get_MobYCoord(spawnID),_ak.get_MobZCoord(spawnID));
		}
		/// <summary>
		///	Moves to a mob until within distance
		/// </summary>
		/// <param name="distance">stop/return if within distance</param>
		/// <param name="spawnID">the mob we are moving to</param>
		/// <param name="stoprunning">Stop once we reach distance from mob</param>
		internal void MoveTo(int spawnID, float distance, bool stoprunning)
		{


			while(DistanceToMob(spawnID) > distance)
			{
				//once in awhile we dont start running for some reason, lag maybe.  So we start running every loop
				_ak.StartRunning();

				//on slow machines (mine!) turnToHeading this often is bad, so lets only turn if we 
				// are really facing the wrong way - more than 15 degrees off of correct heading
				if(Math.Abs(_ak.FindHeading(_ak.gPlayerXCoord,_ak.gPlayerYCoord,_ak.get_MobXCoord(spawnID),_ak.get_MobYCoord(spawnID)) - _ak.PlayerDir) > 15)
					_ak.TurnToHeading(_ak.FindHeading(_ak.gPlayerXCoord,_ak.gPlayerYCoord,_ak.get_MobXCoord(spawnID),_ak.get_MobYCoord(spawnID)));

				//Give the game process a chance at the cpu so it can respond
				System.Threading.Thread.Sleep(50);
//				AddMessage("Going to " + _ak.get_MobName(spawnID) + ", dist: " + DistanceToMob(spawnID));
			}
			
			if(stoprunning)
				_ak.StopRunning();
		}

		internal int FindGrave()
		{
			string graveName = _ak.PlayerName + "'s grave";

			return _ak.SetTarget(graveName,false);
		}
		internal int GetAttackerCount()
		{
			//Find how many things are fighting us or our pet
			ArrayList pets = new ArrayList();
			if(petID > 0)
				pets.Add(petID);

			int numAttackers = 0;
			//first count things fighting player
			int spawnID = _ak.FindClosestMobWithPlayerAsTarget(3000, pets);	//is player targeted by a mob?
			while(spawnID != -1)
			{
				numAttackers++;
				spawnID = _ak.FindNextClosestMobWithPlayerAsTarget(3000, pets);	///or is players pet targeted by a mob?
			}

			//Also add up number attacking our pet, if we have a pet
			if(petID > 0)
			{
				spawnID = _ak.FindClosestMobWithPlayerAsTarget(3000, petID, pets);	//is player targeted by a mob?
				while(spawnID != -1)
				{
					numAttackers++;
					spawnID = _ak.FindNextClosestMobWithPlayerAsTarget(3000, petID, pets);	///or is players pet targeted by a mob?
				}
			}

			return numAttackers;
		}
		
		public virtual void ParseChat( AutoKillerScript.clsAutoKillerScript.AutokillerRegExEventParams e)
		{
			string logline = e.Logline;

			if(logline.IndexOf("You can't see your target") > -1)
			{
				AddMessage("Can't see target!");
				bNoLineOfSight = true;
			}

			if(logline.IndexOf("can't cast while sitting") > -1 && _ak.isPlayerSitting )
				PlayerKeys.SitStand(KeyDirection.KeyUpDown);

		
		}		

		public virtual void UseQbar( string qbar, string key)
		{
			Byte shift = (Byte)Keys.ShiftKey;
			_ak.SendKeys(shift,true,false);
			_ak.SendString(qbar);
			_ak.SendKeys(shift,false,true);
			Thread.Sleep(100);
		
			_ak.SendString(key);
			Thread.Sleep(100);

		}		
	}
}
