//------------------------------------------------------------------------------
//-------------------------------------------------------------------------------
using System;
using System.Threading;
using System.Collections;
using AutoKillerScript;
using AKServer.DLL;
using System.Windows.Forms;
using System.Text.RegularExpressions;

namespace DAoC_Bot
{
	/// <summary>
	/// Summary description for DAoC_Necro.
	/// </summary>
	public class DAoC_Necro : DAoC_BotBase
	{
		public enum eRangedFights
		{
			First,
			Debuff,
			Power,
			Health
		}
		internal eRangedFights nextSpell;
		int nCountFight = 0;
		int NumKilled = 0;
		int currentTarget = -1;

		public DAoC_Necro( AutoKillerScript.clsAutoKillerScript ak, Profile profile, PatrolAreas patrolareas) 
			: base( ak, profile, patrolareas )
		{
			_ak = ak;

			cooldowns.DefineCooldown( "Heal", 15 * 1000);
			cooldowns.DefineCooldown( "Ranged", 1 * 1000);
			cooldowns.DefineCooldown( "MeleePT", 1 * 1000);
			cooldowns.DefineCooldown( "Buff", 3 * 1000);
			cooldowns.DefineCooldown( "FacilitatePain", 30 * 1000);
			cooldowns.DefineCooldown( "BuffStrength", 20 * 60 * 1000);
			cooldowns.DefineCooldown( "BuffDex", 20 * 60 * 1000);
			cooldowns.DefineCooldown( "BuffAbsorb", 10 * 60 * 1000);
		}

		#region DoInitialize
		public override  void DoInitialize()
		{
			AddMessage("Our thread priority is " + Thread.CurrentThread.Priority.ToString());
			//argh, we need to make sure patrol area has a valid GetCurrentTarget, and if
			//we arent going to use SetClosest then we need to do this the long way
			//sets patrolarea internal iterator so it wont throw an exception
			//the next time we try and move
			PatrolArea patrolarea = GetPatrolArea();
			patrolarea.FindClosest(MovementDirection.Forward);

			
			//to avoid answering questions too often, lets make sure the user
			//has set things up correctly.
			string msg = "";
			if(profile.GetString("PatrolArea") == "")
				msg += "You do not have a Patrol Area selected.\n";
			if(patrolarea != null && patrolarea.NumWaypoints == 0)
				msg += "You do not have any waypoints in your selected Patrol Area\n";
			if(
				profile.GetString("Necro.FightRangedLTQ") == "" ||
				profile.GetString("Necro.FightRangedLTKey") == "" ||
				profile.GetString("Necro.FightRangedPTQ") == "" ||
				profile.GetString("Necro.FightRangedPTKey") == "" ||
				profile.GetString("Necro.FightMeleePTQ") == "" ||
				profile.GetString("Necro.FightMeleePTKey") == "" ||
				profile.GetString("Necro.FightFPQ") == "" ||
				profile.GetString("Necro.FightFPKey") == ""
				)
				msg += "You must set up your fight spell keys before fighting.\n";

			if(msg != "")
			{
				msg += "\nFix these problems and press Resume";
				MessageBox.Show (msg);
				bPaused = true;
				return;
			}

			Interaction.AppActivate(_ak.GameProcess);
			
			
			AddMessage("Starting bot");

			Thread.Sleep(1000);

			//Are we dead?
			if(_ak.IsPlayerDead )
			{
				Action = BotAction.DiedReleaseAndRun;
				return;
			}


			if(petID < 1 || ! _ak.get_DoesObjectExist(petID))
			{
				CastPet();
				//we want to do this in a new thread so it can sleep without sleeping
				//the code the delegate that waits for the pet packet.  It didnt work
				//most of the time if it was in the same thread
//				System.Threading.Thread tPet = new Thread(new ThreadStart(CastPet));
//				tPet.Start();				//start the castPet thread
//				tPet.Priority = ThreadPriority.BelowNormal;
//				Thread.Sleep(0);			//yield so the thread really starts
//				tPet.Join();				//wait for that thread to finish
			}
			Action = BotAction.CheckAgro;
			ActionQueue.Enqueue( BotAction.Protect );
			ActionQueue.Enqueue( BotAction.CheckBuffs);
			ActionQueue.Enqueue( BotAction.Rest);
			ActionQueue.Enqueue( BotAction.FindTarget);
		}
		#endregion
 
