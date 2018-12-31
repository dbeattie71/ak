//------------------------------------------------------------------------------
//  <copyright from='2005' to='2006' company='WoWSharp.NET'>
//    Copyright (c) WoWSharp.NET. All Rights Reserved.
//
//    Please look in the accompanying license.htm file for the license that 
//    applies to this source code.(a copy can also be found at: 
//    http://www.wowsharp.net/license.htm)
//  </copyright>
//-------------------------------------------------------------------------------
using System;
using System.Threading;
using System.Collections;
using System.Xml;
using System.Text.RegularExpressions;
using WoW_Sharp;

namespace WoW_Bot
{
	/// <summary>
	/// Summary description for WoW_Rogue.
	/// </summary>
	public class WoW_Rogue : WoW_BotBase
	{
		private bool _castedpoison = false;
		private double NumberKilled = 0;
		private bool SuspectedCaster = false;
		private int NoOfRangePull = 0;
		private bool FirstComboMove = true;
		private bool YouHaveParried = false;
		private int SSEnergy = 45;
		private bool HaveAmmo = true;
		public ArrayList ListOfCrapLoot;
		public float CheckTargetDistance;
		public int LookingForTargetTime = 0;
		public int NotFightingOurMob = 0;
		public bool FoundTarget = false;
		public bool WeAreResting = false;

		/// <summary>
		/// List of all known throwing weapons
		/// </summary>
		public ArrayList ListOfThrowing;

		// List of hostile players
//		public ArrayList HostileAttackingPlayers;

		public WoW_Rogue( WoW wow, Profile profile, PatrolAreas patrolareas, Localization localization) : base( wow, profile, patrolareas, localization)
		{	// Define rogue specific cooldowns
			cooldowns.DefineCooldown( "CasterDetection", 3000);
			cooldowns.DefineCooldown( "RangePull", (int)(profile.GetFloat("RangeCooldown") * 1000));
			cooldowns.DefineCooldown( "Gouge", 10 * 1000);
			cooldowns.DefineCooldown( "Kick", 10 * 1000);
			cooldowns.DefineCooldown( "Cold Blood", 180 * 1000);
			cooldowns.DefineCooldown( "Preparation", 600 * 1000); 
			cooldowns.DefineCooldown( "Blade Flurry", 120 * 1000);
			cooldowns.DefineCooldown( "Riposte", 6 * 1000);
			cooldowns.DefineCooldown( "Hemorrhage", 15 * 1000);
			cooldowns.DefineCooldown( "Premeditation", 120 * 1000);
			cooldowns.DefineCooldown( "Adrenaline Rush", 360 * 1000);
			cooldowns.DefineCooldown( "Ghostly Strike", 20 * 1000);
			ListOfCrapLoot = new ArrayList();
		

//			HostileAttackingPlayers = ArrayList.Synchronized(new ArrayList());

			//Define the Energy Requirements for Sinister Strike.
			if(profile.GetInteger("Rogue.ImprovedSS") == 1)
			{
				SSEnergy = 42;
			}
			else if(profile.GetInteger("Rogue.ImprovedSS") == 2)
			{
				SSEnergy = 40;
			}
			else 
			{
				SSEnergy = 45;
			}

			//Adjust Cooldowns based on Talents
			if(profile.GetInteger("Rogue.ElusivenessRank") != 0)
			{
				int tt = 0;
				tt = (profile.GetInteger("Rogue.ElusivenessRank") * 15);
				cooldowns.DefineCooldown( "Vanish", (300 - tt) * 1000);
				cooldowns.DefineCooldown( "Evasion", (300 - tt) * 1000);
				cooldowns.DefineCooldown( "Blind", (300 - tt) * 1000);		
			}
			else if(profile.GetInteger("Rogue.ElusivenessRank") == 0)
			{
				cooldowns.DefineCooldown( "Evasion", 300 * 1000);
				cooldowns.DefineCooldown( "Vanish", 300 * 1000);
				cooldowns.DefineCooldown( "Blind", 300 * 1000);
			}

			// Adjust Cooldown for Stealth if needed
			if(profile.GetInteger("Rogue.RapidConcealmentRank") != 0)
			{
				int tt = 0;
				tt = (profile.GetInteger("Rogue.RapidConcealmentRank") * 1);
				cooldowns.DefineCooldown( "Stealth", (10 - tt) * 1000);
			}
			else if(profile.GetInteger("Rogue.RapidConcealmentRank") == 0)
			{
				cooldowns.DefineCooldown( "Stealth", 10 * 1000);
			}

			// Adjust Cooldown for Sprint if needed
			if(profile.GetInteger("Rogue.ImprovedSprintRank") != 0)
			{
				int tt = 0;
				tt = (profile.GetInteger("Rogue.ImprovedSprintRank") * 30);
				cooldowns.DefineCooldown( "Sprint", (300 - tt) * 1000);
			}
			else if(profile.GetInteger("Rogue.ImprovedSprintRank") == 0)
			{
				cooldowns.DefineCooldown( "Sprint", 300 * 1000);
			}

			// Adjust Cooldown for Sprint if needed
			if(profile.GetInteger("Rogue.ImprovedKidneyShotRank") == 1 || profile.GetInteger("Rogue.ImprovedKidneyShotRank") == 2)
			{
				int tt = 0;
				tt = (profile.GetInteger("Rogue.ImprovedKidneyShotRank") * 2);
				cooldowns.DefineCooldown( "Kidney Shot", (20 - tt) * 1000);
			}
			else if (profile.GetInteger("Rogue.ImprovedKidneyShotRank") == 3)
			{
				cooldowns.DefineCooldown( "Kidney Shot", (20 - 5) * 1000);
			}
			
			else if(profile.GetInteger("Rogue.ImprovedKidneyShotRank") == 0)
			{
				cooldowns.DefineCooldown( "Kidney Shot", 20 * 1000);
			}

		
			//Load in the Thrown Table
			ListOfThrowing = new ArrayList();

			XmlDocument doc = new XmlDocument();
			try
			{
				doc.Load( "throwing.xml");
				foreach( XmlNode node in doc.SelectNodes("//weapon/@id"))
				ListOfThrowing.Add( int.Parse( node.Value));
			}
			catch
			{
			}
		}

		//Use Stealth all the time Routine.
		bool _hadstealth = false;
		public override void DoAction()
		{	
			// If we're not dead and we have stealth, do the stealth stuff
			if( wow.Player != null && 
				!wow.Player.IsDead && 
				wow.Buffs[localization.GetString("ghost")] == null &&
				profile.GetBool("Rogue.UseStealth"))
			{	
				// Handle the stealth part of the stealth timer
				if( !_hadstealth && wow.Buffs[localization.GetString("stealth")] != null)
					_hadstealth = true;

				if( _hadstealth && wow.Buffs[localization.GetString("stealth")] == null)
				{
					_hadstealth = false;
					cooldowns.SetTime( "Stealth");
					cooldowns.SetTime( "Global");
				}

				if( !wow.Player.IsInCombat && 
					wow.Buffs[localization.GetString("stealth")] == null &&
					!wow.Player.IsCasting)
				{
					bool DoStealth = true;

					if( Action == BotAction.Loot || (ActionQueue.Count > 0 && ActionQueue.Peek(0) == BotAction.Loot))
						DoStealth = false;

					if( Action == BotAction.Skinning || (ActionQueue.Count > 0 && ActionQueue.Peek(0) == BotAction.Skinning))
						DoStealth = false;

					if( Action == BotAction.Rest || (ActionQueue.Count > 0 && ActionQueue.Peek(0) == BotAction.Rest))
						DoStealth = false;

					if( Action == BotAction.CheckBuffs || (ActionQueue.Count > 0 && ActionQueue.Peek(0) == BotAction.CheckBuffs))
						DoStealth = false;

					if( DoStealth && wow.Buffs[localization.GetString("stealth")] == null)
					{
						do
						{
							System.Threading.Thread.Sleep(200);
						}while (!cooldowns.IsReady( "Stealth") && wow.Target == null);
						
						_hadstealth = true;
						cooldowns.SetTime( "Stealth");
						cooldowns.SetTime( "Global");

						wow.CastSpellByName( localization.GetString("stealth"));
						System.Threading.Thread.Sleep(500);  //so botbase shadowmeld sees this buff if we happen to be resting
					}
				}
			}

			base.DoAction ();
		}

