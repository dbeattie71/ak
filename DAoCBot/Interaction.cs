using System;
using System.Runtime.InteropServices;
using System.Diagnostics;
using System.Text;

namespace DAoC_Bot
{
	public class Interaction
	{
		private Interaction()
		{}		

		#region P/Invoke definitions
		// Sets the window to be foreground
		[DllImport("User32")]
		private static extern int SetForegroundWindow(IntPtr hwnd);

		// Activates a window
		[DllImportAttribute("User32.DLL")] 
		private static extern bool ShowWindow(IntPtr hWnd,int nCmdShow); 
		
		// Gets a window title text
		[DllImport("User32.Dll")]
		private static extern void GetWindowText(IntPtr h, StringBuilder s, int nMaxCount);
		#endregion
		#region Constants	
		private const int SW_SHOW = 5; 
		private const int SW_RESTORE = 9; 
		#endregion
		
		/// <summary>
		/// Use the P/Invoke to activate a window (application)
		/// </summary>
		/// <param name="MainWindowHandle">Handle to the main window of the application</param>
		private static void Activate(IntPtr MainWindowHandle)
		{
			ShowWindow(MainWindowHandle,SW_RESTORE); 		
			SetForegroundWindow(MainWindowHandle);
		}
		
		/// <summary>
		/// Activate an application with a given processID
		/// </summary>
		/// <param name="ProcessID">ID of the process to activate</param> 
		public static void AppActivate(int ProcessID)
		{
			// Get the process object
			Process processToActivate = Process.GetProcessById(ProcessID);
			if(null!=processToActivate)
			{	
				// Retrieve the main window handle from the process
				IntPtr mainWindowHandle = processToActivate.MainWindowHandle;
				Interaction.Activate(mainWindowHandle);
			} // if(null!=processToActivate)
		}
		
		/// <summary>
		/// Activate a window with a certain title
		/// </summary>
		/// <param name="Title">Title of the main window</param>		
		public static void AppActivate(string title)
		{
			Interaction.AppActivate(title, false);
		}
		
		/// <summary>
		/// Activate a window with a certain title
		/// </summary>
		/// <param name="Title">Title of the main window</param>
		/// <param name="AllMatchingWindows">True if you want to activate all windows with a smiliar title</param>
		public static void AppActivate(string Title, bool AllMatchingWindows)
		{
			IntPtr mainWindowHandle = IntPtr.Zero;
			
			foreach( Process process in Process.GetProcesses() ) 
			{ 
				mainWindowHandle = process.MainWindowHandle;
				
				if ( mainWindowHandle != IntPtr.Zero ) 
				{
					string title = "";
					StringBuilder sb = new StringBuilder(256);
					sb.Length = 256;
					
					GetWindowText(mainWindowHandle,sb, sb.Length); 				
					
					title = sb.ToString();
					
					if ( title.Length > 0 ) 
					{						
						if ( title.IndexOf( Title ) >= 0 ) 
						{ 							
							Interaction.Activate(mainWindowHandle);							
							if(!AllMatchingWindows)break;
						} // if (title.StartsWithStartsWith(Title)) 
					} // if(ret > 0) 				
				} // if (mainWindowHandle != IntPtr.Zero) 				
			} // foreach(Process process in Process.GetProcesses()) 			
		}
	}
}
