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
	/// Summary description for frmPatrolArea.
	/// </summary>
	public class frmPatrolArea : System.Windows.Forms.Form, IProfileForm
	{
		private System.ComponentModel.IContainer components;
		private System.Windows.Forms.Button btnWaypointAuto;
		private System.Windows.Forms.Button btnDeletePatrolArea;
		internal System.Windows.Forms.ComboBox cbxPatrolAreas;
		private System.Windows.Forms.ListBox lbxTargets;
		private System.Windows.Forms.Button btnTargetDelete;
		private System.Windows.Forms.Label label4;
		private System.Windows.Forms.Button btnWaypointMoveUp;
		private System.Windows.Forms.Button btnWaypointDelete;
		private System.Windows.Forms.Button btnGetTarget1;
		private System.Windows.Forms.ListView lvwPatrolArea;
		private System.Windows.Forms.Button btnAddWaypoint;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Panel panel7;
		private System.Windows.Forms.Panel panel6;
		private System.Windows.Forms.Panel panel3;
		private System.Windows.Forms.Panel panel4;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.Panel panel2;
		private System.Windows.Forms.Panel panel1;
		private System.Windows.Forms.Panel panel5;
		private System.Windows.Forms.Button btnWaypointMoveDown;
		private System.Windows.Forms.Button btnAddPatrolArea;
		private System.Windows.Forms.ColumnHeader chX;
		private System.Windows.Forms.ColumnHeader chName;
		private System.Windows.Forms.ColumnHeader chDist;
		private System.Windows.Forms.ColumnHeader chY;
		private System.Windows.Forms.ToolTip toolTip1;

		public frmPatrolArea(PatrolAreas patrolareas, AutoKillerScript.clsAutoKillerScript ak)
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
			this.btnGetTarget1.Click += new System.EventHandler(this.btnGetTarget_Click);
			this.lvwPatrolArea.Validated +=new EventHandler(lvwPatrolArea_Validated);

			//we have our PatrolAreas class, lets load the comboBox with the area names
			foreach (string patrolareaname in _patrolareas.GetAllPatrolAreas())
				if(_patrolareas.GetPatrolArea(patrolareaname).Type == PatrolType.PatrolArea)
					cbxPatrolAreas.Items.Add(patrolareaname);

			// set up our timer for recording waypoints as the player runs
			AutoWaypointsTimer = new System.Windows.Forms.Timer();
			AutoWaypointsTimer.Interval = 1000;
			AutoWaypointsTimer.Tick += new EventHandler(AutoWaypointsTimer_Expired);
			AutoWaypointsTimer.Enabled = true;
			AutoWaypointsTimer.Stop();

		}

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		protected override void Dispose( bool disposing )
		{
			if( disposing )
			{
				//Save all the PatrolAreas
				_patrolareas.SavePatrolAreas();

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
			this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
			this.btnAddPatrolArea = new System.Windows.Forms.Button();
			this.btnWaypointMoveDown = new System.Windows.Forms.Button();
			this.btnAddWaypoint = new System.Windows.Forms.Button();
			this.btnGetTarget1 = new System.Windows.Forms.Button();
			this.btnWaypointDelete = new System.Windows.Forms.Button();
			this.btnWaypointMoveUp = new System.Windows.Forms.Button();
			this.btnTargetDelete = new System.Windows.Forms.Button();
			this.btnDeletePatrolArea = new System.Windows.Forms.Button();
			this.btnWaypointAuto = new System.Windows.Forms.Button();
			this.chX = new System.Windows.Forms.ColumnHeader();
			this.chName = new System.Windows.Forms.ColumnHeader();
			this.chDist = new System.Windows.Forms.ColumnHeader();
			this.panel5 = new System.Windows.Forms.Panel();
			this.lvwPatrolArea = new System.Windows.Forms.ListView();
			this.panel1 = new System.Windows.Forms.Panel();
			this.cbxPatrolAreas = new System.Windows.Forms.ComboBox();
			this.panel2 = new System.Windows.Forms.Panel();
			this.label1 = new System.Windows.Forms.Label();
			this.label2 = new System.Windows.Forms.Label();
			this.label3 = new System.Windows.Forms.Label();
			this.panel4 = new System.Windows.Forms.Panel();
			this.panel3 = new System.Windows.Forms.Panel();
			this.panel6 = new System.Windows.Forms.Panel();
			this.lbxTargets = new System.Windows.Forms.ListBox();
			this.panel7 = new System.Windows.Forms.Panel();
			this.label4 = new System.Windows.Forms.Label();
			this.chY = new System.Windows.Forms.ColumnHeader();
			this.panel5.SuspendLayout();
			this.panel1.SuspendLayout();
			this.panel2.SuspendLayout();
			this.panel4.SuspendLayout();
			this.panel3.SuspendLayout();
			this.panel6.SuspendLayout();
			this.panel7.SuspendLayout();
			this.SuspendLayout();
			// 
			// btnAddPatrolArea
			// 
			this.btnAddPatrolArea.Location = new System.Drawing.Point(144, 30);
			this.btnAddPatrolArea.Name = "btnAddPatrolArea";
			this.btnAddPatrolArea.Size = new System.Drawing.Size(59, 19);
			this.btnAddPatrolArea.TabIndex = 20;
			this.btnAddPatrolArea.Text = "Add Area";
			this.toolTip1.SetToolTip(this.btnAddPatrolArea, "Add a new Patrol Area using the name entered");
			this.btnAddPatrolArea.Click += new System.EventHandler(this.btnAddPatrolArea_Click);
			// 
			// btnWaypointMoveDown
			// 
			this.btnWaypointMoveDown.Location = new System.Drawing.Point(248, 8);
			this.btnWaypointMoveDown.Name = "btnWaypointMoveDown";
			this.btnWaypointMoveDown.Size = new System.Drawing.Size(76, 22);
			this.btnWaypointMoveDown.TabIndex = 2;
			this.btnWaypointMoveDown.Text = "Move Down";
			this.toolTip1.SetToolTip(this.btnWaypointMoveDown, "Move the selected waypoint(s) down in the list");
			this.btnWaypointMoveDown.Click += new System.EventHandler(this.btnWaypointMoveDown_Click);
			// 
			// btnAddWaypoint
			// 
			this.btnAddWaypoint.Location = new System.Drawing.Point(72, 8);
			this.btnAddWaypoint.Name = "btnAddWaypoint";
			this.btnAddWaypoint.Size = new System.Drawing.Size(56, 22);
			this.btnAddWaypoint.TabIndex = 2;
			this.btnAddWaypoint.Text = "Add";
			this.toolTip1.SetToolTip(this.btnAddWaypoint, "Add the current players position in the game as a waypoint");
			this.btnAddWaypoint.Click += new System.EventHandler(this.btnAddWaypoint_Click);
			// 
			// btnGetTarget1
			// 
			this.btnGetTarget1.Location = new System.Drawing.Point(8, 17);
			this.btnGetTarget1.Name = "btnGetTarget1";
			this.btnGetTarget1.Size = new System.Drawing.Size(76, 21);
			this.btnGetTarget1.TabIndex = 13;
			this.btnGetTarget1.Text = "Add";
			this.toolTip1.SetToolTip(this.btnGetTarget1, "Add the current game target");
			// 
			// btnWaypointDelete
			// 
			this.btnWaypointDelete.Location = new System.Drawing.Point(128, 8);
			this.btnWaypointDelete.Name = "btnWaypointDelete";
			this.btnWaypointDelete.Size = new System.Drawing.Size(56, 22);
			this.btnWaypointDelete.TabIndex = 2;
			this.btnWaypointDelete.Text = "Delete";
			this.toolTip1.SetToolTip(this.btnWaypointDelete, "Delete the selected waypoint(s)");
			this.btnWaypointDelete.Click += new System.EventHandler(this.btnWaypointDelete_Click);
			// 
			// btnWaypointMoveUp
			// 
			this.btnWaypointMoveUp.Location = new System.Drawing.Point(176, 8);
			this.btnWaypointMoveUp.Name = "btnWaypointMoveUp";
			this.btnWaypointMoveUp.Size = new System.Drawing.Size(76, 22);
			this.btnWaypointMoveUp.TabIndex = 2;
			this.btnWaypointMoveUp.Text = "Move Up";
			this.toolTip1.SetToolTip(this.btnWaypointMoveUp, "Move the selected waypoint(s) up in the list");
			this.btnWaypointMoveUp.Click += new System.EventHandler(this.btnWaypointMoveUp_Click);
			// 
			// btnTargetDelete
			// 
			this.btnTargetDelete.Location = new System.Drawing.Point(8, 39);
			this.btnTargetDelete.Name = "btnTargetDelete";
			this.btnTargetDelete.Size = new System.Drawing.Size(76, 21);
			this.btnTargetDelete.TabIndex = 2;
			this.btnTargetDelete.Text = "Delete";
			this.toolTip1.SetToolTip(this.btnTargetDelete, "Delete the selected waypoint(s)");
			this.btnTargetDelete.Click += new System.EventHandler(this.btnTargetDelete_Click);
			// 
			// btnDeletePatrolArea
			// 
			this.btnDeletePatrolArea.Location = new System.Drawing.Point(208, 30);
			this.btnDeletePatrolArea.Name = "btnDeletePatrolArea";
			this.btnDeletePatrolArea.Size = new System.Drawing.Size(119, 19);
			this.btnDeletePatrolArea.TabIndex = 19;
			this.btnDeletePatrolArea.Text = "Delete Selected Area";
			this.toolTip1.SetToolTip(this.btnDeletePatrolArea, "Delete the selected Patrol Area");
			this.btnDeletePatrolArea.Click += new System.EventHandler(this.btnDeletePatrolArea_Click);
			// 
			// btnWaypointAuto
			// 
			this.btnWaypointAuto.Location = new System.Drawing.Point(8, 8);
			this.btnWaypointAuto.Name = "btnWaypointAuto";
			this.btnWaypointAuto.Size = new System.Drawing.Size(64, 22);
			this.btnWaypointAuto.TabIndex = 2;
			this.btnWaypointAuto.Text = "Auto Add";
			this.toolTip1.SetToolTip(this.btnWaypointAuto, "Turn this on to add waypoints as the player moves in the game");
			this.btnWaypointAuto.Click += new System.EventHandler(this.btnWaypointAuto_Click);
			// 
			// chX
			// 
			this.chX.Text = "X";
			// 
			// chName
			// 
			this.chName.Text = "Name (optional)";
			this.chName.Width = 100;
			// 
			// chDist
			// 
			this.chDist.Text = "Dist";
			// 
			// panel5
			// 
			this.panel5.Controls.Add(this.btnAddWaypoint);
			this.panel5.Controls.Add(this.lvwPatrolArea);
			this.panel5.Controls.Add(this.btnWaypointDelete);
			this.panel5.Controls.Add(this.btnWaypointAuto);
			this.panel5.Controls.Add(this.btnWaypointMoveUp);
			this.panel5.Controls.Add(this.btnWaypointMoveDown);
			this.panel5.Dock = System.Windows.Forms.DockStyle.Top;
			this.panel5.Location = new System.Drawing.Point(4, 203);
			this.panel5.Name = "panel5";
			this.panel5.Size = new System.Drawing.Size(335, 189);
			this.panel5.TabIndex = 17;
			// 
			// lvwPatrolArea
			// 
			this.lvwPatrolArea.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
																							this.chName,
																							this.chX,
																							this.chY,
																							this.chDist});
			this.lvwPatrolArea.FullRowSelect = true;
			this.lvwPatrolArea.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable;
			this.lvwPatrolArea.HideSelection = false;
			this.lvwPatrolArea.LabelEdit = true;
			this.lvwPatrolArea.LabelWrap = false;
			this.lvwPatrolArea.Location = new System.Drawing.Point(8, 32);
			this.lvwPatrolArea.Name = "lvwPatrolArea";
			this.lvwPatrolArea.Size = new System.Drawing.Size(320, 144);
			this.lvwPatrolArea.TabIndex = 1;
			this.lvwPatrolArea.View = System.Windows.Forms.View.Details;
			// 
			// panel1
			// 
			this.panel1.Controls.Add(this.btnAddPatrolArea);
			this.panel1.Controls.Add(this.cbxPatrolAreas);
			this.panel1.Controls.Add(this.btnDeletePatrolArea);
			this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
			this.panel1.Location = new System.Drawing.Point(4, 40);
			this.panel1.Name = "panel1";
			this.panel1.Size = new System.Drawing.Size(335, 56);
			this.panel1.TabIndex = 15;
			// 
			// cbxPatrolAreas
			// 
			this.cbxPatrolAreas.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cbxPatrolAreas.Location = new System.Drawing.Point(8, 7);
			this.cbxPatrolAreas.Name = "cbxPatrolAreas";
			this.cbxPatrolAreas.Size = new System.Drawing.Size(320, 21);
			this.cbxPatrolAreas.Sorted = true;
			this.cbxPatrolAreas.TabIndex = 17;
			this.cbxPatrolAreas.SelectedIndexChanged += new System.EventHandler(this.cbxPatrolAreas_SelectedIndexChanged);
			// 
			// panel2
			// 
			this.panel2.BackColor = System.Drawing.SystemColors.ControlDark;
			this.panel2.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
			this.panel2.Controls.Add(this.label1);
			this.panel2.Dock = System.Windows.Forms.DockStyle.Top;
			this.panel2.Location = new System.Drawing.Point(4, 25);
			this.panel2.Name = "panel2";
			this.panel2.Size = new System.Drawing.Size(335, 15);
			this.panel2.TabIndex = 14;
			// 
			// label1
			// 
			this.label1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.label1.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
			this.label1.Location = new System.Drawing.Point(0, 0);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(331, 11);
			this.label1.TabIndex = 0;
			this.label1.Text = "Create or Select Patrol Area";
			this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// label2
			// 
			this.label2.BackColor = System.Drawing.SystemColors.ActiveCaption;
			this.label2.Dock = System.Windows.Forms.DockStyle.Fill;
			this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.label2.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
			this.label2.Location = new System.Drawing.Point(0, 0);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(331, 18);
			this.label2.TabIndex = 0;
			this.label2.Text = "Patrol Areas";
			this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// label3
			// 
			this.label3.Dock = System.Windows.Forms.DockStyle.Fill;
			this.label3.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
			this.label3.Location = new System.Drawing.Point(0, 0);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(331, 10);
			this.label3.TabIndex = 0;
			this.label3.Text = "Targets to fight in this Area ( All blank to fight any target )";
			this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// panel4
			// 
			this.panel4.BackColor = System.Drawing.SystemColors.ControlDark;
			this.panel4.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
			this.panel4.Controls.Add(this.label3);
			this.panel4.Dock = System.Windows.Forms.DockStyle.Top;
			this.panel4.Location = new System.Drawing.Point(4, 96);
			this.panel4.Name = "panel4";
			this.panel4.Size = new System.Drawing.Size(335, 14);
			this.panel4.TabIndex = 18;
			// 
			// panel3
			// 
			this.panel3.BackColor = System.Drawing.SystemColors.ActiveCaption;
			this.panel3.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
			this.panel3.Controls.Add(this.label2);
			this.panel3.Dock = System.Windows.Forms.DockStyle.Top;
			this.panel3.Location = new System.Drawing.Point(4, 3);
			this.panel3.Name = "panel3";
			this.panel3.Size = new System.Drawing.Size(335, 22);
			this.panel3.TabIndex = 14;
			// 
			// panel6
			// 
			this.panel6.Controls.Add(this.lbxTargets);
			this.panel6.Controls.Add(this.btnGetTarget1);
			this.panel6.Controls.Add(this.btnTargetDelete);
			this.panel6.Dock = System.Windows.Forms.DockStyle.Top;
			this.panel6.Location = new System.Drawing.Point(4, 110);
			this.panel6.Name = "panel6";
			this.panel6.Size = new System.Drawing.Size(335, 78);
			this.panel6.TabIndex = 19;
			// 
			// lbxTargets
			// 
			this.lbxTargets.Location = new System.Drawing.Point(96, 7);
			this.lbxTargets.Name = "lbxTargets";
			this.lbxTargets.Size = new System.Drawing.Size(232, 56);
			this.lbxTargets.TabIndex = 14;
			// 
			// panel7
			// 
			this.panel7.BackColor = System.Drawing.SystemColors.ControlDark;
			this.panel7.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
			this.panel7.Controls.Add(this.label4);
			this.panel7.Dock = System.Windows.Forms.DockStyle.Top;
			this.panel7.Location = new System.Drawing.Point(4, 188);
			this.panel7.Name = "panel7";
			this.panel7.Size = new System.Drawing.Size(335, 15);
			this.panel7.TabIndex = 20;
			// 
			// label4
			// 
			this.label4.Dock = System.Windows.Forms.DockStyle.Fill;
			this.label4.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
			this.label4.Location = new System.Drawing.Point(0, 0);
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size(331, 11);
			this.label4.TabIndex = 0;
			this.label4.Text = "Enter or Change Waypoints";
			this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// chY
			// 
			this.chY.Text = "Y";
			// 
			// frmPatrolArea
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.ClientSize = new System.Drawing.Size(343, 400);
			this.Controls.Add(this.panel5);
			this.Controls.Add(this.panel7);
			this.Controls.Add(this.panel6);
			this.Controls.Add(this.panel4);
			this.Controls.Add(this.panel1);
			this.Controls.Add(this.panel2);
			this.Controls.Add(this.panel3);
			this.DockPadding.All = 3;
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
			this.Name = "frmPatrolArea";
			this.Text = "frmPatrolArea";
			this.panel5.ResumeLayout(false);
			this.panel1.ResumeLayout(false);
			this.panel2.ResumeLayout(false);
			this.panel4.ResumeLayout(false);
			this.panel3.ResumeLayout(false);
			this.panel6.ResumeLayout(false);
			this.panel7.ResumeLayout(false);
			this.ResumeLayout(false);

		}
		#endregion

		private PatrolAreas _patrolareas;
		private AutoKillerScript.clsAutoKillerScript _ak;
		private Timer AutoWaypointsTimer;
		private float _mLastDir = 0;
		private float _lastAddedX = 0;
		private float _lastAddedY = 0;
		private float _lastAddedZ = 0;

		#region Patrol Area add and delete

		private void btnAddPatrolArea_Click(object sender, System.EventArgs e)
		{
			frmNewPatrolArea frm = new frmNewPatrolArea();
			if( frm.ShowDialog() == DialogResult.Cancel)
				return;

			//save any changes to the waypoints for the previous area, if any
			SaveWaypoints();

			//clear the displays
			lbxTargets.Items.Clear();
			lvwPatrolArea.Items.Clear();

			try
			{
				_patrolareas.AddPatrolArea(frm.txtPatrolAreaName.Text, PatrolType.PatrolArea );			
			}
			catch(Exception E)
			{
				MessageBox.Show(E.Message , "Name Add Error",
					MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
				return;
			}

			//add the new name to the patrol area dropdown, and select it
			cbxPatrolAreas.Items.Add(frm.txtPatrolAreaName.Text);
			cbxPatrolAreas.SelectedIndex = cbxPatrolAreas.Items.IndexOf(frm.txtPatrolAreaName.Text);
		}

		private void btnDeletePatrolArea_Click(object sender, System.EventArgs e)
		{
			if(cbxPatrolAreas.SelectedIndex == -1)
			{
				MessageBox.Show("Please select the Patrol Area to delete first.", "Area Delete Error",
					MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
				return;
			}

			//remove the patrol area and associated targets and waypoints
			_patrolareas.RemovePatrolArea(cbxPatrolAreas.SelectedItem.ToString());

			//remove the patrol area from the dropdown display
			cbxPatrolAreas.Items.Remove(cbxPatrolAreas.SelectedItem.ToString());
			cbxPatrolAreas.Text = "";
			cbxPatrolAreas.SelectedIndex = -1;
		}

		private void cbxPatrolAreas_SelectedIndexChanged(object sender, EventArgs e)
		{
			if( UpdateVariable != null)
				UpdateVariable( "PatrolArea", cbxPatrolAreas.Text);

			if(SaveCurrentProfile != null)
				SaveCurrentProfile();

			if (cbxPatrolAreas.SelectedIndex == -1)
				return;

			LoadSettings(cbxPatrolAreas.SelectedItem.ToString());
		}
		#endregion

		#region Target add and delete
		private void btnGetTarget_Click(object sender, System.EventArgs e)
		{
			if(cbxPatrolAreas.SelectedIndex == -1)
			{
				MessageBox.Show("Please select the Patrol Area first.", "Add Target Error",
					MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
				return;
			}

			if(_ak.TargetObject == null)
			{
				MessageBox.Show("Target something in the game first.", "Add Target Error",
					MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
				return;
			}

			string targetName = _ak.TargetObject.Name;

			PatrolArea patrolarea = _patrolareas.GetPatrolArea(cbxPatrolAreas.SelectedItem.ToString());

			lbxTargets.Items.Add(targetName);
			patrolarea.AddTarget(targetName);

		}
		private void btnTargetDelete_Click(object sender, System.EventArgs e)
		{
			if(cbxPatrolAreas.SelectedIndex == -1)
			{
				MessageBox.Show("Please select the Patrol Area first.", "Add Target Error",
					MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
				return;
			}

			PatrolArea patrolarea = _patrolareas.GetPatrolArea(cbxPatrolAreas.SelectedItem.ToString());

			//remove the highlighted items
			while(lbxTargets.SelectedIndices.Count > 0)
				lbxTargets.Items.RemoveAt(lbxTargets.SelectedIndices[0]);
			
			//rebuild our patrolarea targets array
			patrolarea.ClearTargets();
			foreach( string target in lbxTargets.Items)
				patrolarea.AddTarget(target);

		}

		#endregion

		#region waypoint add, delete, move, and auto add
		
		private void btnAddWaypoint_Click(object sender, System.EventArgs e)
		{
			if(cbxPatrolAreas.SelectedIndex == -1)
			{
				MessageBox.Show("Please select the Patrol Area first.", "Add Waypoint Error",
					MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
				return;
			}
			
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

			if(_ak.GameProcess == 0)
				return;

			//generate default waypoint name
			string itemName = "no name";
			if(sender == null) //auto add 
				itemName = "auto add";

			// add the current players location to the patrol area waypoint list
			patrolarea.AddWaypoint(_ak.gPlayerXCoord, _ak.gPlayerYCoord, _ak.gPlayerZCoord, itemName);

			//create a new waypoint object for storing in the ListViewItem
			Waypoint wpt = new Waypoint(_ak, patrolarea.GetNewestWaypoint());

			//create a ListViewItem, set its display strings, and attach the waypoint object
			ListViewItem lvi = new ListViewItem(itemName,0);
			lvi.SubItems.Add(_ak.gPlayerXCoord.ToString("N"));
			lvi.SubItems.Add(_ak.gPlayerYCoord.ToString("N"));
			if(_lastAddedX != 0)
				lvi.SubItems.Add(_ak.ZDistance(_ak.gPlayerXCoord,_ak.gPlayerYCoord,_ak.gPlayerZCoord,_lastAddedX, _lastAddedY, _lastAddedZ).ToString());
			lvi.Tag = wpt;

			//add the ListViewItem to our listview
			lvwPatrolArea.Items.Add(lvi);

			//store this location for Dist calc next time
			

			//if not auto add, put new item in edit mode so user sees they can name it
			if(sender != null)
				lvi.BeginEdit();
			
		}

		private void btnWaypointAuto_Click(object sender, System.EventArgs e)
		{
			if(AutoWaypointsTimer.Enabled)
			{
				AutoWaypointsTimer.Stop();
				AutoWaypointsTimer.Enabled = false;
				btnWaypointAuto.TextAlign = ContentAlignment.MiddleCenter;
				btnWaypointAuto.BackColor = SystemColors.Control;
				btnWaypointAuto.ForeColor = SystemColors.ControlText;
				btnWaypointAuto.Text = "Auto Add";
				SaveWaypoints();
			}
			else
			{
				AutoWaypointsTimer.Enabled = true;
				AutoWaypointsTimer.Start();
				btnWaypointAuto.Text = "Stop Auto";
			}
		
		}
		public void AutoWaypointsTimer_Expired(object sender,EventArgs eArgs)
		{
			if(_ak.GameProcess != 0 && _mLastDir != _ak.PlayerDir && (_lastAddedX != _ak.gPlayerXCoord || _lastAddedY != _ak.gPlayerYCoord))
			{
				_mLastDir = _ak.PlayerDir;
				btnAddWaypoint_Click(null, null);
				_lastAddedX = _ak.gPlayerXCoord;
				_lastAddedY = _ak.gPlayerYCoord;
				_lastAddedZ = _ak.gPlayerZCoord;
			}
			
			//okay this is just for fun
			switch((int)btnWaypointAuto.TextAlign)
			{
				case (int)ContentAlignment.MiddleCenter:
				case (int)ContentAlignment.MiddleLeft:
				{
					btnWaypointAuto.TextAlign = ContentAlignment.TopCenter;
					btnWaypointAuto.BackColor = Color.Blue;
					btnWaypointAuto.ForeColor = Color.Gold;
					break;
				}
				case (int)ContentAlignment.TopCenter:
				{
					btnWaypointAuto.TextAlign = ContentAlignment.MiddleRight;
					btnWaypointAuto.BackColor = Color.Gold;
					btnWaypointAuto.ForeColor = Color.Blue;
					break;
				}
				case (int)ContentAlignment.MiddleRight:
				{
					btnWaypointAuto.TextAlign = ContentAlignment.BottomCenter;
					btnWaypointAuto.BackColor = Color.Blue;
					btnWaypointAuto.ForeColor = Color.Gold;
					break;
				}
				case (int)ContentAlignment.BottomCenter:
				{
					btnWaypointAuto.TextAlign = ContentAlignment.MiddleLeft;
					btnWaypointAuto.BackColor = Color.Gold;
					btnWaypointAuto.ForeColor = Color.Blue;
					break;
				}
			}
		}
		private void lvwPatrolArea_Validated(object sender, EventArgs e)
		{
			//save any changes to the patrol area, need to do this here to capture inline edits to the name
			SaveWaypoints();
		}

		private void btnWaypointMoveDown_Click(object sender, System.EventArgs e)
		{
			//work from the end of the list towards the top
			for(int index = lvwPatrolArea.SelectedIndices.Count - 1; index >= 0 ; index--)
			{
				//cannot move the last item down
				if( lvwPatrolArea.SelectedIndices[index] < lvwPatrolArea.Items.Count - 1)
				{
					//check if the item below this one is selected, if so we dont do anything since we
					//dont want to swap the positions of two selected items
					if(index < lvwPatrolArea.SelectedIndices.Count - 1 && lvwPatrolArea.SelectedIndices[index+1] == lvwPatrolArea.SelectedIndices[index] + 1)
						continue;
					//store the index we are moving since as soon as we move it SelectedIndices will change
					int movingItem = lvwPatrolArea.SelectedIndices[index];
					//get the ListViewItem for the index being moved
					ListViewItem lvi = (ListViewItem)lvwPatrolArea.Items[movingItem].Clone();
					//remove it
					lvwPatrolArea.Items.RemoveAt(movingItem);
					//insert it again above the last position
					lvwPatrolArea.Items.Insert(movingItem + 1,lvi.Text );
					//re-add the subitems for the second colum x,y display
					lvwPatrolArea.Items[movingItem + 1].SubItems.Add(lvi.SubItems[1].Text);
					lvwPatrolArea.Items[movingItem + 1].SubItems.Add(lvi.SubItems[2].Text);
					lvwPatrolArea.Items[movingItem + 1].SubItems.Add(lvi.SubItems[3].Text);
					//re-add the tag
					lvwPatrolArea.Items[movingItem + 1].Tag = lvi.Tag;
					//re-select the item
					lvwPatrolArea.Items[movingItem + 1].Selected = true;
				}
			}
			SaveWaypoints();
		}
		private void btnWaypointMoveUp_Click(object sender, System.EventArgs e)
		{
			//work from the end of the list towards the top
			for(int index = 0; index < lvwPatrolArea.SelectedIndices.Count; index++)
			{
				//cannot move the top item up
				if( lvwPatrolArea.SelectedIndices[index] > 0)
				{
					//check if the item above this one is selected, if so we dont do anything since we
					//dont want to swap the positions of two selected items
					if(index > 0 && lvwPatrolArea.SelectedIndices[index-1] == lvwPatrolArea.SelectedIndices[index] - 1)
						continue;
					//store the index we are moving since as soon as we move it SelectedIndices will change
					int movingItem = lvwPatrolArea.SelectedIndices[index];
					//get the ListViewItem for the index being moved
					ListViewItem lvi = (ListViewItem)lvwPatrolArea.Items[movingItem].Clone();
					//remove it
					lvwPatrolArea.Items.RemoveAt(movingItem);
					//insert it again above the last position
					lvwPatrolArea.Items.Insert(movingItem - 1,lvi.Text );
					//re-add the subitems for the second colum x,y display
					lvwPatrolArea.Items[movingItem - 1].SubItems.Add(lvi.SubItems[1].Text);
					lvwPatrolArea.Items[movingItem - 1].SubItems.Add(lvi.SubItems[2].Text);
					lvwPatrolArea.Items[movingItem - 1].SubItems.Add(lvi.SubItems[3].Text);
					//re-add the tag
					lvwPatrolArea.Items[movingItem - 1].Tag = lvi.Tag;
					//re-select the item
					lvwPatrolArea.Items[movingItem - 1].Selected = true;
				}
			}
			SaveWaypoints();
		}
		private void btnWaypointDelete_Click(object sender, System.EventArgs e)
		{
			while(lvwPatrolArea.SelectedIndices.Count > 0)
				lvwPatrolArea.Items.RemoveAt(lvwPatrolArea.SelectedIndices[0]);

			SaveWaypoints();
		}
		#endregion

		#region utilities
		private void SaveWaypoints()
		{
			if( SaveCurrentProfile != null)
				SaveCurrentProfile();

			if(cbxPatrolAreas.SelectedIndex == -1)
				return;

			PatrolArea patrolarea;
			try
			{
				patrolarea = _patrolareas.GetPatrolArea(cbxPatrolAreas.SelectedItem.ToString());
			}
			catch( Exception E)
			{
				MessageBox.Show(E.Message , "Save Waypoints Error",
					MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
				return;
			}

			patrolarea.ClearAllWaypoints();

			//get each ListViewItem from the ListView and save its attached waypoint object
			foreach (ListViewItem lvi in lvwPatrolArea.Items)
			{
				Waypoint wpt = (Waypoint)lvi.Tag;
				patrolarea.AddWaypoint(wpt.X, wpt.Y, wpt.Z, lvi.Text);
			}

		}

		private void LoadSettings(string areaName)
		{
			PatrolArea patrolarea;
			try
			{
				patrolarea = _patrolareas.GetPatrolArea(areaName);
			}
			catch( Exception E)
			{
				MessageBox.Show(E.Message , "Load Area Error",
					MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
				return;
			}

			//clear all first in case of null values
			lbxTargets.Items.Clear();

			lock(patrolarea.Targets.SyncRoot)
			{
				foreach( string target in patrolarea.Targets)
					lbxTargets.Items.Add(target);
			}
			
			_lastAddedX = 0;
			_lastAddedY = 0;
			_lastAddedZ = 0;
			lvwPatrolArea.Items.Clear();
			ArrayList waypoints = patrolarea.GetAllWaypoints;

			// Remember to lock on the SyncRoot before enumeration.
			lock(waypoints.SyncRoot)
			{
				foreach (Waypoint wpt in waypoints)
				{
					ListViewItem lvi = new ListViewItem(wpt.Name,0);
					lvi.SubItems.Add(wpt.X.ToString("N"));
					lvi.SubItems.Add(wpt.Y.ToString("N"));
					if(_lastAddedX != 0)
						lvi.SubItems.Add(wpt.GetDistance(_lastAddedX, _lastAddedY, _lastAddedZ).ToString());
					lvi.Tag = wpt;

					_lastAddedX = wpt.X;
					_lastAddedY = wpt.Y;
					_lastAddedZ = wpt.Z;

					lvwPatrolArea.Items.Add(lvi);
				}
			}
			_lastAddedX = 0;
			_lastAddedY = 0;
			_lastAddedZ = 0;

		}
		#endregion

		#region IProfileForm handling routines
		public event UpdateVariableDelegate UpdateVariable;

		public event CurrentProfileDelegate LoadCurrentProfile; // This event is never used
		public event CurrentProfileDelegate SaveCurrentProfile;

		public void DefineVariables( Profile profile)
		{
			profile.DefineVariable( "PatrolArea", "");
		}

		public void OnProfileChange( Profile profile)
		{
			bool found = false;
			for( int i = 0; i < cbxPatrolAreas.Items.Count; i ++)
				if( (string)cbxPatrolAreas.Items[i] == profile.GetString("PatrolArea"))
				{
					cbxPatrolAreas.SelectedIndex = i;
					found = true;
					break;
				}

			if( !found)
			{
				profile.ResetValue( "PatrolArea");
				cbxPatrolAreas.SelectedIndex = -1;
			}
		}		
		#endregion
	}
}
