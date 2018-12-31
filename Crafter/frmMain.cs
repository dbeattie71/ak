using AutoKillerScript;
using Microsoft.VisualBasic;
using Microsoft.VisualBasic.CompilerServices;
using System;
using System.Collections;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Resources;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;
using System.Threading;
using System.Windows.Forms;
using System.Xml;

namespace Crafter
{
	public class frmMain : Form
	{
		[AccessedThroughProperty("SaveFileDialog1")]
		private SaveFileDialog _SaveFileDialog1;

		[AccessedThroughProperty("TabPage2")]
		private TabPage _TabPage2;

		[AccessedThroughProperty("cmdQuit")]
		private Button _cmdQuit;

		[AccessedThroughProperty("txtRegKey")]
		private TextBox _txtRegKey;

		[AccessedThroughProperty("Label5")]
		private Label _Label5;

		[AccessedThroughProperty("TabControl1")]
		private TabControl _TabControl1;

		[AccessedThroughProperty("cmdStart")]
		private Button _cmdStart;

		[AccessedThroughProperty("Label17")]
		private Label _Label17;

		[AccessedThroughProperty("picStopped")]
		private PictureBox _picStopped;

		[AccessedThroughProperty("picRunning")]
		private PictureBox _picRunning;

		[AccessedThroughProperty("chkOrder")]
		private CheckBox _chkOrder;

		[AccessedThroughProperty("TabPage6")]
		private TabPage _TabPage6;

		[AccessedThroughProperty("Label16")]
		private Label _Label16;

		[AccessedThroughProperty("TxtSkillfilepath")]
		private TextBox _TxtSkillfilepath;

		[AccessedThroughProperty("chkPowerskill")]
		private CheckBox _chkPowerskill;

		[AccessedThroughProperty("ColumnHeader5")]
		private ColumnHeader _ColumnHeader5;

		[AccessedThroughProperty("btnReadSkillfile")]
		private Button _btnReadSkillfile;

		[AccessedThroughProperty("ColumnHeader3")]
		private ColumnHeader _ColumnHeader3;

		[AccessedThroughProperty("ColumnHeader2")]
		private ColumnHeader _ColumnHeader2;

		[AccessedThroughProperty("ColumnHeader1")]
		private ColumnHeader _ColumnHeader1;

		[AccessedThroughProperty("ListView1")]
		private ListView _ListView1;

		[AccessedThroughProperty("cbTradeskill")]
		private ComboBox _cbTradeskill;

		[AccessedThroughProperty("TabPage5")]
		private TabPage _TabPage5;

		[AccessedThroughProperty("Label15")]
		private Label _Label15;

		[AccessedThroughProperty("chkDisableSound")]
		private CheckBox _chkDisableSound;

		[AccessedThroughProperty("rbUS")]
		private RadioButton _rbUS;

		[AccessedThroughProperty("rbEuro")]
		private RadioButton _rbEuro;

		[AccessedThroughProperty("rbToA")]
		private RadioButton _rbToA;

		[AccessedThroughProperty("ColumnHeader6")]
		private ColumnHeader _ColumnHeader6;

		[AccessedThroughProperty("GroupBox4")]
		private GroupBox _GroupBox4;

		[AccessedThroughProperty("GroupBox10")]
		private GroupBox _GroupBox10;

		[AccessedThroughProperty("btnCreateXML")]
		private Button _btnCreateXML;

		[AccessedThroughProperty("chkKillProcess")]
		private CheckBox _chkKillProcess;

		[AccessedThroughProperty("txtGamePath")]
		private TextBox _txtGamePath;

		[AccessedThroughProperty("txtPopCurrentItem")]
		private TextBox _txtPopCurrentItem;

		[AccessedThroughProperty("btnCurrentItem")]
		private Button _btnCurrentItem;

		[AccessedThroughProperty("Label12")]
		private Label _Label12;

		[AccessedThroughProperty("Label9")]
		private Label _Label9;

		[AccessedThroughProperty("TabPage4")]
		private TabPage _TabPage4;

		[AccessedThroughProperty("TabControl2")]
		private TabControl _TabControl2;

		[AccessedThroughProperty("chkQuit")]
		private CheckBox _chkQuit;

		[AccessedThroughProperty("chkSelling")]
		private CheckBox _chkSelling;

		[AccessedThroughProperty("txtItem")]
		private TextBox _txtItem;

		[AccessedThroughProperty("Label7")]
		private Label _Label7;

		[AccessedThroughProperty("ColumnHeader7")]
		private ColumnHeader _ColumnHeader7;

		[AccessedThroughProperty("txtVendorName")]
		private TextBox _txtVendorName;

		[AccessedThroughProperty("chkSalvage")]
		private CheckBox _chkSalvage;

		[AccessedThroughProperty("chkStock")]
		private CheckBox _chkStock;

		[AccessedThroughProperty("txtForgeLoc")]
		private TextBox _txtForgeLoc;

		[AccessedThroughProperty("Label11")]
		private Label _Label11;

		[AccessedThroughProperty("txtVenLoc")]
		private TextBox _txtVenLoc;

		[AccessedThroughProperty("btnAddForgeLoc")]
		private Button _btnAddForgeLoc;

		[AccessedThroughProperty("btnAddVenLoc")]
		private Button _btnAddVenLoc;

		[AccessedThroughProperty("btnAddVenName")]
		private Button _btnAddVenName;

		[AccessedThroughProperty("chkStopQual")]
		private CheckBox _chkStopQual;

		[AccessedThroughProperty("txtQualNum")]
		private TextBox _txtQualNum;

		[AccessedThroughProperty("Label13")]
		private Label _Label13;

		[AccessedThroughProperty("ColumnHeader8")]
		private ColumnHeader _ColumnHeader8;

		[AccessedThroughProperty("Label14")]
		private Label _Label14;

		[AccessedThroughProperty("cbQuality")]
		private ComboBox _cbQuality;

		[AccessedThroughProperty("gbQual")]
		private GroupBox _gbQual;

		[AccessedThroughProperty("Label6")]
		private Label _Label6;

		[AccessedThroughProperty("txtDistance")]
		private TextBox _txtDistance;

		[AccessedThroughProperty("Label3")]
		private Label _Label3;

		[AccessedThroughProperty("txtCraftKey")]
		private TextBox _txtCraftKey;

		[AccessedThroughProperty("txtItemSlot")]
		private TextBox _txtItemSlot;

		[AccessedThroughProperty("Label8")]
		private Label _Label8;

		[AccessedThroughProperty("TabPage3")]
		private TabPage _TabPage3;

		[AccessedThroughProperty("lblItem100Quality")]
		private Label _lblItem100Quality;

		[AccessedThroughProperty("lblItem99Quality")]
		private Label _lblItem99Quality;

		[AccessedThroughProperty("Label18")]
		private Label _Label18;

		[AccessedThroughProperty("lblItem98Quality")]
		private Label _lblItem98Quality;

		[AccessedThroughProperty("lblItem97Quality")]
		private Label _lblItem97Quality;

		[AccessedThroughProperty("lblItem96Quality")]
		private Label _lblItem96Quality;

		[AccessedThroughProperty("lblItem95Quality")]
		private Label _lblItem95Quality;

		[AccessedThroughProperty("lblItem94Quality")]
		private Label _lblItem94Quality;

		[AccessedThroughProperty("rbDarkness")]
		private RadioButton _rbDarkness;

		[AccessedThroughProperty("Label53")]
		private Label _Label53;

		[AccessedThroughProperty("Label54")]
		private Label _Label54;

		[AccessedThroughProperty("Label52")]
		private Label _Label52;

		[AccessedThroughProperty("Label51")]
		private Label _Label51;

		[AccessedThroughProperty("Label50")]
		private Label _Label50;

		[AccessedThroughProperty("Label2")]
		private Label _Label2;

		[AccessedThroughProperty("ColumnHeader10")]
		private ColumnHeader _ColumnHeader10;

		[AccessedThroughProperty("Label1")]
		private Label _Label1;

		[AccessedThroughProperty("GroupBox1")]
		private GroupBox _GroupBox1;

		[AccessedThroughProperty("OpenFileDialog1")]
		private OpenFileDialog _OpenFileDialog1;

		[AccessedThroughProperty("btnSelectGamePath")]
		private Button _btnSelectGamePath;

		[AccessedThroughProperty("Label10")]
		private Label _Label10;

		[AccessedThroughProperty("ColumnHeader11")]
		private ColumnHeader _ColumnHeader11;

		[AccessedThroughProperty("ColumnHeader12")]
		private ColumnHeader _ColumnHeader12;

		[AccessedThroughProperty("btnSelectPSkillFile")]
		private Button _btnSelectPSkillFile;

		[AccessedThroughProperty("btnReadOrder")]
		private Button _btnReadOrder;

		[AccessedThroughProperty("rbCatacombs")]
		private RadioButton _rbCatacombs;

		[AccessedThroughProperty("btnOrderPath")]
		private Button _btnOrderPath;

		[AccessedThroughProperty("lvOrder")]
		private ListView _lvOrder;

		[AccessedThroughProperty("TabPage7")]
		private TabPage _TabPage7;

		[AccessedThroughProperty("GroupBox2")]
		private GroupBox _GroupBox2;

		[AccessedThroughProperty("WPs")]
		private ListBox _WPs;

		[AccessedThroughProperty("txtOrderfile")]
		private TextBox _txtOrderfile;

		[AccessedThroughProperty("cmdAddWP0")]
		private Button _cmdAddWP0;

		[AccessedThroughProperty("txtWPtxt0")]
		private TextBox _txtWPtxt0;

		[AccessedThroughProperty("Label41")]
		private Label _Label41;

		[AccessedThroughProperty("TabPage1")]
		private TabPage _TabPage1;

		[AccessedThroughProperty("TabPage8")]
		private TabPage _TabPage8;

		[AccessedThroughProperty("GroupBox3")]
		private GroupBox _GroupBox3;

		[AccessedThroughProperty("btnSave")]
		private Button _btnSave;

		[AccessedThroughProperty("Label19")]
		private Label _Label19;

		[AccessedThroughProperty("cbProfileName")]
		private ComboBox _cbProfileName;

		private IContainer components;

		private string S0;

		private string S1;

		private string S2;

		private string S3;

		private string S4;

		private string S5;

		private string S6;

		private string S7;

		private string S8;

		private string S9;

		private string S10;

		private string S11;

		private UserKeys Keys;

		private string folder;

		private bool DoSell;

		private Thread tCrafter;

		private Thread tSalvage;

		private Thread tOrder;

		private bool OkToCraft;

		private bool OkToSalvage;

		private string mCraftKey;

		private bool Running;

		private bool Beeping;

		private bool Stock;

		private string GamePath;

		private int PlayerX;

		private int PlayerY;

		private string MobName;

		private int Ninety4;

		private int Ninety5;

		private int Ninety6;

		private int Ninety7;

		private int Ninety8;

		private int Ninety9;

		private int MP;

		private string Quality;

		private short QualNum;

		private bool StopQual;

		private clsSkillDetail skilldetail;

		private ArrayList sd;

		private ArrayList alOrder;

		private clsOrderDetail TheOrder;

		private ArrayList mWaypoints;

		internal virtual Button btnAddForgeLoc
		{
			get
			{
				return this._btnAddForgeLoc;
			}
			[MethodImpl(32)]
			set
			{
				if (this._btnAddForgeLoc != null)
				{
					frmMain _frmMain = this;
					this._btnAddForgeLoc.remove_Click(new EventHandler(_frmMain, _frmMain.btnAddForgeLoc_Click));
				}
				this._btnAddForgeLoc = value;
				if (this._btnAddForgeLoc != null)
				{
					frmMain _frmMain1 = this;
					this._btnAddForgeLoc.add_Click(new EventHandler(_frmMain1, _frmMain1.btnAddForgeLoc_Click));
				}
			}
		}

		internal virtual Button btnAddVenLoc
		{
			get
			{
				return this._btnAddVenLoc;
			}
			[MethodImpl(32)]
			set
			{
				if (this._btnAddVenLoc != null)
				{
					frmMain _frmMain = this;
					this._btnAddVenLoc.remove_Click(new EventHandler(_frmMain, _frmMain.btnAddVenLoc_Click));
				}
				this._btnAddVenLoc = value;
				if (this._btnAddVenLoc != null)
				{
					frmMain _frmMain1 = this;
					this._btnAddVenLoc.add_Click(new EventHandler(_frmMain1, _frmMain1.btnAddVenLoc_Click));
				}
			}
		}

		internal virtual Button btnAddVenName
		{
			get
			{
				return this._btnAddVenName;
			}
			[MethodImpl(32)]
			set
			{
				if (this._btnAddVenName != null)
				{
					frmMain _frmMain = this;
					this._btnAddVenName.remove_Click(new EventHandler(_frmMain, _frmMain.btnAddVenName_Click));
				}
				this._btnAddVenName = value;
				if (this._btnAddVenName != null)
				{
					frmMain _frmMain1 = this;
					this._btnAddVenName.add_Click(new EventHandler(_frmMain1, _frmMain1.btnAddVenName_Click));
				}
			}
		}

		internal virtual Button btnCreateXML
		{
			get
			{
				return this._btnCreateXML;
			}
			[MethodImpl(32)]
			set
			{
				if (this._btnCreateXML != null)
				{
					frmMain _frmMain = this;
					this._btnCreateXML.remove_Click(new EventHandler(_frmMain, _frmMain.btnCreateXML_Click));
				}
				this._btnCreateXML = value;
				if (this._btnCreateXML != null)
				{
					frmMain _frmMain1 = this;
					this._btnCreateXML.add_Click(new EventHandler(_frmMain1, _frmMain1.btnCreateXML_Click));
				}
			}
		}

		internal virtual Button btnCurrentItem
		{
			get
			{
				return this._btnCurrentItem;
			}
			[MethodImpl(32)]
			set
			{
				if (this._btnCurrentItem != null)
				{
					frmMain _frmMain = this;
					this._btnCurrentItem.remove_Click(new EventHandler(_frmMain, _frmMain.btnCurrentItem_Click));
				}
				this._btnCurrentItem = value;
				if (this._btnCurrentItem != null)
				{
					frmMain _frmMain1 = this;
					this._btnCurrentItem.add_Click(new EventHandler(_frmMain1, _frmMain1.btnCurrentItem_Click));
				}
			}
		}

		internal virtual Button btnOrderPath
		{
			get
			{
				return this._btnOrderPath;
			}
			[MethodImpl(32)]
			set
			{
				if (this._btnOrderPath != null)
				{
					frmMain _frmMain = this;
					this._btnOrderPath.remove_Click(new EventHandler(_frmMain, _frmMain.btnOrderPath_Click));
				}
				this._btnOrderPath = value;
				if (this._btnOrderPath != null)
				{
					frmMain _frmMain1 = this;
					this._btnOrderPath.add_Click(new EventHandler(_frmMain1, _frmMain1.btnOrderPath_Click));
				}
			}
		}

		internal virtual Button btnReadOrder
		{
			get
			{
				return this._btnReadOrder;
			}
			[MethodImpl(32)]
			set
			{
				if (this._btnReadOrder != null)
				{
					frmMain _frmMain = this;
					this._btnReadOrder.remove_Click(new EventHandler(_frmMain, _frmMain.btnReadOrder_Click));
				}
				this._btnReadOrder = value;
				if (this._btnReadOrder != null)
				{
					frmMain _frmMain1 = this;
					this._btnReadOrder.add_Click(new EventHandler(_frmMain1, _frmMain1.btnReadOrder_Click));
				}
			}
		}

		internal virtual Button btnReadSkillfile
		{
			get
			{
				return this._btnReadSkillfile;
			}
			[MethodImpl(32)]
			set
			{
				if (this._btnReadSkillfile != null)
				{
					frmMain _frmMain = this;
					this._btnReadSkillfile.remove_Click(new EventHandler(_frmMain, _frmMain.btnReadSkillfile_Click));
				}
				this._btnReadSkillfile = value;
				if (this._btnReadSkillfile != null)
				{
					frmMain _frmMain1 = this;
					this._btnReadSkillfile.add_Click(new EventHandler(_frmMain1, _frmMain1.btnReadSkillfile_Click));
				}
			}
		}

		internal virtual Button btnSave
		{
			get
			{
				return this._btnSave;
			}
			[MethodImpl(32)]
			set
			{
				if (this._btnSave != null)
				{
					frmMain _frmMain = this;
					this._btnSave.remove_Click(new EventHandler(_frmMain, _frmMain.btnSave_Click));
				}
				this._btnSave = value;
				if (this._btnSave != null)
				{
					frmMain _frmMain1 = this;
					this._btnSave.add_Click(new EventHandler(_frmMain1, _frmMain1.btnSave_Click));
				}
			}
		}

		internal virtual Button btnSelectGamePath
		{
			get
			{
				return this._btnSelectGamePath;
			}
			[MethodImpl(32)]
			set
			{
				if (this._btnSelectGamePath != null)
				{
					frmMain _frmMain = this;
					this._btnSelectGamePath.remove_Click(new EventHandler(_frmMain, _frmMain.btnSelectGamePath_Click));
				}
				this._btnSelectGamePath = value;
				if (this._btnSelectGamePath != null)
				{
					frmMain _frmMain1 = this;
					this._btnSelectGamePath.add_Click(new EventHandler(_frmMain1, _frmMain1.btnSelectGamePath_Click));
				}
			}
		}

		internal virtual Button btnSelectPSkillFile
		{
			get
			{
				return this._btnSelectPSkillFile;
			}
			[MethodImpl(32)]
			set
			{
				if (this._btnSelectPSkillFile != null)
				{
					frmMain _frmMain = this;
					this._btnSelectPSkillFile.remove_Click(new EventHandler(_frmMain, _frmMain.btnSelectPSkillFile_Click));
				}
				this._btnSelectPSkillFile = value;
				if (this._btnSelectPSkillFile != null)
				{
					frmMain _frmMain1 = this;
					this._btnSelectPSkillFile.add_Click(new EventHandler(_frmMain1, _frmMain1.btnSelectPSkillFile_Click));
				}
			}
		}

		internal virtual ComboBox cbProfileName
		{
			get
			{
				return this._cbProfileName;
			}
			[MethodImpl(32)]
			set
			{
				if (this._cbProfileName != null)
				{
					frmMain _frmMain = this;
					this._cbProfileName.remove_SelectedIndexChanged(new EventHandler(_frmMain, _frmMain.cbProfileName_SelectedIndexChanged));
				}
				this._cbProfileName = value;
				if (this._cbProfileName != null)
				{
					frmMain _frmMain1 = this;
					this._cbProfileName.add_SelectedIndexChanged(new EventHandler(_frmMain1, _frmMain1.cbProfileName_SelectedIndexChanged));
				}
			}
		}

		internal virtual ComboBox cbQuality
		{
			get
			{
				return this._cbQuality;
			}
			[MethodImpl(32)]
			set
			{
				if (this._cbQuality != null)
				{
					frmMain _frmMain = this;
					this._cbQuality.remove_SelectedIndexChanged(new EventHandler(_frmMain, _frmMain.cbQuality_SelectedIndexChanged));
				}
				this._cbQuality = value;
				if (this._cbQuality != null)
				{
					frmMain _frmMain1 = this;
					this._cbQuality.add_SelectedIndexChanged(new EventHandler(_frmMain1, _frmMain1.cbQuality_SelectedIndexChanged));
				}
			}
		}

