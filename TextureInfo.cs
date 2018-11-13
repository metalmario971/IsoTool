// Decompiled with JetBrains decompiler
// Type: IsoPack.TextureInfo
// Assembly: IsoTool, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A40E7877-59D4-416C-9526-ACFD66F37CC4
// Assembly location: C:\Program Files\Iso Tool\IsoTool.exe

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Windows.Forms;

namespace IsoPack
{
  public class TextureInfo : Form
  {
    private MainForm _objMainForm = (MainForm) null;
    private Image _selected = (Image) null;
    private bool _bShowGrid = false;
    private List<RectangleF> Rectangles = new List<RectangleF>();
    private IContainer components = (IContainer) null;
    private int panelOrigWDt;
    private int panelOrigHDt;
    private PictureBoxWithInterpolationMode TextureBox;
    private PictureBox _pbTexture;
    private Button _btnClose;
    private ListView _lsvTexture;
    private Panel _objPanel;
    private TrackBar _tbZoom;
    private Button _btnGreen;
    private Button _btnBlue;
    private Button _btnRed;
    private Button _btnGrid;
    private NumericUpDown _nudGridWidth;
    private Label label1;
    private Label label2;
    private Label _lblWidth;
    private Label _lblHeight;

    public PackedTexture PackedTexture { get; set; } = (PackedTexture) null;

    public Image Texture { get; set; } = (Image) null;

    public TextureInfo(MainForm frm)
    {
      this.InitializeComponent();
      this._objMainForm = frm;
      this.TextureBox = new PictureBoxWithInterpolationMode();
      Globals.SwapSpriteControl(this._pbTexture, this.TextureBox);
      this.ClearUI();
      this.panelOrigWDt = this.Width - (this._objPanel.Location.X + this._objPanel.Width);
      this.panelOrigHDt = this.Height - (this._objPanel.Location.Y + this._objPanel.Height);
    }

    private void PackedTexture_Load(object sender, EventArgs e)
    {
    }

    public new void Show()
    {
      this.UpdateUI();
      base.Show();
    }

    private MegaTex GetSelectedTexture()
    {
      if (this._lsvTexture.SelectedItems.Count == 0)
        return (MegaTex) null;
      string name = ((MegaTex) this._lsvTexture.SelectedItems[0].Tag).Name;
      return this.PackedTexture.Images.FirstOrDefault<MegaTex>((Func<MegaTex, bool>) (x => x.Name == name));
    }

    public void UpdateUI()
    {
      this._nudGridWidth.Enabled = this._bShowGrid;
      this.UpdateTexturesListView();
      MegaTex megaTex = this.GetSelectedTexture();
      if (megaTex == null && this._lsvTexture.Items.Count > 0)
        megaTex = (MegaTex) this._lsvTexture.Items[0].Tag;
      if (megaTex != null)
        this._selected = (Image) megaTex.MasterImage;
      if (this._selected == null)
        return;
      this._lblWidth.Text = this._selected.Width.ToString();
      this._lblHeight.Text = this._selected.Height.ToString();
      this.Rectangles.Clear();
      foreach (Model model in this._objMainForm.IsoPackFile.Models)
      {
        foreach (Sprite sprite in model.Sprites)
        {
          foreach (Frame frame in sprite.Frames)
          {
            if (frame.texid == megaTex.Id)
              this.Rectangles.Add(new RectangleF((float) frame.x, (float) frame.y, (float) frame.w, (float) frame.h));
          }
        }
      }
      this.ResizePicture();
    }

