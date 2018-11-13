// Decompiled with JetBrains decompiler
// Type: IsoPack.AddEditSprite
// Assembly: IsoTool, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A40E7877-59D4-416C-9526-ACFD66F37CC4
// Assembly location: C:\Program Files\Iso Tool\IsoTool.exe

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Windows.Forms;

namespace IsoPack
{
  public class AddEditSprite : AddEditForm
  {
    private MainForm _objMainForm = (MainForm) null;
    private bool _bChanged = false;
    private Model _objModel = (Model) null;
    private PictureBoxWithInterpolationMode FrameBox = new PictureBoxWithInterpolationMode();
    private SpriteListView SpriteListView = (SpriteListView) null;
    private IContainer components = (IContainer) null;
    private ListView _lsvFrames;
    private TextBox _txtName;
    private Label label1;
    private Label label2;
    private TextBox _txtLocation;
    private Label _lblLocation;
    private Button _btnLocation;
    private Button _btnSave;
    private Button _btnCancel;
    private Button _btnApply;
    private Button _btnPlay;
    private Button _btnLoad;
    private Button _btnAddFrame;
    private Button _btnRemoveFrame;
    private Button _btnAddMultiple;
    private Button _btnMoveFrameUp;
    private Button _btnMoveFrameDown;
    private PictureBox _pbSprite;
    private Label label3;
    private Button _btnResetOrigin;
    private NumericUpDown _nudOriginX;
    private NumericUpDown _nudOriginY;
    private MenuStrip menuStrip1;
    private ToolStripMenuItem toolsToolStripMenuItem;
    private ToolStripMenuItem convertToSpriteToolStripMenuItem;
    private ToolStripMenuItem duplicateSpritesToolStripMenuItem;
    private ToolStripMenuItem createGIFToolStripMenuItem;

    public Sprite Sprite { get; private set; } = (Sprite) null;

    public AddEditSprite(MainForm m)
    {
      this.InitializeComponent();
      this._objMainForm = m;
      this.SpriteListView = new SpriteListView(this._objMainForm, (Func<List<SpriteListViewItem>>) (() =>
      {
        List<SpriteListViewItem> spriteListViewItemList = new List<SpriteListViewItem>();
        foreach (Frame frame in this.Sprite.Frames)
          spriteListViewItemList.Add(new SpriteListViewItem((object) frame, frame));
        return spriteListViewItemList;
      }), (Action<List<object>>) (x =>
      {
        foreach (object obj in x)
        {
          if (obj is Frame)
            this.Sprite.Frames.Remove(obj as Frame);
        }
        this.UpdateUI(true);
      }));
      Globals.SwapControl((Control) this._lsvFrames, (Control) this.SpriteListView);
      this.SpriteListView.SelectedIndexChanged += new EventHandler(this.SpriteListView_SelectedIndexChanged);
      this.SetFrameBox();
      Globals.SetToolTip(new List<Control>()
      {
        (Control) this._btnAddMultiple
      }, "Add multiple frame images to this sprite.  Frames are appended before to the currently selected frame.  If no frame is selected, then frames are appended to the end of the sprite.");
      Globals.SetToolTip(new List<Control>()
      {
        (Control) this._btnAddFrame
      }, "Add an image to this sprite from your computer.");
      Globals.SetToolTip(new List<Control>()
      {
        (Control) this._btnRemoveFrame
      }, "Remove selected frame (delete key).");
      Globals.SetToolTip(new List<Control>()
      {
        (Control) this._btnPlay
      }, "Play the sprite animation in the animation viewer.");
      Globals.SetToolTip(new List<Control>()
      {
        (Control) this._txtName
      }, "The name of the sprite.  Used as an identifier in your game to get a sprite.");
      Globals.SetToolTip(new List<Control>()
      {
        (Control) this._txtLocation,
        (Control) this._btnLoad,
        (Control) this._btnLocation,
        (Control) this._lblLocation
      }, ".ASE files are not supported yet.");
      Globals.SetToolTip(new List<Control>()
      {
        (Control) this._btnMoveFrameUp
      }, "Move the selected frame(s) one frame before.  (This doesn't change the Name of the frame)");
      Globals.SetToolTip(new List<Control>()
      {
        (Control) this._btnMoveFrameDown
      }, "Move the selected frame(s) one frame after. (This doesn't change the Name of the frame)");
    }