		internal virtual ComboBox cbTradeskill
		{
			get
			{
				return this._cbTradeskill;
			}
			[MethodImpl(32)]
			set
			{
				this._cbTradeskill == null;
				this._cbTradeskill = value;
				this._cbTradeskill == null;
			}
		}

		internal virtual CheckBox chkDisableSound
		{
			get
			{
				return this._chkDisableSound;
			}
			[MethodImpl(32)]
			set
			{
				this._chkDisableSound == null;
				this._chkDisableSound = value;
				this._chkDisableSound == null;
			}
		}

		internal virtual CheckBox chkKillProcess
		{
			get
			{
				return this._chkKillProcess;
			}
			[MethodImpl(32)]
			set
			{
				this._chkKillProcess == null;
				this._chkKillProcess = value;
				this._chkKillProcess == null;
			}
		}

		internal virtual CheckBox chkOrder
		{
			get
			{
				return this._chkOrder;
			}
			[MethodImpl(32)]
			set
			{
				this._chkOrder == null;
				this._chkOrder = value;
				this._chkOrder == null;
			}
		}

		internal virtual CheckBox chkPowerskill
		{
			get
			{
				return this._chkPowerskill;
			}
			[MethodImpl(32)]
			set
			{
				this._chkPowerskill == null;
				this._chkPowerskill = value;
				this._chkPowerskill == null;
			}
		}

		internal virtual CheckBox chkQuit
		{
			get
			{
				return this._chkQuit;
			}
			[MethodImpl(32)]
			set
			{
				this._chkQuit == null;
				this._chkQuit = value;
				this._chkQuit == null;
			}
		}

		internal virtual CheckBox chkSalvage
		{
			get
			{
				return this._chkSalvage;
			}
			[MethodImpl(32)]
			set
			{
				this._chkSalvage == null;
				this._chkSalvage = value;
				this._chkSalvage == null;
			}
		}

		internal virtual CheckBox chkSelling
		{
			get
			{
				return this._chkSelling;
			}
			[MethodImpl(32)]
			set
			{
				if (this._chkSelling != null)
				{
					frmMain _frmMain = this;
					this._chkSelling.remove_CheckedChanged(new EventHandler(_frmMain, _frmMain.chkSelling_CheckedChanged));
				}
				this._chkSelling = value;
				if (this._chkSelling != null)
				{
					frmMain _frmMain1 = this;
					this._chkSelling.add_CheckedChanged(new EventHandler(_frmMain1, _frmMain1.chkSelling_CheckedChanged));
				}
			}
		}

		internal virtual CheckBox chkStock
		{
			get
			{
				return this._chkStock;
			}
			[MethodImpl(32)]
			set
			{
				if (this._chkStock != null)
				{
					frmMain _frmMain = this;
					this._chkStock.remove_CheckedChanged(new EventHandler(_frmMain, _frmMain.chkStock_CheckedChanged));
				}
				this._chkStock = value;
				if (this._chkStock != null)
				{
					frmMain _frmMain1 = this;
					this._chkStock.add_CheckedChanged(new EventHandler(_frmMain1, _frmMain1.chkStock_CheckedChanged));
				}
			}
		}

		internal virtual CheckBox chkStopQual
		{
			get
			{
				return this._chkStopQual;
			}
			[MethodImpl(32)]
			set
			{
				if (this._chkStopQual != null)
				{
					frmMain _frmMain = this;
					this._chkStopQual.remove_CheckedChanged(new EventHandler(_frmMain, _frmMain.chkStopQual_CheckedChanged));
				}
				this._chkStopQual = value;
				if (this._chkStopQual != null)
				{
					frmMain _frmMain1 = this;
					this._chkStopQual.add_CheckedChanged(new EventHandler(_frmMain1, _frmMain1.chkStopQual_CheckedChanged));
				}
			}
		}

		internal virtual Button cmdAddWP0
		{
			get
			{
				return this._cmdAddWP0;
			}
			[MethodImpl(32)]
			set
			{
				if (this._cmdAddWP0 != null)
				{
					frmMain _frmMain = this;
					this._cmdAddWP0.remove_Click(new EventHandler(_frmMain, _frmMain.cmdAddWP0_Click));
				}
				this._cmdAddWP0 = value;
				if (this._cmdAddWP0 != null)
				{
					frmMain _frmMain1 = this;
					this._cmdAddWP0.add_Click(new EventHandler(_frmMain1, _frmMain1.cmdAddWP0_Click));
				}
			}
		}

		internal virtual Button cmdQuit
		{
			get
			{
				return this._cmdQuit;
			}
			[MethodImpl(32)]
			set
			{
				if (this._cmdQuit != null)
				{
					frmMain _frmMain = this;
					this._cmdQuit.remove_Click(new EventHandler(_frmMain, _frmMain.cmdQuit_Click));
				}
				this._cmdQuit = value;
				if (this._cmdQuit != null)
				{
					frmMain _frmMain1 = this;
					this._cmdQuit.add_Click(new EventHandler(_frmMain1, _frmMain1.cmdQuit_Click));
				}
			}
		}

		public virtual Button cmdStart
		{
			get
			{
				return this._cmdStart;
			}
			[MethodImpl(32)]
			set
			{
				if (this._cmdStart != null)
				{
					frmMain _frmMain = this;
					this._cmdStart.remove_Click(new EventHandler(_frmMain, _frmMain.cmdStart_Click));
				}
				this._cmdStart = value;
				if (this._cmdStart != null)
				{
					frmMain _frmMain1 = this;
					this._cmdStart.add_Click(new EventHandler(_frmMain1, _frmMain1.cmdStart_Click));
				}
			}
		}

		internal virtual ColumnHeader ColumnHeader1
		{
			get
			{
				return this._ColumnHeader1;
			}
			[MethodImpl(32)]
			set
			{
				this._ColumnHeader1 == null;
				this._ColumnHeader1 = value;
				this._ColumnHeader1 == null;
			}
		}

		internal virtual ColumnHeader ColumnHeader10
		{
			get
			{
				return this._ColumnHeader10;
			}
			[MethodImpl(32)]
			set
			{
				this._ColumnHeader10 == null;
				this._ColumnHeader10 = value;
				this._ColumnHeader10 == null;
			}
		}

		internal virtual ColumnHeader ColumnHeader11
		{
			get
			{
				return this._ColumnHeader11;
			}
			[MethodImpl(32)]
			set
			{
				this._ColumnHeader11 == null;
				this._ColumnHeader11 = value;
				this._ColumnHeader11 == null;
			}
		}

		internal virtual ColumnHeader ColumnHeader12
		{
			get
			{
				return this._ColumnHeader12;
			}
			[MethodImpl(32)]
			set
			{
				this._ColumnHeader12 == null;
				this._ColumnHeader12 = value;
				this._ColumnHeader12 == null;
			}
		}

		internal virtual ColumnHeader ColumnHeader2
		{
			get
			{
				return this._ColumnHeader2;
			}
			[MethodImpl(32)]
			set
			{
				this._ColumnHeader2 == null;
				this._ColumnHeader2 = value;
				this._ColumnHeader2 == null;
			}
		}

		internal virtual ColumnHeader ColumnHeader3
		{
			get
			{
				return this._ColumnHeader3;
			}
			[MethodImpl(32)]
			set
			{
				this._ColumnHeader3 == null;
				this._ColumnHeader3 = value;
				this._ColumnHeader3 == null;
			}
		}

		internal virtual ColumnHeader ColumnHeader5
		{
			get
			{
				return this._ColumnHeader5;
			}
			[MethodImpl(32)]
			set
			{
				this._ColumnHeader5 == null;
				this._ColumnHeader5 = value;
				this._ColumnHeader5 == null;
			}
		}

		internal virtual ColumnHeader ColumnHeader6
		{
			get
			{
				return this._ColumnHeader6;
			}
			[MethodImpl(32)]
			set
			{
				this._ColumnHeader6 == null;
				this._ColumnHeader6 = value;
				this._ColumnHeader6 == null;
			}
		}

		internal virtual ColumnHeader ColumnHeader7
		{
			get
			{
				return this._ColumnHeader7;
			}
			[MethodImpl(32)]
			set
			{
				this._ColumnHeader7 == null;
				this._ColumnHeader7 = value;
				this._ColumnHeader7 == null;
			}
		}

		internal virtual ColumnHeader ColumnHeader8
		{
			get
			{
				return this._ColumnHeader8;
			}
			[MethodImpl(32)]
			set
			{
				this._ColumnHeader8 == null;
				this._ColumnHeader8 = value;
				this._ColumnHeader8 == null;
			}
		}

		internal virtual GroupBox gbQual
		{
			get
			{
				return this._gbQual;
			}
			[MethodImpl(32)]
			set
			{
				this._gbQual == null;
				this._gbQual = value;
				this._gbQual == null;
			}
		}

		internal virtual GroupBox GroupBox1
		{
			get
			{
				return this._GroupBox1;
			}
			[MethodImpl(32)]
			set
			{
				this._GroupBox1 == null;
				this._GroupBox1 = value;
				this._GroupBox1 == null;
			}
		}

		internal virtual GroupBox GroupBox10
		{
			get
			{
				return this._GroupBox10;
			}
			[MethodImpl(32)]
			set
			{
				this._GroupBox10 == null;
				this._GroupBox10 = value;
				this._GroupBox10 == null;
			}
		}

		internal virtual GroupBox GroupBox2
		{
			get
			{
				return this._GroupBox2;
			}
			[MethodImpl(32)]
			set
			{
				this._GroupBox2 == null;
				this._GroupBox2 = value;
				this._GroupBox2 == null;
			}
		}

		internal virtual GroupBox GroupBox3
		{
			get
			{
				return this._GroupBox3;
			}
			[MethodImpl(32)]
			set
			{
				this._GroupBox3 == null;
				this._GroupBox3 = value;
				this._GroupBox3 == null;
			}
		}

		internal virtual GroupBox GroupBox4
		{
			get
			{
				return this._GroupBox4;
			}
			[MethodImpl(32)]
			set
			{
				this._GroupBox4 == null;
				this._GroupBox4 = value;
				this._GroupBox4 == null;
			}
		}

		internal virtual Label Label1
		{
			get
			{
				return this._Label1;
			}
			[MethodImpl(32)]
			set
			{
				this._Label1 == null;
				this._Label1 = value;
				this._Label1 == null;
			}
		}

		public virtual Label Label10
		{
			get
			{
				return this._Label10;
			}
			[MethodImpl(32)]
			set
			{
				this._Label10 == null;
				this._Label10 = value;
				this._Label10 == null;
			}
		}

		internal virtual Label Label11
		{
			get
			{
				return this._Label11;
			}
			[MethodImpl(32)]
			set
			{
				this._Label11 == null;
				this._Label11 = value;
				this._Label11 == null;
			}
		}

		internal virtual Label Label12
		{
			get
			{
				return this._Label12;
			}
			[MethodImpl(32)]
			set
			{
				this._Label12 == null;
				this._Label12 = value;
				this._Label12 == null;
			}
		}

		internal virtual Label Label13
		{
			get
			{
				return this._Label13;
			}
			[MethodImpl(32)]
			set
			{
				this._Label13 == null;
				this._Label13 = value;
				this._Label13 == null;
			}
		}

		internal virtual Label Label14
		{
			get
			{
				return this._Label14;
			}
			[MethodImpl(32)]
			set
			{
				this._Label14 == null;
				this._Label14 = value;
				this._Label14 == null;
			}
		}

		internal virtual Label Label15
		{
			get
			{
				return this._Label15;
			}
			[MethodImpl(32)]
			set
			{
				this._Label15 == null;
				this._Label15 = value;
				this._Label15 == null;
			}
		}

		internal virtual Label Label16
		{
			get
			{
				return this._Label16;
			}
			[MethodImpl(32)]
			set
			{
				this._Label16 == null;
				this._Label16 = value;
				this._Label16 == null;
			}
		}

		internal virtual Label Label17
		{
			get
			{
				return this._Label17;
			}
			[MethodImpl(32)]
			set
			{
				this._Label17 == null;
				this._Label17 = value;
				this._Label17 == null;
			}
		}

		internal virtual Label Label18
		{
			get
			{
				return this._Label18;
			}
			[MethodImpl(32)]
			set
			{
				this._Label18 == null;
				this._Label18 = value;
				this._Label18 == null;
			}
		}

		internal virtual Label Label19
		{
			get
			{
				return this._Label19;
			}
			[MethodImpl(32)]
			set
			{
				this._Label19 == null;
				this._Label19 = value;
				this._Label19 == null;
			}
		}

		internal virtual Label Label2
		{
			get
			{
				return this._Label2;
			}
			[MethodImpl(32)]
			set
			{
				this._Label2 == null;
				this._Label2 = value;
				this._Label2 == null;
			}
		}

		public virtual Label Label3
		{
			get
			{
				return this._Label3;
			}
			[MethodImpl(32)]
			set
			{
				this._Label3 == null;
				this._Label3 = value;
				this._Label3 == null;
			}
		}

		internal virtual Label Label41
		{
			get
			{
				return this._Label41;
			}
			[MethodImpl(32)]
			set
			{
				this._Label41 == null;
				this._Label41 = value;
				this._Label41 == null;
			}
		}

		public virtual Label Label5
		{
			get
			{
				return this._Label5;
			}
			[MethodImpl(32)]
			set
			{
				this._Label5 == null;
				this._Label5 = value;
				this._Label5 == null;
			}
		}

		internal virtual Label Label50
		{
			get
			{
				return this._Label50;
			}
			[MethodImpl(32)]
			set
			{
				this._Label50 == null;
				this._Label50 = value;
				this._Label50 == null;
			}
		}

		internal virtual Label Label51
		{
			get
			{
				return this._Label51;
			}
			[MethodImpl(32)]
			set
			{
				this._Label51 == null;
				this._Label51 = value;
				this._Label51 == null;
			}
		}

		internal virtual Label Label52
		{
			get
			{
				return this._Label52;
			}
			[MethodImpl(32)]
			set
			{
				this._Label52 == null;
				this._Label52 = value;
				this._Label52 == null;
			}
		}

		internal virtual Label Label53
		{
			get
			{
				return this._Label53;
			}
			[MethodImpl(32)]
			set
			{
				this._Label53 == null;
				this._Label53 = value;
				this._Label53 == null;
			}
		}

		internal virtual Label Label54
		{
			get
			{
				return this._Label54;
			}
			[MethodImpl(32)]
			set
			{
				this._Label54 == null;
				this._Label54 = value;
				this._Label54 == null;
			}
		}

		public virtual Label Label6
		{
			get
			{
				return this._Label6;
			}
			[MethodImpl(32)]
			set
			{
				this._Label6 == null;
				this._Label6 = value;
				this._Label6 == null;
			}
		}

		public virtual Label Label7
		{
			get
			{
				return this._Label7;
			}
			[MethodImpl(32)]
			set
			{
				this._Label7 == null;
				this._Label7 = value;
				this._Label7 == null;
			}
		}

		public virtual Label Label8
		{
			get
			{
				return this._Label8;
			}
			[MethodImpl(32)]
			set
			{
				this._Label8 == null;
				this._Label8 = value;
				this._Label8 == null;
			}
		}

		internal virtual Label Label9
		{
			get
			{
				return this._Label9;
			}
			[MethodImpl(32)]
			set
			{
				this._Label9 == null;
				this._Label9 = value;
				this._Label9 == null;
			}
		}

		internal virtual Label lblItem100Quality
		{
			get
			{
				return this._lblItem100Quality;
			}
			[MethodImpl(32)]
			set
			{
				this._lblItem100Quality == null;
				this._lblItem100Quality = value;
				this._lblItem100Quality == null;
			}
		}

		internal virtual Label lblItem94Quality
		{
			get
			{
				return this._lblItem94Quality;
			}
			[MethodImpl(32)]
			set
			{
				this._lblItem94Quality == null;
				this._lblItem94Quality = value;
				this._lblItem94Quality == null;
			}
		}

		internal virtual Label lblItem95Quality
		{
			get
			{
				return this._lblItem95Quality;
			}
			[MethodImpl(32)]
			set
			{
				this._lblItem95Quality == null;
				this._lblItem95Quality = value;
				this._lblItem95Quality == null;
			}
		}

		internal virtual Label lblItem96Quality
		{
			get
			{
				return this._lblItem96Quality;
			}
			[MethodImpl(32)]
			set
			{
				this._lblItem96Quality == null;
				this._lblItem96Quality = value;
				this._lblItem96Quality == null;
			}
		}

		internal virtual Label lblItem97Quality
		{
			get
			{
				return this._lblItem97Quality;
			}
			[MethodImpl(32)]
			set
			{
				this._lblItem97Quality == null;
				this._lblItem97Quality = value;
				this._lblItem97Quality == null;
			}
		}

		internal virtual Label lblItem98Quality
		{
			get
			{
				return this._lblItem98Quality;
			}
			[MethodImpl(32)]
			set
			{
				this._lblItem98Quality == null;
				this._lblItem98Quality = value;
				this._lblItem98Quality == null;
			}
		}

		internal virtual Label lblItem99Quality
		{
			get
			{
				return this._lblItem99Quality;
			}
			[MethodImpl(32)]
			set
			{
				this._lblItem99Quality == null;
				this._lblItem99Quality = value;
				this._lblItem99Quality == null;
			}
		}

		internal virtual ListView ListView1
		{
			get
			{
				return this._ListView1;
			}
			[MethodImpl(32)]
			set
			{
				this._ListView1 == null;
				this._ListView1 = value;
				this._ListView1 == null;
			}
		}

		internal virtual ListView lvOrder
		{
			get
			{
				return this._lvOrder;
			}
			[MethodImpl(32)]
			set
			{
				this._lvOrder == null;
				this._lvOrder = value;
				this._lvOrder == null;
			}
		}

		internal virtual OpenFileDialog OpenFileDialog1
		{
			get
			{
				return this._OpenFileDialog1;
			}
			[MethodImpl(32)]
			set
			{
				this._OpenFileDialog1 == null;
				this._OpenFileDialog1 = value;
				this._OpenFileDialog1 == null;
			}
		}

		internal virtual PictureBox picRunning
		{
			get
			{
				return this._picRunning;
			}
			[MethodImpl(32)]
			set
			{
				if (this._picRunning != null)
				{
					frmMain _frmMain = this;
					this._picRunning.remove_Click(new EventHandler(_frmMain, _frmMain.picRunning_Click));
				}
				this._picRunning = value;
				if (this._picRunning != null)
				{
					frmMain _frmMain1 = this;
					this._picRunning.add_Click(new EventHandler(_frmMain1, _frmMain1.picRunning_Click));
				}
			}
		}

		internal virtual PictureBox picStopped
		{
			get
			{
				return this._picStopped;
			}
			[MethodImpl(32)]
			set
			{
				this._picStopped == null;
				this._picStopped = value;
				this._picStopped == null;
			}
		}

