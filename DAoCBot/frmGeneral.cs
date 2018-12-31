//------------------------------------------------------------------------------
//-------------------------------------------------------------------------------
using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.IO;

namespace DAoC_Bot
{
	/// <summary>
	/// Summary description for frmGeneral.
	/// </summary>
	public class frmGeneral : System.Windows.Forms.Form, IProfileForm
	{
		private System.Windows.Forms.TextBox txtUsername;
		private System.Windows.Forms.Button btnCancel;
		private System.Windows.Forms.Label label5;
		private System.Windows.Forms.Button btnApply;
		private System.Windows.Forms.ComboBox cbxBot;
		private System.Windows.Forms.TextBox txtPassword;
		private System.Windows.Forms.Panel panel7;
		private System.Windows.Forms.Label label6;
		private System.Windows.Forms.Panel panel5;
		private System.Windows.Forms.Label label4;
		private System.Windows.Forms.Panel panel3;
		private System.Windows.Forms.Panel panel2;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Panel panel1;
		private System.Windows.Forms.Label label7;
		private System.Windows.Forms.Panel panel4;
		private System.Windows.Forms.Panel panel6;
		private System.Windows.Forms.ComboBox cbxProfile;
		private System.Windows.Forms.Button btnCreateNewProfile;
		private System.Windows.Forms.Label label8;
		private System.Windows.Forms.Panel panel8;
		private System.Windows.Forms.Panel panel9;
		private System.Windows.Forms.RadioButton radToA;
		private System.Windows.Forms.RadioButton radSI;
		private System.Windows.Forms.RadioButton radCatacombs;
		private System.Windows.Forms.RadioButton radUS;
		private System.Windows.Forms.RadioButton radEuro;
		private System.Windows.Forms.TextBox txtGamePath;
		private System.Windows.Forms.Label label9;
		private System.Windows.Forms.Label label10;
		private System.Windows.Forms.TextBox txtChatLogPath;
		private System.Windows.Forms.GroupBox groupBox1;
		private System.Windows.Forms.GroupBox groupBox2;
		private System.Windows.Forms.Panel panel10;
		private System.Windows.Forms.Panel panel11;
		private System.Windows.Forms.Label label11;
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		public frmGeneral()
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
		private void InitializeComponent() {
			this.panel8 = new System.Windows.Forms.Panel();
			this.cbxBot = new System.Windows.Forms.ComboBox();
			this.label6 = new System.Windows.Forms.Label();
			this.label8 = new System.Windows.Forms.Label();
			this.btnCreateNewProfile = new System.Windows.Forms.Button();
			this.cbxProfile = new System.Windows.Forms.ComboBox();
			this.panel6 = new System.Windows.Forms.Panel();
			this.txtPassword = new System.Windows.Forms.TextBox();
			this.label5 = new System.Windows.Forms.Label();
			this.panel4 = new System.Windows.Forms.Panel();
			this.btnCancel = new System.Windows.Forms.Button();
			this.btnApply = new System.Windows.Forms.Button();
			this.label7 = new System.Windows.Forms.Label();
			this.panel1 = new System.Windows.Forms.Panel();
			this.label1 = new System.Windows.Forms.Label();
			this.label2 = new System.Windows.Forms.Label();
			this.label3 = new System.Windows.Forms.Label();
			this.panel2 = new System.Windows.Forms.Panel();
			this.panel3 = new System.Windows.Forms.Panel();
			this.panel5 = new System.Windows.Forms.Panel();
			this.panel7 = new System.Windows.Forms.Panel();
			this.panel9 = new System.Windows.Forms.Panel();
			this.label4 = new System.Windows.Forms.Label();
			this.radToA = new System.Windows.Forms.RadioButton();
			this.radSI = new System.Windows.Forms.RadioButton();
			this.radCatacombs = new System.Windows.Forms.RadioButton();
			this.radUS = new System.Windows.Forms.RadioButton();
			this.radEuro = new System.Windows.Forms.RadioButton();
			this.txtGamePath = new System.Windows.Forms.TextBox();
			this.label9 = new System.Windows.Forms.Label();
			this.label10 = new System.Windows.Forms.Label();
			this.txtChatLogPath = new System.Windows.Forms.TextBox();
			this.groupBox1 = new System.Windows.Forms.GroupBox();
			this.groupBox2 = new System.Windows.Forms.GroupBox();
			this.panel10 = new System.Windows.Forms.Panel();
			this.panel11 = new System.Windows.Forms.Panel();
			this.label11 = new System.Windows.Forms.Label();
			this.panel8.SuspendLayout();
			this.panel6.SuspendLayout();
			this.panel4.SuspendLayout();
			this.panel1.SuspendLayout();
			this.panel2.SuspendLayout();
			this.panel3.SuspendLayout();
			this.panel5.SuspendLayout();
			this.panel7.SuspendLayout();
			this.panel9.SuspendLayout();
			this.groupBox1.SuspendLayout();
			this.groupBox2.SuspendLayout();
			this.panel10.SuspendLayout();
			this.panel11.SuspendLayout();
			this.SuspendLayout();
			// 
			// panel8
			// 
			this.panel8.Controls.Add(this.cbxBot);
			this.panel8.Controls.Add(this.label6);
			this.panel8.Dock = System.Windows.Forms.DockStyle.Top;
			this.panel8.Location = new System.Drawing.Point(3, 135);
			this.panel8.Name = "panel8";
			this.panel8.Size = new System.Drawing.Size(338, 33);
			this.panel8.TabIndex = 20;
			// 
			// cbxBot
			// 
			this.cbxBot.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cbxBot.Items.AddRange(new object[] {
														"Necro",
														"Scout"});
			this.cbxBot.Location = new System.Drawing.Point(80, 7);
			this.cbxBot.Name = "cbxBot";
			this.cbxBot.Size = new System.Drawing.Size(136, 21);
			this.cbxBot.TabIndex = 29;
			this.cbxBot.SelectedIndexChanged += new System.EventHandler(this.cbxBot_SelectedIndexChanged);
			// 
			// label6
			// 
			this.label6.Location = new System.Drawing.Point(8, 7);
			this.label6.Name = "label6";
			this.label6.Size = new System.Drawing.Size(72, 19);
			this.label6.TabIndex = 28;
			this.label6.Text = "Class:";
			this.label6.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// label8
			// 
			this.label8.Dock = System.Windows.Forms.DockStyle.Fill;
			this.label8.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
			this.label8.Location = new System.Drawing.Point(0, 0);
			this.label8.Name = "label8";
			this.label8.Size = new System.Drawing.Size(334, 11);
			this.label8.TabIndex = 0;
			this.label8.Text = "Character Class Selection";
			this.label8.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// btnCreateNewProfile
			// 
			this.btnCreateNewProfile.Location = new System.Drawing.Point(224, 7);
			this.btnCreateNewProfile.Name = "btnCreateNewProfile";
			this.btnCreateNewProfile.Size = new System.Drawing.Size(56, 20);
			this.btnCreateNewProfile.TabIndex = 2;
			this.btnCreateNewProfile.Text = "Create";
			this.btnCreateNewProfile.Click += new System.EventHandler(this.btnCreateNewProfile_Click);
			// 
			// cbxProfile
			// 
			this.cbxProfile.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cbxProfile.Location = new System.Drawing.Point(80, 7);
			this.cbxProfile.Name = "cbxProfile";
			this.cbxProfile.Size = new System.Drawing.Size(136, 21);
			this.cbxProfile.TabIndex = 0;
			this.cbxProfile.SelectedIndexChanged += new System.EventHandler(this.cbxProfile_SelectedIndexChanged);
			// 
			// panel6
			// 
			this.panel6.Controls.Add(this.txtPassword);
			this.panel6.Controls.Add(this.label5);
			this.panel6.Dock = System.Windows.Forms.DockStyle.Top;
			this.panel6.Location = new System.Drawing.Point(3, 88);
			this.panel6.Name = "panel6";
			this.panel6.Size = new System.Drawing.Size(338, 32);
			this.panel6.TabIndex = 18;
			// 
			// txtPassword
			// 
			this.txtPassword.Location = new System.Drawing.Point(80, 6);
			this.txtPassword.Name = "txtPassword";
			this.txtPassword.PasswordChar = '*';
			this.txtPassword.Size = new System.Drawing.Size(136, 20);
			this.txtPassword.TabIndex = 25;
			this.txtPassword.Text = "";
			this.txtPassword.TextChanged += new System.EventHandler(this.txtPassword_TextChanged);
			// 
			// label5
			// 
			this.label5.Location = new System.Drawing.Point(8, 6);
			this.label5.Name = "label5";
			this.label5.Size = new System.Drawing.Size(72, 18);
			this.label5.TabIndex = 23;
			this.label5.Text = "Reg key:";
			this.label5.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// panel4
			// 
			this.panel4.Controls.Add(this.btnCancel);
			this.panel4.Controls.Add(this.btnApply);
			this.panel4.Dock = System.Windows.Forms.DockStyle.Bottom;
			this.panel4.Location = new System.Drawing.Point(3, 363);
			this.panel4.Name = "panel4";
			this.panel4.Size = new System.Drawing.Size(338, 26);
			this.panel4.TabIndex = 13;
			// 
			// btnCancel
			// 
			this.btnCancel.Location = new System.Drawing.Point(80, 4);
			this.btnCancel.Name = "btnCancel";
			this.btnCancel.Size = new System.Drawing.Size(75, 19);
			this.btnCancel.TabIndex = 1;
			this.btnCancel.Text = "Cancel";
			this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
			// 
			// btnApply
			// 
			this.btnApply.Location = new System.Drawing.Point(4, 4);
			this.btnApply.Name = "btnApply";
			this.btnApply.Size = new System.Drawing.Size(75, 19);
			this.btnApply.TabIndex = 0;
			this.btnApply.Text = "Apply";
			this.btnApply.Click += new System.EventHandler(this.btnApply_Click);
			// 
			// label7
			// 
			this.label7.Dock = System.Windows.Forms.DockStyle.Fill;
			this.label7.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
			this.label7.Location = new System.Drawing.Point(0, 0);
			this.label7.Name = "label7";
			this.label7.Size = new System.Drawing.Size(334, 11);
			this.label7.TabIndex = 0;
			this.label7.Text = "AutoKiller Authentication";
			this.label7.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// panel1
			// 
			this.panel1.BackColor = System.Drawing.SystemColors.ControlDark;
			this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
			this.panel1.Controls.Add(this.label1);
			this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
			this.panel1.Location = new System.Drawing.Point(3, 25);
			this.panel1.Name = "panel1";
			this.panel1.Size = new System.Drawing.Size(338, 15);
			this.panel1.TabIndex = 15;
			// 
			// label1
			// 
			this.label1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.label1.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
			this.label1.Location = new System.Drawing.Point(0, 0);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(334, 11);
			this.label1.TabIndex = 0;
			this.label1.Text = "Profile";
			this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// label2
			// 
			this.label2.Dock = System.Windows.Forms.DockStyle.Fill;
			this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.label2.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
			this.label2.Location = new System.Drawing.Point(0, 0);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(334, 18);
			this.label2.TabIndex = 0;
			this.label2.Text = "General Settings";
			this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// label3
			// 
			this.label3.Location = new System.Drawing.Point(8, 7);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(80, 19);
			this.label3.TabIndex = 0;
			this.label3.Text = "Select Profile:";
			this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// panel2
			// 
			this.panel2.BackColor = System.Drawing.SystemColors.ControlDark;
			this.panel2.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
			this.panel2.Controls.Add(this.label7);
			this.panel2.Dock = System.Windows.Forms.DockStyle.Top;
			this.panel2.Location = new System.Drawing.Point(3, 73);
			this.panel2.Name = "panel2";
			this.panel2.Size = new System.Drawing.Size(338, 15);
			this.panel2.TabIndex = 17;
			// 
			// panel3
			// 
			this.panel3.BackColor = System.Drawing.SystemColors.ActiveCaption;
			this.panel3.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
			this.panel3.Controls.Add(this.label2);
			this.panel3.Dock = System.Windows.Forms.DockStyle.Top;
			this.panel3.Location = new System.Drawing.Point(3, 3);
			this.panel3.Name = "panel3";
			this.panel3.Size = new System.Drawing.Size(338, 22);
			this.panel3.TabIndex = 12;
			// 
			// panel5
			// 
			this.panel5.Controls.Add(this.btnCreateNewProfile);
			this.panel5.Controls.Add(this.cbxProfile);
			this.panel5.Controls.Add(this.label3);
			this.panel5.Dock = System.Windows.Forms.DockStyle.Top;
			this.panel5.Location = new System.Drawing.Point(3, 40);
			this.panel5.Name = "panel5";
			this.panel5.Size = new System.Drawing.Size(338, 33);
			this.panel5.TabIndex = 16;
			// 
			// panel7
			// 
			this.panel7.BackColor = System.Drawing.SystemColors.ControlDark;
			this.panel7.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
			this.panel7.Controls.Add(this.label8);
			this.panel7.Dock = System.Windows.Forms.DockStyle.Top;
			this.panel7.Location = new System.Drawing.Point(3, 120);
			this.panel7.Name = "panel7";
			this.panel7.Size = new System.Drawing.Size(338, 15);
			this.panel7.TabIndex = 19;
			// 
			// panel9
			// 
			this.panel9.BackColor = System.Drawing.SystemColors.ControlDark;
			this.panel9.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
			this.panel9.Controls.Add(this.label4);
			this.panel9.Dock = System.Windows.Forms.DockStyle.Top;
			this.panel9.Location = new System.Drawing.Point(3, 168);
			this.panel9.Name = "panel9";
			this.panel9.Size = new System.Drawing.Size(338, 15);
			this.panel9.TabIndex = 21;
			// 
			// label4
			// 
			this.label4.Dock = System.Windows.Forms.DockStyle.Fill;
			this.label4.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
			this.label4.Location = new System.Drawing.Point(0, 0);
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size(334, 11);
			this.label4.TabIndex = 0;
			this.label4.Text = "Client Selection";
			this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// radToA
			// 
			this.radToA.Location = new System.Drawing.Point(64, 16);
			this.radToA.Name = "radToA";
			this.radToA.Size = new System.Drawing.Size(46, 16);
			this.radToA.TabIndex = 22;
			this.radToA.Text = "ToA";
			this.radToA.CheckedChanged += new System.EventHandler(this.radToA_CheckedChanged);
			// 
			// radSI
			// 
			this.radSI.Location = new System.Drawing.Point(8, 16);
			this.radSI.Name = "radSI";
			this.radSI.Size = new System.Drawing.Size(38, 16);
			this.radSI.TabIndex = 22;
			this.radSI.TabStop = true;
			this.radSI.Text = "SI";
			this.radSI.CheckedChanged += new System.EventHandler(this.radSI_CheckedChanged);
			// 
			// radCatacombs
			// 
			this.radCatacombs.Location = new System.Drawing.Point(8, 32);
			this.radCatacombs.Name = "radCatacombs";
			this.radCatacombs.Size = new System.Drawing.Size(88, 16);
			this.radCatacombs.TabIndex = 22;
			this.radCatacombs.Text = "Catacombs";
			this.radCatacombs.CheckedChanged += new System.EventHandler(this.radCatacombs_CheckedChanged);
			// 
			// radUS
			// 
			this.radUS.Location = new System.Drawing.Point(16, 16);
			this.radUS.Name = "radUS";
			this.radUS.Size = new System.Drawing.Size(40, 16);
			this.radUS.TabIndex = 24;
			this.radUS.TabStop = true;
			this.radUS.Text = "US";
			this.radUS.CheckedChanged += new System.EventHandler(this.radUS_CheckedChanged);
			// 
			// radEuro
			// 
			this.radEuro.Location = new System.Drawing.Point(16, 32);
			this.radEuro.Name = "radEuro";
			this.radEuro.Size = new System.Drawing.Size(46, 16);
			this.radEuro.TabIndex = 24;
			this.radEuro.Text = "Euro";
			this.radEuro.CheckedChanged += new System.EventHandler(this.radEuro_CheckedChanged);
			// 
			// txtGamePath
			// 
			this.txtGamePath.Location = new System.Drawing.Point(104, 296);
			this.txtGamePath.Name = "txtGamePath";
			this.txtGamePath.Size = new System.Drawing.Size(176, 20);
			this.txtGamePath.TabIndex = 23;
			this.txtGamePath.Text = "C:\\Mythic";
			this.txtGamePath.TextChanged += new System.EventHandler(this.txtGamePath_TextChanged);
			// 
			// label9
			// 
			this.label9.Location = new System.Drawing.Point(8, 298);
			this.label9.Name = "label9";
			this.label9.Size = new System.Drawing.Size(72, 19);
			this.label9.TabIndex = 28;
			this.label9.Text = "Game Path:";
			this.label9.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// label10
			// 
			this.label10.Location = new System.Drawing.Point(8, 320);
			this.label10.Name = "label10";
			this.label10.Size = new System.Drawing.Size(80, 19);
			this.label10.TabIndex = 28;
			this.label10.Text = "Chat Log Path:";
			this.label10.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// txtChatLogPath
			// 
			this.txtChatLogPath.Location = new System.Drawing.Point(104, 320);
			this.txtChatLogPath.Name = "txtChatLogPath";
			this.txtChatLogPath.Size = new System.Drawing.Size(176, 20);
			this.txtChatLogPath.TabIndex = 23;
			this.txtChatLogPath.Text = "C:\\Mythic";
			this.txtChatLogPath.TextChanged += new System.EventHandler(this.txtChatLogPath_TextChanged);
			// 
			// groupBox1
			// 
			this.groupBox1.Anchor = System.Windows.Forms.AnchorStyles.Top;
			this.groupBox1.Controls.Add(this.radUS);
			this.groupBox1.Controls.Add(this.radEuro);
			this.groupBox1.Location = new System.Drawing.Point(192, 8);
			this.groupBox1.Name = "groupBox1";
			this.groupBox1.Size = new System.Drawing.Size(64, 56);
			this.groupBox1.TabIndex = 29;
			this.groupBox1.TabStop = false;
			this.groupBox1.Text = "Locale";
			// 
			// groupBox2
			// 
			this.groupBox2.Anchor = System.Windows.Forms.AnchorStyles.Top;
			this.groupBox2.Controls.Add(this.radCatacombs);
			this.groupBox2.Controls.Add(this.radSI);
			this.groupBox2.Controls.Add(this.radToA);
			this.groupBox2.Location = new System.Drawing.Point(72, 8);
			this.groupBox2.Name = "groupBox2";
			this.groupBox2.Size = new System.Drawing.Size(112, 56);
			this.groupBox2.TabIndex = 30;
			this.groupBox2.TabStop = false;
			this.groupBox2.Text = "Client Type";
			// 
			// panel10
			// 
			this.panel10.Controls.Add(this.groupBox2);
			this.panel10.Controls.Add(this.groupBox1);
			this.panel10.Dock = System.Windows.Forms.DockStyle.Top;
			this.panel10.Location = new System.Drawing.Point(3, 183);
			this.panel10.Name = "panel10";
			this.panel10.Size = new System.Drawing.Size(338, 73);
			this.panel10.TabIndex = 31;
			// 
			// panel11
			// 
			this.panel11.BackColor = System.Drawing.SystemColors.ControlDark;
			this.panel11.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
			this.panel11.Controls.Add(this.label11);
			this.panel11.Dock = System.Windows.Forms.DockStyle.Top;
			this.panel11.Location = new System.Drawing.Point(3, 256);
			this.panel11.Name = "panel11";
			this.panel11.Size = new System.Drawing.Size(338, 15);
			this.panel11.TabIndex = 32;
			// 
			// label11
			// 
			this.label11.Dock = System.Windows.Forms.DockStyle.Fill;
			this.label11.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
			this.label11.Location = new System.Drawing.Point(0, 0);
			this.label11.Name = "label11";
			this.label11.Size = new System.Drawing.Size(334, 11);
			this.label11.TabIndex = 0;
			this.label11.Text = "Paths";
			this.label11.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// frmGeneral
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.ClientSize = new System.Drawing.Size(344, 392);
			this.Controls.Add(this.panel11);
			this.Controls.Add(this.panel10);
			this.Controls.Add(this.txtGamePath);
			this.Controls.Add(this.panel9);
			this.Controls.Add(this.panel8);
			this.Controls.Add(this.panel7);
			this.Controls.Add(this.panel6);
			this.Controls.Add(this.panel2);
			this.Controls.Add(this.panel5);
			this.Controls.Add(this.panel1);
			this.Controls.Add(this.panel4);
			this.Controls.Add(this.panel3);
			this.Controls.Add(this.label9);
			this.Controls.Add(this.label10);
			this.Controls.Add(this.txtChatLogPath);
			this.DockPadding.All = 3;
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
			this.Name = "frmGeneral";
			this.Text = "frmGeneral";
			this.Load += new System.EventHandler(this.frmGeneral_Load);
			this.panel8.ResumeLayout(false);
			this.panel6.ResumeLayout(false);
			this.panel4.ResumeLayout(false);
			this.panel1.ResumeLayout(false);
			this.panel2.ResumeLayout(false);
			this.panel3.ResumeLayout(false);
			this.panel5.ResumeLayout(false);
			this.panel7.ResumeLayout(false);
			this.panel9.ResumeLayout(false);
			this.groupBox1.ResumeLayout(false);
			this.groupBox2.ResumeLayout(false);
			this.panel10.ResumeLayout(false);
			this.panel11.ResumeLayout(false);
			this.ResumeLayout(false);

		}
		#endregion