		#region DoCheckBuffs
		public override void DoCheckBuffs()
		{
			// Setup default action
			Action = BotAction.CheckAgro;

			// Are we casting, or is pet casting, or are we still casting last buff?
			if( ! cooldowns.IsReady("Buff") || _ak.isPlayerCasting || _ak.get_isMobCasting(petID))
			{
				// Recheck buffs
				if(ActionQueue.Peek(0) != BotAction.CheckBuffs)
					ActionQueue.Insert( 0, BotAction.CheckBuffs);
//				AddMessage("Check buffs says - Already casting!");
				return;
			}

			//just in case
			_ak.StopRunning();

			// Are we sitting?
			if(_ak.isPlayerSitting )
			{
				PlayerKeys.SitStand(KeyDirection.KeyUpDown);
				Thread.Sleep(750);
				cooldowns.SetTime("Global");
				// Recheck buffs
				if(ActionQueue.Peek(0) != BotAction.CheckBuffs)
					ActionQueue.Insert( 0, BotAction.CheckBuffs);
				return;
			}

			//Lets check each buff and cast if needed
			if(cooldowns.IsReady("BuffDex") && profile.GetString("Necro.BuffDexQ") != "" && profile.GetString("Necro.BuffDexKey") != "" )
			{
				AddMessage("Casting Dex Buff");
				_ak.SetTarget(_ak.PlayerID);
				System.Threading.Thread.Sleep(500);
				//switch quickbar and cast
				UseQbar(profile.GetString("Necro.BuffDexQ"), profile.GetString("Necro.BuffDexKey"));
				
				cooldowns.SetTime("BuffDex");
				cooldowns.SetTime("Buff");
				// Recheck buffs
				if(ActionQueue.Peek(0) != BotAction.CheckBuffs)
					ActionQueue.Insert( 0, BotAction.CheckBuffs);
				return;
			}
			if(cooldowns.IsReady("BuffAbsorb") && profile.GetString("Necro.BuffAbsorbQ") != "" && profile.GetString("Necro.BuffAbsorbKey") != "" )
			{
				AddMessage("Casting Absorb Buff");
				_ak.SetTarget(petID);
				System.Threading.Thread.Sleep(500);
				//switch quickbar and cast
				UseQbar(profile.GetString("Necro.BuffAbsorbQ"), profile.GetString("Necro.BuffAbsorbKey"));
				
				cooldowns.SetTime("BuffAbsorb");
				cooldowns.SetTime("Buff");
				// Recheck buffs
				if(ActionQueue.Peek(0) != BotAction.CheckBuffs)
					ActionQueue.Insert( 0, BotAction.CheckBuffs);
				return;
			}
			if(cooldowns.IsReady("BuffStrength") && profile.GetString("Necro.BuffStrengthQ") != "" && profile.GetString("Necro.BuffStrengthKey") != "" )
			{
				AddMessage("Casting Strength Buff");
				_ak.SetTarget(_ak.PlayerID);
				System.Threading.Thread.Sleep(500);
				//switch quickbar and cast
				UseQbar(profile.GetString("Necro.BuffStrengthQ"), profile.GetString("Necro.BuffStrengthKey"));
				
				cooldowns.SetTime("BuffStrength");
				cooldowns.SetTime("Buff");
				// Recheck buffs
				if(ActionQueue.Peek(0) != BotAction.CheckBuffs)
					ActionQueue.Insert( 0, BotAction.CheckBuffs);
				return;
			}
		}
		#endregion 