		internal virtual RadioButton rbCatacombs
		{
			get
			{
				return this._rbCatacombs;
			}
			[MethodImpl(32)]
			set
			{
				this._rbCatacombs == null;
				this._rbCatacombs = value;
				this._rbCatacombs == null;
			}
		}

		internal virtual RadioButton rbDarkness
		{
			get
			{
				return this._rbDarkness;
			}
			[MethodImpl(32)]
			set
			{
				this._rbDarkness == null;
				this._rbDarkness = value;
				this._rbDarkness == null;
			}
		}

		internal virtual RadioButton rbEuro
		{
			get
			{
				return this._rbEuro;
			}
			[MethodImpl(32)]
			set
			{
				this._rbEuro == null;
				this._rbEuro = value;
				this._rbEuro == null;
			}
		}

		internal virtual RadioButton rbToA
		{
			get
			{
				return this._rbToA;
			}
			[MethodImpl(32)]
			set
			{
				this._rbToA == null;
				this._rbToA = value;
				this._rbToA == null;
			}
		}

		internal virtual RadioButton rbUS
		{
			get
			{
				return this._rbUS;
			}
			[MethodImpl(32)]
			set
			{
				this._rbUS == null;
				this._rbUS = value;
				this._rbUS == null;
			}
		}

		internal virtual SaveFileDialog SaveFileDialog1
		{
			get
			{
				return this._SaveFileDialog1;
			}
			[MethodImpl(32)]
			set
			{
				this._SaveFileDialog1 == null;
				this._SaveFileDialog1 = value;
				this._SaveFileDialog1 == null;
			}
		}

		internal virtual TabControl TabControl1
		{
			get
			{
				return this._TabControl1;
			}
			[MethodImpl(32)]
			set
			{
				this._TabControl1 == null;
				this._TabControl1 = value;
				this._TabControl1 == null;
			}
		}

		internal virtual TabControl TabControl2
		{
			get
			{
				return this._TabControl2;
			}
			[MethodImpl(32)]
			set
			{
				this._TabControl2 == null;
				this._TabControl2 = value;
				this._TabControl2 == null;
			}
		}

		internal virtual TabPage TabPage1
		{
			get
			{
				return this._TabPage1;
			}
			[MethodImpl(32)]
			set
			{
				this._TabPage1 == null;
				this._TabPage1 = value;
				this._TabPage1 == null;
			}
		}

		internal virtual TabPage TabPage2
		{
			get
			{
				return this._TabPage2;
			}
			[MethodImpl(32)]
			set
			{
				this._TabPage2 == null;
				this._TabPage2 = value;
				this._TabPage2 == null;
			}
		}

		internal virtual TabPage TabPage3
		{
			get
			{
				return this._TabPage3;
			}
			[MethodImpl(32)]
			set
			{
				this._TabPage3 == null;
				this._TabPage3 = value;
				this._TabPage3 == null;
			}
		}

		internal virtual TabPage TabPage4
		{
			get
			{
				return this._TabPage4;
			}
			[MethodImpl(32)]
			set
			{
				this._TabPage4 == null;
				this._TabPage4 = value;
				this._TabPage4 == null;
			}
		}

		internal virtual TabPage TabPage5
		{
			get
			{
				return this._TabPage5;
			}
			[MethodImpl(32)]
			set
			{
				this._TabPage5 == null;
				this._TabPage5 = value;
				this._TabPage5 == null;
			}
		}

		internal virtual TabPage TabPage6
		{
			get
			{
				return this._TabPage6;
			}
			[MethodImpl(32)]
			set
			{
				this._TabPage6 == null;
				this._TabPage6 = value;
				this._TabPage6 == null;
			}
		}

		internal virtual TabPage TabPage7
		{
			get
			{
				return this._TabPage7;
			}
			[MethodImpl(32)]
			set
			{
				this._TabPage7 == null;
				this._TabPage7 = value;
				this._TabPage7 == null;
			}
		}

		internal virtual TabPage TabPage8
		{
			get
			{
				return this._TabPage8;
			}
			[MethodImpl(32)]
			set
			{
				this._TabPage8 == null;
				this._TabPage8 = value;
				this._TabPage8 == null;
			}
		}

		internal virtual TextBox txtCraftKey
		{
			get
			{
				return this._txtCraftKey;
			}
			[MethodImpl(32)]
			set
			{
				this._txtCraftKey == null;
				this._txtCraftKey = value;
				this._txtCraftKey == null;
			}
		}

		internal virtual TextBox txtDistance
		{
			get
			{
				return this._txtDistance;
			}
			[MethodImpl(32)]
			set
			{
				this._txtDistance == null;
				this._txtDistance = value;
				this._txtDistance == null;
			}
		}

		internal virtual TextBox txtForgeLoc
		{
			get
			{
				return this._txtForgeLoc;
			}
			[MethodImpl(32)]
			set
			{
				this._txtForgeLoc == null;
				this._txtForgeLoc = value;
				this._txtForgeLoc == null;
			}
		}

		internal virtual TextBox txtGamePath
		{
			get
			{
				return this._txtGamePath;
			}
			[MethodImpl(32)]
			set
			{
				this._txtGamePath == null;
				this._txtGamePath = value;
				this._txtGamePath == null;
			}
		}

		internal virtual TextBox txtItem
		{
			get
			{
				return this._txtItem;
			}
			[MethodImpl(32)]
			set
			{
				this._txtItem == null;
				this._txtItem = value;
				this._txtItem == null;
			}
		}

		internal virtual TextBox txtItemSlot
		{
			get
			{
				return this._txtItemSlot;
			}
			[MethodImpl(32)]
			set
			{
				this._txtItemSlot == null;
				this._txtItemSlot = value;
				this._txtItemSlot == null;
			}
		}

		internal virtual TextBox txtOrderfile
		{
			get
			{
				return this._txtOrderfile;
			}
			[MethodImpl(32)]
			set
			{
				this._txtOrderfile == null;
				this._txtOrderfile = value;
				this._txtOrderfile == null;
			}
		}

		internal virtual TextBox txtPopCurrentItem
		{
			get
			{
				return this._txtPopCurrentItem;
			}
			[MethodImpl(32)]
			set
			{
				this._txtPopCurrentItem == null;
				this._txtPopCurrentItem = value;
				this._txtPopCurrentItem == null;
			}
		}

		internal virtual TextBox txtQualNum
		{
			get
			{
				return this._txtQualNum;
			}
			[MethodImpl(32)]
			set
			{
				if (this._txtQualNum != null)
				{
					frmMain _frmMain = this;
					this._txtQualNum.remove_TextChanged(new EventHandler(_frmMain, _frmMain.txtQualNum_TextChanged));
				}
				this._txtQualNum = value;
				if (this._txtQualNum != null)
				{
					frmMain _frmMain1 = this;
					this._txtQualNum.add_TextChanged(new EventHandler(_frmMain1, _frmMain1.txtQualNum_TextChanged));
				}
			}
		}

		internal virtual TextBox txtRegKey
		{
			get
			{
				return this._txtRegKey;
			}
			[MethodImpl(32)]
			set
			{
				this._txtRegKey == null;
				this._txtRegKey = value;
				this._txtRegKey == null;
			}
		}

		internal virtual TextBox TxtSkillfilepath
		{
			get
			{
				return this._TxtSkillfilepath;
			}
			[MethodImpl(32)]
			set
			{
				this._TxtSkillfilepath == null;
				this._TxtSkillfilepath = value;
				this._TxtSkillfilepath == null;
			}
		}

		internal virtual TextBox txtVendorName
		{
			get
			{
				return this._txtVendorName;
			}
			[MethodImpl(32)]
			set
			{
				this._txtVendorName == null;
				this._txtVendorName = value;
				this._txtVendorName == null;
			}
		}

		internal virtual TextBox txtVenLoc
		{
			get
			{
				return this._txtVenLoc;
			}
			[MethodImpl(32)]
			set
			{
				this._txtVenLoc == null;
				this._txtVenLoc = value;
				this._txtVenLoc == null;
			}
		}

		internal virtual TextBox txtWPtxt0
		{
			get
			{
				return this._txtWPtxt0;
			}
			[MethodImpl(32)]
			set
			{
				this._txtWPtxt0 == null;
				this._txtWPtxt0 = value;
				this._txtWPtxt0 == null;
			}
		}

		internal virtual ListBox WPs
		{
			get
			{
				return this._WPs;
			}
			[MethodImpl(32)]
			set
			{
				if (this._WPs != null)
				{
					frmMain _frmMain = this;
					this._WPs.remove_SelectedIndexChanged(new EventHandler(_frmMain, _frmMain.WPs_SelectedIndexChanged));
					frmMain _frmMain1 = this;
					this._WPs.remove_DoubleClick(new EventHandler(_frmMain1, _frmMain1.WPs_DoubleClick));
				}
				this._WPs = value;
				if (this._WPs != null)
				{
					frmMain _frmMain2 = this;
					this._WPs.add_SelectedIndexChanged(new EventHandler(_frmMain2, _frmMain2.WPs_SelectedIndexChanged));
					frmMain _frmMain3 = this;
					this._WPs.add_DoubleClick(new EventHandler(_frmMain3, _frmMain3.WPs_DoubleClick));
				}
			}
		}

		public frmMain()
		{
			frmMain _frmMain = this;
			base.add_Load(new EventHandler(_frmMain, _frmMain.frmMain_Load));
			frmMain _frmMain1 = this;
			base.add_Closing(new CancelEventHandler(_frmMain1, _frmMain1.frmMain_Closing));
			this.S0 = "You are missing";
			this.S1 = "(?<PlayerName>[A-Za-z]*) says,\\s(?<Message>[^\\n\\r]*)";
			this.S2 = "(?<PlayerName>[A-Za-z]*) sends,\\s(?<Message>[^\\n\\r]*)";
			this.S3 = "You successfully make (the |)(?<item>.*)!\\s\\((?<quality>[0-9]*)\\)";
			this.S4 = "You fail to make";
			this.S5 = "You must talk to your Order Trainer to raise";
			this.S6 = "Your product is cancelled";
			this.S7 = "You salvage";
			this.S8 = "You gain skill in (?<craft>[a-zA-Z\\s]*)!\\s\\((?<skill>[0-9]*)\\)";
			this.S9 = "You move and cancel the product you were making";
			this.S10 = "You are no longer making";
			this.S11 = "You successfully make (the |)(?<item>.*)!  \\((?<quality>[0-9]*)\\)";
			this.folder = Application.get_StartupPath();
			this.sd = new ArrayList();
			this.alOrder = new ArrayList();
			this.mWaypoints = new ArrayList();
			this.InitializeComponent();
		}

		private void Alarm()
		{
			while (this.Beeping)
			{
				try
				{
					MainMod.AK.Sound(1000, 1000);
					Thread.Sleep(1000);
				}
				catch (Exception exception)
				{
					ProjectData.SetProjectError(exception);
					ProjectData.ClearProjectError();
				}
			}
		}

		private void btnAddForgeLoc_Click(object sender, EventArgs e)
		{
			this.GetXY();
			this.txtForgeLoc.set_Text(string.Concat(StringType.FromInteger(this.PlayerX), ",", StringType.FromInteger(this.PlayerY)));
		}

		private void btnAddVenLoc_Click(object sender, EventArgs e)
		{
			try
			{
				MainMod.AK = new clsAutoKillerScript();
				clsAutoKillerScript aK = MainMod.AK;
				if (this.rbEuro.get_Checked())
				{
					aK.set_EnableEuro(true);
				}
				if (this.rbToA.get_Checked())
				{
					aK.set_EnableToA(true);
				}
				if (this.rbCatacombs.get_Checked())
				{
					aK.set_EnableCatacombs(true);
				}
				if (this.rbDarkness.get_Checked())
				{
					aK.set_EnableDarknessRising(true);
				}
				aK.set_RegKey(this.txtRegKey.get_Text());
				aK.DoInit();
				this.txtVenLoc.set_Text(string.Concat(StringType.FromInteger(aK.get_MobXCoord(aK.get_TargetIndex())), ",", StringType.FromInteger(aK.get_MobYCoord(aK.get_TargetIndex()))));
				aK.StopInit();
				aK = null;
				MainMod.AK = null;
			}
			catch (Exception exception)
			{
				ProjectData.SetProjectError(exception);
				ProjectData.ClearProjectError();
			}
		}

		private void btnAddVenName_Click(object sender, EventArgs e)
		{
			this.txtVendorName.set_Text(this.GetMobName());
		}

		private void btnCreateXML_Click(object sender, EventArgs e)
		{
			this.SaveStrings();
		}

		private void btnCurrentItem_Click(object sender, EventArgs e)
		{
			try
			{
				MainMod.AK = new clsAutoKillerScript();
				clsAutoKillerScript aK = MainMod.AK;
				if (this.rbEuro.get_Checked())
				{
					aK.set_EnableEuro(true);
				}
				if (this.rbToA.get_Checked())
				{
					aK.set_EnableToA(true);
				}
				if (this.rbCatacombs.get_Checked())
				{
					aK.set_EnableCatacombs(true);
				}
				if (this.rbDarkness.get_Checked())
				{
					aK.set_EnableDarknessRising(true);
				}
				aK.set_RegKey(this.txtRegKey.get_Text());
				aK.DoInit();
				this.txtItem.set_Text(aK.get_InvName(ByteType.FromString(this.txtPopCurrentItem.get_Text())));
				aK.StopInit();
				aK = null;
				MainMod.AK = null;
			}
			catch (Exception exception)
			{
				ProjectData.SetProjectError(exception);
				ProjectData.ClearProjectError();
			}
		}

		private void btnOrderPath_Click(object sender, EventArgs e)
		{
			OpenFileDialog openFileDialog = new OpenFileDialog();
			openFileDialog.set_FileName("*.txt");
			openFileDialog.set_CheckFileExists(true);
			openFileDialog.set_CheckPathExists(true);
			openFileDialog.set_Title("Select Order File");
			try
			{
				if (openFileDialog.ShowDialog() == 1)
				{
					this.txtOrderfile.set_Text(openFileDialog.get_FileName());
				}
			}
			catch (Exception exception)
			{
				ProjectData.SetProjectError(exception);
				MessageBox.Show(exception.get_Message());
				ProjectData.ClearProjectError();
			}
		}

		private void btnReadOrder_Click(object sender, EventArgs e)
		{
			if (StringType.StrCmp(this.txtOrderfile.get_Text(), null, false) != 0)
			{
				this.ReadOrderFile(this.txtOrderfile.get_Text());
			}
			else
			{
				MessageBox.Show("You must enter a file name");
			}
		}

		private void btnReadSkillfile_Click(object sender, EventArgs e)
		{
			this.ReadTextFile(this.TxtSkillfilepath.get_Text());
		}

		private void btnSave_Click(object sender, EventArgs e)
		{
			if (StringType.StrCmp(this.cbProfileName.get_Text(), null, false) != 0)
			{
				if (File.Exists(string.Concat(this.cbProfileName.get_Text(), ".acp")) && Interaction.MsgBox("The profile already exists, do you want to overwrite it?", 1, null) == 2)
				{
					return;
				}
				this.SaveSettings(this.cbProfileName.get_Text());
			}
		}

		private void btnSelectGamePath_Click(object sender, EventArgs e)
		{
			FolderBrowserDialog folderBrowserDialog = new FolderBrowserDialog();
			try
			{
				if (folderBrowserDialog.ShowDialog() == 1)
				{
					this.txtGamePath.set_Text(folderBrowserDialog.get_SelectedPath());
					this.GamePath = folderBrowserDialog.get_SelectedPath();
				}
			}
			catch (Exception exception)
			{
				ProjectData.SetProjectError(exception);
				MessageBox.Show(exception.get_Message());
				ProjectData.ClearProjectError();
			}
		}

		private void btnSelectPSkillFile_Click(object sender, EventArgs e)
		{
			OpenFileDialog openFileDialog = new OpenFileDialog();
			openFileDialog.set_FileName("*.txt");
			openFileDialog.set_CheckFileExists(true);
			openFileDialog.set_CheckPathExists(true);
			openFileDialog.set_Title("Select Power Skill File");
			try
			{
				if (openFileDialog.ShowDialog() == 1)
				{
					this.TxtSkillfilepath.set_Text(openFileDialog.get_FileName());
				}
			}
			catch (Exception exception)
			{
				ProjectData.SetProjectError(exception);
				MessageBox.Show(exception.get_Message());
				ProjectData.ClearProjectError();
			}
		}

		private void cbProfileName_SelectedIndexChanged(object sender, EventArgs e)
		{
			if (File.Exists(string.Concat(this.cbProfileName.get_Text(), ".acp")))
			{
				this.mWaypoints.Clear();
				this.WPs.get_Items().Clear();
				this.LoadSettings(this.cbProfileName.get_Text());
			}
		}

		private void cbQuality_SelectedIndexChanged(object sender, EventArgs e)
		{
			this.Quality = StringType.FromObject(this.cbQuality.get_SelectedItem());
		}

		private void chkSelling_CheckedChanged(object sender, EventArgs e)
		{
			if (!this.chkSelling.get_Checked())
			{
				this.DoSell = false;
			}
			else
			{
				this.DoSell = true;
			}
		}

		private void chkStock_CheckedChanged(object sender, EventArgs e)
		{
			if (!this.chkStock.get_Checked())
			{
				this.Stock = false;
				this.gbQual.set_Visible(false);
			}
			else
			{
				this.Stock = true;
				this.gbQual.set_Visible(true);
			}
		}

		private void chkStopQual_CheckedChanged(object sender, EventArgs e)
		{
			if (!this.chkStopQual.get_Checked())
			{
				this.StopQual = false;
			}
			else
			{
				this.StopQual = true;
			}
		}

		private void cmdAddWP0_Click(object sender, EventArgs e)
		{
			this.GetXY();
			this.txtWPtxt0.set_Text(string.Concat(StringType.FromInteger(this.PlayerX), ",", StringType.FromInteger(this.PlayerY)));
			this.WPs.get_Items().Add(this.txtWPtxt0.get_Text());
			this.mWaypoints.Add(this.txtWPtxt0.get_Text());
		}

		private void cmdQuit_Click(object sender, EventArgs e)
		{
			if (!Information.IsNothing(MainMod.AK))
			{
				MainMod.AK.StopInit();
				MainMod.AK = null;
			}
			this.EndThreads();
			this.StartAlarm();
			this.Close();
		}

