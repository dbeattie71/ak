//------------------------------------------------------------------------------
//-------------------------------------------------------------------------------
using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using AutoKillerScript;

namespace DAoC_Bot
{
	/// <summary>
	/// Summary description for frmNecro.
	/// </summary>
	public class frmNecroSettings : System.Windows.Forms.Form, IProfileForm
	{
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		public frmNecroSettings(AutoKillerScript.clsAutoKillerScript ak)
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();

			//
			// TODO: Add any constructor code after InitializeComponent call
			//
			_ak = ak;

			// Create the ToolTips
			ToolTip toolTip1 = new ToolTip();

			// Set up the delays for the ToolTip.
			toolTip1.AutoPopDelay = 5000;
			toolTip1.InitialDelay = 1000;
			toolTip1.ReshowDelay = 500;
			// Force the ToolTip text to be displayed whether or not the form is active.
			toolTip1.ShowAlways = true;
      
			// Set up the ToolTips text
			// General Section
//			toolTip1.SetToolTip(this.cbxUseStealth, "Use Stealth all the Time.");
//			toolTip1.SetToolTip(this.cbxUseGouge, "Gouge when you get additional mobs and it is up.");
	
			

		}

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		protected override void Dispose( bool disposing )
		{
			if( disposing )
			{
				if(components != null)
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
			this.panel3 = new System.Windows.Forms.Panel();
			this.label2 = new System.Windows.Forms.Label();
			this.panel6 = new System.Windows.Forms.Panel();
			this.btnCancel = new System.Windows.Forms.Button();
			this.btnApply = new System.Windows.Forms.Button();
			this.tabPage5 = new System.Windows.Forms.TabPage();
			this.panel16 = new System.Windows.Forms.Panel();
			this.chkFleeLeavePet = new System.Windows.Forms.CheckBox();
			this.panel15 = new System.Windows.Forms.Panel();
			this.label4 = new System.Windows.Forms.Label();
			this.tabPage2 = new System.Windows.Forms.TabPage();
			this.panel5 = new System.Windows.Forms.Panel();
			this.label7 = new System.Windows.Forms.Label();
			this.cbxPetPassiveQ = new System.Windows.Forms.ComboBox();
			this.label8 = new System.Windows.Forms.Label();
			this.cbxPetDefensiveQ = new System.Windows.Forms.ComboBox();
			this.label10 = new System.Windows.Forms.Label();
			this.cbxPetFollowQ = new System.Windows.Forms.ComboBox();
			this.label11 = new System.Windows.Forms.Label();
			this.cbxPetFollowKey = new System.Windows.Forms.ComboBox();
			this.cbxPetPassiveKey = new System.Windows.Forms.ComboBox();
			this.cbxPetDefensiveKey = new System.Windows.Forms.ComboBox();
			this.label12 = new System.Windows.Forms.Label();
			this.cbxPetHereQ = new System.Windows.Forms.ComboBox();
			this.label13 = new System.Windows.Forms.Label();
			this.cbxPetHereKey = new System.Windows.Forms.ComboBox();
			this.panel4 = new System.Windows.Forms.Panel();
			this.label6 = new System.Windows.Forms.Label();
			this.panel8 = new System.Windows.Forms.Panel();
			this.chkUseFP = new System.Windows.Forms.CheckBox();
			this.label3 = new System.Windows.Forms.Label();
			this.cbxFightRangedLTQ = new System.Windows.Forms.ComboBox();
			this.lblRangedPowerTap = new System.Windows.Forms.Label();
			this.cbxFightRangedPTQ = new System.Windows.Forms.ComboBox();
			this.lblRangedLifeTap = new System.Windows.Forms.Label();
			this.cbxFightMeleePTQ = new System.Windows.Forms.ComboBox();
			this.lblMeleePowerTap = new System.Windows.Forms.Label();
			this.cbxFightMeleePTKey = new System.Windows.Forms.ComboBox();
			this.cbxFightRangedLTKey = new System.Windows.Forms.ComboBox();
			this.cbxFightRangedPTKey = new System.Windows.Forms.ComboBox();
			this.label5 = new System.Windows.Forms.Label();
			this.label14 = new System.Windows.Forms.Label();
			this.cbxFightFPKey = new System.Windows.Forms.ComboBox();
			this.cbxFightFPQ = new System.Windows.Forms.ComboBox();
			this.panel7 = new System.Windows.Forms.Panel();
			this.label9 = new System.Windows.Forms.Label();
			this.tabPage3 = new System.Windows.Forms.TabPage();
			this.panel12 = new System.Windows.Forms.Panel();
			this.label15 = new System.Windows.Forms.Label();
			this.cbxBuffDexQ = new System.Windows.Forms.ComboBox();
			this.label16 = new System.Windows.Forms.Label();
			this.cbxBuffAbsorbQ = new System.Windows.Forms.ComboBox();
			this.label17 = new System.Windows.Forms.Label();
			this.cbxBuffDexKey = new System.Windows.Forms.ComboBox();
			this.cbxBuffAbsorbKey = new System.Windows.Forms.ComboBox();
			this.label19 = new System.Windows.Forms.Label();
			this.cbxBuffStrengthQ = new System.Windows.Forms.ComboBox();
			this.cbxBuffStrengthKey = new System.Windows.Forms.ComboBox();
			this.label18 = new System.Windows.Forms.Label();
			this.panel11 = new System.Windows.Forms.Panel();
			this.label20 = new System.Windows.Forms.Label();
			this.tabPage6 = new System.Windows.Forms.TabPage();
			this.panel2 = new System.Windows.Forms.Panel();
			this.panel1 = new System.Windows.Forms.Panel();
			this.label1 = new System.Windows.Forms.Label();
			this.tabControl1 = new System.Windows.Forms.TabControl();
			this.tabPage1 = new System.Windows.Forms.TabPage();
			this.nudHealHealth = new System.Windows.Forms.NumericUpDown();
			this.panel13 = new System.Windows.Forms.Panel();
			this.label26 = new System.Windows.Forms.Label();
			this.panel10 = new System.Windows.Forms.Panel();
			this.label23 = new System.Windows.Forms.Label();
			this.cbxSummonPetQ = new System.Windows.Forms.ComboBox();
			this.label24 = new System.Windows.Forms.Label();
			this.cbxSummonPetKey = new System.Windows.Forms.ComboBox();
			this.label25 = new System.Windows.Forms.Label();
			this.panel9 = new System.Windows.Forms.Panel();
			this.label22 = new System.Windows.Forms.Label();
			this.label27 = new System.Windows.Forms.Label();
			this.cbxHealPetQ = new System.Windows.Forms.ComboBox();
			this.label28 = new System.Windows.Forms.Label();
			this.cbxHealPetKey = new System.Windows.Forms.ComboBox();
			this.label29 = new System.Windows.Forms.Label();
			this.label30 = new System.Windows.Forms.Label();
			this.label21 = new System.Windows.Forms.Label();
			this.cbxDebuffQ = new System.Windows.Forms.ComboBox();
			this.label31 = new System.Windows.Forms.Label();
			this.cbxDebuffKey = new System.Windows.Forms.ComboBox();
			this.label32 = new System.Windows.Forms.Label();
			this.panel3.SuspendLayout();
			this.panel6.SuspendLayout();
			this.tabPage5.SuspendLayout();
			this.panel16.SuspendLayout();
			this.panel15.SuspendLayout();
			this.tabPage2.SuspendLayout();
			this.panel5.SuspendLayout();
			this.panel4.SuspendLayout();
			this.panel8.SuspendLayout();
			this.panel7.SuspendLayout();
			this.tabPage3.SuspendLayout();
			this.panel12.SuspendLayout();
			this.panel11.SuspendLayout();
			this.tabPage6.SuspendLayout();
			this.panel2.SuspendLayout();
			this.panel1.SuspendLayout();
			this.tabControl1.SuspendLayout();
			this.tabPage1.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.nudHealHealth)).BeginInit();
			this.panel13.SuspendLayout();
			this.panel10.SuspendLayout();
			this.panel9.SuspendLayout();
			this.SuspendLayout();
			// 
			// panel3
			// 
			this.panel3.BackColor = System.Drawing.SystemColors.ActiveCaption;
			this.panel3.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
			this.panel3.Controls.Add(this.label2);
			this.panel3.Dock = System.Windows.Forms.DockStyle.Top;
			this.panel3.Location = new System.Drawing.Point(4, 4);
			this.panel3.Name = "panel3";
			this.panel3.Size = new System.Drawing.Size(335, 24);
			this.panel3.TabIndex = 14;
			// 
			// label2
			// 
			this.label2.Dock = System.Windows.Forms.DockStyle.Fill;
			this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.label2.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
			this.label2.Location = new System.Drawing.Point(0, 0);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(331, 20);
			this.label2.TabIndex = 0;
			this.label2.Text = "Necro Settings";
			this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// panel6
			// 
			this.panel6.Controls.Add(this.btnCancel);
			this.panel6.Controls.Add(this.btnApply);
			this.panel6.Dock = System.Windows.Forms.DockStyle.Bottom;
			this.panel6.Location = new System.Drawing.Point(4, 364);
			this.panel6.Name = "panel6";
			this.panel6.Size = new System.Drawing.Size(335, 24);
			this.panel6.TabIndex = 21;
			// 
			// btnCancel
			// 
			this.btnCancel.Location = new System.Drawing.Point(88, 0);
			this.btnCancel.Name = "btnCancel";
			this.btnCancel.Size = new System.Drawing.Size(75, 21);
			this.btnCancel.TabIndex = 1;
			this.btnCancel.Text = "Cancel";
			this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
			// 
			// btnApply
			// 
			this.btnApply.Location = new System.Drawing.Point(8, 0);
			this.btnApply.Name = "btnApply";
			this.btnApply.Size = new System.Drawing.Size(75, 21);
			this.btnApply.TabIndex = 0;
			this.btnApply.Text = "Apply";
			this.btnApply.Click += new System.EventHandler(this.btnApply_Click);
			// 
			// tabPage5
			// 
			this.tabPage5.Controls.Add(this.panel16);
			this.tabPage5.Controls.Add(this.panel15);
			this.tabPage5.Location = new System.Drawing.Point(4, 22);
			this.tabPage5.Name = "tabPage5";
			this.tabPage5.Size = new System.Drawing.Size(327, 306);
			this.tabPage5.TabIndex = 4;
			this.tabPage5.Text = "Flee";
			// 
			// panel16
			// 
			this.panel16.Controls.Add(this.chkFleeLeavePet);
			this.panel16.Dock = System.Windows.Forms.DockStyle.Top;
			this.panel16.Location = new System.Drawing.Point(0, 16);
			this.panel16.Name = "panel16";
			this.panel16.Size = new System.Drawing.Size(327, 128);
			this.panel16.TabIndex = 1;
			// 
			// chkFleeLeavePet
			// 
			this.chkFleeLeavePet.Location = new System.Drawing.Point(8, 8);
			this.chkFleeLeavePet.Name = "chkFleeLeavePet";
			this.chkFleeLeavePet.Size = new System.Drawing.Size(144, 24);
			this.chkFleeLeavePet.TabIndex = 2;
			this.chkFleeLeavePet.Text = "Leave pet behind";
			// 
			// panel15
			// 
			this.panel15.Controls.Add(this.label4);
			this.panel15.Dock = System.Windows.Forms.DockStyle.Top;
			this.panel15.Location = new System.Drawing.Point(0, 0);
			this.panel15.Name = "panel15";
			this.panel15.Size = new System.Drawing.Size(327, 16);
			this.panel15.TabIndex = 0;
			// 
			// label4
			// 
			this.label4.BackColor = System.Drawing.SystemColors.ControlDark;
			this.label4.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
			this.label4.Dock = System.Windows.Forms.DockStyle.Fill;
			this.label4.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
			this.label4.Location = new System.Drawing.Point(0, 0);
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size(327, 16);
			this.label4.TabIndex = 0;
			this.label4.Text = "Flee";
			this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// tabPage2
			// 
			this.tabPage2.Controls.Add(this.panel5);
			this.tabPage2.Controls.Add(this.panel4);
			this.tabPage2.Controls.Add(this.panel8);
			this.tabPage2.Controls.Add(this.panel7);
			this.tabPage2.Location = new System.Drawing.Point(4, 22);
			this.tabPage2.Name = "tabPage2";
			this.tabPage2.Size = new System.Drawing.Size(327, 306);
			this.tabPage2.TabIndex = 1;
			this.tabPage2.Text = "Fights";
			// 
			// panel5
			// 
			this.panel5.Controls.Add(this.label7);
			this.panel5.Controls.Add(this.cbxPetPassiveQ);
			this.panel5.Controls.Add(this.label8);
			this.panel5.Controls.Add(this.cbxPetDefensiveQ);
			this.panel5.Controls.Add(this.label10);
			this.panel5.Controls.Add(this.cbxPetFollowQ);
			this.panel5.Controls.Add(this.label11);
			this.panel5.Controls.Add(this.cbxPetFollowKey);
			this.panel5.Controls.Add(this.cbxPetPassiveKey);
			this.panel5.Controls.Add(this.cbxPetDefensiveKey);
			this.panel5.Controls.Add(this.label12);
			this.panel5.Controls.Add(this.cbxPetHereQ);
			this.panel5.Controls.Add(this.label13);
			this.panel5.Controls.Add(this.cbxPetHereKey);
			this.panel5.Dock = System.Windows.Forms.DockStyle.Top;
			this.panel5.Location = new System.Drawing.Point(0, 184);
			this.panel5.Name = "panel5";
			this.panel5.Size = new System.Drawing.Size(327, 120);
			this.panel5.TabIndex = 69;
			// 
			// label7
			// 
			this.label7.Location = new System.Drawing.Point(168, 4);
			this.label7.Name = "label7";
			this.label7.Size = new System.Drawing.Size(32, 17);
			this.label7.TabIndex = 54;
			this.label7.Text = "QBar";
			// 
			// cbxPetPassiveQ
			// 
			this.cbxPetPassiveQ.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cbxPetPassiveQ.Items.AddRange(new object[] {
																"1",
																"2",
																"3",
																"4",
																"5",
																"6",
																"7",
																"8",
																"9",
																"10"});
			this.cbxPetPassiveQ.Location = new System.Drawing.Point(160, 24);
			this.cbxPetPassiveQ.Name = "cbxPetPassiveQ";
			this.cbxPetPassiveQ.Size = new System.Drawing.Size(40, 21);
			this.cbxPetPassiveQ.TabIndex = 42;
			// 
			// label8
			// 
			this.label8.Location = new System.Drawing.Point(8, 48);
			this.label8.Name = "label8";
			this.label8.Size = new System.Drawing.Size(104, 21);
			this.label8.TabIndex = 50;
			this.label8.Text = "Defensive";
			this.label8.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// cbxPetDefensiveQ
			// 
			this.cbxPetDefensiveQ.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cbxPetDefensiveQ.Items.AddRange(new object[] {
																  "1",
																  "2",
																  "3",
																  "4",
																  "5",
																  "6",
																  "7",
																  "8",
																  "9",
																  "10"});
			this.cbxPetDefensiveQ.Location = new System.Drawing.Point(160, 48);
			this.cbxPetDefensiveQ.Name = "cbxPetDefensiveQ";
			this.cbxPetDefensiveQ.Size = new System.Drawing.Size(40, 21);
			this.cbxPetDefensiveQ.TabIndex = 43;
			// 
			// label10
			// 
			this.label10.Location = new System.Drawing.Point(8, 24);
			this.label10.Name = "label10";
			this.label10.Size = new System.Drawing.Size(104, 21);
			this.label10.TabIndex = 48;
			this.label10.Text = "Passive";
			this.label10.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// cbxPetFollowQ
			// 
			this.cbxPetFollowQ.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cbxPetFollowQ.ItemHeight = 13;
			this.cbxPetFollowQ.Items.AddRange(new object[] {
															   "1",
															   "2",
															   "3",
															   "4",
															   "5",
															   "6",
															   "7",
															   "8",
															   "9",
															   "10"});
			this.cbxPetFollowQ.Location = new System.Drawing.Point(160, 72);
			this.cbxPetFollowQ.Name = "cbxPetFollowQ";
			this.cbxPetFollowQ.Size = new System.Drawing.Size(40, 21);
			this.cbxPetFollowQ.TabIndex = 52;
			// 
			// label11
			// 
			this.label11.Location = new System.Drawing.Point(8, 72);
			this.label11.Name = "label11";
			this.label11.Size = new System.Drawing.Size(104, 21);
			this.label11.TabIndex = 53;
			this.label11.Text = "Follow";
			this.label11.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// cbxPetFollowKey
			// 
			this.cbxPetFollowKey.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cbxPetFollowKey.ItemHeight = 13;
			this.cbxPetFollowKey.Items.AddRange(new object[] {
																 "1",
																 "2",
																 "3",
																 "4",
																 "5",
																 "6",
																 "7",
																 "8",
																 "9",
																 "10"});
			this.cbxPetFollowKey.Location = new System.Drawing.Point(224, 72);
			this.cbxPetFollowKey.Name = "cbxPetFollowKey";
			this.cbxPetFollowKey.Size = new System.Drawing.Size(40, 21);
			this.cbxPetFollowKey.TabIndex = 52;
			// 
			// cbxPetPassiveKey
			// 
			this.cbxPetPassiveKey.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cbxPetPassiveKey.Items.AddRange(new object[] {
																  "1",
																  "2",
																  "3",
																  "4",
																  "5",
																  "6",
																  "7",
																  "8",
																  "9",
																  "10"});
			this.cbxPetPassiveKey.Location = new System.Drawing.Point(224, 24);
			this.cbxPetPassiveKey.Name = "cbxPetPassiveKey";
			this.cbxPetPassiveKey.Size = new System.Drawing.Size(40, 21);
			this.cbxPetPassiveKey.TabIndex = 42;
			// 
			// cbxPetDefensiveKey
			// 
			this.cbxPetDefensiveKey.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cbxPetDefensiveKey.Items.AddRange(new object[] {
																	"1",
																	"2",
																	"3",
																	"4",
																	"5",
																	"6",
																	"7",
																	"8",
																	"9",
																	"10"});
			this.cbxPetDefensiveKey.Location = new System.Drawing.Point(224, 48);
			this.cbxPetDefensiveKey.Name = "cbxPetDefensiveKey";
			this.cbxPetDefensiveKey.Size = new System.Drawing.Size(40, 21);
			this.cbxPetDefensiveKey.TabIndex = 43;
			// 
			// label12
			// 
			this.label12.Location = new System.Drawing.Point(232, 4);
			this.label12.Name = "label12";
			this.label12.Size = new System.Drawing.Size(32, 17);
			this.label12.TabIndex = 54;
			this.label12.Text = "Key";
			// 
			// cbxPetHereQ
			// 
			this.cbxPetHereQ.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cbxPetHereQ.ItemHeight = 13;
			this.cbxPetHereQ.Items.AddRange(new object[] {
															 "1",
															 "2",
															 "3",
															 "4",
															 "5",
															 "6",
															 "7",
															 "8",
															 "9",
															 "10"});
			this.cbxPetHereQ.Location = new System.Drawing.Point(160, 96);
			this.cbxPetHereQ.Name = "cbxPetHereQ";
			this.cbxPetHereQ.Size = new System.Drawing.Size(40, 21);
			this.cbxPetHereQ.TabIndex = 52;
			// 
			// label13
			// 
			this.label13.Location = new System.Drawing.Point(8, 96);
			this.label13.Name = "label13";
			this.label13.Size = new System.Drawing.Size(104, 21);
			this.label13.TabIndex = 53;
			this.label13.Text = "Here";
			this.label13.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// cbxPetHereKey
			// 
			this.cbxPetHereKey.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cbxPetHereKey.ItemHeight = 13;
			this.cbxPetHereKey.Items.AddRange(new object[] {
															   "1",
															   "2",
															   "3",
															   "4",
															   "5",
															   "6",
															   "7",
															   "8",
															   "9",
															   "10"});
			this.cbxPetHereKey.Location = new System.Drawing.Point(224, 96);
			this.cbxPetHereKey.Name = "cbxPetHereKey";
			this.cbxPetHereKey.Size = new System.Drawing.Size(40, 21);
			this.cbxPetHereKey.TabIndex = 52;
			// 
			// panel4
			// 
			this.panel4.Controls.Add(this.label6);
			this.panel4.Dock = System.Windows.Forms.DockStyle.Top;
			this.panel4.Location = new System.Drawing.Point(0, 168);
			this.panel4.Name = "panel4";
			this.panel4.Size = new System.Drawing.Size(327, 16);
			this.panel4.TabIndex = 68;
			// 
			// label6
			// 
			this.label6.BackColor = System.Drawing.SystemColors.ControlDark;
			this.label6.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
			this.label6.Dock = System.Windows.Forms.DockStyle.Fill;
			this.label6.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
			this.label6.Location = new System.Drawing.Point(0, 0);
			this.label6.Name = "label6";
			this.label6.Size = new System.Drawing.Size(327, 16);
			this.label6.TabIndex = 0;
			this.label6.Text = "Pet Actions";
			this.label6.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// panel8
			// 
			this.panel8.Controls.Add(this.chkUseFP);
			this.panel8.Controls.Add(this.label3);
			this.panel8.Controls.Add(this.cbxFightRangedLTQ);
			this.panel8.Controls.Add(this.lblRangedPowerTap);
			this.panel8.Controls.Add(this.cbxFightRangedPTQ);
			this.panel8.Controls.Add(this.lblRangedLifeTap);
			this.panel8.Controls.Add(this.cbxFightMeleePTQ);
			this.panel8.Controls.Add(this.lblMeleePowerTap);
			this.panel8.Controls.Add(this.cbxFightMeleePTKey);
			this.panel8.Controls.Add(this.cbxFightRangedLTKey);
			this.panel8.Controls.Add(this.cbxFightRangedPTKey);
			this.panel8.Controls.Add(this.label5);
			this.panel8.Controls.Add(this.label14);
			this.panel8.Controls.Add(this.cbxFightFPKey);
			this.panel8.Controls.Add(this.cbxFightFPQ);
			this.panel8.Dock = System.Windows.Forms.DockStyle.Top;
			this.panel8.Location = new System.Drawing.Point(0, 16);
			this.panel8.Name = "panel8";
			this.panel8.Size = new System.Drawing.Size(327, 152);
			this.panel8.TabIndex = 67;
			// 
			// chkUseFP
			// 
			this.chkUseFP.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.chkUseFP.Location = new System.Drawing.Point(8, 121);
			this.chkUseFP.Name = "chkUseFP";
			this.chkUseFP.Size = new System.Drawing.Size(166, 21);
			this.chkUseFP.TabIndex = 55;
			this.chkUseFP.Text = "Use Facilitate Painworking";
			// 
			// label3
			// 
			this.label3.Location = new System.Drawing.Point(168, 4);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(32, 17);
			this.label3.TabIndex = 54;
			this.label3.Text = "QBar";
			// 
			// cbxFightRangedLTQ
			// 
			this.cbxFightRangedLTQ.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cbxFightRangedLTQ.Items.AddRange(new object[] {
																   "1",
																   "2",
																   "3",
																   "4",
																   "5",
																   "6",
																   "7",
																   "8",
																   "9",
																   "10"});
			this.cbxFightRangedLTQ.Location = new System.Drawing.Point(160, 24);
			this.cbxFightRangedLTQ.Name = "cbxFightRangedLTQ";
			this.cbxFightRangedLTQ.Size = new System.Drawing.Size(40, 21);
			this.cbxFightRangedLTQ.TabIndex = 42;
			// 
			// lblRangedPowerTap
			// 
			this.lblRangedPowerTap.Location = new System.Drawing.Point(8, 48);
			this.lblRangedPowerTap.Name = "lblRangedPowerTap";
			this.lblRangedPowerTap.Size = new System.Drawing.Size(104, 21);
			this.lblRangedPowerTap.TabIndex = 50;
			this.lblRangedPowerTap.Text = "Ranged Power Tap";
			this.lblRangedPowerTap.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// cbxFightRangedPTQ
			// 
			this.cbxFightRangedPTQ.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cbxFightRangedPTQ.Items.AddRange(new object[] {
																   "1",
																   "2",
																   "3",
																   "4",
																   "5",
																   "6",
																   "7",
																   "8",
																   "9",
																   "10"});
			this.cbxFightRangedPTQ.Location = new System.Drawing.Point(160, 48);
			this.cbxFightRangedPTQ.Name = "cbxFightRangedPTQ";
			this.cbxFightRangedPTQ.Size = new System.Drawing.Size(40, 21);
			this.cbxFightRangedPTQ.TabIndex = 43;
			// 
			// lblRangedLifeTap
			// 
			this.lblRangedLifeTap.Location = new System.Drawing.Point(8, 24);
			this.lblRangedLifeTap.Name = "lblRangedLifeTap";
			this.lblRangedLifeTap.Size = new System.Drawing.Size(104, 21);
			this.lblRangedLifeTap.TabIndex = 48;
			this.lblRangedLifeTap.Text = "Ranged Life Tap";
			this.lblRangedLifeTap.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// cbxFightMeleePTQ
			// 
			this.cbxFightMeleePTQ.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cbxFightMeleePTQ.ItemHeight = 13;
			this.cbxFightMeleePTQ.Items.AddRange(new object[] {
																  "1",
																  "2",
																  "3",
																  "4",
																  "5",
																  "6",
																  "7",
																  "8",
																  "9",
																  "10"});
			this.cbxFightMeleePTQ.Location = new System.Drawing.Point(160, 72);
			this.cbxFightMeleePTQ.Name = "cbxFightMeleePTQ";
			this.cbxFightMeleePTQ.Size = new System.Drawing.Size(40, 21);
			this.cbxFightMeleePTQ.TabIndex = 52;
			// 
			// lblMeleePowerTap
			// 
			this.lblMeleePowerTap.Location = new System.Drawing.Point(8, 72);
			this.lblMeleePowerTap.Name = "lblMeleePowerTap";
			this.lblMeleePowerTap.Size = new System.Drawing.Size(104, 21);
			this.lblMeleePowerTap.TabIndex = 53;
			this.lblMeleePowerTap.Text = "Melee Power Tap";
			this.lblMeleePowerTap.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// cbxFightMeleePTKey
			// 
			this.cbxFightMeleePTKey.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cbxFightMeleePTKey.ItemHeight = 13;
			this.cbxFightMeleePTKey.Items.AddRange(new object[] {
																	"1",
																	"2",
																	"3",
																	"4",
																	"5",
																	"6",
																	"7",
																	"8",
																	"9",
																	"10"});
			this.cbxFightMeleePTKey.Location = new System.Drawing.Point(224, 72);
			this.cbxFightMeleePTKey.Name = "cbxFightMeleePTKey";
			this.cbxFightMeleePTKey.Size = new System.Drawing.Size(40, 21);
			this.cbxFightMeleePTKey.TabIndex = 52;
			// 
			// cbxFightRangedLTKey
			// 
			this.cbxFightRangedLTKey.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cbxFightRangedLTKey.Items.AddRange(new object[] {
																	 "1",
																	 "2",
																	 "3",
																	 "4",
																	 "5",
																	 "6",
																	 "7",
																	 "8",
																	 "9",
																	 "10"});
			this.cbxFightRangedLTKey.Location = new System.Drawing.Point(224, 24);
			this.cbxFightRangedLTKey.Name = "cbxFightRangedLTKey";
			this.cbxFightRangedLTKey.Size = new System.Drawing.Size(40, 21);
			this.cbxFightRangedLTKey.TabIndex = 42;
			// 
			// cbxFightRangedPTKey
			// 
			this.cbxFightRangedPTKey.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cbxFightRangedPTKey.Items.AddRange(new object[] {
																	 "1",
																	 "2",
																	 "3",
																	 "4",
																	 "5",
																	 "6",
																	 "7",
																	 "8",
																	 "9",
																	 "10"});
			this.cbxFightRangedPTKey.Location = new System.Drawing.Point(224, 48);
			this.cbxFightRangedPTKey.Name = "cbxFightRangedPTKey";
			this.cbxFightRangedPTKey.Size = new System.Drawing.Size(40, 21);
			this.cbxFightRangedPTKey.TabIndex = 43;
			// 
			// label5
			// 
			this.label5.Location = new System.Drawing.Point(232, 4);
			this.label5.Name = "label5";
			this.label5.Size = new System.Drawing.Size(32, 17);
			this.label5.TabIndex = 54;
			this.label5.Text = "Key";
			// 
			// label14
			// 
			this.label14.Location = new System.Drawing.Point(8, 96);
			this.label14.Name = "label14";
			this.label14.Size = new System.Drawing.Size(120, 21);
			this.label14.TabIndex = 53;
			this.label14.Text = "Facilitate Painworking";
			this.label14.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// cbxFightFPKey
			// 
			this.cbxFightFPKey.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cbxFightFPKey.ItemHeight = 13;
			this.cbxFightFPKey.Items.AddRange(new object[] {
															   "1",
															   "2",
															   "3",
															   "4",
															   "5",
															   "6",
															   "7",
															   "8",
															   "9",
															   "10"});
			this.cbxFightFPKey.Location = new System.Drawing.Point(224, 96);
			this.cbxFightFPKey.Name = "cbxFightFPKey";
			this.cbxFightFPKey.Size = new System.Drawing.Size(40, 21);
			this.cbxFightFPKey.TabIndex = 52;
			// 
			// cbxFightFPQ
			// 
			this.cbxFightFPQ.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cbxFightFPQ.ItemHeight = 13;
			this.cbxFightFPQ.Items.AddRange(new object[] {
															 "1",
															 "2",
															 "3",
															 "4",
															 "5",
															 "6",
															 "7",
															 "8",
															 "9",
															 "10"});
			this.cbxFightFPQ.Location = new System.Drawing.Point(160, 96);
			this.cbxFightFPQ.Name = "cbxFightFPQ";
			this.cbxFightFPQ.Size = new System.Drawing.Size(40, 21);
			this.cbxFightFPQ.TabIndex = 52;
			// 
			// panel7
			// 
			this.panel7.Controls.Add(this.label9);
			this.panel7.Dock = System.Windows.Forms.DockStyle.Top;
			this.panel7.Location = new System.Drawing.Point(0, 0);
			this.panel7.Name = "panel7";
			this.panel7.Size = new System.Drawing.Size(327, 16);
			this.panel7.TabIndex = 66;
			// 
			// label9
			// 
			this.label9.BackColor = System.Drawing.SystemColors.ControlDark;
			this.label9.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
			this.label9.Dock = System.Windows.Forms.DockStyle.Fill;
			this.label9.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
			this.label9.Location = new System.Drawing.Point(0, 0);
			this.label9.Name = "label9";
			this.label9.Size = new System.Drawing.Size(327, 16);
			this.label9.TabIndex = 0;
			this.label9.Text = "Fight Keys";
			this.label9.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// tabPage3
			// 
			this.tabPage3.Controls.Add(this.panel12);
			this.tabPage3.Controls.Add(this.panel11);
			this.tabPage3.Location = new System.Drawing.Point(4, 22);
			this.tabPage3.Name = "tabPage3";
			this.tabPage3.Size = new System.Drawing.Size(327, 306);
			this.tabPage3.TabIndex = 2;
			this.tabPage3.Text = "Buffs";
			// 
			// panel12
			// 
			this.panel12.Controls.Add(this.label15);
			this.panel12.Controls.Add(this.cbxBuffDexQ);
			this.panel12.Controls.Add(this.label16);
			this.panel12.Controls.Add(this.cbxBuffAbsorbQ);
			this.panel12.Controls.Add(this.label17);
			this.panel12.Controls.Add(this.cbxBuffDexKey);
			this.panel12.Controls.Add(this.cbxBuffAbsorbKey);
			this.panel12.Controls.Add(this.label19);
			this.panel12.Controls.Add(this.cbxBuffStrengthQ);
			this.panel12.Controls.Add(this.cbxBuffStrengthKey);
			this.panel12.Controls.Add(this.label18);
			this.panel12.Dock = System.Windows.Forms.DockStyle.Top;
			this.panel12.Location = new System.Drawing.Point(0, 16);
			this.panel12.Name = "panel12";
			this.panel12.Size = new System.Drawing.Size(327, 168);
			this.panel12.TabIndex = 7;
			// 
			// label15
			// 
			this.label15.Location = new System.Drawing.Point(168, 8);
			this.label15.Name = "label15";
			this.label15.Size = new System.Drawing.Size(32, 17);
			this.label15.TabIndex = 65;
			this.label15.Text = "QBar";
			// 
			// cbxBuffDexQ
			// 
			this.cbxBuffDexQ.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cbxBuffDexQ.Items.AddRange(new object[] {
															 "1",
															 "2",
															 "3",
															 "4",
															 "5",
															 "6",
															 "7",
															 "8",
															 "9",
															 "10"});
			this.cbxBuffDexQ.Location = new System.Drawing.Point(160, 24);
			this.cbxBuffDexQ.Name = "cbxBuffDexQ";
			this.cbxBuffDexQ.Size = new System.Drawing.Size(40, 21);
			this.cbxBuffDexQ.TabIndex = 55;
			// 
			// label16
			// 
			this.label16.Location = new System.Drawing.Point(8, 48);
			this.label16.Name = "label16";
			this.label16.Size = new System.Drawing.Size(144, 21);
			this.label16.TabIndex = 60;
			this.label16.Text = "Deathsight Absorb";
			this.label16.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// cbxBuffAbsorbQ
			// 
			this.cbxBuffAbsorbQ.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cbxBuffAbsorbQ.Items.AddRange(new object[] {
																"1",
																"2",
																"3",
																"4",
																"5",
																"6",
																"7",
																"8",
																"9",
																"10"});
			this.cbxBuffAbsorbQ.Location = new System.Drawing.Point(160, 48);
			this.cbxBuffAbsorbQ.Name = "cbxBuffAbsorbQ";
			this.cbxBuffAbsorbQ.Size = new System.Drawing.Size(40, 21);
			this.cbxBuffAbsorbQ.TabIndex = 57;
			// 
			// label17
			// 
			this.label17.Location = new System.Drawing.Point(8, 24);
			this.label17.Name = "label17";
			this.label17.Size = new System.Drawing.Size(144, 21);
			this.label17.TabIndex = 59;
			this.label17.Text = "Deathsight Dexterity";
			this.label17.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// cbxBuffDexKey
			// 
			this.cbxBuffDexKey.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cbxBuffDexKey.Items.AddRange(new object[] {
															   "1",
															   "2",
															   "3",
															   "4",
															   "5",
															   "6",
															   "7",
															   "8",
															   "9",
															   "10"});
			this.cbxBuffDexKey.Location = new System.Drawing.Point(224, 24);
			this.cbxBuffDexKey.Name = "cbxBuffDexKey";
			this.cbxBuffDexKey.Size = new System.Drawing.Size(40, 21);
			this.cbxBuffDexKey.TabIndex = 56;
			// 
			// cbxBuffAbsorbKey
			// 
			this.cbxBuffAbsorbKey.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cbxBuffAbsorbKey.Items.AddRange(new object[] {
																  "1",
																  "2",
																  "3",
																  "4",
																  "5",
																  "6",
																  "7",
																  "8",
																  "9",
																  "10"});
			this.cbxBuffAbsorbKey.Location = new System.Drawing.Point(224, 48);
			this.cbxBuffAbsorbKey.Name = "cbxBuffAbsorbKey";
			this.cbxBuffAbsorbKey.Size = new System.Drawing.Size(40, 21);
			this.cbxBuffAbsorbKey.TabIndex = 58;
			// 
			// label19
			// 
			this.label19.Location = new System.Drawing.Point(232, 8);
			this.label19.Name = "label19";
			this.label19.Size = new System.Drawing.Size(32, 17);
			this.label19.TabIndex = 64;
			this.label19.Text = "Key";
			// 
			// cbxBuffStrengthQ
			// 
			this.cbxBuffStrengthQ.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cbxBuffStrengthQ.Items.AddRange(new object[] {
																  "1",
																  "2",
																  "3",
																  "4",
																  "5",
																  "6",
																  "7",
																  "8",
																  "9",
																  "10"});
			this.cbxBuffStrengthQ.Location = new System.Drawing.Point(160, 72);
			this.cbxBuffStrengthQ.Name = "cbxBuffStrengthQ";
			this.cbxBuffStrengthQ.Size = new System.Drawing.Size(40, 21);
			this.cbxBuffStrengthQ.TabIndex = 57;
			// 
			// cbxBuffStrengthKey
			// 
			this.cbxBuffStrengthKey.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cbxBuffStrengthKey.Items.AddRange(new object[] {
																	"1",
																	"2",
																	"3",
																	"4",
																	"5",
																	"6",
																	"7",
																	"8",
																	"9",
																	"10"});
			this.cbxBuffStrengthKey.Location = new System.Drawing.Point(224, 72);
			this.cbxBuffStrengthKey.Name = "cbxBuffStrengthKey";
			this.cbxBuffStrengthKey.Size = new System.Drawing.Size(40, 21);
			this.cbxBuffStrengthKey.TabIndex = 58;
			// 
			// label18
			// 
			this.label18.Location = new System.Drawing.Point(8, 72);
			this.label18.Name = "label18";
			this.label18.Size = new System.Drawing.Size(144, 21);
			this.label18.TabIndex = 60;
			this.label18.Text = "Death Servant Strength";
			this.label18.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// panel11
			// 
			this.panel11.Controls.Add(this.label20);
			this.panel11.Dock = System.Windows.Forms.DockStyle.Top;
			this.panel11.Location = new System.Drawing.Point(0, 0);
			this.panel11.Name = "panel11";
			this.panel11.Size = new System.Drawing.Size(327, 16);
			this.panel11.TabIndex = 6;
			// 
			// label20
			// 
			this.label20.BackColor = System.Drawing.SystemColors.ControlDark;
			this.label20.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
			this.label20.Dock = System.Windows.Forms.DockStyle.Fill;
			this.label20.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
			this.label20.Location = new System.Drawing.Point(0, 0);
			this.label20.Name = "label20";
			this.label20.Size = new System.Drawing.Size(327, 16);
			this.label20.TabIndex = 0;
			this.label20.Text = "Self Buffs";
			this.label20.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// tabPage6
			// 
			this.tabPage6.Controls.Add(this.panel2);
			this.tabPage6.Controls.Add(this.panel1);
			this.tabPage6.Location = new System.Drawing.Point(4, 22);
			this.tabPage6.Name = "tabPage6";
			this.tabPage6.Size = new System.Drawing.Size(327, 306);
			this.tabPage6.TabIndex = 5;
			this.tabPage6.Text = "DeBuffs";
			// 
			// panel2
			// 
			this.panel2.Controls.Add(this.label21);
			this.panel2.Controls.Add(this.cbxDebuffQ);
			this.panel2.Controls.Add(this.label31);
			this.panel2.Controls.Add(this.cbxDebuffKey);
			this.panel2.Controls.Add(this.label32);
			this.panel2.Dock = System.Windows.Forms.DockStyle.Top;
			this.panel2.Location = new System.Drawing.Point(0, 16);
			this.panel2.Name = "panel2";
			this.panel2.Size = new System.Drawing.Size(327, 80);
			this.panel2.TabIndex = 61;
			// 
			// panel1
			// 
			this.panel1.Controls.Add(this.label1);
			this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
			this.panel1.Location = new System.Drawing.Point(0, 0);
			this.panel1.Name = "panel1";
			this.panel1.Size = new System.Drawing.Size(327, 16);
			this.panel1.TabIndex = 60;
			// 
			// label1
			// 
			this.label1.BackColor = System.Drawing.SystemColors.ControlDark;
			this.label1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
			this.label1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.label1.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
			this.label1.Location = new System.Drawing.Point(0, 0);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(327, 16);
			this.label1.TabIndex = 62;
			this.label1.Text = "Misc";
			this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// tabControl1
			// 
			this.tabControl1.Controls.Add(this.tabPage5);
			this.tabControl1.Controls.Add(this.tabPage2);
			this.tabControl1.Controls.Add(this.tabPage6);
			this.tabControl1.Controls.Add(this.tabPage3);
			this.tabControl1.Controls.Add(this.tabPage1);
			this.tabControl1.ItemSize = new System.Drawing.Size(47, 18);
			this.tabControl1.Location = new System.Drawing.Point(0, 32);
			this.tabControl1.Name = "tabControl1";
			this.tabControl1.SelectedIndex = 0;
			this.tabControl1.Size = new System.Drawing.Size(335, 332);
			this.tabControl1.TabIndex = 26;
			// 
			// tabPage1
			// 
			this.tabPage1.Controls.Add(this.nudHealHealth);
			this.tabPage1.Controls.Add(this.panel13);
			this.tabPage1.Controls.Add(this.panel10);
			this.tabPage1.Controls.Add(this.panel9);
			this.tabPage1.Controls.Add(this.label27);
			this.tabPage1.Controls.Add(this.cbxHealPetQ);
			this.tabPage1.Controls.Add(this.label28);
			this.tabPage1.Controls.Add(this.cbxHealPetKey);
			this.tabPage1.Controls.Add(this.label29);
			this.tabPage1.Controls.Add(this.label30);
			this.tabPage1.Location = new System.Drawing.Point(4, 22);
			this.tabPage1.Name = "tabPage1";
			this.tabPage1.Size = new System.Drawing.Size(327, 306);
			this.tabPage1.TabIndex = 6;
			this.tabPage1.Text = "Pet ";
			// 
			// nudHealHealth
			// 
			this.nudHealHealth.Location = new System.Drawing.Point(160, 168);
			this.nudHealHealth.Name = "nudHealHealth";
			this.nudHealHealth.Size = new System.Drawing.Size(40, 20);
			this.nudHealHealth.TabIndex = 71;
			// 
			// panel13
			// 
			this.panel13.Controls.Add(this.label26);
			this.panel13.Dock = System.Windows.Forms.DockStyle.Top;
			this.panel13.Location = new System.Drawing.Point(0, 96);
			this.panel13.Name = "panel13";
			this.panel13.Size = new System.Drawing.Size(327, 16);
			this.panel13.TabIndex = 63;
			// 
			// label26
			// 
			this.label26.BackColor = System.Drawing.SystemColors.ControlDark;
			this.label26.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
			this.label26.Dock = System.Windows.Forms.DockStyle.Fill;
			this.label26.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
			this.label26.Location = new System.Drawing.Point(0, 0);
			this.label26.Name = "label26";
			this.label26.Size = new System.Drawing.Size(327, 16);
			this.label26.TabIndex = 62;
			this.label26.Text = "Heal Pet Key";
			this.label26.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// panel10
			// 
			this.panel10.Controls.Add(this.label23);
			this.panel10.Controls.Add(this.cbxSummonPetQ);
			this.panel10.Controls.Add(this.label24);
			this.panel10.Controls.Add(this.cbxSummonPetKey);
			this.panel10.Controls.Add(this.label25);
			this.panel10.Dock = System.Windows.Forms.DockStyle.Top;
			this.panel10.Location = new System.Drawing.Point(0, 16);
			this.panel10.Name = "panel10";
			this.panel10.Size = new System.Drawing.Size(327, 80);
			this.panel10.TabIndex = 62;
			// 
			// label23
			// 
			this.label23.Location = new System.Drawing.Point(168, 8);
			this.label23.Name = "label23";
			this.label23.Size = new System.Drawing.Size(32, 17);
			this.label23.TabIndex = 70;
			this.label23.Text = "QBar";
			// 
			// cbxSummonPetQ
			// 
			this.cbxSummonPetQ.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cbxSummonPetQ.Items.AddRange(new object[] {
															   "1",
															   "2",
															   "3",
															   "4",
															   "5",
															   "6",
															   "7",
															   "8",
															   "9",
															   "10"});
			this.cbxSummonPetQ.Location = new System.Drawing.Point(160, 24);
			this.cbxSummonPetQ.Name = "cbxSummonPetQ";
			this.cbxSummonPetQ.Size = new System.Drawing.Size(40, 21);
			this.cbxSummonPetQ.TabIndex = 66;
			// 
			// label24
			// 
			this.label24.Location = new System.Drawing.Point(8, 24);
			this.label24.Name = "label24";
			this.label24.Size = new System.Drawing.Size(144, 21);
			this.label24.TabIndex = 68;
			this.label24.Text = "Summon Pet";
			this.label24.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// cbxSummonPetKey
			// 
			this.cbxSummonPetKey.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cbxSummonPetKey.Items.AddRange(new object[] {
																 "1",
																 "2",
																 "3",
																 "4",
																 "5",
																 "6",
																 "7",
																 "8",
																 "9",
																 "10"});
			this.cbxSummonPetKey.Location = new System.Drawing.Point(224, 24);
			this.cbxSummonPetKey.Name = "cbxSummonPetKey";
			this.cbxSummonPetKey.Size = new System.Drawing.Size(40, 21);
			this.cbxSummonPetKey.TabIndex = 67;
			// 
			// label25
			// 
			this.label25.Location = new System.Drawing.Point(232, 8);
			this.label25.Name = "label25";
			this.label25.Size = new System.Drawing.Size(32, 17);
			this.label25.TabIndex = 69;
			this.label25.Text = "Key";
			// 
			// panel9
			// 
			this.panel9.Controls.Add(this.label22);
			this.panel9.Dock = System.Windows.Forms.DockStyle.Top;
			this.panel9.Location = new System.Drawing.Point(0, 0);
			this.panel9.Name = "panel9";
			this.panel9.Size = new System.Drawing.Size(327, 16);
			this.panel9.TabIndex = 61;
			// 
			// label22
			// 
			this.label22.BackColor = System.Drawing.SystemColors.ControlDark;
			this.label22.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
			this.label22.Dock = System.Windows.Forms.DockStyle.Fill;
			this.label22.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
			this.label22.Location = new System.Drawing.Point(0, 0);
			this.label22.Name = "label22";
			this.label22.Size = new System.Drawing.Size(327, 16);
			this.label22.TabIndex = 62;
			this.label22.Text = "Call Pet Key";
			this.label22.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// label27
			// 
			this.label27.Location = new System.Drawing.Point(168, 120);
			this.label27.Name = "label27";
			this.label27.Size = new System.Drawing.Size(32, 17);
			this.label27.TabIndex = 70;
			this.label27.Text = "QBar";
			// 
			// cbxHealPetQ
			// 
			this.cbxHealPetQ.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cbxHealPetQ.Items.AddRange(new object[] {
															 "1",
															 "2",
															 "3",
															 "4",
															 "5",
															 "6",
															 "7",
															 "8",
															 "9",
															 "10"});
			this.cbxHealPetQ.Location = new System.Drawing.Point(160, 136);
			this.cbxHealPetQ.Name = "cbxHealPetQ";
			this.cbxHealPetQ.Size = new System.Drawing.Size(40, 21);
			this.cbxHealPetQ.TabIndex = 66;
			// 
			// label28
			// 
			this.label28.Location = new System.Drawing.Point(8, 136);
			this.label28.Name = "label28";
			this.label28.Size = new System.Drawing.Size(144, 21);
			this.label28.TabIndex = 68;
			this.label28.Text = "Heal Pet";
			this.label28.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// cbxHealPetKey
			// 
			this.cbxHealPetKey.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cbxHealPetKey.Items.AddRange(new object[] {
															   "1",
															   "2",
															   "3",
															   "4",
															   "5",
															   "6",
															   "7",
															   "8",
															   "9",
															   "10"});
			this.cbxHealPetKey.Location = new System.Drawing.Point(224, 136);
			this.cbxHealPetKey.Name = "cbxHealPetKey";
			this.cbxHealPetKey.Size = new System.Drawing.Size(40, 21);
			this.cbxHealPetKey.TabIndex = 67;
			// 
			// label29
			// 
			this.label29.Location = new System.Drawing.Point(232, 120);
			this.label29.Name = "label29";
			this.label29.Size = new System.Drawing.Size(32, 17);
			this.label29.TabIndex = 69;
			this.label29.Text = "Key";
			// 
			// label30
			// 
			this.label30.Location = new System.Drawing.Point(8, 169);
			this.label30.Name = "label30";
			this.label30.Size = new System.Drawing.Size(152, 16);
			this.label30.TabIndex = 68;
			this.label30.Text = "Heal at what health percent?";
			this.label30.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// label21
			// 
			this.label21.Location = new System.Drawing.Point(176, 8);
			this.label21.Name = "label21";
			this.label21.Size = new System.Drawing.Size(32, 17);
			this.label21.TabIndex = 59;
			this.label21.Text = "QBar";
			// 
			// cbxDebuffQ
			// 
			this.cbxDebuffQ.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cbxDebuffQ.Items.AddRange(new object[] {
															"1",
															"2",
															"3",
															"4",
															"5",
															"6",
															"7",
															"8",
															"9",
															"10"});
			this.cbxDebuffQ.Location = new System.Drawing.Point(168, 32);
			this.cbxDebuffQ.Name = "cbxDebuffQ";
			this.cbxDebuffQ.Size = new System.Drawing.Size(40, 21);
			this.cbxDebuffQ.TabIndex = 55;
			// 
			// label31
			// 
			this.label31.Location = new System.Drawing.Point(16, 32);
			this.label31.Name = "label31";
			this.label31.Size = new System.Drawing.Size(104, 21);
			this.label31.TabIndex = 57;
			this.label31.Text = "Instant con debuff";
			this.label31.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// cbxDebuffKey
			// 
			this.cbxDebuffKey.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cbxDebuffKey.Items.AddRange(new object[] {
															  "1",
															  "2",
															  "3",
															  "4",
															  "5",
															  "6",
															  "7",
															  "8",
															  "9",
															  "10"});
			this.cbxDebuffKey.Location = new System.Drawing.Point(232, 32);
			this.cbxDebuffKey.Name = "cbxDebuffKey";
			this.cbxDebuffKey.Size = new System.Drawing.Size(40, 21);
			this.cbxDebuffKey.TabIndex = 56;
			// 
			// label32
			// 
			this.label32.Location = new System.Drawing.Point(240, 8);
			this.label32.Name = "label32";
			this.label32.Size = new System.Drawing.Size(32, 17);
			this.label32.TabIndex = 58;
			this.label32.Text = "Key";
			// 
			// frmNecroSettings
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.ClientSize = new System.Drawing.Size(343, 392);
			this.Controls.Add(this.panel6);
			this.Controls.Add(this.panel3);
			this.Controls.Add(this.tabControl1);
			this.DockPadding.All = 4;
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
			this.Name = "frmNecroSettings";
			this.Text = "frmNecro";
			this.panel3.ResumeLayout(false);
			this.panel6.ResumeLayout(false);
			this.tabPage5.ResumeLayout(false);
			this.panel16.ResumeLayout(false);
			this.panel15.ResumeLayout(false);
			this.tabPage2.ResumeLayout(false);
			this.panel5.ResumeLayout(false);
			this.panel4.ResumeLayout(false);
			this.panel8.ResumeLayout(false);
			this.panel7.ResumeLayout(false);
			this.tabPage3.ResumeLayout(false);
			this.panel12.ResumeLayout(false);
			this.panel11.ResumeLayout(false);
			this.tabPage6.ResumeLayout(false);
			this.panel2.ResumeLayout(false);
			this.panel1.ResumeLayout(false);
			this.tabControl1.ResumeLayout(false);
			this.tabPage1.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.nudHealHealth)).EndInit();
			this.panel13.ResumeLayout(false);
			this.panel10.ResumeLayout(false);
			this.panel9.ResumeLayout(false);
			this.ResumeLayout(false);

		}
		#endregion

		AutoKillerScript.clsAutoKillerScript _ak;
		private System.Windows.Forms.Panel panel3;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.Panel panel6;
		private System.Windows.Forms.Button btnCancel;
		private System.Windows.Forms.Button btnApply;
		private System.Windows.Forms.TabPage tabPage5;
		private System.Windows.Forms.Panel panel16;
		private System.Windows.Forms.Panel panel15;
		private System.Windows.Forms.Label label4;
		private System.Windows.Forms.TabPage tabPage2;
		private System.Windows.Forms.Panel panel8;
		private System.Windows.Forms.ComboBox cbxFightRangedLTQ;
		private System.Windows.Forms.Label lblRangedPowerTap;
		private System.Windows.Forms.ComboBox cbxFightRangedPTQ;
		private System.Windows.Forms.Label lblRangedLifeTap;
		private System.Windows.Forms.ComboBox cbxFightMeleePTQ;
		private System.Windows.Forms.Label lblMeleePowerTap;
		private System.Windows.Forms.Panel panel7;
		private System.Windows.Forms.Label label9;
		private System.Windows.Forms.TabPage tabPage3;
		private System.Windows.Forms.Panel panel12;
		private System.Windows.Forms.Panel panel11;
		private System.Windows.Forms.Label label20;
		private System.Windows.Forms.TabPage tabPage6;
		private System.Windows.Forms.Panel panel2;
		private System.Windows.Forms.Panel panel1;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.TabControl tabControl1;
		private System.Windows.Forms.ComboBox cbxFightMeleePTKey;
		private System.Windows.Forms.ComboBox cbxFightRangedLTKey;
		private System.Windows.Forms.ComboBox cbxFightRangedPTKey;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.Label label5;
		private System.Windows.Forms.Panel panel4;
		private System.Windows.Forms.Label label6;
		private System.Windows.Forms.Panel panel5;
		private System.Windows.Forms.Label label7;
		private System.Windows.Forms.ComboBox cbxPetPassiveQ;
		private System.Windows.Forms.Label label8;
		private System.Windows.Forms.ComboBox cbxPetDefensiveQ;
		private System.Windows.Forms.Label label10;
		private System.Windows.Forms.ComboBox cbxPetFollowQ;
		private System.Windows.Forms.Label label11;
		private System.Windows.Forms.ComboBox cbxPetFollowKey;
		private System.Windows.Forms.ComboBox cbxPetPassiveKey;
		private System.Windows.Forms.ComboBox cbxPetDefensiveKey;
		private System.Windows.Forms.Label label12;
		private System.Windows.Forms.ComboBox cbxPetHereQ;
		private System.Windows.Forms.Label label13;
		private System.Windows.Forms.ComboBox cbxPetHereKey;
		private System.Windows.Forms.Label label14;
		private System.Windows.Forms.ComboBox cbxFightFPKey;
		private System.Windows.Forms.ComboBox cbxFightFPQ;
		private System.Windows.Forms.Label label15;
		private System.Windows.Forms.ComboBox cbxBuffDexQ;
		private System.Windows.Forms.Label label16;
		private System.Windows.Forms.ComboBox cbxBuffAbsorbQ;
		private System.Windows.Forms.Label label17;
		private System.Windows.Forms.ComboBox cbxBuffDexKey;
		private System.Windows.Forms.ComboBox cbxBuffAbsorbKey;
		private System.Windows.Forms.Label label19;
		private System.Windows.Forms.ComboBox cbxBuffStrengthQ;
		private System.Windows.Forms.ComboBox cbxBuffStrengthKey;
		private System.Windows.Forms.Label label18;
		private System.Windows.Forms.TabPage tabPage1;
		private System.Windows.Forms.Panel panel9;
		private System.Windows.Forms.Label label22;
		private System.Windows.Forms.Panel panel10;
		private System.Windows.Forms.Label label23;
		private System.Windows.Forms.ComboBox cbxSummonPetQ;
		private System.Windows.Forms.Label label24;
		private System.Windows.Forms.ComboBox cbxSummonPetKey;
		private System.Windows.Forms.Label label25;
		private System.Windows.Forms.Panel panel13;
		private System.Windows.Forms.Label label26;
		private System.Windows.Forms.Label label27;
		private System.Windows.Forms.ComboBox cbxHealPetQ;
		private System.Windows.Forms.Label label28;
		private System.Windows.Forms.ComboBox cbxHealPetKey;
		private System.Windows.Forms.Label label29;
		private System.Windows.Forms.CheckBox chkFleeLeavePet;
		private System.Windows.Forms.CheckBox chkUseFP;
		private System.Windows.Forms.Label label30;
		private System.Windows.Forms.Label label21;
		private System.Windows.Forms.ComboBox cbxDebuffQ;
		private System.Windows.Forms.Label label31;
		private System.Windows.Forms.ComboBox cbxDebuffKey;
		private System.Windows.Forms.Label label32;
		private System.Windows.Forms.NumericUpDown nudHealHealth;


		public void InitializeForm(Profile profile)
		{
			
		}

		#region IProfileForm handling routines
		public event UpdateVariableDelegate UpdateVariable;

		public event CurrentProfileDelegate LoadCurrentProfile; // This event is never used
		public event CurrentProfileDelegate SaveCurrentProfile;

		public void DefineVariables( Profile profile)
		{
			// Flee tab
			profile.DefineVariable( "Necro.FleeLeavePet", false);

			//Fight tab
			profile.DefineVariable( "Necro.FightRangedLTQ", "");
			profile.DefineVariable( "Necro.FightRangedLTKey", "");

			profile.DefineVariable( "Necro.FightRangedPTQ", "");
			profile.DefineVariable( "Necro.FightRangedPTKey", "");

			profile.DefineVariable( "Necro.FightMeleePTQ", "");
			profile.DefineVariable( "Necro.FightMeleePTKey", "");

			profile.DefineVariable( "Necro.FightFPQ", "");
			profile.DefineVariable( "Necro.FightFPKey", "");

			profile.DefineVariable( "Necro.UseFP", false);

			profile.DefineVariable( "Necro.PetPassiveQ", "");
			profile.DefineVariable( "Necro.PetPassiveKey", "");

			profile.DefineVariable( "Necro.PetDefensiveQ", "");
			profile.DefineVariable( "Necro.PetDefensiveKey", "");

			profile.DefineVariable( "Necro.PetFollowQ", "");
			profile.DefineVariable( "Necro.PetFollowKey", "");

			profile.DefineVariable( "Necro.PetHereQ", "");
			profile.DefineVariable( "Necro.PetHereKey", "");

			//Buffs tab
			profile.DefineVariable( "Necro.BuffDexQ", "");
			profile.DefineVariable( "Necro.BuffDexKey", "");

			profile.DefineVariable( "Necro.BuffAbsorbQ", "");
			profile.DefineVariable( "Necro.BuffAbsorbKey", "");

			profile.DefineVariable( "Necro.BuffStrengthQ", "");
			profile.DefineVariable( "Necro.BuffStrengthKey", "");

			//DeBuffs tab
			profile.DefineVariable( "Necro.DebuffQ", "");
			profile.DefineVariable( "Necro.DebuffKey", "");

			//Pet tab
			profile.DefineVariable( "Necro.SummonPetQ", "");
			profile.DefineVariable( "Necro.SummonPetKey", "");

			profile.DefineVariable( "Necro.HealPetQ", "");
			profile.DefineVariable( "Necro.HealPetKey", "");

			profile.DefineVariable( "Necro.HealHealth", (int)40);


		}

		public void OnProfileChange( Profile profile)
		{
			// Flee tab
			chkFleeLeavePet.Checked = profile.GetBool("Necro.FleeLeavePet");

			//Fight tab
			SetComboBox("", profile.GetString("Necro.FightRangedLTQ"), ref cbxFightRangedLTQ , false, profile);
			SetComboBox("", profile.GetString("Necro.FightRangedLTKey"), ref cbxFightRangedLTKey , false, profile);

			SetComboBox("", profile.GetString("Necro.FightRangedPTQ"), ref cbxFightRangedPTQ , false, profile);
			SetComboBox("", profile.GetString("Necro.FightRangedPTKey"), ref cbxFightRangedPTKey  , false, profile);

			SetComboBox("", profile.GetString("Necro.FightMeleePTQ"), ref cbxFightMeleePTQ , false, profile);
			SetComboBox("", profile.GetString("Necro.FightMeleePTKey"), ref cbxFightMeleePTKey , false, profile);

			SetComboBox("", profile.GetString("Necro.FightFPQ"), ref cbxFightFPQ , false, profile);
			SetComboBox("", profile.GetString("Necro.FightFPKey"), ref cbxFightFPKey , false, profile);

			chkUseFP.Checked = profile.GetBool("Necro.UseFP");

			SetComboBox("", profile.GetString("Necro.PetPassiveQ"), ref cbxPetPassiveQ , false, profile);
			SetComboBox("", profile.GetString("Necro.PetPassiveKey"), ref cbxPetPassiveKey , false, profile);

			SetComboBox("", profile.GetString("Necro.PetDefensiveQ"), ref cbxPetDefensiveQ , false, profile);
			SetComboBox("", profile.GetString("Necro.PetDefensiveKey"), ref cbxPetDefensiveKey , false, profile);

			SetComboBox("", profile.GetString("Necro.PetFollowQ"), ref cbxPetFollowQ , false, profile);
			SetComboBox("", profile.GetString("Necro.PetFollowKey"), ref cbxPetFollowKey , false, profile);

			SetComboBox("", profile.GetString("Necro.PetHereQ"), ref cbxPetHereQ , false, profile);
			SetComboBox("", profile.GetString("Necro.PetHereKey"), ref cbxPetHereKey , false, profile);

			//Buffs tab
			SetComboBox("", profile.GetString("Necro.BuffDexQ"), ref cbxBuffDexQ , false, profile);
			SetComboBox("", profile.GetString("Necro.BuffDexKey"), ref cbxBuffDexKey , false, profile);

			SetComboBox("", profile.GetString("Necro.BuffAbsorbQ"), ref cbxBuffAbsorbQ , false, profile);
			SetComboBox("", profile.GetString("Necro.BuffAbsorbKey"), ref cbxBuffAbsorbKey , false, profile);

			SetComboBox("", profile.GetString("Necro.BuffStrengthQ"), ref cbxBuffStrengthQ , false, profile);
			SetComboBox("", profile.GetString("Necro.BuffStrengthKey"), ref cbxBuffStrengthKey , false, profile);

			//DeBuffs tab
			SetComboBox("", profile.GetString("Necro.DebuffQ"), ref cbxDebuffQ , false, profile);
			SetComboBox("", profile.GetString("Necro.DebuffKey"), ref cbxDebuffKey , false, profile);

			//Pet tab
			SetComboBox("", profile.GetString("Necro.SummonPetQ"), ref cbxSummonPetQ , false, profile);
			SetComboBox("", profile.GetString("Necro.SummonPetKey"), ref cbxSummonPetKey , false, profile);

			SetComboBox("", profile.GetString("Necro.HealPetQ"), ref cbxHealPetQ , false, profile);
			SetComboBox("", profile.GetString("Necro.HealPetKey"), ref cbxHealPetKey , false, profile);

			nudHealHealth.Value = profile.GetInteger("Necro.HealHealth");
		}		

		internal void SetComboBox(string variableName, string itemtext, ref ComboBox theBox, bool AddItem, Profile profile)
		{
			if(itemtext != null && itemtext != "")
				if(theBox.Items.Contains(itemtext))
				{
					theBox.SelectedIndex = theBox.Items.IndexOf(itemtext);
				}
				else if(AddItem)
				{
					theBox.Items.Add(itemtext);
					theBox.SelectedIndex = theBox.Items.IndexOf(itemtext);
				}
				else
				{
					//					profile.ResetValue( variableName);
					theBox.SelectedIndex = -1;
					theBox.Text = "";
				}
		}
		internal void SaveComboBox(string variableName, ref ComboBox theBox, Profile profile)
		{
			if(theBox.SelectedIndex == -1)
			{
				UpdateVariable( variableName, "");
				return;
			}

			UpdateVariable( variableName, theBox.SelectedItem.ToString());
		}

		#endregion


		private void btnApply_Click(object sender, System.EventArgs e)
		{
			frmMain frmmain = (frmMain) MdiParent;
			Profile profile = 	frmmain._profile;

			if( UpdateVariable != null)
			{
				// Flee tab
				UpdateVariable( "Necro.FleeLeavePet", chkFleeLeavePet.Checked );

				//Fight tab
				SaveComboBox( "Necro.FightRangedLTQ", ref cbxFightRangedLTQ , profile);
				SaveComboBox( "Necro.FightRangedLTKey", ref cbxFightRangedLTKey , profile);

				SaveComboBox( "Necro.FightRangedPTQ", ref cbxFightRangedPTQ , profile);
				SaveComboBox( "Necro.FightRangedPTKey", ref cbxFightRangedPTKey  , profile);

				SaveComboBox( "Necro.FightMeleePTQ", ref cbxFightMeleePTQ , profile);
				SaveComboBox( "Necro.FightMeleePTKey", ref cbxFightMeleePTKey , profile);

				SaveComboBox( "Necro.FightFPQ", ref cbxFightFPQ , profile);
				SaveComboBox( "Necro.FightFPKey", ref cbxFightFPKey , profile);

				UpdateVariable( "Necro.UseFP", chkUseFP.Checked );

				SaveComboBox( "Necro.PetPassiveQ", ref cbxPetPassiveQ , profile);
				SaveComboBox( "Necro.PetPassiveKey", ref cbxPetPassiveKey , profile);

				SaveComboBox( "Necro.PetDefensiveQ", ref cbxPetDefensiveQ , profile);
				SaveComboBox( "Necro.PetDefensiveKey", ref cbxPetDefensiveKey , profile);

				SaveComboBox( "Necro.PetFollowQ", ref cbxPetFollowQ , profile);
				SaveComboBox( "Necro.PetFollowKey", ref cbxPetFollowKey , profile);

				SaveComboBox( "Necro.PetHereQ", ref cbxPetHereQ , profile);
				SaveComboBox( "Necro.PetHereKey", ref cbxPetHereKey , profile);

				//Buffs tab
				SaveComboBox( "Necro.BuffDexQ", ref cbxBuffDexQ , profile);
				SaveComboBox( "Necro.BuffDexKey", ref cbxBuffDexKey , profile);

				SaveComboBox( "Necro.BuffAbsorbQ", ref cbxBuffAbsorbQ , profile);
				SaveComboBox( "Necro.BuffAbsorbKey", ref cbxBuffAbsorbKey , profile);

				SaveComboBox( "Necro.BuffStrengthQ", ref cbxBuffStrengthQ , profile);
				SaveComboBox( "Necro.BuffStrengthKey", ref cbxBuffStrengthKey , profile);

				//DeBuffs tab
				SaveComboBox( "Necro.DebuffQ", ref cbxDebuffQ , profile);
				SaveComboBox( "Necro.DebuffKey", ref cbxDebuffKey , profile);

				SaveComboBox( "Necro.DebuffQ", ref cbxDebuffQ , profile);
				SaveComboBox( "Necro.DebuffKey", ref cbxDebuffKey , profile);

				//Pet tab
				SaveComboBox( "Necro.SummonPetQ", ref cbxSummonPetQ , profile);
				SaveComboBox( "Necro.SummonPetKey", ref cbxSummonPetKey , profile);

				SaveComboBox( "Necro.HealPetQ", ref cbxHealPetQ , profile);
				SaveComboBox( "Necro.HealPetKey", ref cbxHealPetKey , profile);

				UpdateVariable( "Necro.HealHealth", (int)nudHealHealth.Value );

				
			}

			if( SaveCurrentProfile != null)
				SaveCurrentProfile();
		}

		private void btnCancel_Click(object sender, System.EventArgs e)
		{
			if( LoadCurrentProfile != null)
				LoadCurrentProfile();
		}
	}
}