		#region DoMeleeFight
		public override void DoMeleeFight()
		{
			//just in case
			_ak.StopRunning();

			Action = BotAction.CheckFlee ;
			ActionQueue.Clear();
			ActionQueue.Enqueue( BotAction.Protect );
			ActionQueue.Enqueue( BotAction.MeleeFight );

			//nothing to do if still casting last melee spell
			if( _ak.isPlayerCasting || !cooldowns.IsReady("MeleePT"))
				return;

			//did we start with a melee range fight so currentTarget never got set?
			if(currentTarget < 1 && _ak.TargetIndex > 0)
				currentTarget = _ak.TargetIndex;

			// Do we need to heal the pet?
			if( cooldowns.IsReady( "Heal") && _ak.get_MobHealth(petID) < profile.GetInteger("Necro.HealHealth") && profile.GetString("Necro.HealPetQ") != "" && profile.GetString("Necro.HealPetKey") != "" ) 
			{ 
				AddMessage("Healing pet at health " + _ak.get_MobHealth(petID));
				//switch quickbar and cast
				UseQbar(profile.GetString("Necro.HealPetQ"), profile.GetString("Necro.HealPetKey"));
			
				cooldowns.SetTime("Heal");
				return;
			}
			
			//if too far away for melee fight
			if(_ak.TargetIndex > 0 && DistanceToPet(_ak.TargetIndex) > profile.GetFloat("MinimumRangedAttackDistance"))
			{				
				Action = BotAction.RangedFight ;

				//face target
				PlayerKeys.Face(KeyDirection.KeyUpDown);

				return;
			}

			//do repeated melee range spell
			if(_ak.TargetIndex > 0  )
			{

				AddMessage("Close Range Spell ");
				//switch quickbar and cast
				UseQbar(profile.GetString("Necro.FightMeleePTQ"), profile.GetString("Necro.FightMeleePTKey"));

				cooldowns.SetTime("MeleePT");

				nCountFight++;
			}
			

			if(_ak.TargetIndex < 1 || _ak.get_IsMobDead(_ak.TargetIndex))
			{
				AddMessage("Target Dead");
				// Target is lost, check for agro
				Action = BotAction.CheckAgro;

				ActionQueue.Clear();
				ActionQueue.Enqueue( BotAction.Protect );
				ActionQueue.Enqueue( BotAction.Loot);
				ActionQueue.Enqueue( BotAction.CheckAgro);

				NumKilled++;
				AddMessage(string.Format("==> Number Killed {0} <==",NumKilled));
				nextSpell = eRangedFights.First;
				nCountFight = 0;
				currentTarget = 0;
				Thread.Sleep(500);
				return;
			}
		}
		#endregion

