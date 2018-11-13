// Decompiled with JetBrains decompiler
// Type: IsoPack.AddEditFrameMultiple
// Assembly: IsoTool, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A40E7877-59D4-416C-9526-ACFD66F37CC4
// Assembly location: C:\Program Files\Iso Tool\IsoTool.exe

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace IsoPack
{
  public class AddEditFrameMultiple : Form
  {
    private MainForm _objMainForm = (MainForm) null;
    private SpriteListView SpriteListView = (SpriteListView) null;
    private int iAddAtFrameId = 0;
    private List<Frame> newFrames = new List<Frame>();
    private IContainer components = (IContainer) null;
    private Sprite Sprite;
    private Button _btnLoad;
    private TextBox _txtNamePrefix;
    private Label _lblName;
    private Label label1;
    private Button _btnOk;
    private Button _btnCancel;
    private ListView _lsvFrames;

    public AddEditFrameMultiple(MainForm m, Sprite s, int addAtFrameId)
    {
      this.InitializeComponent();
      this._objMainForm = m;
      this.Sprite = s;
      this.iAddAtFrameId = addAtFrameId;
      this.SpriteListView = new SpriteListView(this._objMainForm, (Func<List<SpriteListViewItem>>) (() =>
      {
        List<SpriteListViewItem> spriteListViewItemList = new List<SpriteListViewItem>();
        foreach (Frame newFrame in this.newFrames)
          spriteListViewItemList.Add(new SpriteListViewItem((object) newFrame, newFrame));
        return spriteListViewItemList;
      }), (Action<List<object>>) (x =>
      {
        foreach (object obj in x)
        {
          if (obj is Frame)
            this.newFrames.Remove(obj as Frame);
        }
        this.SpriteListView.UpdateListView();
      }));
      Globals.SwapControl((Control) this._lsvFrames, (Control) this.SpriteListView);
      Globals.SetToolTip(new List<Control>()
      {
        (Control) this._btnLoad
      }, "Select one or more images to add to the sprite.");
      Globals.SetToolTip(new List<Control>()
      {
        (Control) this._txtNamePrefix
      }, "The prefix that is used to name each sprite.  An index will be appended to the end of the prefix in the order in which the sprites are selected from the filesystem.");
    }

    private void _btnLoad_Click(object sender, EventArgs e)
    {
      string[] openSaveUserFile = Globals.GetValidOpenSaveUserFile(false, Globals.SupportedLoadSpriteImageFilter, Globals.SupportedLoadSpriteImageFilter, "png", this._objMainForm.IsoPackFile.AppSettings.ProjectRoot, true);
      int num = 0;
      this.newFrames.Clear();
      foreach (string file in openSaveUserFile)
      {
        Frame frame = new Frame()
        {
          Name = Globals.GetNewFrameName(this._objMainForm),
          Sprite = this.Sprite,
          FrameId = this.iAddAtFrameId + num
        };
        frame.Name = this._txtNamePrefix.Text + num.ToString();
        frame.LoadFrameImage(file, this._objMainForm);
        this.newFrames.Add(frame);
        ++num;
      }
      this.SpriteListView.UpdateListView();
      this.newFrames.Sort((Comparison<Frame>) ((x, y) => x.FrameId.CompareTo(y.FrameId)));
    }

    private void _btnOk_Click(object sender, EventArgs e)
    {
      foreach (Frame frame in this.Sprite.Frames)
      {
        if (frame.FrameId >= this.iAddAtFrameId)
          frame.FrameId += this.newFrames.Count;
      }
      foreach (Frame newFrame in this.newFrames)
        this.Sprite.Frames.Add(newFrame);
      this.newFrames.Clear();
      this.Sprite.SortFrames();
      this._objMainForm.UpdateWindowUI(this.Sprite, true);
      this.Close();
    }

    private void _btnCancel_Click(object sender, EventArgs e)
    {
      this.Close();
    }

    private void AddEditFrameMultiple_Load(object sender, EventArgs e)
    {
    }

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof (AddEditFrameMultiple));
      this._btnLoad = new Button();
      this._txtNamePrefix = new TextBox();
      this._lblName = new Label();
      this.label1 = new Label();
      this._btnOk = new Button();
      this._btnCancel = new Button();
      this._lsvFrames = new ListView();
      this.SuspendLayout();
      this._btnLoad.Font = new Font("Microsoft Sans Serif", 9.75f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this._btnLoad.Location = new Point(95, 54);
      this._btnLoad.Name = "_btnLoad";
      this._btnLoad.Size = new Size(28, 23);
      this._btnLoad.TabIndex = 2;
      this._btnLoad.Text = "...";
      this._btnLoad.UseVisualStyleBackColor = true;
      this._btnLoad.Click += new EventHandler(this._btnLoad_Click);
      this._txtNamePrefix.Font = new Font("Microsoft Sans Serif", 9.75f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this._txtNamePrefix.Location = new Point(100, 17);
      this._txtNamePrefix.Name = "_txtNamePrefix";
      this._txtNamePrefix.Size = new Size(222, 22);
      this._txtNamePrefix.TabIndex = 3;
      this._txtNamePrefix.Text = "Frame-";
      this._lblName.AutoSize = true;
      this._lblName.Font = new Font("Microsoft Sans Serif", 9.75f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this._lblName.Location = new Point(13, 20);
      this._lblName.Name = "_lblName";
      this._lblName.Size = new Size(81, 16);
      this._lblName.TabIndex = 4;
      this._lblName.Text = "Name Prefix";
      this.label1.AutoSize = true;
      this.label1.Font = new Font("Microsoft Sans Serif", 9.75f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.label1.Location = new Point(13, 57);
      this.label1.Name = "label1";
      this.label1.Size = new Size(78, 16);
      this.label1.TabIndex = 4;
      this.label1.Text = "Select Files";
      this._btnOk.Anchor = AnchorStyles.Bottom;
      this._btnOk.Font = new Font("Microsoft Sans Serif", 9.75f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this._btnOk.Location = new Point((int) sbyte.MaxValue, 217);
      this._btnOk.Name = "_btnOk";
      this._btnOk.Size = new Size(75, 27);
      this._btnOk.TabIndex = 5;
      this._btnOk.Text = "OK";
      this._btnOk.UseVisualStyleBackColor = true;
      this._btnOk.Click += new EventHandler(this._btnOk_Click);
      this._btnCancel.Anchor = AnchorStyles.Bottom;
      this._btnCancel.Font = new Font("Microsoft Sans Serif", 9.75f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this._btnCancel.Location = new Point(208, 217);
      this._btnCancel.Name = "_btnCancel";
      this._btnCancel.Size = new Size(75, 27);
      this._btnCancel.TabIndex = 5;
      this._btnCancel.Text = "Cancel";
      this._btnCancel.UseVisualStyleBackColor = true;
      this._btnCancel.Click += new EventHandler(this._btnCancel_Click);
      this._lsvFrames.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
      this._lsvFrames.Font = new Font("Microsoft Sans Serif", 9.75f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this._lsvFrames.Location = new Point(16, 93);
      this._lsvFrames.Name = "_lsvFrames";
      this._lsvFrames.Size = new Size(401, 105);
      this._lsvFrames.TabIndex = 1;
      this._lsvFrames.UseCompatibleStateImageBehavior = false;
      this.AutoScaleDimensions = new SizeF(6f, 13f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.ClientSize = new Size(425, 256);
      this.Controls.Add((Control) this._btnCancel);
      this.Controls.Add((Control) this._btnOk);
      this.Controls.Add((Control) this.label1);
      this.Controls.Add((Control) this._lblName);
      this.Controls.Add((Control) this._txtNamePrefix);
      this.Controls.Add((Control) this._btnLoad);
      this.Controls.Add((Control) this._lsvFrames);
      this.Icon = (Icon) componentResourceManager.GetObject("$this.Icon");
      this.Name = nameof (AddEditFrameMultiple);
      this.Text = "Add (Multiple)";
      this.Load += new EventHandler(this.AddEditFrameMultiple_Load);
      this.ResumeLayout(false);
      this.PerformLayout();
    }
  }
}