		private void cmdStart_Click(object sender, EventArgs e)
		{
			if (MainMod.AK != null)
			{
				this.Running = false;
				MainMod.AK.StopInit();
				MainMod.AK = null;
				this.EndThreads();
				this.OkToCraft = false;
				this.Beeping = false;
				this.picStopped.set_Visible(true);
				this.picRunning.set_Visible(false);
				this.cmdStart.set_Text("Start");
			}
			else
			{
				this.Running = true;
				MainMod.AK = new clsAutoKillerScript();
				clsAutoKillerScript aK = MainMod.AK;
				frmMain _frmMain = this;
				aK.add_OnRegExTrue(new clsAutoKillerScript.OnRegExTrueEventHandler(_frmMain, _frmMain.Query));
				this.GamePath = this.txtGamePath.get_Text();
				aK.set_GamePath(this.GamePath);
				this.Keys = new UserKeys(MainMod.AK);
				aK.set_RegKey(this.txtRegKey.get_Text());
				aK.set_SetLeftTurnKey(this.Keys.get_TurnLeftKey());
				aK.set_SetRightTurnKey(this.Keys.get_TurnRightKey());
				aK.set_UseRegEx(true);
				if (this.rbEuro.get_Checked())
				{
					aK.set_EnableEuro(true);
				}
				if (this.rbToA.get_Checked())
				{
					aK.set_EnableToA(true);
				}
				if (this.rbCatacombs.get_Checked())
				{
					aK.set_EnableCatacombs(true);
				}
				if (this.rbDarkness.get_Checked())
				{
					aK.set_EnableDarknessRising(true);
				}
				this.LoadStrings();
				aK.AddString(0, this.S0);
				aK.AddString(1, this.S1);
				aK.AddString(2, this.S2);
				aK.AddString(3, this.S3);
				aK.AddString(4, this.S4);
				aK.AddString(5, this.S5);
				aK.AddString(6, this.S6);
				aK.AddString(7, this.S7);
				aK.AddString(8, this.S8);
				aK.AddString(9, this.S9);
				aK.AddString(10, this.S10);
				aK.AddString(11, this.S11);
				aK.DoInit();
				int gameProcess = aK.get_GameProcess();
				aK = null;
				Interaction.AppActivate(gameProcess);
				this.OkToCraft = true;
				this.picStopped.set_Visible(false);
				this.picRunning.set_Visible(true);
				bool flag = true;
				if (flag == this.chkSalvage.get_Checked())
				{
					frmMain _frmMain1 = this;
					this.tSalvage = new Thread(new ThreadStart(_frmMain1, _frmMain1.Salvage));
					this.tSalvage.Start();
				}
				else if (flag != this.chkOrder.get_Checked())
				{
					frmMain _frmMain2 = this;
					this.tCrafter = new Thread(new ThreadStart(_frmMain2, _frmMain2.Crafter));
					this.tCrafter.Start();
				}
				else
				{
					frmMain _frmMain3 = this;
					this.tOrder = new Thread(new ThreadStart(_frmMain3, _frmMain3.MakeOrder));
					this.tOrder.Start();
				}
				this.cmdStart.set_Text("Stop");
			}
		}

		private void Crafter()
		{
			short bag;
			ArrayList arrayList = new ArrayList();
			Sell sell = new Sell(MainMod.AK);
			this.mCraftKey = this.txtCraftKey.get_Text();
			while (this.Running)
			{
				try
				{
					if (this.OkToCraft)
					{
						MainMod.AK.SendString(this.mCraftKey);
						this.OkToCraft = false;
					}
					Thread.Sleep(50);
					if (this.DoSell && StringType.StrCmp(MainMod.AK.get_InvName(ByteType.FromString(this.txtItemSlot.get_Text())), this.txtItem.get_Text(), false) == 0)
					{
						if (StringType.StrCmp(this.txtItemSlot.get_Text(), null, false) != 0)
						{
							bag = this.GetBag((short)ByteType.FromString(this.txtItemSlot.get_Text()));
						}
						else
						{
							bag = 0;
						}
						this.RuntoVendor();
						MainMod.AK.SetTarget(this.txtVendorName.get_Text(), true);
						Thread.Sleep(2000);
						this.Keys.Stick(2);
						Thread.Sleep(2000);
						sell.Sell(bag, arrayList);
						Thread.Sleep(1000);
						this.Keys.MoveBackward(2);
						Thread.Sleep(1000);
						this.RuntoForge();
						this.OkToCraft = true;
						Thread.Sleep(3000);
					}
					if (this.Stock && StringType.StrCmp(MainMod.AK.get_InvName(ByteType.FromString(this.txtItemSlot.get_Text())), this.txtItem.get_Text(), false) == 0)
					{
						if (!this.chkDisableSound.get_Checked())
						{
							this.StartAlarm();
						}
						break;
					}
				}
				catch (Exception exception)
				{
					ProjectData.SetProjectError(exception);
					ProjectData.ClearProjectError();
				}
			}
		}

		protected override void Dispose(bool disposing)
		{
			if (disposing && this.components != null)
			{
				this.components.Dispose();
			}
			base.Dispose(disposing);
		}

		private void EndThreads()
		{
			try
			{
				if (this.tCrafter.get_ThreadState() == 0 || this.tCrafter.get_ThreadState() == 32)
				{
					this.tCrafter.Abort();
					this.tCrafter = null;
				}
			}
			catch (Exception exception)
			{
				ProjectData.SetProjectError(exception);
				ProjectData.ClearProjectError();
			}
		}

		private void frmMain_Closing(object sender, CancelEventArgs e)
		{
			if (MainMod.AK != null)
			{
				MainMod.AK.StopInit();
				MainMod.AK = null;
			}
			this.Running = false;
			this.EndThreads();
			this.OkToCraft = false;
			this.Beeping = false;
			this.picStopped.set_Visible(true);
			this.picRunning.set_Visible(false);
			this.cmdStart.set_Text("Start");
			if (StringType.StrCmp(this.cbProfileName.get_Text(), null, false) != 0)
			{
				this.SaveSettings(this.cbProfileName.get_Text());
			}
		}

		private void frmMain_Load(object sender, EventArgs e)
		{
			(new CVersionChecker()).Check(App.AutoKillerCrafter);
			Directory.SetCurrentDirectory(Application.get_StartupPath());
			this.UpdateProfiles();
			if (this.cbProfileName.get_Items().get_Count() > 0)
			{
				this.cbProfileName.set_SelectedIndex(0);
			}
		}

		private short GetBag(short Slot)
		{
			switch (Slot)
			{
				case 0:
				case 1:
				case 2:
				case 3:
				case 4:
				case 5:
				case 6:
				case 7:
				{
					return 0;
				}
				case 8:
				case 9:
				case 10:
				case 11:
				case 12:
				case 13:
				case 14:
				case 15:
				{
					return 1;
				}
				case 16:
				case 17:
				case 18:
				case 19:
				case 20:
				case 21:
				case 22:
				case 23:
				{
					return 2;
				}
				case 24:
				case 25:
				case 26:
				case 27:
				case 28:
				case 29:
				case 30:
				case 31:
				{
					return 3;
				}
				case 32:
				case 33:
				case 34:
				case 35:
				case 36:
				case 37:
				case 38:
				case 39:
				{
					return 4;
				}
			}
			return 4;
		}

		private string GetMobName()
		{
			string mobName = null;
			try
			{
				MainMod.AK = new clsAutoKillerScript();
				clsAutoKillerScript aK = MainMod.AK;
				if (this.rbEuro.get_Checked())
				{
					aK.set_EnableEuro(true);
				}
				if (this.rbToA.get_Checked())
				{
					aK.set_EnableToA(true);
				}
				if (this.rbCatacombs.get_Checked())
				{
					aK.set_EnableCatacombs(true);
				}
				if (this.rbDarkness.get_Checked())
				{
					aK.set_EnableDarknessRising(true);
				}
				aK.set_RegKey(this.txtRegKey.get_Text());
				aK.DoInit();
				mobName = aK.get_MobName(aK.get_TargetIndex());
				aK.StopInit();
				aK = null;
				MainMod.AK = null;
			}
			catch (Exception exception)
			{
				ProjectData.SetProjectError(exception);
				ProjectData.ClearProjectError();
			}
			return mobName;
		}

		private void GetXY()
		{
			try
			{
				MainMod.AK = new clsAutoKillerScript();
				clsAutoKillerScript aK = MainMod.AK;
				if (this.rbEuro.get_Checked())
				{
					aK.set_EnableEuro(true);
				}
				if (this.rbToA.get_Checked())
				{
					aK.set_EnableToA(true);
				}
				if (this.rbCatacombs.get_Checked())
				{
					aK.set_EnableCatacombs(true);
				}
				if (this.rbDarkness.get_Checked())
				{
					aK.set_EnableDarknessRising(true);
				}
				aK.set_RegKey(this.txtRegKey.get_Text());
				aK.DoInit();
				this.PlayerX = aK.get_gPlayerXCoord();
				this.PlayerY = aK.get_gPlayerYCoord();
				aK.StopInit();
				aK = null;
				MainMod.AK = null;
			}
			catch (Exception exception)
			{
				ProjectData.SetProjectError(exception);
				ProjectData.ClearProjectError();
			}
		}

		private void gGotoXY(double x, double y, double Distance)
		{
			MainMod.AK.TurnToHeading(MainMod.AK.FindHeading((double)MainMod.AK.get_gPlayerXCoord(), (double)MainMod.AK.get_gPlayerYCoord(), x, y));
			Thread.Sleep(250);
			MainMod.AK.StartRunning();
			while (MainMod.AK.ZDistance((double)MainMod.AK.get_gPlayerXCoord(), (double)MainMod.AK.get_gPlayerYCoord(), (double)MainMod.AK.get_gPlayerZCoord(), x, y, (double)MainMod.AK.get_gPlayerZCoord()) >= Distance)
			{
				MainMod.AK.TurnToHeading(MainMod.AK.FindHeading((double)MainMod.AK.get_gPlayerXCoord(), (double)MainMod.AK.get_gPlayerYCoord(), x, y));
				Thread.Sleep(50);
			}
			MainMod.AK.StopRunning();
		}

