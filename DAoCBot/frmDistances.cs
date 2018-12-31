//------------------------------------------------------------------------------
//-------------------------------------------------------------------------------
using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;

namespace DAoC_Bot
{
	/// <summary>
	/// Summary description for frmBasic.
	/// </summary>
	public class frmDistances : System.Windows.Forms.Form, IProfileForm
	{
		private System.Windows.Forms.Panel panel3;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.Panel panel2;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Panel panel1;
		private System.Windows.Forms.Label label4;
		private System.Windows.Forms.Panel panel6;
		private System.Windows.Forms.Button btnCancel;
		private System.Windows.Forms.Button btnApply;
		private System.Windows.Forms.Label lblMana;
		private System.Windows.Forms.Label lblEnergy;
		private System.Windows.Forms.ToolTip toolTip1;
		private System.Windows.Forms.Panel panel4;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.NumericUpDown nudWander;
		private System.Windows.Forms.NumericUpDown nudMaxRange;
		private System.Windows.Forms.NumericUpDown nudSearch;
		private System.Windows.Forms.Panel panel5;
		private System.Windows.Forms.Label label5;
		private System.Windows.Forms.NumericUpDown nudMinRange;
		private System.Windows.Forms.Label label6;
		private System.Windows.Forms.Panel panel7;
		private System.Windows.Forms.Label label7;
		private System.Windows.Forms.Label label8;
		private System.Windows.Forms.NumericUpDown nudMinLevel;
		private System.Windows.Forms.CheckBox chkAutoLevel;
		private System.Windows.Forms.NumericUpDown nudMaxLevel;
		private System.Windows.Forms.CheckBox chkYellow;
		private System.Windows.Forms.CheckBox chkBlue;
		private System.Windows.Forms.CheckBox chkGreen;
		private System.Windows.Forms.CheckBox chkOrange;
		private System.ComponentModel.IContainer components;