		public event UpdateVariableDelegate UpdateVariable;

		public event CurrentProfileDelegate LoadCurrentProfile;
		public event CurrentProfileDelegate SaveCurrentProfile;

		public void DefineVariables( Profile profile)
		{
			profile.DefineVariable( "Client", "ToA");
			profile.DefineVariable( "Location", "US");
			profile.DefineVariable( "Password", "trial");

			profile.DefineVariable( "GamePath", "C:\\Mythic");
			profile.DefineVariable( "ChatLogPath", "C:\\Mythic");

			profile.DefineVariable( "Class", "");
		}

		public void OnProfileChange( Profile profile)
		{
			string Client = profile.GetString( "Client");
			if(Client == "ToA")
				radToA.Checked = true;
			else if(Client == "Catacombs")
				radCatacombs.Checked = true;
			else
				radSI.Checked = true;

			string Location = profile.GetString( "Location");
			if(Location == "US")
				radUS.Checked = true;
			else
				radEuro.Checked = true;

			txtPassword.Text = profile.GetString( "Password");
			txtGamePath.Text = profile.GetString( "GamePath");
			txtChatLogPath.Text = profile.GetString( "ChatLogPath");

			bool found = false;
			for( int i = 0; i < cbxBot.Items.Count; i ++)
				if( (string)cbxBot.Items[i] == profile.GetString("Class"))
				{
					cbxBot.SelectedIndex = i;
					found = true;
					break;
				}
			
			if( !found)
			{
				profile.ResetValue( "Class");
				cbxBot.SelectedIndex = -1;
			}
		}