		public override void DoRest()
		{

			// Reset code for checking on stuck mobs
			LookingForTargetTime = 0;

			// Stop running just in case
			if(!wow.Player.IsSitting)
				wow.StopMovement(WoW_Movement.Forward);

			// Setup default action
			Action = BotAction.CheckAgro;
			ActionQueue.Clear();
			ActionQueue.Enqueue( BotAction.Rest);

			// If we're casting something, like a heal, wait till we're done
			if( wow.Player.IsCasting)
				return;

			// Do we really need to rest?
			if( wow.Player.HealthPercentage < profile.GetFloat( "RestBelowHealth"))
			{
				// Yes, set the rest flag
				// This will stay set until users full health conditions are met
				Resting = true;
			}

			// Check if we're eating and/or drinking
			bool eating = wow.Buffs[localization.GetString("food")] != null;
			bool bandaging = wow.Buffs[localization.GetString("First Aid")] != null;
			bool recentlyBandaged = wow.Buffs[localization.GetString("Recently Bandaged")] != null;

			// Is the RestUntilFullyRecovered bool set to true?
			if(profile.GetBool("RestUntilFullyRecovered"))
			{	//yes, was our health low enough that we are really needing to rest?
				if(Resting)
				{
					// Are we back to full health?
					if(wow.Player.HealthPercentage > 98)
					{
						// We are fully recovered, lets move on
						Action = BotAction.CheckAgro;
						ActionQueue.Clear();
						ActionQueue.Enqueue( BotAction.Loot);

						//check if we are too far from current waypoint
						PatrolArea patrolarea = GetPatrolArea();
						if( patrolarea.GetCurrentWaypoint().GetDistance() > profile.GetFloat( "WanderDistance"))
						{
							//wow.LogLine("Too far from current waypoint, returning");
							AddMessage("Too far from current waypoint, returning " + patrolarea.GetCurrentWaypoint().GetDistance().ToString());
							if (profile.GetBool( "Rogue.AllowWandering")) 
								CheckForTargets = true;
							else
								CheckForTargets = false;
							ActionQueue.Enqueue( BotAction.MoveToWaypoint);
						}

						// Check our buffs
						ActionQueue.Insert( 0, BotAction.CheckBuffs);
						ActionQueue.Enqueue( BotAction.FindTarget);

						//Done resting
						Resting = false;
					
						//Stand Up if sitting
						if(wow.Player.IsSitting) wow.TimedMovement(WoW_Movement.Forward,1);

						return;
					}
				}
				else
					//Our health didnt get low enough that we need to rest? Then we can move on
				{
					// Check our buffs
					ActionQueue.Clear();
					ActionQueue.Enqueue( BotAction.Loot);

					//check if we are too far from current waypoint
					PatrolArea patrolarea = GetPatrolArea();
					if( patrolarea.GetCurrentWaypoint().GetDistance() > profile.GetFloat( "WanderDistance"))
					{
						//wow.LogLine("Too far from current waypoint, returning");
						AddMessage("Too far from current waypoint, returning " + patrolarea.GetCurrentWaypoint().GetDistance().ToString());
						if (profile.GetBool( "Rogue.AllowWandering")) 
							CheckForTargets = true;
						else
							CheckForTargets = false;
						ActionQueue.Enqueue( BotAction.MoveToWaypoint);
					}

					ActionQueue.Insert( 0, BotAction.CheckBuffs);
					ActionQueue.Enqueue( BotAction.FindTarget);

					//Stand Up if sitting
					if(wow.Player.IsSitting) wow.TimedMovement(WoW_Movement.Forward,1);

					return;
				}
			}
				//else, "rest until fully recovered" is not set.
			else if( 
				// Are we healthy and not still eating
				(wow.Player.HealthPercentage >= profile.GetFloat( "RestBelowHealth") && !eating) ||
				(wow.Player.HealthPercentage >= profile.GetFloat( "RestBelowHealth") && eating && profile.GetBool("StopRestingWhenRested")) ||
				(wow.Player.HealthPercentage >= 98 && eating && profile.GetBool("StopRestingWhenFull")))
			{	
				// Yes, check our buffs
				Action = BotAction.CheckAgro;
				ActionQueue.Clear();
				ActionQueue.Enqueue(BotAction.Loot);
				//check if we are too far from current waypoint
				PatrolArea patrolarea = GetPatrolArea();
				if( patrolarea.GetCurrentWaypoint().GetDistance() > profile.GetFloat( "WanderDistance"))
				{
					//wow.LogLine("Too far from current waypoint, returning");
					AddMessage("Too far from current waypoint, returning " + patrolarea.GetCurrentWaypoint().GetDistance().ToString());
					if (profile.GetBool( "Rogue.AllowWandering")) 
						CheckForTargets = true;
					else
						CheckForTargets = false;
					ActionQueue.Enqueue( BotAction.MoveToWaypoint);
				}
				// Check our buffs
				ActionQueue.Insert( 0, BotAction.CheckBuffs);
				ActionQueue.Enqueue( BotAction.FindTarget);

				//Done resting
				Resting = false;

				//Stand Up if sitting
				if(wow.Player.IsSitting) wow.TimedMovement(WoW_Movement.Forward,1);

				return;
			}

			// Do we need to eat or bandage, and arent currently eating ?
			if( wow.Player.HealthPercentage < profile.GetFloat( "EatBelowHealth") && !eating)
			{

				// Check if the global cooldown is ready
				if( !cooldowns.IsReady( "Global"))
					return;

				// Make sure we are NOT eating
				eating = wow.Buffs[localization.GetString("food")] != null;
				if (eating == true) return;

				// Find the first food item
				foreach( int itemid in ListOfFood)
				{
					WoW_Object obj = wow.Inventory.GetItemById( itemid);
					if( obj == null)
						continue;

					// Eat
					wow.Inventory.UseItem( obj.Name);
					cooldowns.SetTime( "Global");

					AddMessage("Eating " + obj.Name);
					eating = true;
					return;
				}

				// Didn't find any food? Try bandaging
				if( !recentlyBandaged )
				{
					foreach( int itemid in ListOfBandages)
					{
						WoW_Object obj = wow.Inventory.GetItemById( itemid);
						if( obj == null)
							continue;

						if(wow.Player.IsSitting)
							wow.TimedMovement(WoW_Movement.Backward, 5);

						// Bandage
						wow.Inventory.UseItem( obj.Name);
						cooldowns.SetTime( "Global");

						wow.RightClick(wow.Player);

						AddMessage("Use " + obj.Name);
						bandaging = true;
						System.Threading.Thread.Sleep(500);

						return;
					}
				}			
			}

			// Are we eating or drinking?  Are we sitting? Make sure we dont sit down while waiting for a bandage to finish
			if( !bandaging) //&& !wow.Player.IsSitting
			{
				// Check if the global cooldown is ready
				if( !cooldowns.IsReady( "Global"))
					return;

				//wow.SendScript( "SitOrStand()");
				cooldowns.SetTime( "Global");

				// Are we a Night Elf? Are we not stealthed? If so we can hide using shadowmeld
				if( wow.Buffs[localization.GetString("stealth")] == null && 
					wow.Buffs[localization.GetString("Shadowmeld")] == null && 
					wow.HighestKnownSpells.ContainsKey(localization.GetString("Shadowmeld")) && 
					cooldowns.IsReady("Shadowmeld") &&
					profile.GetBool("RestShadowmeld")) 
				{ 
					AddMessage("Shadowmeld while resting");
					wow.CastSpellByName("Shadowmeld"); 
					cooldowns.SetTime("Shadowmeld"); 
					System.Threading.Thread.Sleep(300);
				} 
				else if( wow.Buffs[localization.GetString("stealth")] == null && 
					wow.Buffs[localization.GetString("Shadowmeld")] == null && 
					profile.GetBool("Rogue.UseStealth") && 
					cooldowns.IsReady("Stealth")) 
				{ 
					AddMessage("Stealth while resting");
					wow.CastSpellByName(localization.GetString("Stealth")); 
					cooldowns.SetTime("Stealth"); 
					System.Threading.Thread.Sleep(300);
				}
 
				// See if we should be sitting while resting
				Random RN = new Random();
				if (profile.GetBool("SitWhenResting") && !wow.Player.IsSitting)
				{
					if (profile.GetBool("SitWhenRestingRandom"))
					{
						if (RN.Next(1000) < 400) 
						{
							wow.SendScript( "SitOrStand()");
						}
					}
					else
					{
						wow.SendScript( "SitOrStand()");
					}
				}
				
				// Prevent Spamming of Resting on the log
				if (WeAreResting == false)
				{
                    AddMessage("Resting");
					WeAreResting = true;
                }
				return;
			}
		    // So check if we are stealthed, if not, check if we can stealth / have to stealth
			
			if( cooldowns.IsReady( "Stealth") && 
				!wow.Player.IsInCombat && 
				(wow.Buffs[localization.GetString("stealth")] == null) && 
				profile.GetBool("Rogue.UseStealth") && 
				(wow.Buffs[localization.GetString("first aid")] == null))
			{
				System.Threading.Thread.Sleep(1000);
				// Check again just to be safe after resting a second
				if( cooldowns.IsReady( "Stealth") && 
					!wow.Player.IsInCombat && 
					(wow.Buffs[localization.GetString("stealth")] == null) && 
					profile.GetBool("Rogue.UseStealth") && 
					(wow.Buffs[localization.GetString("first aid")] == null))
				{
					_hadstealth = true;
					cooldowns.SetTime( "Stealth");
					wow.CastSpellByName( localization.GetString("stealth"));
				}
			}
		}



		/// <summary>
		/// Apply a poison to a weapon
		/// </summary>
		/// <param name="slotname">Weaponslot name</param>
		/// <param name="poison">Poison name</param>
		/// <returns>True if applying</returns>
		private bool CheckAndApplyPoison( string slotname, string poisonname)
		{
			WoW_Object item = wow.Inventory.GetInventoryItem( slotname);
			if( item != null)
			{
				for( int i = 0; i < item.ItemBuffs.Count; i ++)
				{
					if( item.ItemBuffs[i].EnchantmentName.StartsWith(poisonname))
						return false;
					if( profile.GetString("Rogue.MainhandPoison") == "Mind-numbing Poison" && 
						item.ItemBuffs[i].EnchantmentName.StartsWith("Mind Numbing Poison"))
						return false;

				}

				for( int b = 0; b < wow.Inventory.Count; b ++)
					for( int i = 0; i < wow.Inventory[b].Count; i ++)
					{
						WoW_Object poison = wow.Inventory[b,i];
						if( poison == null || !poison.Name.StartsWith(poisonname))
							continue;

						wow.Inventory.UseItem( poison.Name);
						wow.Inventory.PickupInventoryItem( slotname);
						AddMessage("Applying Poison to Weapon");

						_castedpoison = true;
						return true;
					}
			}

			return false;
		}

		public override void DoCheckBuffs()
		{
			// Make sure we've stopped walking
			wow.StopMovement( WoW_Movement.Forward);

			// Setup default action
			Action = BotAction.CheckAgro;

			// Are we casting, or did we just start casting?
			if( wow.Player.IsCasting || !cooldowns.IsReady("Global"))
			{
				// Recheck buffs
				ActionQueue.Insert( 0, BotAction.CheckBuffs);
				return;
			}

			// Are we sitting?
			if( wow.Player.IsSitting)
			{
				wow.SendScript( localization.GetString("sitorstand()"));
				cooldowns.SetTime("Global");

				// Recheck buffs
				ActionQueue.Insert( 0, BotAction.CheckBuffs);
				return;
			}

			// Did we cast a poison? If so, set the global cooldown
			if( _castedpoison)
			{
				_castedpoison = false;
				cooldowns.SetTime("Global");

				// Recheck buffs
				ActionQueue.Insert( 0, BotAction.CheckBuffs);
				return;
			}

			// Do we have poisons ?
			if( wow.HighestKnownSpells.ContainsKey( localization.GetString("Poisons")))
			{
				if(profile.GetString("Rogue.MainhandPoison") != "Not Used")
				{
					if( CheckAndApplyPoison( "MainHandSlot", localization.GetString(profile.GetString("Rogue.MainhandPoison"))))
					{
						// Recheck buffs
						ActionQueue.Insert( 0, BotAction.CheckBuffs);
						return;
					}
				}

				if(profile.GetString("Rogue.OffhandPoison") != "Not Used")
				{
					if( CheckAndApplyPoison( "SecondaryHandSlot", localization.GetString(profile.GetString("Rogue.OffhandPoison"))))
					{
						// Recheck buffs
						ActionQueue.Insert( 0, BotAction.CheckBuffs);
						return;
					}
				}
			}
		}

		/// <summary>
		/// Handler for chat log parser
		/// </summary>
		/// <param name="line">Chat log line</param>
		public virtual void OnChatLogLine(WoW_ChatLogLine line)
		{
			Match matchExpression;
			try 
			{
				switch ((ChatIDs)line.Id)
				{
					case ChatIDs.SPELL_CREATURE_VS_CREATURE_DAMAGE:
					{
						matchExpression = Regex.Match(line.Message, localization.GetString("BeginsToCast"));
						if (matchExpression.Success)
							SuspectedCaster = true;
//							AddMessage("Chatlog Code - Suspected Caster");

						break;
					}

					case ChatIDs.COMBAT_CREATURE_VS_SELF_MISSES: 
					{
						string checkcombatparry = "parry";

						if( line.Message.ToLower().IndexOf( checkcombatparry.ToLower()) != -1 )
						{
							YouHaveParried = true;
//							AddMessage("Chatlog Code - You has Parried");
						}

						break;
					}

					case ChatIDs.MONSTER_EMOTE:
					{
						// Target attempts to run away in fear
						matchExpression = Regex.Match(line.Message, localization.GetString("CreatureRunsAway"));
						if (matchExpression.Success)
						{
//							AddMessage("Chatlog Code - TargetisFleeing");
						}

						break;
					}

					default:
						break;
				}
			}
			catch ( Exception E)
			{
				wow.LogLine("Exception in OnChatLogLine: " + E.Message);
				AddMessage("Exception in OnChatLogLine" + E.Message);
			}
		}