		[DebuggerStepThrough]
		private void InitializeComponent()
		{
			ResourceManager resourceManager = new ResourceManager(typeof(frmMain));
			this.TabControl1 = new TabControl();
			this.TabPage1 = new TabPage();
			this.GroupBox1 = new GroupBox();
			this.lblItem100Quality = new Label();
			this.lblItem99Quality = new Label();
			this.lblItem98Quality = new Label();
			this.lblItem97Quality = new Label();
			this.lblItem96Quality = new Label();
			this.lblItem95Quality = new Label();
			this.lblItem94Quality = new Label();
			this.Label53 = new Label();
			this.Label54 = new Label();
			this.Label52 = new Label();
			this.Label51 = new Label();
			this.Label50 = new Label();
			this.Label2 = new Label();
			this.Label1 = new Label();
			this.cmdQuit = new Button();
			this.cmdStart = new Button();
			this.picStopped = new PictureBox();
			this.picRunning = new PictureBox();
			this.TabPage2 = new TabPage();
			this.chkDisableSound = new CheckBox();
			this.GroupBox10 = new GroupBox();
			this.GroupBox4 = new GroupBox();
			this.rbCatacombs = new RadioButton();
			this.rbToA = new RadioButton();
			this.rbEuro = new RadioButton();
			this.rbUS = new RadioButton();
			this.btnCreateXML = new Button();
			this.chkKillProcess = new CheckBox();
			this.txtGamePath = new TextBox();
			this.btnSelectGamePath = new Button();
			this.Label10 = new Label();
			this.txtRegKey = new TextBox();
			this.Label5 = new Label();
			this.TabPage3 = new TabPage();
			this.TabControl2 = new TabControl();
			this.TabPage4 = new TabPage();
			this.txtPopCurrentItem = new TextBox();
			this.btnCurrentItem = new Button();
			this.Label12 = new Label();
			this.Label9 = new Label();
			this.txtCraftKey = new TextBox();
			this.btnAddVenName = new Button();
			this.btnAddVenLoc = new Button();
			this.btnAddForgeLoc = new Button();
			this.txtVenLoc = new TextBox();
			this.Label11 = new Label();
			this.txtForgeLoc = new TextBox();
			this.Label6 = new Label();
			this.chkStock = new CheckBox();
			this.chkSalvage = new CheckBox();
			this.txtVendorName = new TextBox();
			this.Label7 = new Label();
			this.txtItem = new TextBox();
			this.chkSelling = new CheckBox();
			this.chkQuit = new CheckBox();
			this.Label8 = new Label();
			this.txtItemSlot = new TextBox();
			this.Label3 = new Label();
			this.txtDistance = new TextBox();
			this.gbQual = new GroupBox();
			this.cbQuality = new ComboBox();
			this.Label14 = new Label();
			this.Label13 = new Label();
			this.txtQualNum = new TextBox();
			this.chkStopQual = new CheckBox();
			this.TabPage5 = new TabPage();
			this.btnSelectPSkillFile = new Button();
			this.Label16 = new Label();
			this.btnReadSkillfile = new Button();
			this.ListView1 = new ListView();
			this.ColumnHeader1 = new ColumnHeader();
			this.ColumnHeader2 = new ColumnHeader();
			this.ColumnHeader3 = new ColumnHeader();
			this.cbTradeskill = new ComboBox();
			this.chkPowerskill = new CheckBox();
			this.Label15 = new Label();
			this.TxtSkillfilepath = new TextBox();
			this.TabPage6 = new TabPage();
			this.btnOrderPath = new Button();
			this.Label18 = new Label();
			this.btnReadOrder = new Button();
			this.lvOrder = new ListView();
			this.ColumnHeader10 = new ColumnHeader();
			this.ColumnHeader12 = new ColumnHeader();
			this.ColumnHeader11 = new ColumnHeader();
			this.Label17 = new Label();
			this.txtOrderfile = new TextBox();
			this.chkOrder = new CheckBox();
			this.TabPage7 = new TabPage();
			this.GroupBox2 = new GroupBox();
			this.WPs = new ListBox();
			this.cmdAddWP0 = new Button();
			this.txtWPtxt0 = new TextBox();
			this.Label41 = new Label();
			this.TabPage8 = new TabPage();
			this.GroupBox3 = new GroupBox();
			this.btnSave = new Button();
			this.Label19 = new Label();
			this.cbProfileName = new ComboBox();
			this.SaveFileDialog1 = new SaveFileDialog();
			this.OpenFileDialog1 = new OpenFileDialog();
			this.ColumnHeader5 = new ColumnHeader();
			this.ColumnHeader6 = new ColumnHeader();
			this.ColumnHeader7 = new ColumnHeader();
			this.ColumnHeader8 = new ColumnHeader();
			this.rbDarkness = new RadioButton();
			this.TabControl1.SuspendLayout();
			this.TabPage1.SuspendLayout();
			this.GroupBox1.SuspendLayout();
			this.TabPage2.SuspendLayout();
			this.GroupBox10.SuspendLayout();
			this.GroupBox4.SuspendLayout();
			this.TabPage3.SuspendLayout();
			this.TabControl2.SuspendLayout();
			this.TabPage4.SuspendLayout();
			this.gbQual.SuspendLayout();
			this.TabPage5.SuspendLayout();
			this.TabPage6.SuspendLayout();
			this.TabPage7.SuspendLayout();
			this.GroupBox2.SuspendLayout();
			this.TabPage8.SuspendLayout();
			this.GroupBox3.SuspendLayout();
			this.SuspendLayout();
			this.TabControl1.get_Controls().Add(this.TabPage1);
			this.TabControl1.get_Controls().Add(this.TabPage2);
			this.TabControl1.get_Controls().Add(this.TabPage3);
			this.TabControl1.get_Controls().Add(this.TabPage7);
			this.TabControl1.get_Controls().Add(this.TabPage8);
			TabControl tabControl1 = this.TabControl1;
			Point point = new Point(0, 0);
			tabControl1.set_Location(point);
			this.TabControl1.set_Name("TabControl1");
			this.TabControl1.set_SelectedIndex(0);
			TabControl tabControl = this.TabControl1;
			Size size = new Size(360, 272);
			tabControl.set_Size(size);
			this.TabControl1.set_TabIndex(34);
			this.TabPage1.set_BackColor(Color.get_Black());
			this.TabPage1.get_Controls().Add(this.GroupBox1);
			this.TabPage1.get_Controls().Add(this.cmdQuit);
			this.TabPage1.get_Controls().Add(this.cmdStart);
			this.TabPage1.get_Controls().Add(this.picStopped);
			this.TabPage1.get_Controls().Add(this.picRunning);
			TabPage tabPage1 = this.TabPage1;
			point = new Point(4, 22);
			tabPage1.set_Location(point);
			this.TabPage1.set_Name("TabPage1");
			TabPage tabPage = this.TabPage1;
			size = new Size(352, 246);
			tabPage.set_Size(size);
			this.TabPage1.set_TabIndex(0);
			this.TabPage1.set_Text("Main");
			this.GroupBox1.get_Controls().Add(this.lblItem100Quality);
			this.GroupBox1.get_Controls().Add(this.lblItem99Quality);
			this.GroupBox1.get_Controls().Add(this.lblItem98Quality);
			this.GroupBox1.get_Controls().Add(this.lblItem97Quality);
			this.GroupBox1.get_Controls().Add(this.lblItem96Quality);
			this.GroupBox1.get_Controls().Add(this.lblItem95Quality);
			this.GroupBox1.get_Controls().Add(this.lblItem94Quality);
			this.GroupBox1.get_Controls().Add(this.Label53);
			this.GroupBox1.get_Controls().Add(this.Label54);
			this.GroupBox1.get_Controls().Add(this.Label52);
			this.GroupBox1.get_Controls().Add(this.Label51);
			this.GroupBox1.get_Controls().Add(this.Label50);
			this.GroupBox1.get_Controls().Add(this.Label2);
			this.GroupBox1.get_Controls().Add(this.Label1);
			GroupBox groupBox1 = this.GroupBox1;
			point = new Point(48, 8);
			groupBox1.set_Location(point);
			this.GroupBox1.set_Name("GroupBox1");
			GroupBox groupBox = this.GroupBox1;
			size = new Size(264, 96);
			groupBox.set_Size(size);
			this.GroupBox1.set_TabIndex(50);
			this.GroupBox1.set_TabStop(false);
			this.lblItem100Quality.set_ForeColor(Color.get_LawnGreen());
			Label label = this.lblItem100Quality;
			point = new Point(160, 72);
			label.set_Location(point);
			this.lblItem100Quality.set_Name("lblItem100Quality");
			Label label1 = this.lblItem100Quality;
			size = new Size(32, 16);
			label1.set_Size(size);
			this.lblItem100Quality.set_TabIndex(13);
			this.lblItem99Quality.set_ForeColor(Color.get_LawnGreen());
			Label label2 = this.lblItem99Quality;
			point = new Point(224, 48);
			label2.set_Location(point);
			this.lblItem99Quality.set_Name("lblItem99Quality");
			Label label3 = this.lblItem99Quality;
			size = new Size(32, 16);
			label3.set_Size(size);
			this.lblItem99Quality.set_TabIndex(12);
			this.lblItem98Quality.set_ForeColor(Color.get_LawnGreen());
			Label label4 = this.lblItem98Quality;
			point = new Point(96, 48);
			label4.set_Location(point);
			this.lblItem98Quality.set_Name("lblItem98Quality");
			Label label5 = this.lblItem98Quality;
			size = new Size(32, 16);
			label5.set_Size(size);
			this.lblItem98Quality.set_TabIndex(11);
			this.lblItem97Quality.set_ForeColor(Color.get_LawnGreen());
			Label label6 = this.lblItem97Quality;
			point = new Point(224, 32);
			label6.set_Location(point);
			this.lblItem97Quality.set_Name("lblItem97Quality");
			Label label7 = this.lblItem97Quality;
			size = new Size(32, 16);
			label7.set_Size(size);
			this.lblItem97Quality.set_TabIndex(10);
			this.lblItem96Quality.set_ForeColor(Color.get_LawnGreen());
			Label label8 = this.lblItem96Quality;
			point = new Point(96, 32);
			label8.set_Location(point);
			this.lblItem96Quality.set_Name("lblItem96Quality");
			Label label9 = this.lblItem96Quality;
			size = new Size(32, 16);
			label9.set_Size(size);
			this.lblItem96Quality.set_TabIndex(9);
			this.lblItem95Quality.set_ForeColor(Color.get_LawnGreen());
			Label label10 = this.lblItem95Quality;
			point = new Point(224, 16);
			label10.set_Location(point);
			this.lblItem95Quality.set_Name("lblItem95Quality");
			Label label11 = this.lblItem95Quality;
			size = new Size(32, 16);
			label11.set_Size(size);
			this.lblItem95Quality.set_TabIndex(8);
			this.lblItem94Quality.set_ForeColor(Color.get_LawnGreen());
			Label label12 = this.lblItem94Quality;
			point = new Point(96, 16);
			label12.set_Location(point);
			this.lblItem94Quality.set_Name("lblItem94Quality");
			Label label13 = this.lblItem94Quality;
			size = new Size(32, 16);
			label13.set_Size(size);
			this.lblItem94Quality.set_TabIndex(7);
			this.Label53.set_ForeColor(Color.get_LawnGreen());
			Label label53 = this.Label53;
			point = new Point(136, 48);
			label53.set_Location(point);
			this.Label53.set_Name("Label53");
			Label label531 = this.Label53;
			size = new Size(88, 16);
			label531.set_Size(size);
			this.Label53.set_TabIndex(6);
			this.Label53.set_Text("Item 99 Quality:");
			this.Label54.set_ForeColor(Color.get_LawnGreen());
			Label label54 = this.Label54;
			point = new Point(72, 72);
			label54.set_Location(point);
			this.Label54.set_Name("Label54");
			Label label541 = this.Label54;
			size = new Size(96, 16);
			label541.set_Size(size);
			this.Label54.set_TabIndex(5);
			this.Label54.set_Text("Item 100 Quality:");
			this.Label52.set_ForeColor(Color.get_LawnGreen());
			Label label52 = this.Label52;
			point = new Point(8, 48);
			label52.set_Location(point);
			this.Label52.set_Name("Label52");
			Label label521 = this.Label52;
			size = new Size(88, 16);
			label521.set_Size(size);
			this.Label52.set_TabIndex(4);
			this.Label52.set_Text("Item 98 Quality:");
			this.Label51.set_ForeColor(Color.get_LawnGreen());
			Label label51 = this.Label51;
			point = new Point(136, 32);
			label51.set_Location(point);
			this.Label51.set_Name("Label51");
			Label label511 = this.Label51;
			size = new Size(88, 16);
			label511.set_Size(size);
			this.Label51.set_TabIndex(3);
			this.Label51.set_Text("Item 97 Quality:");
			this.Label50.set_ForeColor(Color.get_LawnGreen());
			Label label50 = this.Label50;
			point = new Point(8, 32);
			label50.set_Location(point);
			this.Label50.set_Name("Label50");
			Label label501 = this.Label50;
			size = new Size(88, 16);
			label501.set_Size(size);
			this.Label50.set_TabIndex(2);
			this.Label50.set_Text("Item 96 Quality:");
			this.Label2.set_ForeColor(Color.get_LawnGreen());
			Label label21 = this.Label2;
			point = new Point(136, 16);
			label21.set_Location(point);
			this.Label2.set_Name("Label2");
			Label label22 = this.Label2;
			size = new Size(88, 16);
			label22.set_Size(size);
			this.Label2.set_TabIndex(1);
			this.Label2.set_Text("Item 95 Quality:");
			this.Label1.set_ForeColor(Color.get_LawnGreen());
			Label label14 = this.Label1;
			point = new Point(8, 16);
			label14.set_Location(point);
			this.Label1.set_Name("Label1");
			Label label15 = this.Label1;
			size = new Size(88, 16);
			label15.set_Size(size);
			this.Label1.set_TabIndex(0);
			this.Label1.set_Text("Item 94 Quality:");
			this.cmdQuit.set_FlatStyle(3);
			Button button = this.cmdQuit;
			point = new Point(80, 216);
			button.set_Location(point);
			this.cmdQuit.set_Name("cmdQuit");
			Button button1 = this.cmdQuit;
			size = new Size(64, 24);
			button1.set_Size(size);
			this.cmdQuit.set_TabIndex(49);
			this.cmdQuit.set_Text("Exit");
			this.cmdStart.set_BackColor(SystemColors.get_Control());
			this.cmdStart.set_Cursor(Cursors.get_Default());
			this.cmdStart.set_FlatStyle(3);
			this.cmdStart.set_Font(new Font("Arial", 8f, 0, 3, 0));
			this.cmdStart.set_ForeColor(SystemColors.get_ControlText());
			Button button2 = this.cmdStart;
			point = new Point(8, 216);
			button2.set_Location(point);
			this.cmdStart.set_Name("cmdStart");
			this.cmdStart.set_RightToLeft(0);
			Button button3 = this.cmdStart;
			size = new Size(64, 24);
			button3.set_Size(size);
			this.cmdStart.set_TabIndex(35);
			this.cmdStart.set_Text("Start");
			this.picStopped.set_Image((Image)resourceManager.GetObject("picStopped.Image"));
			PictureBox pictureBox = this.picStopped;
			point = new Point(0, 8);
			pictureBox.set_Location(point);
			this.picStopped.set_Name("picStopped");
			PictureBox pictureBox1 = this.picStopped;
			size = new Size(24, 16);
			pictureBox1.set_Size(size);
			this.picStopped.set_TabIndex(23);
			this.picStopped.set_TabStop(false);
			this.picRunning.set_Image((Image)resourceManager.GetObject("picRunning.Image"));
			PictureBox pictureBox2 = this.picRunning;
			point = new Point(0, 8);
			pictureBox2.set_Location(point);
			this.picRunning.set_Name("picRunning");
			PictureBox pictureBox3 = this.picRunning;
			size = new Size(16, 16);
			pictureBox3.set_Size(size);
			this.picRunning.set_TabIndex(22);
			this.picRunning.set_TabStop(false);
			this.picRunning.set_Visible(false);
			this.TabPage2.get_Controls().Add(this.chkDisableSound);
			this.TabPage2.get_Controls().Add(this.GroupBox10);
			this.TabPage2.get_Controls().Add(this.btnCreateXML);
			this.TabPage2.get_Controls().Add(this.chkKillProcess);
			this.TabPage2.get_Controls().Add(this.txtGamePath);
			this.TabPage2.get_Controls().Add(this.btnSelectGamePath);
			this.TabPage2.get_Controls().Add(this.Label10);
			this.TabPage2.get_Controls().Add(this.txtRegKey);
			this.TabPage2.get_Controls().Add(this.Label5);
			TabPage tabPage2 = this.TabPage2;
			point = new Point(4, 22);
			tabPage2.set_Location(point);
			this.TabPage2.set_Name("TabPage2");
			TabPage tabPage21 = this.TabPage2;
			size = new Size(352, 246);
			tabPage21.set_Size(size);
			this.TabPage2.set_TabIndex(1);
			this.TabPage2.set_Text("Settings");
			this.chkDisableSound.set_FlatStyle(3);
			CheckBox checkBox = this.chkDisableSound;
			point = new Point(232, 88);
			checkBox.set_Location(point);
			this.chkDisableSound.set_Name("chkDisableSound");
			this.chkDisableSound.set_TabIndex(77);
			this.chkDisableSound.set_Text("Disable Sound");
			this.GroupBox10.get_Controls().Add(this.GroupBox4);
			this.GroupBox10.get_Controls().Add(this.rbEuro);
			this.GroupBox10.get_Controls().Add(this.rbUS);
			GroupBox groupBox10 = this.GroupBox10;
			point = new Point(8, 64);
			groupBox10.set_Location(point);
			this.GroupBox10.set_Name("GroupBox10");
			GroupBox groupBox101 = this.GroupBox10;
			size = new Size(216, 176);
			groupBox101.set_Size(size);
			this.GroupBox10.set_TabIndex(76);
			this.GroupBox10.set_TabStop(false);
			this.GroupBox4.get_Controls().Add(this.rbDarkness);
			this.GroupBox4.get_Controls().Add(this.rbCatacombs);
			this.GroupBox4.get_Controls().Add(this.rbToA);
			GroupBox groupBox4 = this.GroupBox4;
			point = new Point(8, 40);
			groupBox4.set_Location(point);
			this.GroupBox4.set_Name("GroupBox4");
			GroupBox groupBox41 = this.GroupBox4;
			size = new Size(200, 104);
			groupBox41.set_Size(size);
			this.GroupBox4.set_TabIndex(46);
			this.GroupBox4.set_TabStop(false);
			this.GroupBox4.set_Text("Client Info");
			this.rbCatacombs.set_FlatStyle(3);
			RadioButton radioButton = this.rbCatacombs;
			point = new Point(72, 24);
			radioButton.set_Location(point);
			this.rbCatacombs.set_Name("rbCatacombs");
			this.rbCatacombs.set_TabIndex(4);
			this.rbCatacombs.set_Text("Catacombs");
			this.rbToA.set_Checked(true);
			this.rbToA.set_FlatStyle(3);
			RadioButton radioButton1 = this.rbToA;
			point = new Point(16, 24);
			radioButton1.set_Location(point);
			this.rbToA.set_Name("rbToA");
			RadioButton radioButton2 = this.rbToA;
			size = new Size(56, 24);
			radioButton2.set_Size(size);
			this.rbToA.set_TabIndex(2);
			this.rbToA.set_TabStop(true);
			this.rbToA.set_Text("ToA");
			this.rbEuro.set_FlatStyle(3);
			RadioButton radioButton3 = this.rbEuro;
			point = new Point(72, 16);
			radioButton3.set_Location(point);
			this.rbEuro.set_Name("rbEuro");
			RadioButton radioButton4 = this.rbEuro;
			size = new Size(64, 24);
			radioButton4.set_Size(size);
			this.rbEuro.set_TabIndex(1);
			this.rbEuro.set_Text("Euro");
			this.rbUS.set_Checked(true);
			this.rbUS.set_FlatStyle(3);
			RadioButton radioButton5 = this.rbUS;
			point = new Point(16, 16);
			radioButton5.set_Location(point);
			this.rbUS.set_Name("rbUS");
			RadioButton radioButton6 = this.rbUS;
			size = new Size(48, 24);
			radioButton6.set_Size(size);
			this.rbUS.set_TabIndex(0);
			this.rbUS.set_TabStop(true);
			this.rbUS.set_Text("US");
			this.btnCreateXML.set_FlatStyle(3);
			this.btnCreateXML.set_Font(new Font("Arial", 6.75f));
			Button button4 = this.btnCreateXML;
			point = new Point(240, 216);
			button4.set_Location(point);
			this.btnCreateXML.set_Name("btnCreateXML");
			Button button5 = this.btnCreateXML;
			size = new Size(96, 20);
			button5.set_Size(size);
			this.btnCreateXML.set_TabIndex(75);
			this.btnCreateXML.set_Text("Create String XML");
			this.chkKillProcess.set_FlatStyle(3);
			CheckBox checkBox1 = this.chkKillProcess;
			point = new Point(232, 64);
			checkBox1.set_Location(point);
			this.chkKillProcess.set_Name("chkKillProcess");
			CheckBox checkBox2 = this.chkKillProcess;
			size = new Size(88, 24);
			checkBox2.set_Size(size);
			this.chkKillProcess.set_TabIndex(74);
			this.chkKillProcess.set_Text("Kill Process");
			TextBox textBox = this.txtGamePath;
			point = new Point(88, 8);
			textBox.set_Location(point);
			this.txtGamePath.set_Name("txtGamePath");
			TextBox textBox1 = this.txtGamePath;
			size = new Size(128, 20);
			textBox1.set_Size(size);
			this.txtGamePath.set_TabIndex(66);
			this.txtGamePath.set_Text("C:\\Mythic\\Isles\\");
			this.btnSelectGamePath.set_FlatStyle(3);
			Button button6 = this.btnSelectGamePath;
			point = new Point(224, 8);
			button6.set_Location(point);
			this.btnSelectGamePath.set_Name("btnSelectGamePath");
			Button button7 = this.btnSelectGamePath;
			size = new Size(24, 20);
			button7.set_Size(size);
			this.btnSelectGamePath.set_TabIndex(65);
			this.btnSelectGamePath.set_Text("...");
			this.Label10.set_BackColor(SystemColors.get_Control());
			this.Label10.set_Cursor(Cursors.get_Default());
			this.Label10.set_Font(new Font("Arial", 8f, 0, 3, 0));
			this.Label10.set_ForeColor(SystemColors.get_ControlText());
			Label label101 = this.Label10;
			point = new Point(8, 8);
			label101.set_Location(point);
			this.Label10.set_Name("Label10");
			this.Label10.set_RightToLeft(0);
			Label label102 = this.Label10;
			size = new Size(88, 24);
			label102.set_Size(size);
			this.Label10.set_TabIndex(64);
			this.Label10.set_Text("Game Path:");
			TextBox textBox2 = this.txtRegKey;
			point = new Point(88, 40);
			textBox2.set_Location(point);
			this.txtRegKey.set_Name("txtRegKey");
			TextBox textBox3 = this.txtRegKey;
			size = new Size(128, 20);
			textBox3.set_Size(size);
			this.txtRegKey.set_TabIndex(46);
			this.txtRegKey.set_Text("");
			this.Label5.set_BackColor(SystemColors.get_Control());
			this.Label5.set_Cursor(Cursors.get_Default());
			this.Label5.set_Font(new Font("Arial", 8f, 0, 3, 0));
			this.Label5.set_ForeColor(SystemColors.get_ControlText());
			Label label55 = this.Label5;
			point = new Point(8, 40);
			label55.set_Location(point);
			this.Label5.set_Name("Label5");
			this.Label5.set_RightToLeft(0);
			Label label56 = this.Label5;
			size = new Size(73, 17);
			label56.set_Size(size);
			this.Label5.set_TabIndex(47);
			this.Label5.set_Text("Reg Key");
			this.TabPage3.get_Controls().Add(this.TabControl2);
			TabPage tabPage3 = this.TabPage3;
			point = new Point(4, 22);
			tabPage3.set_Location(point);
			this.TabPage3.set_Name("TabPage3");
			TabPage tabPage31 = this.TabPage3;
			size = new Size(352, 246);
			tabPage31.set_Size(size);
			this.TabPage3.set_TabIndex(2);
			this.TabPage3.set_Text("Crafting");
			this.TabControl2.set_Alignment(1);
			this.TabControl2.get_Controls().Add(this.TabPage4);
			this.TabControl2.get_Controls().Add(this.TabPage5);
			this.TabControl2.get_Controls().Add(this.TabPage6);
			this.TabControl2.set_Dock(5);
			TabControl tabControl2 = this.TabControl2;
			point = new Point(0, 0);
			tabControl2.set_Location(point);
			this.TabControl2.set_Multiline(true);
			this.TabControl2.set_Name("TabControl2");
			this.TabControl2.set_SelectedIndex(0);
			TabControl tabControl21 = this.TabControl2;
			size = new Size(352, 246);
			tabControl21.set_Size(size);
			this.TabControl2.set_TabIndex(93);
			this.TabPage4.get_Controls().Add(this.txtPopCurrentItem);
			this.TabPage4.get_Controls().Add(this.btnCurrentItem);
			this.TabPage4.get_Controls().Add(this.Label12);
			this.TabPage4.get_Controls().Add(this.Label9);
			this.TabPage4.get_Controls().Add(this.txtCraftKey);
			this.TabPage4.get_Controls().Add(this.btnAddVenName);
			this.TabPage4.get_Controls().Add(this.btnAddVenLoc);
			this.TabPage4.get_Controls().Add(this.btnAddForgeLoc);
			this.TabPage4.get_Controls().Add(this.txtVenLoc);
			this.TabPage4.get_Controls().Add(this.Label11);
			this.TabPage4.get_Controls().Add(this.txtForgeLoc);
			this.TabPage4.get_Controls().Add(this.Label6);
			this.TabPage4.get_Controls().Add(this.chkStock);
			this.TabPage4.get_Controls().Add(this.chkSalvage);
			this.TabPage4.get_Controls().Add(this.txtVendorName);
			this.TabPage4.get_Controls().Add(this.Label7);
			this.TabPage4.get_Controls().Add(this.txtItem);
			this.TabPage4.get_Controls().Add(this.chkSelling);
			this.TabPage4.get_Controls().Add(this.chkQuit);
			this.TabPage4.get_Controls().Add(this.Label8);
			this.TabPage4.get_Controls().Add(this.txtItemSlot);
			this.TabPage4.get_Controls().Add(this.Label3);
			this.TabPage4.get_Controls().Add(this.txtDistance);
			this.TabPage4.get_Controls().Add(this.gbQual);
			TabPage tabPage4 = this.TabPage4;
			point = new Point(4, 4);
			tabPage4.set_Location(point);
			this.TabPage4.set_Name("TabPage4");
			TabPage tabPage41 = this.TabPage4;
			size = new Size(344, 220);
			tabPage41.set_Size(size);
			this.TabPage4.set_TabIndex(0);
			this.TabPage4.set_Text("Options");
			TextBox textBox4 = this.txtPopCurrentItem;
			point = new Point(248, 152);
			textBox4.set_Location(point);
			this.txtPopCurrentItem.set_Name("txtPopCurrentItem");
			TextBox textBox5 = this.txtPopCurrentItem;
			size = new Size(24, 20);
			textBox5.set_Size(size);
			this.txtPopCurrentItem.set_TabIndex(96);
			this.txtPopCurrentItem.set_Text("");
			this.btnCurrentItem.set_FlatStyle(3);
			Button button8 = this.btnCurrentItem;
			point = new Point(216, 152);
			button8.set_Location(point);
			this.btnCurrentItem.set_Name("btnCurrentItem");
			Button button9 = this.btnCurrentItem;
			size = new Size(24, 20);
			button9.set_Size(size);
			this.btnCurrentItem.set_TabIndex(95);
			this.btnCurrentItem.set_Text("...");
			Label label121 = this.Label12;
			point = new Point(8, 128);
			label121.set_Location(point);
			this.Label12.set_Name("Label12");
			Label label122 = this.Label12;
			size = new Size(65, 16);
			label122.set_Size(size);
			this.Label12.set_TabIndex(94);
			this.Label12.set_Text("Vendor Loc");
			Label label91 = this.Label9;
			point = new Point(8, 176);
			label91.set_Location(point);
			this.Label9.set_Name("Label9");
			Label label92 = this.Label9;
			size = new Size(80, 16);
			label92.set_Size(size);
			this.Label9.set_TabIndex(93);
			this.Label9.set_Text("Vendor Name");
			TextBox textBox6 = this.txtCraftKey;
			point = new Point(72, 8);
			textBox6.set_Location(point);
			this.txtCraftKey.set_MaxLength(1);
			this.txtCraftKey.set_Name("txtCraftKey");
			TextBox textBox7 = this.txtCraftKey;
			size = new Size(24, 20);
			textBox7.set_Size(size);
			this.txtCraftKey.set_TabIndex(59);
			this.txtCraftKey.set_Text("");
			this.btnAddVenName.set_FlatStyle(3);
			Button button10 = this.btnAddVenName;
			point = new Point(216, 176);
			button10.set_Location(point);
			this.btnAddVenName.set_Name("btnAddVenName");
			Button button11 = this.btnAddVenName;
			size = new Size(24, 20);
			button11.set_Size(size);
			this.btnAddVenName.set_TabIndex(91);
			this.btnAddVenName.set_Text("...");
			this.btnAddVenLoc.set_FlatStyle(3);
			Button button12 = this.btnAddVenLoc;
			point = new Point(216, 128);
			button12.set_Location(point);
			this.btnAddVenLoc.set_Name("btnAddVenLoc");
			Button button13 = this.btnAddVenLoc;
			size = new Size(24, 20);
			button13.set_Size(size);
			this.btnAddVenLoc.set_TabIndex(90);
			this.btnAddVenLoc.set_Text("...");
			this.btnAddForgeLoc.set_FlatStyle(3);
			Button button14 = this.btnAddForgeLoc;
			point = new Point(216, 104);
			button14.set_Location(point);
			this.btnAddForgeLoc.set_Name("btnAddForgeLoc");
			Button button15 = this.btnAddForgeLoc;
			size = new Size(24, 20);
			button15.set_Size(size);
			this.btnAddForgeLoc.set_TabIndex(89);
			this.btnAddForgeLoc.set_Text("...");
			TextBox textBox8 = this.txtVenLoc;
			point = new Point(88, 128);
			textBox8.set_Location(point);
			this.txtVenLoc.set_Name("txtVenLoc");
			this.txtVenLoc.set_ReadOnly(true);
			TextBox textBox9 = this.txtVenLoc;
			size = new Size(128, 20);
			textBox9.set_Size(size);
			this.txtVenLoc.set_TabIndex(88);
			this.txtVenLoc.set_Text("");
			Label label111 = this.Label11;
			point = new Point(8, 104);
			label111.set_Location(point);
			this.Label11.set_Name("Label11");
			Label label112 = this.Label11;
			size = new Size(65, 16);
			label112.set_Size(size);
			this.Label11.set_TabIndex(86);
			this.Label11.set_Text("Forge Loc");
			TextBox textBox10 = this.txtForgeLoc;
			point = new Point(88, 104);
			textBox10.set_Location(point);
			this.txtForgeLoc.set_Name("txtForgeLoc");
			this.txtForgeLoc.set_ReadOnly(true);
			TextBox textBox11 = this.txtForgeLoc;
			size = new Size(128, 20);
			textBox11.set_Size(size);
			this.txtForgeLoc.set_TabIndex(85);
			this.txtForgeLoc.set_Text("");
			this.Label6.set_BackColor(SystemColors.get_Control());
			this.Label6.set_Cursor(Cursors.get_Default());
			this.Label6.set_Font(new Font("Arial", 8f, 0, 3, 0));
			this.Label6.set_ForeColor(SystemColors.get_ControlText());
			Label label61 = this.Label6;
			point = new Point(8, 56);
			label61.set_Location(point);
			this.Label6.set_Name("Label6");
			this.Label6.set_RightToLeft(0);
			Label label62 = this.Label6;
			size = new Size(49, 17);
			label62.set_Size(size);
			this.Label6.set_TabIndex(62);
			this.Label6.set_Text("Distance");
			this.chkStock.set_FlatStyle(3);
			CheckBox checkBox3 = this.chkStock;
			point = new Point(136, 40);
			checkBox3.set_Location(point);
			this.chkStock.set_Name("chkStock");
			CheckBox checkBox4 = this.chkStock;
			size = new Size(64, 16);
			checkBox4.set_Size(size);
			this.chkStock.set_TabIndex(84);
			this.chkStock.set_Text("Stock");
			this.chkSalvage.set_FlatStyle(3);
			CheckBox checkBox5 = this.chkSalvage;
			point = new Point(136, 56);
			checkBox5.set_Location(point);
			this.chkSalvage.set_Name("chkSalvage");
			CheckBox checkBox6 = this.chkSalvage;
			size = new Size(64, 16);
			checkBox6.set_Size(size);
			this.chkSalvage.set_TabIndex(83);
			this.chkSalvage.set_Text("Salvage");
			TextBox textBox12 = this.txtVendorName;
			point = new Point(88, 176);
			textBox12.set_Location(point);
			this.txtVendorName.set_Name("txtVendorName");
			this.txtVendorName.set_ReadOnly(true);
			TextBox textBox13 = this.txtVendorName;
			size = new Size(128, 20);
			textBox13.set_Size(size);
			this.txtVendorName.set_TabIndex(81);
			this.txtVendorName.set_Text("");
			this.Label7.set_BackColor(SystemColors.get_Control());
			this.Label7.set_Cursor(Cursors.get_Default());
			this.Label7.set_Font(new Font("Arial", 8f, 0, 3, 0));
			this.Label7.set_ForeColor(SystemColors.get_ControlText());
			Label label71 = this.Label7;
			point = new Point(8, 152);
			label71.set_Location(point);
			this.Label7.set_Name("Label7");
			this.Label7.set_RightToLeft(0);
			Label label72 = this.Label7;
			size = new Size(65, 17);
			label72.set_Size(size);
			this.Label7.set_TabIndex(80);
			this.Label7.set_Text("Current item");
			TextBox textBox14 = this.txtItem;
			point = new Point(88, 152);
			textBox14.set_Location(point);
			this.txtItem.set_Name("txtItem");
			TextBox textBox15 = this.txtItem;
			size = new Size(128, 20);
			textBox15.set_Size(size);
			this.txtItem.set_TabIndex(79);
			this.txtItem.set_Text("");
			this.chkSelling.set_FlatStyle(3);
			CheckBox checkBox7 = this.chkSelling;
			point = new Point(136, 24);
			checkBox7.set_Location(point);
			this.chkSelling.set_Name("chkSelling");
			CheckBox checkBox8 = this.chkSelling;
			size = new Size(64, 16);
			checkBox8.set_Size(size);
			this.chkSelling.set_TabIndex(78);
			this.chkSelling.set_Text("Selling");
			this.chkQuit.set_FlatStyle(3);
			CheckBox checkBox9 = this.chkQuit;
			point = new Point(136, 8);
			checkBox9.set_Location(point);
			this.chkQuit.set_Name("chkQuit");
			CheckBox checkBox10 = this.chkQuit;
			size = new Size(48, 17);
			checkBox10.set_Size(size);
			this.chkQuit.set_TabIndex(77);
			this.chkQuit.set_Text("Quit");
			this.Label8.set_BackColor(SystemColors.get_Control());
			this.Label8.set_Cursor(Cursors.get_Default());
			this.Label8.set_Font(new Font("Arial", 8f, 0, 3, 0));
			this.Label8.set_ForeColor(SystemColors.get_ControlText());
			Label label81 = this.Label8;
			point = new Point(8, 32);
			label81.set_Location(point);
			this.Label8.set_Name("Label8");
			this.Label8.set_RightToLeft(0);
			Label label82 = this.Label8;
			size = new Size(48, 18);
			label82.set_Size(size);
			this.Label8.set_TabIndex(64);
			this.Label8.set_Text("Item slot");
			TextBox textBox16 = this.txtItemSlot;
			point = new Point(72, 32);
			textBox16.set_Location(point);
			this.txtItemSlot.set_Name("txtItemSlot");
			TextBox textBox17 = this.txtItemSlot;
			size = new Size(24, 20);
			textBox17.set_Size(size);
			this.txtItemSlot.set_TabIndex(63);
			this.txtItemSlot.set_Text("");
			this.Label3.set_BackColor(SystemColors.get_Control());
			this.Label3.set_Cursor(Cursors.get_Default());
			this.Label3.set_Font(new Font("Arial", 8f, 0, 3, 0));
			this.Label3.set_ForeColor(SystemColors.get_ControlText());
			Label label31 = this.Label3;
			point = new Point(8, 8);
			label31.set_Location(point);
			this.Label3.set_Name("Label3");
			this.Label3.set_RightToLeft(0);
			Label label32 = this.Label3;
			size = new Size(56, 18);
			label32.set_Size(size);
			this.Label3.set_TabIndex(60);
			this.Label3.set_Text("Craft Key");
			TextBox textBox18 = this.txtDistance;
			point = new Point(72, 56);
			textBox18.set_Location(point);
			this.txtDistance.set_Name("txtDistance");
			TextBox textBox19 = this.txtDistance;
			size = new Size(32, 20);
			textBox19.set_Size(size);
			this.txtDistance.set_TabIndex(61);
			this.txtDistance.set_Text("");
			this.gbQual.get_Controls().Add(this.cbQuality);
			this.gbQual.get_Controls().Add(this.Label14);
			this.gbQual.get_Controls().Add(this.Label13);
			this.gbQual.get_Controls().Add(this.txtQualNum);
			this.gbQual.get_Controls().Add(this.chkStopQual);
			GroupBox groupBox2 = this.gbQual;
			point = new Point(224, 0);
			groupBox2.set_Location(point);
			this.gbQual.set_Name("gbQual");
			GroupBox groupBox3 = this.gbQual;
			size = new Size(112, 96);
			groupBox3.set_Size(size);
			this.gbQual.set_TabIndex(92);
			this.gbQual.set_TabStop(false);
			this.gbQual.set_Visible(false);
			this.cbQuality.set_DropDownStyle(2);
			ComboBox.ObjectCollection items = this.cbQuality.get_Items();
			object[] objArray = new object[] { "94", "95", "96", "97", "98", "99", "100" };
			items.AddRange(objArray);
			ComboBox comboBox = this.cbQuality;
			point = new Point(8, 64);
			comboBox.set_Location(point);
			this.cbQuality.set_Name("cbQuality");
			ComboBox comboBox1 = this.cbQuality;
			size = new Size(48, 21);
			comboBox1.set_Size(size);
			this.cbQuality.set_TabIndex(62);
			Label label141 = this.Label14;
			point = new Point(64, 64);
			label141.set_Location(point);
			this.Label14.set_Name("Label14");
			Label label142 = this.Label14;
			size = new Size(40, 16);
			label142.set_Size(size);
			this.Label14.set_TabIndex(61);
			this.Label14.set_Text("Quality");
			Label label131 = this.Label13;
			point = new Point(40, 40);
			label131.set_Location(point);
			this.Label13.set_Name("Label13");
			Label label132 = this.Label13;
			size = new Size(56, 16);
			label132.set_Size(size);
			this.Label13.set_TabIndex(59);
			this.Label13.set_Text("Quantity");
			TextBox textBox20 = this.txtQualNum;
			point = new Point(8, 40);
			textBox20.set_Location(point);
			this.txtQualNum.set_Name("txtQualNum");
			TextBox textBox21 = this.txtQualNum;
			size = new Size(24, 20);
			textBox21.set_Size(size);
			this.txtQualNum.set_TabIndex(58);
			this.txtQualNum.set_Text("");
			CheckBox checkBox11 = this.chkStopQual;
			point = new Point(8, 16);
			checkBox11.set_Location(point);
			this.chkStopQual.set_Name("chkStopQual");
			CheckBox checkBox12 = this.chkStopQual;
			size = new Size(96, 24);
			checkBox12.set_Size(size);
			this.chkStopQual.set_TabIndex(0);
			this.chkStopQual.set_Text("Stop On Qual");
			this.TabPage5.get_Controls().Add(this.btnSelectPSkillFile);
			this.TabPage5.get_Controls().Add(this.Label16);
			this.TabPage5.get_Controls().Add(this.btnReadSkillfile);
			this.TabPage5.get_Controls().Add(this.ListView1);
			this.TabPage5.get_Controls().Add(this.cbTradeskill);
			this.TabPage5.get_Controls().Add(this.chkPowerskill);
			this.TabPage5.get_Controls().Add(this.Label15);
			this.TabPage5.get_Controls().Add(this.TxtSkillfilepath);
			TabPage tabPage5 = this.TabPage5;
			point = new Point(4, 4);
			tabPage5.set_Location(point);
			this.TabPage5.set_Name("TabPage5");
			TabPage tabPage51 = this.TabPage5;
			size = new Size(344, 220);
			tabPage51.set_Size(size);
			this.TabPage5.set_TabIndex(1);
			this.TabPage5.set_Text("PowerSkill");
			this.btnSelectPSkillFile.set_FlatStyle(3);
			Button button16 = this.btnSelectPSkillFile;
			point = new Point(312, 40);
			button16.set_Location(point);
			this.btnSelectPSkillFile.set_Name("btnSelectPSkillFile");
			Button button17 = this.btnSelectPSkillFile;
			size = new Size(24, 20);
			button17.set_Size(size);
			this.btnSelectPSkillFile.set_TabIndex(53);
			this.btnSelectPSkillFile.set_Text("...");
			this.Label16.set_Font(new Font("Arial", 8.25f, 0, 3, 0));
			Label label16 = this.Label16;
			point = new Point(24, 187);
			label16.set_Location(point);
			this.Label16.set_Name("Label16");
			Label label161 = this.Label16;
			size = new Size(152, 24);
			label161.set_Size(size);
			this.Label16.set_TabIndex(7);
			this.Label16.set_Text("File contains 10 lines max. Items seperated by colons");
			this.btnReadSkillfile.set_FlatStyle(3);
			Button button18 = this.btnReadSkillfile;
			point = new Point(244, 191);
			button18.set_Location(point);
			this.btnReadSkillfile.set_Name("btnReadSkillfile");
			this.btnReadSkillfile.set_TabIndex(5);
			this.btnReadSkillfile.set_Text("Read File");
			ListView.ColumnHeaderCollection columns = this.ListView1.get_Columns();
			ColumnHeader[] columnHeader1 = new ColumnHeader[] { this.ColumnHeader1, this.ColumnHeader2, this.ColumnHeader3 };
			columns.AddRange(columnHeader1);
			this.ListView1.set_FullRowSelect(true);
			this.ListView1.set_GridLines(true);
			this.ListView1.set_HeaderStyle(1);
			ListView listView1 = this.ListView1;
			point = new Point(23, 69);
			listView1.set_Location(point);
			this.ListView1.set_Name("ListView1");
			ListView listView = this.ListView1;
			size = new Size(296, 112);
			listView.set_Size(size);
			this.ListView1.set_TabIndex(4);
			this.ListView1.set_View(1);
			this.ColumnHeader1.set_Text("Start Skill");
			this.ColumnHeader1.set_Width(65);
			this.ColumnHeader2.set_Text("Item Name");
			this.ColumnHeader2.set_Width(193);
			this.ColumnHeader3.set_Text("Key");
			this.ColumnHeader3.set_Width(33);
			ComboBox.ObjectCollection objectCollection = this.cbTradeskill.get_Items();
			objArray = new object[] { "Weaponcraft", "Armorcraft", "Fletching", "Tailoring", "Spellcrafting", "Alchemy" };
			objectCollection.AddRange(objArray);
			ComboBox comboBox2 = this.cbTradeskill;
			point = new Point(198, 8);
			comboBox2.set_Location(point);
			this.cbTradeskill.set_Name("cbTradeskill");
			ComboBox comboBox3 = this.cbTradeskill;
			size = new Size(121, 21);
			comboBox3.set_Size(size);
			this.cbTradeskill.set_TabIndex(3);
			this.cbTradeskill.set_Text("TradeSkill");
			this.chkPowerskill.set_FlatStyle(3);
			CheckBox checkBox13 = this.chkPowerskill;
			point = new Point(28, 8);
			checkBox13.set_Location(point);
			this.chkPowerskill.set_Name("chkPowerskill");
			CheckBox checkBox14 = this.chkPowerskill;
			size = new Size(136, 24);
			checkBox14.set_Size(size);
			this.chkPowerskill.set_TabIndex(2);
			this.chkPowerskill.set_Text("Turn on PowerSkilling");
			this.Label15.set_Font(new Font("Arial", 8.25f, 0, 3, 0));
			this.Label15.set_ImageAlign(512);
			Label label151 = this.Label15;
			point = new Point(8, 40);
			label151.set_Location(point);
			this.Label15.set_Name("Label15");
			Label label152 = this.Label15;
			size = new Size(64, 17);
			label152.set_Size(size);
			this.Label15.set_TabIndex(1);
			this.Label15.set_Text("Path to File");
			TextBox txtSkillfilepath = this.TxtSkillfilepath;
			point = new Point(80, 40);
			txtSkillfilepath.set_Location(point);
			this.TxtSkillfilepath.set_Name("TxtSkillfilepath");
			TextBox txtSkillfilepath1 = this.TxtSkillfilepath;
			size = new Size(224, 20);
			txtSkillfilepath1.set_Size(size);
			this.TxtSkillfilepath.set_TabIndex(0);
			this.TxtSkillfilepath.set_Text("");
			this.TabPage6.get_Controls().Add(this.btnOrderPath);
			this.TabPage6.get_Controls().Add(this.Label18);
			this.TabPage6.get_Controls().Add(this.btnReadOrder);
			this.TabPage6.get_Controls().Add(this.lvOrder);
			this.TabPage6.get_Controls().Add(this.Label17);
			this.TabPage6.get_Controls().Add(this.txtOrderfile);
			this.TabPage6.get_Controls().Add(this.chkOrder);
			TabPage tabPage6 = this.TabPage6;
			point = new Point(4, 4);
			tabPage6.set_Location(point);
			this.TabPage6.set_Name("TabPage6");
			TabPage tabPage61 = this.TabPage6;
			size = new Size(344, 220);
			tabPage61.set_Size(size);
			this.TabPage6.set_TabIndex(2);
			this.TabPage6.set_Text("Order");
			this.btnOrderPath.set_FlatStyle(3);
			Button button19 = this.btnOrderPath;
			point = new Point(312, 40);
			button19.set_Location(point);
			this.btnOrderPath.set_Name("btnOrderPath");
			Button button20 = this.btnOrderPath;
			size = new Size(24, 20);
			button20.set_Size(size);
			this.btnOrderPath.set_TabIndex(54);
			this.btnOrderPath.set_Text("...");
			this.Label18.set_Font(new Font("Arial", 8.25f, 0, 3, 0));
			Label label18 = this.Label18;
			point = new Point(24, 187);
			label18.set_Location(point);
			this.Label18.set_Name("Label18");
			Label label181 = this.Label18;
			size = new Size(152, 24);
			label181.set_Size(size);
			this.Label18.set_TabIndex(10);
			this.Label18.set_Text("File contains 10 lines max. Items seperated by colons");
			this.btnReadOrder.set_FlatStyle(3);
			Button button21 = this.btnReadOrder;
			point = new Point(244, 191);
			button21.set_Location(point);
			this.btnReadOrder.set_Name("btnReadOrder");
			this.btnReadOrder.set_TabIndex(9);
			this.btnReadOrder.set_Text("Read File");
			ListView.ColumnHeaderCollection columnHeaderCollection = this.lvOrder.get_Columns();
			columnHeader1 = new ColumnHeader[] { this.ColumnHeader10, this.ColumnHeader12, this.ColumnHeader11 };
			columnHeaderCollection.AddRange(columnHeader1);
			this.lvOrder.set_FullRowSelect(true);
			this.lvOrder.set_GridLines(true);
			this.lvOrder.set_HeaderStyle(1);
			ListView listView2 = this.lvOrder;
			point = new Point(23, 69);
			listView2.set_Location(point);
			this.lvOrder.set_Name("lvOrder");
			ListView listView3 = this.lvOrder;
			size = new Size(296, 112);
			listView3.set_Size(size);
			this.lvOrder.set_TabIndex(8);
			this.lvOrder.set_View(1);
			this.ColumnHeader10.set_Text("Item Name");
			this.ColumnHeader10.set_Width(139);
			this.ColumnHeader12.set_Text("Min Qual");
			this.ColumnHeader12.set_Width(59);
			this.ColumnHeader11.set_Text("Key");
			this.ColumnHeader11.set_Width(93);
			this.Label17.set_Font(new Font("Arial", 8.25f, 0, 3, 0));
			this.Label17.set_ImageAlign(512);
			Label label17 = this.Label17;
			point = new Point(23, 40);
			label17.set_Location(point);
			this.Label17.set_Name("Label17");
			Label label171 = this.Label17;
			size = new Size(67, 17);
			label171.set_Size(size);
			this.Label17.set_TabIndex(3);
			this.Label17.set_Text("Path to File");
			TextBox textBox22 = this.txtOrderfile;
			point = new Point(95, 38);
			textBox22.set_Location(point);
			this.txtOrderfile.set_Name("txtOrderfile");
			TextBox textBox23 = this.txtOrderfile;
			size = new Size(209, 20);
			textBox23.set_Size(size);
			this.txtOrderfile.set_TabIndex(2);
			this.txtOrderfile.set_Text("");
			this.chkOrder.set_FlatStyle(3);
			CheckBox checkBox15 = this.chkOrder;
			point = new Point(28, 8);
			checkBox15.set_Location(point);
			this.chkOrder.set_Name("chkOrder");
			this.chkOrder.set_TabIndex(0);
			this.chkOrder.set_Text("Build this order");
			this.TabPage7.get_Controls().Add(this.GroupBox2);
			TabPage tabPage7 = this.TabPage7;
			point = new Point(4, 22);
			tabPage7.set_Location(point);
			this.TabPage7.set_Name("TabPage7");
			TabPage tabPage71 = this.TabPage7;
			size = new Size(352, 246);
			tabPage71.set_Size(size);
			this.TabPage7.set_TabIndex(3);
			this.TabPage7.set_Text("Waypoints");
			this.GroupBox2.get_Controls().Add(this.WPs);
			this.GroupBox2.get_Controls().Add(this.cmdAddWP0);
			this.GroupBox2.get_Controls().Add(this.txtWPtxt0);
			this.GroupBox2.get_Controls().Add(this.Label41);
			GroupBox groupBox21 = this.GroupBox2;
			point = new Point(8, 8);
			groupBox21.set_Location(point);
			this.GroupBox2.set_Name("GroupBox2");
			GroupBox groupBox22 = this.GroupBox2;
			size = new Size(280, 232);
			groupBox22.set_Size(size);
			this.GroupBox2.set_TabIndex(0);
			this.GroupBox2.set_TabStop(false);
			ListBox wPs = this.WPs;
			point = new Point(32, 64);
			wPs.set_Location(point);
			this.WPs.set_Name("WPs");
			ListBox listBox = this.WPs;
			size = new Size(144, 160);
			listBox.set_Size(size);
			this.WPs.set_TabIndex(72);
			this.cmdAddWP0.set_BackColor(SystemColors.get_Control());
			this.cmdAddWP0.set_Cursor(Cursors.get_Default());
			this.cmdAddWP0.set_FlatStyle(3);
			this.cmdAddWP0.set_Font(new Font("Arial", 8f, 0, 3, 0));
			this.cmdAddWP0.set_ForeColor(SystemColors.get_ControlText());
			Button button22 = this.cmdAddWP0;
			point = new Point(160, 40);
			button22.set_Location(point);
			this.cmdAddWP0.set_Name("cmdAddWP0");
			this.cmdAddWP0.set_RightToLeft(0);
			Button button23 = this.cmdAddWP0;
			size = new Size(16, 16);
			button23.set_Size(size);
			this.cmdAddWP0.set_TabIndex(71);
			this.cmdAddWP0.set_TabStop(false);
			this.cmdAddWP0.set_Text("..");
			this.txtWPtxt0.set_AcceptsReturn(true);
			this.txtWPtxt0.set_AutoSize(false);
			this.txtWPtxt0.set_BackColor(SystemColors.get_Window());
			this.txtWPtxt0.set_Cursor(Cursors.get_IBeam());
			this.txtWPtxt0.set_Font(new Font("Arial", 8f, 0, 3, 0));
			this.txtWPtxt0.set_ForeColor(SystemColors.get_WindowText());
			TextBox textBox24 = this.txtWPtxt0;
			point = new Point(32, 40);
			textBox24.set_Location(point);
			this.txtWPtxt0.set_MaxLength(11);
			this.txtWPtxt0.set_Name("txtWPtxt0");
			this.txtWPtxt0.set_ReadOnly(true);
			this.txtWPtxt0.set_RightToLeft(0);
			TextBox textBox25 = this.txtWPtxt0;
			size = new Size(121, 19);
			textBox25.set_Size(size);
			this.txtWPtxt0.set_TabIndex(70);
			this.txtWPtxt0.set_Text("");
			this.Label41.set_BackColor(SystemColors.get_Control());
			this.Label41.set_Cursor(Cursors.get_Default());
			this.Label41.set_FlatStyle(3);
			this.Label41.set_Font(new Font("Arial", 8f, 0, 3, 0));
			this.Label41.set_ForeColor(SystemColors.get_ControlText());
			Label label41 = this.Label41;
			point = new Point(64, 16);
			label41.set_Location(point);
			this.Label41.set_Name("Label41");
			this.Label41.set_RightToLeft(0);
			Label label411 = this.Label41;
			size = new Size(56, 16);
			label411.set_Size(size);
			this.Label41.set_TabIndex(69);
			this.Label41.set_Text("Waypoints:");
			this.TabPage8.get_Controls().Add(this.GroupBox3);
			TabPage tabPage8 = this.TabPage8;
			point = new Point(4, 22);
			tabPage8.set_Location(point);
			this.TabPage8.set_Name("TabPage8");
			TabPage tabPage81 = this.TabPage8;
			size = new Size(352, 246);
			tabPage81.set_Size(size);
			this.TabPage8.set_TabIndex(4);
			this.TabPage8.set_Text("Profile");
			this.GroupBox3.get_Controls().Add(this.btnSave);
			this.GroupBox3.get_Controls().Add(this.Label19);
			this.GroupBox3.get_Controls().Add(this.cbProfileName);
			GroupBox groupBox31 = this.GroupBox3;
			point = new Point(8, 8);
			groupBox31.set_Location(point);
			this.GroupBox3.set_Name("GroupBox3");
			GroupBox groupBox32 = this.GroupBox3;
			size = new Size(312, 216);
			groupBox32.set_Size(size);
			this.GroupBox3.set_TabIndex(0);
			this.GroupBox3.set_TabStop(false);
			this.btnSave.set_FlatStyle(3);
			Button button24 = this.btnSave;
			point = new Point(216, 24);
			button24.set_Location(point);
			this.btnSave.set_Name("btnSave");
			Button button25 = this.btnSave;
			size = new Size(48, 23);
			button25.set_Size(size);
			this.btnSave.set_TabIndex(7);
			this.btnSave.set_Text("Save");
			this.Label19.set_FlatStyle(3);
			this.Label19.set_Font(new Font("Arial", 8.25f, 0, 3, 0));
			Label label19 = this.Label19;
			point = new Point(16, 24);
			label19.set_Location(point);
			this.Label19.set_Name("Label19");
			Label label191 = this.Label19;
			size = new Size(72, 16);
			label191.set_Size(size);
			this.Label19.set_TabIndex(5);
			this.Label19.set_Text("Profile Name");
			ComboBox comboBox4 = this.cbProfileName;
			point = new Point(88, 24);
			comboBox4.set_Location(point);
			this.cbProfileName.set_Name("cbProfileName");
			ComboBox comboBox5 = this.cbProfileName;
			size = new Size(120, 21);
			comboBox5.set_Size(size);
			this.cbProfileName.set_TabIndex(6);
			this.SaveFileDialog1.set_FileName("chat.log");
			this.ColumnHeader5.set_Text("Start Skill");
			this.ColumnHeader5.set_Width(65);
			this.ColumnHeader6.set_Text("Item Name");
			this.ColumnHeader6.set_Width(193);
			this.ColumnHeader7.set_Text("Key");
			this.ColumnHeader7.set_Width(33);
			this.ColumnHeader8.set_Width(1);
			this.rbDarkness.set_FlatStyle(3);
			RadioButton radioButton7 = this.rbDarkness;
			point = new Point(16, 56);
			radioButton7.set_Location(point);
			this.rbDarkness.set_Name("rbDarkness");
			this.rbDarkness.set_TabIndex(5);
			this.rbDarkness.set_Text("Darkness Rising");
			size = new Size(5, 13);
			this.set_AutoScaleBaseSize(size);
			this.set_BackColor(SystemColors.get_Control());
			size = new Size(358, 268);
			this.set_ClientSize(size);
			this.get_Controls().Add(this.TabControl1);
			this.set_FormBorderStyle(3);
			this.set_Icon((Icon)resourceManager.GetObject("$this.Icon"));
			this.set_Name("frmMain");
			this.set_StartPosition(1);
			this.set_Text("Crafter");
			this.set_TopMost(true);
			this.TabControl1.ResumeLayout(false);
			this.TabPage1.ResumeLayout(false);
			this.GroupBox1.ResumeLayout(false);
			this.TabPage2.ResumeLayout(false);
			this.GroupBox10.ResumeLayout(false);
			this.GroupBox4.ResumeLayout(false);
			this.TabPage3.ResumeLayout(false);
			this.TabControl2.ResumeLayout(false);
			this.TabPage4.ResumeLayout(false);
			this.gbQual.ResumeLayout(false);
			this.TabPage5.ResumeLayout(false);
			this.TabPage6.ResumeLayout(false);
			this.TabPage7.ResumeLayout(false);
			this.GroupBox2.ResumeLayout(false);
			this.TabPage8.ResumeLayout(false);
			this.GroupBox3.ResumeLayout(false);
			this.ResumeLayout(false);
		}

