// Decompiled with JetBrains decompiler
// Type: IsoPack.Settings
// Assembly: IsoTool, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A40E7877-59D4-416C-9526-ACFD66F37CC4
// Assembly location: C:\Program Files\Iso Tool\IsoTool.exe

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace IsoPack
{
  public class Settings : Form
  {
    private bool _bChanged = true;
    private MainForm _objMainForm = (MainForm) null;
    private IContainer components = (IContainer) null;
    private TextBox _txtLog;
    private Label label1;
    private Label label2;
    private Label label3;
    private TextBox _txtBlenderPath;
    private Label _lblBlenderPath;
    private Button _btnBlenderPath;
    private TextBox _txtTempFolder;
    private Label _lblTempFolder;
    private Button _btnTempFolder;
    private Button _btnClose;
    private TextBox _txtProjectRoot;
    private Label _lblProjectRoot;
    private Button _btnProjectRoot;
    private TextBox _txtPythonScriptPath;
    private Label label7;
    private Button _btnPythonScriptPath;
    private Label label8;
    private Label _lblVersion;
    private Button _btnClearLog;
    private NumericUpDown _nudMaxSize;
    private Label _lblMaxSize;
    private Label _lblMinSize;
    private NumericUpDown _nudMinSize;
    private Label _lblGrowBy;
    private NumericUpDown _nudGrowBy;
    private Label label10;
    private Label label11;
    private Label label12;
    private CheckBox _chkPackImages;
    private CheckBox _chkSaveImagesDebug;
    private Button _btnClearTemp;
    private Button _btnApply;
    private Button _btnOk;
    private ComboBox _cboTextureFileType;
    private Label _lblTextureFileType;
    private GroupBox groupBox1;

    private void MarkChanged()
    {
      this._bChanged = true;
      this._btnApply.Enabled = true;
    }

    public Settings(MainForm mf)
    {
      this.InitializeComponent();
      this._objMainForm = mf;
      this.ClearUI();
      Globals.SetToolTip(new List<Control>()
      {
        (Control) this._txtBlenderPath,
        (Control) this._btnBlenderPath,
        (Control) this._lblBlenderPath
      }, "Folder ocation of the Blender.exe.  When set to AUTO this will search for the latest installation of blender in %ProgramW6432%/Blender Foundation/Blender, if not found it'll search in %ProgramFiles(x86)%/Blender Foundation/Blender");
      Globals.SetToolTip(new List<Control>()
      {
        (Control) this._txtTempFolder,
        (Control) this._lblTempFolder,
        (Control) this._btnTempFolder
      }, "Temporary location where rendered images get stored.");
      Globals.SetToolTip(new List<Control>()
      {
        (Control) this._txtProjectRoot,
        (Control) this._lblProjectRoot,
        (Control) this._btnProjectRoot
      }, "Root directory of your project.  This is used as the default location for loading new sprites.");
      Globals.SetToolTip(new List<Control>()
      {
        (Control) this._txtPythonScriptPath
      }, "Location of the Python Script.");
      Globals.SetToolTip(new List<Control>()
      {
        (Control) this._nudGrowBy
      }, "Number of pixels to grow the texture when creating the Mega Texture.");
      Globals.SetToolTip(new List<Control>()
      {
        (Control) this._nudMaxSize
      }, "Maximum size of a texture.  If textures become greater than this, then we will split the texture up.");
      Globals.SetToolTip(new List<Control>()
      {
        (Control) this._nudMinSize
      }, "Starting size of the texture.  Setting this higher may increase performance (slightly).");
      Globals.SetToolTip(new List<Control>()
      {
        (Control) this._chkPackImages
      }, "Check to pack image data along with other data in the .ip file.");
      Globals.SetToolTip(new List<Control>()
      {
        (Control) this._chkSaveImagesDebug
      }, "Regardles of whether we pack textures, save the images for viewing.  If Save Images is unchecked, and Pack Images is checked, then no visible texture will be saved with the .IP file.");
      Globals.SetToolTip(new List<Control>()
      {
        (Control) this._btnClearTemp
      }, "Delete the temp (cache) folder.");
    }

    public new void Show()
    {
      this.LoadData();
      base.Show();
      this.BringToFront();
    }

    private void LoadData()
    {
      this._txtBlenderPath.Text = this._objMainForm.IsoPackFile.AppSettings.BlenderPath;
      this._txtTempFolder.Text = this._objMainForm.IsoPackFile.AppSettings.TempFolder;
      this._txtProjectRoot.Text = this._objMainForm.IsoPackFile.AppSettings.ProjectRoot;
      this._txtPythonScriptPath.Text = this._objMainForm.IsoPackFile.AppSettings.PythonScriptPath;
      this._nudGrowBy.Value = (Decimal) this._objMainForm.IsoPackFile.AppSettings.GrowBy;
      this._nudMaxSize.Value = (Decimal) this._objMainForm.IsoPackFile.AppSettings.MaxSize;
      this._nudMinSize.Value = (Decimal) this._objMainForm.IsoPackFile.AppSettings.MinSize;
      this._chkPackImages.Checked = this._objMainForm.IsoPackFile.AppSettings.PackImages;
      this._chkSaveImagesDebug.Checked = this._objMainForm.IsoPackFile.AppSettings.SaveImagesDebug;
      if (this._objMainForm.IsoPackFile.AppSettings.TextureFileType == TextureFileType.Png)
      {
        this._cboTextureFileType.SelectedItem = (object) ".png";
      }
      else
      {
        if (this._objMainForm.IsoPackFile.AppSettings.TextureFileType != TextureFileType.Jpg)
          return;
        this._cboTextureFileType.SelectedItem = (object) ".jpg";
      }
    }

    private void SaveData()
    {
      this._objMainForm.IsoPackFile.AppSettings.BlenderPath = this._txtBlenderPath.Text;
      this._objMainForm.IsoPackFile.AppSettings.TempFolder = this._txtTempFolder.Text;
      this._objMainForm.IsoPackFile.AppSettings.ProjectRoot = this._txtProjectRoot.Text;
      this._objMainForm.IsoPackFile.AppSettings.PythonScriptPath = this._txtPythonScriptPath.Text;
      this._objMainForm.IsoPackFile.AppSettings.GrowBy = (int) this._nudGrowBy.Value;
      this._objMainForm.IsoPackFile.AppSettings.MaxSize = (int) this._nudMaxSize.Value;
      this._objMainForm.IsoPackFile.AppSettings.MinSize = (int) this._nudMinSize.Value;
      this._objMainForm.IsoPackFile.AppSettings.PackImages = this._chkPackImages.Checked;
      this._objMainForm.IsoPackFile.AppSettings.SaveImagesDebug = this._chkSaveImagesDebug.Checked;
      this._objMainForm.IsoPackFile.AppSettings.TextureFileType = this._cboTextureFileType.SelectedText == ".png" ? TextureFileType.Png : TextureFileType.Jpg;
      this._bChanged = false;
      this._btnApply.Enabled = false;
    }

    public void ClearUI()
    {
      this.LoadData();
    }

    public void Log(string str)
    {
      this._txtLog.Text = this._txtLog.Text + str + "\r\n";
      this._txtLog.SelectionStart = this._txtLog.TextLength;
      this._txtLog.ScrollToCaret();
    }

    private void _btnClose_Click(object sender, EventArgs e)
    {
      this.Hide();
    }

    private void label1_Click(object sender, EventArgs e)
    {
    }

    private void _btnClearLog_Click(object sender, EventArgs e)
    {
      this._txtLog.Text = "";
    }

    private void Settings_FormClosing(object sender, FormClosingEventArgs e)
    {
      if (e.CloseReason != CloseReason.UserClosing)
        return;
      e.Cancel = true;
      this.Hide();
    }

    private void numericUpDown1_ValueChanged(object sender, EventArgs e)
    {
      this.MarkChanged();
    }

    private void label7_Click(object sender, EventArgs e)
    {
    }

    private void _btnClearTemp_Click(object sender, EventArgs e)
    {
      string fullPath = Path.GetFullPath(this._objMainForm.IsoPackFile.AppSettings.GetOrCreateTempFolder());
      if (Directory.Exists(fullPath))
      {
        if (MessageBox.Show("You are about to delete the contents of '" + fullPath + "'.  Continue?", "Delete Folder", MessageBoxButtons.OKCancel) != DialogResult.OK)
          return;
        try
        {
          Directory.Delete(fullPath, true);
        }
        catch (Exception ex)
        {
          string str = "Error deleting '" + fullPath + "'.  You may have a folder or file open which belongs to that directory.  Close all folders or files which are in that directory, and try again.";
          int num = (int) MessageBox.Show(str, "Delete Folder", MessageBoxButtons.OK);
          this._objMainForm.Log(str);
        }
      }
      else
        this._objMainForm.Log("ERROR! " + fullPath + " does not exist..");
    }

    private void _btnApply_Click(object sender, EventArgs e)
    {
      this.SaveData();
    }

    private void _btnOk_Click(object sender, EventArgs e)
    {
      this.SaveData();
      this.Hide();
    }

    private void Settings_Load(object sender, EventArgs e)
    {
      this._lblVersion.Text = string.Format("v{0:00}", (object) this._objMainForm.GetProgramVersion());
    }

    private void _nudMinSize_ValueChanged(object sender, EventArgs e)
    {
      if ((int) this._nudMinSize.Value != this._objMainForm.IsoPackFile.AppSettings.MinSize)
        this.MarkChanged();
      if (!(this._nudMinSize.Value > this._nudMaxSize.Value))
        return;
      this._nudMaxSize.Value = this._nudMinSize.Value;
    }

    private void _nudMaxSize_ValueChanged(object sender, EventArgs e)
    {
      if ((int) this._nudMaxSize.Value != this._objMainForm.IsoPackFile.AppSettings.MaxSize)
        this.MarkChanged();
      if (!(this._nudMaxSize.Value < this._nudMinSize.Value))
        return;
      this._nudMinSize.Value = this._nudMaxSize.Value;
    }

    private void _nudGrowBy_ValueChanged(object sender, EventArgs e)
    {
      if ((int) this._nudGrowBy.Value == this._objMainForm.IsoPackFile.AppSettings.GrowBy)
        return;
      this.MarkChanged();
    }

    private void _txtPythonScriptPath_TextChanged(object sender, EventArgs e)
    {
      if (!(this._txtPythonScriptPath.Text != this._objMainForm.IsoPackFile.AppSettings.PythonScriptPath))
        return;
      this.MarkChanged();
    }

    private void _txtProjectRoot_TextChanged(object sender, EventArgs e)
    {
      if (!(this._txtProjectRoot.Text != this._objMainForm.IsoPackFile.AppSettings.ProjectRoot))
        return;
      this.MarkChanged();
    }

    private void _txtTempFolder_TextChanged(object sender, EventArgs e)
    {
      if (!(this._txtTempFolder.Text != this._objMainForm.IsoPackFile.AppSettings.TempFolder))
        return;
      this.MarkChanged();
    }

    private void _txtBlenderPath_TextChanged(object sender, EventArgs e)
    {
      if (!(this._txtBlenderPath.Text != this._objMainForm.IsoPackFile.AppSettings.BlenderPath))
        return;
      this.MarkChanged();
    }

    private void _chkPackImages_CheckedChanged(object sender, EventArgs e)
    {
      if (this._chkPackImages.Checked == this._objMainForm.IsoPackFile.AppSettings.PackImages)
        return;
      this.MarkChanged();
    }

    private void _chkSaveImagesDebug_CheckedChanged(object sender, EventArgs e)
    {
      if (this._chkSaveImagesDebug.Checked == this._objMainForm.IsoPackFile.AppSettings.SaveImagesDebug)
        return;
      this.MarkChanged();
    }

    private void _cboTextureFileType_SelectedIndexChanged(object sender, EventArgs e)
    {
      if ((!(this._cboTextureFileType.SelectedText == ".png") || this._objMainForm.IsoPackFile.AppSettings.TextureFileType == TextureFileType.Png) && (!(this._cboTextureFileType.SelectedText == ".jpg") || this._objMainForm.IsoPackFile.AppSettings.TextureFileType == TextureFileType.Jpg))
        return;
      this.MarkChanged();
    }

    private void button2_Click(object sender, EventArgs e)
    {
      string[] openSaveUserFile = Globals.GetValidOpenSaveUserFile(false, Globals.ExeFilter, Globals.ExeFilter, "exe", this._objMainForm.IsoPackFile.AppSettings.ProjectRoot, false);
      if (openSaveUserFile.Length != 1)
        return;
      this._txtBlenderPath.Text = openSaveUserFile[0];
      this.MarkChanged();
    }

    private void _btnTempFolder_Click(object sender, EventArgs e)
    {
      string[] validUserFolder = Globals.GetValidUserFolder(this._objMainForm.IsoPackFile.AppSettings.ProjectRoot, false);
      if (validUserFolder.Length != 1)
        return;
      this._txtTempFolder.Text = validUserFolder[0];
      this.MarkChanged();
    }

    private void _btnProjectRoot_Click(object sender, EventArgs e)
    {
      string[] validUserFolder = Globals.GetValidUserFolder(this._objMainForm.IsoPackFile.AppSettings.ProjectRoot, false);
      if (validUserFolder.Length != 1)
        return;
      this._txtProjectRoot.Text = validUserFolder[0];
      this.MarkChanged();
    }

    private void _btnPythonScriptPath_Click(object sender, EventArgs e)
    {
      string[] validUserFolder = Globals.GetValidUserFolder(this._objMainForm.IsoPackFile.AppSettings.ProjectRoot, false);
      if (validUserFolder.Length != 1)
        return;
      this._txtPythonScriptPath.Text = validUserFolder[0];
      this.MarkChanged();
    }

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof (Settings));
      this._txtLog = new TextBox();
      this.label1 = new Label();
      this.label2 = new Label();
      this.label3 = new Label();
      this._txtBlenderPath = new TextBox();
      this._lblBlenderPath = new Label();
      this._btnBlenderPath = new Button();
      this._txtTempFolder = new TextBox();
      this._lblTempFolder = new Label();
      this._btnTempFolder = new Button();
      this._btnClose = new Button();
      this._txtProjectRoot = new TextBox();
      this._lblProjectRoot = new Label();
      this._btnProjectRoot = new Button();
      this._txtPythonScriptPath = new TextBox();
      this.label7 = new Label();
      this._btnPythonScriptPath = new Button();
      this.label8 = new Label();
      this._lblVersion = new Label();
      this._btnClearLog = new Button();
      this._nudMaxSize = new NumericUpDown();
      this._lblMaxSize = new Label();
      this._lblMinSize = new Label();
      this._nudMinSize = new NumericUpDown();
      this._lblGrowBy = new Label();
      this._nudGrowBy = new NumericUpDown();
      this.label10 = new Label();
      this.label11 = new Label();
      this.label12 = new Label();
      this._chkPackImages = new CheckBox();
      this._chkSaveImagesDebug = new CheckBox();
      this._btnClearTemp = new Button();
      this._btnApply = new Button();
      this._btnOk = new Button();
      this._cboTextureFileType = new ComboBox();
      this._lblTextureFileType = new Label();
      this.groupBox1 = new GroupBox();
      this._nudMaxSize.BeginInit();
      this._nudMinSize.BeginInit();
      this._nudGrowBy.BeginInit();
      this.groupBox1.SuspendLayout();
      this.SuspendLayout();
      this._txtLog.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
      this._txtLog.Font = new Font("Lucida Console", 9.75f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this._txtLog.Location = new Point(15, 361);
      this._txtLog.Multiline = true;
      this._txtLog.Name = "_txtLog";
      this._txtLog.ScrollBars = ScrollBars.Vertical;
      this._txtLog.Size = new Size(631, 163);
      this._txtLog.TabIndex = 0;
      this.label1.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
      this.label1.AutoSize = true;
      this.label1.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Italic, GraphicsUnit.Point, (byte) 0);
      this.label1.Location = new Point(12, 617);
      this.label1.Name = "label1";
      this.label1.Size = new Size(46, 13);
      this.label1.TabIndex = 1;
      this.label1.Text = "IsoPack";
      this.label1.Click += new EventHandler(this.label1_Click);
      this.label2.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
      this.label2.AutoSize = true;
      this.label2.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Italic, GraphicsUnit.Point, (byte) 0);
      this.label2.Location = new Point(191, 617);
      this.label2.Name = "label2";
      this.label2.Size = new Size(111, 13);
      this.label2.TabIndex = 1;
      this.label2.Text = "Armor Monkey Games";
      this.label3.AutoSize = true;
      this.label3.Font = new Font("Microsoft Sans Serif", 9.75f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.label3.Location = new Point(18, 339);
      this.label3.Name = "label3";
      this.label3.Size = new Size(31, 16);
      this.label3.TabIndex = 1;
      this.label3.Text = "Log";
      this._txtBlenderPath.Font = new Font("Microsoft Sans Serif", 9.75f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this._txtBlenderPath.Location = new Point(163, 19);
      this._txtBlenderPath.Name = "_txtBlenderPath";
      this._txtBlenderPath.Size = new Size(475, 22);
      this._txtBlenderPath.TabIndex = 4;
      this._txtBlenderPath.Text = "AUTO";
      this._txtBlenderPath.TextChanged += new EventHandler(this._txtBlenderPath_TextChanged);
      this._lblBlenderPath.AutoSize = true;
      this._lblBlenderPath.Font = new Font("Microsoft Sans Serif", 9.75f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this._lblBlenderPath.Location = new Point(12, 22);
      this._lblBlenderPath.Name = "_lblBlenderPath";
      this._lblBlenderPath.Size = new Size(114, 16);
      this._lblBlenderPath.TabIndex = 5;
      this._lblBlenderPath.Text = "Blender EXE Path";
      this._btnBlenderPath.Font = new Font("Microsoft Sans Serif", 9.75f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this._btnBlenderPath.Location = new Point(129, 19);
      this._btnBlenderPath.Name = "_btnBlenderPath";
      this._btnBlenderPath.Size = new Size(28, 23);
      this._btnBlenderPath.TabIndex = 6;
      this._btnBlenderPath.Text = "...";
      this._btnBlenderPath.UseVisualStyleBackColor = true;
      this._btnBlenderPath.Click += new EventHandler(this.button2_Click);
      this._txtTempFolder.Font = new Font("Microsoft Sans Serif", 9.75f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this._txtTempFolder.Location = new Point(149, 51);
      this._txtTempFolder.Name = "_txtTempFolder";
      this._txtTempFolder.Size = new Size(392, 22);
      this._txtTempFolder.TabIndex = 4;
      this._txtTempFolder.Text = ".\\tmp";
      this._txtTempFolder.TextChanged += new EventHandler(this._txtTempFolder_TextChanged);
      this._lblTempFolder.AutoSize = true;
      this._lblTempFolder.Font = new Font("Microsoft Sans Serif", 9.75f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this._lblTempFolder.Location = new Point(12, 54);
      this._lblTempFolder.Name = "_lblTempFolder";
      this._lblTempFolder.Size = new Size(86, 16);
      this._lblTempFolder.TabIndex = 5;
      this._lblTempFolder.Text = "Temp Folder";
      this._btnTempFolder.Font = new Font("Microsoft Sans Serif", 9.75f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this._btnTempFolder.Location = new Point(115, 51);
      this._btnTempFolder.Name = "_btnTempFolder";
      this._btnTempFolder.Size = new Size(28, 23);
      this._btnTempFolder.TabIndex = 6;
      this._btnTempFolder.Text = "...";
      this._btnTempFolder.UseVisualStyleBackColor = true;
      this._btnTempFolder.Click += new EventHandler(this._btnTempFolder_Click);
      this._btnClose.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
      this._btnClose.Font = new Font("Microsoft Sans Serif", 9.75f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this._btnClose.Location = new Point(362, 542);
      this._btnClose.Name = "_btnClose";
      this._btnClose.Size = new Size(86, 31);
      this._btnClose.TabIndex = 7;
      this._btnClose.Text = "Cancel";
      this._btnClose.UseVisualStyleBackColor = true;
      this._btnClose.Click += new EventHandler(this._btnClose_Click);
      this._txtProjectRoot.Font = new Font("Microsoft Sans Serif", 9.75f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this._txtProjectRoot.Location = new Point(129, 84);
      this._txtProjectRoot.Name = "_txtProjectRoot";
      this._txtProjectRoot.Size = new Size(509, 22);
      this._txtProjectRoot.TabIndex = 4;
      this._txtProjectRoot.Text = ".\\";
      this._txtProjectRoot.TextChanged += new EventHandler(this._txtProjectRoot_TextChanged);
      this._lblProjectRoot.AutoSize = true;
      this._lblProjectRoot.Font = new Font("Microsoft Sans Serif", 9.75f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this._lblProjectRoot.Location = new Point(12, 87);
      this._lblProjectRoot.Name = "_lblProjectRoot";
      this._lblProjectRoot.Size = new Size(82, 16);
      this._lblProjectRoot.TabIndex = 5;
      this._lblProjectRoot.Text = "Project Root";
      this._btnProjectRoot.Font = new Font("Microsoft Sans Serif", 9.75f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this._btnProjectRoot.Location = new Point(98, 84);
      this._btnProjectRoot.Name = "_btnProjectRoot";
      this._btnProjectRoot.Size = new Size(28, 23);
      this._btnProjectRoot.TabIndex = 6;
      this._btnProjectRoot.Text = "...";
      this._btnProjectRoot.UseVisualStyleBackColor = true;
      this._btnProjectRoot.Click += new EventHandler(this._btnProjectRoot_Click);
      this._txtPythonScriptPath.Font = new Font("Microsoft Sans Serif", 9.75f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this._txtPythonScriptPath.Location = new Point(168, 116);
      this._txtPythonScriptPath.Name = "_txtPythonScriptPath";
      this._txtPythonScriptPath.Size = new Size(470, 22);
      this._txtPythonScriptPath.TabIndex = 4;
      this._txtPythonScriptPath.Text = ".\\iso_export.py";
      this._txtPythonScriptPath.Visible = false;
      this._txtPythonScriptPath.TextChanged += new EventHandler(this._txtPythonScriptPath_TextChanged);
      this.label7.AutoSize = true;
      this.label7.Font = new Font("Microsoft Sans Serif", 9.75f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.label7.Location = new Point(12, 119);
      this.label7.Name = "label7";
      this.label7.Size = new Size(116, 16);
      this.label7.TabIndex = 5;
      this.label7.Text = "Python Script Path";
      this.label7.Visible = false;
      this.label7.Click += new EventHandler(this.label7_Click);
      this._btnPythonScriptPath.Font = new Font("Microsoft Sans Serif", 9.75f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this._btnPythonScriptPath.Location = new Point(134, 116);
      this._btnPythonScriptPath.Name = "_btnPythonScriptPath";
      this._btnPythonScriptPath.Size = new Size(28, 23);
      this._btnPythonScriptPath.TabIndex = 6;
      this._btnPythonScriptPath.Text = "...";
      this._btnPythonScriptPath.UseVisualStyleBackColor = true;
      this._btnPythonScriptPath.Visible = false;
      this._btnPythonScriptPath.Click += new EventHandler(this._btnPythonScriptPath_Click);
      this.label8.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
      this.label8.AutoSize = true;
      this.label8.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Italic, GraphicsUnit.Point, (byte) 0);
      this.label8.Location = new Point(121, 617);
      this.label8.Name = "label8";
      this.label8.Size = new Size(64, 13);
      this.label8.TabIndex = 1;
      this.label8.Text = "Derek Page";
      this._lblVersion.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
      this._lblVersion.AutoSize = true;
      this._lblVersion.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Italic, GraphicsUnit.Point, (byte) 0);
      this._lblVersion.Location = new Point(64, 617);
      this._lblVersion.Name = "_lblVersion";
      this._lblVersion.Size = new Size(13, 13);
      this._lblVersion.TabIndex = 1;
      this._lblVersion.Text = "v";
      this._lblVersion.Click += new EventHandler(this.label1_Click);
      this._btnClearLog.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
      this._btnClearLog.Font = new Font("Microsoft Sans Serif", 9.75f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this._btnClearLog.Location = new Point(570, 530);
      this._btnClearLog.Name = "_btnClearLog";
      this._btnClearLog.Size = new Size(76, 24);
      this._btnClearLog.TabIndex = 7;
      this._btnClearLog.Text = "Clear Log";
      this._btnClearLog.UseVisualStyleBackColor = true;
      this._btnClearLog.Click += new EventHandler(this._btnClearLog_Click);
      this._nudMaxSize.Font = new Font("Microsoft Sans Serif", 9.75f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this._nudMaxSize.Location = new Point(80, 63);
      this._nudMaxSize.Maximum = new Decimal(new int[4]
      {
        9999999,
        0,
        0,
        0
      });
      this._nudMaxSize.Minimum = new Decimal(new int[4]
      {
        1,
        0,
        0,
        0
      });
      this._nudMaxSize.Name = "_nudMaxSize";
      this._nudMaxSize.Size = new Size(48, 22);
      this._nudMaxSize.TabIndex = 8;
      this._nudMaxSize.Value = new Decimal(new int[4]
      {
        2048,
        0,
        0,
        0
      });
      this._nudMaxSize.ValueChanged += new EventHandler(this._nudMaxSize_ValueChanged);
      this._lblMaxSize.AutoSize = true;
      this._lblMaxSize.Font = new Font("Microsoft Sans Serif", 9.75f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this._lblMaxSize.Location = new Point(16, 65);
      this._lblMaxSize.Name = "_lblMaxSize";
      this._lblMaxSize.Size = new Size(62, 16);
      this._lblMaxSize.TabIndex = 5;
      this._lblMaxSize.Text = "Max Size";
      this._lblMaxSize.Click += new EventHandler(this.label7_Click);
      this._lblMinSize.AutoSize = true;
      this._lblMinSize.Font = new Font("Microsoft Sans Serif", 9.75f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this._lblMinSize.Location = new Point(16, 42);
      this._lblMinSize.Name = "_lblMinSize";
      this._lblMinSize.Size = new Size(58, 16);
      this._lblMinSize.TabIndex = 5;
      this._lblMinSize.Text = "Min Size";
      this._lblMinSize.Click += new EventHandler(this.label7_Click);
      this._nudMinSize.Font = new Font("Microsoft Sans Serif", 9.75f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this._nudMinSize.Location = new Point(80, 40);
      this._nudMinSize.Maximum = new Decimal(new int[4]
      {
        9999999,
        0,
        0,
        0
      });
      this._nudMinSize.Minimum = new Decimal(new int[4]
      {
        1,
        0,
        0,
        0
      });
      this._nudMinSize.Name = "_nudMinSize";
      this._nudMinSize.Size = new Size(48, 22);
      this._nudMinSize.TabIndex = 8;
      this._nudMinSize.Value = new Decimal(new int[4]
      {
        256,
        0,
        0,
        0
      });
      this._nudMinSize.ValueChanged += new EventHandler(this._nudMinSize_ValueChanged);
      this._lblGrowBy.AutoSize = true;
      this._lblGrowBy.Font = new Font("Microsoft Sans Serif", 9.75f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this._lblGrowBy.Location = new Point(20, 91);
      this._lblGrowBy.Name = "_lblGrowBy";
      this._lblGrowBy.Size = new Size(58, 16);
      this._lblGrowBy.TabIndex = 5;
      this._lblGrowBy.Text = "Grow By";
      this._lblGrowBy.Click += new EventHandler(this.label7_Click);
      this._nudGrowBy.Font = new Font("Microsoft Sans Serif", 9.75f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this._nudGrowBy.Location = new Point(80, 89);
      this._nudGrowBy.Maximum = new Decimal(new int[4]
      {
        9999999,
        0,
        0,
        0
      });
      this._nudGrowBy.Minimum = new Decimal(new int[4]
      {
        1,
        0,
        0,
        0
      });
      this._nudGrowBy.Name = "_nudGrowBy";
      this._nudGrowBy.Size = new Size(48, 22);
      this._nudGrowBy.TabIndex = 8;
      this._nudGrowBy.Value = new Decimal(new int[4]
      {
        256,
        0,
        0,
        0
      });
      this._nudGrowBy.ValueChanged += new EventHandler(this._nudGrowBy_ValueChanged);
      this.label10.AutoSize = true;
      this.label10.Font = new Font("Microsoft Sans Serif", 9.75f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.label10.Location = new Point(134, 91);
      this.label10.Name = "label10";
      this.label10.Size = new Size(22, 16);
      this.label10.TabIndex = 5;
      this.label10.Text = "px";
      this.label10.Click += new EventHandler(this.label7_Click);
      this.label11.AutoSize = true;
      this.label11.Font = new Font("Microsoft Sans Serif", 9.75f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.label11.Location = new Point(134, 42);
      this.label11.Name = "label11";
      this.label11.Size = new Size(22, 16);
      this.label11.TabIndex = 5;
      this.label11.Text = "px";
      this.label11.Click += new EventHandler(this.label7_Click);
      this.label12.AutoSize = true;
      this.label12.Font = new Font("Microsoft Sans Serif", 9.75f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.label12.Location = new Point(134, 65);
      this.label12.Name = "label12";
      this.label12.Size = new Size(22, 16);
      this.label12.TabIndex = 5;
      this.label12.Text = "px";
      this.label12.Click += new EventHandler(this.label7_Click);
      this._chkPackImages.AutoSize = true;
      this._chkPackImages.Location = new Point(209, 29);
      this._chkPackImages.Name = "_chkPackImages";
      this._chkPackImages.Size = new Size(106, 20);
      this._chkPackImages.TabIndex = 9;
      this._chkPackImages.Text = "Pack Images";
      this._chkPackImages.UseVisualStyleBackColor = true;
      this._chkPackImages.CheckedChanged += new EventHandler(this._chkPackImages_CheckedChanged);
      this._chkSaveImagesDebug.AutoSize = true;
      this._chkSaveImagesDebug.Location = new Point(209, 52);
      this._chkSaveImagesDebug.Name = "_chkSaveImagesDebug";
      this._chkSaveImagesDebug.Size = new Size(236, 20);
      this._chkSaveImagesDebug.TabIndex = 9;
      this._chkSaveImagesDebug.Text = "Save Images Anyway (For Testing)";
      this._chkSaveImagesDebug.UseVisualStyleBackColor = true;
      this._chkSaveImagesDebug.CheckedChanged += new EventHandler(this._chkSaveImagesDebug_CheckedChanged);
      this._btnClearTemp.Font = new Font("Microsoft Sans Serif", 9.75f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this._btnClearTemp.Location = new Point(547, 51);
      this._btnClearTemp.Name = "_btnClearTemp";
      this._btnClearTemp.Size = new Size(75, 22);
      this._btnClearTemp.TabIndex = 10;
      this._btnClearTemp.Text = "Clear";
      this._btnClearTemp.UseVisualStyleBackColor = true;
      this._btnClearTemp.Click += new EventHandler(this._btnClearTemp_Click);
      this._btnApply.Enabled = false;
      this._btnApply.Font = new Font("Microsoft Sans Serif", 9.75f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this._btnApply.Location = new Point(270, 542);
      this._btnApply.Name = "_btnApply";
      this._btnApply.Size = new Size(86, 31);
      this._btnApply.TabIndex = 7;
      this._btnApply.Text = "Apply";
      this._btnApply.UseVisualStyleBackColor = true;
      this._btnApply.Click += new EventHandler(this._btnApply_Click);
      this._btnOk.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
      this._btnOk.Font = new Font("Microsoft Sans Serif", 9.75f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this._btnOk.Location = new Point(178, 542);
      this._btnOk.Name = "_btnOk";
      this._btnOk.Size = new Size(86, 31);
      this._btnOk.TabIndex = 7;
      this._btnOk.Text = "Ok";
      this._btnOk.UseVisualStyleBackColor = true;
      this._btnOk.Click += new EventHandler(this._btnOk_Click);
      this._cboTextureFileType.Font = new Font("Microsoft Sans Serif", 9.75f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this._cboTextureFileType.FormattingEnabled = true;
      this._cboTextureFileType.Items.AddRange(new object[2]
      {
        (object) ".png",
        (object) ".jpg"
      });
      this._cboTextureFileType.Location = new Point(273, 92);
      this._cboTextureFileType.Name = "_cboTextureFileType";
      this._cboTextureFileType.Size = new Size(69, 24);
      this._cboTextureFileType.TabIndex = 11;
      this._cboTextureFileType.SelectedIndexChanged += new EventHandler(this._cboTextureFileType_SelectedIndexChanged);
      this._lblTextureFileType.AutoSize = true;
      this._lblTextureFileType.Font = new Font("Microsoft Sans Serif", 9.75f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this._lblTextureFileType.Location = new Point(206, 95);
      this._lblTextureFileType.Name = "_lblTextureFileType";
      this._lblTextureFileType.Size = new Size(65, 16);
      this._lblTextureFileType.TabIndex = 12;
      this._lblTextureFileType.Text = "File Type";
      this.groupBox1.Controls.Add((Control) this._lblMinSize);
      this.groupBox1.Controls.Add((Control) this._lblTextureFileType);
      this.groupBox1.Controls.Add((Control) this._chkSaveImagesDebug);
      this.groupBox1.Controls.Add((Control) this._cboTextureFileType);
      this.groupBox1.Controls.Add((Control) this._chkPackImages);
      this.groupBox1.Controls.Add((Control) this._lblMaxSize);
      this.groupBox1.Controls.Add((Control) this._lblGrowBy);
      this.groupBox1.Controls.Add((Control) this.label10);
      this.groupBox1.Controls.Add((Control) this.label11);
      this.groupBox1.Controls.Add((Control) this.label12);
      this.groupBox1.Controls.Add((Control) this._nudGrowBy);
      this.groupBox1.Controls.Add((Control) this._nudMaxSize);
      this.groupBox1.Controls.Add((Control) this._nudMinSize);
      this.groupBox1.Font = new Font("Microsoft Sans Serif", 9.75f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.groupBox1.Location = new Point(15, 157);
      this.groupBox1.Name = "groupBox1";
      this.groupBox1.Size = new Size(470, 158);
      this.groupBox1.TabIndex = 13;
      this.groupBox1.TabStop = false;
      this.groupBox1.Text = "Texture Settings";
      this.AutoScaleDimensions = new SizeF(6f, 13f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.ClientSize = new Size(656, 639);
      this.Controls.Add((Control) this.groupBox1);
      this.Controls.Add((Control) this._btnClearTemp);
      this.Controls.Add((Control) this._btnClearLog);
      this.Controls.Add((Control) this._btnOk);
      this.Controls.Add((Control) this._btnApply);
      this.Controls.Add((Control) this._btnClose);
      this.Controls.Add((Control) this.label8);
      this.Controls.Add((Control) this.label2);
      this.Controls.Add((Control) this._lblVersion);
      this.Controls.Add((Control) this.label1);
      this.Controls.Add((Control) this._btnPythonScriptPath);
      this.Controls.Add((Control) this._btnProjectRoot);
      this.Controls.Add((Control) this._btnTempFolder);
      this.Controls.Add((Control) this._btnBlenderPath);
      this.Controls.Add((Control) this.label7);
      this.Controls.Add((Control) this._lblProjectRoot);
      this.Controls.Add((Control) this._lblTempFolder);
      this.Controls.Add((Control) this._txtPythonScriptPath);
      this.Controls.Add((Control) this._txtProjectRoot);
      this.Controls.Add((Control) this._txtTempFolder);
      this.Controls.Add((Control) this._lblBlenderPath);
      this.Controls.Add((Control) this._txtBlenderPath);
      this.Controls.Add((Control) this.label3);
      this.Controls.Add((Control) this._txtLog);
      this.Icon = (Icon) componentResourceManager.GetObject("$this.Icon");
      this.Name = nameof (Settings);
      this.Text = nameof (Settings);
      this.FormClosing += new FormClosingEventHandler(this.Settings_FormClosing);
      this.Load += new EventHandler(this.Settings_Load);
      this._nudMaxSize.EndInit();
      this._nudMinSize.EndInit();
      this._nudGrowBy.EndInit();
      this.groupBox1.ResumeLayout(false);
      this.groupBox1.PerformLayout();
      this.ResumeLayout(false);
      this.PerformLayout();
    }
  }
}
