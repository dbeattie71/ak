//------------------------------------------------------------------------------
//-------------------------------------------------------------------------------
using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Text.RegularExpressions;

namespace DAoC_Bot
{
	/// <summary>
	/// Used to update variables in the profile
	/// </summary>

	/// <summary>
	/// Summary description for frmStatistics.
	/// </summary>
	public class frmStatistics : System.Windows.Forms.Form
	{
		private System.Windows.Forms.Panel panel3;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.TabControl tabControl1;
		private System.Windows.Forms.TabPage tabLog;
		private System.Windows.Forms.TabPage tabKillReport;
		private System.Windows.Forms.ListBox lbxStatus;
		private System.Windows.Forms.ListView lvwReport;
		private System.Windows.Forms.ColumnHeader columnHeader1;
		private System.Windows.Forms.ColumnHeader columnHeader2;
		private System.Windows.Forms.Button btnReset;
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;
		private System.Windows.Forms.ColumnHeader columnHeader3;
		private System.Windows.Forms.ColumnHeader columnHeader4;
		private AutoKillerScript.clsAutoKillerScript _ak;
		private System.Windows.Forms.Label lblExpPerHour;
		DateTime botStartTime = DateTime.Now;
		private Regex _combat_xp_gain;
		private Regex _combat_monster_name;

		public frmStatistics( AutoKillerScript.clsAutoKillerScript ak)
		{
			_ak = ak;
			_ak.OnRegExTrue += new AutoKillerScript.clsAutoKillerScript.OnRegExTrueEventHandler(XPNotification);
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();

			//
			// TODO: Add any constructor code after InitializeComponent call
			//

			//You get 1,216 experience points. (576 camp bonus)
			_combat_xp_gain = new Regex( "get (?<xp>.+) experience");

			//The zombie servant kills the lesser water elemental!
			_combat_monster_name = new Regex( "The .+ kills the (?<monster>.+)!");

		}