		public frmDistances()
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
			this.components = new System.ComponentModel.Container();
			this.panel3 = new System.Windows.Forms.Panel();
			this.label2 = new System.Windows.Forms.Label();
			this.panel2 = new System.Windows.Forms.Panel();
			this.label1 = new System.Windows.Forms.Label();
			this.panel1 = new System.Windows.Forms.Panel();
			this.nudWander = new System.Windows.Forms.NumericUpDown();
			this.lblEnergy = new System.Windows.Forms.Label();
			this.nudSearch = new System.Windows.Forms.NumericUpDown();
			this.label4 = new System.Windows.Forms.Label();
			this.nudMaxRange = new System.Windows.Forms.NumericUpDown();
			this.lblMana = new System.Windows.Forms.Label();
			this.panel6 = new System.Windows.Forms.Panel();
			this.btnCancel = new System.Windows.Forms.Button();
			this.btnApply = new System.Windows.Forms.Button();
			this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
			this.nudMinRange = new System.Windows.Forms.NumericUpDown();
			this.nudMaxLevel = new System.Windows.Forms.NumericUpDown();
			this.nudMinLevel = new System.Windows.Forms.NumericUpDown();
			this.panel4 = new System.Windows.Forms.Panel();
			this.label3 = new System.Windows.Forms.Label();
			this.panel5 = new System.Windows.Forms.Panel();
			this.label5 = new System.Windows.Forms.Label();
			this.label6 = new System.Windows.Forms.Label();
			this.panel7 = new System.Windows.Forms.Panel();
			this.chkAutoLevel = new System.Windows.Forms.CheckBox();
			this.label7 = new System.Windows.Forms.Label();
			this.label8 = new System.Windows.Forms.Label();
			this.chkYellow = new System.Windows.Forms.CheckBox();
			this.chkBlue = new System.Windows.Forms.CheckBox();
			this.chkGreen = new System.Windows.Forms.CheckBox();
			this.chkOrange = new System.Windows.Forms.CheckBox();
			this.panel3.SuspendLayout();
			this.panel2.SuspendLayout();
			this.panel1.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.nudWander)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.nudSearch)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.nudMaxRange)).BeginInit();
			this.panel6.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.nudMinRange)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.nudMaxLevel)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.nudMinLevel)).BeginInit();
			this.panel4.SuspendLayout();
			this.panel5.SuspendLayout();
			this.panel7.SuspendLayout();
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
			this.panel3.Size = new System.Drawing.Size(284, 24);
			this.panel3.TabIndex = 13;
			// 
			// label2
			// 
			this.label2.Dock = System.Windows.Forms.DockStyle.Fill;
			this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.label2.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
			this.label2.Location = new System.Drawing.Point(0, 0);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(280, 20);
			this.label2.TabIndex = 0;
			this.label2.Text = "Distance Settings";
			this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// panel2
			// 
			this.panel2.BackColor = System.Drawing.SystemColors.ControlDark;
			this.panel2.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
			this.panel2.Controls.Add(this.label1);
			this.panel2.Dock = System.Windows.Forms.DockStyle.Top;
			this.panel2.Location = new System.Drawing.Point(4, 28);
			this.panel2.Name = "panel2";
			this.panel2.Size = new System.Drawing.Size(284, 16);
			this.panel2.TabIndex = 16;
			// 
			// label1
			// 
			this.label1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.label1.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
			this.label1.Location = new System.Drawing.Point(0, 0);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(280, 12);
			this.label1.TabIndex = 0;
			this.label1.Text = "Searching for Targets";
			this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// panel1
			// 
			this.panel1.Controls.Add(this.nudWander);
			this.panel1.Controls.Add(this.lblEnergy);
			this.panel1.Controls.Add(this.nudSearch);
			this.panel1.Controls.Add(this.label4);
			this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
			this.panel1.Location = new System.Drawing.Point(4, 44);
			this.panel1.Name = "panel1";
			this.panel1.Size = new System.Drawing.Size(284, 60);
			this.panel1.TabIndex = 17;
			// 
			// nudWander
			// 
			this.nudWander.Increment = new System.Decimal(new int[] {
																		100,
																		0,
																		0,
																		0});
			this.nudWander.Location = new System.Drawing.Point(216, 32);
			this.nudWander.Maximum = new System.Decimal(new int[] {
																	  10000,
																	  0,
																	  0,
																	  0});
			this.nudWander.Name = "nudWander";
			this.nudWander.Size = new System.Drawing.Size(64, 20);
			this.nudWander.TabIndex = 27;
			this.toolTip1.SetToolTip(this.nudWander, "Run away if your Energy is below this, and your targets health is higher than you" +
				"r health");
			// 
			// lblEnergy
			// 
			this.lblEnergy.Location = new System.Drawing.Point(8, 33);
			this.lblEnergy.Name = "lblEnergy";
			this.lblEnergy.Size = new System.Drawing.Size(208, 16);
			this.lblEnergy.TabIndex = 26;
			this.lblEnergy.Text = "Wander how far from current Waypoint:";
			this.lblEnergy.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// nudSearch
			// 
			this.nudSearch.Increment = new System.Decimal(new int[] {
																		100,
																		0,
																		0,
																		0});
			this.nudSearch.Location = new System.Drawing.Point(216, 8);
			this.nudSearch.Maximum = new System.Decimal(new int[] {
																	  10000,
																	  0,
																	  0,
																	  0});
			this.nudSearch.Name = "nudSearch";
			this.nudSearch.Size = new System.Drawing.Size(64, 20);
			this.nudSearch.TabIndex = 24;
			this.toolTip1.SetToolTip(this.nudSearch, "Run away if your Health is below this, and your targets health is higher than you" +
				"r health");
			// 
			// label4
			// 
			this.label4.Location = new System.Drawing.Point(8, 9);
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size(208, 20);
			this.label4.TabIndex = 21;
			this.label4.Text = "Search for targets within what distance:";
			this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// nudMaxRange
			// 
			this.nudMaxRange.Increment = new System.Decimal(new int[] {
																		  100,
																		  0,
																		  0,
																		  0});
			this.nudMaxRange.Location = new System.Drawing.Point(216, 8);
			this.nudMaxRange.Maximum = new System.Decimal(new int[] {
																		3000,
																		0,
																		0,
																		0});
			this.nudMaxRange.Name = "nudMaxRange";
			this.nudMaxRange.Size = new System.Drawing.Size(64, 20);
			this.nudMaxRange.TabIndex = 25;
			this.toolTip1.SetToolTip(this.nudMaxRange, "Run away if your Mana is below this, and your targets health is higher than your " +
				"health");
			// 
			// lblMana
			// 
			this.lblMana.Location = new System.Drawing.Point(8, 9);
			this.lblMana.Name = "lblMana";
			this.lblMana.Size = new System.Drawing.Size(200, 24);
			this.lblMana.TabIndex = 23;
			this.lblMana.Text = "Maximum ranged attack distance:";
			this.lblMana.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// panel6
			// 
			this.panel6.Controls.Add(this.btnCancel);
			this.panel6.Controls.Add(this.btnApply);
			this.panel6.Dock = System.Windows.Forms.DockStyle.Bottom;
			this.panel6.Location = new System.Drawing.Point(4, 312);
			this.panel6.Name = "panel6";
			this.panel6.Size = new System.Drawing.Size(284, 28);
			this.panel6.TabIndex = 20;
			// 
			// btnCancel
			// 
			this.btnCancel.Location = new System.Drawing.Point(83, 4);
			this.btnCancel.Name = "btnCancel";
			this.btnCancel.Size = new System.Drawing.Size(75, 21);
			this.btnCancel.TabIndex = 1;
			this.btnCancel.Text = "Cancel";
			this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
			// 
			// btnApply
			// 
			this.btnApply.Location = new System.Drawing.Point(4, 4);
			this.btnApply.Name = "btnApply";
			this.btnApply.Size = new System.Drawing.Size(75, 21);
			this.btnApply.TabIndex = 0;
			this.btnApply.Text = "Apply";
			this.btnApply.Click += new System.EventHandler(this.btnApply_Click);
			// 
			// nudMinRange
			// 
			this.nudMinRange.Increment = new System.Decimal(new int[] {
																		  100,
																		  0,
																		  0,
																		  0});
			this.nudMinRange.Location = new System.Drawing.Point(216, 32);
			this.nudMinRange.Maximum = new System.Decimal(new int[] {
																		3000,
																		0,
																		0,
																		0});
			this.nudMinRange.Name = "nudMinRange";
			this.nudMinRange.Size = new System.Drawing.Size(64, 20);
			this.nudMinRange.TabIndex = 25;
			this.toolTip1.SetToolTip(this.nudMinRange, "Run away if your Mana is below this, and your targets health is higher than your " +
				"health");
			// 
			// nudMaxLevel
			// 
			this.nudMaxLevel.Location = new System.Drawing.Point(216, 64);
			this.nudMaxLevel.Maximum = new System.Decimal(new int[] {
																		70,
																		0,
																		0,
																		0});
			this.nudMaxLevel.Name = "nudMaxLevel";
			this.nudMaxLevel.Size = new System.Drawing.Size(64, 20);
			this.nudMaxLevel.TabIndex = 25;
			this.toolTip1.SetToolTip(this.nudMaxLevel, "Maximum level of MOB to for the bot to target and attack");
			// 
			// nudMinLevel
			// 
			this.nudMinLevel.Location = new System.Drawing.Point(216, 88);
			this.nudMinLevel.Maximum = new System.Decimal(new int[] {
																		70,
																		0,
																		0,
																		0});
			this.nudMinLevel.Name = "nudMinLevel";
			this.nudMinLevel.Size = new System.Drawing.Size(64, 20);
			this.nudMinLevel.TabIndex = 25;
			this.toolTip1.SetToolTip(this.nudMinLevel, "Minimum level of MOB to for the bot to target and attack");
			// 
			// panel4
			// 
			this.panel4.BackColor = System.Drawing.SystemColors.ControlDark;
			this.panel4.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
			this.panel4.Controls.Add(this.label3);
			this.panel4.Dock = System.Windows.Forms.DockStyle.Top;
			this.panel4.Location = new System.Drawing.Point(4, 104);
			this.panel4.Name = "panel4";
			this.panel4.Size = new System.Drawing.Size(284, 16);
			this.panel4.TabIndex = 29;
			// 
			// label3
			// 
			this.label3.Dock = System.Windows.Forms.DockStyle.Fill;
			this.label3.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
			this.label3.Location = new System.Drawing.Point(0, 0);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(280, 12);
			this.label3.TabIndex = 0;
			this.label3.Text = "Attacks";
			this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// panel5
			// 
			this.panel5.Controls.Add(this.nudMaxRange);
			this.panel5.Controls.Add(this.lblMana);
			this.panel5.Controls.Add(this.label5);
			this.panel5.Controls.Add(this.nudMinRange);
			this.panel5.Dock = System.Windows.Forms.DockStyle.Top;
			this.panel5.Location = new System.Drawing.Point(4, 120);
			this.panel5.Name = "panel5";
			this.panel5.Size = new System.Drawing.Size(284, 56);
			this.panel5.TabIndex = 30;
			// 
			// label5
			// 
			this.label5.Location = new System.Drawing.Point(8, 32);
			this.label5.Name = "label5";
			this.label5.Size = new System.Drawing.Size(200, 20);
			this.label5.TabIndex = 23;
			this.label5.Text = "Minimum ranged attack distance:";
			this.label5.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// label6
			// 
			this.label6.BackColor = System.Drawing.SystemColors.ControlDark;
			this.label6.Dock = System.Windows.Forms.DockStyle.Top;
			this.label6.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
			this.label6.Location = new System.Drawing.Point(4, 176);
			this.label6.Name = "label6";
			this.label6.Size = new System.Drawing.Size(284, 16);
			this.label6.TabIndex = 31;
			this.label6.Text = "Level Range";
			this.label6.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// panel7
			// 
			this.panel7.Controls.Add(this.chkAutoLevel);
			this.panel7.Controls.Add(this.nudMaxLevel);
			this.panel7.Controls.Add(this.label7);
			this.panel7.Controls.Add(this.label8);
			this.panel7.Controls.Add(this.nudMinLevel);
			this.panel7.Controls.Add(this.chkYellow);
			this.panel7.Controls.Add(this.chkBlue);
			this.panel7.Controls.Add(this.chkGreen);
			this.panel7.Controls.Add(this.chkOrange);
			this.panel7.Dock = System.Windows.Forms.DockStyle.Top;
			this.panel7.Location = new System.Drawing.Point(4, 192);
			this.panel7.Name = "panel7";
			this.panel7.Size = new System.Drawing.Size(284, 112);
			this.panel7.TabIndex = 32;
			// 
			// chkAutoLevel
			// 
			this.chkAutoLevel.Location = new System.Drawing.Point(8, 8);
			this.chkAutoLevel.Name = "chkAutoLevel";
			this.chkAutoLevel.Size = new System.Drawing.Size(146, 24);
			this.chkAutoLevel.TabIndex = 26;
			this.chkAutoLevel.Text = "Automatic Level Ranges";
			this.toolTip1.SetToolTip(this.chkAutoLevel, "Decide what to fight based on con color instead of setting exact levels");
			this.chkAutoLevel.CheckedChanged += new System.EventHandler(this.chkAutoLevel_CheckedChanged);
			// 
			// label7
			// 
			this.label7.Location = new System.Drawing.Point(8, 64);
			this.label7.Name = "label7";
			this.label7.Size = new System.Drawing.Size(200, 24);
			this.label7.TabIndex = 23;
			this.label7.Text = "Maximum MOB Level";
			this.label7.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// label8
			// 
			this.label8.Location = new System.Drawing.Point(8, 88);
			this.label8.Name = "label8";
			this.label8.Size = new System.Drawing.Size(200, 20);
			this.label8.TabIndex = 23;
			this.label8.Text = "Minimum MOB Level";
			this.label8.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// chkYellow
			// 
			this.chkYellow.Location = new System.Drawing.Point(88, 28);
			this.chkYellow.Name = "chkYellow";
			this.chkYellow.Size = new System.Drawing.Size(57, 24);
			this.chkYellow.TabIndex = 26;
			this.chkYellow.Text = "Yellow";
			this.toolTip1.SetToolTip(this.chkYellow, "Fight Yellow con mobs");
			// 
			// chkBlue
			// 
			this.chkBlue.Location = new System.Drawing.Point(160, 28);
			this.chkBlue.Name = "chkBlue";
			this.chkBlue.Size = new System.Drawing.Size(46, 24);
			this.chkBlue.TabIndex = 26;
			this.chkBlue.Text = "Blue";
			this.toolTip1.SetToolTip(this.chkBlue, "Fight Blue con mobs");
			// 
			// chkGreen
			// 
			this.chkGreen.Location = new System.Drawing.Point(216, 28);
			this.chkGreen.Name = "chkGreen";
			this.chkGreen.Size = new System.Drawing.Size(55, 24);
			this.chkGreen.TabIndex = 26;
			this.chkGreen.Text = "Green";
			this.toolTip1.SetToolTip(this.chkGreen, "Fight Green con mobs");
			// 
			// chkOrange
			// 
			this.chkOrange.Location = new System.Drawing.Point(8, 28);
			this.chkOrange.Name = "chkOrange";
			this.chkOrange.Size = new System.Drawing.Size(61, 24);
			this.chkOrange.TabIndex = 26;
			this.chkOrange.Text = "Orange";
			this.toolTip1.SetToolTip(this.chkOrange, "Fight Orange con mobs");
			// 
			// frmDistances
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.ClientSize = new System.Drawing.Size(292, 344);
			this.Controls.Add(this.panel7);
			this.Controls.Add(this.label6);
			this.Controls.Add(this.panel5);
			this.Controls.Add(this.panel4);
			this.Controls.Add(this.panel6);
			this.Controls.Add(this.panel1);
			this.Controls.Add(this.panel2);
			this.Controls.Add(this.panel3);
			this.DockPadding.All = 4;
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
			this.Name = "frmDistances";
			this.Text = "frmDistances";
			this.panel3.ResumeLayout(false);
			this.panel2.ResumeLayout(false);
			this.panel1.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.nudWander)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.nudSearch)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.nudMaxRange)).EndInit();
			this.panel6.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.nudMinRange)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.nudMaxLevel)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.nudMinLevel)).EndInit();
			this.panel4.ResumeLayout(false);
			this.panel5.ResumeLayout(false);
			this.panel7.ResumeLayout(false);
			this.ResumeLayout(false);

		}
		#endregion

		public event UpdateVariableDelegate UpdateVariable;
		public event CurrentProfileDelegate SaveCurrentProfile;
		public event CurrentProfileDelegate LoadCurrentProfile;

		public void DefineVariables( Profile profile)
		{
			profile.DefineVariable("MinimumRangedAttackDistance", 300f);
			profile.DefineVariable("MaximumRangedAttackDistance", 1500f);
			profile.DefineVariable("SearchDistance", 3000f);
			profile.DefineVariable("WanderDistance", 1500f);
			profile.DefineVariable("AutomaticLevelSelect", true);
			profile.DefineVariable("MinimumLevel", 1);
			profile.DefineVariable("MaximumLevel", 2);
			profile.DefineVariable("FightGreens", true);
			profile.DefineVariable("FightBlues", true);
			profile.DefineVariable("FightYellows", true);
			profile.DefineVariable("FightOranges", false);
		}
		
		public void OnProfileChange( Profile profile)
		{
			nudSearch.Value = (decimal) profile.GetFloat("SearchDistance");
			nudWander.Value = (decimal) profile.GetFloat("WanderDistance");
			nudMaxRange.Value = (decimal) profile.GetFloat("MaximumRangedAttackDistance");
			nudMinRange.Value = (decimal) profile.GetFloat("MinimumRangedAttackDistance");
			nudMaxLevel.Value		= profile.GetInteger("MaximumLevel");
			nudMinLevel.Value		= profile.GetInteger("MinimumLevel");
			chkAutoLevel.Checked	= (bool) profile.GetBool("AutomaticLevelSelect");
			chkGreen.Checked	= (bool) profile.GetBool("FightGreens");
			chkBlue.Checked	= (bool) profile.GetBool("FightBlues");
			chkYellow.Checked	= (bool) profile.GetBool("FightYellows");
			chkOrange.Checked	= (bool) profile.GetBool("FightOranges");
		}


		private void btnApply_Click(object sender, System.EventArgs e)
		{
			if(chkAutoLevel.Checked &&
				! chkGreen.Checked &&
				! chkBlue.Checked &&
				! chkYellow.Checked)
			{
				MessageBox.Show ("To use automatic levels select one or more of Green Blue and Yellow check boxes");

			}
			if( UpdateVariable != null)
			{
				UpdateVariable( "SearchDistance", (float) nudSearch.Value);
				UpdateVariable( "WanderDistance", (float) nudWander.Value);
				UpdateVariable( "MaximumRangedAttackDistance", (float) nudMaxRange.Value);
				UpdateVariable( "MinimumRangedAttackDistance", (float) nudMinRange.Value);
				UpdateVariable( "MaximumLevel", (int) nudMaxLevel.Value);
				UpdateVariable( "MinimumLevel", (int) nudMinLevel.Value);
				UpdateVariable( "AutomaticLevelSelect", (bool) chkAutoLevel.Checked);
				UpdateVariable( "FightGreens", (bool) chkGreen.Checked);
				UpdateVariable( "FightBlues", (bool) chkBlue.Checked);
				UpdateVariable( "FightYellows", (bool) chkYellow.Checked);
				UpdateVariable( "FightOranges", (bool) chkOrange.Checked);
			}
			if( SaveCurrentProfile != null)
				SaveCurrentProfile();
		}

		private void btnCancel_Click(object sender, System.EventArgs e)
		{
			if( LoadCurrentProfile != null)
				SaveCurrentProfile();
		}

		private void chkAutoLevel_CheckedChanged(object sender, System.EventArgs e)
		{
			if(chkAutoLevel.Checked)
			{
				nudMaxLevel.Enabled = false;
				nudMinLevel.Enabled = false;
				chkOrange.Enabled = true;
				chkYellow.Enabled = true;
				chkBlue.Enabled = true;
				chkGreen.Enabled = true;
			}
			else
			{
				nudMaxLevel.Enabled = true;
				nudMinLevel.Enabled = true;
				chkOrange.Enabled = false;
				chkYellow.Enabled = false;
				chkBlue.Enabled = false;
				chkGreen.Enabled = false;
			}
		}
	}
}
