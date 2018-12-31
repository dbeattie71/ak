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
	public class frmRest : System.Windows.Forms.Form, IProfileForm
	{
		private System.Windows.Forms.Panel panel3;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.Panel panel2;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Panel panel1;
		private System.Windows.Forms.NumericUpDown nudRestBelowHealth;
		private System.Windows.Forms.NumericUpDown nudRestBelowMana;
		private System.Windows.Forms.CheckBox chkFullyRecovered;
		private System.Windows.Forms.Label lblManaIsBelowRest;
		private System.Windows.Forms.Label lblHealthIsBelowRest;
		private System.Windows.Forms.Button btnCancel;
		private System.Windows.Forms.Button btnApply;
		private System.Windows.Forms.Panel panel6;
		private System.Windows.Forms.Label lblEnduranceIsBelowRest;
		private System.Windows.Forms.NumericUpDown nudRestBelowEndurance;
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		public frmRest()
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();

			//
			// TODO: Add any constructor code after InitializeComponent call
			//.

			// Create the ToolTips
			ToolTip toolTip1 = new ToolTip();

			// Set up the delays for the ToolTip.
			toolTip1.AutoPopDelay = 3000;
			toolTip1.InitialDelay = 1000;
			toolTip1.ReshowDelay = 500;
			// Force the ToolTip text to be displayed whether or not the form is active.
			toolTip1.ShowAlways = true;
      
			// Set up the ToolTips text
			// General Section
			toolTip1.SetToolTip(this.lblHealthIsBelowRest, "Rest if your Health is below this percent.");
			toolTip1.SetToolTip(this.lblManaIsBelowRest, "Rest if your Mana is below this percent.");
			toolTip1.SetToolTip(this.lblEnduranceIsBelowRest, "Rest if your Endurance is below this percent.");
			toolTip1.SetToolTip(this.chkFullyRecovered, "Always Rest until fully recovered.");

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
			this.panel2 = new System.Windows.Forms.Panel();
			this.label1 = new System.Windows.Forms.Label();
			this.panel1 = new System.Windows.Forms.Panel();
			this.chkFullyRecovered = new System.Windows.Forms.CheckBox();
			this.nudRestBelowMana = new System.Windows.Forms.NumericUpDown();
			this.nudRestBelowHealth = new System.Windows.Forms.NumericUpDown();
			this.lblManaIsBelowRest = new System.Windows.Forms.Label();
			this.lblHealthIsBelowRest = new System.Windows.Forms.Label();
			this.nudRestBelowEndurance = new System.Windows.Forms.NumericUpDown();
			this.lblEnduranceIsBelowRest = new System.Windows.Forms.Label();
			this.btnCancel = new System.Windows.Forms.Button();
			this.btnApply = new System.Windows.Forms.Button();
			this.panel6 = new System.Windows.Forms.Panel();
			this.panel3.SuspendLayout();
			this.panel2.SuspendLayout();
			this.panel1.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.nudRestBelowMana)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.nudRestBelowHealth)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.nudRestBelowEndurance)).BeginInit();
			this.panel6.SuspendLayout();
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
			this.label2.Text = "Rest Settings";
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
			this.label1.Text = "Rest when";
			this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// panel1
			// 
			this.panel1.Controls.Add(this.chkFullyRecovered);
			this.panel1.Controls.Add(this.nudRestBelowMana);
			this.panel1.Controls.Add(this.nudRestBelowHealth);
			this.panel1.Controls.Add(this.lblManaIsBelowRest);
			this.panel1.Controls.Add(this.lblHealthIsBelowRest);
			this.panel1.Controls.Add(this.nudRestBelowEndurance);
			this.panel1.Controls.Add(this.lblEnduranceIsBelowRest);
			this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
			this.panel1.Location = new System.Drawing.Point(4, 44);
			this.panel1.Name = "panel1";
			this.panel1.Size = new System.Drawing.Size(284, 148);
			this.panel1.TabIndex = 17;
			// 
			// chkFullyRecovered
			// 
			this.chkFullyRecovered.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.chkFullyRecovered.Location = new System.Drawing.Point(8, 112);
			this.chkFullyRecovered.Name = "chkFullyRecovered";
			this.chkFullyRecovered.Size = new System.Drawing.Size(168, 21);
			this.chkFullyRecovered.TabIndex = 26;
			this.chkFullyRecovered.Text = "Rest until fully recovered";
			// 
			// nudRestBelowMana
			// 
			this.nudRestBelowMana.Location = new System.Drawing.Point(128, 40);
			this.nudRestBelowMana.Name = "nudRestBelowMana";
			this.nudRestBelowMana.Size = new System.Drawing.Size(48, 20);
			this.nudRestBelowMana.TabIndex = 25;
			// 
			// nudRestBelowHealth
			// 
			this.nudRestBelowHealth.Location = new System.Drawing.Point(128, 8);
			this.nudRestBelowHealth.Name = "nudRestBelowHealth";
			this.nudRestBelowHealth.Size = new System.Drawing.Size(48, 20);
			this.nudRestBelowHealth.TabIndex = 24;
			// 
			// lblManaIsBelowRest
			// 
			this.lblManaIsBelowRest.Location = new System.Drawing.Point(8, 40);
			this.lblManaIsBelowRest.Name = "lblManaIsBelowRest";
			this.lblManaIsBelowRest.Size = new System.Drawing.Size(80, 20);
			this.lblManaIsBelowRest.TabIndex = 23;
			this.lblManaIsBelowRest.Text = "Mana is below:";
			this.lblManaIsBelowRest.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// lblHealthIsBelowRest
			// 
			this.lblHealthIsBelowRest.Location = new System.Drawing.Point(8, 8);
			this.lblHealthIsBelowRest.Name = "lblHealthIsBelowRest";
			this.lblHealthIsBelowRest.Size = new System.Drawing.Size(88, 20);
			this.lblHealthIsBelowRest.TabIndex = 21;
			this.lblHealthIsBelowRest.Text = "Health is below:";
			this.lblHealthIsBelowRest.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// nudRestBelowEndurance
			// 
			this.nudRestBelowEndurance.Location = new System.Drawing.Point(128, 72);
			this.nudRestBelowEndurance.Name = "nudRestBelowEndurance";
			this.nudRestBelowEndurance.Size = new System.Drawing.Size(48, 20);
			this.nudRestBelowEndurance.TabIndex = 25;
			// 
			// lblEnduranceIsBelowRest
			// 
			this.lblEnduranceIsBelowRest.Location = new System.Drawing.Point(8, 72);
			this.lblEnduranceIsBelowRest.Name = "lblEnduranceIsBelowRest";
			this.lblEnduranceIsBelowRest.Size = new System.Drawing.Size(112, 20);
			this.lblEnduranceIsBelowRest.TabIndex = 23;
			this.lblEnduranceIsBelowRest.Text = "Endurance is below:";
			this.lblEnduranceIsBelowRest.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
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
			// panel6
			// 
			this.panel6.Controls.Add(this.btnCancel);
			this.panel6.Controls.Add(this.btnApply);
			this.panel6.Dock = System.Windows.Forms.DockStyle.Bottom;
			this.panel6.Location = new System.Drawing.Point(4, 252);
			this.panel6.Name = "panel6";
			this.panel6.Size = new System.Drawing.Size(284, 32);
			this.panel6.TabIndex = 20;
			// 
			// frmRest
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.ClientSize = new System.Drawing.Size(292, 288);
			this.Controls.Add(this.panel6);
			this.Controls.Add(this.panel1);
			this.Controls.Add(this.panel2);
			this.Controls.Add(this.panel3);
			this.DockPadding.All = 4;
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
			this.Name = "frmRest";
			this.Text = "frmRest";
			this.panel3.ResumeLayout(false);
			this.panel2.ResumeLayout(false);
			this.panel1.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.nudRestBelowMana)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.nudRestBelowHealth)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.nudRestBelowEndurance)).EndInit();
			this.panel6.ResumeLayout(false);
			this.ResumeLayout(false);

		}
		#endregion

		public event UpdateVariableDelegate UpdateVariable;
		public event CurrentProfileDelegate SaveCurrentProfile;
		public event CurrentProfileDelegate LoadCurrentProfile;

		public void DefineVariables( Profile profile)
		{
			profile.DefineVariable( "RestBelowHealth", 85f);
			profile.DefineVariable( "RestBelowMana", 85f);
			profile.DefineVariable( "RestBelowEndurance", 85f);
			profile.DefineVariable( "RestUntilFullyRecovered", false);
		}
		
		public void OnProfileChange( Profile profile)
		{
			nudRestBelowHealth.Value = (decimal) profile.GetFloat("RestBelowHealth");
			nudRestBelowMana.Value = (decimal) profile.GetFloat("RestBelowMana");
			nudRestBelowEndurance.Value = (decimal) profile.GetFloat("RestBelowEndurance");
			chkFullyRecovered.Checked = profile.GetBool("RestUntilFullyRecovered");
		}


		private void btnApply_Click(object sender, System.EventArgs e)
		{
			if( UpdateVariable != null)
			{
				UpdateVariable( "RestBelowHealth", (float) nudRestBelowHealth.Value);
				UpdateVariable( "RestBelowMana", (float) nudRestBelowMana.Value);
				UpdateVariable( "RestBelowEndurance", (float) nudRestBelowEndurance.Value);
				UpdateVariable( "RestUntilFullyRecovered", chkFullyRecovered.Checked);
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