		private void frmGeneral_Load(object sender, System.EventArgs e)
		{
			string directory = Path.GetDirectoryName(Application.ExecutablePath);
			if( !directory.EndsWith( "\\"))
				directory += "\\";
			directory += "Profiles";

			if( !Directory.Exists( directory))
				Directory.CreateDirectory( directory);

			string [] profiles = Directory.GetFiles( directory, "*.xml");

			cbxProfile.Items.Clear();
			foreach( string profile in profiles)
			{
				string profilename = Path.GetFileNameWithoutExtension( profile);
				cbxProfile.Items.Add( profilename);
			}
		}

		private void btnCreateNewProfile_Click(object sender, System.EventArgs e)
		{
			frmNewProfile frm = new frmNewProfile();
			if( frm.ShowDialog() == DialogResult.Cancel)
				return;

			frmMain frmmain = (frmMain) MdiParent;
			frmmain._profile.ResetAll();
			frmmain._profile.SaveProfile( frm.txtProfileName.Text);

			frmGeneral_Load( null, null);

			for( int i = 0; i < cbxProfile.Items.Count; i ++)
				if( (string)cbxProfile.Items[i] == frm.txtProfileName.Text)
				{
					cbxProfile.SelectedIndex = i;
					break;
				}
		}

		private void cbxProfile_SelectedIndexChanged(object sender, System.EventArgs e)
		{
			frmMain frmmain = (frmMain) MdiParent;
			if( cbxProfile.SelectedIndex != -1)
				frmmain._profile.LoadProfile( cbxProfile.Text);
		}