    private void SetFrameBox()
    {
      Globals.SwapSpriteControl(this._pbSprite, this.FrameBox);
      this.FrameBox.MouseMove += (MouseEventHandler) ((s, e) =>
      {
        if (e.Button != MouseButtons.Left)
          return;
        Point location = e.Location;
        int x = location.X;
        location = e.Location;
        int y = location.Y;
        this.PickSpriteOrigin(x, y);
      });
    }

    private void SpriteListView_SelectedIndexChanged(object sender, EventArgs e)
    {
      this.UpdateSpriteImage();
    }

    private void SpriteInfo_Load(object sender, EventArgs e)
    {
    }

    private void MarkChanged()
    {
      this.CheckSpriteExists();
      this._bChanged = true;
      this._btnApply.Enabled = true;
    }

    public Sprite CreateNewSprite(Model m)
    {
      return new Sprite()
      {
        Name = this.GetNewSpriteName(),
        Model = m
      };
    }

    public void Show(Sprite s, Model m)
    {
      this._objModel = m;
      this._bChanged = false;
      if (s == null)
      {
        this.FormMode = FormMode.Add;
        this.Text = "Add Sprite";
        this.Sprite = this.CreateNewSprite(m);
      }
      else
      {
        this.FormMode = FormMode.Edit;
        this.Text = "Edit Sprite";
        this.Sprite = s;
      }
      this.LoadData();
      if (this._txtLocation.Text != "")
        this.CheckSpriteExists();
      if (this.SpriteListView.Items.Count > 0)
      {
        this.SpriteListView.Items[0].Selected = true;
        this.SpriteListView.Select();
      }
      this.Show();
      this.BringToFront();
    }

    private string GetNewSpriteName()
    {
      string str = "NewSprite-0";
      int num = 0;
      bool flag = true;
      while (flag)
      {
        str = string.Format("NewSprite-{0}", (object) num);
        flag = false;
        foreach (Model model in this._objMainForm.IsoPackFile.Models)
        {
          foreach (Sprite sprite in model.Sprites)
          {
            if (sprite.Name.Equals(str))
            {
              flag = true;
              ++num;
            }
          }
          if (flag)
            break;
        }
      }
      return str;
    }

    public override void UpdateUI(bool bDirty)
    {
      this.SpriteListView.UpdateListView();
      if (!bDirty)
        return;
      this.MarkChanged();
    }

    private void SaveData()
    {
      this.Sprite.Name = this._txtName.Text;
      this.Sprite.FileLocation = this._txtLocation.Text;
      this.Sprite.Origin_X = (float) this._nudOriginX.Value;
      this.Sprite.Origin_Y = (float) this._nudOriginY.Value;
      bool bDirty = false;
      if (this.FormMode == FormMode.Add)
      {
        bDirty = true;
        this._objModel.Sprites.Add(this.Sprite);
        this.FormMode = FormMode.Edit;
        this.Text = "Edit Sprite";
      }
      this._objMainForm.UpdateWindowUI(this._objModel, bDirty);
      this._bChanged = false;
      this._btnApply.Enabled = false;
    }

    private void LoadData()
    {
      this._txtName.Text = this.Sprite.Name;
      this._txtLocation.Text = this.Sprite.FileLocation;
      this._nudOriginX.Value = (Decimal) this.Sprite.Origin_X;
      this._nudOriginY.Value = (Decimal) this.Sprite.Origin_Y;
      this.UpdateUI(false);
    }

    private void _btnSave_Click(object sender, EventArgs e)
    {
      this.SaveData();
      this.Close();
    }

    private void SpriteInfo_FormClosing(object sender, FormClosingEventArgs e)
    {
      this._objMainForm.AddEditObject(this.Sprite, this._objModel, true);
    }

    private void _txtName_TextChanged(object sender, EventArgs e)
    {
      if (!(this._txtName.Text != this.Sprite.Name))
        return;
      this.MarkChanged();
    }