		private void LoadSettings(string Profile)
		{
			IEnumerator enumerator = null;
			UpdateInfo updateInfo = new UpdateInfo();
			UpdateTable updateTable = new UpdateTable()
			{
				Hash = updateTable.Deserialize(string.Concat(Application.get_StartupPath(), "\\", Profile, ".acp"))
			};
			if (updateTable.Hash.get_Count() == 0)
			{
				updateTable.Hash.Add(updateInfo);
			}
			updateInfo = (UpdateInfo)updateTable.Hash.get_Item(0);
			this.rbEuro.set_Checked(updateInfo.mEuro);
			this.rbUS.set_Checked(updateInfo.mUS);
			this.rbToA.set_Checked(updateInfo.mToA);
			this.rbCatacombs.set_Checked(updateInfo.mCAT);
			this.rbDarkness.set_Checked(updateInfo.mDR);
			this.chkKillProcess.set_Checked(updateInfo.mKillProcess);
			this.chkStopQual.set_Checked(updateInfo.mStopQual);
			this.txtQualNum.set_Text(updateInfo.mQualNum);
			this.chkSelling.set_Checked(updateInfo.mSelling);
			this.txtItem.set_Text(updateInfo.mCurrentItem);
			this.chkStock.set_Checked(updateInfo.mStock);
			this.txtVendorName.set_Text(updateInfo.mVendorName);
			this.txtItemSlot.set_Text(updateInfo.mItemSlot);
			this.set_Left(updateInfo.mXPos);
			this.set_Top(updateInfo.mYPos);
			this.txtCraftKey.set_Text(updateInfo.mCraftKey);
			this.chkQuit.set_Checked(updateInfo.mQuit);
			this.txtDistance.set_Text(updateInfo.mDistance);
			this.txtRegKey.set_Text(updateInfo.mRegKey);
			this.txtGamePath.set_Text(updateInfo.mGamePath);
			this.chkSalvage.set_Checked(updateInfo.mSalvage);
			this.txtVenLoc.set_Text(updateInfo.mVendorLoc);
			this.txtForgeLoc.set_Text(updateInfo.mForgeLoc);
			this.cbQuality.set_SelectedIndex(updateInfo.mQuality);
			this.chkDisableSound.set_Checked(updateInfo.mDisableSound);
			if (!Information.IsNothing(updateInfo.mWaypoints) && updateInfo.mWaypoints.get_Count() != 0)
			{
				this.mWaypoints = updateInfo.mWaypoints;
				try
				{
					enumerator = this.mWaypoints.GetEnumerator();
					while (enumerator.MoveNext())
					{
						string str = StringType.FromObject(enumerator.get_Current());
						this.WPs.get_Items().Add(str);
					}
				}
				finally
				{
					if (enumerator is IDisposable)
					{
						((IDisposable)enumerator).Dispose();
					}
				}
			}
		}

