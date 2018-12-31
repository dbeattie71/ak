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
	/// Summary description for frmNewPatrolArea.
	/// </summary>
	public class frmNewPatrolArea : System.Windows.Forms.Form
	{
		private System.Windows.Forms.Button btnCancel;
		private System.Windows.Forms.Button btnCreate;
		public System.Windows.Forms.TextBox txtPatrolAreaName;
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		public frmNewPatrolArea()
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
			this.btnCancel = new System.Windows.Forms.Button();
			this.btnCreate = new System.Windows.Forms.Button();
			this.txtPatrolAreaName = new System.Windows.Forms.TextBox();
			this.SuspendLayout();
			// 
			// btnCancel
			// 
			this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.btnCancel.Location = new System.Drawing.Point(206, 28);
			this.btnCancel.Name = "btnCancel";
			this.btnCancel.Size = new System.Drawing.Size(72, 23);
			this.btnCancel.TabIndex = 5;
			this.btnCancel.Text = "Cancel";
			// 
			// btnCreate
			// 
			this.btnCreate.Location = new System.Drawing.Point(128, 28);
			this.btnCreate.Name = "btnCreate";
			this.btnCreate.Size = new System.Drawing.Size(72, 23);
			this.btnCreate.TabIndex = 4;
			this.btnCreate.Text = "Create";
			this.btnCreate.Click += new System.EventHandler(this.btnCreate_Click);
			// 
			// txtPatrolAreaName
			// 
			this.txtPatrolAreaName.Dock = System.Windows.Forms.DockStyle.Top;
			this.txtPatrolAreaName.Location = new System.Drawing.Point(4, 4);
			this.txtPatrolAreaName.MaxLength = 32;
			this.txtPatrolAreaName.Name = "txtPatrolAreaName";
			this.txtPatrolAreaName.Size = new System.Drawing.Size(274, 20);
			this.txtPatrolAreaName.TabIndex = 3;
			this.txtPatrolAreaName.Text = "";
			// 
			// frmNewPatrolArea
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.ClientSize = new System.Drawing.Size(282, 56);
			this.Controls.Add(this.btnCancel);
			this.Controls.Add(this.btnCreate);
			this.Controls.Add(this.txtPatrolAreaName);
			this.DockPadding.All = 4;
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
			this.Name = "frmNewPatrolArea";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			this.Text = "Name for a new Patrol Area";
			this.ResumeLayout(false);

		}
		#endregion

		private void btnCreate_Click(object sender, System.EventArgs e)
		{
			if( txtPatrolAreaName.Text == "")
			{
				MessageBox.Show( "Please enter a patrol area name.");
				return;
			}

			string directory = Path.GetDirectoryName(Application.ExecutablePath);
			if( !directory.EndsWith( "\\"))
				directory += "\\";
			directory += "PatrolAreas";

			if( !Directory.Exists( directory))
				Directory.CreateDirectory( directory);

			if( File.Exists( directory + "\\" + txtPatrolAreaName.Text + ".xml"))
			{
				if( MessageBox.Show( "A patrol area with this name already exists, do you want to overwrite it?", "Already exists", MessageBoxButtons.OKCancel) == DialogResult.OK)
					DialogResult = DialogResult.OK;
			}
			else
				DialogResult = DialogResult.OK;
		
		}

	}
}