		#region DoRangedFight
		public override void DoRangedFight()
		{
			//just in case
			_ak.StopRunning();

			Action = BotAction.CheckFlee;
			ActionQueue.Clear();
			ActionQueue.Enqueue( BotAction.Protect );
			ActionQueue.Enqueue( BotAction.RangedFight );

			//if target close enough to switch to melee fight, do it
			//done before cooldown check since we can cast melee range spells immediately
			if(_ak.TargetIndex > 0 && DistanceToPet(_ak.TargetIndex) < profile.GetFloat("MinimumRangedAttackDistance"))
			{
				//put pet back in defensive mode
				UseQbar(profile.GetString("Necro.PetDefensiveQ"), profile.GetString("Necro.PetDefensiveKey"));
				
				Action = BotAction.MeleeFight;

				//face target
				PlayerKeys.Face(KeyDirection.KeyUpDown);

				return;
			}

			//nothing to do if cooldowns arent ready
			if( _ak.isPlayerCasting || _ak.get_isMobCasting(petID) || !cooldowns.IsReady("Ranged"))
				return;

			//check if we targeted something in a dungeon we cant get see, "not in line of sight"
			if(bNoLineOfSight) 
				if(_ak.TargetIndex > 0)
				{
					IgnoreThis(_ak.TargetIndex);
					ActionQueue.Clear();
					ActionQueue.Enqueue( BotAction.FindTarget  );
					//lets clear the stuff we use to test for unfightable targets
					currentTarget = 0;
					AddMessage("In Ranged Fight, no LOS, added to bad target list");
					return;
				}

			//check if this is a new fight, so we can store our mob for the evade test below
			if(currentTarget < 1 && _ak.TargetIndex > 0)
				currentTarget = _ak.TargetIndex ;

			//Got out of range somehow?
			if(_ak.TargetIndex > 0 && DistanceToPet(_ak.TargetIndex) > profile.GetFloat("MaximumRangedAttackDistance"))
			{
				AddMessage("Target out of range, moving closer");

				//for some reason we need to break auto face
				//before moveto will work right
				PlayerKeys.TurnLeft(KeyDirection.KeyDown );
				Thread.Sleep(25);
				PlayerKeys.TurnLeft(KeyDirection.KeyUp );

				Action = BotAction.CheckAgro;
				ActionQueue.Clear();
				ActionQueue.Enqueue( BotAction.Protect );
				ActionQueue.Enqueue( BotAction.GetInRange);
				ActionQueue.Enqueue( BotAction.RangedFight);

				return;
			}

			if(nextSpell == eRangedFights.First)
				AddMessage(string.Format("Fight Starting at distance: {0}",DistanceToPet(_ak.TargetIndex)));

			//if this is our first ranged cast we get to cast twice to fill up the queue
			if (_ak.TargetIndex > 0 && 
				nextSpell == eRangedFights.First && 
				cooldowns.IsReady("Global") && 
				! _ak.isPlayerCasting ) 
			{       
				//put pet back in passive mode so it doesnt run to mob
				UseQbar(profile.GetString("Necro.PetPassiveQ"), profile.GetString("Necro.PetPassiveKey"));

				nextSpell = eRangedFights.Power;  //default

				//do we need health more than power?
				if(_ak.PlayerMana > 75 && _ak.get_MobHealth(petID) < 75)
					nextSpell = eRangedFights.Health;

				//Now cast the first spell
				if(nextSpell == eRangedFights.Power)
				{
					AddMessage("Casting Power Tap ");
					//switch quickbar and cast
					UseQbar(profile.GetString("Necro.FightRangedPTQ"), profile.GetString("Necro.FightRangedPTKey"));
				}
				else
				{
					AddMessage("Casting Life Tap ");
					//switch quickbar and cast
					UseQbar(profile.GetString("Necro.FightRangedLTQ"), profile.GetString("Necro.FightRangedLTKey"));
				}

				nextSpell = eRangedFights.Debuff ;
				//Thread.Sleep(500);
			} 
			
			//Debuff
			if(_ak.TargetIndex > 0 &&				//make sure we have a target
				nextSpell == eRangedFights.Debuff  &&			//not pull spell
				cooldowns.IsReady("Ranged") &&				//not still casting previous ranged spell
				!_ak.isPlayerCasting )						//not casting anything
			{
				// Are the instant debuff keys set?  If so use it
				if(profile.GetString("Necro.DebuffQ") != "" &&
					profile.GetString("Necro.DebuffKey") != "")
				{
					AddMessage("Debuffing");
					UseQbar(profile.GetString("Necro.DebuffQ"), profile.GetString("Necro.DebuffKey"));
				}

//				cooldowns.SetTime("Ranged");
				nextSpell = eRangedFights.Power ;

				return;
			}
			
			//cast subsequent ranged spells
			if(_ak.TargetIndex > 0 &&				//make sure we have a target
				nextSpell != eRangedFights.First &&			//not pull spell
				cooldowns.IsReady("Ranged") &&				//not still casting previous ranged spell
				!_ak.isPlayerCasting )						//not casting anything
			{

				nextSpell = eRangedFights.Power;  //default

				//do we need health more than power?
				if(_ak.PlayerMana > 75 && _ak.get_MobHealth(petID) < 75)
					nextSpell = eRangedFights.Health;

				//Now cast the spell
				if(nextSpell == eRangedFights.Power)
				{
					AddMessage("Casting Power Tap - Range " + DistanceToPet(_ak.TargetIndex).ToString());
					//switch quickbar and cast
					UseQbar(profile.GetString("Necro.FightRangedPTQ"), profile.GetString("Necro.FightRangedPTKey"));
				}
				else
				{
					AddMessage("Casting Life Tap - Range " + DistanceToPet(_ak.TargetIndex).ToString());
					//switch quickbar and cast
					UseQbar(profile.GetString("Necro.FightRangedLTQ"), profile.GetString("Necro.FightRangedLTKey"));
				}
				cooldowns.SetTime("Ranged");

				nCountFight++;
			}
	
			if(_ak.TargetIndex < 1 || _ak.get_IsMobDead(_ak.TargetIndex) )
			{
				AddMessage("Target Dead");
				// Target is lost, check for agro
				Action = BotAction.CheckAgro;

				ActionQueue.Clear();
				ActionQueue.Enqueue( BotAction.Protect );
				ActionQueue.Enqueue( BotAction.Loot);
				ActionQueue.Enqueue( BotAction.CheckAgro);

				NumKilled++;
				AddMessage(string.Format("==> Number Killed {0} <==",NumKilled));
				nextSpell = eRangedFights.First;
				nCountFight = 0;
				currentTarget = 0;
				Thread.Sleep(500);
				return;
			}

		}	
		#endregion

