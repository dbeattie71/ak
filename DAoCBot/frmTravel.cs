//------------------------------------------------------------------------------
//-------------------------------------------------------------------------------
using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Threading;
using AutoKillerScript;

namespace DAoC_Bot
{
	/// <summary>
	/// Summary description for frmTravel.
	/// </summary>
	public class frmTravel : System.Windows.Forms.Form
	{
		private System.Windows.Forms.ToolTip toolTip1;
		private System.Windows.Forms.Panel panel3;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.Panel panel2;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Panel panel1;
		internal System.Windows.Forms.ComboBox cbxPatrolAreas;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.Label label4;
		internal System.Windows.Forms.ComboBox cbxDestinations;
		private System.Windows.Forms.Panel panel4;
		private System.Windows.Forms.Label label6;
		private System.Windows.Forms.Panel panel5;
		private System.Windows.Forms.Button btnStart;
		private System.Windows.Forms.Button btnStop;
		private System.Windows.Forms.Panel panel6;
		private System.Windows.Forms.Label label7;
		private System.Windows.Forms.Panel panel7;
		private System.Windows.Forms.ListView lvwDisplay;
		private System.Windows.Forms.ColumnHeader columnHeader1;
		private System.Windows.Forms.ColumnHeader columnHeader2;
		private System.Windows.Forms.ColumnHeader columnHeader3;
		private System.Windows.Forms.CheckBox cbxStopOnDamage;
		private System.ComponentModel.IContainer components;