		public System.Windows.Forms.ListView Report
		{
			get
			{
				return lvwReport;
			}
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
			this.tabControl1 = new System.Windows.Forms.TabControl();
			this.tabLog = new System.Windows.Forms.TabPage();
			this.lbxStatus = new System.Windows.Forms.ListBox();
			this.tabKillReport = new System.Windows.Forms.TabPage();
			this.lvwReport = new System.Windows.Forms.ListView();
			this.columnHeader1 = new System.Windows.Forms.ColumnHeader();
			this.columnHeader2 = new System.Windows.Forms.ColumnHeader();
			this.columnHeader3 = new System.Windows.Forms.ColumnHeader();
			this.columnHeader4 = new System.Windows.Forms.ColumnHeader();
			this.btnReset = new System.Windows.Forms.Button();
			this.lblExpPerHour = new System.Windows.Forms.Label();
			this.panel3.SuspendLayout();
			this.tabControl1.SuspendLayout();
			this.tabLog.SuspendLayout();
			this.tabKillReport.SuspendLayout();
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
			this.panel3.TabIndex = 15;
			// 
			// label2
			// 
			this.label2.BackColor = System.Drawing.SystemColors.ActiveCaption;
			this.label2.Dock = System.Windows.Forms.DockStyle.Fill;
			this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.label2.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
			this.label2.Location = new System.Drawing.Point(0, 0);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(331, 20);
			this.label2.TabIndex = 0;
			this.label2.Text = "Statistics";
			this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// tabControl1
			// 
			this.tabControl1.Controls.Add(this.tabLog);
			this.tabControl1.Controls.Add(this.tabKillReport);
			this.tabControl1.Location = new System.Drawing.Point(8, 56);
			this.tabControl1.Name = "tabControl1";
			this.tabControl1.SelectedIndex = 0;
			this.tabControl1.Size = new System.Drawing.Size(328, 296);
			this.tabControl1.TabIndex = 16;
			// 
			// tabLog
			// 
			this.tabLog.Controls.Add(this.lbxStatus);
			this.tabLog.Location = new System.Drawing.Point(4, 22);
			this.tabLog.Name = "tabLog";
			this.tabLog.Size = new System.Drawing.Size(320, 270);
			this.tabLog.TabIndex = 0;
			this.tabLog.Text = "Current Action";
			// 
			// lbxStatus
			// 
			this.lbxStatus.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right)));
			this.lbxStatus.HorizontalScrollbar = true;
			this.lbxStatus.Location = new System.Drawing.Point(8, 8);
			this.lbxStatus.Name = "lbxStatus";
			this.lbxStatus.Size = new System.Drawing.Size(304, 251);
			this.lbxStatus.TabIndex = 0;
			// 
			// tabKillReport
			// 
			this.tabKillReport.Controls.Add(this.lvwReport);
			this.tabKillReport.Location = new System.Drawing.Point(4, 22);
			this.tabKillReport.Name = "tabKillReport";
			this.tabKillReport.Size = new System.Drawing.Size(320, 270);
			this.tabKillReport.TabIndex = 1;
			this.tabKillReport.Text = "Kills Report";
			// 
			// lvwReport
			// 
			this.lvwReport.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
																						this.columnHeader1,
																						this.columnHeader2,
																						this.columnHeader3,
																						this.columnHeader4});
			this.lvwReport.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable;
			this.lvwReport.Location = new System.Drawing.Point(8, 8);
			this.lvwReport.Name = "lvwReport";
			this.lvwReport.Size = new System.Drawing.Size(304, 256);
			this.lvwReport.TabIndex = 0;
			this.lvwReport.View = System.Windows.Forms.View.Details;
			// 
			// columnHeader1
			// 
			this.columnHeader1.Text = "Name";
			this.columnHeader1.Width = 140;
			// 
			// columnHeader2
			// 
			this.columnHeader2.Text = "Kills";
			this.columnHeader2.Width = 40;
			// 
			// columnHeader3
			// 
			this.columnHeader3.Text = "Total XP";
			// 
			// columnHeader4
			// 
			this.columnHeader4.Text = "Avg. XP";
			// 
			// btnReset
			// 
			this.btnReset.Location = new System.Drawing.Point(232, 32);
			this.btnReset.Name = "btnReset";
			this.btnReset.Size = new System.Drawing.Size(75, 21);
			this.btnReset.TabIndex = 17;
			this.btnReset.Text = "Reset";
			this.btnReset.Click += new System.EventHandler(this.btnReset_Click);
			// 
			// lblExpPerHour
			// 
			this.lblExpPerHour.Location = new System.Drawing.Point(16, 32);
			this.lblExpPerHour.Name = "lblExpPerHour";
			this.lblExpPerHour.Size = new System.Drawing.Size(200, 16);
			this.lblExpPerHour.TabIndex = 0;
			this.lblExpPerHour.Text = "XXXXX XP/Hour";
			// 
			// frmStatistics
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.ClientSize = new System.Drawing.Size(343, 362);
			this.Controls.Add(this.lblExpPerHour);
			this.Controls.Add(this.btnReset);
			this.Controls.Add(this.tabControl1);
			this.Controls.Add(this.panel3);
			this.DockPadding.All = 4;
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
			this.Name = "frmStatistics";
			this.Text = "frmStatistics";
			this.panel3.ResumeLayout(false);
			this.tabControl1.ResumeLayout(false);
			this.tabLog.ResumeLayout(false);
			this.tabKillReport.ResumeLayout(false);
			this.ResumeLayout(false);

		}
		#endregion

		internal void ConnectDelegates(DAoC_BotBase botbase)
		{
			//TODO: okay I know there must be a right way to do this, like the inherited profile class
			// but we also need to time this correctly after botbase is (re)created
			botbase.UpdateStatus += new DAoC_BotBase.UpdateStatusDelegate(AddMessage);
		}

		internal void AddMessage(string str)
		{
			while(str.IndexOf('\0') != -1)
				str.Remove(str.IndexOf('\0'),1);
			lbxStatus.Items.Insert(0,str);
			if(lbxStatus.Items.Count > 1000) //if more than 1000 lines remove last line
				lbxStatus.Items.RemoveAt(lbxStatus.Items.Count-1); 
			lbxStatus.Update();
		}

		private void btnReset_Click(object sender, System.EventArgs e)
		{
			if(tabControl1.SelectedTab == tabLog)
				lbxStatus.Items.Clear();
			else if(tabControl1.SelectedTab == tabKillReport)
			{
				lvwReport.Items.Clear();
			}
			botStartTime = DateTime.Now;
			lblExpPerHour.Text = "XXXXX Exp/Hour";
		}

		private void AddKill( string monster, int xp)
		{
			ListViewItem item = null;
			foreach( ListViewItem lvi in lvwReport.Items)
			{
				if( lvi.Text == monster)
				{
					item = lvi;
					break;
				}
			}

			if( item == null)
			{
				item = new ListViewItem();
				item.Text = monster;
				item.SubItems.Add( "0");
				item.SubItems.Add( "0");
				item.SubItems.Add( "0");

				// Add player deaths / total stats at bottom, rest at top
				if( monster == "Player Deaths" || monster == "Total Kills")
					lvwReport.Items.Add( item);
				else
					lvwReport.Items.Insert( 0, item);
			}

			int kills = int.Parse( item.SubItems[1].Text) + 1;
			int totalxp = int.Parse( item.SubItems[2].Text) + xp;

			item.SubItems[1].Text = kills.ToString();
			item.SubItems[2].Text = totalxp.ToString();
			item.SubItems[3].Text = ((float)totalxp / (float)kills).ToString("N");

			// Add EXP Per Hour 
			DateTime botCheckTime;
			TimeSpan elapsedTime;
			int secondsElapsed;
			System.Globalization.NumberFormatInfo nfi = new System.Globalization.CultureInfo( "en-US", false ).NumberFormat;
			nfi.NumberDecimalDigits = 0;

			botCheckTime = DateTime.Now;
			elapsedTime = botCheckTime - botStartTime;
			secondsElapsed = (int)elapsedTime.TotalSeconds;
			int ExpPerHour = Convert.ToInt32((totalxp /(secondsElapsed / 3600.00)));
			lblExpPerHour.Text = ExpPerHour.ToString("N", nfi) + "   XP/Hour";
		}

		string lastMonster = "";
		// The XP gotten through this function does not include rested xp
		private void XPNotification( AutoKillerScript.clsAutoKillerScript.AutokillerRegExEventParams e)
		{
			string xp = e.Logline;
			//we watch for two kinds of lines
			//The zombie servant kills the lesser water elemental!
			//You get 1,216 experience points. (576 camp bonus)

			Match mm = _combat_monster_name.Match( xp);
			if( mm.Success)
			{
				lastMonster =  mm.Groups["monster"].Value  ;
			}

			Match m = _combat_xp_gain.Match( xp);
			if( m.Success)
			{
				int ixp = int.Parse(m.Groups["xp"].Value, System.Globalization.NumberStyles.AllowThousands );
				
				AddKill( lastMonster, ixp);
				AddKill( "Total Kills", ixp);
			}

			if(xp.IndexOf("You have died.") != -1)
				AddKill( "Player Deaths", 0);
		}

	}
}