    private void UpdateTexturesListView()
    {
      this._lsvTexture.SelectedIndexChanged -= new EventHandler(this._lsvTexture_SelectedIndexChanged);
      MegaTex selectedTexture = this.GetSelectedTexture();
      this._lsvTexture.Items.Clear();
      this._lsvTexture.View = View.SmallIcon;
      this._lsvTexture.Alignment = ListViewAlignment.Top;
      this._lsvTexture.HideSelection = false;
      ImageList imageList = new ImageList();
      for (int index = 0; index < this.PackedTexture.Images.Count; ++index)
      {
        MegaTex image = this.PackedTexture.Images[index];
        if (image == null)
          imageList.Images.Add((Image) this.PackedTexture.GetDefaultXImage());
        else
          imageList.Images.Add((Image) image.MasterImage);
        this._lsvTexture.Items.Add(new ListViewItem()
        {
          ImageIndex = index,
          Text = image.Name,
          Tag = (object) image
        });
      }
      this._lsvTexture.SmallImageList = imageList;
      Globals.SetListViewToSelectedObjectTag(this._lsvTexture, (object) selectedTexture);
      Application.DoEvents();
      this._lsvTexture.SelectedIndexChanged += new EventHandler(this._lsvTexture_SelectedIndexChanged);
    }

    public void ClearUI()
    {
      this.PackedTexture = new PackedTexture(this._objMainForm);
      this.TextureBox.Image = (Image) null;
      this.UpdateTexturesListView();
    }

    private void _btnClose_Click(object sender, EventArgs e)
    {
      this.Hide();
    }

    private void PackedTexture_FormClosing(object sender, FormClosingEventArgs e)
    {
      if (e.CloseReason != CloseReason.UserClosing)
        return;
      e.Cancel = true;
      this.Hide();
    }

    private void _lsvTexture_SelectedIndexChanged(object sender, EventArgs e)
    {
      this.UpdateUI();
    }

    private void _tbZoom_Scroll(object sender, EventArgs e)
    {
      this.ResizePicture();
    }

    private void ResizePicture()
    {
      if (this._selected == null)
        return;
      float num = (float) this._tbZoom.Value * (1f / 1000f);
      Size newSize = new Size((int) ((double) this._selected.Width * (double) num), (int) ((double) this._selected.Height * (double) num));
      Bitmap bitmap = new Bitmap(this._selected);
      if (this._bShowGrid)
      {
        using (Graphics graphics = Graphics.FromImage((Image) bitmap))
        {
          Pen pen = new Pen(Color.FromArgb(120, Color.Gray), (float) (int) this._nudGridWidth.Value);
          graphics.DrawRectangles(pen, this.Rectangles.ToArray());
        }
      }
      this.TextureBox.Image = (Image) new Bitmap((Image) bitmap, newSize);
      this.TextureBox.InterpolationMode = InterpolationMode.NearestNeighbor;
    }

    private void _btnGrid_Click(object sender, EventArgs e)
    {
      this._bShowGrid = !this._bShowGrid;
      this.UpdateUI();
    }

    private void _nudGridWidth_ValueChanged(object sender, EventArgs e)
    {
      this.UpdateUI();
    }

    private void _lblWidth_Click(object sender, EventArgs e)
    {
    }

    private void label1_Click(object sender, EventArgs e)
    {
    }

