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
	/// Summary description for frmScout.
	/// </summary>
	public class frmScoutSettings : System.Windows.Forms.Form, IProfileForm
	{
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		public frmScoutSettings(AutoKillerScript.clsAutoKillerScript ak)
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
			this.chkStealthResting = new System.Windows.Forms.CheckBox();
			this.panel15 = new System.Windows.Forms.Panel();
			this.label4 = new System.Windows.Forms.Label();
			this.tabPage2 = new System.Windows.Forms.TabPage();
			this.panel5 = new System.Windows.Forms.Panel();
			this.panel4 = new System.Windows.Forms.Panel();
			this.label6 = new System.Windows.Forms.Label();
			this.panel8 = new System.Windows.Forms.Panel();
			this.label3 = new System.Windows.Forms.Label();
			this.cbxFightRangedCritQ = new System.Windows.Forms.ComboBox();
			this.lblRangedPowerTap = new System.Windows.Forms.Label();
			this.cbxFightRangedBowQ = new System.Windows.Forms.ComboBox();
			this.lblCrit = new System.Windows.Forms.Label();
			this.cbxFightRangedRFQ = new System.Windows.Forms.ComboBox();
			this.lblMeleePowerTap = new System.Windows.Forms.Label();
			this.cbxFightRangedRFKey = new System.Windows.Forms.ComboBox();
			this.cbxFightRangedCritKey = new System.Windows.Forms.ComboBox();
			this.cbxFightRangedBowKey = new System.Windows.Forms.ComboBox();
			this.label5 = new System.Windows.Forms.Label();
			this.panel7 = new System.Windows.Forms.Panel();
			this.label9 = new System.Windows.Forms.Label();
			this.tabControl1 = new System.Windows.Forms.TabControl();
			this.chkStealthAlways = new System.Windows.Forms.CheckBox();
			this.tabControl2 = new System.Windows.Forms.TabControl();
			this.tabPage4 = new System.Windows.Forms.TabPage();
			this.tabPage7 = new System.Windows.Forms.TabPage();
			this.label13 = new System.Windows.Forms.Label();
			this.label14 = new System.Windows.Forms.Label();
			this.cbxSlashNormalKey = new System.Windows.Forms.ComboBox();
			this.cbxSlashBlockKey = new System.Windows.Forms.ComboBox();
			this.cbxSlashNormalQ = new System.Windows.Forms.ComboBox();
			this.label32 = new System.Windows.Forms.Label();
			this.cbxSlashBlockQ = new System.Windows.Forms.ComboBox();
			this.label33 = new System.Windows.Forms.Label();
			this.label34 = new System.Windows.Forms.Label();
			this.label35 = new System.Windows.Forms.Label();
			this.cbxSlashBlockChainQ = new System.Windows.Forms.ComboBox();
			this.cbxSlashBlockChainKey = new System.Windows.Forms.ComboBox();
			this.label36 = new System.Windows.Forms.Label();
			this.label7 = new System.Windows.Forms.Label();
			this.label8 = new System.Windows.Forms.Label();
			this.comboBox1 = new System.Windows.Forms.ComboBox();
			this.comboBox2 = new System.Windows.Forms.ComboBox();
			this.comboBox8 = new System.Windows.Forms.ComboBox();
			this.label10 = new System.Windows.Forms.Label();
			this.comboBox10 = new System.Windows.Forms.ComboBox();
			this.label11 = new System.Windows.Forms.Label();
			this.label12 = new System.Windows.Forms.Label();
			this.label31 = new System.Windows.Forms.Label();
			this.comboBox11 = new System.Windows.Forms.ComboBox();
			this.comboBox12 = new System.Windows.Forms.ComboBox();
			this.label37 = new System.Windows.Forms.Label();
			this.cbxSlashNormalChainQ = new System.Windows.Forms.ComboBox();
			this.cbxSlashNormalChainKey = new System.Windows.Forms.ComboBox();
			this.label1 = new System.Windows.Forms.Label();
			this.cbxStealthQ = new System.Windows.Forms.ComboBox();
			this.label15 = new System.Windows.Forms.Label();
			this.cbxStealthKey = new System.Windows.Forms.ComboBox();
			this.label16 = new System.Windows.Forms.Label();
			this.cbxMeleeWeaponKey = new System.Windows.Forms.ComboBox();
			this.cbxMeleeWeaponQ = new System.Windows.Forms.ComboBox();
			this.label17 = new System.Windows.Forms.Label();
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
			this.tabControl1.SuspendLayout();
			this.tabControl2.SuspendLayout();
			this.tabPage4.SuspendLayout();
			this.tabPage7.SuspendLayout();
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
			this.panel3.Size = new System.Drawing.Size(328, 24);
			this.panel3.TabIndex = 14;
			// 
			// label2
			// 
			this.label2.Dock = System.Windows.Forms.DockStyle.Fill;
			this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.label2.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
			this.label2.Location = new System.Drawing.Point(0, 0);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(324, 20);
			this.label2.TabIndex = 0;
			this.label2.Text = "Scout Settings";
			this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// panel6
			// 
			this.panel6.Controls.Add(this.btnCancel);
			this.panel6.Controls.Add(this.btnApply);
			this.panel6.Dock = System.Windows.Forms.DockStyle.Bottom;
			this.panel6.Location = new System.Drawing.Point(4, 364);
			this.panel6.Name = "panel6";
			this.panel6.Size = new System.Drawing.Size(328, 24);
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
			this.tabPage5.Text = "Stealth";
			// 
			// panel16
			// 
			this.panel16.Controls.Add(this.label1);
			this.panel16.Controls.Add(this.cbxStealthQ);
			this.panel16.Controls.Add(this.label15);
			this.panel16.Controls.Add(this.cbxStealthKey);
			this.panel16.Controls.Add(this.label16);
			this.panel16.Controls.Add(this.chkStealthResting);
			this.panel16.Controls.Add(this.chkStealthAlways);
			this.panel16.Dock = System.Windows.Forms.DockStyle.Top;
			this.panel16.Location = new System.Drawing.Point(0, 16);
			this.panel16.Name = "panel16";
			this.panel16.Size = new System.Drawing.Size(327, 128);
			this.panel16.TabIndex = 1;
			// 
			// chkStealthResting
			// 
			this.chkStealthResting.Location = new System.Drawing.Point(64, 16);
			this.chkStealthResting.Name = "chkStealthResting";
			this.chkStealthResting.Size = new System.Drawing.Size(144, 24);
			this.chkStealthResting.TabIndex = 2;
			this.chkStealthResting.Text = "Stealth while resting";
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
			this.label4.Text = "Stealth";
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
			this.panel5.Controls.Add(this.tabControl2);
			this.panel5.Dock = System.Windows.Forms.DockStyle.Top;
			this.panel5.Location = new System.Drawing.Point(0, 160);
			this.panel5.Name = "panel5";
			this.panel5.Size = new System.Drawing.Size(327, 128);
			this.panel5.TabIndex = 69;
			// 
			// panel4
			// 
			this.panel4.Controls.Add(this.label6);
			this.panel4.Dock = System.Windows.Forms.DockStyle.Top;
			this.panel4.Location = new System.Drawing.Point(0, 144);
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
			this.label6.Text = "Melee Keys";
			this.label6.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// panel8
			// 
			this.panel8.Controls.Add(this.label3);
			this.panel8.Controls.Add(this.cbxFightRangedCritQ);
			this.panel8.Controls.Add(this.lblRangedPowerTap);
			this.panel8.Controls.Add(this.cbxFightRangedBowQ);
			this.panel8.Controls.Add(this.lblCrit);
			this.panel8.Controls.Add(this.cbxFightRangedRFQ);
			this.panel8.Controls.Add(this.lblMeleePowerTap);
			this.panel8.Controls.Add(this.cbxFightRangedRFKey);
			this.panel8.Controls.Add(this.cbxFightRangedCritKey);
			this.panel8.Controls.Add(this.cbxFightRangedBowKey);
			this.panel8.Controls.Add(this.label5);
			this.panel8.Controls.Add(this.cbxMeleeWeaponKey);
			this.panel8.Controls.Add(this.cbxMeleeWeaponQ);
			this.panel8.Controls.Add(this.label17);
			this.panel8.Dock = System.Windows.Forms.DockStyle.Top;
			this.panel8.Location = new System.Drawing.Point(0, 16);
			this.panel8.Name = "panel8";
			this.panel8.Size = new System.Drawing.Size(327, 128);
			this.panel8.TabIndex = 67;
			// 
			// label3
			// 
			this.label3.Location = new System.Drawing.Point(168, 4);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(32, 17);
			this.label3.TabIndex = 54;
			this.label3.Text = "QBar";
			// 
			// cbxFightRangedCritQ
			// 
			this.cbxFightRangedCritQ.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cbxFightRangedCritQ.Items.AddRange(new object[] {
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
			this.cbxFightRangedCritQ.Location = new System.Drawing.Point(160, 24);
			this.cbxFightRangedCritQ.Name = "cbxFightRangedCritQ";
			this.cbxFightRangedCritQ.Size = new System.Drawing.Size(40, 21);
			this.cbxFightRangedCritQ.TabIndex = 42;
			// 
			// lblRangedPowerTap
			// 
			this.lblRangedPowerTap.Location = new System.Drawing.Point(8, 48);
			this.lblRangedPowerTap.Name = "lblRangedPowerTap";
			this.lblRangedPowerTap.Size = new System.Drawing.Size(104, 21);
			this.lblRangedPowerTap.TabIndex = 50;
			this.lblRangedPowerTap.Text = "Bow";
			this.lblRangedPowerTap.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// cbxFightRangedBowQ
			// 
			this.cbxFightRangedBowQ.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cbxFightRangedBowQ.Items.AddRange(new object[] {
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
			this.cbxFightRangedBowQ.Location = new System.Drawing.Point(160, 48);
			this.cbxFightRangedBowQ.Name = "cbxFightRangedBowQ";
			this.cbxFightRangedBowQ.Size = new System.Drawing.Size(40, 21);
			this.cbxFightRangedBowQ.TabIndex = 43;
			// 
			// lblCrit
			// 
			this.lblCrit.Location = new System.Drawing.Point(8, 24);
			this.lblCrit.Name = "lblCrit";
			this.lblCrit.Size = new System.Drawing.Size(104, 21);
			this.lblCrit.TabIndex = 48;
			this.lblCrit.Text = "Critical Shot";
			this.lblCrit.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// cbxFightRangedRFQ
			// 
			this.cbxFightRangedRFQ.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cbxFightRangedRFQ.ItemHeight = 13;
			this.cbxFightRangedRFQ.Items.AddRange(new object[] {
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
			this.cbxFightRangedRFQ.Location = new System.Drawing.Point(160, 72);
			this.cbxFightRangedRFQ.Name = "cbxFightRangedRFQ";
			this.cbxFightRangedRFQ.Size = new System.Drawing.Size(40, 21);
			this.cbxFightRangedRFQ.TabIndex = 52;
			// 
			// lblMeleePowerTap
			// 
			this.lblMeleePowerTap.Location = new System.Drawing.Point(8, 72);
			this.lblMeleePowerTap.Name = "lblMeleePowerTap";
			this.lblMeleePowerTap.Size = new System.Drawing.Size(104, 21);
			this.lblMeleePowerTap.TabIndex = 53;
			this.lblMeleePowerTap.Text = "Rapid Fire";
			this.lblMeleePowerTap.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// cbxFightRangedRFKey
			// 
			this.cbxFightRangedRFKey.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cbxFightRangedRFKey.ItemHeight = 13;
			this.cbxFightRangedRFKey.Items.AddRange(new object[] {
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
			this.cbxFightRangedRFKey.Location = new System.Drawing.Point(224, 72);
			this.cbxFightRangedRFKey.Name = "cbxFightRangedRFKey";
			this.cbxFightRangedRFKey.Size = new System.Drawing.Size(40, 21);
			this.cbxFightRangedRFKey.TabIndex = 52;
			// 
			// cbxFightRangedCritKey
			// 
			this.cbxFightRangedCritKey.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cbxFightRangedCritKey.Items.AddRange(new object[] {
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
			this.cbxFightRangedCritKey.Location = new System.Drawing.Point(224, 24);
			this.cbxFightRangedCritKey.Name = "cbxFightRangedCritKey";
			this.cbxFightRangedCritKey.Size = new System.Drawing.Size(40, 21);
			this.cbxFightRangedCritKey.TabIndex = 42;
			// 
			// cbxFightRangedBowKey
			// 
			this.cbxFightRangedBowKey.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cbxFightRangedBowKey.Items.AddRange(new object[] {
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
			this.cbxFightRangedBowKey.Location = new System.Drawing.Point(224, 48);
			this.cbxFightRangedBowKey.Name = "cbxFightRangedBowKey";
			this.cbxFightRangedBowKey.Size = new System.Drawing.Size(40, 21);
			this.cbxFightRangedBowKey.TabIndex = 43;
			// 
			// label5
			// 
			this.label5.Location = new System.Drawing.Point(232, 4);
			this.label5.Name = "label5";
			this.label5.Size = new System.Drawing.Size(32, 17);
			this.label5.TabIndex = 54;
			this.label5.Text = "Key";
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
			this.label9.Text = "Ranged Fight Keys";
			this.label9.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// tabControl1
			// 
			this.tabControl1.Controls.Add(this.tabPage5);
			this.tabControl1.Controls.Add(this.tabPage2);
			this.tabControl1.ItemSize = new System.Drawing.Size(47, 18);
			this.tabControl1.Location = new System.Drawing.Point(0, 32);
			this.tabControl1.Name = "tabControl1";
			this.tabControl1.SelectedIndex = 0;
			this.tabControl1.Size = new System.Drawing.Size(335, 332);
			this.tabControl1.TabIndex = 26;
			// 
			// chkStealthAlways
			// 
			this.chkStealthAlways.Location = new System.Drawing.Point(64, 40);
			this.chkStealthAlways.Name = "chkStealthAlways";
			this.chkStealthAlways.Size = new System.Drawing.Size(192, 24);
			this.chkStealthAlways.TabIndex = 2;
			this.chkStealthAlways.Text = "Stealth while approaching target";
			// 
			// tabControl2
			// 
			this.tabControl2.Controls.Add(this.tabPage4);
			this.tabControl2.Controls.Add(this.tabPage7);
			this.tabControl2.Location = new System.Drawing.Point(8, 8);
			this.tabControl2.Name = "tabControl2";
			this.tabControl2.SelectedIndex = 0;
			this.tabControl2.Size = new System.Drawing.Size(312, 144);
			this.tabControl2.TabIndex = 27;
			// 
			// tabPage4
			// 
			this.tabPage4.Controls.Add(this.label13);
			this.tabPage4.Controls.Add(this.label14);
			this.tabPage4.Controls.Add(this.cbxSlashNormalKey);
			this.tabPage4.Controls.Add(this.cbxSlashBlockKey);
			this.tabPage4.Controls.Add(this.cbxSlashNormalQ);
			this.tabPage4.Controls.Add(this.label32);
			this.tabPage4.Controls.Add(this.cbxSlashBlockQ);
			this.tabPage4.Controls.Add(this.label33);
			this.tabPage4.Controls.Add(this.label34);
			this.tabPage4.Controls.Add(this.label35);
			this.tabPage4.Controls.Add(this.cbxSlashBlockChainQ);
			this.tabPage4.Controls.Add(this.cbxSlashBlockChainKey);
			this.tabPage4.Controls.Add(this.label36);
			this.tabPage4.Controls.Add(this.cbxSlashNormalChainQ);
			this.tabPage4.Controls.Add(this.cbxSlashNormalChainKey);
			this.tabPage4.Location = new System.Drawing.Point(4, 22);
			this.tabPage4.Name = "tabPage4";
			this.tabPage4.Size = new System.Drawing.Size(304, 118);
			this.tabPage4.TabIndex = 0;
			this.tabPage4.Text = "Slash";
			// 
			// tabPage7
			// 
			this.tabPage7.Controls.Add(this.label7);
			this.tabPage7.Controls.Add(this.label8);
			this.tabPage7.Controls.Add(this.comboBox1);
			this.tabPage7.Controls.Add(this.comboBox2);
			this.tabPage7.Controls.Add(this.comboBox8);
			this.tabPage7.Controls.Add(this.label10);
			this.tabPage7.Controls.Add(this.comboBox10);
			this.tabPage7.Controls.Add(this.label11);
			this.tabPage7.Controls.Add(this.label12);
			this.tabPage7.Controls.Add(this.label31);
			this.tabPage7.Controls.Add(this.comboBox11);
			this.tabPage7.Controls.Add(this.comboBox12);
			this.tabPage7.Controls.Add(this.label37);
			this.tabPage7.Location = new System.Drawing.Point(4, 22);
			this.tabPage7.Name = "tabPage7";
			this.tabPage7.Size = new System.Drawing.Size(304, 118);
			this.tabPage7.TabIndex = 1;
			this.tabPage7.Text = "Thrust (not done)";
			// 
			// label13
			// 
			this.label13.Location = new System.Drawing.Point(16, 35);
			this.label13.Name = "label13";
			this.label13.Size = new System.Drawing.Size(72, 21);
			this.label13.TabIndex = 59;
			this.label13.Text = "Normal Style";
			this.label13.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// label14
			// 
			this.label14.Location = new System.Drawing.Point(152, 18);
			this.label14.Name = "label14";
			this.label14.Size = new System.Drawing.Size(32, 13);
			this.label14.TabIndex = 65;
			this.label14.Text = "Key";
			// 
			// cbxSlashNormalKey
			// 
			this.cbxSlashNormalKey.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cbxSlashNormalKey.Items.AddRange(new object[] {
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
			this.cbxSlashNormalKey.Location = new System.Drawing.Point(144, 35);
			this.cbxSlashNormalKey.Name = "cbxSlashNormalKey";
			this.cbxSlashNormalKey.Size = new System.Drawing.Size(40, 21);
			this.cbxSlashNormalKey.TabIndex = 55;
			// 
			// cbxSlashBlockKey
			// 
			this.cbxSlashBlockKey.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cbxSlashBlockKey.Items.AddRange(new object[] {
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
			this.cbxSlashBlockKey.Location = new System.Drawing.Point(144, 59);
			this.cbxSlashBlockKey.Name = "cbxSlashBlockKey";
			this.cbxSlashBlockKey.Size = new System.Drawing.Size(40, 21);
			this.cbxSlashBlockKey.TabIndex = 57;
			// 
			// cbxSlashNormalQ
			// 
			this.cbxSlashNormalQ.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cbxSlashNormalQ.Items.AddRange(new object[] {
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
			this.cbxSlashNormalQ.Location = new System.Drawing.Point(96, 35);
			this.cbxSlashNormalQ.Name = "cbxSlashNormalQ";
			this.cbxSlashNormalQ.Size = new System.Drawing.Size(40, 21);
			this.cbxSlashNormalQ.TabIndex = 56;
			// 
			// label32
			// 
			this.label32.Location = new System.Drawing.Point(16, 59);
			this.label32.Name = "label32";
			this.label32.Size = new System.Drawing.Size(72, 21);
			this.label32.TabIndex = 60;
			this.label32.Text = "You block";
			this.label32.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// cbxSlashBlockQ
			// 
			this.cbxSlashBlockQ.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cbxSlashBlockQ.Items.AddRange(new object[] {
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
			this.cbxSlashBlockQ.Location = new System.Drawing.Point(96, 59);
			this.cbxSlashBlockQ.Name = "cbxSlashBlockQ";
			this.cbxSlashBlockQ.Size = new System.Drawing.Size(40, 21);
			this.cbxSlashBlockQ.TabIndex = 58;
			// 
			// label33
			// 
			this.label33.Location = new System.Drawing.Point(104, 18);
			this.label33.Name = "label33";
			this.label33.Size = new System.Drawing.Size(32, 13);
			this.label33.TabIndex = 64;
			this.label33.Text = "QBar";
			// 
			// label34
			// 
			this.label34.Location = new System.Drawing.Point(208, 3);
			this.label34.Name = "label34";
			this.label34.Size = new System.Drawing.Size(80, 15);
			this.label34.TabIndex = 59;
			this.label34.Text = "Chained Style";
			this.label34.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// label35
			// 
			this.label35.Location = new System.Drawing.Point(208, 18);
			this.label35.Name = "label35";
			this.label35.Size = new System.Drawing.Size(32, 13);
			this.label35.TabIndex = 64;
			this.label35.Text = "QBar";
			// 
			// cbxSlashBlockChainQ
			// 
			this.cbxSlashBlockChainQ.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cbxSlashBlockChainQ.Items.AddRange(new object[] {
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
			this.cbxSlashBlockChainQ.Location = new System.Drawing.Point(200, 59);
			this.cbxSlashBlockChainQ.Name = "cbxSlashBlockChainQ";
			this.cbxSlashBlockChainQ.Size = new System.Drawing.Size(40, 21);
			this.cbxSlashBlockChainQ.TabIndex = 58;
			// 
			// cbxSlashBlockChainKey
			// 
			this.cbxSlashBlockChainKey.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cbxSlashBlockChainKey.Items.AddRange(new object[] {
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
			this.cbxSlashBlockChainKey.Location = new System.Drawing.Point(248, 59);
			this.cbxSlashBlockChainKey.Name = "cbxSlashBlockChainKey";
			this.cbxSlashBlockChainKey.Size = new System.Drawing.Size(40, 21);
			this.cbxSlashBlockChainKey.TabIndex = 57;
			// 
			// label36
			// 
			this.label36.Location = new System.Drawing.Point(256, 18);
			this.label36.Name = "label36";
			this.label36.Size = new System.Drawing.Size(32, 13);
			this.label36.TabIndex = 65;
			this.label36.Text = "Key";
			// 
			// label7
			// 
			this.label7.Location = new System.Drawing.Point(16, 35);
			this.label7.Name = "label7";
			this.label7.Size = new System.Drawing.Size(72, 21);
			this.label7.TabIndex = 73;
			this.label7.Text = "Normal Style";
			this.label7.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// label8
			// 
			this.label8.Location = new System.Drawing.Point(152, 18);
			this.label8.Name = "label8";
			this.label8.Size = new System.Drawing.Size(32, 13);
			this.label8.TabIndex = 78;
			this.label8.Text = "Key";
			// 
			// comboBox1
			// 
			this.comboBox1.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.comboBox1.Enabled = false;
			this.comboBox1.Items.AddRange(new object[] {
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
			this.comboBox1.Location = new System.Drawing.Point(144, 35);
			this.comboBox1.Name = "comboBox1";
			this.comboBox1.Size = new System.Drawing.Size(40, 21);
			this.comboBox1.TabIndex = 66;
			// 
			// comboBox2
			// 
			this.comboBox2.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.comboBox2.Enabled = false;
			this.comboBox2.Items.AddRange(new object[] {
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
			this.comboBox2.Location = new System.Drawing.Point(144, 59);
			this.comboBox2.Name = "comboBox2";
			this.comboBox2.Size = new System.Drawing.Size(40, 21);
			this.comboBox2.TabIndex = 68;
			// 
			// comboBox8
			// 
			this.comboBox8.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.comboBox8.Enabled = false;
			this.comboBox8.Items.AddRange(new object[] {
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
			this.comboBox8.Location = new System.Drawing.Point(96, 35);
			this.comboBox8.Name = "comboBox8";
			this.comboBox8.Size = new System.Drawing.Size(40, 21);
			this.comboBox8.TabIndex = 67;
			// 
			// label10
			// 
			this.label10.Location = new System.Drawing.Point(16, 59);
			this.label10.Name = "label10";
			this.label10.Size = new System.Drawing.Size(72, 21);
			this.label10.TabIndex = 74;
			this.label10.Text = "You block";
			this.label10.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// comboBox10
			// 
			this.comboBox10.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.comboBox10.Enabled = false;
			this.comboBox10.Items.AddRange(new object[] {
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
			this.comboBox10.Location = new System.Drawing.Point(96, 59);
			this.comboBox10.Name = "comboBox10";
			this.comboBox10.Size = new System.Drawing.Size(40, 21);
			this.comboBox10.TabIndex = 70;
			// 
			// label11
			// 
			this.label11.Location = new System.Drawing.Point(104, 18);
			this.label11.Name = "label11";
			this.label11.Size = new System.Drawing.Size(32, 13);
			this.label11.TabIndex = 76;
			this.label11.Text = "QBar";
			// 
			// label12
			// 
			this.label12.Location = new System.Drawing.Point(208, 3);
			this.label12.Name = "label12";
			this.label12.Size = new System.Drawing.Size(80, 15);
			this.label12.TabIndex = 72;
			this.label12.Text = "Chained Style";
			this.label12.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// label31
			// 
			this.label31.Location = new System.Drawing.Point(208, 18);
			this.label31.Name = "label31";
			this.label31.Size = new System.Drawing.Size(32, 13);
			this.label31.TabIndex = 75;
			this.label31.Text = "QBar";
			// 
			// comboBox11
			// 
			this.comboBox11.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.comboBox11.Enabled = false;
			this.comboBox11.Items.AddRange(new object[] {
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
			this.comboBox11.Location = new System.Drawing.Point(200, 59);
			this.comboBox11.Name = "comboBox11";
			this.comboBox11.Size = new System.Drawing.Size(40, 21);
			this.comboBox11.TabIndex = 71;
			// 
			// comboBox12
			// 
			this.comboBox12.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.comboBox12.Enabled = false;
			this.comboBox12.Items.AddRange(new object[] {
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
			this.comboBox12.Location = new System.Drawing.Point(248, 59);
			this.comboBox12.Name = "comboBox12";
			this.comboBox12.Size = new System.Drawing.Size(40, 21);
			this.comboBox12.TabIndex = 69;
			// 
			// label37
			// 
			this.label37.Location = new System.Drawing.Point(256, 18);
			this.label37.Name = "label37";
			this.label37.Size = new System.Drawing.Size(32, 13);
			this.label37.TabIndex = 77;
			this.label37.Text = "Key";
			// 
			// cbxSlashNormalChainQ
			// 
			this.cbxSlashNormalChainQ.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cbxSlashNormalChainQ.Items.AddRange(new object[] {
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
			this.cbxSlashNormalChainQ.Location = new System.Drawing.Point(200, 35);
			this.cbxSlashNormalChainQ.Name = "cbxSlashNormalChainQ";
			this.cbxSlashNormalChainQ.Size = new System.Drawing.Size(40, 21);
			this.cbxSlashNormalChainQ.TabIndex = 58;
			// 
			// cbxSlashNormalChainKey
			// 
			this.cbxSlashNormalChainKey.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cbxSlashNormalChainKey.Items.AddRange(new object[] {
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
			this.cbxSlashNormalChainKey.Location = new System.Drawing.Point(248, 35);
			this.cbxSlashNormalChainKey.Name = "cbxSlashNormalChainKey";
			this.cbxSlashNormalChainKey.Size = new System.Drawing.Size(40, 21);
			this.cbxSlashNormalChainKey.TabIndex = 57;
			// 
			// label1
			// 
			this.label1.Location = new System.Drawing.Point(184, 72);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(32, 17);
			this.label1.TabIndex = 59;
			this.label1.Text = "QBar";
			// 
			// cbxStealthQ
			// 
			this.cbxStealthQ.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cbxStealthQ.Items.AddRange(new object[] {
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
			this.cbxStealthQ.Location = new System.Drawing.Point(176, 90);
			this.cbxStealthQ.Name = "cbxStealthQ";
			this.cbxStealthQ.Size = new System.Drawing.Size(40, 21);
			this.cbxStealthQ.TabIndex = 55;
			// 
			// label15
			// 
			this.label15.Location = new System.Drawing.Point(64, 88);
			this.label15.Name = "label15";
			this.label15.Size = new System.Drawing.Size(96, 21);
			this.label15.TabIndex = 57;
			this.label15.Text = "Stealth Command";
			this.label15.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// cbxStealthKey
			// 
			this.cbxStealthKey.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cbxStealthKey.Items.AddRange(new object[] {
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
			this.cbxStealthKey.Location = new System.Drawing.Point(240, 90);
			this.cbxStealthKey.Name = "cbxStealthKey";
			this.cbxStealthKey.Size = new System.Drawing.Size(40, 21);
			this.cbxStealthKey.TabIndex = 56;
			// 
			// label16
			// 
			this.label16.Location = new System.Drawing.Point(248, 72);
			this.label16.Name = "label16";
			this.label16.Size = new System.Drawing.Size(32, 17);
			this.label16.TabIndex = 58;
			this.label16.Text = "Key";
			// 
			// cbxMeleeWeaponKey
			// 
			this.cbxMeleeWeaponKey.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cbxMeleeWeaponKey.ItemHeight = 13;
			this.cbxMeleeWeaponKey.Items.AddRange(new object[] {
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
			this.cbxMeleeWeaponKey.Location = new System.Drawing.Point(224, 96);
			this.cbxMeleeWeaponKey.Name = "cbxMeleeWeaponKey";
			this.cbxMeleeWeaponKey.Size = new System.Drawing.Size(40, 21);
			this.cbxMeleeWeaponKey.TabIndex = 52;
			// 
			// cbxMeleeWeaponQ
			// 
			this.cbxMeleeWeaponQ.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cbxMeleeWeaponQ.ItemHeight = 13;
			this.cbxMeleeWeaponQ.Items.AddRange(new object[] {
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
			this.cbxMeleeWeaponQ.Location = new System.Drawing.Point(160, 96);
			this.cbxMeleeWeaponQ.Name = "cbxMeleeWeaponQ";
			this.cbxMeleeWeaponQ.Size = new System.Drawing.Size(40, 21);
			this.cbxMeleeWeaponQ.TabIndex = 52;
			// 
			// label17
			// 
			this.label17.Location = new System.Drawing.Point(8, 96);
			this.label17.Name = "label17";
			this.label17.Size = new System.Drawing.Size(104, 21);
			this.label17.TabIndex = 53;
			this.label17.Text = "Melee Weapon";
			this.label17.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// frmScoutSettings
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.ClientSize = new System.Drawing.Size(336, 392);
			this.Controls.Add(this.panel6);
			this.Controls.Add(this.panel3);
			this.Controls.Add(this.tabControl1);
			this.DockPadding.All = 4;
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
			this.Name = "frmScoutSettings";
			this.Text = "frmScout";
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
			this.tabControl1.ResumeLayout(false);
			this.tabControl2.ResumeLayout(false);
			this.tabPage4.ResumeLayout(false);
			this.tabPage7.ResumeLayout(false);
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
		private System.Windows.Forms.Label lblRangedPowerTap;
		private System.Windows.Forms.Label lblMeleePowerTap;
		private System.Windows.Forms.Panel panel7;
		private System.Windows.Forms.Label label9;
		private System.Windows.Forms.TabControl tabControl1;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.Label label5;
		private System.Windows.Forms.Panel panel4;
		private System.Windows.Forms.Label label6;
		private System.Windows.Forms.Panel panel5;
		private System.Windows.Forms.CheckBox chkStealthResting;
		private System.Windows.Forms.ComboBox cbxFightRangedCritQ;
		private System.Windows.Forms.ComboBox cbxFightRangedBowQ;
		private System.Windows.Forms.Label lblCrit;
		private System.Windows.Forms.ComboBox cbxFightRangedCritKey;
		private System.Windows.Forms.ComboBox cbxFightRangedBowKey;
		private System.Windows.Forms.CheckBox chkStealthAlways;
		private System.Windows.Forms.TabControl tabControl2;
		private System.Windows.Forms.TabPage tabPage4;
		private System.Windows.Forms.TabPage tabPage7;
		private System.Windows.Forms.Label label13;
		private System.Windows.Forms.Label label14;
		private System.Windows.Forms.Label label32;
		private System.Windows.Forms.Label label34;
		private System.Windows.Forms.Label label35;
		private System.Windows.Forms.Label label36;
		private System.Windows.Forms.Label label7;
		private System.Windows.Forms.Label label8;
		private System.Windows.Forms.ComboBox comboBox1;
		private System.Windows.Forms.ComboBox comboBox2;
		private System.Windows.Forms.ComboBox comboBox8;
		private System.Windows.Forms.Label label10;
		private System.Windows.Forms.ComboBox comboBox10;
		private System.Windows.Forms.Label label11;
		private System.Windows.Forms.Label label12;
		private System.Windows.Forms.Label label31;
		private System.Windows.Forms.ComboBox comboBox11;
		private System.Windows.Forms.ComboBox comboBox12;
		private System.Windows.Forms.Label label37;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.ComboBox cbxStealthQ;
		private System.Windows.Forms.Label label15;
		private System.Windows.Forms.ComboBox cbxStealthKey;
		private System.Windows.Forms.Label label16;
		private System.Windows.Forms.ComboBox cbxFightRangedRFQ;
		private System.Windows.Forms.ComboBox cbxFightRangedRFKey;
		private System.Windows.Forms.ComboBox cbxSlashNormalKey;
		private System.Windows.Forms.ComboBox cbxSlashBlockKey;
		private System.Windows.Forms.ComboBox cbxSlashNormalQ;
		private System.Windows.Forms.ComboBox cbxSlashBlockQ;
		private System.Windows.Forms.ComboBox cbxSlashBlockChainQ;
		private System.Windows.Forms.ComboBox cbxSlashBlockChainKey;
		private System.Windows.Forms.ComboBox cbxSlashNormalChainQ;
		private System.Windows.Forms.ComboBox cbxSlashNormalChainKey;
		private System.Windows.Forms.ComboBox cbxMeleeWeaponKey;
		private System.Windows.Forms.ComboBox cbxMeleeWeaponQ;
		private System.Windows.Forms.Label label17;
		private System.Windows.Forms.Label label33;


		public void InitializeForm(Profile profile)
		{
			
		}

		#region IProfileForm handling routines
		public event UpdateVariableDelegate UpdateVariable;

		public event CurrentProfileDelegate LoadCurrentProfile; // This event is never used
		public event CurrentProfileDelegate SaveCurrentProfile;

		public void DefineVariables( Profile profile)
		{
			// Stealth tab
			profile.DefineVariable( "Scout.StealthResting", true);
			profile.DefineVariable( "Scout.StealthAlways", false);
			profile.DefineVariable( "Scout.StealthQ", "");
			profile.DefineVariable( "Scout.StealthKey", "");

			//Fight tab
			profile.DefineVariable( "Scout.FightRangedCritQ", "");
			profile.DefineVariable( "Scout.FightRangedCritKey", "");

			profile.DefineVariable( "Scout.FightRangedBowQ", "");
			profile.DefineVariable( "Scout.FightRangedBowKey", "");

			profile.DefineVariable( "Scout.FightRangedRapidFireQ", "");
			profile.DefineVariable( "Scout.FightRangedRapidFireKey", "");

			profile.DefineVariable( "Scout.FightMeleeWeaponQ", "");
			profile.DefineVariable( "Scout.FightMeleeWeaponKey", "");

			//Fight tab => Slash
			profile.DefineVariable( "Scout.SlashNormalQ", "");
			profile.DefineVariable( "Scout.SlashNormalKey", "");
			profile.DefineVariable( "Scout.SlashNormalChainQ", "");
			profile.DefineVariable( "Scout.SlashNormalChainKey", "");

			profile.DefineVariable( "Scout.SlashBlockQ", "");
			profile.DefineVariable( "Scout.SlashBlockKey", "");
			profile.DefineVariable( "Scout.SlashBlockChainQ", "");
			profile.DefineVariable( "Scout.SlashBlockChainKey", "");

			//Fight tab => Thrust
		}

		public void OnProfileChange( Profile profile)
		{
			// Stealth tab
			chkStealthResting.Checked = profile.GetBool("Scout.StealthResting");
			chkStealthAlways.Checked = profile.GetBool("Scout.StealthAlways");
			SetComboBox("", profile.GetString("Scout.StealthQ"), ref cbxStealthQ , false, profile);
			SetComboBox("", profile.GetString("Scout.StealthKey"), ref cbxStealthKey , false, profile);

			//Fight tab
			SetComboBox("", profile.GetString("Scout.FightRangedCritQ"), ref cbxFightRangedCritQ , false, profile);
			SetComboBox("", profile.GetString("Scout.FightRangedCritKey"), ref cbxFightRangedCritKey , false, profile);

			SetComboBox("", profile.GetString("Scout.FightRangedBowQ"), ref cbxFightRangedBowQ , false, profile);
			SetComboBox("", profile.GetString("Scout.FightRangedBowKey"), ref cbxFightRangedBowKey  , false, profile);

			SetComboBox("", profile.GetString("Scout.FightRangedRapidFireQ"), ref cbxFightRangedRFQ , false, profile);
			SetComboBox("", profile.GetString("Scout.FightRangedRapidFireKey"), ref cbxFightRangedRFKey , false, profile);
			
			SetComboBox("", profile.GetString("Scout.FightMeleeWeaponQ"), ref cbxMeleeWeaponQ , false, profile);
			SetComboBox("", profile.GetString("Scout.FightMeleeWeaponKey"), ref cbxMeleeWeaponKey , false, profile);
			
			//Fight tab => Slash
			SetComboBox("", profile.GetString("Scout.SlashNormalQ"), ref cbxSlashNormalQ , false, profile);
			SetComboBox("", profile.GetString("Scout.SlashNormalKey"), ref cbxSlashNormalKey , false, profile);
			SetComboBox("", profile.GetString("Scout.SlashNormalChainQ"), ref cbxSlashNormalChainQ , false, profile);
			SetComboBox("", profile.GetString("Scout.SlashNormalChainKey"), ref cbxSlashNormalChainKey , false, profile);

			SetComboBox("", profile.GetString("Scout.SlashBlockQ"), ref cbxSlashBlockQ , false, profile);
			SetComboBox("", profile.GetString("Scout.SlashBlockKey"), ref cbxSlashBlockKey , false, profile);
			SetComboBox("", profile.GetString("Scout.SlashBlockChainQ"), ref cbxSlashBlockChainQ , false, profile);
			SetComboBox("", profile.GetString("Scout.SlashBlockChainKey"), ref cbxSlashBlockChainKey , false, profile);

			//Fight tab => Thrust
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
				// Stealth tab
				chkStealthResting.Checked = profile.GetBool("Scout.StealthResting");
				chkStealthAlways.Checked = profile.GetBool("Scout.StealthAlways");
				SaveComboBox("Scout.StealthQ", ref cbxStealthQ, profile);
				SaveComboBox("Scout.StealthKey", ref cbxStealthKey, profile);

				//Fight tab
				SaveComboBox("Scout.FightRangedCritQ", ref cbxFightRangedCritQ, profile);
				SaveComboBox("Scout.FightRangedCritKey", ref cbxFightRangedCritKey, profile);

				SaveComboBox("Scout.FightRangedBowQ", ref cbxFightRangedBowQ, profile);
				SaveComboBox("Scout.FightRangedBowKey", ref cbxFightRangedBowKey , profile);

				SaveComboBox("Scout.FightMeleeWeaponQ", ref cbxMeleeWeaponQ, profile);
				SaveComboBox("Scout.FightMeleeWeaponKey", ref cbxMeleeWeaponKey, profile);

				SaveComboBox("Scout.FightRangedRapidFireQ", ref cbxFightRangedRFQ, profile);
				SaveComboBox("Scout.FightRangedRapidFireKey", ref cbxFightRangedRFKey, profile);
			
				//Fight tab => Slash
				SaveComboBox("Scout.SlashNormalQ", ref cbxSlashNormalQ, profile);
				SaveComboBox("Scout.SlashNormalKey", ref cbxSlashNormalKey, profile);
				SaveComboBox("Scout.SlashNormalChainQ", ref cbxSlashNormalChainQ, profile);
				SaveComboBox("Scout.SlashNormalChainKey", ref cbxSlashNormalChainKey, profile);

				SaveComboBox("Scout.SlashBlockQ", ref cbxSlashBlockQ, profile);
				SaveComboBox("Scout.SlashBlockKey", ref cbxSlashBlockKey, profile);
				SaveComboBox("Scout.SlashBlockChainQ", ref cbxSlashBlockChainQ, profile);
				SaveComboBox("Scout.SlashBlockChainKey", ref cbxSlashBlockChainKey, profile);

				//Fight tab => Thrust

				
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