		/// <summary>
		/// Check if we have to flee, might need a change for some classes
		/// </summary>
		public override void DoCheckFlee()
		{
			// Are we under attack
			if (wow.NumberOfAttackers == 0)
			{
				// Next Action
				Action = ActionQueue.Dequeue();
				return;
			}
			
			// Do we need to drink a heal potion? 
			if( wow.Player.HealthPercentage < profile.GetInteger("FleeBelowHealth") && 
				profile.GetBool("FleeUsePotions") &&
				cooldowns.IsReady( "Potion"))
			{ 
				// Check if the global cooldown is ready 
				if( !cooldowns.IsReady( "Global")) 
					return; 

				// Find the first heal potions 
				foreach( int itemid in ListOfHealPotions) 
				{ 
					WoW_Object obj = wow.Inventory.GetItemById( itemid); 
					if( obj == null) 
						continue; 

					// Quaff Potion 
					wow.Inventory.UseItem( obj.Name); 
					cooldowns.SetTime( "Global"); 
					cooldowns.SetTime( "Potion");

					AddMessage("Quaffing " + obj.Name); 
					System.Threading.Thread.Sleep(500);
					wow.Player.Update();
					return; 
				} 
			} 
		
			// Less then specified hp, and more then 1 attacker?
			int FleeMod = (profile.GetInteger("Rogue.FleeAddModifier"));
			if( wow.Player.HealthPercentage < (profile.GetInteger("FleeBelowHealth") + ((wow.NumberOfAttackers - 1) * FleeMod)))
			{
				// Yes? RUN FOREST RUN !
				Action = BotAction.Flee;

				ActionQueue.Clear();
				ActionQueue.Enqueue( BotAction.Rest);
				return;
			}

			// Less then specified hp
			if( wow.Player.HealthPercentage < profile.GetInteger("FleeBelowHealth"))
			{
				// Our attacker has more HP then us?
				if(wow.Target != null && wow.Player.HealthPercentage < wow.Target.HealthPercentage)
				{
					// Yes? RUN FOREST RUN !
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
		/// 
		/// Flee Code for Rogues
		/// 
		/// </summary>
		public override void DoFlee()
		{	
			// If we're dead, we cant do much either
			if( wow.Player.IsDead || wow.Buffs[ localization.GetString("ghost")] != null)
			{
				Action = BotAction.CheckAgro;
				return;
			}
			
			if( wow.HighestKnownSpells.Contains(localization.GetString("preparation")) && 
				cooldowns.IsReady("Preparation") && 
				!cooldowns.IsReady( "Vanish") && 
				wow.HighestKnownSpells.Contains (localization.GetString("vanish")) &&
				wow.Buffs[localization.GetString("Stealth")] != null &&
				profile.GetBool("Rogue.FleeUseVanish")) 
			{ 
				wow.CastSpellByName(localization.GetString("preparation")); 
				AddMessage("Using Preparation to reset Vanish"); 
				cooldowns.SetTime("Preparation"); 
				cooldowns.ClearTime("Vanish");
				System.Threading.Thread.Sleep(300); 
			}

			// Use Evasion if it is up
			if( cooldowns.IsReady( "Evasion") && 
				profile.GetBool("Rogue.FleeUseEvasion") &&
				wow.Buffs[localization.GetString("Evasion")] == null)
			{
				wow.CastSpellByName( localization.GetString("evasion"));
				cooldowns.SetTime( "Evasion");
				System.Threading.Thread.Sleep(500);
			}
			
			// Use Blind and Run
			if( cooldowns.IsReady( "Blind") && profile.GetBool("Rogue.FleeUseBlindAndRun"))
			{
				do
				{
					System.Threading.Thread.Sleep(500); 
				}while (wow.Player.Energy < 30);
				wow.CastSpellByName( localization.GetString("blind"));
				cooldowns.SetTime( "Blind");
				System.Threading.Thread.Sleep(500);
			}

			// Use Blind and Bandage
			if( cooldowns.IsReady( "Blind") && profile.GetBool("Rogue.FleeUseBlindAndBandage") && wow.NumberOfAttackers < 2)
			{
				do
				{
					System.Threading.Thread.Sleep(500); 
				}while (wow.Player.Energy < 30);
				wow.CastSpellByName( localization.GetString("blind"));
				cooldowns.SetTime( "Blind");
				System.Threading.Thread.Sleep(500);
				
				foreach( int itemid in ListOfBandages)
				{
					WoW_Object obj = wow.Inventory.GetItemById( itemid);
					if( obj == null)
						continue;

					// Bandage
					wow.Inventory.UseItem( obj.Name);
					cooldowns.SetTime( "Global");

					wow.RightClick(wow.Player);

					AddMessage("Use " + obj.Name);
					bool bandaging = true;
					do
					{
						System.Threading.Thread.Sleep(500);
						bandaging = wow.Buffs[localization.GetString("First Aid")] != null;
					}
					while ( bandaging != false);
					Action = BotAction.FindTarget;
					return;
				}
			}
			
			// Gouge and Run
			if( cooldowns.IsReady( "Gouge") && profile.GetBool("Rogue.FleeUseGouge"))
			{
				do
				{
					System.Threading.Thread.Sleep(500); 
				}while (wow.Player.Energy < 45);
				wow.CastSpellByName( localization.GetString("gouge"));
				cooldowns.SetTime( "Gouge");
				System.Threading.Thread.Sleep(500);
			}
			
			// Use Sprint if it is up
			if( cooldowns.IsReady( "Sprint") && 
				profile.GetBool("Rogue.FleeUseSprint") &&
				wow.Buffs[localization.GetString("Sprint")] == null)
			{
				wow.CastSpellByName( localization.GetString("sprint"));
				cooldowns.SetTime( "Sprint");
				System.Threading.Thread.Sleep(1000);
			}

			// Use Vanish now if it is up
			if( cooldowns.IsReady( "Vanish") && 
				profile.GetBool("Rogue.FleeUseVanish"))
			{
				AddMessage("Emergency Vanish");
				wow.CastSpellByName( localization.GetString("vanish"));
				cooldowns.SetTime( "Vanish");
				System.Threading.Thread.Sleep(500);
			}

			//dont keep kicking and reset flags.
			SuspectedCaster = false;
			YouHaveParried = false;
			
			//
			// run away code here
			// Reset target
			if( wow.Target != null)
				wow.ClearTarget();

			PatrolArea patrolarea = GetPatrolArea();

			// Did all attackers give up ?
			if( wow.AttackersByDistance.Count == 0 && 
				!wow.Player.IsInCombat && 
				wow.Buffs[localization.GetString("Vanish")] == null)
			{
				// Yes, stop moving
				wow.StopMovement( WoW_Movement.Forward);

				// Start the next action
				
				Action = BotAction.CheckAgro;
				ActionQueue.Clear();
				ActionQueue.Enqueue( BotAction.Rest);
				ActionQueue.Enqueue( BotAction.RangedFight);
				
				
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

			// Run Away!
			wow.StartMovement( WoW_Movement.Forward);

			if( profile.GetBool( "FleeFaceAway") )
			{	// Attackers can be 0 but player still in combat
				if( wow.AttackersByDistance.Count == 0)
					return;

				WoW_Object obj = ((WoW_Distance)wow.AttackersByDistance[0]).Obj;
				wow.FaceAway( obj.X, obj.Y);
			}
			else
			{
				// if we are just starting to run away, change direction so that
				// we will run back through the area we just cleared
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
				wpt.MoveTo(5,false,false);
				
				//and then come here again to check if we need to keep running
				Action = BotAction.Flee;

			}
		}

		public override void DoMeleeFight()
		{
			//AddMessage("Entering DoMeleeFight");
			ActionQueue.Clear();
			ActionQueue.Enqueue( BotAction.Loot);
			if( wow.Target == null || wow.Target.IsDead)
			{
				wow.ClearTarget();

				// Target is lost, check for agro
				Action = BotAction.CheckAgro;

				//dont keep kicking and reset mob flags
				SuspectedCaster = false;
				YouHaveParried = false;
				FoundTarget = false;
				WeAreResting = false;

				// We have probably killed our target, check for Loot
				ActionQueue.Clear();
				ActionQueue.Enqueue( BotAction.Loot);
				ActionQueue.Enqueue( BotAction.Rest);

				//make people amused and impressed
				AddMessage("==> Number Killed " + (++NumberKilled).ToString() + " <==");

				//Reset our first combo
				FirstComboMove = true;
				// Reset Range count if it was used
				NoOfRangePull = 0;

				//make loot work better, if no attackers wait for loot to be ready
				if(wow.AttackersByDistance.Count < 2) //stil showing the one attacker probably
				//	Thread.Sleep(1500);

				// Set loot timeout
				cooldowns.SetTime( "LootFlag");

				// AddMessage("Looting");
				return;
			}

			// Reset Range count if it was used

			// Setup default action
			Action = BotAction.CheckFlee;
			ActionQueue.Clear();
			ActionQueue.Enqueue( BotAction.MeleeFight);

			// Calculate the distance to the target
			float TargetDistance = wow.Player.GetDistance( wow.Target);

			// Has the monster run away?
			if( wow.Target != null && TargetDistance > profile.GetFloat("MaximumMeleeAttackDistance"))
			{
				// Yes, follow it
				if (profile.GetBool("Rogue.WaitRanged") == false || 
					wow.Target.HealthPercentage <= 30 || 
					profile.GetString("Rogue.StealthAttack") != "Not Used" ||
					HaveAmmo == false || NoOfRangePull != profile.GetInteger("Rogue.NoOfRangePulls")) 
				{
					Action = BotAction.GetInRange;
					ActionQueue.Clear();
					ActionQueue.Enqueue( BotAction.MeleeFight);
				}
			}

			// I SO hate these messages
			string RedMessage = wow.GetRedMessage();
			if( wow.Target != null && RedMessage.StartsWith(localization.GetString("target needs to be in front of you")))
			{	// Face the target
				wow.Face(wow.Target);
				// And move back a little
				wow.TimedMovement(WoW_Movement.Backward, 200);
				return;
			}
			// code to walk a bit closer if out of range
			 if(wow.Target != null && ( RedMessage.StartsWith(localization.GetString("out of range.")) || RedMessage == localization.GetString("you are too far away")))
			{
				LookingForTargetTime = LookingForTargetTime + 1;
				wow.TimedMovement( WoW_Movement.Forward, 350 + rnd.Next( 500));
				 // Check if we have LOS or cant get to the damned mob
				 if( wow.GetRedMessage().StartsWith(localization.GetString("lineofsight")) || LookingForTargetTime >= 5)
				 {
					 IgnoreThis( wow.Target);

					 Action = BotAction.CheckAgro;

					 ActionQueue.Clear();
					 ActionQueue.Enqueue( BotAction.FindTarget);
					 LookingForTargetTime = 0;
					 return;
				 }

				return;
			}

			// Make sure we are facing the target
			//AddMessage(string.Format("Facing Target in melee"));
			wow.Face( wow.Target);
			//AddMessage(string.Format("Done Facing Target in melee"));

			// We're 'casting', wait for it to finish
			if( wow.Target == null || wow.Player.IsCasting || !cooldowns.IsReady("Global"))
				return;

			//If we are doing StealthAttack run new Routine to Stealth near the mob
			if( profile.GetString("Rogue.StealthAttack") != "Not Used" && 
				wow.Buffs[localization.GetString("stealth")] != null )
			{
				//AddMessage(string.Format("Going into Stealth Attack Mode"));
				DoStealthAttack();
			}

			// We're not in melee attack mode
			if( !wow.Player.IsAttacking && wow.Buffs[localization.GetString("stealth")] == null)
			{
				wow.CastSpellByName( localization.GetString("attack"));
				cooldowns.SetTime("Global");
				return;
			}

			//Do we have Blade flurry and more than 1 mob?
			if (cooldowns.IsReady("Blade Flurry") &&
				wow.NumberOfAttackers > 1 && 
				profile.GetBool("Rogue.UseBladeFlurryOnAdds") &&
				cooldowns.IsReady("Global"))
			{
				if (wow.Player.Energy < 25)
				{
					do 
					{
						wow.Player.Update();
						System.Threading.Thread.Sleep(500);
					}while (wow.Player.Energy < 25);
				}
				//This should kick in Blade Flurry if we have more than 1 attacker
				wow.CastSpellByName( localization.GetString("Blade Flurry"));
				cooldowns.SetTime("Blade Flurry");
				cooldowns.SetTime("Global");
				AddMessage(string.Format("Using Blade Flurry"));
				return;
			}

			//Do we have Blade flurry?
			if  (cooldowns.IsReady("Blade Flurry") &&
				profile.GetBool("Rogue.UseBladeFlurry") &&
				!profile.GetBool("Rogue.UseBladeFlurryOnAdds") &&
				cooldowns.IsReady("Global"))
			{
				if (wow.Player.Energy < 25)
				{
					do 
					{
						wow.Player.Update();
						System.Threading.Thread.Sleep(500);
					}while (wow.Player.Energy < 25);
				}
				//This should kick in Blade Flurry
				wow.CastSpellByName( localization.GetString("Blade Flurry"));
				cooldowns.SetTime("Blade Flurry");
				cooldowns.SetTime("Global");
				AddMessage(string.Format("Using Blade Flurry"));
				return;
			}

			// Is this our first melee and do we think this is a caster?
			if(SuspectedCaster && 
				wow.HighestKnownSpells.Contains( localization.GetString("kick")) &&
				cooldowns.IsReady("Kick") &&
				cooldowns.IsReady("Global") &&
				wow.Player.GetDistance(wow.Target) <= 5)
			{

				// Kick the caster
				wow.CastSpellByName( localization.GetString("kick"));
				cooldowns.SetTime("Global");
				cooldowns.SetTime("Kick");
				SuspectedCaster = false;
				AddMessage(string.Format("Do Kick"));
				return;
			}

			// Did we parry? Can we do Riposte?
			if( wow.HighestKnownSpells.Contains( localization.GetString("riposte")) && 
				cooldowns.IsReady("Riposte") && YouHaveParried && 
				wow.Player.GetDistance(wow.Target) <= 5 && profile.GetBool("Rogue.UseRiposte") &&
				cooldowns.IsReady("Global"))
			{
				while( !cooldowns.IsReady("Global") || wow.Player.Energy < 10 ) System.Threading.Thread.Sleep(100);

				wow.CastSpellByName( localization.GetString("riposte"));
				cooldowns.SetTime("Global");
				cooldowns.SetTime("Riposte");
				YouHaveParried = false;
				AddMessage(string.Format("You Parried - Trying to Riposte"));
				return;
			}

			// Do we have evasion?
			if( wow.HighestKnownSpells.Contains( localization.GetString("evasion")) && 
				!profile.GetBool("Rogue.FleeUseEvasion"))
			{
				bool DoEvasion = false;

				// Less then 80% hp and more then 1 attack? Do evasion
				if( wow.Player.HealthPercentage < 80 && wow.NumberOfAttackers > 1)
					DoEvasion = true;

				// Less then 50% hp ?
				if( wow.Player.HealthPercentage < 40 && 
					wow.Target.HealthPercentage > wow.Player.HealthPercentage)
					DoEvasion = true;

				// Do evasion and cooldown has passed?
				if( cooldowns.IsReady( localization.GetString("evasion")) && DoEvasion)
				{
					wow.CastSpellByName( localization.GetString("evasion"));
					cooldowns.SetTime( "Evasion");
					AddMessage("Evasion");
				}
			}

			// Do we have Ghostly Strike, is it checked, and do we need it?
			if( cooldowns.IsReady( localization.GetString("ghostly strike")) &&
				wow.Player.Energy >= 50 &&
				profile.GetBool("Rogue.UseGhostlyStrike"))
			{
				bool DoStrike = false;

				// Less then 80% hp and more then 1 attack? Do strike
				if( wow.Player.HealthPercentage < 80 && wow.NumberOfAttackers > 1)
					DoStrike = true;

				// Less then the mobs HP and less than 40%?
				if( wow.Player.HealthPercentage < 40 && 
					wow.Target.HealthPercentage > wow.Player.HealthPercentage)
					DoStrike = true;

				// Do evasion and cooldown has passed?
				if( cooldowns.IsReady( localization.GetString("ghostly strike")) && DoStrike)
				{
					wow.CastSpellByName( localization.GetString("ghostly strike"));
					cooldowns.SetTime( "Ghostly Strike");
					AddMessage("Ghostly Strike");
				}
			}		
			
			// Do Adrenaline Rush Code
			if( cooldowns.IsReady( localization.GetString("adrenaline rush")) &&
				wow.Player.Energy < profile.GetInteger("Rogue.UseAdrenalineRushPercent") &&
				profile.GetBool("Rogue.UseAdrenalineRush"))
			{
				wow.CastSpellByName( localization.GetString("adrenaline rush"));
				cooldowns.SetTime( "Adrenaline Rush");
				AddMessage("Use - Adrenaline Rush");
			}
			
			//
			// Should we do our finishing move?
			// Get values from the profile depending on whether this is the first combo attack
			int FinishingMoveAt = profile.GetInteger("Rogue.NormComboPts");

			if (FirstComboMove)
			{
				FinishingMoveAt = profile.GetInteger("Rogue.OpenComboPts");
			}

			//Checks to see if we should finish off the mob early. checks to make sure
			//combo points dont go below 2

			if (FinishingMoveAt >= 3)
			{
				if( wow.Target.HealthPercentage < 45)
					FinishingMoveAt --;
			}
			if (FinishingMoveAt >= 3)
			{
				if( wow.Target.HealthPercentage < 30)
					FinishingMoveAt --;
			}

			bool DoFinishingMove = wow.Player.ComboPoints >= FinishingMoveAt;

			// Check how many attackers, and if we can gouge one
			if( wow.HighestKnownSpells.ContainsKey(localization.GetString("gouge")) && 
				cooldowns.IsReady("Gouge") && wow.NumberOfAttackers > 1 &&
				profile.GetBool("Rogue.UseGouge") &&
				wow.Target.HealthPercentage > 30)
			{
				// Check if we have a 2nd target close by to Gouge
				WoW_Object GougeTarget = null;
				foreach( WoW_Distance dist in wow.AttackersByDistance)
				{
					if( wow.Target != null && dist.Distance < profile.GetFloat("MaximumMeleeAttackDistance") && dist.Obj != wow.Target)
					{
						GougeTarget = dist.Obj;
						break;
					}
				}

				// We have combopoints, use them before Gouge
				if( GougeTarget != null && wow.Player.ComboPoints > 1)
					DoFinishingMove = true;

				// We used up our combo points and we have enough Energy to Gouge
				if( wow.Target != null && GougeTarget != null && wow.Player.Energy >= 45 && 
					wow.Player.ComboPoints <= 1 && !(DoFinishingMove))
				{
					// We do not want to lose our combo points due to Gouge
					//wow.LogLine( "Found {0} attackers, Gouge is up, lets Gouge the add", wow.NumberOfAttackers);
					//wow.LogLine( "Gouge target {0}", GougeTarget.Name);
					AddMessage(string.Format("Found {0} attackers, Gouge is up, lets Gouge the add", wow.NumberOfAttackers));

					// Save our current target
					WoW_Object currentTarget = wow.Target;

					// Select the monster
					wow.LeftClick( GougeTarget);

					// Wait for the target change, max 3s
					for( int w = 0; w < 3; w ++)
					{
						if( wow.Target == GougeTarget)
							break;

						System.Threading.Thread.Sleep(100);
					}

					// Did we succeed in switching targets?
					if( wow.Target == GougeTarget)
					{
						// Face it
						wow.Face( GougeTarget.X, GougeTarget.Y);

						// Gouge it
						wow.CastSpellByName( localization.GetString("gouge"));
						System.Threading.Thread.Sleep(1000 + rnd.Next(250));

						// Back to our last target
						wow.LeftClick( currentTarget);

						// Wait for the target change, max 3s
						for( int w = 0; w < 3; w ++)
						{
							if( wow.Target == currentTarget)
								break;

							System.Threading.Thread.Sleep(100);
						}

						wow.Face( wow.Target);

						cooldowns.SetTime("Gouge");
						cooldowns.SetTime("Global");
						return;
					}
				}
			}

			// Should we use our finishing move?
			if( DoFinishingMove)
			{
				string ComboMove = (localization.GetString(profile.GetString("Rogue.NormCombo")));

				//Check to see if first move this fight.
				if (FirstComboMove)
				{
					ComboMove = (localization.GetString(profile.GetString("Rogue.OpenCombo")));
				}

				// Wait till we have enough energy for Combo Move
				int ComboEnergy = 25;

				if (ComboMove == localization.GetString("eviscerate"))
				{
					ComboEnergy = 35;
				}
				
				if( wow.Player.Energy >= ComboEnergy)
				{
					if(	cooldowns.IsReady("Cold Blood") && 
						wow.Target.HealthPercentage >= 45 && 
						wow.Player.ComboPoints >= 4 &&
						profile.GetBool("Rogue.UseColdBlood") &&
						ComboMove == localization.GetString("eviscerate")) 
					{ 
						wow.CastSpellByName(localization.GetString("coldblood")); 
						AddMessage("Using Cold Blood"); 
						cooldowns.SetTime("Cold Blood");
						cooldowns.SetTime("Global");
						System.Threading.Thread.Sleep(300); 
					}

					if (wow.Buffs[localization.GetString("cold blood")] != null) ComboMove = localization.GetString("eviscerate");
					wow.CastSpellByName( ComboMove);
					FirstComboMove = false; // Set the variable after it has completed the move
					cooldowns.SetTime("Global");
					AddMessage(string.Format("Do ") + ComboMove);
				}

				return;
			}

			// Do Final check to make sure we are in range
			if (wow.Target != null && TargetDistance > profile.GetFloat("MaximumMeleeAttackDistance"))
			{
				Action = BotAction.GetInRange;
			}
			// Do Sinister Strike attack
			else if( wow.Player.Energy >= SSEnergy && cooldowns.IsReady("Global") && wow.Buffs[localization.GetString("cold blood")] == null)
			{
				wow.CastSpellByName( localization.GetString("sinister strike"));
				cooldowns.SetTime("Global");
				AddMessage(string.Format("Do Sinister Strike with Energy - ") + wow.Player.Energy);
				return;
			}
		}

		private float _lastdistance = 0;
		public override void DoRangedFight()
		{
			// Check if we still have a target
			
			// Wait for monster to come into range if WaitRanged is checked
					
			if (HaveAmmo == true) CheckAmmo(wow.GetRedMessage());
			if (HaveAmmo == false)
			{
				ActionQueue.Clear();
				Action = BotAction.GetInRange;
				ActionQueue.Enqueue( BotAction.MeleeFight);
				return;
			}
			if( wow.Target == null || wow.Target.IsDead)
			{
				wow.ClearTarget();

				// Target is lost, check for agro
				Action = BotAction.CheckAgro;

				//dont keep kicking
				SuspectedCaster = false;
				YouHaveParried = false;

				// We have probably killed our target, check for Loot
				NoOfRangePull = 0;
				ActionQueue.Clear();
				// if (wow.Player.HealthPercentage >= profile.GetFloat( "RestBelowHealth")) 
				ActionQueue.Enqueue( BotAction.Loot);

				//make people amused and impressed
				AddMessage("==> Number Killed " + (++NumberKilled).ToString() + " <==");

				//Reset our first combo
				FirstComboMove = true;
				// Reset Range count if it was used

				//make loot work better, if no attackers wait for loot to be ready
				if(wow.AttackersByDistance.Count < 2) //stil showing the one attacker probably
			    // Thread.Sleep(1500);

				// Set loot timeout
				cooldowns.SetTime( "LootFlag");

				// AddMessage("Looting");
				Action = BotAction.CheckAgro;
				return;
			}

			// Dont do any checks when casting, we might have cast but monster is moving away
			// doing other tests might make the bot run after the monster (not intended)
			if( wow.Player.IsCasting || !cooldowns.IsReady("Global"))
				return;

			//If we are doing StealthAttack run new Routine
			//AddMessage(string.Format("Doing Stealth for Attack Routine"));
			if( profile.GetString("Rogue.StealthAttack") != "Not Used" )
			{
				AddMessage(string.Format("Inside Stealth Routine"));
				StealthforAttack();
			}

			// Have we already pulled? if so switch to melee
			// Does Profile want to use range at all?

			if( NoOfRangePull >= profile.GetInteger("Rogue.NoOfRangePulls") || 
				profile.GetString("Rogue.PullWith") == "Not Used" || profile.GetString("Rogue.StealthAttack") != "Not Used")
			{	// Ranged is disabled
				if(wow.Target.Target != wow.Player)
				{
					// Action = BotAction.CheckAgro;
					// And Fight
					ActionQueue.Clear();
					Action = BotAction.GetInRange;
					ActionQueue.Enqueue( BotAction.MeleeFight);
					return;
				}
				else
				{
					// And Fight
					ActionQueue.Clear();
					Action = BotAction.GetInRange;
					ActionQueue.Enqueue( BotAction.MeleeFight);
					return;
				}
			}

			// Did we run out of throw weapons?
			if( wow.Inventory.GetInventoryItem("RangedSlot") == null &&	
				profile.GetString("Rogue.PullWith") == "Throw")
			{
				// Find the first throwing item
				foreach( int itemid in ListOfThrowing)
				{
					WoW_Object obj = wow.Inventory.GetItemById( itemid);
					if( obj == null)
						continue;

					// Using the weapon will auto-equipt it
					wow.Inventory.UseItem( obj.Name);
					cooldowns.SetTime( "Global");

					AddMessage("Equiping " + obj.Name);
					return;
				}
			}

			// Check to see if we have ammo and weapon equipped and set the HaveAmmo variable
			if (HaveAmmo == true) CheckAmmo(wow.GetRedMessage());

			// Check if we have LOS
			if( wow.GetRedMessage().StartsWith(localization.GetString("lineofsight")))
			{
				IgnoreThis( wow.Target);

				Action = BotAction.CheckAgro;

				ActionQueue.Clear();
				ActionQueue.Enqueue( BotAction.FindTarget);
				return;
			}

			// Calculate the distance to the target
			float TargetDistance = wow.Player.GetDistance( wow.Target);

			// Check if we still have a target
			if( wow.Target == null)
			{
				// Target is lost, check for agro
				Action = BotAction.CheckAgro;

				// And find a new target
				ActionQueue.Clear();
				ActionQueue.Enqueue( BotAction.FindTarget);
				return;
			}

			// Out of distance?
			if( TargetDistance > profile.GetFloat("MaximumRangedAttackDistance"))
			{
				// Get in range again
				Action = BotAction.GetInRange;

				// And find a new target
				ActionQueue.Clear();
				ActionQueue.Enqueue( BotAction.FindTarget);
				return;
			}

			// In melee range
			if( TargetDistance < profile.GetFloat("MinimumRangedAttackDistance"))
			{
				// Make sure we're close enough to actually melee
				Action = BotAction.GetInRange;

				// And Fight
				ActionQueue.Clear();
				ActionQueue.Enqueue( BotAction.MeleeFight);
				return;
			}

			// Check if we still have a target
			if( wow.Target != null)
			{
				// Face the target
				wow.Face( wow.Target.X, wow.Target.Y);

				// Setup default action
				Action = BotAction.CheckFlee;
				ActionQueue.Clear();
				ActionQueue.Enqueue( BotAction.RangedFight);
			}

			// Do a caster check
			 if( TargetDistance == _lastdistance)
			{
				if( cooldowns.IsReady("CasterDetection") && cooldowns.IsReady("RangePull"))
				{	// Spot the caster :)
					// Since we're not good at range, run to him to kill him
					Action = BotAction.GetInRange;

					//Set global variable so we can Kick
					SuspectedCaster = true;

					// And Fight
					ActionQueue.Clear();
					ActionQueue.Enqueue( BotAction.MeleeFight);
					AddMessage(string.Format("Found Caster? Run to it"));

					return;
				}
			}
			else
			{	// Reset caster check
				_lastdistance = TargetDistance;
				cooldowns.SetTime("CasterDetection");
				SuspectedCaster = false;
			}

			// Check if we actually have the RangePull spell
			if( !wow.HighestKnownSpells.ContainsKey(localization.GetString(profile.GetString("Rogue.PullWith"))))
			{
				// Make sure we're close enough to actually melee
				Action = BotAction.GetInRange;

				// And Fight
				ActionQueue.Clear();
				ActionQueue.Enqueue( BotAction.MeleeFight);

				return;
			}

			// Check if we can actually throw - Check for Pull Once Bool
			string rangepull = (localization.GetString(profile.GetString("Rogue.PullWith")));
			if( cooldowns.IsReady("RangePull") && 
				TargetDistance > (profile.GetFloat("MinimumRangedAttackDistance") + 5) &&
				NoOfRangePull < profile.GetInteger("Rogue.NoOfRangePulls") && 
				localization.GetString(profile.GetString("Rogue.PullWith")) != "Not Used" && HaveAmmo == true)
			{
				string RdMessage = "";
				string RM = "";
				AddMessage(string.Format("Attempting to " + rangepull));
				// if (!wow.Player.IsAttacking) wow.CastSpellByName( localization.GetString("attack"));
				wow.CastSpellByName(rangepull);
				System.Threading.Thread.Sleep(500);
				RM = wow.GetRedMessage();
				if( RM == localization.GetString("Invalid target"))
				{
					IgnoreThis( wow.Target);
					AddMessage(string.Format("Bad Target"));

					Action = BotAction.CheckAgro;

					ActionQueue.Clear();
					ActionQueue.Enqueue( BotAction.FindTarget);
					return;
				}
				if (HaveAmmo == true) CheckAmmo(RM);
				if (HaveAmmo == false) 
				{
					AddMessage(string.Format("Out of Ammo for " + rangepull));
					ActionQueue.Clear();
					ActionQueue.Enqueue( BotAction.MeleeFight);
					DoMeleeFight();
				}
				// Check if we have LOS, if not go to another mob
				if( RM.StartsWith(localization.GetString("lineofsight")))
				{
					IgnoreThis( wow.Target);
					AddMessage(string.Format("Dont Have Line of Sight. Skipping Mob"));

					Action = BotAction.CheckAgro;

					ActionQueue.Clear();
					ActionQueue.Enqueue( BotAction.FindTarget);
					return;
				}
				// Make sure we didnt fail 
				RdMessage = RM;
				if (RdMessage.StartsWith(localization.GetString("another action is in progress"))  || 
					RdMessage.StartsWith(localization.GetString("you have no target.")) ||
					RdMessage.StartsWith(localization.GetString("out of range.")) ||
					RdMessage.StartsWith(localization.GetString("you are too far away")) || 
					RdMessage.StartsWith(localization.GetString("cant do that while moving"))) 
				{
					AddMessage(string.Format("Problem when " + rangepull));
					RM = "";
					System.Threading.Thread.Sleep(500);
					return;
				}
				else
				{
					//Add one to the number of pulls completed
					NoOfRangePull ++;

					//Set Cooldowns
					cooldowns.SetTime("RangePull");
					cooldowns.SetTime("Global");
					System.Threading.Thread.Sleep(500);
				}
			}
			else 
				if (HaveAmmo == false) 
			{
				AddMessage(string.Format("Problem with Ranged Attack, Check Ammo and Weapon for " + rangepull));
				ActionQueue.Clear();
				ActionQueue.Enqueue( BotAction.MeleeFight);
				DoMeleeFight();
			}
				ActionQueue.Clear();
				int THP = wow.Player.Health;
				int TCHP = THP;
				int THPCycles = 0;
				if (profile.GetBool("Rogue.WaitRanged") == true)
				{
					AddMessage(string.Format("Waiting 10s for Mob to come to us"));
					ActionQueue.Clear();
					ActionQueue.Enqueue( BotAction.MeleeFight);
					do 
					{
						wow.Target.Update();
						wow.Player.Update();
						System.Threading.Thread.Sleep(100);
						TCHP = wow.Player.Health;
						if (TCHP < THP) break;
						wow.Face(wow.Target);
						THPCycles++;
						if (THPCycles >= 100) 
						{
							// Check if we have LOS, if not go to another mob
							if( wow.GetRedMessage().StartsWith(localization.GetString("lineofsight")))
							{
								IgnoreThis( wow.Target);

								Action = BotAction.CheckAgro;

								ActionQueue.Clear();
								ActionQueue.Enqueue( BotAction.FindTarget);
								return;
							}
							// The mob isnt coming to us or we arent in melee range yet, so get to mob and start fighting.
							AddMessage(string.Format("Waited too long for Mob so going to Mob"));
						
							string GarbageString = wow.GetRedMessage();
							ActionQueue.Clear();
							ActionQueue.Enqueue( BotAction.MeleeFight);
							break;
						}
					}
					while (wow.Player.GetDistance(wow.Target) > 5);
					string GarbageString2 = wow.GetRedMessage();
					AddMessage(string.Format("Attacking Mob"));
					ActionQueue.Enqueue( BotAction.MeleeFight);
					System.Threading.Thread.Sleep(200);
				}
				else
				{			
					ActionQueue.Enqueue( BotAction.RangedFight);
					AddMessage(string.Format("Not using Wait Ranged so head to Melee"));
				}
	}


		/// Returns the durability of an inventory item
		/// </summary>
		/// <param name="slotname">Slotname</param>
		/// <returns>-1 for invalid item, 0-n remaining durability</returns>
		private int GetDurability ( string slotname )
		{
			WoW_Object item = wow.Inventory.GetInventoryItem ( slotname );

			if ( item == null ) 
			{
				return -1;
			}

			int Durability = item.ReadStorageInt ( wow.Descriptors[item.Type, "ITEM_FIELD_DURABILITY"] );
			return Durability;
		}

		///
		/// Check for items that we looted that should be deleted that are in our inventory
		///
		public virtual void CleanupInventory()
		{
			// check if we looted any crap
			if(ListOfCrapLoot.Count > 0 && ListOfCrapLoot.Contains(""))
			{
				ListOfCrapLoot.Remove("");
			}
			
			if(ListOfCrapLoot.Count > 0 && !ListOfCrapLoot.Contains(""))
			{
				AddMessage("Searching for crap loot");

				// search through all bags and slots
				for(int m = 0; m < wow.Inventory.Count; m++)
				{
					for( int n = 0; n < wow.Inventory[m].Count; n++)
					{
						WoW_Object obj = wow.Inventory[m,n];
						// check for matches
						if(obj != null && ListOfCrapLoot.Contains(obj.Name))
						{
							// send the UI script to delete crap loot
							wow.SendScript("PickupContainerItem("+m+", "+(n+1)+");");
							wow.SendScript("DeleteCursorItem();");
							AddMessage( "Deleted: " + obj.Name);
							ListOfCrapLoot.Remove(obj.Name);
							if (ListOfCrapLoot.Count == 0) break;
						}
					}
				} 
				//ListOfCrapLoot = new ArrayList();
			} 
			else if(ListOfCrapLoot.Count > 0 && ListOfCrapLoot.Contains(""))
			{
				ListOfCrapLoot.Remove("");
			}
		}

		/// <summary>
		/// Check for out of ammo or need weapon strings and update HaveAmmo
		/// </summary>
		public virtual void CheckAmmo( string RM)
		{
			string RdMessage = RM;
			if ( (GetDurability("RangedSlot") == -1 && profile.GetString("Rogue.PullWith") != "Throw" && profile.GetString("Rogue.PullWith") == "Not Used") || 
				RdMessage.StartsWith(localization.GetString("out of ammo"))  || 
				RdMessage.StartsWith(localization.GetString("ammo needs to be")) ||
				RdMessage.StartsWith(localization.GetString("must have a gun equipped")) ||
				RdMessage.StartsWith(localization.GetString("must have a bow equipped")) ||
				RdMessage.StartsWith(localization.GetString("must have a crossbow equipped")) ||
				RdMessage.StartsWith(localization.GetString("must have a thrown equipped")) ) HaveAmmo = false;		
				// Check actual
			
		}

		/// <summary>
		/// Loot all close by objects
		/// </summary>
		public override void DoLoot()
		{
			
			//AddMessage("Entering DoLoot");
			// Do we even want to loot?
			int bagSpace = 0;
			int bagUsed = wow.Inventory.GetAllItems().Count;
			for(int i = 0; i <= 4; i++)
			{
				WoW_InventoryBag bag = wow.Inventory[i];
				if(bag != null)
					bagSpace += bag.Count;
			}

			if( !profile.GetBool( "Looting"))
			{
				// Check for agro
				Action = BotAction.CheckAgro;

				// Skin or Rest
				//ActionQueue.Clear();
				if( wow.HighestKnownSpells.ContainsKey(localization.GetString("skinning")) && profile.GetBool( "Skinning"))
					ActionQueue.Insert( 0, BotAction.Skinning);
				else
				{
					ActionQueue.Enqueue( BotAction.FindTarget);
				}
				return;
			}
			
			// Wait 1s before checking for skinnable objects
			if( !cooldowns.IsReady("LootFlag"))
				return;
		
			// Loot only after being rested
			if (wow.Player.HealthPercentage < profile.GetFloat( "RestBelowHealth") && profile.GetBool( "RestBeforeLooting"))
			{
				Action = BotAction.Rest;
				return;
			}
			
			// Clear the list of invalid objects		
			ClearObjectIgnoreList( IgnoreLootObjects);

			ArrayList Monsters = wow.GetNearest( wow.Player, profile.GetInteger("LootDistance"), WoW_ObjectTypes.Unit);
			foreach( WoW_Distance dist in Monsters)
			{
				WoW_Object obj = dist.Obj;

				// Check if its a valid loot object
				if( !obj.IsValid || !obj.IsDead || !obj.CanLoot || IgnoreLootObjects.IndexOf( obj) != -1)
					continue;

				// Are we close enough to pick up the loot?
				if( dist.Distance > 5)
				{
					// Move to the dead object
					wow.MoveTo( obj, 5, true, true);

					// Recheck agro
					Action = BotAction.CheckAgro;

					// Continue looting after
					ActionQueue.Clear();
					cooldowns.SetTime("LootFlag");
					ActionQueue.Enqueue( BotAction.Loot);
					return;
				}

				// Are we too close to pick up the loot?
				if( dist.Distance < 1)
				{
					wow.TimedMovement(WoW_Movement.Backward, 100);
				}

				// Make sure we are facing the target
				//AddMessage(string.Format("Facing Target in loot"));
				if (obj != null) wow.Face( obj);
				//AddMessage(string.Format("Done Facing Target in loot"));
				// Right click it
				wow.RightClick( obj);

				// Is there loot?
				if( ListOfCrapLoot.Count > 0) CleanupInventory();
				if( WaitForLoot())
				{
					if ( wow.Loot.Count() > 0 && bagSpace != bagUsed)
					{
						AddMessage( "Looting Corpse");
						for( int lootIndex = 0; lootIndex < wow.Loot.Count(); lootIndex++ )
						{
							if ( wow.Loot[ lootIndex ].IsCoins )
							{
								AddMessage( "Loot: Coins");
							}
							if ( (wow.Loot[ lootIndex ].ItemQuality == 0 && (profile.GetBool("Rogue.LootGreyItems")) == true ) ||
								(wow.Loot[ lootIndex ].ItemQuality == 1 && (profile.GetBool("Rogue.LootWhiteItems")) == true ) ||
								(wow.Loot[ lootIndex ].ItemQuality == 2 && (profile.GetBool("Rogue.LootGreenItems")) == true ) ||
								(wow.Loot[ lootIndex ].ItemQuality == 3 && (profile.GetBool("Rogue.LootBlueItems")) == true ) ||
								(wow.Loot[ lootIndex ].ItemQuality == 4 && (profile.GetBool("Rogue.LootPurpleItems")) == true ) ||
								(wow.Loot[ lootIndex ].ItemQuality == 5 && (profile.GetBool("Rogue.LootOrangeItems")) == true ) ||
								(wow.Loot[ lootIndex ].ItemQuality == 6 && (profile.GetBool("Rogue.LootRedItems")) == true ) || 
								(profile.GetBool("Rogue.CustomLooting")) == false && !wow.Loot[ lootIndex ].IsCoins )
							{
								string QualMessage = "";
								if (wow.Loot[ lootIndex ].ItemQuality == 0) QualMessage = "Poor";
								if (wow.Loot[ lootIndex ].ItemQuality == 1) QualMessage = "Normal";
								if (wow.Loot[ lootIndex ].ItemQuality == 2) QualMessage = "Uncommon";
								if (wow.Loot[ lootIndex ].ItemQuality == 3) QualMessage = "Rare";
								if (wow.Loot[ lootIndex ].ItemQuality == 4) QualMessage = "Epic";
								if (wow.Loot[ lootIndex ].ItemQuality == 5) QualMessage = "Legendary";
								if (wow.Loot[ lootIndex ].ItemQuality == 6) QualMessage = "Artifact";
								AddMessage( "Loot: " + wow.Loot[lootIndex].ItemName + " Quality: " + QualMessage);
								//System.Threading.Thread.Sleep(100 + rnd.Next(500) );
							}
							else
							{								
								ListOfCrapLoot.Add( wow.Loot[ lootIndex ].ItemName);
								//System.Threading.Thread.Sleep(100 + rnd.Next(500) );
							}
						}
						int PossibleBadCorpse = 0;
						do
						{
							wow.Loot.PickupAllLoot();
							PossibleBadCorpse = PossibleBadCorpse + 1;
						}while (PossibleBadCorpse < 15);
						if (PossibleBadCorpse >= 15) wow.TimedMovement(WoW_Movement.Backward,1);
						wow.ClearTarget();
						IgnoreLootObjects.Add(obj);
						cooldowns.SetTime("LootFlag");

						if( ListOfCrapLoot.Count > 0) CleanupInventory();
						Action = BotAction.CheckAgro;
						if( wow.HighestKnownSpells.ContainsKey(localization.GetString("skinning")) && profile.GetBool( "Skinning"))
							ActionQueue.Insert( 0, BotAction.Skinning);
						else
						{
							ActionQueue.Insert( 0, BotAction.Loot);
							ActionQueue.Enqueue( BotAction.FindTarget);
						}
						return;
					} 
				}
				else
				{
					// There was no loot, make sure the loot window closes
					wow.ClearTarget();
					wow.TimedMovement(WoW_Movement.Backward,1);
				}

				// Wait for the loot flag to disapear

				// Flag this object as 'looted'
				IgnoreLootObjects.Add( obj);
				wow.TimedMovement(WoW_Movement.Backward,1);

				// Recheck agro
				Action = BotAction.CheckAgro;
				ActionQueue.Insert( 0, BotAction.Loot);
				ActionQueue.Enqueue( BotAction.FindTarget);

				// Continue looting after
				cooldowns.SetTime("LootFlag");
				return;
			}
			// Check if we looted anything we can "open"
			if(ListOfOpenableLoot.Count > 0 && profile.GetBool("Rogue.OpenLoot"))

			foreach (WoW_Object obj in wow.Inventory.GetAllItems())
			{
				if(ListOfOpenableLoot.Contains(obj.Name))
				{
					wow.Inventory.UseItem(obj.Name);
					if( WaitForLoot())
					{
						int PossibleBadClam = 0;
						do
						{
							wow.Loot.PickupAllLoot();
							PossibleBadClam = PossibleBadClam + 1;
						}while (PossibleBadClam < 15);
					}
				}
			}
								
			// Check for anything on the Crap Loot List
			if ( profile.GetBool ( "CrapLoot.Crap" ) )
			{
				ListOfTrash.Clear ( );
				
				// #### i had to do dirty things for valvet to get this
				ArrayList ListOfCrapLoot = new ArrayList ( );
				ListOfCrapLoot.Clear ( );
				
				string crap = "";
				char delimiter = ',';
				string [ ] itemssplit = profile.GetString ( "CrapLoot.CrapLoot" ).Split ( delimiter );
				for ( int i = 0; i < itemssplit.Length; i ++ ) 
				{
					crap = itemssplit [ i ].ToString ( );

					if ( crap != "" ) 
					{
						ListOfCrapLoot.Add ( crap );
					}
				
				}
			
				foreach ( string word in ListOfCrapLoot ) 
				{
					
					if ( word != "" ) 
					{
						ListOfTrash.Add ( word );
					}
					
				}
				
				// #### Check if we looted anything we can "trash"
				if ( ListOfTrash.Count > 0 ) 
				{
					AddMessage ( "Checking for loot we can 'trash'" );
				}
				
				int trash = 0;
				foreach (WoW_Object obj in wow.Inventory.GetAllItems())
				{
					if(ListOfTrash.Contains(obj.Name))
					{
						trash ++;
					}
				}
				
				if ( trash > 0 ) 
				{
					
					for ( int bag = 0; bag < wow.Inventory.Count; bag ++ ) 
					{
				
						for ( int slot = 0; slot < wow.Inventory[bag].Count; slot ++ ) 
						{
							WoW_Object obj = wow.Inventory[bag, slot];
							if ( obj == null || !ListOfTrash.Contains ( obj.Name ) )
							{
								continue;
							}
					
							slot = ( slot + 1 );
					
							AddMessage ( "Trashing " + obj.Name );
							wow.SendScript ( string.Format ( localization.GetString ( "pickupcontaineritem()" ), bag, slot ) );
							wow.SendScript ( localization.GetString ( "deletecursoritem()" ) );
							Thread.Sleep ( 50 );
							trash --;
							if ( trash == 0 ) 
							{
								break;
							}
					
						}
				
					}
				
				}

			}
			
			// Check for agro
			Action = BotAction.CheckAgro;

			// Either skin or rest
			if( wow.HighestKnownSpells.ContainsKey(localization.GetString("skinning")) && profile.GetBool( "Skinning"))
				ActionQueue.Insert( 0, BotAction.Skinning);
			else
				ActionQueue.Enqueue( BotAction.Rest);
		}


		/// <summary>
		/// Skin all close by objects
		/// </summary>
		public override void DoSkinning()
		{
			// Even though this should have been handled by whatever set it to skinning, we'll check it again
			if( !wow.HighestKnownSpells.ContainsKey(localization.GetString("skinning")) || !profile.GetBool( "Skinning"))
			{
				// Check for agro
				Action = BotAction.CheckAgro;

				// Rest
				ActionQueue.Clear();
				ActionQueue.Enqueue( BotAction.Rest);
			}

			// Wait 1s before checking for skinnable objects
			if( !cooldowns.IsReady("SkinFlag"))
				return;

			// Clear the ignore list
			ClearObjectIgnoreList( IgnoreSkinnableObjects);

			ArrayList Monsters = wow.GetNearest( wow.Player, profile.GetInteger("SkinDistance"), WoW_ObjectTypes.Unit);
			foreach( WoW_Distance dist in Monsters)
			{
				WoW_Object obj = dist.Obj;

				// Is it a valid skinnable object?
				if( !obj.IsDead || !obj.IsSkinnable || IgnoreSkinnableObjects.IndexOf( obj) != -1)
					continue;

				// Are we close enough to skin?
				if( dist.Distance > 6)
				{
					// Move to the dead object
					wow.MoveTo( obj, 5, true, true);

					// Recheck agro
					Action = BotAction.CheckAgro;

					// Continue skinning after
					//ActionQueue.Clear();
					ActionQueue.Enqueue( BotAction.Skinning);
					return;
				}

				string msg = wow.GetRedMessage();
				// Face it
				wow.Face( obj);
				// Right click it
				wow.RightClick( obj);

				// Wait for a maximum of 5s for skinning to finish
				for( int sanity = 0; sanity < 50; sanity ++)
				{
					if( !wow.Player.IsCasting)
						break;

					Thread.Sleep( 100);
				}

				// TODO: Check if we actually skinned it
				if( WaitForLoot() && wow.GetRedMessage() == "") // did we skin successfull
				{
					IgnoreSkinnableObjects.Add( obj);
					wow.Loot.PickupAllLoot();
					WaitForPickup();
				}


				// Next :D
				Action = BotAction.CheckAgro;
				
				ActionQueue.Clear();
				ActionQueue.Enqueue( BotAction.Skinning);
				return;
			}

			// Out of stuff to skin

			Action = BotAction.CheckAgro;
			
			// Rest up
			//ActionQueue.Clear();
			ActionQueue.Enqueue( BotAction.Rest);
		}

		/// <summary>
		/// Tests if a target is suitable to fight
		/// </summary>
		public override bool TestTarget(WoW_Object obj)
		{

			// Is it dead?
			if( obj.IsDead)
				return false;

			// Is it in our ignore list?
			if( IgnoreTargetObjects.Contains( obj))
				return false;
			
			// Although the agro check should have found this, handle it anyhow
			if( obj.Target == wow.Player)
			{
				// Target it
				wow.LeftClick( obj);

				// Setup the appropriate action
				Action = BotAction.GetInRange;
				ActionQueue.Clear();
				ActionQueue.Enqueue( BotAction.RangedFight);
				return true;
			}

			// Check if it has someone or something targeted
			if( obj.Target != null)
				return false;

			// Check if there is a list of target names, and if so check if this monster is in the list
			PatrolArea patrolarea = GetPatrolArea();
			if(patrolarea.NumTargets > 0)
			{
				bool bFightThis = false;

				// Don't forget to lock on SyncRoot.
				lock(patrolarea.Targets.SyncRoot)
				{
					foreach (string target in patrolarea.Targets)
						if(target == obj.Name)
							bFightThis = true;
				}
				if(!bFightThis)
					return false;
			}

			// Check field flags, this is used to exclude guards and elite mobs
			// TODO: Perhaps an advanced option where people can turn on/off certain flags
			int flags = obj.ReadStorageInt( wow.Descriptors[ obj.Type, "UNIT_FIELD_FLAGS"]);
			flags &= ~0x8000; // 0x8000 is used by turtles, unknown why
			if( flags != 0)
				return false;

			if( profile.GetBool( "AutomaticLevelSelect"))
			{
				// Get the difficulty of the monster
				int UnitDifficulty = wow.GetDifficulty( obj);

				// Is it green or yellow?
				if( UnitDifficulty == 0 || UnitDifficulty > 2)
					return false; // No :(
			}
			else
			{
				// Is it within level range?
				if( obj.Level < profile.GetInteger( "MinimumLevel") ||
					obj.Level > profile.GetInteger( "MaximumLevel"))
					return false; // No :(
			}

			// Can we even attack this?
			wow.LeftClick( obj);
			wow.SendScript("if not ( UnitCanAttack(\"player\", \"target\") ) then ClearTarget(); end");
			if(wow.Target == null)
			{
				IgnoreThis( wow.Target);
				Action = BotAction.FindTarget;
				return false; // No :(
			}

			return true;
		}


		/// <summary>
		/// Finds a target
		/// </summary>
		public override void DoFindTarget()
		{
			// This is the best time to check if we need a rest
			if( wow.Player.HealthPercentage < profile.GetFloat( "RestBelowHealth") ||
				wow.Player.ManaPercentage < profile.GetFloat( "RestBelowMana"))
			{
				// Stop running
				wow.StopMovement(WoW_Movement.Forward);

				// Switch to rest
				Action = BotAction.Rest;

				// Clear the queue
				ActionQueue.Clear();
				return;
			}
			
			// Retrieve a list of objects we could attack
			ArrayList Monsters = wow.GetNearest( wow.Player, (int)profile.GetFloat("SearchDistance"), WoW_ObjectTypes.Unit);
			for(int i = 0; i < Monsters.Count; i++)
			{
				// Get the actual WoW_Object
				WoW_Object obj = ((WoW_Distance)Monsters[i]).Obj;

				// Does this object match our fight selections?
				if( ! TestTarget(obj) )
					continue;

				// Is this object neutral or friendly, and if so is there an aggresive one near it?
				if(wow.GetUnitReaction(wow.Player, obj) >= 3)  //friendly
				{
					//AddMessage("Primary target reaction is friendly");
					// Is there another target farther away, aggresive, and near our friendly target?
					while(++i < Monsters.Count)
					{
						WoW_Object nextobj = ((WoW_Distance)Monsters[i]).Obj;

						// Is it near our primary target? Is it hostile or agressive? Does it match our fight settings?
						if(obj.GetDistance(nextobj) < 15 && wow.GetUnitReaction(wow.Player, nextobj) < 3 && TestTarget(nextobj))
						{
							//yes, lets fight this instead
							AddMessage(string.Format("Skipping {0} to fight aggresive {1}",obj.Name, nextobj.Name));

							obj = nextobj;
							FoundTarget = false;
							break;
						}
					}
				}
				
				// Target it
				wow.LeftClick( obj);
				
				if (FoundTarget == false)
				{
					AddMessage(string.Format("Found target {0} at distance {1:N} {2}",obj.Name,wow.Player.GetDistance(obj),obj.UnitRace));
					FoundTarget = true;
				}
				// Setup the appropriate action
				
				//wow.StopMovement(WoW_Movement.Forward);
				Action = BotAction.GetInRange;
				ActionQueue.Clear();
				ActionQueue.Enqueue( BotAction.RangedFight);
				return;
			}

			// are we just checking for targets while we run to the next waypoint?
			if( ActionQueue.Count > 0 && ActionQueue.Peek(0) == BotAction.MoveToWaypoint)
			{
				CheckForTargets = true;
				//yes, we are running to the next waypoint and didnt find a target, so just let the queued actions do their stuff
				Action = ActionQueue.Dequeue();
				return;
			}

			//we must be done with all targets at this waypoint, lets do our gathering stuff
			Action = BotAction.CheckAgro;
			
			ActionQueue.Clear();
			ActionQueue.Enqueue( BotAction.Mine);

		}



		private void StealthforAttack()
		{
			//First we need to run near the mob, when we get to within MinRangeAttack Float,Stealth

			//AddMessage(string.Format("In StealthforAttack"));
			// Calculate the distance to the target
			float TargetDistance = wow.Player.GetDistance( wow.Target);

			// Check if we still have a target
			if( wow.Target == null)
			{
				// Target is lost, check for agro
				Action = BotAction.CheckAgro;

				// And find a new target
				ActionQueue.Clear();
				ActionQueue.Enqueue( BotAction.FindTarget);
				return;
			}

			// In melee range, check profile to see how close we should get b4 stealth
			//AddMessage(string.Format("Checking Distance for Stealth"));
			if( TargetDistance < profile.GetInteger("Rogue.StealthxMob"))
			{
				if( wow.Player != null && 
					wow.HighestKnownSpells.ContainsKey(localization.GetString("stealth")) &&
					!wow.Player.IsDead && 
					wow.Buffs[localization.GetString("ghost")] == null &&
					wow.Buffs[localization.GetString("stealth")] == null)
				{
					if ( !cooldowns.IsReady("Stealth") || !cooldowns.IsReady("Global"))
						return;

					//AddMessage(string.Format("Doing Stealth cast"));
					wow.CastSpellByName( localization.GetString("stealth"));
					cooldowns.SetTime( "Stealth");
					cooldowns.SetTime("Global");
					//Need to give the buff a few secs to activate
					if( wow.Buffs[localization.GetString("stealth")] == null)
						return;			
				}
			}
		}

	
		public bool moveTo (float x, float y, float z, float distance) 
		{
		   	int sanity = 100;
		   	while (Math.Sqrt((wow.Player.X)*(wow.Player.X)+(wow.Player.Y)*(wow.Player.Y)) > distance - 5) 
			{
				double dist = Math.Sqrt((wow.Player.X)*(wow.Player.X)+(wow.Player.Y)*(wow.Player.Y));
				wow.MoveTo(avg(x,wow.Player.X), avg(y,wow.Player.Y),wow.Player.Z, 3,true,true);
				sanity--;
				if (sanity == 0)
				return false;
			}
			return true;
        }

		public float avg (float a, float b) 
		{
			return (a+b)/2;
		}	
		
		
		
		/// <summary>
		/// Get in range of our current target
		/// </summary>
		public override void DoGetInRange()
		{
			//AddMessage("Entering DoGetInRange");
			// Do we have a target and is it our target
			if( wow.Target == null || (wow.Target.Target != null && wow.Target.Target != wow.Player))
			{	// No? Stop moving
				wow.StopMovement(WoW_Movement.Forward);

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
					ActionQueue.Enqueue( BotAction.Loot);
					ActionQueue.Enqueue( BotAction.FindTarget);
				}
				
				return;
			}

			// Get the next action from the queue
			BotAction NextAction;
			if (ActionQueue.Count > 0)
			{
				NextAction = ActionQueue.Dequeue();
			}
			else
			{
				NextAction = BotAction.RangedFight;
			}

			// By default assume a ranged attack distance (move closer then the max, so mobs that run away are caught easier)
			float AttackDistance = profile.GetFloat("MaximumRangedAttackDistance") * 0.95f;

			// Calculate the distance to the target
			float TargetDistance = wow.Player.GetDistance( wow.Target);

			// Check if we're past the minimum distance, go to melee distance else
			if( TargetDistance < profile.GetFloat("MinimumRangedAttackDistance"))
				NextAction = BotAction.MeleeFight;

			// Should we go to melee attack distance?
			if( NextAction == BotAction.MeleeFight || (ActionQueue.Count > 0 && ActionQueue.Peek(0) == BotAction.MeleeFight ))
				AttackDistance = profile.GetFloat("MaximumMeleeAttackDistance");

			// TODO: Add (extra) stuck checking here

			float distToWalk = TargetDistance - AttackDistance;

			// Do our final distance move?
			if( wow.Target != null && distToWalk <= 10)
			{
				// Do our final move
				if ((wow.Target.HealthPercentage > 2 && wow.Target.HealthPercentage < 13) || 
					(profile.GetString("Rogue.StealthAttack") != "Not Used" && 
					wow.Buffs[localization.GetString("stealth")] != null)) 
				{
					wow.MoveTo( wow.Target, 1, true, true);
					
				}
				else
				{
					wow.MoveTo( wow.Target, AttackDistance, true, true);
				}

				// Make sure we are facing the target
				//AddMessage(string.Format("Facing Target in getinrange"));
				wow.Face( wow.Target);
				//AddMessage(string.Format("Done Facing Target in getinrange"));

				if(wow.Player.IsInCombat)
				{
					Action = NextAction;
					ActionQueue.Clear();
				}
				else
				{
					// One final agro check, might be the same monster though
					Action = BotAction.CheckAgro;

					// Make sure we do the appropriate action
					ActionQueue.Clear();
					ActionQueue.Enqueue( NextAction);
				}
			}
			else
			{
				// Walk to our target in small steps
				if (wow.Target != null)
				{
					float TempMoveX = 0;
					float TempMoveY = 0;
					float TempMoveZ = 0;

					if (wow.Player.X < 0)
					{
						if (wow.Player.X > wow.Target.X)TempMoveX = wow.Player.X - wow.Target.X;
						else TempMoveX = wow.Target.X - wow.Player.X; 
					}
					else
					{
						if (wow.Player.X > wow.Target.X)TempMoveX = wow.Player.X - wow.Target.X;
						else TempMoveX = wow.Target.X - wow.Player.X; 
					}

					if (wow.Player.Y < 0)
					{
						if (wow.Player.Y > wow.Target.Y)TempMoveY = wow.Player.Y - wow.Target.Y;
						else TempMoveY = wow.Target.Y - wow.Player.Y; 
					}
					else
					{
						if (wow.Player.Y > wow.Target.Y)TempMoveY = wow.Player.Y - wow.Target.Y;
						else TempMoveY = wow.Target.Y - wow.Player.Y; 
					}

					if (wow.Player.Z < 0)
					{
						if (wow.Player.Z > wow.Target.Z)TempMoveZ = wow.Player.Z - wow.Target.Z;
						else TempMoveZ = wow.Target.Z - wow.Player.Z; 
					}
					else
					{
						if (wow.Player.Z > wow.Target.Z)TempMoveZ = wow.Player.Z - wow.Target.Z;
						else TempMoveZ = wow.Target.Z - wow.Player.Z; 
					}

					if (TempMoveZ < 100 && (TempMoveX > 8 || TempMoveY > 8))
					{
						
						//moveTo(wow.Target.X,wow.Target.Y,wow.Target.Z, TargetDistance - (distToWalk / 4));
						if (TargetDistance >= 50)wow.MoveTo( wow.Target.X,wow.Target.Y,wow.Player.Z, TargetDistance - (distToWalk / 6), false, true);
						if (TargetDistance < 50)wow.MoveTo( wow.Target.X,wow.Target.Y,wow.Player.Z, TargetDistance - (distToWalk / 4), false, true);
						//wow.MoveTo( TempMoveX ,TempMoveY,TempMoveZ, AttackDistance, true, true);
					}
					else
					{
						IgnoreThis( wow.Target);

						Action = BotAction.CheckAgro;

						ActionQueue.Clear();
						ActionQueue.Enqueue( BotAction.FindTarget);
						return;
					}
					}

					

				// Recheck if our target is still the closest once, only if not incombat
				if( !wow.Player.IsInCombat)
				{
					// Setup our bot actions
					Action = BotAction.CheckAgro;
					ActionQueue.Clear();
					ActionQueue.Enqueue( BotAction.FindTarget);
				}
				else
				{	// In combat, just get in range
					// Action = BotAction.GetInRange;
					ActionQueue.Clear();
					ActionQueue.Enqueue( NextAction);
				}
			}
		}

		//The actual Attack Moves
		public void DoStealthAttack()
		{

			//AddMessage(string.Format("Checking for Stealth Buff"));
			if( wow.Buffs[localization.GetString("stealth")] != null)
			{
				//AddMessage(string.Format("Checking for Ambush"));
				if( wow.HighestKnownSpells.ContainsKey(localization.GetString("Ambush")) && 
					profile.GetString("Rogue.StealthAttack") == "Ambush" && cooldowns.IsReady("Global"))
				{

					if( Math.Abs(wow.Player.Facing - wow.Target.Facing) < (Math.PI / 2))
					{	
						string RM = "";
						wow.CastSpellByName( localization.GetString("Ambush"));
						RM = wow.GetRedMessage();
						cooldowns.SetTime("Global");
						//Wait for possible Red message
						System.Threading.Thread.Sleep(500);

						if (RM.StartsWith(localization.GetString("must have a dagger equipped")))
						{
							AddMessage(string.Format("Ambush failed - no Dagger"));
							cooldowns.ClearTime("Global");
						}
						if (RM.StartsWith(localization.GetString("out of range.")) || RM == localization.GetString("you are too far away"))
						{	// Walk a bit closer
								wow.TimedMovement( WoW_Movement.Forward, 350 + rnd.Next( 250));
								wow.CastSpellByName( localization.GetString("Ambush"));
						}
						AddMessage(string.Format("Do ") + localization.GetString("Ambush"));
						//return;
					}
				}

				//AddMessage(string.Format("Checking for Backstab"));
				if( wow.HighestKnownSpells.ContainsKey(localization.GetString("Backstab")) && 
					profile.GetString("Rogue.StealthAttack") == "Backstab" && cooldowns.IsReady("Global"))
				{
					if( Math.Abs(wow.Player.Facing - wow.Target.Facing) < (Math.PI / 2))
					{
						string RM = "";
						wow.CastSpellByName( localization.GetString("Backstab"));
						RM = wow.GetRedMessage();
						cooldowns.SetTime("Global");
						//Wait for possible Red message
						System.Threading.Thread.Sleep(500);
					
						if (wow.GetRedMessage().StartsWith(localization.GetString("must have a dagger equipped")))
						{
							AddMessage(string.Format("Backstab failed - no Dagger"));							
							cooldowns.ClearTime("Global");
							//return;
						}
						if (RM.StartsWith(localization.GetString("out of range.")) || RM == localization.GetString("you are too far away"))
						{	// Walk a bit closer
							wow.TimedMovement( WoW_Movement.Forward, 350 + rnd.Next( 250));
							wow.CastSpellByName( localization.GetString("Backstab"));
						}
							AddMessage(string.Format("Do ") + localization.GetString("Backstab"));
							//return;
					}
				}

				//AddMessage(string.Format("Checking for Garrote"));
				if( wow.HighestKnownSpells.ContainsKey(localization.GetString("Garrote")) && 
					profile.GetString("Rogue.StealthAttack") == "Garrote" && cooldowns.IsReady("Global"))
				{
					string RM = "";
					if( Math.Abs(wow.Player.Facing - wow.Target.Facing) < (Math.PI / 2))
					{
						
						wow.CastSpellByName( localization.GetString("garrote"));
						RM = wow.GetRedMessage();
						cooldowns.SetTime("Global");
						AddMessage(string.Format("Do ") + localization.GetString("Garrote"));
						
					}
					if (RM.StartsWith(localization.GetString("out of range.")) || RM == localization.GetString("you are too far away"))
					{	// Walk a bit closer
						wow.TimedMovement( WoW_Movement.Forward, 350 + rnd.Next( 250));
						wow.CastSpellByName( localization.GetString("Garrote"));
					}
				}

				//AddMessage(string.Format("Checking for Cheap Shot"));
				// We assume enough energy here. This is our fall back
				if( wow.HighestKnownSpells.ContainsKey(localization.GetString("Cheap Shot")) && 
					wow.Buffs[localization.GetString("stealth")] != null &&
					cooldowns.IsReady("Global"))
				{
					string RM = "";
					wow.CastSpellByName( localization.GetString("Cheap Shot"));
					RM = wow.GetRedMessage();
					AddMessage(string.Format("Do ") + localization.GetString("Cheap Shot"));
					cooldowns.SetTime("Global");
					if (RM.StartsWith(localization.GetString("out of range.")) || RM == localization.GetString("you are too far away"))
					{	// Walk a bit closer
						wow.TimedMovement( WoW_Movement.Forward, 350 + rnd.Next( 250));
						wow.CastSpellByName( localization.GetString("Cheap Shot"));
					}
					return;
				}
			
				// Do Sinister Strike attack if all else fails 
				if( wow.Player.Energy >= SSEnergy && cooldowns.IsReady("Global"))
				{
					wow.CastSpellByName( localization.GetString("sinister strike"));
					wow.CastSpellByName( localization.GetString("attack"));
					cooldowns.SetTime("Global");
					AddMessage(string.Format("Stealth Attack Failed - Do Sinister Strike"));
					return;
				}
			}
		}
	}
}