    private void _txtLocation_TextChanged(object sender, EventArgs e)
    {
      if (!(this._txtLocation.Text != this.Sprite.FileLocation))
        return;
      this.MarkChanged();
    }

    private void _btnCancel_Click(object sender, EventArgs e)
    {
      this.Close();
    }

    private void _btnPlay_Click(object sender, EventArgs e)
    {
      if (this.Sprite == null)
        return;
      new SpritePlayer(this._objMainForm).Show(this.Sprite.Frames);
    }

    private void _btnLocation_Click(object sender, EventArgs e)
    {
      string[] openSaveUserFile = Globals.GetValidOpenSaveUserFile(false, Globals.SupportedLoadSpriteImageFilter, Globals.SupportedLoadSpriteImageFilter, "png", this._objMainForm.IsoPackFile.AppSettings.ProjectRoot, false);
      if ((uint) openSaveUserFile.Length <= 0U)
        return;
      this._txtLocation.Text = openSaveUserFile[0];
    }

    private bool CheckSpriteExists()
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

    private void _btnAddFrame_Click(object sender, EventArgs e)
    {
      this._objMainForm.AddEditObject((Frame) null, this.Sprite, false);
    }

    private void _btnRemoveFrame_Click(object sender, EventArgs e)
    {
      Frame selectedObject = this.SpriteListView.GetSelectedObject() as Frame;
      if (selectedObject != null)
        this.Sprite.Frames.Remove(selectedObject);
      this.UpdateUI(true);
    }

    private void _btnApply_Click(object sender, EventArgs e)
    {
      this.SaveData();
    }

    private void _btnAddMultiple_Click(object sender, EventArgs e)
    {
      Frame selectedObject = this.SpriteListView.GetSelectedObject() as Frame;
      int addAtFrameId = 0;
      if (selectedObject != null)
        addAtFrameId = selectedObject.FrameId;
      else if (this.Sprite.Frames.Count > 0)
        addAtFrameId = this.Sprite.Frames[this.Sprite.Frames.Count - 1].FrameId;
      int num = (int) new AddEditFrameMultiple(this._objMainForm, this.Sprite, addAtFrameId).ShowDialog();
    }

    private void _btnMoveFrameUp_Click(object sender, EventArgs e)
    {
    }

    private void _btnMoveFrameDown_Click(object sender, EventArgs e)
    {
    }

    private void _btnResetOrigin_Click(object sender, EventArgs e)
    {
      this._nudOriginX.Value = Decimal.Zero;
      this._nudOriginY.Value = Decimal.Zero;
      this.UpdateSpriteImage();
    }

    private void PickSpriteOrigin(int x, int y)
    {
      float num1 = (float) x / (float) this.FrameBox.Width;
      float num2 = (float) y / (float) this.FrameBox.Height;
      this._nudOriginX.Value = (Decimal) ((float) this.FrameBox.Image.Width * num1);
      this._nudOriginY.Value = (Decimal) ((float) this.FrameBox.Image.Height * num2);
    }

    private void _nudOriginX_ValueChanged(object sender, EventArgs e)
    {
      if ((double) (float) this._nudOriginX.Value == (double) this.Sprite.Origin_X)
        return;
      this.UpdateSpriteImage();
      this.MarkChanged();
    }

    private void _nudOriginY_ValueChanged(object sender, EventArgs e)
    {
      if ((double) (float) this._nudOriginY.Value == (double) this.Sprite.Origin_Y)
        return;
      this.UpdateSpriteImage();
      this.MarkChanged();
    }

    private void UpdateSpriteImage()
    {
      if (this.SpriteListView.SelectedItems.Count <= 0)
        return;
      Frame tag = this.SpriteListView.SelectedItems[0].Tag as Frame;
      if (tag != null && tag.ImageTemp != null && tag.ImageTemp.Image != null)
        this.SetSpriteImage(tag.ImageTemp.Image);
    }