		#region DoCheckFlee
		public override void DoCheckFlee()
		{

			int numAttackers = GetAttackerCount();
			
			// Less then specified hp or mana plus 10%, and more then 1 attacker?
			if( (_ak.get_MobHealth(petID) < profile.GetInteger("FleeBelowHealth") + 10  ||
				 _ak.PlayerMana < profile.GetInteger("FleeBelowMana") + 10) && 
				numAttackers > 1)
			{
				// Yes? RUN FOREST RUN !
				// Should we leave the pet behind to die? 
				if (profile.GetBool("Necro.FleeLeavePet"))
				{
					AddMessage("Leave pet to die as we flee"); 
					//switch quickbar and cast
					UseQbar(profile.GetString("Necro.PetHereQ"), profile.GetString("Necro.PetHereKey"));
				}

				Action = BotAction.Flee;

				ActionQueue.Clear();
				ActionQueue.Enqueue( BotAction.Rest);
				return;
			}

			//Just one attacker, and below health at which we should run away?
			if( _ak.get_MobHealth(petID) < profile.GetInteger("FleeBelowHealth") ||
				_ak.PlayerMana < profile.GetInteger("FleeBelowMana"))
			{
				// Our attacker has more HP then us?
				if(_ak.TargetIndex > 0 && _ak.get_MobHealth(petID) < _ak.TargetObject.Health )
				{
					// Yes? RUN FOREST RUN !
					// Should we leave the pet behind to die? 
					if (profile.GetBool("Necro.FleeLeavePet"))
					{
						AddMessage("Leave pet to die as we flee"); 
						//switch quickbar and cast
						UseQbar(profile.GetString("Necro.PetHereQ"), profile.GetString("Necro.PetHereKey"));
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
		#endregion

		#region Get in range / overriden to use distance from pet to target
		public override void DoGetInRange()
		{
			nextSpell = eRangedFights.First;

			// Do we have a target and is it not fighting someone else?
			if( _ak.TargetIndex < 1 || (_ak.TargetObject != null && _ak.TargetObject.TargetID != 0 && _ak.TargetObject.TargetID != _ak.PlayerID ))
			{	// No? Stop moving
				_ak.StopRunning();

				// Check agro
				Action = BotAction.Protect;

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
					ActionQueue.Enqueue( BotAction.CheckAgro );
					ActionQueue.Enqueue( BotAction.FindTarget);
				}
				
				return;
			}

			// Get the next action from the queue
			BotAction NextAction = ActionQueue.Dequeue();

			// By default assume a ranged attack distance (move closer then the max, so mobs that run away are caught easier)
			float AttackDistance = profile.GetFloat("MaximumRangedAttackDistance") * 0.95f;

			// Calculate the distance to the target FROM PET
			float TargetDistance = DistanceToPet(_ak.TargetIndex);

			// Check if we're past the minimum distance, go to melee distance else
			if( TargetDistance < profile.GetFloat("MinimumRangedAttackDistance"))
				NextAction = BotAction.MeleeFight;

			// Should we go to melee attack distance?
			if( NextAction == BotAction.MeleeFight || (ActionQueue.Count > 0 && ActionQueue.Peek(0) == BotAction.MeleeFight ))
				AttackDistance = 200;  //200 should be close enough to /stick to the target

			// TODO: Add (extra) stuck checking here

			float distToWalk = TargetDistance - AttackDistance;

//			AddMessage("DEBUG: distToWalk " + distToWalk.ToString() + " target index " + _ak.TargetIndex);

			// Do our final distance move?
			if( _ak.TargetIndex > 0 && distToWalk <= 200)
			{
				// Do our final move
				MoveTo( _ak.TargetIndex, AttackDistance, true);

				// make sure we are facing the target
				PlayerKeys.Face(KeyDirection.KeyUpDown  );

				// One final agro check, might be the same monster though
				Action = BotAction.CheckAgro;

				// Make sure we do the appropriate action
				ActionQueue.Clear();
				ActionQueue.Enqueue( BotAction.Protect );
				ActionQueue.Enqueue( NextAction);
			}
			else
			{
				// Walk to our target in small steps
				if ( _ak.TargetIndex > 0)
					MoveTo( _ak.TargetIndex, TargetDistance - (distToWalk / 4), false);

				// Recheck if our target is still the closest once, only if not incombat
				// Use multiple checks because once in a great while GetAttackerCount is 0 and something is hitting us
				if( GetAttackerCount() == 0 ||  _ak.isPlayerInCombat || (petID > 0 && ! _ak.get_isMobInCombat(petID)))
				{
					// Setup our bot actions
					Action = BotAction.Protect;
					ActionQueue.Clear();
					ActionQueue.Enqueue( BotAction.CheckAgro );
					ActionQueue.Enqueue( BotAction.FindTarget);
				}
				else
				{	// In combat, just get in range

					//for some reason we need to break auto face
					//before moveto will work right
					PlayerKeys.TurnLeft(KeyDirection.KeyDown );
					Thread.Sleep(25);
					PlayerKeys.TurnLeft(KeyDirection.KeyUp );

					Action = BotAction.GetInRange;
					ActionQueue.Clear();
					ActionQueue.Enqueue( BotAction.Protect );
					ActionQueue.Enqueue( NextAction);
				}
			}
		}
		#endregion

		#region protect group
		public override void DoCheckAgro()
		{
			// Override to insert a protect during every checkAgro, should help like when we are running somewhere
			ActionQueue.Insert(0, BotAction.Protect );
			base.DoCheckAgro ();
		}

		public override void DoProtect()
		{
//			Do we have instant debuff set? If not dont bother trying to protect (for now)
			if(profile.GetString("Necro.DebuffQ") == "" ||
                profile.GetString("Necro.DebuffKey") == "")
				return;

			try
			{
				// Retrieve a list of objects near enough to be attacking our group
				int myX = _ak.gPlayerXCoord;
				int myY = _ak.gPlayerYCoord;
				AKServer.DLL.DAoCServer.Group groupMembers = _ak.GroupMemberInfo;
				
				// In a group? If not just return
				if(groupMembers.GroupMemberTable.Count == 0)
					return;

				ArrayList pets = new ArrayList();
				if(petID > 0)
					pets.Add(petID);

				int spawnID = _ak.SearchArea((int)profile.GetFloat( "SearchDistance"),AKServer.DLL.DAoCServer.DAOCObjectClass.ocMob,pets);
				while(spawnID != -1 )
				{
					int mobTarget = _ak.get_MobTarget(spawnID );

					foreach ( AKServer.DLL.DAoCServer.GroupMember  grpMember in  groupMembers.GroupMemberTable.Values   )
					{
						//Is this group member being attacked?
						if( grpMember.ID == mobTarget)
						{ //Yes, use instant debuff to get aggro!
							if(_ak.isPlayerSitting )
							{
								PlayerKeys.SitStand(KeyDirection.KeyUpDown );
								Thread.Sleep(500);
							}
							_ak.SetTarget(spawnID);
							Thread.Sleep(150);
							PlayerKeys.Face(KeyDirection.KeyUpDown);
							UseQbar(profile.GetString("Necro.DebuffQ"), profile.GetString("Necro.DebuffKey"));
							AddMessage("Protected " + _ak.get_MobName(mobTarget) + " by attacking " + _ak.get_MobName(spawnID));
						}
					}
					spawnID = _ak.SearchAreaNext((int)profile.GetFloat( "SearchDistance"),AKServer.DLL.DAoCServer.DAOCObjectClass.ocMob,pets);
				}
			}
			catch (Exception e)
			{
				AddMessage("caught exception " + e.Message );
			}
			finally
			{
				// Check if we have a next action
				if( ActionQueue.Count == 0)
					throw new Exception("No action in ActionQueue");

				// Next Action
				Action = ActionQueue.Dequeue();
			}			
		}

		#endregion

		#region Pet
		private void CastPet()
		{
			petID = -1;

			AddMessage("Summoning servant");

			if(_ak.isPlayerSitting)
			{
				PlayerKeys.SitStand(KeyDirection.KeyUpDown);
				Thread.Sleep(1000);
			}

			//test if we are already a shade by checking local buffs
			bool bShade = false;
			AKServer.DLL.DAoCServer.LocalBuffList buffs = _ak.LocalBufs;
			ICollection MyValues = buffs.BuffTable.Values;
			foreach( AKServer.DLL.DAoCServer.LocalBuff buff in MyValues)
				if(buff.Name.StartsWith("Shade"))
					bShade = true;

			//If we are a shade, we need to change pet stance to get a pet packet from server
			if(bShade)
			{
				//it appears if you log out while the pet is summoned AKServer thinks you still have the Shade buff when you log in again
				//so we try and cast just in case.
				UseQbar(profile.GetString("Necro.SummonPetQ"), profile.GetString("Necro.SummonPetKey"));
				
				AddMessage("We are already a shade! Trying to get pet ID...");
				UseQbar(profile.GetString("Necro.PetPassiveQ"),profile.GetString("Necro.PetPassiveKey"));
				Thread.Sleep(100);
				UseQbar(profile.GetString("Necro.PetDefensiveQ"),profile.GetString("Necro.PetDefensiveKey"));
				Thread.Sleep(100);
			}
			else
			{
				//switch quickbar and cast
				UseQbar(profile.GetString("Necro.SummonPetQ"), profile.GetString("Necro.SummonPetKey"));
			}

			//No idea why, but I get the localBuff "Shade" sometimes even after a reboot and before casting pet
			//So lets do both and see if we get the packet
//			UseQbar(profile.GetString("PetPassiveQ"),profile.GetString("PetPassiveKey"));
//			Thread.Sleep(100);
//			UseQbar(profile.GetString("PetDefensiveQ"),profile.GetString("PetDefensiveKey"));
//			Thread.Sleep(100);
//			UseQbar(profile.GetString("SummonPetQ"), profile.GetString("SummonPetKey"));


			//Now we can wait for a pet packet to which will fill in petID
			//check once a second for up to 30 seconds
			int sanity = 0;  //no endless loops please
			while(petID <= 0 && sanity++ < 30)
				Thread.Sleep(1000);

			// Did we get the petID?
			if(petID <= 0)
			{
				AddMessage("We didnt find the pet ID!  Restart everything");
				bPaused = true;
			}
			else
				AddMessage("Got Pet ID " + petID.ToString() + " for pet " + _ak.get_MobName(petID));

		}
		#endregion

		#region ParseChatLogLine
		/// <summary>
		/// Handler for chat log parser
		/// </summary>
		public override void ParseChat( AutoKillerScript.clsAutoKillerScript.AutokillerRegExEventParams e)
		{
			string logline = e.Logline;
			
			if(logline.IndexOf("can't see its target!") > -1)
			{
				AddMessage("Can't see target!");
				bNoLineOfSight = true;
			}

			if(logline.IndexOf("servant is too far away from you ") > -1)
			{
				AddMessage("Servant wandered off!");
				//put pet back in passive mode so it doesnt run to mob
				UseQbar(profile.GetString("Necro.PetPassiveQ"), profile.GetString("Necro.PetPassiveKey"));
			}

			base.ParseChat(e);
		}
		#endregion

	}
}
