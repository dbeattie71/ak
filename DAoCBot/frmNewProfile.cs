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
	/// Summary description for frmNewProfile.
	/// </summary>
	public class frmNewProfile : System.Windows.Forms.Form
	{
		public System.Windows.Forms.TextBox txtProfileName;
		private System.Windows.Forms.Button btnCreate;
		private System.Windows.Forms.Button btnCancel;
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		public frmNewProfile()
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
			this.txtProfileName = new System.Windows.Forms.TextBox();
			this.btnCreate = new System.Windows.Forms.Button();
			this.btnCancel = new System.Windows.Forms.Button();
			this.SuspendLayout();
			// 
			// txtProfileName
			// 
			this.txtProfileName.Dock = System.Windows.Forms.DockStyle.Top;
			this.txtProfileName.Location = new System.Drawing.Point(4, 4);
			this.txtProfileName.MaxLength = 32;
			this.txtProfileName.Name = "txtProfileName";
			this.txtProfileName.Size = new System.Drawing.Size(274, 20);
			this.txtProfileName.TabIndex = 0;
			this.txtProfileName.Text = "";
			// 
			// btnCreate
			// 
			this.btnCreate.Location = new System.Drawing.Point(128, 28);
			this.btnCreate.Name = "btnCreate";
			this.btnCreate.Size = new System.Drawing.Size(72, 23);
			this.btnCreate.TabIndex = 1;
			this.btnCreate.Text = "Create";
			this.btnCreate.Click += new System.EventHandler(this.btnCreate_Click);
			// 
			// btnCancel
			// 
			this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.btnCancel.Location = new System.Drawing.Point(206, 28);
			this.btnCancel.Name = "btnCancel";
			this.btnCancel.Size = new System.Drawing.Size(72, 23);
			this.btnCancel.TabIndex = 2;
			this.btnCancel.Text = "Cancel";
			// 
			// frmNewProfile
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.ClientSize = new System.Drawing.Size(282, 56);
			this.Controls.Add(this.btnCancel);
			this.Controls.Add(this.btnCreate);
			this.Controls.Add(this.txtProfileName);
			this.DockPadding.All = 4;
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
			this.Name = "frmNewProfile";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			this.Text = "Enter a profile name";
			this.ResumeLayout(false);

		}
		#endregion

		private void btnCreate_Click(object sender, System.EventArgs e)
		{
			if( txtProfileName.Text == "")
			{
				MessageBox.Show( "Please enter a profile name.");
				return;
			}

			string directory = Path.GetDirectoryName(Application.ExecutablePath);
			if( !directory.EndsWith( "\\"))
				directory += "\\";
			directory += "Profiles";

			if( !Directory.Exists( directory))
				Directory.CreateDirectory( directory);

			if( File.Exists( directory + "\\" + txtProfileName.Text + ".xml"))
			{
				if( MessageBox.Show( "A profile with this name already exists, do you want to overwrite it?", "Already exists", MessageBoxButtons.OKCancel) == DialogResult.OK)
					DialogResult = DialogResult.OK;
			}
			else
				DialogResult = DialogResult.OK;
		}
	}
}