		public frmTravel(PatrolAreas patrolareas, AutoKillerScript.clsAutoKillerScript ak)
		{
			_patrolareas = patrolareas;
			_ak = ak;

			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();

			//
			// TODO: Add any constructor code after InitializeComponent call
			//
			cbxPatrolAreas.GotFocus += new EventHandler(cbxPatrolAreas_GotFocus);
			cbxDestinations.GotFocus +=new EventHandler(cbxDestinations_GotFocus);
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
			this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
			this.panel3 = new System.Windows.Forms.Panel();
			this.label2 = new System.Windows.Forms.Label();
			this.panel2 = new System.Windows.Forms.Panel();
			this.label1 = new System.Windows.Forms.Label();
			this.panel1 = new System.Windows.Forms.Panel();
			this.cbxPatrolAreas = new System.Windows.Forms.ComboBox();
			this.label3 = new System.Windows.Forms.Label();
			this.label4 = new System.Windows.Forms.Label();
			this.cbxDestinations = new System.Windows.Forms.ComboBox();
			this.panel4 = new System.Windows.Forms.Panel();
			this.label6 = new System.Windows.Forms.Label();
			this.panel5 = new System.Windows.Forms.Panel();
			this.btnStart = new System.Windows.Forms.Button();
			this.btnStop = new System.Windows.Forms.Button();
			this.panel6 = new System.Windows.Forms.Panel();
			this.label7 = new System.Windows.Forms.Label();
			this.panel7 = new System.Windows.Forms.Panel();
			this.lvwDisplay = new System.Windows.Forms.ListView();
			this.columnHeader1 = new System.Windows.Forms.ColumnHeader();
			this.columnHeader2 = new System.Windows.Forms.ColumnHeader();
			this.columnHeader3 = new System.Windows.Forms.ColumnHeader();
			this.cbxStopOnDamage = new System.Windows.Forms.CheckBox();
			this.panel3.SuspendLayout();
			this.panel2.SuspendLayout();
			this.panel1.SuspendLayout();
			this.panel4.SuspendLayout();
			this.panel5.SuspendLayout();
			this.panel6.SuspendLayout();
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
			this.label2.Text = "Travel";
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
			this.panel2.Size = new System.Drawing.Size(335, 16);
			this.panel2.TabIndex = 16;
			// 
			// label1
			// 
			this.label1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.label1.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
			this.label1.Location = new System.Drawing.Point(0, 0);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(331, 12);
			this.label1.TabIndex = 0;
			this.label1.Text = "Select the path to travel, and destination to stop at";
			this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// panel1
			// 
			this.panel1.Controls.Add(this.cbxStopOnDamage);
			this.panel1.Controls.Add(this.label3);
			this.panel1.Controls.Add(this.cbxPatrolAreas);
			this.panel1.Controls.Add(this.label4);
			this.panel1.Controls.Add(this.cbxDestinations);
			this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
			this.panel1.Location = new System.Drawing.Point(4, 44);
			this.panel1.Name = "panel1";
			this.panel1.Size = new System.Drawing.Size(335, 92);
			this.panel1.TabIndex = 17;
			// 
			// cbxPatrolAreas
			// 
			this.cbxPatrolAreas.Location = new System.Drawing.Point(120, 8);
			this.cbxPatrolAreas.Name = "cbxPatrolAreas";
			this.cbxPatrolAreas.Size = new System.Drawing.Size(200, 21);
			this.cbxPatrolAreas.Sorted = true;
			this.cbxPatrolAreas.TabIndex = 18;
			this.toolTip1.SetToolTip(this.cbxPatrolAreas, "Select what Travel Path or Patrol Area you want to run along");
			// 
			// label3
			// 
			this.label3.Location = new System.Drawing.Point(8, 12);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(88, 15);
			this.label3.TabIndex = 19;
			this.label3.Text = "Available Paths:";
			// 
			// label4
			// 
			this.label4.Location = new System.Drawing.Point(8, 40);
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size(80, 15);
			this.label4.TabIndex = 19;
			this.label4.Text = "Destination:";
			// 
			// cbxDestinations
			// 
			this.cbxDestinations.Location = new System.Drawing.Point(120, 37);
			this.cbxDestinations.Name = "cbxDestinations";
			this.cbxDestinations.Size = new System.Drawing.Size(200, 21);
			this.cbxDestinations.TabIndex = 18;
			this.toolTip1.SetToolTip(this.cbxDestinations, "Optionally, select what named waypoint you want to stop at");
			// 
			// panel4
			// 
			this.panel4.BackColor = System.Drawing.SystemColors.ControlDark;
			this.panel4.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
			this.panel4.Controls.Add(this.label6);
			this.panel4.Dock = System.Windows.Forms.DockStyle.Top;
			this.panel4.Location = new System.Drawing.Point(4, 136);
			this.panel4.Name = "panel4";
			this.panel4.Size = new System.Drawing.Size(335, 16);
			this.panel4.TabIndex = 18;
			// 
			// label6
			// 
			this.label6.Dock = System.Windows.Forms.DockStyle.Fill;
			this.label6.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
			this.label6.Location = new System.Drawing.Point(0, 0);
			this.label6.Name = "label6";
			this.label6.Size = new System.Drawing.Size(331, 12);
			this.label6.TabIndex = 0;
			this.label6.Text = "Move";
			this.label6.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// panel5
			// 
			this.panel5.Controls.Add(this.btnStart);
			this.panel5.Controls.Add(this.btnStop);
			this.panel5.Dock = System.Windows.Forms.DockStyle.Top;
			this.panel5.Location = new System.Drawing.Point(4, 152);
			this.panel5.Name = "panel5";
			this.panel5.Size = new System.Drawing.Size(335, 40);
			this.panel5.TabIndex = 19;
			// 
			// btnStart
			// 
			this.btnStart.Location = new System.Drawing.Point(63, 8);
			this.btnStart.Name = "btnStart";
			this.btnStart.Size = new System.Drawing.Size(88, 21);
			this.btnStart.TabIndex = 0;
			this.btnStart.Text = "Start Running";
			this.toolTip1.SetToolTip(this.btnStart, "Start running to the selected Destination");
			this.btnStart.Click += new System.EventHandler(this.btnStart_Click);
			// 
			// btnStop
			// 
			this.btnStop.Location = new System.Drawing.Point(183, 8);
			this.btnStop.Name = "btnStop";
			this.btnStop.Size = new System.Drawing.Size(88, 21);
			this.btnStop.TabIndex = 0;
			this.btnStop.Text = "Stop Running";
			this.toolTip1.SetToolTip(this.btnStop, "Stop running at the next waypoint");
			this.btnStop.Click += new System.EventHandler(this.btnStop_Click);
			// 
			// panel6
			// 
			this.panel6.BackColor = System.Drawing.SystemColors.ControlDark;
			this.panel6.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
			this.panel6.Controls.Add(this.label7);
			this.panel6.Dock = System.Windows.Forms.DockStyle.Top;
			this.panel6.Location = new System.Drawing.Point(4, 192);
			this.panel6.Name = "panel6";
			this.panel6.Size = new System.Drawing.Size(335, 16);
			this.panel6.TabIndex = 20;
			// 
			// label7
			// 
			this.label7.Dock = System.Windows.Forms.DockStyle.Fill;
			this.label7.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
			this.label7.Location = new System.Drawing.Point(0, 0);
			this.label7.Name = "label7";
			this.label7.Size = new System.Drawing.Size(331, 12);
			this.label7.TabIndex = 0;
			this.label7.Text = "Update Display";
			this.label7.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// panel7
			// 
			this.panel7.Controls.Add(this.lvwDisplay);
			this.panel7.Dock = System.Windows.Forms.DockStyle.Fill;
			this.panel7.Location = new System.Drawing.Point(4, 208);
			this.panel7.Name = "panel7";
			this.panel7.Size = new System.Drawing.Size(335, 150);
			this.panel7.TabIndex = 21;
			// 
			// lvwDisplay
			// 
			this.lvwDisplay.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
																						 this.columnHeader1,
																						 this.columnHeader2,
																						 this.columnHeader3});
			this.lvwDisplay.GridLines = true;
			this.lvwDisplay.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable;
			this.lvwDisplay.Location = new System.Drawing.Point(8, 8);
			this.lvwDisplay.MultiSelect = false;
			this.lvwDisplay.Name = "lvwDisplay";
			this.lvwDisplay.Size = new System.Drawing.Size(320, 136);
			this.lvwDisplay.TabIndex = 0;
			this.lvwDisplay.View = System.Windows.Forms.View.Details;
			// 
			// columnHeader1
			// 
			this.columnHeader1.Text = "Running to Waypoint";
			this.columnHeader1.Width = 120;
			// 
			// columnHeader2
			// 
			this.columnHeader2.Text = "Coordinates";
			this.columnHeader2.Width = 120;
			// 
			// columnHeader3
			// 
			this.columnHeader3.Text = "Distance";
			// 
			// cbxStopOnDamage
			// 
			this.cbxStopOnDamage.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.cbxStopOnDamage.Location = new System.Drawing.Point(8, 68);
			this.cbxStopOnDamage.Name = "cbxStopOnDamage";
			this.cbxStopOnDamage.Size = new System.Drawing.Size(126, 15);
			this.cbxStopOnDamage.TabIndex = 21;
			this.cbxStopOnDamage.Text = "Stop on Damage";
			this.toolTip1.SetToolTip(this.cbxStopOnDamage, "Check this if you want to abort running if your health goes down");
			// 
			// frmTravel
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.ClientSize = new System.Drawing.Size(343, 362);
			this.Controls.Add(this.panel7);
			this.Controls.Add(this.panel6);
			this.Controls.Add(this.panel5);
			this.Controls.Add(this.panel4);
			this.Controls.Add(this.panel1);
			this.Controls.Add(this.panel2);
			this.Controls.Add(this.panel3);
			this.DockPadding.All = 4;
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
			this.Name = "frmTravel";
			this.Text = "frmTravel";
			this.panel3.ResumeLayout(false);
			this.panel2.ResumeLayout(false);
			this.panel1.ResumeLayout(false);
			this.panel4.ResumeLayout(false);
			this.panel5.ResumeLayout(false);
			this.panel6.ResumeLayout(false);
			this.panel7.ResumeLayout(false);
			this.ResumeLayout(false);

		}
		#endregion

		private PatrolAreas _patrolareas;
		private AutoKillerScript.clsAutoKillerScript _ak;
		private bool _bKeepRunning = false;

		private void btnStart_Click(object sender, System.EventArgs e)
		{
			if(cbxPatrolAreas.SelectedIndex == -1)
				return;

			PatrolArea patrolarea;
			try
			{
				patrolarea = _patrolareas.GetPatrolArea(cbxPatrolAreas.SelectedItem.ToString());
			}
			catch( Exception E)
			{
				MessageBox.Show(E.Message , "Load Area Error",
					MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
				return;
			}

			// make sure this travel list has waypoints defined
			if(patrolarea.NumWaypoints < 1)
			{
				MessageBox.Show("There are no waypoints defined for this Travel Path / Patrol Area" , "No Waypoints Error",
					MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
				return;
			}

			// make sure the user selected a destination
			if(cbxDestinations.SelectedIndex == -1)
			{
				MessageBox.Show("You must select a Destination!" , "No Destination Error",
					MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
				return;
			}

			//lets see if the user is far away from the selected path/area and warn them
			Waypoint wpt = patrolarea.FindClosest(FindDirection()); //also sets next point in waypoint internal list iterations
			double dist = _ak.ZDistance(_ak.gPlayerXCoord,_ak.gPlayerYCoord,_ak.gPlayerZCoord, wpt.X, wpt.Y, wpt.Z );
			if(dist > 5000)
			{
				if(MessageBox.Show("You are far away from this Waypoint Path / Patrol Area, are you sure you want to run to this location?" , "Distance Warning",
					MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No )
					return;
			}

			Interaction.AppActivate(_ak.GameProcess);
			Thread.Sleep(750);

			//tell movement thread to run
			_bKeepRunning = true;
		}
		
		
		/// <summary>
		/// Finds which direction the currently selected Destination name is from the players position
		/// true = forward, false = back
		/// </summary>
		private MovementDirection FindDirection()
		{
			if(cbxDestinations.SelectedIndex == -1)
				return MovementDirection.Forward ; //shouldnt happen

			if(cbxDestinations.SelectedItem.ToString() == "First Waypoint")
				return MovementDirection.Back ;  //go back

			if(cbxDestinations.SelectedItem.ToString() == "Last Waypoint")
				return MovementDirection.Forward ;  //go forward

			PatrolArea patrolarea;
			try
			{
				patrolarea = _patrolareas.GetPatrolArea(cbxPatrolAreas.SelectedItem.ToString());
			}
			catch( Exception E)
			{
				MessageBox.Show(E.Message , "Load Area Error",
					MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
				return MovementDirection.Forward;
			}

			return patrolarea.GetDirectionTo(cbxDestinations.SelectedItem.ToString());

		}


		/// <summary>
		/// Thread for actual moving
		/// </summary>
		public void Movement()
		{
			MovementDirection bForward = MovementDirection.Forward ;

			if(_bKeepRunning)
			{
				//dont let the user change where we are going while we run
				cbxDestinations.Enabled = false;
				cbxPatrolAreas.Enabled = false;

				PatrolArea patrolarea;
				try
				{
					patrolarea = _patrolareas.GetPatrolArea(cbxPatrolAreas.SelectedItem.ToString());
				}
				catch( Exception E)
				{
					MessageBox.Show(E.Message , "Load Area Error",
						MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
					return;
				}

				//clear display of last time we ran
				lvwDisplay.Items.Clear();

				//determine direction
				bForward = FindDirection();

				//set patrolareas direction
				patrolarea.Direction = bForward;

				//find closest and set waypoint internal iterator to next waypoint in the correct direction
				//this also sets the waypoint internal direction setting
				Waypoint wpt = patrolarea.FindClosest(bForward);

				//get the next waypoint in the correct direction
				wpt = patrolarea.GetNextWaypoint();

				//run through all the waypoints
				while(wpt != null && _bKeepRunning && _ak.GameProcess != 0)
				{
					//update the display
					ListViewItem lvi = new ListViewItem(wpt.Name,0);
					lvi.SubItems.Add(wpt.X.ToString() + ", " + wpt.Y.ToString());
					lvi.SubItems.Add(_ak.ZDistance(_ak.gPlayerXCoord,_ak.gPlayerYCoord,_ak.gPlayerZCoord, wpt.X , wpt.Y , wpt.Z ).ToString("N"));
					lvwDisplay.Items.Add(lvi);
					lvwDisplay.EnsureVisible(lvwDisplay.Items.Count - 1);

					wpt.MoveTo(250,false,cbxStopOnDamage.Checked);

					//have we gotten to the selected destination?
					if(cbxDestinations.SelectedIndex != -1 && wpt.Name == cbxDestinations.SelectedItem.ToString())
						break;

					//get the next waypoint in the correct direction
					wpt = patrolarea.GetNextWaypoint();
				}

				_ak.StopRunning(); 

				string status = "Reached Destination";
				if( ! _bKeepRunning)
					status = "Stopped";
				ListViewItem doneLVI = new ListViewItem(status,0);
				lvwDisplay.Items.Add(doneLVI);
				lvwDisplay.EnsureVisible(lvwDisplay.Items.Count - 1);

				_bKeepRunning = false;

				//done running, let them change destinations again
				cbxDestinations.Enabled = true;
				cbxPatrolAreas.Enabled = true;
			}			

		}	
		private void btnStop_Click(object sender, System.EventArgs e)
		{
			_bKeepRunning = false;
			ListViewItem lvi = new ListViewItem("Stopping...",0);
			lvwDisplay.Items.Add(lvi);
			lvwDisplay.EnsureVisible(lvwDisplay.Items.Count - 1);
		}


		/// <summary>
		/// This allows us to make sure our comboBox contents AND SELECTION dont show
		/// a patrol area or travel path that have meanwhile been deleted by the user
		/// switching to another form, deleting, and then switching back here.
		/// </summary>
		private void cbxPatrolAreas_GotFocus(object sender, EventArgs e)
		{
			string oldSelect = "";
			
			//store what is selected before we reload the comboBox
			if(cbxPatrolAreas.SelectedIndex != -1)
				oldSelect = cbxPatrolAreas.SelectedItem.ToString();
			
			//clear and reload
			cbxPatrolAreas.Items.Clear();
			cbxPatrolAreas.Text="";
			foreach (string patrolareaname in _patrolareas.GetAllPatrolAreas())
				cbxPatrolAreas.Items.Add(patrolareaname);
			
			//if something had been selected, and its still a valid item, select it
			if(oldSelect != "" && cbxPatrolAreas.Items.Contains(oldSelect))
				cbxPatrolAreas.SelectedIndex = cbxPatrolAreas.Items.IndexOf(oldSelect);
		}

		/// <summary>
		/// This allows us to make sure our comboBox contents AND SELECTION dont show
		/// a patrol area or travel path that have meanwhile been deleted by the user
		/// switching to another form, deleting, and then switching back here.
		/// </summary>
		private void cbxDestinations_GotFocus(object sender, EventArgs e)
		{
			if(cbxPatrolAreas.SelectedIndex == -1)
				return;

			PatrolArea patrolarea;
			try
			{
				patrolarea = _patrolareas.GetPatrolArea(cbxPatrolAreas.SelectedItem.ToString());
			}
			catch( Exception E)
			{
				//if the item has disappeared, the user must have changed to patrol area or travel path form and deleted it
				cbxPatrolAreas.Items.RemoveAt(cbxPatrolAreas.SelectedIndex);
				cbxPatrolAreas.Text = "";
				cbxPatrolAreas.SelectedIndex = -1;

				MessageBox.Show(E.Message , "Load Area Error",
					MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
				
				return;
			}

			// make sure this travel list has waypoints defined
			if(patrolarea.NumWaypoints < 1)
			{
				return;
			}

			//store what is selected before we reload the comboBox
			string oldSelect = "";
			if(cbxDestinations.SelectedIndex != -1)
				oldSelect = cbxDestinations.SelectedItem.ToString();
			
			//clear old entries so we can reload
			cbxDestinations.Items.Clear();
			cbxDestinations.Text = "";

			cbxDestinations.Items.Add("First Waypoint");

			//then add every waypoint that the user named
			ArrayList waypoints = patrolarea.GetAllWaypoints;

			// Remember to lock on the SyncRoot before enumeration.
			lock(waypoints.SyncRoot)
			{
				foreach (Waypoint thiswpt in waypoints)
					if(thiswpt.Name != "auto add" && thiswpt.Name != "no name")
						cbxDestinations.Items.Add(thiswpt.Name);
			}

			cbxDestinations.Items.Add("Last Waypoint");

			//if something had been selected, and its still a valid item, select it again
			if(oldSelect != "" && cbxDestinations.Items.Contains(oldSelect))
				cbxDestinations.SelectedIndex = cbxDestinations.Items.IndexOf(oldSelect);
			else
				cbxDestinations.SelectedIndex = cbxDestinations.Items.IndexOf("Last Waypoint");
		}
	}
}
