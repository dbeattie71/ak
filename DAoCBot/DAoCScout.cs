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
	/// Summary description for DAoC_Scout.
	/// </summary>
	public class DAoC_Scout : DAoC_BotBase
	{
		public enum eRangedFights
		{
			Crit,
			Bow,
			RapidFire
		}
		public enum eMeleeFights
		{
			Normal,
			NormalChain,
			Blocked,
			BlockedChain
		}
		internal eRangedFights nextRanged;
		internal eMeleeFights nextMelee;
		int nCountFight = 0;
		int NumKilled = 0;
		int currentTarget = -1;
		internal bool playerShooting = false;
		internal bool playerSwinging = false;
		

		public DAoC_Scout( AutoKillerScript.clsAutoKillerScript ak, Profile profile, PatrolAreas patrolareas) 
			: base( ak, profile, patrolareas )
		{
			_ak = ak;

			cooldowns.DefineCooldown( "Stealth", 10 * 1000);
			cooldowns.DefineCooldown( "FightDelay", (int)(1.5 * 1000));
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
				profile.GetString("Scout.FightRangedBowQ") == "" ||
				profile.GetString("Scout.FightRangedBowKey") == "" ||
				profile.GetString("Scout.SlashNormalQ") == "" ||
				profile.GetString("Scout.SlashNormalKey") == "" ||
				profile.GetString("Scout.FightMeleeWeaponQ") == "" ||
				profile.GetString("Scout.FightMeleeWeaponKey") == ""
				)
				msg += "You must set up your fight keys before fighting.\n";

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

			Action = BotAction.CheckAgro;
			ActionQueue.Enqueue( BotAction.Rest);
			ActionQueue.Enqueue( BotAction.FindTarget);
		}
		#endregion
 
		#region DoMeleeFight
		public override void DoMeleeFight()
		{
			if(_ak.TargetIndex < 1 || _ak.get_IsMobDead(_ak.TargetIndex))
			{
				AddMessage("Target Dead");
				// Target is lost, check for agro
				Action = BotAction.CheckAgro;

				ActionQueue.Clear();
				ActionQueue.Enqueue( BotAction.Loot);
				ActionQueue.Enqueue( BotAction.CheckAgro);

				playerShooting = false;
				playerSwinging = false;
				NumKilled++;
				AddMessage(string.Format("==> Number Killed {0} <==",NumKilled));
				nextRanged = eRangedFights.Crit;
				nCountFight = 0;
				currentTarget = 0;
				Thread.Sleep(500);

				//select bow
				AddMessage("selecting bow");
				UseQbar(profile.GetString("Scout.FightRangedBowQ"), profile.GetString("Scout.FightRangedBowKey"));

				return;
			}

			//just in case
			_ak.StopRunning();

			Action = BotAction.CheckFlee ;
			ActionQueue.Clear();
			ActionQueue.Enqueue( BotAction.MeleeFight );

			//are we still doing the last style?
			if(playerSwinging || !cooldowns.IsReady("FightDelay"))
				return;

			//did we start with a melee range fight so currentTarget never got set?
			if(currentTarget < 1 && _ak.TargetIndex > 0)
			{
				//switch to melee weapon
				UseQbar(profile.GetString("Scout.FightMeleeWeaponQ"), profile.GetString("Scout.FightMeleeWeaponKey"));
				currentTarget = _ak.TargetIndex;
//				playerShooting = false;
//				playerSwinging = false;
			}

			//do repeated melee range spell
			if(_ak.TargetIndex > 0  )
			{
				AddMessage("Close Range Style " + nextMelee.ToString());

				switch (nextMelee)
				{
					case eMeleeFights.Normal:
						UseQbar(profile.GetString("Scout.SlashNormalQ"), profile.GetString("Scout.SlashNormalKey"));
						if(profile.GetString("Scout.SlashNormalChainQ") != "" &&
							profile.GetString("Scout.SlashNormalChainKey") != "")
							nextMelee = eMeleeFights.NormalChain;
						break;
				
					case eMeleeFights.NormalChain:
						UseQbar(profile.GetString("Scout.SlashNormalChainQ"), profile.GetString("Scout.SlashNormalChainKey"));
						//done with chain, back to normal fight
						nextMelee = eMeleeFights.Normal;
						break;
				
					case eMeleeFights.Blocked:
						UseQbar(profile.GetString("Scout.SlashBlockQ"), profile.GetString("Scout.SlashBlockKey"));
						//if user has set up a chain for blocked styles, do it next time
						if(profile.GetString("Scout.SlashBlockChainQ") != "" &&
							profile.GetString("Scout.SlashBlockChainKey") != "")
							nextMelee = eMeleeFights.NormalChain;
						else
							nextMelee = eMeleeFights.Normal;
						break;
				
					case eMeleeFights.BlockedChain:
						UseQbar(profile.GetString("Scout.SlashBlockChainQ"), profile.GetString("Scout.SlashBlockChainKey"));
						//done with chain, back to normal fight
						nextMelee = eMeleeFights.Normal;
						break;
				
		
				}
				//make sure we give the game time to send the chat string "you prepare"
				cooldowns.SetTime("FightDelay");
				playerSwinging = true;

				nCountFight++;
			}
			

		}
		#endregion

		#region DoRangedFight
		public override void DoFindTarget()
		{
//			playerShooting = false;
//			playerSwinging = false;
			nextRanged = eRangedFights.Crit;
			currentTarget = 0;
			
			base.DoFindTarget ();
		}

		public override void DoRangedFight()
		{
			if(_ak.TargetIndex < 1 || _ak.get_IsMobDead(_ak.TargetIndex) )
			{
				AddMessage("Target Dead");
				// Target is lost, check for agro
				Action = BotAction.CheckAgro;

				ActionQueue.Clear();
				ActionQueue.Enqueue( BotAction.Loot);
				ActionQueue.Enqueue( BotAction.CheckAgro);

				playerShooting = false;
				playerSwinging = false;
				NumKilled++;
				AddMessage(string.Format("==> Number Killed {0} <==",NumKilled));
				nextRanged = eRangedFights.Crit;
				nCountFight = 0;
				currentTarget = 0;
				//in case we have bow drawn, make sure to put it away so we can 
				//use crit shot for next kill even if we dont run anywhere
				UseQbar(profile.GetString("Scout.FightMeleeWeaponQ"), profile.GetString("Scout.FightMeleeWeaponKey"));

				return;
			}

			//just in case
			_ak.StopRunning();

			Action = BotAction.CheckFlee;
			ActionQueue.Clear();
			ActionQueue.Enqueue( BotAction.RangedFight );

			//if target close enough to switch to melee fight, do it
			//done before cooldown check since we can cast melee range spells immediately
			if(_ak.TargetIndex > 0 && DistanceToMob(_ak.TargetIndex) < profile.GetFloat("MinimumRangedAttackDistance"))
			{				
				Action = BotAction.MeleeFight;

				//face target
				PlayerKeys.Face(KeyDirection.KeyUpDown);

				//switch to melee weapon
				UseQbar(profile.GetString("Scout.FightMeleeWeaponQ"), profile.GetString("Scout.FightMeleeWeaponKey"));

				return;
			}

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

			//check if this is a new fight, so we can store our mob
			if(currentTarget < 1 && _ak.TargetIndex > 0)
			{
				currentTarget = _ak.TargetIndex ;
				nextRanged = eRangedFights.Crit;
//				playerShooting = false;
//				playerSwinging = false;
			}
			//Got out of range somehow?
			if(_ak.TargetIndex > 0 && DistanceToMob(_ak.TargetIndex) > profile.GetFloat("MaximumRangedAttackDistance"))
			{
				AddMessage("Target out of range, moving closer");

				//for some reason we need to break auto face
				//before moveto will work right
				PlayerKeys.TurnLeft(KeyDirection.KeyDown );
				Thread.Sleep(25);
				PlayerKeys.TurnLeft(KeyDirection.KeyUp );

				Action = BotAction.GetInRange;
				ActionQueue.Clear();
				ActionQueue.Enqueue( BotAction.RangedFight);

				return;
			}

			//nothing more to do if still shooting
			if( playerShooting || !cooldowns.IsReady("FightDelay"))
				return;

			if(_ak.PlayerLeftHand != AutoKillerScript.WeaponSlots.Ranged )
				UseQbar(profile.GetString("Scout.FightRangedBowQ"), profile.GetString("Scout.FightRangedBowKey"));

			if(nextRanged == eRangedFights.Crit)
				AddMessage(string.Format("Fight Starting at distance: {0}",DistanceToMob(_ak.TargetIndex)));

			//if this is our first ranged fight, do crit shot
			if (_ak.TargetIndex > 0 && 
				nextRanged == eRangedFights.Crit && 
				! playerShooting ) 
			{       
				// Do the crit shot
				UseQbar(profile.GetString("Scout.FightRangedCritQ"), profile.GetString("Scout.FightRangedCritKey"));

				//release and reload
				UseQbar(profile.GetString("Scout.FightRangedBowQ"), profile.GetString("Scout.FightRangedBowKey"));
				UseQbar(profile.GetString("Scout.FightRangedBowQ"), profile.GetString("Scout.FightRangedBowKey"));

				playerShooting = true;
				nextRanged = eRangedFights.Bow ;  
				cooldowns.SetTime("FightDelay");
				nCountFight++;

				return;
			} 
			
			if(_ak.TargetIndex > 0 &&				//make sure we have a target
				nextRanged == eRangedFights.Bow &&			//not pull spell
				! playerShooting )						//not casting anything
			{

				AddMessage("Regular bow shot - Range " + DistanceToMob(_ak.TargetIndex).ToString());

				//release and reload
				UseQbar(profile.GetString("Scout.FightRangedBowQ"), profile.GetString("Scout.FightRangedBowKey"));
				UseQbar(profile.GetString("Scout.FightRangedBowQ"), profile.GetString("Scout.FightRangedBowKey"));

				playerShooting = true;
				nextRanged = eRangedFights.Bow ;  
				cooldowns.SetTime("FightDelay");
				nCountFight++;
			}
	
		}	
		#endregion
		
		public override void DoCheckBuffs()
		{
			// Setup default action
			Action = BotAction.CheckAgro;
		}

		#region protect group

		public override void DoProtect()
		{
			
			// Check if we have a next action
			if( ActionQueue.Count == 0)
				throw new Exception("No action in ActionQueue");

			// Next Action
			Action = ActionQueue.Dequeue();
			
		}

		#endregion

		#region DoCheckFlee
		public override void DoCheckFlee()
		{

			int numAttackers = GetAttackerCount();
			
			// Less then specified hp or mana plus 10%, and more then 1 attacker?
			if( _ak.PlayerHealth < profile.GetInteger("FleeBelowHealth") + 10 &&
				numAttackers > 1)
			{
				// Yes? RUN FOREST RUN !
				AddMessage("Run away!");
				Action = BotAction.Flee;
				PlayerKeys.Sprint(KeyDirection.KeyUpDown);

				ActionQueue.Clear();
				ActionQueue.Enqueue( BotAction.Rest);
				return;
			}

			//Just one attacker, and below health at which we should run away?
			if( _ak.PlayerHealth < profile.GetInteger("FleeBelowHealth") )
			{
				// Our attacker has more HP then us?
				if(_ak.TargetIndex > 0 && _ak.PlayerHealth < _ak.TargetObject.Health )
				{
					// Yes? RUN FOREST RUN !
					AddMessage("Run away!");
					Action = BotAction.Flee;
					PlayerKeys.Sprint(KeyDirection.KeyUpDown);

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

		#region ParseChatLogLine
		/// <summary>
		/// Handler for chat log parser
		/// </summary>
		public override void ParseChat( AutoKillerScript.clsAutoKillerScript.AutokillerRegExEventParams e)
		{
			string logline = e.Logline;
			
//			_ak.AddString(7, "You prepare your shot");
//			_ak.AddString(8, "You miss");
//			_ak.AddString(9, "You move and interrupt");
//			_ak.AddString(10, "You shoot");
			if(logline.IndexOf("You prepare to perform") > -1)
			{
				playerSwinging = true;
			}
			if(logline.IndexOf("You miss") > -1 ||
				logline.IndexOf("You hit ") > -1 ||
				logline.IndexOf("You attack ") > -1 ||
				logline.IndexOf("You fumble ") > -1 ||
				logline.IndexOf("You move and interrupt") > -1 ||
				logline.IndexOf("You shoot") > -1
				)
			{
				playerShooting = false;
				playerSwinging = false;
			}

			//Did we block, and if so is there a block style to use?
			if(logline.IndexOf("you block the blow") > -1 &&
				profile.GetString("Scout.SlashBlockQ") != "" &&
				profile.GetString("Scout.SlashBlockKey") != "")
					nextMelee = eMeleeFights.Blocked ;


			base.ParseChat(e);
		}
		#endregion

	}
}
