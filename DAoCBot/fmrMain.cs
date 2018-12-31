//------------------------------------------------------------------------------

//-------------------------------------------------------------------------------
using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Data;
using System.Runtime.InteropServices;
using AutoKillerScript;
using System.IO;
using System.Xml;
using System.Threading;
using System.Diagnostics;

namespace DAoC_Bot
{
	/// <summary>
	/// Summary description for Form1.
	/// </summary>
	public class frmMain : System.Windows.Forms.Form
	{
		private System.Windows.Forms.Button btnDistances;
		private System.Windows.Forms.Button btnInitialize;
		private System.Windows.Forms.Button btnClassSettings;
		private System.Windows.Forms.Button btnFlee;
		private System.Windows.Forms.Button btnTravelPath;
		private System.Windows.Forms.Panel panel4;
		private System.Windows.Forms.Panel panel9;
		private System.Windows.Forms.Panel panel8;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Button btnAdvancedStatus;
		private System.Windows.Forms.Button btnTravel;
		private System.Windows.Forms.Button btnStart;
		private System.Windows.Forms.Label label8;
		private System.Windows.Forms.Button btnGeneral;
		private System.Windows.Forms.Panel panel3;
		private System.Windows.Forms.Panel panel2;
		private System.Windows.Forms.Panel panel1;
		private System.Windows.Forms.Panel panel7;
		private System.Windows.Forms.Panel panel6;
		private System.Windows.Forms.Panel panel5;
		private System.Windows.Forms.Panel panel12;
		private System.Windows.Forms.Panel panel13;
		private System.Windows.Forms.Panel panel10;
		private System.Windows.Forms.Panel panel11;
		private System.Windows.Forms.Panel panel16;
		private System.Windows.Forms.Panel panel17;
		private System.Windows.Forms.Panel panel14;
		private System.Windows.Forms.Panel panel15;
		private System.Windows.Forms.Button btnPatrolArea;
		private System.Windows.Forms.Button btnPause;
		private System.Windows.Forms.Button btnStatistics;
		private System.Windows.Forms.Button btnRest;
		private System.Windows.Forms.Button btnStop;
		private System.Windows.Forms.NotifyIcon notifyIcon1;
		private System.Windows.Forms.Timer notifyIconTimer;
		private System.Windows.Forms.ContextMenu notifyIconMenu;
		private System.ComponentModel.IContainer components;
		private System.Windows.Forms.MenuItem notifyIconMenuItem1;
		private System.Windows.Forms.MenuItem notifyIconMenuItem2;
		private System.Windows.Forms.MenuItem notifyIconMenuItem3;
		private System.Windows.Forms.MenuItem notifyIconMenuItem4;
		private System.Windows.Forms.Button btnLoot;
		private int tickCounter = 1;

		public frmMain()
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();

			//
			// TODO: Add any constructor code after InitializeComponent call
			//
		}

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		protected override void Dispose( bool disposing )
		{
			if( disposing )
			{
				//get rid of the engine thread
				if(bEngineStarted)
				{
					bEngineStarted = false;
					engine.Join();				//wait for that thread to finish
				}

				string directory = Path.GetDirectoryName(Application.ExecutablePath);
				if( !directory.EndsWith( "\\"))
					directory += "\\";

				directory += "\\form.xml";

				XmlDocument doc = new XmlDocument();
				doc.LoadXml( "<Form/>");

				// Create the element
				XmlNode CoordNode = doc.CreateElement("Coords");

				//add the X coord
				XmlAttribute attr = doc.CreateAttribute("X");
				attr.Value = this.Left.ToString();
				CoordNode.Attributes.Append( attr);
				
				//add the Y coord
				attr = doc.CreateAttribute("Y");
				attr.Value = this.Top.ToString();
				CoordNode.Attributes.Append( attr);
				
				//append to the xml doc
				doc.DocumentElement.AppendChild( CoordNode);


				//save it
				doc.Save( directory);

				if (components != null) 
				{
					components.Dispose();
				}
			}
			base.Dispose( disposing );
		}