		private void LoadStrings()
		{
			XmlDocument xmlDocument = new XmlDocument();
			if (!(new FileInfo(string.Concat(this.folder, "\\CrafterStrings.xml"))).get_Exists())
			{
				return;
			}
			try
			{
				xmlDocument.Load(string.Concat(this.folder, "\\CrafterStrings.xml"));
				XmlElement xmlElement = (XmlElement)xmlDocument.SelectSingleNode("//section[@name='General Settings']");
				XmlElement xmlElement1 = (XmlElement)xmlElement.SelectSingleNode("item[@key='S0']");
				if (xmlElement1 != null)
				{
					this.S0 = xmlElement1.GetAttribute("newValue");
				}
				xmlElement1 = (XmlElement)xmlElement.SelectSingleNode("item[@key='S1']");
				if (xmlElement1 != null)
				{
					this.S1 = xmlElement1.GetAttribute("newValue");
				}
				xmlElement1 = (XmlElement)xmlElement.SelectSingleNode("item[@key='S2']");
				if (xmlElement1 != null)
				{
					this.S2 = xmlElement1.GetAttribute("newValue");
				}
				xmlElement1 = (XmlElement)xmlElement.SelectSingleNode("item[@key='S3']");
				if (xmlElement1 != null)
				{
					this.S3 = xmlElement1.GetAttribute("newValue");
				}
				xmlElement1 = (XmlElement)xmlElement.SelectSingleNode("item[@key='S4']");
				if (xmlElement1 != null)
				{
					this.S4 = xmlElement1.GetAttribute("newValue");
				}
				xmlElement1 = (XmlElement)xmlElement.SelectSingleNode("item[@key='S5']");
				if (xmlElement1 != null)
				{
					this.S5 = xmlElement1.GetAttribute("newValue");
				}
				xmlElement1 = (XmlElement)xmlElement.SelectSingleNode("item[@key='S6']");
				if (xmlElement1 != null)
				{
					this.S6 = xmlElement1.GetAttribute("newValue");
				}
				xmlElement1 = (XmlElement)xmlElement.SelectSingleNode("item[@key='S7']");
				if (xmlElement1 != null)
				{
					this.S7 = xmlElement1.GetAttribute("newValue");
				}
				xmlElement1 = (XmlElement)xmlElement.SelectSingleNode("item[@key='S8']");
				if (xmlElement1 != null)
				{
					this.S8 = xmlElement1.GetAttribute("newValue");
				}
				xmlElement1 = (XmlElement)xmlElement.SelectSingleNode("item[@key='S9']");
				if (xmlElement1 != null)
				{
					this.S9 = xmlElement1.GetAttribute("newValue");
				}
			}
			catch (Exception exception)
			{
				ProjectData.SetProjectError(exception);
				ProjectData.ClearProjectError();
			}
		}

		[STAThread]
		public static void Main()
		{
			Application.Run(new frmMain());
		}

		private void MakeOrder()
		{
			int num = 0;
			clsOrderDetail item = (clsOrderDetail)this.alOrder.get_Item(num);
			this.mCraftKey = item.CraftKey;
			while (this.Running)
			{
				try
				{
					switch (ShortType.FromString(item.MinQuality))
					{
						case 94:
						{
							if (this.Ninety4 <= 0)
							{
								break;
							}
							num = checked(num + 1);
							if (num <= checked(this.alOrder.get_Count() - 1))
							{
								item = (clsOrderDetail)this.alOrder.get_Item(num);
								this.mCraftKey = item.CraftKey;
							}
							else
							{
								this.Running = false;
								if (!this.chkDisableSound.get_Checked())
								{
									this.StartAlarm();
								}
							}
							break;
						}
						case 95:
						{
							if (this.Ninety5 <= 0)
							{
								break;
							}
							num = checked(num + 1);
							if (num <= checked(this.alOrder.get_Count() - 1))
							{
								item = (clsOrderDetail)this.alOrder.get_Item(num);
								this.mCraftKey = item.CraftKey;
							}
							else
							{
								this.Running = false;
								if (!this.chkDisableSound.get_Checked())
								{
									this.StartAlarm();
								}
							}
							break;
						}
						case 96:
						{
							if (this.Ninety6 <= 0)
							{
								break;
							}
							num = checked(num + 1);
							if (num <= checked(this.alOrder.get_Count() - 1))
							{
								item = (clsOrderDetail)this.alOrder.get_Item(num);
								this.mCraftKey = item.CraftKey;
							}
							else
							{
								this.Running = false;
								if (!this.chkDisableSound.get_Checked())
								{
									this.StartAlarm();
								}
							}
							break;
						}
						case 97:
						{
							if (this.Ninety7 <= 0)
							{
								break;
							}
							num = checked(num + 1);
							if (num <= checked(this.alOrder.get_Count() - 1))
							{
								item = (clsOrderDetail)this.alOrder.get_Item(num);
								this.mCraftKey = item.CraftKey;
							}
							else
							{
								this.Running = false;
								if (!this.chkDisableSound.get_Checked())
								{
									this.StartAlarm();
								}
							}
							break;
						}
						case 98:
						{
							if (this.Ninety8 <= 0)
							{
								break;
							}
							num = checked(num + 1);
							if (num <= checked(this.alOrder.get_Count() - 1))
							{
								item = (clsOrderDetail)this.alOrder.get_Item(num);
								this.mCraftKey = item.CraftKey;
							}
							else
							{
								this.Running = false;
								if (!this.chkDisableSound.get_Checked())
								{
									this.StartAlarm();
								}
							}
							break;
						}
						case 99:
						{
							if (this.Ninety9 <= 0)
							{
								break;
							}
							num = checked(num + 1);
							if (num <= checked(this.alOrder.get_Count() - 1))
							{
								item = (clsOrderDetail)this.alOrder.get_Item(num);
								this.mCraftKey = item.CraftKey;
							}
							else
							{
								this.Running = false;
								if (!this.chkDisableSound.get_Checked())
								{
									this.StartAlarm();
								}
							}
							break;
						}
						case 100:
						{
							if (this.MP <= 0)
							{
								break;
							}
							num = checked(num + 1);
							if (num <= checked(this.alOrder.get_Count() - 1))
							{
								item = (clsOrderDetail)this.alOrder.get_Item(num);
								this.mCraftKey = item.CraftKey;
								break;
							}
							else
							{
								this.Running = false;
								if (this.chkDisableSound.get_Checked())
								{
									break;
								}
								this.StartAlarm();
								break;
							}
						}
					}
					if (this.OkToCraft)
					{
						MainMod.AK.SendString(this.mCraftKey);
						this.OkToCraft = false;
					}
				}
				catch (Exception exception)
				{
					ProjectData.SetProjectError(exception);
					ProjectData.ClearProjectError();
				}
			}
		}