    private void SetSpriteImage(Bitmap img)
    {
      if (img == null)
        return;
      Bitmap bitmap = new Bitmap((Image) img);
      using (Graphics graphics = Graphics.FromImage((Image) bitmap))
      {
        Color color = Color.FromArgb(200, Color.Red);
        int num1 = (int) ((double) bitmap.Width * 0.0299999993294477) + 1;
        int num2 = (int) ((double) bitmap.Width * 0.0599999986588955) + 1;
        Pen pen1 = new Pen(color, (float) num1);
        int num3 = (int) this._nudOriginX.Value;
        int num4 = (int) this._nudOriginY.Value;
        int x1_1 = num3 - num2;
        int y1_1 = num4;
        int x2_1 = num3 + num2;
        int y2_1 = num4;
        int x1_2 = num3;
        int y1_2 = num4 - num2;
        int x2_2 = num3;
        int y2_2 = num4 + num2;
        graphics.DrawLine(pen1, x1_1, y1_1, x2_1, y2_1);
        graphics.DrawLine(pen1, x1_2, y1_2, x2_2, y2_2);
        Pen pen2 = new Pen(Color.Black, 1f);
        graphics.DrawLine(pen2, 0, 0, bitmap.Width - 1, 0);
        graphics.DrawLine(pen2, bitmap.Width - 1, 0, bitmap.Width - 1, bitmap.Height - 1);
        graphics.DrawLine(pen2, bitmap.Width - 1, bitmap.Height - 1, 0, bitmap.Height - 1);
        graphics.DrawLine(pen2, 0, bitmap.Height - 1, 0, 0);
        graphics.DrawLine(pen2, x1_2, y1_2, x2_2, y2_2);
      }
      this.FrameBox.Height = (int) ((double) this.FrameBox.Width * (double) ((float) bitmap.Height / (float) bitmap.Width));
      this.FrameBox.Image = (Image) bitmap;
      this.FrameBox.InterpolationMode = InterpolationMode.NearestNeighbor;
    }

    private void convertToSpriteToolStripMenuItem_Click(object sender, EventArgs e)
    {
      if (MessageBox.Show("Convert Selected Frames to Sprites? (This will DELETE the selected frames from this sprite and convert those frames into new sprites for the sprite's model.)", "Convert?", MessageBoxButtons.OKCancel) != DialogResult.OK || (this.Sprite == null || this.Sprite.Model == null))
        return;
      List<Frame> frameList = new List<Frame>();
      foreach (ListViewItem selectedItem in this.SpriteListView.SelectedItems)
      {
        if (selectedItem.Tag is Frame)
          frameList.Add(selectedItem.Tag as Frame);
      }
      Model model = this.Sprite.Model;
      foreach (Frame frame in frameList)
      {
        this.Sprite.Frames.Remove(frame);
        Sprite newSprite = this.CreateNewSprite(model);
        newSprite.Frames.Add(frame);
        model.Sprites.Add(newSprite);
      }
      this.UpdateUI(true);
    }

    private void duplicateSpritesToolStripMenuItem_Click(object sender, EventArgs e)
    {
      List<Frame> frameList = new List<Frame>();
      foreach (ListViewItem selectedItem in this.SpriteListView.SelectedItems)
      {
        if (selectedItem.Tag is Frame)
          this.Sprite.Frames.Add((selectedItem.Tag as Frame).CreateCopy());
      }
      this.UpdateUI(true);
    }