		#region Windows Form Designer generated code
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent() 
		{
			this.components = new System.ComponentModel.Container();
			this.btnStop = new System.Windows.Forms.Button();
			this.btnRest = new System.Windows.Forms.Button();
			this.btnStatistics = new System.Windows.Forms.Button();
			this.btnPause = new System.Windows.Forms.Button();
			this.btnPatrolArea = new System.Windows.Forms.Button();
			this.panel15 = new System.Windows.Forms.Panel();
			this.panel14 = new System.Windows.Forms.Panel();
			this.panel17 = new System.Windows.Forms.Panel();
			this.panel16 = new System.Windows.Forms.Panel();
			this.panel11 = new System.Windows.Forms.Panel();
			this.panel10 = new System.Windows.Forms.Panel();
			this.panel13 = new System.Windows.Forms.Panel();
			this.panel12 = new System.Windows.Forms.Panel();
			this.panel5 = new System.Windows.Forms.Panel();
			this.label2 = new System.Windows.Forms.Label();
			this.panel6 = new System.Windows.Forms.Panel();
			this.panel7 = new System.Windows.Forms.Panel();
			this.panel1 = new System.Windows.Forms.Panel();
			this.btnDistances = new System.Windows.Forms.Button();
			this.btnFlee = new System.Windows.Forms.Button();
			this.btnClassSettings = new System.Windows.Forms.Button();
			this.btnTravel = new System.Windows.Forms.Button();
			this.btnTravelPath = new System.Windows.Forms.Button();
			this.btnAdvancedStatus = new System.Windows.Forms.Button();
			this.panel4 = new System.Windows.Forms.Panel();
			this.btnGeneral = new System.Windows.Forms.Button();
			this.panel3 = new System.Windows.Forms.Panel();
			this.panel2 = new System.Windows.Forms.Panel();
			this.label1 = new System.Windows.Forms.Label();
			this.label8 = new System.Windows.Forms.Label();
			this.btnStart = new System.Windows.Forms.Button();
			this.panel8 = new System.Windows.Forms.Panel();
			this.panel9 = new System.Windows.Forms.Panel();
			this.btnInitialize = new System.Windows.Forms.Button();
			this.notifyIcon1 = new System.Windows.Forms.NotifyIcon(this.components);
			this.notifyIconMenu = new System.Windows.Forms.ContextMenu();
			this.notifyIconMenuItem1 = new System.Windows.Forms.MenuItem();
			this.notifyIconMenuItem2 = new System.Windows.Forms.MenuItem();
			this.notifyIconMenuItem3 = new System.Windows.Forms.MenuItem();
			this.notifyIconTimer = new System.Windows.Forms.Timer(this.components);
			this.notifyIconMenuItem4 = new System.Windows.Forms.MenuItem();
			this.btnLoot = new System.Windows.Forms.Button();
			this.panel5.SuspendLayout();
			this.panel1.SuspendLayout();
			this.panel4.SuspendLayout();
			this.panel2.SuspendLayout();
			this.panel8.SuspendLayout();
			this.panel9.SuspendLayout();
			this.SuspendLayout();
			// 
			// btnStop
			// 
			this.btnStop.Enabled = false;
			this.btnStop.Location = new System.Drawing.Point(172, 24);
			this.btnStop.Name = "btnStop";
			this.btnStop.Size = new System.Drawing.Size(81, 23);
			this.btnStop.TabIndex = 11;
			this.btnStop.Text = "Stop Fighting";
			this.btnStop.Click += new System.EventHandler(this.btnStop_Click);
			// 
			// btnRest
			// 
			this.btnRest.Dock = System.Windows.Forms.DockStyle.Top;
			this.btnRest.Enabled = false;
			this.btnRest.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
			this.btnRest.Location = new System.Drawing.Point(2, 57);
			this.btnRest.Name = "btnRest";
			this.btnRest.Size = new System.Drawing.Size(158, 23);
			this.btnRest.TabIndex = 15;
			this.btnRest.Text = "Rest";
			this.btnRest.Click += new System.EventHandler(this.btnRest_Click);
			// 
			// btnStatistics
			// 
			this.btnStatistics.Dock = System.Windows.Forms.DockStyle.Bottom;
			this.btnStatistics.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
			this.btnStatistics.Location = new System.Drawing.Point(2, 433);
			this.btnStatistics.Name = "btnStatistics";
			this.btnStatistics.Size = new System.Drawing.Size(158, 23);
			this.btnStatistics.TabIndex = 20;
			this.btnStatistics.Text = "Statistics";
			this.btnStatistics.Click += new System.EventHandler(this.btnStatistics_Click);
			// 
			// btnPause
			// 
			this.btnPause.Enabled = false;
			this.btnPause.Location = new System.Drawing.Point(256, 24);
			this.btnPause.Name = "btnPause";
			this.btnPause.Size = new System.Drawing.Size(88, 23);
			this.btnPause.TabIndex = 11;
			this.btnPause.Text = "Pause Fighting";
			this.btnPause.Click += new System.EventHandler(this.btnPause_Click);
			// 
			// btnPatrolArea
			// 
			this.btnPatrolArea.Dock = System.Windows.Forms.DockStyle.Top;
			this.btnPatrolArea.Enabled = false;
			this.btnPatrolArea.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
			this.btnPatrolArea.Location = new System.Drawing.Point(2, 84);
			this.btnPatrolArea.Name = "btnPatrolArea";
			this.btnPatrolArea.Size = new System.Drawing.Size(158, 23);
			this.btnPatrolArea.TabIndex = 24;
			this.btnPatrolArea.Text = "Patrol Areas";
			this.btnPatrolArea.Click += new System.EventHandler(this.btnPatrolArea_Click);
			// 
			// panel15
			// 
			this.panel15.Dock = System.Windows.Forms.DockStyle.Top;
			this.panel15.Location = new System.Drawing.Point(2, 188);
			this.panel15.Name = "panel15";
			this.panel15.Size = new System.Drawing.Size(158, 4);
			this.panel15.TabIndex = 32;
			// 
			// panel14
			// 
			this.panel14.Dock = System.Windows.Forms.DockStyle.Top;
			this.panel14.Location = new System.Drawing.Point(2, 161);
			this.panel14.Name = "panel14";
			this.panel14.Size = new System.Drawing.Size(158, 4);
			this.panel14.TabIndex = 30;
			// 
			// panel17
			// 
			this.panel17.Dock = System.Windows.Forms.DockStyle.Top;
			this.panel17.Location = new System.Drawing.Point(2, 243);
			this.panel17.Name = "panel17";
			this.panel17.Size = new System.Drawing.Size(158, 3);
			this.panel17.TabIndex = 36;
			// 
			// panel16
			// 
			this.panel16.Dock = System.Windows.Forms.DockStyle.Top;
			this.panel16.Location = new System.Drawing.Point(2, 215);
			this.panel16.Name = "panel16";
			this.panel16.Size = new System.Drawing.Size(158, 4);
			this.panel16.TabIndex = 34;
			// 
			// panel11
			// 
			this.panel11.Dock = System.Windows.Forms.DockStyle.Top;
			this.panel11.Location = new System.Drawing.Point(2, 80);
			this.panel11.Name = "panel11";
			this.panel11.Size = new System.Drawing.Size(158, 4);
			this.panel11.TabIndex = 25;
			// 
			// panel10
			// 
			this.panel10.Location = new System.Drawing.Point(4, 55);
			this.panel10.Name = "panel10";
			this.panel10.Size = new System.Drawing.Size(156, 4);
			this.panel10.TabIndex = 15;
			// 
			// panel13
			// 
			this.panel13.Dock = System.Windows.Forms.DockStyle.Top;
			this.panel13.Location = new System.Drawing.Point(2, 134);
			this.panel13.Name = "panel13";
			this.panel13.Size = new System.Drawing.Size(158, 5);
			this.panel13.TabIndex = 28;
			// 
			// panel12
			// 
			this.panel12.Dock = System.Windows.Forms.DockStyle.Top;
			this.panel12.Location = new System.Drawing.Point(2, 107);
			this.panel12.Name = "panel12";
			this.panel12.Size = new System.Drawing.Size(158, 4);
			this.panel12.TabIndex = 26;
			// 
			// panel5
			// 
			this.panel5.BackColor = System.Drawing.SystemColors.ActiveCaption;
			this.panel5.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
			this.panel5.Controls.Add(this.label2);
			this.panel5.Dock = System.Windows.Forms.DockStyle.Bottom;
			this.panel5.Location = new System.Drawing.Point(2, 405);
			this.panel5.Name = "panel5";
			this.panel5.Size = new System.Drawing.Size(158, 24);
			this.panel5.TabIndex = 23;
			// 
			// label2
			// 
			this.label2.Dock = System.Windows.Forms.DockStyle.Fill;
			this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.label2.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
			this.label2.Location = new System.Drawing.Point(0, 0);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(154, 20);
			this.label2.TabIndex = 0;
			this.label2.Text = "Bot Status";
			this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// panel6
			// 
			this.panel6.Dock = System.Windows.Forms.DockStyle.Bottom;
			this.panel6.Location = new System.Drawing.Point(2, 429);
			this.panel6.Name = "panel6";
			this.panel6.Size = new System.Drawing.Size(158, 4);
			this.panel6.TabIndex = 22;
			// 
			// panel7
			// 
			this.panel7.Dock = System.Windows.Forms.DockStyle.Bottom;
			this.panel7.Location = new System.Drawing.Point(2, 456);
			this.panel7.Name = "panel7";
			this.panel7.Size = new System.Drawing.Size(158, 5);
			this.panel7.TabIndex = 19;
			// 
			// panel1
			// 
			this.panel1.Controls.Add(this.btnLoot);
			this.panel1.Controls.Add(this.panel17);
			this.panel1.Controls.Add(this.btnDistances);
			this.panel1.Controls.Add(this.panel16);
			this.panel1.Controls.Add(this.btnFlee);
			this.panel1.Controls.Add(this.panel15);
			this.panel1.Controls.Add(this.btnClassSettings);
			this.panel1.Controls.Add(this.panel14);
			this.panel1.Controls.Add(this.btnTravel);
			this.panel1.Controls.Add(this.panel13);
			this.panel1.Controls.Add(this.btnTravelPath);
			this.panel1.Controls.Add(this.panel12);
			this.panel1.Controls.Add(this.btnPatrolArea);
			this.panel1.Controls.Add(this.panel11);
			this.panel1.Controls.Add(this.panel5);
			this.panel1.Controls.Add(this.panel6);
			this.panel1.Controls.Add(this.btnStatistics);
			this.panel1.Controls.Add(this.panel7);
			this.panel1.Controls.Add(this.btnAdvancedStatus);
			this.panel1.Controls.Add(this.btnRest);
			this.panel1.Controls.Add(this.panel4);
			this.panel1.Controls.Add(this.btnGeneral);
			this.panel1.Controls.Add(this.panel3);
			this.panel1.Controls.Add(this.panel2);
			this.panel1.Dock = System.Windows.Forms.DockStyle.Left;
			this.panel1.DockPadding.Bottom = 2;
			this.panel1.DockPadding.Left = 2;
			this.panel1.DockPadding.Top = 2;
			this.panel1.Location = new System.Drawing.Point(0, 0);
			this.panel1.Name = "panel1";
			this.panel1.Size = new System.Drawing.Size(160, 485);
			this.panel1.TabIndex = 7;
			// 
			// btnDistances
			// 
			this.btnDistances.Dock = System.Windows.Forms.DockStyle.Top;
			this.btnDistances.Enabled = false;
			this.btnDistances.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
			this.btnDistances.Location = new System.Drawing.Point(2, 219);
			this.btnDistances.Name = "btnDistances";
			this.btnDistances.Size = new System.Drawing.Size(158, 24);
			this.btnDistances.TabIndex = 35;
			this.btnDistances.Text = "Distances and Levels";
			this.btnDistances.Click += new System.EventHandler(this.btnDistances_Click);
			// 
			// btnFlee
			// 
			this.btnFlee.Dock = System.Windows.Forms.DockStyle.Top;
			this.btnFlee.Enabled = false;
			this.btnFlee.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
			this.btnFlee.Location = new System.Drawing.Point(2, 192);
			this.btnFlee.Name = "btnFlee";
			this.btnFlee.Size = new System.Drawing.Size(158, 23);
			this.btnFlee.TabIndex = 33;
			this.btnFlee.Text = "Flee";
			this.btnFlee.Click += new System.EventHandler(this.btnFlee_Click);
			// 
			// btnClassSettings
			// 
			this.btnClassSettings.Dock = System.Windows.Forms.DockStyle.Top;
			this.btnClassSettings.Enabled = false;
			this.btnClassSettings.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
			this.btnClassSettings.Location = new System.Drawing.Point(2, 165);
			this.btnClassSettings.Name = "btnClassSettings";
			this.btnClassSettings.Size = new System.Drawing.Size(158, 23);
			this.btnClassSettings.TabIndex = 31;
			this.btnClassSettings.Text = "Class Settings";
			this.btnClassSettings.Click += new System.EventHandler(this.btnClassSettings_Click);
			// 
			// btnTravel
			// 
			this.btnTravel.Dock = System.Windows.Forms.DockStyle.Top;
			this.btnTravel.Enabled = false;
			this.btnTravel.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
			this.btnTravel.Location = new System.Drawing.Point(2, 139);
			this.btnTravel.Name = "btnTravel";
			this.btnTravel.Size = new System.Drawing.Size(158, 22);
			this.btnTravel.TabIndex = 29;
			this.btnTravel.Text = "Travel";
			this.btnTravel.Click += new System.EventHandler(this.btnTravel_Click);
			// 
			// btnTravelPath
			// 
			this.btnTravelPath.Dock = System.Windows.Forms.DockStyle.Top;
			this.btnTravelPath.Enabled = false;
			this.btnTravelPath.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
			this.btnTravelPath.Location = new System.Drawing.Point(2, 111);
			this.btnTravelPath.Name = "btnTravelPath";
			this.btnTravelPath.Size = new System.Drawing.Size(158, 23);
			this.btnTravelPath.TabIndex = 27;
			this.btnTravelPath.Text = "Waypoints";
			this.btnTravelPath.Click += new System.EventHandler(this.btnTravelPath_Click);
			// 
			// btnAdvancedStatus
			// 
			this.btnAdvancedStatus.Dock = System.Windows.Forms.DockStyle.Bottom;
			this.btnAdvancedStatus.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
			this.btnAdvancedStatus.Location = new System.Drawing.Point(2, 461);
			this.btnAdvancedStatus.Name = "btnAdvancedStatus";
			this.btnAdvancedStatus.Size = new System.Drawing.Size(158, 22);
			this.btnAdvancedStatus.TabIndex = 18;
			this.btnAdvancedStatus.Text = "Advanced";
			this.btnAdvancedStatus.Click += new System.EventHandler(this.btnAdvancedStatus_Click);
			// 
			// panel4
			// 
			this.panel4.Controls.Add(this.panel10);
			this.panel4.Dock = System.Windows.Forms.DockStyle.Top;
			this.panel4.Location = new System.Drawing.Point(2, 53);
			this.panel4.Name = "panel4";
			this.panel4.Size = new System.Drawing.Size(158, 4);
			this.panel4.TabIndex = 14;
			// 
			// btnGeneral
			// 
			this.btnGeneral.Dock = System.Windows.Forms.DockStyle.Top;
			this.btnGeneral.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
			this.btnGeneral.Location = new System.Drawing.Point(2, 30);
			this.btnGeneral.Name = "btnGeneral";
			this.btnGeneral.Size = new System.Drawing.Size(158, 23);
			this.btnGeneral.TabIndex = 13;
			this.btnGeneral.Text = "General";
			this.btnGeneral.Click += new System.EventHandler(this.btnGeneral_Click);
			// 
			// panel3
			// 
			this.panel3.Dock = System.Windows.Forms.DockStyle.Top;
			this.panel3.Location = new System.Drawing.Point(2, 26);
			this.panel3.Name = "panel3";
			this.panel3.Size = new System.Drawing.Size(158, 4);
			this.panel3.TabIndex = 12;
			// 
			// panel2
			// 
			this.panel2.BackColor = System.Drawing.SystemColors.ActiveCaption;
			this.panel2.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
			this.panel2.Controls.Add(this.label1);
			this.panel2.Dock = System.Windows.Forms.DockStyle.Top;
			this.panel2.Location = new System.Drawing.Point(2, 2);
			this.panel2.Name = "panel2";
			this.panel2.Size = new System.Drawing.Size(158, 24);
			this.panel2.TabIndex = 11;
			// 
			// label1
			// 
			this.label1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.label1.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
			this.label1.Location = new System.Drawing.Point(0, 0);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(154, 20);
			this.label1.TabIndex = 0;
			this.label1.Text = "Settings";
			this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// label8
			// 
			this.label8.Dock = System.Windows.Forms.DockStyle.Fill;
			this.label8.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
			this.label8.Location = new System.Drawing.Point(0, 0);
			this.label8.Name = "label8";
			this.label8.Size = new System.Drawing.Size(339, 12);
			this.label8.TabIndex = 0;
			this.label8.Text = "Bot Control";
			this.label8.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// btnStart
			// 
			this.btnStart.Enabled = false;
			this.btnStart.Location = new System.Drawing.Point(88, 24);
			this.btnStart.Name = "btnStart";
			this.btnStart.Size = new System.Drawing.Size(81, 23);
			this.btnStart.TabIndex = 12;
			this.btnStart.Text = "Start Fighting";
			this.btnStart.Click += new System.EventHandler(this.btnStart_Click);
			// 
			// panel8
			// 
			this.panel8.Controls.Add(this.panel9);
			this.panel8.Controls.Add(this.btnInitialize);
			this.panel8.Controls.Add(this.btnStart);
			this.panel8.Controls.Add(this.btnStop);
			this.panel8.Controls.Add(this.btnPause);
			this.panel8.Dock = System.Windows.Forms.DockStyle.Bottom;
			this.panel8.DockPadding.All = 2;
			this.panel8.Location = new System.Drawing.Point(160, 433);
			this.panel8.Name = "panel8";
			this.panel8.Size = new System.Drawing.Size(347, 52);
			this.panel8.TabIndex = 9;
			// 
			// panel9
			// 
			this.panel9.BackColor = System.Drawing.SystemColors.ControlDark;
			this.panel9.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
			this.panel9.Controls.Add(this.label8);
			this.panel9.Dock = System.Windows.Forms.DockStyle.Top;
			this.panel9.Location = new System.Drawing.Point(2, 2);
			this.panel9.Name = "panel9";
			this.panel9.Size = new System.Drawing.Size(343, 16);
			this.panel9.TabIndex = 20;
			// 
			// btnInitialize
			// 
			this.btnInitialize.Location = new System.Drawing.Point(4, 24);
			this.btnInitialize.Name = "btnInitialize";
			this.btnInitialize.Size = new System.Drawing.Size(81, 23);
			this.btnInitialize.TabIndex = 10;
			this.btnInitialize.Text = "Initialize";
			this.btnInitialize.Click += new System.EventHandler(this.btnInitialize_Click);
			// 
			// notifyIcon1
			// 
			this.notifyIcon1.ContextMenu = this.notifyIconMenu;
			this.notifyIcon1.Text = "Waiting for WoW to start";
			this.notifyIcon1.Visible = true;
			this.notifyIcon1.DoubleClick += new System.EventHandler(this.notifyIcon1_DoubleClick);
			// 
			// notifyIconMenu
			// 
			this.notifyIconMenu.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
																						   this.notifyIconMenuItem1,
																						   this.notifyIconMenuItem2,
																						   this.notifyIconMenuItem3});
			// 
			// notifyIconMenuItem1
			// 
			this.notifyIconMenuItem1.Enabled = false;
			this.notifyIconMenuItem1.Index = 0;
			this.notifyIconMenuItem1.Text = "Start Fighting";
			this.notifyIconMenuItem1.Click += new System.EventHandler(this.notifyIconMenuItem1_Click);
			// 
			// notifyIconMenuItem2
			// 
			this.notifyIconMenuItem2.Enabled = false;
			this.notifyIconMenuItem2.Index = 1;
			this.notifyIconMenuItem2.Text = "Pause Fighting";
			this.notifyIconMenuItem2.Click += new System.EventHandler(this.notifyIconMenuItem2_Click);
			// 
			// notifyIconMenuItem3
			// 
			this.notifyIconMenuItem3.Index = 2;
			this.notifyIconMenuItem3.Text = "Initialize";
			this.notifyIconMenuItem3.Click += new System.EventHandler(this.notifyIconMenuItem3_Click);
			// 
			// notifyIconTimer
			// 
			this.notifyIconTimer.Interval = 5000;
			this.notifyIconTimer.Tick += new System.EventHandler(this.notifyIconTimer_Tick);
			// 
			// notifyIconMenuItem4
			// 
			this.notifyIconMenuItem4.Index = -1;
			this.notifyIconMenuItem4.Text = "";
			// 
			// btnLoot
			// 
			this.btnLoot.Dock = System.Windows.Forms.DockStyle.Top;
			this.btnLoot.Enabled = false;
			this.btnLoot.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
			this.btnLoot.Location = new System.Drawing.Point(2, 246);
			this.btnLoot.Name = "btnLoot";
			this.btnLoot.Size = new System.Drawing.Size(158, 24);
			this.btnLoot.TabIndex = 37;
			this.btnLoot.Text = "Loot";
			this.btnLoot.Click += new System.EventHandler(this.btnLoot_Click);
			// 
			// frmMain
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.ClientSize = new System.Drawing.Size(507, 485);
			this.Controls.Add(this.panel8);
			this.Controls.Add(this.panel1);
			this.IsMdiContainer = true;
			this.Name = "frmMain";
			this.Text = "NotaBot";
			this.Closing += new System.ComponentModel.CancelEventHandler(this.frmMain_Closing);
			this.Load += new System.EventHandler(this.frmMain_Load);
			this.panel5.ResumeLayout(false);
			this.panel1.ResumeLayout(false);
			this.panel4.ResumeLayout(false);
			this.panel2.ResumeLayout(false);
			this.panel8.ResumeLayout(false);
			this.panel9.ResumeLayout(false);
			this.ResumeLayout(false);

		}
		#endregion

		/// <summary>
		/// The main entry point for the application.
		/// </summary>
		[STAThread]
		static void Main() 
		{
			Application.Run(new frmMain());

		}

		[DllImport("user32")]
		public static extern int GetWindowLong( IntPtr hWnd, int nIndex);

		[DllImport("user32")]
		public static extern int SetWindowLong( IntPtr hWnd, int nIndex, int dwNewLong);

		private System.Threading.Thread engine;
		private bool bEngineStarted = false;
		private bool bRunning;
		
		const int GWL_EXSTYLE = -20;
		const int WS_EX_CLIENTEDGE = 0x200;

		public AutoKillerScript.clsAutoKillerScript _ak;
		public Profile _profile;
		public DAoC_BotBase _botbase;
		public PatrolAreas _patrolareas;
		public Localization _localization;

		public frmGeneral _frmGeneral;
		public frmRest _frmRest;
		public frmPatrolArea _frmPatrolArea;
		public frmTravelPath _frmTravelPath;
		public frmTravel _frmTravel;
		public frmNecroSettings _frmNecroSettings;
		public frmScoutSettings _frmScoutSettings;
		public frmFlee _frmFlee;
		public frmDistances _frmDistances;
		public frmStatistics _frmStatistics;
		public frmAdvanced _frmAdvanced;
		public frmLoot _frmLoot;

		private void frmMain_Load(object sender, System.EventArgs e)
		{
			_ak = new AutoKillerScript.clsAutoKillerScript() ;


			_profile = new Profile();

			// No setting screens for these yet
			_profile.DefineVariable("MinimumCastDistance", 300f);
			_localization = new Localization();
			
			_patrolareas = new PatrolAreas(_ak);

			_frmGeneral = new frmGeneral();
			_profile.AddProfileForm( _frmGeneral);
			ShowForm( (Form) _frmGeneral);

			_frmRest = new frmRest();
			_profile.AddProfileForm( _frmRest);
			ShowForm( (Form) _frmRest);

			_frmPatrolArea = new frmPatrolArea(_patrolareas, _ak);
			_profile.AddProfileForm( _frmPatrolArea);
			ShowForm( (Form) _frmPatrolArea);

			_frmTravelPath = new frmTravelPath(_patrolareas, _ak);
			_profile.AddProfileForm( _frmTravelPath);
			ShowForm( (Form) _frmTravelPath);

			_frmTravel = new frmTravel(_patrolareas, _ak);
			ShowForm( (Form) _frmTravel);

			_frmNecroSettings = new frmNecroSettings(_ak);
			_profile.AddProfileForm( _frmNecroSettings);
			ShowForm( (Form) _frmNecroSettings);
			
			_frmScoutSettings = new frmScoutSettings(_ak);
			_profile.AddProfileForm( _frmScoutSettings);
			ShowForm( (Form) _frmScoutSettings);

			_frmFlee = new frmFlee( );
			_profile.AddProfileForm( _frmFlee);
			ShowForm( (Form) _frmFlee);

			_frmDistances = new frmDistances( );
			_profile.AddProfileForm( _frmDistances);
			ShowForm( (Form) _frmDistances);

			_frmLoot = new frmLoot( _ak );
			_profile.AddProfileForm( _frmLoot);
			ShowForm( (Form) _frmLoot);

			_frmStatistics = new frmStatistics( _ak);
			ShowForm( (Form) _frmStatistics);

			_frmAdvanced = new frmAdvanced( _ak);
			ShowForm( (Form) _frmAdvanced);
			
			_frmGeneral.Focus();

			// This hides the MDI frame border
			foreach( Control c in Controls)
			{
				int windowLong = GetWindowLong( c.Handle, GWL_EXSTYLE);
				windowLong &= ~WS_EX_CLIENTEDGE;
				SetWindowLong( c.Handle, GWL_EXSTYLE, windowLong);

				c.Width = c.Width + 1;
			}

			try
			{
				//get path
				string directory = Path.GetDirectoryName(Application.ExecutablePath);
				if( !directory.EndsWith( "\\"))
					directory += "\\";

				directory += "\\form.xml";

				XmlDocument doc = new XmlDocument();
				doc.Load( directory);

				XmlNode variableNode = doc.SelectSingleNode( "//Coords/@X");
				int myLeft = int.Parse( variableNode.Value);

				variableNode = doc.SelectSingleNode( "//Coords/@Y");
				int myTop = int.Parse( variableNode.Value);

				this.Top = myTop;
				this.Left = myLeft;
			}
			catch
			{
			}

			bRunning = false;
			engine = new Thread(new ThreadStart(Engine));
//			engine.Priority = ThreadPriority.AboveNormal ;
			bEngineStarted = true;
			engine.Start();				
			Thread.Sleep(0);	//so the other thread can really start		
			Thread.CurrentThread.Priority = ThreadPriority.BelowNormal ;  //let gui thread give up cpu to fight thread
		}

		private void ShowForm( Form frm)
		{
			frm.MinimizeBox = false;
			frm.MaximizeBox = false;
			frm.MdiParent = this;
			frm.ControlBox = false;
			frm.Dock = DockStyle.Fill;
			frm.Show();
		}

		private void frmMain_Closing(object sender, CancelEventArgs e)
		{
			btnStop_Click( null, null);
		}

		private void btnGeneral_Click(object sender, System.EventArgs e)
		{
			_frmGeneral.Focus();
		}

		private void btnRest_Click(object sender, System.EventArgs e)
		{
			_frmRest.Focus();
		}

		private void btnPatrolArea_Click(object sender, System.EventArgs e)
		{
			if( _ak.GameProcess == 0 )
			{
				MessageBox.Show("Please start DAoC and login a character first!");
				return;
			}

			_frmPatrolArea.Focus();
		}

		private void btnTravelPath_Click(object sender, System.EventArgs e)
		{
			if( _ak.GameProcess == 0 )
			{
				MessageBox.Show("Please start DAoC and login a character first!");
				return;
			}

			_frmTravelPath.Focus();
		}

		private void btnTravel_Click(object sender, System.EventArgs e)
		{
			if( _ak.GameProcess == 0 )
			{
				MessageBox.Show("Please start DAoC and login a character first!");
				return;
			}
			if( _botbase != null && ! _botbase.bPaused)
			{
				MessageBox.Show("Bot must be initialized and either fighting not yet started, or fighting paused to use Travel feature");
				return;
			}
			_frmTravel.Focus();
		}

		private void btnClassSettings_Click(object sender, System.EventArgs e)
		{
			switch( _profile.GetString("Class"))
			{
				case "Necro":
					_frmNecroSettings.InitializeForm(_profile);
					_frmNecroSettings.Focus();
					break;

				case "Scout":
					_frmScoutSettings.InitializeForm(_profile);
					_frmScoutSettings.Focus();
					break;
					
			}
		}


		private void btnFlee_Click(object sender, System.EventArgs e)
		{
			_frmFlee.Focus();
		}

		private void btnDistances_Click(object sender, System.EventArgs e)
		{
			_frmDistances.Focus();
		}

		private void btnLoot_Click(object sender, System.EventArgs e)
		{
			_frmLoot.Focus();
		}

		private void btnInitialize_Click(object sender, System.EventArgs e)
		{
			if( _profile.ProfileName == null ||  _profile.ProfileName == "")
			{
				MessageBox.Show("Please create or select a profile first.");
				btnGeneral_Click( null, null);
				return;
			}

//			if( _ak.GameProcess == 0 )
//				return;

			if(_frmStatistics != null)
				_frmStatistics.AddMessage("Connecting...");

			Cursor currentCursor = Cursor.Current;
			Cursor.Current = Cursors.WaitCursor;

			_botbase = null;

			_ak.RegKey = _profile.GetString("Password");
			_ak.GamePath = _profile.GetString("GamePath");

			string Client = _profile.GetString( "Client");
			if(Client == "ToA")
				_ak.EnableToA = true;
			else if(Client == "Catacombs")
				_ak.EnableCatacombs = true;

			string Location = _profile.GetString( "Location");
			if(Location != "US")
				_ak.EnableEuro = true;

			_ak.EnableAutoQuery = true;
			_ak.UseRegEx = true;

			_ak.AddString(0, "experience points");
			_ak.AddString(1, " kills the ");
			_ak.AddString(2, "You have died");
			_ak.AddString(3, "You can't see your target");
			_ak.AddString(4, "can't see its target!");
			_ak.AddString(5, "You can't cast while sitting");
			_ak.AddString(6, "\\@\\@");
			_ak.AddString(7, "You prepare your shot");
			_ak.AddString(8, "You miss");
			_ak.AddString(9, "You move and interrupt");
			_ak.AddString(10, "You shoot");
			_ak.AddString(11, "Your servant is too far away from you ");
			_ak.AddString(12, " you block the blow");
			_ak.AddString(13, "You hit ");
			_ak.AddString(14, "You attack ");
			_ak.AddString(15, "You fumble ");
			_ak.AddString(16, "You prepare to perform ");

			try
			{

				_ak.DoInit();

				btnRest.Enabled = true;
				btnPatrolArea.Enabled = true;
				btnTravelPath.Enabled = true;
				btnTravel.Enabled = true;
				btnClassSettings.Enabled = true;
				btnLoot.Enabled = true;
				btnFlee.Enabled = true;
				btnDistances.Enabled = true;
			
				btnInitialize.Enabled = false;
				btnStart.Enabled = true;
				btnStop.Enabled = false;
				btnPause.Enabled = false;				
			
				notifyIconMenuItem1.Enabled = true;
				notifyIconMenuItem2.Enabled = true;
				notifyIconMenuItem3.Enabled = false;

				_ak.OnPetWindowUpdate += new clsAutoKillerScript.OnPetWindowUpdateEventHandler(petupdate);

				bRunning = true;

			}
			catch( Exception ex)
			{
				MessageBox.Show(string.Format( "Unable to start AutoKillerscript:\n{0}", ex.Message));
			}
			finally
			{
				Cursor.Current = currentCursor;
			}
			
			if(_frmStatistics != null)
				_frmStatistics.AddMessage("Connected");
		}

		private void btnStart_Click(object sender, System.EventArgs e)
		{
			btnStart.Enabled = false;
			btnStop.Enabled = true;
			btnPatrolArea.Enabled = false;
			btnTravelPath.Enabled = false;
			btnTravel.Enabled = false;
			btnLoot.Enabled = false;
			btnPause.Enabled = true;
			
			notifyIconMenuItem2.Enabled = true;
			notifyIconMenuItem1.Text = "Stop";			
			this.notifyIconTimer.Start();

			//lets trigger saving all patrolareas to xml here assuming the user has settings
			//they like if they are starting the bot
			_patrolareas.SavePatrolAreas();

			switch( _profile.GetString("Class"))
			{
				case "Necro":
					_botbase = new DAoC_Necro( _ak, _profile, _patrolareas);
//					_chatlogparser.LogLine += new LogLineDelegate(((WoW_Rogue)_botbase).OnChatLogLine);
					break;

				case "Scout":
					_botbase = new DAoC_Scout( _ak, _profile, _patrolareas);
////					_chatlogparser.LogLine += new LogLineDelegate(((WoW_Mage)_botbase).ParseChatLogLine);
					break;
			}

			//add the statistics form's delegate to the new _botbase
			_frmStatistics.ConnectDelegates(_botbase);

			_frmStatistics.Focus();
		}

		private void btnStop_Click(object sender, System.EventArgs e)
		{
			btnRest.Enabled = false;
			btnPatrolArea.Enabled = false;
			btnTravelPath.Enabled = false;
			btnTravel.Enabled = false;
			btnClassSettings.Enabled = false;
			btnFlee.Enabled = false;
			btnDistances.Enabled = false;
			btnLoot.Enabled = false;

			bRunning = false;

			//just in case
			_ak.StopRunning();

			if (_botbase != null) 
			{
				//pause the bot
				_botbase.bPaused = true;

				switch( _profile.GetString("Class"))
				{		
					case "Necro":
//						_chatlogparser.LogLine -= new LogLineDelegate(((WoW_Rogue)_botbase).OnChatLogLine);
						break;
					case "Scout":
//						_chatlogparser.LogLine -= new LogLineDelegate(((WoW_Warrior)_botbase).ParseChatLogLine);
						break;
				}
			}

			// Remove the botbase
			_botbase = null;

			// Unregister Remote Chat
			//moved to dispose
			//_chatlogparser.LogLine -= new LogLineDelegate(_frmRemoteChat.ParseChatLogLine);

			if(_ak.GameProcess > 0)
				_ak.StopInit();

			btnInitialize.Enabled = true;
			btnStart.Enabled = false;
			btnStop.Enabled = false;
			btnPause.Enabled = false;
			
			notifyIconMenuItem1.Enabled = false;
			notifyIconMenuItem2.Enabled = false;
			notifyIconMenuItem3.Enabled = true;
			notifyIconMenuItem1.Text = "Start";

			this.notifyIconTimer.Stop();
		}

		protected void btnPause_Click(object sender, System.EventArgs e)
		{
			btnStart.Enabled = false;
			if(_botbase != null)
			{
				_botbase.bPaused =  ! _botbase.bPaused;
				if(_botbase.bPaused)
				{
					btnPause.Text = "Resume";
					notifyIconMenuItem2.Text = "Resume";
					_ak.StopRunning();

					this.notifyIconTimer.Enabled = true;

					btnTravelPath.Enabled = true;
					btnPatrolArea.Enabled = true;
					btnTravel.Enabled = true;
				}
				else
				{
					btnPause.Text = "Pause";
					notifyIconMenuItem2.Text = "Pause";

					this.notifyIconTimer.Enabled = false;

					btnTravelPath.Enabled = false;
					btnPatrolArea.Enabled = false;
					btnTravel.Enabled = false;

					Interaction.AppActivate(_ak.GameProcess);
					Thread.Sleep(750);
				}
			}
		}

		private void btnStatistics_Click(object sender, System.EventArgs e)
		{
			_frmStatistics.Focus();
		}

		private void btnAdvancedStatus_Click(object sender, System.EventArgs e)
		{
			_frmAdvanced.Focus();
		}


		private void notifyIconTimer_Tick(object sender, System.EventArgs e)
		{			
			int runningSeconds = tickCounter * 5;
			int runningMinutes;
			int runningHours;

			runningHours = runningSeconds/360;
			runningSeconds -= (runningHours * 360);
			runningMinutes = runningSeconds/60;
			runningSeconds -= (runningMinutes * 60);
			
			if (_ak.GameProcess > 0)
			{
				ListViewItem item = null;

				foreach( ListViewItem lvi in this._frmStatistics.Report.Items)
				{
					if( lvi.Text == "Total Kills")
					{
						item = lvi;
						break;
					}
				}

				if (item != null)
				{
					int kills = int.Parse( item.SubItems[1].Text) ;
					int totalxp = int.Parse( item.SubItems[2].Text);

					this.notifyIcon1.Text = string.Format("Time Running: {0} - XP: {1} - Kills: {2}", string.Format("{0:D2}:{1:D2}:{2:D2}", runningHours, runningMinutes, runningSeconds),totalxp, kills);
				}
				else
				{
					this.notifyIcon1.Text = string.Format("Time Running: {0} - XP: 0 - Kills: 0", string.Format("{0:D2}:{1:D2}:{2:D2}", runningHours, runningMinutes, runningSeconds));
				}
				
				tickCounter++;
			}
			else
			{
				this.notifyIcon1.Text = "Waiting for DAoC to start";
			}
		}

		private void notifyIcon1_DoubleClick(object sender, System.EventArgs e)
		{
			if (this.Visible)
				this.Visible = false;
			else
				this.Visible = true;
		}

		private void notifyIconMenuItem1_Click(object sender, EventArgs e)
		{
			if (this.notifyIconMenuItem1.Text == "Start")
			{
				btnStart_Click(sender, e);
				this.notifyIconMenuItem1.Text = "Stop";
			}
			else
			{	
				btnStop_Click(sender, e);
				this.notifyIconMenuItem1.Text = "Start";
			}
		}

		private void notifyIconMenuItem3_Click(object sender, EventArgs e)
		{
			btnInitialize_Click(sender, e);
		}

		private void notifyIconMenuItem2_Click(object sender, EventArgs e)
		{
			btnPause_Click(sender, e);
		}

		private void petupdate(object Sender, AKServer.DLL.DAoCServer.DAOCEventArgs e)
		{
			AKServer.DLL.DAoCServer.DAOCPet pet = e.Pet ;
			
			//			AddMessage( "Got new petID " + pet.ID.ToString());
			if(_botbase != null)
				_botbase.petID = pet.ID;
		}

		private void Engine()
		{
			while(bEngineStarted)
			{
				if(bRunning)  //if initialized
				{
					//if no fighting started we can use the travel feature
					if( _botbase == null || _botbase.bPaused )
					{
						_frmTravel.Movement();
					}
					else
					{
						//do whatever
						_botbase.DoAction();
					}
				}	
	
				//lets pulse all our actions
				Thread.Sleep(100);
			}
		}
	}
}
