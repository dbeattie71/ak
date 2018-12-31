//------------------------------------------------------------------------------
//-------------------------------------------------------------------------------
using System;
using System.Collections;

namespace DAoC_Bot
{
	/// <summary>
	/// Until WoWSharp gets a 'GetCooldown' function, this will have to do
	/// </summary>
	public class Cooldowns
	{
		Hashtable _cooldowns;

		public Cooldowns()
		{
			_cooldowns = Hashtable.Synchronized(new Hashtable());
		}

		private class Cooldown
		{
			public string Id;
			public DateTime Time;
			public int Timeout;
			public bool Disabled;
		}

		/// <summary>
		/// Defines a cooldown
		/// </summary>
		/// <param name="id"></param>
		/// <param name="timeout"></param>
		public void DefineCooldown( string id, int timeout)
		{
			Cooldown cooldown = new Cooldown();
			cooldown.Id = id;
			cooldown.Timeout = timeout;
			cooldown.Time = DateTime.Today;
			cooldown.Disabled = false;

			lock ( _cooldowns.SyncRoot ) 
			{
				_cooldowns.Add( id, cooldown);
			}
		}

		/// <summary>
		/// Has the cooldown expired?
		/// </summary>
		/// <param name="id">Cooldown</param>
		/// <returns>Has expired</returns>
		public bool IsReady( string id)
		{
			if( !_cooldowns.ContainsKey( id))
				throw new Exception(string.Format("Invalid cooldown: {0}",id));

			lock ( _cooldowns.SyncRoot ) 
			{
				Cooldown cooldown = (Cooldown) _cooldowns[ id];

				if (cooldown.Disabled == true)
					return false;

				return (DateTime.Now - cooldown.Time).TotalMilliseconds > cooldown.Timeout;
			}
		}

		/// <summary>
		/// Sets the time of the cooldown to now
		/// </summary>
		/// <param name="id">Cooldown</param>
		public void SetTime( string id)
		{
			if( !_cooldowns.ContainsKey( id))
				throw new Exception(string.Format("Invalid cooldown: {0}",id));

			lock ( _cooldowns.SyncRoot ) 
			{
				Cooldown cooldown = (Cooldown) _cooldowns[ id];
				cooldown.Time = DateTime.Now;
			}
		}

		/// <summary>
		/// Clears the time of a cooldown
		/// </summary>
		/// <param name="id">Cooldown</param>
		public void ClearTime( string id)
		{
			if( !_cooldowns.ContainsKey( id))
				throw new Exception(string.Format("Invalid cooldown: {0}",id));

			lock ( _cooldowns.SyncRoot ) 
			{
				Cooldown cooldown = (Cooldown) _cooldowns[ id];
				cooldown.Time = DateTime.Today;
				cooldown.Disabled = false;
			}
		}

		public void Disable(string id)
		{
			if( !_cooldowns.ContainsKey( id))
				throw new Exception(string.Format("Invalid cooldown: {0}",id));

			lock ( _cooldowns.SyncRoot ) 
			{
				Cooldown cooldown = (Cooldown) _cooldowns[ id];
				cooldown.Disabled = true;
			}
		}
	}
}