		private void btnApply_Click(object sender, System.EventArgs e)
		{
			if( SaveCurrentProfile != null)
				SaveCurrentProfile();
			
			//when creating a new profile we might enter a name, then the class.
			//since the profile index doesnt change, we need to force this call
			//so the "Class" profile setting is updated
			cbxBot_SelectedIndexChanged(null,null);
		}

		private void btnCancel_Click(object sender, System.EventArgs e)
		{
			if( LoadCurrentProfile != null)
				LoadCurrentProfile();
		}

		private int previousBotIndex = -1;
		private void cbxBot_SelectedIndexChanged(object sender, System.EventArgs e)
		{
			// You cant change this while initialized
			frmMain frmmain = (frmMain) MdiParent;
			if( frmmain._ak.GameProcess != 0 && previousBotIndex != cbxBot.SelectedIndex)
				cbxBot.SelectedIndex = previousBotIndex;

			previousBotIndex = cbxBot.SelectedIndex;
			if( UpdateVariable != null)
				UpdateVariable( "Class", cbxBot.Text);
		}

		private void radSI_CheckedChanged(object sender, System.EventArgs e)
		{
			if( UpdateVariable != null)
				UpdateVariable( "Client", "SI");
		
		}

		private void radToA_CheckedChanged(object sender, System.EventArgs e)
		{
			if( UpdateVariable != null)
				UpdateVariable( "Client", "ToA");
		
		}

		private void radCatacombs_CheckedChanged(object sender, System.EventArgs e)
		{
			if( UpdateVariable != null)
				UpdateVariable( "Client", "Catacombs");
		
		}

		private void radUS_CheckedChanged(object sender, System.EventArgs e)
		{
			if( UpdateVariable != null)
				UpdateVariable( "Location", "US");
		
		}

		private void radEuro_CheckedChanged(object sender, System.EventArgs e)
		{
			if( UpdateVariable != null)
				UpdateVariable( "Location", "Euro");
		
		}

		private void txtGamePath_TextChanged(object sender, System.EventArgs e)
		{
			if( UpdateVariable != null)
			{
				UpdateVariable( "GamePath", txtGamePath.Text);
			}
		
		}

		private void txtPassword_TextChanged(object sender, System.EventArgs e)
		{
			if( UpdateVariable != null)
			{
				UpdateVariable( "Password", txtPassword.Text);
			}
		}

		private void txtChatLogPath_TextChanged(object sender, System.EventArgs e)
		{
			if( UpdateVariable != null)
			{
				UpdateVariable( "ChatLogPath", txtChatLogPath.Text);
			}
		
		}
	}
}