		private void picRunning_Click(object sender, EventArgs e)
		{
			this.Beeping = false;
		}

		private void Query(clsAutoKillerScript.AutokillerRegExEventParams e)
		{
			IEnumerator enumerator = null;
			switch (e.QueryID)
			{
				case 0:
				case 5:
				{
					this.EndThreads();
					if (!this.chkDisableSound.get_Checked())
					{
						this.StartAlarm();
					}
					if (!this.chkQuit.get_Checked())
					{
						break;
					}
					MainMod.AK.SendString("/quit~");
					if (this.chkKillProcess.get_Checked())
					{
						Thread.Sleep(60000);
						MainMod.AK.KillGame();
					}
					break;
				}
				case 1:
				case 2:
				{
					if (!this.chkDisableSound.get_Checked())
					{
						this.StartAlarm();
					}
					if (MainMod.AK.SearchArea(IntegerType.FromString(this.txtDistance.get_Text()), 2) <= -1)
					{
						break;
					}
					if (this.chkQuit.get_Checked())
					{
						this.EndThreads();
						MainMod.AK.SendString("/quit~");
						if (this.chkKillProcess.get_Checked())
						{
							Thread.Sleep(60000);
							MainMod.AK.KillGame();
						}
					}
					break;
				}
				case 3:
				case 4:
				case 6:
				case 9:
				case 10:
				case 11:
				{
					if (e.QueryID == 3)
					{
						switch (ShortType.FromString(e.RegExMatch.get_Groups().get_Item("quality").get_Value()))
						{
							case 94:
							{
								this.Ninety4 = checked(this.Ninety4 + 1);
								this.lblItem94Quality.set_Text(this.Ninety4.ToString());
								break;
							}
							case 95:
							{
								this.Ninety5 = checked(this.Ninety5 + 1);
								this.lblItem95Quality.set_Text(this.Ninety5.ToString());
								break;
							}
							case 96:
							{
								this.Ninety6 = checked(this.Ninety6 + 1);
								this.lblItem96Quality.set_Text(this.Ninety6.ToString());
								break;
							}
							case 97:
							{
								this.Ninety7 = checked(this.Ninety7 + 1);
								this.lblItem97Quality.set_Text(this.Ninety7.ToString());
								break;
							}
							case 98:
							{
								this.Ninety8 = checked(this.Ninety8 + 1);
								this.lblItem98Quality.set_Text(this.Ninety8.ToString());
								break;
							}
							case 99:
							{
								this.Ninety9 = checked(this.Ninety9 + 1);
								this.lblItem99Quality.set_Text(this.Ninety9.ToString());
								break;
							}
							case 100:
							{
								this.MP = checked(this.MP + 1);
								this.lblItem100Quality.set_Text(this.MP.ToString());
								break;
							}
						}
						if (this.StopQual)
						{
							string quality = this.Quality;
							if (StringType.StrCmp(quality, "94", false) == 0)
							{
								if (this.Ninety4 >= this.QualNum)
								{
									this.Running = false;
									if (!this.chkDisableSound.get_Checked())
									{
										this.StartAlarm();
									}
								}
							}
							else if (StringType.StrCmp(quality, "95", false) == 0)
							{
								if (this.Ninety5 >= this.QualNum)
								{
									this.Running = false;
									if (!this.chkDisableSound.get_Checked())
									{
										this.StartAlarm();
									}
								}
							}
							else if (StringType.StrCmp(quality, "96", false) == 0)
							{
								if (this.Ninety6 >= this.QualNum)
								{
									this.Running = false;
									if (!this.chkDisableSound.get_Checked())
									{
										this.StartAlarm();
									}
								}
							}
							else if (StringType.StrCmp(quality, "97", false) == 0)
							{
								if (this.Ninety7 >= this.QualNum)
								{
									this.Running = false;
									if (!this.chkDisableSound.get_Checked())
									{
										this.StartAlarm();
									}
								}
							}
							else if (StringType.StrCmp(quality, "98", false) == 0)
							{
								if (this.Ninety8 >= this.QualNum)
								{
									this.Running = false;
									if (!this.chkDisableSound.get_Checked())
									{
										this.StartAlarm();
									}
								}
							}
							else if (StringType.StrCmp(quality, "99", false) == 0)
							{
								if (this.Ninety9 >= this.QualNum)
								{
									this.Running = false;
									if (!this.chkDisableSound.get_Checked())
									{
										this.StartAlarm();
									}
								}
							}
							else if (StringType.StrCmp(quality, "100", false) == 0 && this.MP >= this.QualNum)
							{
								this.Running = false;
								if (!this.chkDisableSound.get_Checked())
								{
									this.StartAlarm();
								}
							}
						}
					}
					if (!this.Running)
					{
						break;
					}
					this.OkToCraft = true;
					break;
				}
				case 7:
				{
					this.OkToSalvage = true;
					break;
				}
				case 8:
				{
					string value = e.RegExMatch.get_Groups().get_Item("craft").get_Value();
					string str = e.RegExMatch.get_Groups().get_Item("skill").get_Value();
					if (!this.chkPowerskill.get_Checked() || StringType.StrCmp(value, this.cbTradeskill.get_Text(), false) != 0)
					{
						break;
					}
					try
					{
						enumerator = this.sd.GetEnumerator();
						while (enumerator.MoveNext())
						{
							clsSkillDetail current = (clsSkillDetail)enumerator.get_Current();
							if (StringType.StrCmp(str, current.SkillLevel, false) != 0)
							{
								continue;
							}
							this.mCraftKey = current.CraftKey;
							this.txtItem.set_Text(current.ItemName);
						}
						break;
					}
					finally
					{
						if (enumerator is IDisposable)
						{
							((IDisposable)enumerator).Dispose();
						}
					}
					break;
				}
			}
		}

		private void ReadOrderFile(string txtFileName)
		{
			StreamReader streamReader = null;
			try
			{
				try
				{
					streamReader = File.OpenText(txtFileName);
					for (string i = streamReader.ReadLine(); i != null; i = streamReader.ReadLine())
					{
						string[] strArray = i.Split(new char[] { ':' });
						ListViewItem listViewItem = this.lvOrder.get_Items().Add(strArray[0]);
						listViewItem.get_SubItems().Add(strArray[1]);
						listViewItem.get_SubItems().Add(strArray[2]);
						this.TheOrder = new clsOrderDetail()
						{
							ItemName = strArray[0],
							MinQuality = strArray[1],
							CraftKey = strArray[2]
						};
						this.alOrder.Add(this.TheOrder);
					}
				}
				catch (Exception exception1)
				{
					ProjectData.SetProjectError(exception1);
					Exception exception = exception1;
					Interaction.MsgBox(string.Concat("File could not be opened or read.\r\nPlease verify that the filename is correct, and that you have read permissions for the desired directory.\r\n\r\nException: ", exception.get_Message()), 0, null);
					ProjectData.ClearProjectError();
				}
			}
			finally
			{
				if (streamReader != null)
				{
					streamReader.Close();
				}
			}
		}

		private void ReadTextFile(string txtFileName)
		{
			StreamReader streamReader = null;
			try
			{
				try
				{
					streamReader = File.OpenText(txtFileName);
					for (string i = streamReader.ReadLine(); i != null; i = streamReader.ReadLine())
					{
						string[] strArray = i.Split(new char[] { ':' });
						ListViewItem listViewItem = this.ListView1.get_Items().Add(strArray[0]);
						listViewItem.get_SubItems().Add(strArray[1]);
						listViewItem.get_SubItems().Add(strArray[2]);
						this.skilldetail = new clsSkillDetail()
						{
							SkillLevel = strArray[0],
							ItemName = strArray[1],
							CraftKey = strArray[2]
						};
						this.sd.Add(this.skilldetail);
					}
				}
				catch (Exception exception1)
				{
					ProjectData.SetProjectError(exception1);
					Exception exception = exception1;
					Interaction.MsgBox(string.Concat("File could not be opened or read.\r\nPlease verify that the filename is correct, and that you have read permissions for the desired directory.\r\n\r\nException: ", exception.get_Message()), 0, null);
					ProjectData.ClearProjectError();
				}
			}
			finally
			{
				if (streamReader != null)
				{
					streamReader.Close();
				}
			}
		}

		private void RuntoForge()
		{
			for (int i = checked(this.mWaypoints.get_Count() - 1); i >= 0; i = checked(i + -1))
			{
				string[] strArray = Strings.Split(StringType.FromObject(this.mWaypoints.get_Item(i)), ",", -1, 0);
				this.gGotoXY(DoubleType.FromString(strArray[0]), DoubleType.FromString(strArray[1]), 250);
			}
			string[] strArray1 = Strings.Split(this.txtForgeLoc.get_Text(), ",", -1, 0);
			this.gGotoXY(DoubleType.FromString(strArray1[0]), DoubleType.FromString(strArray1[1]), 50);
		}

		private void RuntoVendor()
		{
			string[] strArray;
			IEnumerator enumerator = null;
			try
			{
				enumerator = this.mWaypoints.GetEnumerator();
				while (enumerator.MoveNext())
				{
					string str = StringType.FromObject(enumerator.get_Current());
					strArray = Strings.Split(str, ",", -1, 0);
					this.gGotoXY(DoubleType.FromString(strArray[0]), DoubleType.FromString(strArray[1]), 250);
				}
			}
			finally
			{
				if (enumerator is IDisposable)
				{
					((IDisposable)enumerator).Dispose();
				}
			}
			strArray = Strings.Split(this.txtVenLoc.get_Text(), ",", -1, 0);
			this.gGotoXY(DoubleType.FromString(strArray[0]), DoubleType.FromString(strArray[1]), 250);
		}

		private void Salvage()
		{
			short bag;
			if (StringType.StrCmp(this.txtItemSlot.get_Text(), null, false) != 0)
			{
				bag = this.GetBag(ShortType.FromString(this.txtItemSlot.get_Text()));
			}
			else
			{
				bag = 0;
			}
			StatsWindow statsWindow = new StatsWindow(MainMod.AK);
			WindowManager windowManager = new WindowManager(MainMod.AK, 30);
			statsWindow.ClickPage(0);
			Thread.Sleep(1000);
			statsWindow.ClickPage(1);
			short num = bag;
			for (short i = 0; i <= num; i = checked((short)(i + 1)))
			{
				statsWindow.SelectInventoryBag(i);
				Thread.Sleep(2000);
				byte num1 = 0;
				do
				{
					statsWindow.SelectInventoryItem((int)num1);
					MainMod.AK.RightClick();
					Thread.Sleep(1000);
					this.Keys.CraftSalvage(2);
					Thread.Sleep(1000);
					if (!this.rbToA.get_Checked())
					{
						MainMod.AK.MouseMove(checked(windowManager.get_Left() + 50), checked(windowManager.get_Top() + 80));
						Thread.Sleep(250);
						MainMod.AK.LeftClick();
					}
					else
					{
						MainMod.AK.MouseMove(checked(windowManager.get_Left() + 208), checked(windowManager.get_Top() + 61));
						Thread.Sleep(250);
						MainMod.AK.LeftClick();
					}
					while (!this.OkToSalvage && this.Running)
					{
						Thread.Sleep(1);
					}
					this.OkToSalvage = false;
					if (!this.Running)
					{
						return;
					}
					num1 = checked((byte)(num1 + 1));
				}
				while (num1 <= 7);
			}
			if (!this.chkDisableSound.get_Checked())
			{
				this.StartAlarm();
			}
		}

		private void SaveSettings(string Profile)
		{
			UpdateInfo updateInfo = new UpdateInfo();
			UpdateTable updateTable = new UpdateTable();
			updateInfo.mEuro = this.rbEuro.get_Checked();
			updateInfo.mUS = this.rbUS.get_Checked();
			updateInfo.mToA = this.rbToA.get_Checked();
			updateInfo.mCAT = this.rbCatacombs.get_Checked();
			updateInfo.mDR = this.rbDarkness.get_Checked();
			updateInfo.mKillProcess = this.chkKillProcess.get_Checked();
			updateInfo.mStopQual = this.chkStopQual.get_Checked();
			updateInfo.mQualNum = this.txtQualNum.get_Text();
			updateInfo.mSelling = this.chkSelling.get_Checked();
			updateInfo.mCurrentItem = this.txtItem.get_Text();
			updateInfo.mStock = this.chkStock.get_Checked();
			updateInfo.mVendorName = this.txtVendorName.get_Text();
			updateInfo.mItemSlot = this.txtItemSlot.get_Text();
			updateInfo.mXPos = this.get_Left();
			updateInfo.mYPos = this.get_Top();
			updateInfo.mCraftKey = this.txtCraftKey.get_Text();
			updateInfo.mQuit = this.chkQuit.get_Checked();
			updateInfo.mDistance = this.txtDistance.get_Text();
			updateInfo.mRegKey = this.txtRegKey.get_Text();
			updateInfo.mGamePath = this.txtGamePath.get_Text();
			updateInfo.mSalvage = this.chkSalvage.get_Checked();
			updateInfo.mVendorLoc = this.txtVenLoc.get_Text();
			updateInfo.mForgeLoc = this.txtForgeLoc.get_Text();
			updateInfo.mQuality = this.cbQuality.get_SelectedIndex();
			updateInfo.mDisableSound = this.chkDisableSound.get_Checked();
			updateInfo.mWaypoints = this.mWaypoints;
			updateTable.Hash.Add(updateInfo);
			updateTable.Serialize(string.Concat(Application.get_StartupPath(), "\\", Profile, ".acp"));
		}

		private void SaveStrings()
		{
			XmlDocument xmlDocument = new XmlDocument();
			xmlDocument.LoadXml("<?xml version=\"1.0\" encoding=\"UTF-8\"?><sections></sections>");
			XmlElement xmlElement = xmlDocument.CreateElement("section");
			XmlAttribute xmlAttribute = xmlDocument.CreateAttribute("name");
			xmlAttribute.set_Value("General Settings");
			xmlElement.get_Attributes().SetNamedItem(xmlAttribute);
			xmlDocument.get_DocumentElement().AppendChild(xmlElement);
			xmlElement.AppendChild(xmlDocument.CreateComment("Sample XML document"));
			XmlElement xmlElement1 = xmlDocument.CreateElement("item");
			xmlElement1.SetAttribute("key", "S0");
			xmlElement1.SetAttribute("newValue", this.S0);
			xmlElement.AppendChild(xmlElement1);
			xmlElement1 = xmlDocument.CreateElement("item");
			xmlElement1.SetAttribute("key", "S1");
			xmlElement1.SetAttribute("newValue", this.S1);
			xmlElement.AppendChild(xmlElement1);
			xmlElement1 = xmlDocument.CreateElement("item");
			xmlElement1.SetAttribute("key", "S2");
			xmlElement1.SetAttribute("newValue", this.S2);
			xmlElement.AppendChild(xmlElement1);
			xmlElement1 = xmlDocument.CreateElement("item");
			xmlElement1.SetAttribute("key", "S3");
			xmlElement1.SetAttribute("newValue", this.S3);
			xmlElement.AppendChild(xmlElement1);
			xmlElement1 = xmlDocument.CreateElement("item");
			xmlElement1.SetAttribute("key", "S4");
			xmlElement1.SetAttribute("newValue", this.S4);
			xmlElement.AppendChild(xmlElement1);
			xmlElement1 = xmlDocument.CreateElement("item");
			xmlElement1.SetAttribute("key", "S5");
			xmlElement1.SetAttribute("newValue", this.S5);
			xmlElement.AppendChild(xmlElement1);
			xmlElement1 = xmlDocument.CreateElement("item");
			xmlElement1.SetAttribute("key", "S6");
			xmlElement1.SetAttribute("newValue", this.S6);
			xmlElement.AppendChild(xmlElement1);
			xmlElement1 = xmlDocument.CreateElement("item");
			xmlElement1.SetAttribute("key", "S7");
			xmlElement1.SetAttribute("newValue", this.S7);
			xmlElement.AppendChild(xmlElement1);
			xmlElement1 = xmlDocument.CreateElement("item");
			xmlElement1.SetAttribute("key", "S8");
			xmlElement1.SetAttribute("newValue", this.S8);
			xmlElement.AppendChild(xmlElement1);
			xmlElement1 = xmlDocument.CreateElement("item");
			xmlElement1.SetAttribute("key", "S9");
			xmlElement1.SetAttribute("newValue", this.S9);
			xmlElement.AppendChild(xmlElement1);
			xmlDocument.Save(string.Concat(this.folder, "\\CrafterStrings.xml"));
		}

		public void Sleep(int miliseconds)
		{
			int num = checked((int)Math.Round((double)miliseconds / 50));
			for (int i = 1; i <= num; i = checked(i + 1))
			{
				Thread.Sleep(50);
				Application.DoEvents();
			}
		}

		private void StartAlarm()
		{
			this.Beeping = true;
			frmMain _frmMain = this;
			frmMain.TaskDelegate taskDelegate = new frmMain.TaskDelegate(_frmMain.Alarm);
			taskDelegate.BeginInvoke(null, null);
		}

		private void txtQualNum_TextChanged(object sender, EventArgs e)
		{
			if (!this.txtQualNum.get_Text().Equals(""))
			{
				this.QualNum = ShortType.FromString(this.txtQualNum.get_Text());
			}
			else
			{
				this.txtQualNum.set_Text("1");
			}
		}

		private void UpdateProfiles()
		{
			this.cbProfileName.get_Items().Clear();
			string[] files = Directory.GetFiles(".");
			for (int i = 0; i < (int)files.Length; i = checked(i + 1))
			{
				string str = files[i];
				if (StringType.StrCmp(Path.GetExtension(str).ToLower(), ".acp", false) == 0)
				{
					this.cbProfileName.get_Items().Add(Path.GetFileNameWithoutExtension(str));
				}
			}
		}

		private void WPs_DoubleClick(object sender, EventArgs e)
		{
			if (this.WPs.get_SelectedIndex() != -1)
			{
				this.mWaypoints.RemoveAt(this.WPs.get_SelectedIndex());
				this.WPs.get_Items().RemoveAt(this.WPs.get_SelectedIndex());
				this.txtWPtxt0.set_Text(null);
			}
		}

		private void WPs_SelectedIndexChanged(object sender, EventArgs e)
		{
			try
			{
				if (this.WPs.get_SelectedIndex() != -1)
				{
					this.txtWPtxt0.set_Text(StringType.FromObject(this.WPs.get_Items().get_Item(this.WPs.get_SelectedIndex())));
				}
			}
			catch (Exception exception)
			{
				ProjectData.SetProjectError(exception);
				ProjectData.ClearProjectError();
			}
		}

		public delegate void TaskDelegate();
	}
}