    private void TextureInfo_Resize(object sender, EventArgs e)
    {
      this._objPanel.Width = this.ClientRectangle.Width - this.panelOrigWDt - this._objPanel.Location.X;
      this._objPanel.Height = this.ClientRectangle.Height - this.panelOrigHDt - this._objPanel.Location.Y;
      this.TextureBox.Location = new Point(0, 0);
      this.TextureBox.Width = this._objPanel.Width;
      this.TextureBox.Height = this._objPanel.Height;
      this.ResizePicture();
    }

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof (TextureInfo));
      this._pbTexture = new PictureBox();
      this._btnClose = new Button();
      this._lsvTexture = new ListView();
      this._objPanel = new Panel();
      this._tbZoom = new TrackBar();
      this._btnGreen = new Button();
      this._btnBlue = new Button();
      this._btnRed = new Button();
      this._btnGrid = new Button();
      this._nudGridWidth = new NumericUpDown();
      this.label1 = new Label();
      this.label2 = new Label();
      this._lblWidth = new Label();
      this._lblHeight = new Label();
      ((ISupportInitialize) this._pbTexture).BeginInit();
      this._objPanel.SuspendLayout();
      this._tbZoom.BeginInit();
      this._nudGridWidth.BeginInit();
      this.SuspendLayout();
      this._pbTexture.BackColor = Color.White;
      this._pbTexture.BorderStyle = BorderStyle.FixedSingle;
      this._pbTexture.Location = new Point(3, 3);
      this._pbTexture.Name = "_pbTexture";
      this._pbTexture.Size = new Size(353, 316);
      this._pbTexture.SizeMode = PictureBoxSizeMode.AutoSize;
      this._pbTexture.TabIndex = 0;
      this._pbTexture.TabStop = false;
      this._btnClose.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
      this._btnClose.Font = new Font("Microsoft Sans Serif", 9.75f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this._btnClose.Location = new Point(399, 427);
      this._btnClose.Name = "_btnClose";
      this._btnClose.Size = new Size(80, 30);
      this._btnClose.TabIndex = 1;
      this._btnClose.Text = "Close";
      this._btnClose.UseVisualStyleBackColor = true;
      this._btnClose.Click += new EventHandler(this._btnClose_Click);
      this._lsvTexture.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left;
      this._lsvTexture.Font = new Font("Microsoft Sans Serif", 9.75f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this._lsvTexture.Location = new Point(12, 12);
      this._lsvTexture.Name = "_lsvTexture";
      this._lsvTexture.Size = new Size(102, 409);
      this._lsvTexture.TabIndex = 2;
      this._lsvTexture.UseCompatibleStateImageBehavior = false;
      this._lsvTexture.SelectedIndexChanged += new EventHandler(this._lsvTexture_SelectedIndexChanged);
      this._objPanel.AutoScroll = true;
      this._objPanel.Controls.Add((Control) this._pbTexture);
      this._objPanel.Location = new Point(120, 12);
      this._objPanel.Name = "_objPanel";
      this._objPanel.Size = new Size(359, 334);
      this._objPanel.TabIndex = 3;
      this._tbZoom.Anchor = AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
      this._tbZoom.Location = new Point(120, 352);
      this._tbZoom.Maximum = 10000;
      this._tbZoom.Minimum = 1;
      this._tbZoom.Name = "_tbZoom";
      this._tbZoom.Size = new Size(359, 45);
      this._tbZoom.TabIndex = 4;
      this._tbZoom.TickStyle = TickStyle.None;
      this._tbZoom.Value = 1000;
      this._tbZoom.Scroll += new EventHandler(this._tbZoom_Scroll);
      this._btnGreen.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
      this._btnGreen.BackColor = Color.Lime;
      this._btnGreen.FlatStyle = FlatStyle.Flat;
      this._btnGreen.Location = new Point(43, 427);
      this._btnGreen.Name = "_btnGreen";
      this._btnGreen.Size = new Size(22, 23);
      this._btnGreen.TabIndex = 5;
      this._btnGreen.UseVisualStyleBackColor = false;
      this._btnBlue.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
      this._btnBlue.BackColor = Color.Blue;
      this._btnBlue.FlatStyle = FlatStyle.Flat;
      this._btnBlue.Location = new Point(74, 427);
      this._btnBlue.Name = "_btnBlue";
      this._btnBlue.Size = new Size(21, 23);
      this._btnBlue.TabIndex = 5;
      this._btnBlue.UseVisualStyleBackColor = false;
      this._btnRed.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
      this._btnRed.BackColor = Color.Red;
      this._btnRed.FlatStyle = FlatStyle.Flat;
      this._btnRed.Location = new Point(12, 427);
      this._btnRed.Name = "_btnRed";
      this._btnRed.Size = new Size(22, 23);
      this._btnRed.TabIndex = 5;
      this._btnRed.UseVisualStyleBackColor = false;
      this._btnGrid.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
      this._btnGrid.BackgroundImage = (Image) componentResourceManager.GetObject("_btnGrid.BackgroundImage");
      this._btnGrid.BackgroundImageLayout = ImageLayout.Stretch;
      this._btnGrid.Location = new Point(142, 413);
      this._btnGrid.Name = "_btnGrid";
      this._btnGrid.Size = new Size(41, 37);
      this._btnGrid.TabIndex = 6;
      this._btnGrid.UseVisualStyleBackColor = true;
      this._btnGrid.Click += new EventHandler(this._btnGrid_Click);
      this._nudGridWidth.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
      this._nudGridWidth.Font = new Font("Microsoft Sans Serif", 9.75f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this._nudGridWidth.Location = new Point(189, 421);
      this._nudGridWidth.Maximum = new Decimal(new int[4]
      {
        999,
        0,
        0,
        0
      });
      this._nudGridWidth.Minimum = new Decimal(new int[4]
      {
        1,
        0,
        0,
        0
      });
      this._nudGridWidth.Name = "_nudGridWidth";
      this._nudGridWidth.Size = new Size(37, 22);
      this._nudGridWidth.TabIndex = 7;
      this._nudGridWidth.Value = new Decimal(new int[4]
      {
        1,
        0,
        0,
        0
      });
      this._nudGridWidth.ValueChanged += new EventHandler(this._nudGridWidth_ValueChanged);
      this.label1.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
      this.label1.AutoSize = true;
      this.label1.Font = new Font("Microsoft Sans Serif", 9.75f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.label1.Location = new Point(311, 381);
      this.label1.Name = "label1";
      this.label1.Size = new Size(45, 16);
      this.label1.TabIndex = 8;
      this.label1.Text = "Width:";
      this.label1.Click += new EventHandler(this.label1_Click);
      this.label2.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
      this.label2.AutoSize = true;
      this.label2.Font = new Font("Microsoft Sans Serif", 9.75f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.label2.Location = new Point(412, 381);
      this.label2.Name = "label2";
      this.label2.Size = new Size(50, 16);
      this.label2.TabIndex = 8;
      this.label2.Text = "Height:";
      this._lblWidth.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
      this._lblWidth.AutoSize = true;
      this._lblWidth.Font = new Font("Microsoft Sans Serif", 9.75f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this._lblWidth.Location = new Point(351, 381);
      this._lblWidth.Name = "_lblWidth";
      this._lblWidth.Size = new Size(15, 16);
      this._lblWidth.TabIndex = 8;
      this._lblWidth.Text = "0";
      this._lblWidth.Click += new EventHandler(this._lblWidth_Click);
      this._lblHeight.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
      this._lblHeight.AutoSize = true;
      this._lblHeight.Font = new Font("Microsoft Sans Serif", 9.75f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this._lblHeight.Location = new Point(459, 381);
      this._lblHeight.Name = "_lblHeight";
      this._lblHeight.Size = new Size(15, 16);
      this._lblHeight.TabIndex = 8;
      this._lblHeight.Text = "0";
      this.AutoScaleDimensions = new SizeF(6f, 13f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.ClientSize = new Size(491, 468);
      this.Controls.Add((Control) this.label2);
      this.Controls.Add((Control) this._lblHeight);
      this.Controls.Add((Control) this._lblWidth);
      this.Controls.Add((Control) this.label1);
      this.Controls.Add((Control) this._nudGridWidth);
      this.Controls.Add((Control) this._btnGrid);
      this.Controls.Add((Control) this._btnRed);
      this.Controls.Add((Control) this._btnBlue);
      this.Controls.Add((Control) this._btnGreen);
      this.Controls.Add((Control) this._btnClose);
      this.Controls.Add((Control) this._tbZoom);
      this.Controls.Add((Control) this._objPanel);
      this.Controls.Add((Control) this._lsvTexture);
      this.Icon = (Icon) componentResourceManager.GetObject("$this.Icon");
      this.Name = nameof (TextureInfo);
      this.Text = "Texture Info";
      this.FormClosing += new FormClosingEventHandler(this.PackedTexture_FormClosing);
      this.Load += new EventHandler(this.PackedTexture_Load);
      this.Resize += new EventHandler(this.TextureInfo_Resize);
      ((ISupportInitialize) this._pbTexture).EndInit();
      this._objPanel.ResumeLayout(false);
      this._objPanel.PerformLayout();
      this._tbZoom.EndInit();
      this._nudGridWidth.EndInit();
      this.ResumeLayout(false);
      this.PerformLayout();
    }
  }
}
