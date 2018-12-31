using System;
using System.Runtime.InteropServices;

namespace DAoC_Bot
{
	/// <summary>
	/// Class to play sounds, code was gotten from:
	/// http://www.codeguru.com/Csharp/Csharp/cs_graphics/sound/article.php/c6143/
	/// (with a small modification
	/// 
	/// </summary>
	public class WAVSounds
	{
		[DllImport("WinMM.dll")]
		private static extern bool PlaySound( string wfname, int mod, int fuSound);

		//  flag values for SoundFlags argument on PlaySound
		public const int SND_SYNC = 0x0000;		// play synchronously
												// (default)
		public const int SND_ASYNC = 0x0001;		// play asynchronously
		public const int SND_NODEFAULT  = 0x0002;		// silence (!default)
												// if sound not found
		public const int SND_MEMORY = 0x0004;		// pszSound poconst ints to
												// a memory file
		public const int SND_LOOP = 0x0008;		// loop the sound until
												// next sndPlaySound
		public const int SND_NOSTOP = 0x0010;		// don't stop any
												// currently playing
												// sound

		public const int SND_NOWAIT = 0x00002000;// don't wait if the
												// driver is busy
		public const int SND_ALIAS = 0x00010000;// name is a Registry
												// alias
		public const int SND_ALIAS_ID = 0x00110000;// alias is a predefined
												// ID
		public const int SND_FILENAME = 0x00020000;// name is file name
		public const int SND_RESOURCE = 0x00040004;// name is resource name
												// or atom
		public const int SND_PURGE = 0x0040;	// purge non-static
												// events for task
		public const int SND_APPLICATION = 0x0080;	// look for application-
												// specific association

		//-----------------------------------------------------------------
		public static void Play( string wfname, int SoundFlags)
		{
			PlaySound( wfname, 0, SoundFlags);
		}
		//-----------------------------------------------------------------
		public static void StopPlay()
		{
			PlaySound( null, 0, SND_PURGE);
		}
		//-----------------------------------------------------------------
	}   //End WAVSounds class
}
