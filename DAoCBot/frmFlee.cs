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
	public class frmFlee : System.Windows.Forms.Form, IProfileForm
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
		private System.Windows.Forms.NumericUpDown nudFleeBelowMana;
		private System.Windows.Forms.NumericUpDown nudFleeBelowHealth;
		private System.Windows.Forms.Label lblMana;
		private System.Windows.Forms.ToolTip toolTip1;
		private System.Windows.Forms.Label label3;
		private System.ComponentModel.IContainer components;

		public frmFlee()
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
			this.label3 = new System.Windows.Forms.Label();
			this.nudFleeBelowMana = new System.Windows.Forms.NumericUpDown();
			this.nudFleeBelowHealth = new System.Windows.Forms.NumericUpDown();
			this.lblMana = new System.Windows.Forms.Label();
			this.label4 = new System.Windows.Forms.Label();
			this.panel6 = new System.Windows.Forms.Panel();
			this.btnCancel = new System.Windows.Forms.Button();
			this.btnApply = new System.Windows.Forms.Button();
			this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
			this.panel3.SuspendLayout();
			this.panel2.SuspendLayout();
			this.panel1.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.nudFleeBelowMana)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.nudFleeBelowHealth)).BeginInit();
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
			this.label2.Text = "Flee Settings";
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
			this.label1.Text = "Run Away from a fight when";
			this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// panel1
			// 
			this.panel1.Controls.Add(this.label3);
			this.panel1.Controls.Add(this.nudFleeBelowMana);
			this.panel1.Controls.Add(this.nudFleeBelowHealth);
			this.panel1.Controls.Add(this.lblMana);
			this.panel1.Controls.Add(this.label4);
			this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
			this.panel1.Location = new System.Drawing.Point(4, 44);
			this.panel1.Name = "panel1";
			this.panel1.Size = new System.Drawing.Size(284, 204);
			this.panel1.TabIndex = 17;
			// 
			// label3
			// 
			this.label3.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.label3.Location = new System.Drawing.Point(166, 16);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(112, 88);
			this.label3.TabIndex = 29;
			this.label3.Text = "These settings are for fighting a single attacker. They will be automatically adj" +
				"usted if fighting multiple attackers";
			this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// nudFleeBelowMana
			// 
			this.nudFleeBelowMana.Location = new System.Drawing.Point(96, 56);
			this.nudFleeBelowMana.Name = "nudFleeBelowMana";
			this.nudFleeBelowMana.Size = new System.Drawing.Size(64, 20);
			this.nudFleeBelowMana.TabIndex = 25;
			this.toolTip1.SetToolTip(this.nudFleeBelowMana, "Run away if your Mana is below this, and your targets health is higher than your " +
				"health");
			// 
			// nudFleeBelowHealth
			// 
			this.nudFleeBelowHealth.Location = new System.Drawing.Point(96, 32);
			this.nudFleeBelowHealth.Name = "nudFleeBelowHealth";
			this.nudFleeBelowHealth.Size = new System.Drawing.Size(64, 20);
			this.nudFleeBelowHealth.TabIndex = 24;
			this.toolTip1.SetToolTip(this.nudFleeBelowHealth, "Run away if your Health is below this, and your targets health is higher than you" +
				"r health");
			// 
			// lblMana
			// 
			this.lblMana.Location = new System.Drawing.Point(8, 56);
			this.lblMana.Name = "lblMana";
			this.lblMana.Size = new System.Drawing.Size(88, 20);
			this.lblMana.TabIndex = 23;
			this.lblMana.Text = "Mana is below:";
			this.lblMana.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// label4
			// 
			this.label4.Location = new System.Drawing.Point(8, 32);
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size(88, 20);
			this.label4.TabIndex = 21;
			this.label4.Text = "Health is below:";
			this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// panel6
			// 
			this.panel6.Controls.Add(this.btnCancel);
			this.panel6.Controls.Add(this.btnApply);
			this.panel6.Dock = System.Windows.Forms.DockStyle.Bottom;
			this.panel6.Location = new System.Drawing.Point(4, 288);
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
			// frmFlee
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.ClientSize = new System.Drawing.Size(292, 320);
			this.Controls.Add(this.panel6);
			this.Controls.Add(this.panel1);
			this.Controls.Add(this.panel2);
			this.Controls.Add(this.panel3);
			this.DockPadding.All = 4;
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
			this.Name = "frmFlee";
			this.Text = "frmFlee";
			this.panel3.ResumeLayout(false);
			this.panel2.ResumeLayout(false);
			this.panel1.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.nudFleeBelowMana)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.nudFleeBelowHealth)).EndInit();
			this.panel6.ResumeLayout(false);
			this.ResumeLayout(false);

		}
		#endregion

		public event UpdateVariableDelegate UpdateVariable;
		public event CurrentProfileDelegate SaveCurrentProfile;
		public event CurrentProfileDelegate LoadCurrentProfile;

		public void DefineVariables( Profile profile)
		{
			profile.DefineVariable( "FleeBelowHealth", 40);
			profile.DefineVariable( "FleeBelowMana", 15);
		}
		
		public void OnProfileChange( Profile profile)
		{
			nudFleeBelowHealth.Value = (decimal) profile.GetInteger("FleeBelowHealth");
			nudFleeBelowMana.Value = (decimal) profile.GetInteger("FleeBelowMana");
			
			// Check if we need to show mana settings
			switch(profile.GetString("Class"))
			{
				case "Necro":
				case "Wizard":
					lblMana.Visible = true;
					nudFleeBelowMana.Visible = true;
					break;

				default:
					lblMana.Visible = false;
					nudFleeBelowMana.Visible = false;
					break;
			}
		}


		private void btnApply_Click(object sender, System.EventArgs e)
		{
			if( UpdateVariable != null)
			{
				UpdateVariable( "FleeBelowHealth", (int) nudFleeBelowHealth.Value);
				UpdateVariable( "FleeBelowMana", (int) nudFleeBelowMana.Value);
			}
			if( SaveCurrentProfile != null)
				SaveCurrentProfile();
		}

		private void btnCancel_Click(object sender, System.EventArgs e)
		{
			if( LoadCurrentProfile != null)
				SaveCurrentProfile();
		}
	}
}
