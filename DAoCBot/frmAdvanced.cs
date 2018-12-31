using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using AutoKillerScript;

namespace DAoC_Bot
{
	/// <summary>
	/// Summary description for frmAdvanced.
	/// </summary>
	public class frmAdvanced : System.Windows.Forms.Form
	{
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;
		private System.Windows.Forms.Panel panel3;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.TabControl tcAdvanced;
		private System.Windows.Forms.TabPage tpWoWSharp;
		private System.Windows.Forms.Panel panel4;
		private System.Windows.Forms.TabPage tpRawLog;
		private System.Windows.Forms.ListBox lbRawChatLog;
		private AutoKillerScript.clsAutoKillerScript _ak;
		private System.Windows.Forms.TabPage tpWhispers;
		private System.Windows.Forms.ListBox lbWhispers;
		private System.Windows.Forms.ListBox lbAK;

		public frmAdvanced( AutoKillerScript.clsAutoKillerScript ak)
		{
			_ak = ak;

			_ak.OnLog += new clsAutoKillerScript.OnLogEventHandler(OnLogLine);
			_ak.OnRegExTrue += new clsAutoKillerScript.OnRegExTrueEventHandler(RawLogLine);

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
			this.panel3 = new System.Windows.Forms.Panel();
			this.label2 = new System.Windows.Forms.Label();
			this.tcAdvanced = new System.Windows.Forms.TabControl();
			this.tpWoWSharp = new System.Windows.Forms.TabPage();
			this.lbAK = new System.Windows.Forms.ListBox();
			this.tpRawLog = new System.Windows.Forms.TabPage();
			this.lbRawChatLog = new System.Windows.Forms.ListBox();
			this.tpWhispers = new System.Windows.Forms.TabPage();
			this.lbWhispers = new System.Windows.Forms.ListBox();
			this.panel4 = new System.Windows.Forms.Panel();
			this.panel3.SuspendLayout();
			this.tcAdvanced.SuspendLayout();
			this.tpWoWSharp.SuspendLayout();
			this.tpRawLog.SuspendLayout();
			this.tpWhispers.SuspendLayout();
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
			this.panel3.Size = new System.Drawing.Size(312, 24);
			this.panel3.TabIndex = 14;
			// 
			// label2
			// 
			this.label2.Dock = System.Windows.Forms.DockStyle.Fill;
			this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.label2.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
			this.label2.Location = new System.Drawing.Point(0, 0);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(308, 20);
			this.label2.TabIndex = 0;
			this.label2.Text = "Advanced Information";
			this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// tcAdvanced
			// 
			this.tcAdvanced.Appearance = System.Windows.Forms.TabAppearance.FlatButtons;
			this.tcAdvanced.Controls.Add(this.tpWoWSharp);
			this.tcAdvanced.Controls.Add(this.tpRawLog);
			this.tcAdvanced.Controls.Add(this.tpWhispers);
			this.tcAdvanced.Dock = System.Windows.Forms.DockStyle.Fill;
			this.tcAdvanced.Location = new System.Drawing.Point(4, 32);
			this.tcAdvanced.Name = "tcAdvanced";
			this.tcAdvanced.SelectedIndex = 0;
			this.tcAdvanced.Size = new System.Drawing.Size(312, 230);
			this.tcAdvanced.TabIndex = 15;
			// 
			// tpWoWSharp
			// 
			this.tpWoWSharp.Controls.Add(this.lbAK);
			this.tpWoWSharp.Location = new System.Drawing.Point(4, 25);
			this.tpWoWSharp.Name = "tpWoWSharp";
			this.tpWoWSharp.Size = new System.Drawing.Size(304, 201);
			this.tpWoWSharp.TabIndex = 0;
			this.tpWoWSharp.Text = "AKScript\'s Log";
			// 
			// lbAK
			// 
			this.lbAK.Dock = System.Windows.Forms.DockStyle.Fill;
			this.lbAK.Location = new System.Drawing.Point(0, 0);
			this.lbAK.Name = "lbAK";
			this.lbAK.Size = new System.Drawing.Size(304, 199);
			this.lbAK.TabIndex = 0;
			// 
			// tpRawLog
			// 
			this.tpRawLog.Controls.Add(this.lbRawChatLog);
			this.tpRawLog.Location = new System.Drawing.Point(4, 25);
			this.tpRawLog.Name = "tpRawLog";
			this.tpRawLog.Size = new System.Drawing.Size(304, 201);
			this.tpRawLog.TabIndex = 1;
			this.tpRawLog.Text = "Raw Chat Log";
			// 
			// lbRawChatLog
			// 
			this.lbRawChatLog.Dock = System.Windows.Forms.DockStyle.Fill;
			this.lbRawChatLog.Location = new System.Drawing.Point(0, 0);
			this.lbRawChatLog.Name = "lbRawChatLog";
			this.lbRawChatLog.Size = new System.Drawing.Size(304, 201);
			this.lbRawChatLog.TabIndex = 1;
			// 
			// tpWhispers
			// 
			this.tpWhispers.Controls.Add(this.lbWhispers);
			this.tpWhispers.Location = new System.Drawing.Point(4, 25);
			this.tpWhispers.Name = "tpWhispers";
			this.tpWhispers.Size = new System.Drawing.Size(304, 201);
			this.tpWhispers.TabIndex = 2;
			this.tpWhispers.Text = "Player talk";
			// 
			// lbWhispers
			// 
			this.lbWhispers.Dock = System.Windows.Forms.DockStyle.Fill;
			this.lbWhispers.Location = new System.Drawing.Point(0, 0);
			this.lbWhispers.Name = "lbWhispers";
			this.lbWhispers.Size = new System.Drawing.Size(304, 201);
			this.lbWhispers.TabIndex = 2;
			// 
			// panel4
			// 
			this.panel4.BackColor = System.Drawing.SystemColors.Control;
			this.panel4.Dock = System.Windows.Forms.DockStyle.Top;
			this.panel4.Location = new System.Drawing.Point(4, 28);
			this.panel4.Name = "panel4";
			this.panel4.Size = new System.Drawing.Size(312, 4);
			this.panel4.TabIndex = 30;
			// 
			// frmAdvanced
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.ClientSize = new System.Drawing.Size(320, 266);
			this.Controls.Add(this.tcAdvanced);
			this.Controls.Add(this.panel4);
			this.Controls.Add(this.panel3);
			this.DockPadding.All = 4;
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
			this.Name = "frmAdvanced";
			this.Text = "frmAdvanced";
			this.panel3.ResumeLayout(false);
			this.tcAdvanced.ResumeLayout(false);
			this.tpWoWSharp.ResumeLayout(false);
			this.tpRawLog.ResumeLayout(false);
			this.tpWhispers.ResumeLayout(false);
			this.ResumeLayout(false);

		}
		#endregion

		private void OnLogLine(string line)
		{
			lbAK.Items.Insert( 0, line);
			if( lbAK.Items.Count > 256)
				lbAK.Items.RemoveAt( 256);
		}

		private void RawLogLine(AutoKillerScript.clsAutoKillerScript.AutokillerRegExEventParams e)
		{
			string rawlogline = e.Logline;
			lbRawChatLog.Items.Insert( 0, rawlogline);
			if( lbRawChatLog.Items.Count > 256)
				lbRawChatLog.Items.RemoveAt( 256);
			
			// Is this a communication from a player or to a player?
			if( rawlogline.IndexOf("@@") > -1)
			{
				lbWhispers.Items.Insert( 0, rawlogline);
				if( lbWhispers.Items.Count > 256)
					lbWhispers.Items.RemoveAt( 256);
			}		
		}
	}
}
