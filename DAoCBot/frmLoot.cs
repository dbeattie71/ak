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
	/// Summary description for frmLoot.
	/// </summary>
	public class frmLoot : System.Windows.Forms.Form, IProfileForm
	{
		private System.ComponentModel.IContainer components;
		private System.Windows.Forms.ToolTip toolTip1;
		private System.Windows.Forms.Label label4;
		private System.Windows.Forms.Panel panel3;
		private System.Windows.Forms.Panel panel7;
		private System.Windows.Forms.Panel panel5;
		private System.Windows.Forms.Button btnAdd;
		private System.Windows.Forms.Button btnDelete;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Panel panel2;
		private System.Windows.Forms.Panel panel1;
		private System.Windows.Forms.CheckBox chkDoLooting;
		private System.Windows.Forms.TextBox txtAddKeyword;
		private System.Windows.Forms.ListView lvwBadLoot;
		private System.Windows.Forms.Panel panel6;
		private System.Windows.Forms.Button btnCancel;
		private System.Windows.Forms.Button btnApply;
		private System.Windows.Forms.CheckBox chkMoneyOnly;
		private System.Windows.Forms.ColumnHeader chName;

		public frmLoot(AutoKillerScript.clsAutoKillerScript ak)
		{
			_ak = ak;

			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();

			//
			// TODO: Add any constructor code after InitializeComponent call
			//
			this.lvwBadLoot.Validated +=new EventHandler(lvwBadLoot_Validated);

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
		private void InitializeComponent() {
			this.components = new System.ComponentModel.Container();
			this.chName = new System.Windows.Forms.ColumnHeader();
			this.panel5 = new System.Windows.Forms.Panel();
			this.txtAddKeyword = new System.Windows.Forms.TextBox();
			this.btnAdd = new System.Windows.Forms.Button();
			this.lvwBadLoot = new System.Windows.Forms.ListView();
			this.btnDelete = new System.Windows.Forms.Button();
			this.panel7 = new System.Windows.Forms.Panel();
			this.label4 = new System.Windows.Forms.Label();
			this.panel3 = new System.Windows.Forms.Panel();
			this.label2 = new System.Windows.Forms.Label();
			this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
			this.label1 = new System.Windows.Forms.Label();
			this.panel2 = new System.Windows.Forms.Panel();
			this.panel1 = new System.Windows.Forms.Panel();
			this.chkDoLooting = new System.Windows.Forms.CheckBox();
			this.panel6 = new System.Windows.Forms.Panel();
			this.btnCancel = new System.Windows.Forms.Button();
			this.btnApply = new System.Windows.Forms.Button();
			this.chkMoneyOnly = new System.Windows.Forms.CheckBox();
			this.panel5.SuspendLayout();
			this.panel7.SuspendLayout();
			this.panel3.SuspendLayout();
			this.panel2.SuspendLayout();
			this.panel1.SuspendLayout();
			this.panel6.SuspendLayout();
			this.SuspendLayout();
			// 
			// chName
			// 
			this.chName.Text = "Key Word (loot with these words in name will not be collected)";
			this.chName.Width = 315;
			// 
			// panel5
			// 
			this.panel5.Controls.Add(this.txtAddKeyword);
			this.panel5.Controls.Add(this.btnAdd);
			this.panel5.Controls.Add(this.lvwBadLoot);
			this.panel5.Controls.Add(this.btnDelete);
			this.panel5.Dock = System.Windows.Forms.DockStyle.Top;
			this.panel5.Location = new System.Drawing.Point(3, 96);
			this.panel5.Name = "panel5";
			this.panel5.Size = new System.Drawing.Size(336, 224);
			this.panel5.TabIndex = 17;
			// 
			// txtAddKeyword
			// 
			this.txtAddKeyword.Location = new System.Drawing.Point(16, 8);
			this.txtAddKeyword.Name = "txtAddKeyword";
			this.txtAddKeyword.Size = new System.Drawing.Size(136, 20);
			this.txtAddKeyword.TabIndex = 3;
			this.txtAddKeyword.Text = "Enter new Keyword here";
			// 
			// btnAdd
			// 
			this.btnAdd.Location = new System.Drawing.Point(168, 8);
			this.btnAdd.Name = "btnAdd";
			this.btnAdd.Size = new System.Drawing.Size(56, 22);
			this.btnAdd.TabIndex = 2;
			this.btnAdd.Text = "Add";
			this.toolTip1.SetToolTip(this.btnAdd, "Add the current players position in the game as a waypoint");
			this.btnAdd.Click += new System.EventHandler(this.btnAdd_Click);
			// 
			// lvwBadLoot
			// 
			this.lvwBadLoot.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
																						 this.chName});
			this.lvwBadLoot.FullRowSelect = true;
			this.lvwBadLoot.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable;
			this.lvwBadLoot.HideSelection = false;
			this.lvwBadLoot.LabelEdit = true;
			this.lvwBadLoot.LabelWrap = false;
			this.lvwBadLoot.Location = new System.Drawing.Point(8, 40);
			this.lvwBadLoot.Name = "lvwBadLoot";
			this.lvwBadLoot.Size = new System.Drawing.Size(320, 176);
			this.lvwBadLoot.TabIndex = 1;
			this.lvwBadLoot.View = System.Windows.Forms.View.Details;
			// 
			// btnDelete
			// 
			this.btnDelete.Location = new System.Drawing.Point(224, 8);
			this.btnDelete.Name = "btnDelete";
			this.btnDelete.Size = new System.Drawing.Size(96, 22);
			this.btnDelete.TabIndex = 2;
			this.btnDelete.Text = "Delete Selected";
			this.toolTip1.SetToolTip(this.btnDelete, "Delete the selected key words");
			this.btnDelete.Click += new System.EventHandler(this.btnWaypointDelete_Click);
			// 
			// panel7
			// 
			this.panel7.BackColor = System.Drawing.SystemColors.ControlDark;
			this.panel7.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
			this.panel7.Controls.Add(this.label4);
			this.panel7.Dock = System.Windows.Forms.DockStyle.Top;
			this.panel7.Location = new System.Drawing.Point(3, 80);
			this.panel7.Name = "panel7";
			this.panel7.Size = new System.Drawing.Size(336, 16);
			this.panel7.TabIndex = 20;
			// 
			// label4
			// 
			this.label4.Dock = System.Windows.Forms.DockStyle.Fill;
			this.label4.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
			this.label4.Location = new System.Drawing.Point(0, 0);
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size(332, 12);
			this.label4.TabIndex = 0;
			this.label4.Text = "Enter or Change Key Words to Skip";
			this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// panel3
			// 
			this.panel3.BackColor = System.Drawing.SystemColors.ActiveCaption;
			this.panel3.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
			this.panel3.Controls.Add(this.label2);
			this.panel3.Dock = System.Windows.Forms.DockStyle.Top;
			this.panel3.Location = new System.Drawing.Point(3, 3);
			this.panel3.Name = "panel3";
			this.panel3.Size = new System.Drawing.Size(336, 23);
			this.panel3.TabIndex = 14;
			// 
			// label2
			// 
			this.label2.BackColor = System.Drawing.SystemColors.ActiveCaption;
			this.label2.Dock = System.Windows.Forms.DockStyle.Fill;
			this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.label2.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
			this.label2.Location = new System.Drawing.Point(0, 0);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(332, 19);
			this.label2.TabIndex = 0;
			this.label2.Text = "Loot";
			this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// label1
			// 
			this.label1.BackColor = System.Drawing.SystemColors.ControlDark;
			this.label1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.label1.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
			this.label1.Location = new System.Drawing.Point(0, 0);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(332, 11);
			this.label1.TabIndex = 0;
			this.label1.Text = "Do Looting";
			this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// panel2
			// 
			this.panel2.BackColor = System.Drawing.SystemColors.ControlDark;
			this.panel2.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
			this.panel2.Controls.Add(this.label1);
			this.panel2.Dock = System.Windows.Forms.DockStyle.Top;
			this.panel2.Location = new System.Drawing.Point(3, 26);
			this.panel2.Name = "panel2";
			this.panel2.Size = new System.Drawing.Size(336, 15);
			this.panel2.TabIndex = 14;
			// 
			// panel1
			// 
			this.panel1.Controls.Add(this.chkDoLooting);
			this.panel1.Controls.Add(this.chkMoneyOnly);
			this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
			this.panel1.Location = new System.Drawing.Point(3, 41);
			this.panel1.Name = "panel1";
			this.panel1.Size = new System.Drawing.Size(336, 39);
			this.panel1.TabIndex = 15;
			// 
			// chkDoLooting
			// 
			this.chkDoLooting.Location = new System.Drawing.Point(40, 8);
			this.chkDoLooting.Name = "chkDoLooting";
			this.chkDoLooting.TabIndex = 0;
			this.chkDoLooting.Text = "Pick up Loot";
			// 
			// panel6
			// 
			this.panel6.Controls.Add(this.btnCancel);
			this.panel6.Controls.Add(this.btnApply);
			this.panel6.Dock = System.Windows.Forms.DockStyle.Bottom;
			this.panel6.Location = new System.Drawing.Point(3, 335);
			this.panel6.Name = "panel6";
			this.panel6.Size = new System.Drawing.Size(336, 32);
			this.panel6.TabIndex = 21;
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
			// chkMoneyOnly
			// 
			this.chkMoneyOnly.Location = new System.Drawing.Point(200, 8);
			this.chkMoneyOnly.Name = "chkMoneyOnly";
			this.chkMoneyOnly.TabIndex = 0;
			this.chkMoneyOnly.Text = "Money Only";
			// 
			// frmLoot
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.ClientSize = new System.Drawing.Size(342, 370);
			this.Controls.Add(this.panel6);
			this.Controls.Add(this.panel5);
			this.Controls.Add(this.panel7);
			this.Controls.Add(this.panel1);
			this.Controls.Add(this.panel2);
			this.Controls.Add(this.panel3);
			this.DockPadding.All = 3;
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
			this.Name = "frmLoot";
			this.Text = "frmLoot";
			this.panel5.ResumeLayout(false);
			this.panel7.ResumeLayout(false);
			this.panel3.ResumeLayout(false);
			this.panel2.ResumeLayout(false);
			this.panel1.ResumeLayout(false);
			this.panel6.ResumeLayout(false);
			this.ResumeLayout(false);

		}
		#endregion

		private AutoKillerScript.clsAutoKillerScript  _ak;

		#region keyword add, delete
		
		private void btnAdd_Click(object sender, System.EventArgs e)
		{
			if(txtAddKeyword.Text == "Enter new Keyword here" || txtAddKeyword.Text == "")
			{
				MessageBox.Show("Please enter a keyword to indicate loot to be ignored.", "Add Keyword Error",
					MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
				return;
			}

			lvwBadLoot.Items.Add(txtAddKeyword.Text);
			
			SaveKeywords();
		}

		private void lvwBadLoot_Validated(object sender, EventArgs e)
		{
			//save any changes to the patrol area, need to do this here to capture inline edits to the name
			SaveKeywords();
		}

		private void btnWaypointDelete_Click(object sender, System.EventArgs e)
		{
			while(lvwBadLoot.SelectedIndices.Count > 0)
				lvwBadLoot.Items.RemoveAt(lvwBadLoot.SelectedIndices[0]);

			SaveKeywords();
		}
		#endregion

		#region utilities
		private void SaveKeywords()
		{
			string saveStr = "";

			//get each ListViewItem from the ListView and save its attached waypoint object
			foreach (ListViewItem lvi in lvwBadLoot.Items)
			{
				saveStr += lvi.Text ;
				saveStr += "|";
			}

			UpdateVariable("BadLoot",saveStr);
		}

		#endregion

		#region IProfileForm handling routines
		public event UpdateVariableDelegate UpdateVariable;

		public event CurrentProfileDelegate LoadCurrentProfile; // This event is never used
		public event CurrentProfileDelegate SaveCurrentProfile;

		public void DefineVariables( Profile profile)
		{
			profile.DefineVariable( "BadLoot", "");
			profile.DefineVariable("MoneyOnly", false);
			profile.DefineVariable("DoLooting", true);
		}

		public void OnProfileChange( Profile profile)
		{
			lvwBadLoot.Items.Clear();

			
			string saveStr = profile.GetString("BadLoot");

			char delimiter = '|';
			string [ ] itemssplit = saveStr.Split ( delimiter );
			foreach (string keyWord in itemssplit) 
			{
				if ( keyWord != "" ) 
				{
					lvwBadLoot.Items.Add(keyWord);
				}
				
			}

			chkDoLooting.Checked = profile.GetBool("DoLooting");
			chkMoneyOnly.Checked = profile.GetBool("MoneyOnly");
		}		
		#endregion

		private void btnApply_Click(object sender, System.EventArgs e)
		{
			UpdateVariable("DoLooting", chkDoLooting.Checked);
			UpdateVariable("MoneyOnly", chkMoneyOnly.Checked);

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
