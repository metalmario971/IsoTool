// Decompiled with JetBrains decompiler
// Type: IsoPack.AddEditFrame
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
  public class AddEditFrame : AddEditForm
  {
    private MainForm _objMainForm = (MainForm) null;
    private PictureBoxWithInterpolationMode FrameBox = new PictureBoxWithInterpolationMode();
    private bool _bChanged = false;
    private IContainer components = (IContainer) null;
    private Sprite _objSprite;
    private PictureBox _pbFrame;
    private Button _btnOk;
    private Button _btnApply;
    private Button _btnCancel;
    private TextBox _txtName;
    private Label _lblName;
    private TextBox _txtLocation;
    private Button _btnLocation;
    private Button _btnLoad;
    private Label _lblLocation;
    private NumericUpDown _nudDelay;
    private Label _lblDelay;
    private Label label2;
    private Label _lblFrameIndex0;
    private Label _lblFrameIndex1;
    private MenuStrip menuStrip1;
    private ToolStripMenuItem toolsToolStripMenuItem;
    private ToolStripMenuItem hueSaturationToolStripMenuItem;

    public Frame Frame { get; private set; } = (Frame) null;

    public AddEditFrame(MainForm mf)
    {
      this.InitializeComponent();
      this._objMainForm = mf;
      Globals.SwapSpriteControl(this._pbFrame, this.FrameBox);
      Globals.SetToolTip(new List<Control>()
      {
        (Control) this._btnLocation,
        (Control) this._btnLoad,
        (Control) this._txtLocation,
        (Control) this._lblLocation
      }, "Click the ellipsis to load an image from your computer.");
      Globals.SetToolTip(new List<Control>()
      {
        (Control) this._txtName
      }, "The name of this frame image.");
      Globals.SetToolTip(new List<Control>()
      {
        (Control) this._nudDelay,
        (Control) this._lblDelay
      }, "Animation delay in milliseconds (1000 each second).  This is the amount of time this frame is visible before the next frame shows, and so on.");
      Globals.SetToolTip(new List<Control>()
      {
        (Control) this._lblFrameIndex0,
        (Control) this._lblFrameIndex1
      }, "The sort order of this frame in it's parent sprite.");
    }

    public void Show(Frame f, Sprite s)
    {
      if (s == null)
        throw new Exception("Sprite was null");
      this._objSprite = s;
      this._bChanged = false;
      if (f == null)
      {
        this.FormMode = FormMode.Add;
        this.Text = "Add Model";
        this.Frame = new Frame();
        this.Frame.Name = Globals.GetNewFrameName(this._objMainForm);
        this.Frame.Sprite = s;
      }
      else
      {
        this.FormMode = FormMode.Edit;
        this.Text = "Edit Frame";
        this.Frame = f;
      }
      this.LoadData();
      this.Show();
      this.BringToFront();
      if (!(this._txtLocation.Text != ""))
        return;
      this.CheckFrameExists();
    }

    private void LoadData()
    {
      this._txtName.Text = this.Frame.Name;
      this._txtLocation.Text = this.Frame.Location;
      this._nudDelay.Value = (Decimal) this.Frame.Delay;
      this._lblFrameIndex1.Text = this.Frame.FrameId.ToString();
      this.UpdateUI(false);
    }

    public override void UpdateUI(bool bDirty)
    {
      if (this.Frame.ImageTemp != null)
        this.FrameBox.Image = (Image) this.Frame.ImageTemp.Image;
      else
        this.FrameBox.Image = (Image) null;
      this.CheckFrameExists();
      if (!bDirty)
        return;
      this.MarkChanged();
    }

    private void SaveData()
    {
      this.Frame.Name = this._txtName.Text;
      this.Frame.Location = this._txtLocation.Text;
      this.Frame.Delay = (int) this._nudDelay.Value;
      bool bDirty = false;
      if (this.FormMode == FormMode.Add)
      {
        bDirty = true;
        this._objSprite.Frames.Add(this.Frame);
        this.FormMode = FormMode.Edit;
        this.Text = "Edit Frame";
      }
      this._objMainForm.UpdateWindowUI(this._objSprite, bDirty);
      this._bChanged = false;
      this._btnApply.Enabled = false;
    }

    private bool CheckFrameExists()
    {
      if (!this._txtLocation.Visible || !this._txtLocation.Enabled)
        return false;
      if (!File.Exists(this._txtLocation.Text))
      {
        this._txtLocation.BackColor = Color.LightCoral;
        return false;
      }
      this._txtLocation.BackColor = Color.White;
      return true;
    }

    private void AddEditFrame_Load(object sender, EventArgs e)
    {
    }

    private void _btnLocation_Click(object sender, EventArgs e)
    {
      string[] openSaveUserFile = Globals.GetValidOpenSaveUserFile(false, Globals.SupportedLoadSpriteImageFilter, Globals.SupportedLoadSpriteImageFilter, "png", this._objMainForm.IsoPackFile.AppSettings.ProjectRoot, false);
      if ((uint) openSaveUserFile.Length <= 0U)
        return;
      this._txtLocation.Text = openSaveUserFile[0];
    }

    private void MarkChanged()
    {
      this._bChanged = true;
      this._btnApply.Enabled = true;
      this.CheckFrameExists();
    }

    private void _btnApply_Click(object sender, EventArgs e)
    {
      this.SaveData();
    }

    private void _btnLoad_Click(object sender, EventArgs e)
    {
      if (!File.Exists(this._txtLocation.Text))
        return;
      this.Frame.LoadFrameImage(this._txtLocation.Text, this._objMainForm);
      this.UpdateUI(true);
    }

    private void _btnCancel_Click(object sender, EventArgs e)
    {
      this.Frame = (Frame) null;
      this.Close();
    }

    private void AddEditFrame_FormClosing(object sender, FormClosingEventArgs e)
    {
      this._objMainForm.AddEditObject(this.Frame, this._objSprite, true);
    }

    private void _btnOk_Click(object sender, EventArgs e)
    {
      this.SaveData();
      this.Close();
    }

    private void _txtName_TextChanged(object sender, EventArgs e)
    {
      if (!(this._txtName.Text != this.Frame.Name))
        return;
      this.MarkChanged();
    }

    private void _txtLocation_TextChanged(object sender, EventArgs e)
    {
      if (!(this._txtLocation.Text != this.Frame.Location))
        return;
      this.MarkChanged();
    }

    private void _nudDelay_ValueChanged(object sender, EventArgs e)
    {
      if (!(this._nudDelay.Value != (Decimal) this.Frame.Delay))
        return;
      this.MarkChanged();
    }

    private void toolsToolStripMenuItem_Click(object sender, EventArgs e)
    {
    }

    private void hueSaturationToolStripMenuItem_Click(object sender, EventArgs e)
    {
      int num = (int) new HSVDialog()
      {
        Frame = this.Frame,
        AddEditForm = ((AddEditForm) this)
      }.ShowDialog();
    }

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof (AddEditFrame));
      this._pbFrame = new PictureBox();
      this._btnOk = new Button();
      this._btnApply = new Button();
      this._btnCancel = new Button();
      this._txtName = new TextBox();
      this._lblName = new Label();
      this._txtLocation = new TextBox();
      this._btnLocation = new Button();
      this._btnLoad = new Button();
      this._lblLocation = new Label();
      this._nudDelay = new NumericUpDown();
      this._lblDelay = new Label();
      this.label2 = new Label();
      this._lblFrameIndex0 = new Label();
      this._lblFrameIndex1 = new Label();
      this.menuStrip1 = new MenuStrip();
      this.toolsToolStripMenuItem = new ToolStripMenuItem();
      this.hueSaturationToolStripMenuItem = new ToolStripMenuItem();
      ((ISupportInitialize) this._pbFrame).BeginInit();
      this._nudDelay.BeginInit();
      this.menuStrip1.SuspendLayout();
      this.SuspendLayout();
      this._pbFrame.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
      this._pbFrame.BackColor = Color.White;
      this._pbFrame.BorderStyle = BorderStyle.FixedSingle;
      this._pbFrame.Location = new Point(119, 98);
      this._pbFrame.Name = "_pbFrame";
      this._pbFrame.Size = new Size(202, 187);
      this._pbFrame.SizeMode = PictureBoxSizeMode.Zoom;
      this._pbFrame.TabIndex = 0;
      this._pbFrame.TabStop = false;
      this._btnOk.Anchor = AnchorStyles.Bottom;
      this._btnOk.Font = new Font("Microsoft Sans Serif", 9.75f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this._btnOk.Location = new Point(84, 336);
      this._btnOk.Name = "_btnOk";
      this._btnOk.Size = new Size(86, 35);
      this._btnOk.TabIndex = 1;
      this._btnOk.Text = "Ok";
      this._btnOk.UseVisualStyleBackColor = true;
      this._btnOk.Click += new EventHandler(this._btnOk_Click);
      this._btnApply.Anchor = AnchorStyles.Bottom;
      this._btnApply.Enabled = false;
      this._btnApply.Font = new Font("Microsoft Sans Serif", 9.75f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this._btnApply.Location = new Point(176, 336);
      this._btnApply.Name = "_btnApply";
      this._btnApply.Size = new Size(86, 35);
      this._btnApply.TabIndex = 1;
      this._btnApply.Text = "Apply";
      this._btnApply.UseVisualStyleBackColor = true;
      this._btnApply.Click += new EventHandler(this._btnApply_Click);
      this._btnCancel.Anchor = AnchorStyles.Bottom;
      this._btnCancel.Font = new Font("Microsoft Sans Serif", 9.75f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this._btnCancel.Location = new Point(268, 336);
      this._btnCancel.Name = "_btnCancel";
      this._btnCancel.Size = new Size(86, 35);
      this._btnCancel.TabIndex = 1;
      this._btnCancel.Text = "Cancel";
      this._btnCancel.UseVisualStyleBackColor = true;
      this._btnCancel.Click += new EventHandler(this._btnCancel_Click);
      this._txtName.Font = new Font("Microsoft Sans Serif", 9.75f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this._txtName.Location = new Point(60, 36);
      this._txtName.Name = "_txtName";
      this._txtName.Size = new Size(201, 22);
      this._txtName.TabIndex = 2;
      this._txtName.TextChanged += new EventHandler(this._txtName_TextChanged);
      this._lblName.AutoSize = true;
      this._lblName.Font = new Font("Microsoft Sans Serif", 9.75f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this._lblName.Location = new Point(9, 39);
      this._lblName.Name = "_lblName";
      this._lblName.Size = new Size(45, 16);
      this._lblName.TabIndex = 3;
      this._lblName.Text = "Name";
      this._txtLocation.Font = new Font("Microsoft Sans Serif", 9.75f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this._txtLocation.Location = new Point(119, 61);
      this._txtLocation.Name = "_txtLocation";
      this._txtLocation.Size = new Size(235, 22);
      this._txtLocation.TabIndex = 2;
      this._txtLocation.TextChanged += new EventHandler(this._txtLocation_TextChanged);
      this._btnLocation.Font = new Font("Microsoft Sans Serif", 9.75f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this._btnLocation.Location = new Point(86, 61);
      this._btnLocation.Name = "_btnLocation";
      this._btnLocation.Size = new Size(27, 22);
      this._btnLocation.TabIndex = 1;
      this._btnLocation.Text = "...";
      this._btnLocation.UseVisualStyleBackColor = true;
      this._btnLocation.Click += new EventHandler(this._btnLocation_Click);
      this._btnLoad.Font = new Font("Microsoft Sans Serif", 9.75f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this._btnLoad.Location = new Point(360, 61);
      this._btnLoad.Name = "_btnLoad";
      this._btnLoad.Size = new Size(76, 22);
      this._btnLoad.TabIndex = 1;
      this._btnLoad.Text = "Load";
      this._btnLoad.UseVisualStyleBackColor = true;
      this._btnLoad.Click += new EventHandler(this._btnLoad_Click);
      this._lblLocation.AutoSize = true;
      this._lblLocation.Font = new Font("Microsoft Sans Serif", 9.75f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this._lblLocation.Location = new Point(9, 64);
      this._lblLocation.Name = "_lblLocation";
      this._lblLocation.Size = new Size(71, 16);
      this._lblLocation.TabIndex = 4;
      this._lblLocation.Text = "Image File";
      this._nudDelay.Anchor = AnchorStyles.Bottom;
      this._nudDelay.Font = new Font("Microsoft Sans Serif", 9.75f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this._nudDelay.Location = new Point(297, 291);
      this._nudDelay.Name = "_nudDelay";
      this._nudDelay.Size = new Size(52, 22);
      this._nudDelay.TabIndex = 5;
      this._nudDelay.ValueChanged += new EventHandler(this._nudDelay_ValueChanged);
      this._lblDelay.Anchor = AnchorStyles.Bottom;
      this._lblDelay.AutoSize = true;
      this._lblDelay.Font = new Font("Microsoft Sans Serif", 9.75f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this._lblDelay.Location = new Point(249, 293);
      this._lblDelay.Name = "_lblDelay";
      this._lblDelay.Size = new Size(44, 16);
      this._lblDelay.TabIndex = 4;
      this._lblDelay.Text = "Delay";
      this.label2.Anchor = AnchorStyles.Bottom;
      this.label2.AutoSize = true;
      this.label2.Font = new Font("Microsoft Sans Serif", 9.75f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.label2.Location = new Point(355, 293);
      this.label2.Name = "label2";
      this.label2.Size = new Size(26, 16);
      this.label2.TabIndex = 4;
      this.label2.Text = "ms";
      this._lblFrameIndex0.Anchor = AnchorStyles.Bottom;
      this._lblFrameIndex0.AutoSize = true;
      this._lblFrameIndex0.Font = new Font("Microsoft Sans Serif", 9.75f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this._lblFrameIndex0.Location = new Point(67, 293);
      this._lblFrameIndex0.Name = "_lblFrameIndex0";
      this._lblFrameIndex0.Size = new Size(85, 16);
      this._lblFrameIndex0.TabIndex = 4;
      this._lblFrameIndex0.Text = "Frame Index:";
      this._lblFrameIndex1.Anchor = AnchorStyles.Bottom;
      this._lblFrameIndex1.AutoSize = true;
      this._lblFrameIndex1.Font = new Font("Microsoft Sans Serif", 9.75f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this._lblFrameIndex1.Location = new Point(155, 293);
      this._lblFrameIndex1.Name = "_lblFrameIndex1";
      this._lblFrameIndex1.Size = new Size(15, 16);
      this._lblFrameIndex1.TabIndex = 4;
      this._lblFrameIndex1.Text = "0";
      this.menuStrip1.Items.AddRange(new ToolStripItem[1]
      {
        (ToolStripItem) this.toolsToolStripMenuItem
      });
      this.menuStrip1.Location = new Point(0, 0);
      this.menuStrip1.Name = "menuStrip1";
      this.menuStrip1.Size = new Size(447, 24);
      this.menuStrip1.TabIndex = 6;
      this.menuStrip1.Text = "menuStrip1";
      this.toolsToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[1]
      {
        (ToolStripItem) this.hueSaturationToolStripMenuItem
      });
      this.toolsToolStripMenuItem.Name = "toolsToolStripMenuItem";
      this.toolsToolStripMenuItem.Size = new Size(39, 20);
      this.toolsToolStripMenuItem.Text = "Edit";
      this.toolsToolStripMenuItem.Click += new EventHandler(this.toolsToolStripMenuItem_Click);
      this.hueSaturationToolStripMenuItem.Name = "hueSaturationToolStripMenuItem";
      this.hueSaturationToolStripMenuItem.Size = new Size(155, 22);
      this.hueSaturationToolStripMenuItem.Text = "Hue/Saturation";
      this.hueSaturationToolStripMenuItem.Click += new EventHandler(this.hueSaturationToolStripMenuItem_Click);
      this.AutoScaleDimensions = new SizeF(6f, 13f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.ClientSize = new Size(447, 391);
      this.Controls.Add((Control) this._nudDelay);
      this.Controls.Add((Control) this.label2);
      this.Controls.Add((Control) this._lblFrameIndex1);
      this.Controls.Add((Control) this._lblFrameIndex0);
      this.Controls.Add((Control) this._lblDelay);
      this.Controls.Add((Control) this._lblLocation);
      this.Controls.Add((Control) this._lblName);
      this.Controls.Add((Control) this._txtLocation);
      this.Controls.Add((Control) this._txtName);
      this.Controls.Add((Control) this._btnCancel);
      this.Controls.Add((Control) this._btnApply);
      this.Controls.Add((Control) this._btnLocation);
      this.Controls.Add((Control) this._btnLoad);
      this.Controls.Add((Control) this._btnOk);
      this.Controls.Add((Control) this._pbFrame);
      this.Controls.Add((Control) this.menuStrip1);
      this.Icon = (Icon) componentResourceManager.GetObject("$this.Icon");
      this.MainMenuStrip = this.menuStrip1;
      this.Name = nameof (AddEditFrame);
      this.Text = "Add/Edit Sprite Frame";
      this.FormClosing += new FormClosingEventHandler(this.AddEditFrame_FormClosing);
      this.Load += new EventHandler(this.AddEditFrame_Load);
      ((ISupportInitialize) this._pbFrame).EndInit();
      this._nudDelay.EndInit();
      this.menuStrip1.ResumeLayout(false);
      this.menuStrip1.PerformLayout();
      this.ResumeLayout(false);
      this.PerformLayout();
    }
  }
}