    private void createGIFToolStripMenuItem_Click(object sender, EventArgs e)
    {
      if (this.Sprite == null)
        return;
      string[] openSaveUserFile = Globals.GetValidOpenSaveUserFile(true, Globals.GifFilter, Globals.GifFilter, "gif", this._objMainForm.IsoPackFile.AppSettings.ProjectRoot, false);
      if (openSaveUserFile.Length == 1 && openSaveUserFile[0].Length > 0)
      {
        GifWriter gifWriter = new GifWriter(openSaveUserFile[0], 41, -1);
        foreach (Frame frame in this.Sprite.Frames)
        {
          if (frame.ImageTemp != null && frame.ImageTemp.Image != null)
            gifWriter.WriteFrame((Image) frame.ImageTemp.Image, frame.Delay);
        }
        gifWriter.Dispose();
      }
    }

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof (AddEditSprite));
      this._lsvFrames = new ListView();
      this._txtName = new TextBox();
      this.label1 = new Label();
      this.label2 = new Label();
      this._txtLocation = new TextBox();
      this._lblLocation = new Label();
      this._btnLocation = new Button();
      this._btnSave = new Button();
      this._btnCancel = new Button();
      this._btnApply = new Button();
      this._btnPlay = new Button();
      this._btnLoad = new Button();
      this._btnAddFrame = new Button();
      this._btnRemoveFrame = new Button();
      this._btnAddMultiple = new Button();
      this._btnMoveFrameUp = new Button();
      this._btnMoveFrameDown = new Button();
      this._pbSprite = new PictureBox();
      this.label3 = new Label();
      this._btnResetOrigin = new Button();
      this._nudOriginX = new NumericUpDown();
      this._nudOriginY = new NumericUpDown();
      this.menuStrip1 = new MenuStrip();
      this.toolsToolStripMenuItem = new ToolStripMenuItem();
      this.convertToSpriteToolStripMenuItem = new ToolStripMenuItem();
      this.duplicateSpritesToolStripMenuItem = new ToolStripMenuItem();
      this.createGIFToolStripMenuItem = new ToolStripMenuItem();
      ((ISupportInitialize) this._pbSprite).BeginInit();
      this._nudOriginX.BeginInit();
      this._nudOriginY.BeginInit();
      this.menuStrip1.SuspendLayout();
      this.SuspendLayout();
      this._lsvFrames.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
      this._lsvFrames.Font = new Font("Microsoft Sans Serif", 9.75f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this._lsvFrames.Location = new Point(20, 146);
      this._lsvFrames.Name = "_lsvFrames";
      this._lsvFrames.Size = new Size(361, 185);
      this._lsvFrames.TabIndex = 0;
      this._lsvFrames.UseCompatibleStateImageBehavior = false;
      this._txtName.Font = new Font("Microsoft Sans Serif", 9.75f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this._txtName.Location = new Point(63, 37);
      this._txtName.Name = "_txtName";
      this._txtName.Size = new Size(197, 22);
      this._txtName.TabIndex = 1;
      this._txtName.TextChanged += new EventHandler(this._txtName_TextChanged);
      this.label1.AutoSize = true;
      this.label1.Font = new Font("Microsoft Sans Serif", 9.75f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.label1.Location = new Point(12, 40);
      this.label1.Name = "label1";
      this.label1.Size = new Size(45, 16);
      this.label1.TabIndex = 2;
      this.label1.Text = "Name";
      this.label2.AutoSize = true;
      this.label2.Font = new Font("Microsoft Sans Serif", 9.75f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.label2.Location = new Point(18, 124);
      this.label2.Name = "label2";
      this.label2.Size = new Size(54, 16);
      this.label2.TabIndex = 2;
      this.label2.Text = "Frames";
      this._txtLocation.Enabled = false;
      this._txtLocation.Font = new Font("Microsoft Sans Serif", 9.75f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this._txtLocation.Location = new Point(113, 64);
      this._txtLocation.Name = "_txtLocation";
      this._txtLocation.Size = new Size(277, 22);
      this._txtLocation.TabIndex = 1;
      this._txtLocation.TextChanged += new EventHandler(this._txtLocation_TextChanged);
      this._lblLocation.AutoSize = true;
      this._lblLocation.Enabled = false;
      this._lblLocation.Font = new Font("Microsoft Sans Serif", 9.75f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this._lblLocation.Location = new Point(11, 67);
      this._lblLocation.Name = "_lblLocation";
      this._lblLocation.Size = new Size(59, 16);
      this._lblLocation.TabIndex = 2;
      this._lblLocation.Text = ".ase File";
      this._btnLocation.Enabled = false;
      this._btnLocation.Location = new Point(77, 64);
      this._btnLocation.Name = "_btnLocation";
      this._btnLocation.Size = new Size(30, 23);
      this._btnLocation.TabIndex = 3;
      this._btnLocation.Text = "...";
      this._btnLocation.UseVisualStyleBackColor = true;
      this._btnLocation.Click += new EventHandler(this._btnLocation_Click);
      this._btnSave.Anchor = AnchorStyles.Bottom;
      this._btnSave.Font = new Font("Microsoft Sans Serif", 9.75f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this._btnSave.Location = new Point(142, 337);
      this._btnSave.Name = "_btnSave";
      this._btnSave.Size = new Size(81, 31);
      this._btnSave.TabIndex = 4;
      this._btnSave.Text = "Ok";
      this._btnSave.UseVisualStyleBackColor = true;
      this._btnSave.Click += new EventHandler(this._btnSave_Click);
      this._btnCancel.Anchor = AnchorStyles.Bottom;
      this._btnCancel.Font = new Font("Microsoft Sans Serif", 9.75f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this._btnCancel.Location = new Point(316, 337);
      this._btnCancel.Name = "_btnCancel";
      this._btnCancel.Size = new Size(80, 31);
      this._btnCancel.TabIndex = 4;
      this._btnCancel.Text = "Cancel";
      this._btnCancel.UseVisualStyleBackColor = true;
      this._btnCancel.Click += new EventHandler(this._btnCancel_Click);
      this._btnApply.Anchor = AnchorStyles.Bottom;
      this._btnApply.Enabled = false;
      this._btnApply.Font = new Font("Microsoft Sans Serif", 9.75f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this._btnApply.Location = new Point(229, 337);
      this._btnApply.Name = "_btnApply";
      this._btnApply.Size = new Size(81, 31);
      this._btnApply.TabIndex = 4;
      this._btnApply.Text = "Apply";
      this._btnApply.UseVisualStyleBackColor = true;
      this._btnApply.Click += new EventHandler(this._btnApply_Click);
      this._btnPlay.Font = new Font("Microsoft Sans Serif", 9.75f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this._btnPlay.Location = new Point(170, 112);
      this._btnPlay.Name = "_btnPlay";
      this._btnPlay.Size = new Size(53, 28);
      this._btnPlay.TabIndex = 4;
      this._btnPlay.Text = "Play";
      this._btnPlay.UseVisualStyleBackColor = true;
      this._btnPlay.Click += new EventHandler(this._btnPlay_Click);
      this._btnLoad.Enabled = false;
      this._btnLoad.Font = new Font("Microsoft Sans Serif", 9.75f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this._btnLoad.Location = new Point(396, 64);
      this._btnLoad.Name = "_btnLoad";
      this._btnLoad.Size = new Size(68, 23);
      this._btnLoad.TabIndex = 4;
      this._btnLoad.Text = "Load";
      this._btnLoad.UseVisualStyleBackColor = true;
      this._btnLoad.Click += new EventHandler(this._btnPlay_Click);
      this._btnAddFrame.Font = new Font("Microsoft Sans Serif", 9.75f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this._btnAddFrame.Location = new Point(311, 112);
      this._btnAddFrame.Name = "_btnAddFrame";
      this._btnAddFrame.Size = new Size(32, 28);
      this._btnAddFrame.TabIndex = 4;
      this._btnAddFrame.Text = "+";
      this._btnAddFrame.UseVisualStyleBackColor = true;
      this._btnAddFrame.Click += new EventHandler(this._btnAddFrame_Click);
      this._btnRemoveFrame.Font = new Font("Microsoft Sans Serif", 9.75f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this._btnRemoveFrame.Location = new Point(349, 112);
      this._btnRemoveFrame.Name = "_btnRemoveFrame";
      this._btnRemoveFrame.Size = new Size(32, 28);
      this._btnRemoveFrame.TabIndex = 4;
      this._btnRemoveFrame.Text = "-";
      this._btnRemoveFrame.UseVisualStyleBackColor = true;
      this._btnRemoveFrame.Click += new EventHandler(this._btnRemoveFrame_Click);
      this._btnAddMultiple.Font = new Font("Microsoft Sans Serif", 9.75f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this._btnAddMultiple.Location = new Point(264, 112);
      this._btnAddMultiple.Name = "_btnAddMultiple";
      this._btnAddMultiple.Size = new Size(41, 28);
      this._btnAddMultiple.TabIndex = 4;
      this._btnAddMultiple.Text = "+...";
      this._btnAddMultiple.UseVisualStyleBackColor = true;
      this._btnAddMultiple.Click += new EventHandler(this._btnAddMultiple_Click);
      this._btnMoveFrameUp.Font = new Font("Microsoft Sans Serif", 9.75f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this._btnMoveFrameUp.Location = new Point(83, 112);
      this._btnMoveFrameUp.Name = "_btnMoveFrameUp";
      this._btnMoveFrameUp.Size = new Size(32, 28);
      this._btnMoveFrameUp.TabIndex = 4;
      this._btnMoveFrameUp.Text = "<-";
      this._btnMoveFrameUp.UseVisualStyleBackColor = true;
      this._btnMoveFrameUp.Click += new EventHandler(this._btnMoveFrameUp_Click);
      this._btnMoveFrameDown.Font = new Font("Microsoft Sans Serif", 9.75f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this._btnMoveFrameDown.Location = new Point(121, 112);
      this._btnMoveFrameDown.Name = "_btnMoveFrameDown";
      this._btnMoveFrameDown.Size = new Size(32, 28);
      this._btnMoveFrameDown.TabIndex = 4;
      this._btnMoveFrameDown.Text = "->";
      this._btnMoveFrameDown.UseVisualStyleBackColor = true;
      this._btnMoveFrameDown.Click += new EventHandler(this._btnMoveFrameDown_Click);
      this._pbSprite.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this._pbSprite.BackColor = Color.White;
      this._pbSprite.Location = new Point(396, 165);
      this._pbSprite.Name = "_pbSprite";
      this._pbSprite.Size = new Size(130, 130);
      this._pbSprite.SizeMode = PictureBoxSizeMode.StretchImage;
      this._pbSprite.TabIndex = 5;
      this._pbSprite.TabStop = false;
      this.label3.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.label3.AutoSize = true;
      this.label3.Font = new Font("Microsoft Sans Serif", 9.75f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.label3.Location = new Point(395, 124);
      this.label3.Name = "label3";
      this.label3.Size = new Size(46, 16);
      this.label3.TabIndex = 2;
      this.label3.Text = "Origin:";
      this._btnResetOrigin.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this._btnResetOrigin.Font = new Font("Microsoft Sans Serif", 9.75f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this._btnResetOrigin.Location = new Point(474, 114);
      this._btnResetOrigin.Name = "_btnResetOrigin";
      this._btnResetOrigin.Size = new Size(52, 23);
      this._btnResetOrigin.TabIndex = 4;
      this._btnResetOrigin.Text = "Reset";
      this._btnResetOrigin.UseVisualStyleBackColor = true;
      this._btnResetOrigin.Click += new EventHandler(this._btnResetOrigin_Click);
      this._nudOriginX.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this._nudOriginX.DecimalPlaces = 2;
      this._nudOriginX.Location = new Point(396, 143);
      this._nudOriginX.Maximum = new Decimal(new int[4]
      {
        999999,
        0,
        0,
        0
      });
      this._nudOriginX.Minimum = new Decimal(new int[4]
      {
        999999,
        0,
        0,
        int.MinValue
      });
      this._nudOriginX.Name = "_nudOriginX";
      this._nudOriginX.Size = new Size(62, 20);
      this._nudOriginX.TabIndex = 6;
      this._nudOriginX.ValueChanged += new EventHandler(this._nudOriginX_ValueChanged);
      this._nudOriginY.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this._nudOriginY.DecimalPlaces = 2;
      this._nudOriginY.Location = new Point(464, 143);
      this._nudOriginY.Maximum = new Decimal(new int[4]
      {
        999999,
        0,
        0,
        0
      });
      this._nudOriginY.Minimum = new Decimal(new int[4]
      {
        999999,
        0,
        0,
        int.MinValue
      });
      this._nudOriginY.Name = "_nudOriginY";
      this._nudOriginY.Size = new Size(62, 20);
      this._nudOriginY.TabIndex = 6;
      this._nudOriginY.ValueChanged += new EventHandler(this._nudOriginY_ValueChanged);
      this.menuStrip1.Items.AddRange(new ToolStripItem[1]
      {
        (ToolStripItem) this.toolsToolStripMenuItem
      });
      this.menuStrip1.Location = new Point(0, 0);
      this.menuStrip1.Name = "menuStrip1";
      this.menuStrip1.Size = new Size(543, 25);
      this.menuStrip1.TabIndex = 7;
      this.menuStrip1.Text = "menuStrip1";
      this.toolsToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[3]
      {
        (ToolStripItem) this.convertToSpriteToolStripMenuItem,
        (ToolStripItem) this.duplicateSpritesToolStripMenuItem,
        (ToolStripItem) this.createGIFToolStripMenuItem
      });
      this.toolsToolStripMenuItem.Font = new Font("Segoe UI", 9.75f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.toolsToolStripMenuItem.Name = "toolsToolStripMenuItem";
      this.toolsToolStripMenuItem.Size = new Size(51, 21);
      this.toolsToolStripMenuItem.Text = "Tools";
      this.convertToSpriteToolStripMenuItem.Name = "convertToSpriteToolStripMenuItem";
      this.convertToSpriteToolStripMenuItem.Size = new Size(229, 22);
      this.convertToSpriteToolStripMenuItem.Text = "Convert Frames To Sprites";
      this.convertToSpriteToolStripMenuItem.Click += new EventHandler(this.convertToSpriteToolStripMenuItem_Click);
      this.duplicateSpritesToolStripMenuItem.Name = "duplicateSpritesToolStripMenuItem";
      this.duplicateSpritesToolStripMenuItem.Size = new Size(229, 22);
      this.duplicateSpritesToolStripMenuItem.Text = "Duplicate Sprites";
      this.duplicateSpritesToolStripMenuItem.Click += new EventHandler(this.duplicateSpritesToolStripMenuItem_Click);
      this.createGIFToolStripMenuItem.Name = "createGIFToolStripMenuItem";
      this.createGIFToolStripMenuItem.Size = new Size(229, 22);
      this.createGIFToolStripMenuItem.Text = "Save as GIF";
      this.createGIFToolStripMenuItem.Click += new EventHandler(this.createGIFToolStripMenuItem_Click);
      this.AutoScaleDimensions = new SizeF(6f, 13f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.ClientSize = new Size(543, 373);
      this.Controls.Add((Control) this._nudOriginY);
      this.Controls.Add((Control) this._nudOriginX);
      this.Controls.Add((Control) this._pbSprite);
      this.Controls.Add((Control) this._btnCancel);
      this.Controls.Add((Control) this._btnResetOrigin);
      this.Controls.Add((Control) this._btnLoad);
      this.Controls.Add((Control) this._btnRemoveFrame);
      this.Controls.Add((Control) this._btnAddMultiple);
      this.Controls.Add((Control) this._btnMoveFrameDown);
      this.Controls.Add((Control) this._btnMoveFrameUp);
      this.Controls.Add((Control) this._btnAddFrame);
      this.Controls.Add((Control) this._btnPlay);
      this.Controls.Add((Control) this._btnApply);
      this.Controls.Add((Control) this._btnSave);
      this.Controls.Add((Control) this._btnLocation);
      this.Controls.Add((Control) this.label3);
      this.Controls.Add((Control) this.label2);
      this.Controls.Add((Control) this._lblLocation);
      this.Controls.Add((Control) this.label1);
      this.Controls.Add((Control) this._txtLocation);
      this.Controls.Add((Control) this._txtName);
      this.Controls.Add((Control) this._lsvFrames);
      this.Controls.Add((Control) this.menuStrip1);
      this.Icon = (Icon) componentResourceManager.GetObject("$this.Icon");
      this.MainMenuStrip = this.menuStrip1;
      this.Name = nameof (AddEditSprite);
      this.Text = "Sprite Info";
      this.FormClosing += new FormClosingEventHandler(this.SpriteInfo_FormClosing);
      this.Load += new EventHandler(this.SpriteInfo_Load);
      ((ISupportInitialize) this._pbSprite).EndInit();
      this._nudOriginX.EndInit();
      this._nudOriginY.EndInit();
      this.menuStrip1.ResumeLayout(false);
      this.menuStrip1.PerformLayout();
      this.ResumeLayout(false);
      this.PerformLayout();
    }
  }
}
