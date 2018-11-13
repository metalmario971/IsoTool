// Decompiled with JetBrains decompiler
// Type: IsoPack.SpritePlayer
// Assembly: IsoTool, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A40E7877-59D4-416C-9526-ACFD66F37CC4
// Assembly location: C:\Program Files\Iso Tool\IsoTool.exe

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;
using System.Windows.Threading;

namespace IsoPack
{
  public class SpritePlayer : Form
  {
    private SpritePlayer.PlayState _ePlayState = SpritePlayer.PlayState.Play;
    private List<Frame> Frames = new List<Frame>();
    private MainForm _objMainForm = (MainForm) null;
    private PictureBoxWithInterpolationMode SpriteFrame = new PictureBoxWithInterpolationMode();
    private DispatcherTimer _playTimer = new DispatcherTimer();
    private long _iLastTick = 0;
    private IContainer components = (IContainer) null;
    private PictureBox _pbSpriteFrame;
    private Button _btnPlay;
    private Button _btnPause;
    private Label _lblFrame;
    private Button _btnFramePrev;
    private Button _btnFrameNext;

    private int PlayFrameIndex { get; set; } = 0;

    private float SpeedMultiplier { get; set; } = 1f;

    private float SpeedMultiplierMax { get; set; } = 2f;

    public SpritePlayer(MainForm mf)
    {
      this.InitializeComponent();
      this._objMainForm = mf;
      Globals.SwapSpriteControl(this._pbSpriteFrame, this.SpriteFrame);
    }

    private void SpritePlayer_Load(object sender, EventArgs e)
    {
    }

    public void ClearUI()
    {
      this.SpriteFrame.Image = (Image) null;
      this.Frames.Clear();
    }

    public void Show(List<Frame> frames)
    {
      this.StopAnimation();
      this.ClearUI();
      if (frames.Count > 0)
      {
        this.Frames.Clear();
        foreach (Frame frame in frames)
          this.Frames.Add(frame);
        this.PlayFrameIndex = 0;
        this.PlayAnimation();
        this.Show();
        this.BringToFront();
        this._playTimer = new DispatcherTimer();
        this._playTimer.Tick += new EventHandler(this._playTimer_Tick);
        this._playTimer.Interval = new TimeSpan(0, 0, 0, 0, 1);
        this._playTimer.Start();
      }
      else
        this._objMainForm.Log("No sprite selected, or sprite had no frames to play animation.");
    }

    private void _playTimer_Tick(object sender, EventArgs e)
    {
      if ((long) Environment.TickCount - this._iLastTick < (long) this.Frames[this.PlayFrameIndex].Delay)
        return;
      this._iLastTick = (long) Environment.TickCount;
      if (this._ePlayState == SpritePlayer.PlayState.Play)
      {
        this.PlayFrameIndex = (this.PlayFrameIndex + 1) % this.Frames.Count;
        this.UpdateFrame();
      }
    }

    private void _btnStop_Click(object sender, EventArgs e)
    {
    }

    private void SpritePlayer_FormClosing(object sender, FormClosingEventArgs e)
    {
      this.StopAnimation();
    }

    private void _btnPlay_Click(object sender, EventArgs e)
    {
      this.PlayAnimation();
    }

    private void _btnPause_Click(object sender, EventArgs e)
    {
      this.PauseAnimation();
    }

    public void UpdateFrame()
    {
      this._lblFrame.Text = "Frame" + this.PlayFrameIndex.ToString();
      if (this.PlayFrameIndex < this.Frames.Count && this.PlayFrameIndex >= 0)
      {
        if (this.Frames[this.PlayFrameIndex].ImageTemp == null || this.Frames[this.PlayFrameIndex].ImageTemp.Image == null)
          return;
        this.SpriteFrame.Image = (Image) this.Frames[this.PlayFrameIndex].ImageTemp.Image;
      }
      else
        Debugger.Break();
    }

    public void PlayAnimation()
    {
      this._ePlayState = SpritePlayer.PlayState.Play;
    }

    public void StopAnimation()
    {
      this._playTimer.Stop();
    }

    public void PauseAnimation()
    {
      this._ePlayState = SpritePlayer.PlayState.Pause;
    }

    private void SpritePlayer_Resize(object sender, EventArgs e)
    {
    }

    private void _btnFrameNext_Click(object sender, EventArgs e)
    {
      if (this._ePlayState != SpritePlayer.PlayState.Pause)
        return;
      this.PlayFrameIndex = (this.PlayFrameIndex + 1) % this.Frames.Count;
      this.UpdateFrame();
    }

    private void _btnFramePrev_Click(object sender, EventArgs e)
    {
      if (this._ePlayState != SpritePlayer.PlayState.Pause)
        return;
      --this.PlayFrameIndex;
      if (this.PlayFrameIndex < 0)
        this.PlayFrameIndex += this.Frames.Count;
      this.UpdateFrame();
    }

    private void _pbSpriteFrame_Click(object sender, EventArgs e)
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
      ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof (SpritePlayer));
      this._pbSpriteFrame = new PictureBox();
      this._btnPlay = new Button();
      this._btnPause = new Button();
      this._lblFrame = new Label();
      this._btnFramePrev = new Button();
      this._btnFrameNext = new Button();
      ((ISupportInitialize) this._pbSpriteFrame).BeginInit();
      this.SuspendLayout();
      this._pbSpriteFrame.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
      this._pbSpriteFrame.BackColor = Color.White;
      this._pbSpriteFrame.BorderStyle = BorderStyle.FixedSingle;
      this._pbSpriteFrame.Location = new Point(13, 13);
      this._pbSpriteFrame.Name = "_pbSpriteFrame";
      this._pbSpriteFrame.Size = new Size(259, 245);
      this._pbSpriteFrame.SizeMode = PictureBoxSizeMode.Zoom;
      this._pbSpriteFrame.TabIndex = 0;
      this._pbSpriteFrame.TabStop = false;
      this._pbSpriteFrame.Click += new EventHandler(this._pbSpriteFrame_Click);
      this._btnPlay.Anchor = AnchorStyles.Bottom;
      this._btnPlay.Location = new Point(144, 285);
      this._btnPlay.Name = "_btnPlay";
      this._btnPlay.Size = new Size(38, 23);
      this._btnPlay.TabIndex = 1;
      this._btnPlay.Text = ">";
      this._btnPlay.UseVisualStyleBackColor = true;
      this._btnPlay.Click += new EventHandler(this._btnPlay_Click);
      this._btnPause.AccessibleRole = AccessibleRole.Application;
      this._btnPause.Anchor = AnchorStyles.Bottom;
      this._btnPause.Location = new Point(100, 285);
      this._btnPause.Name = "_btnPause";
      this._btnPause.Size = new Size(38, 23);
      this._btnPause.TabIndex = 1;
      this._btnPause.Text = "| |";
      this._btnPause.UseVisualStyleBackColor = true;
      this._btnPause.Click += new EventHandler(this._btnPause_Click);
      this._lblFrame.Anchor = AnchorStyles.Bottom;
      this._lblFrame.AutoSize = true;
      this._lblFrame.Font = new Font("Microsoft Sans Serif", 9.75f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this._lblFrame.ForeColor = SystemColors.ControlText;
      this._lblFrame.Location = new Point(115, 261);
      this._lblFrame.Name = "_lblFrame";
      this._lblFrame.Size = new Size(47, 16);
      this._lblFrame.TabIndex = 2;
      this._lblFrame.Text = "Frame";
      this._btnFramePrev.AccessibleRole = AccessibleRole.Application;
      this._btnFramePrev.Anchor = AnchorStyles.Bottom;
      this._btnFramePrev.Location = new Point(35, 285);
      this._btnFramePrev.Name = "_btnFramePrev";
      this._btnFramePrev.Size = new Size(38, 23);
      this._btnFramePrev.TabIndex = 1;
      this._btnFramePrev.Text = "<<";
      this._btnFramePrev.UseVisualStyleBackColor = true;
      this._btnFramePrev.Click += new EventHandler(this._btnFramePrev_Click);
      this._btnFrameNext.AccessibleRole = AccessibleRole.Application;
      this._btnFrameNext.Anchor = AnchorStyles.Bottom;
      this._btnFrameNext.Location = new Point(209, 285);
      this._btnFrameNext.Name = "_btnFrameNext";
      this._btnFrameNext.Size = new Size(38, 23);
      this._btnFrameNext.TabIndex = 1;
      this._btnFrameNext.Text = ">>";
      this._btnFrameNext.UseVisualStyleBackColor = true;
      this._btnFrameNext.Click += new EventHandler(this._btnFrameNext_Click);
      this.AutoScaleDimensions = new SizeF(6f, 13f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.ClientSize = new Size(284, 320);
      this.Controls.Add((Control) this._lblFrame);
      this.Controls.Add((Control) this._btnFrameNext);
      this.Controls.Add((Control) this._btnFramePrev);
      this.Controls.Add((Control) this._btnPause);
      this.Controls.Add((Control) this._btnPlay);
      this.Controls.Add((Control) this._pbSpriteFrame);
      this.Icon = (Icon) componentResourceManager.GetObject("$this.Icon");
      this.Name = nameof (SpritePlayer);
      this.Text = "Animation Player";
      this.Load += new EventHandler(this.SpritePlayer_Load);
      this.Resize += new EventHandler(this.SpritePlayer_Resize);
      ((ISupportInitialize) this._pbSpriteFrame).EndInit();
      this.ResumeLayout(false);
      this.PerformLayout();
    }

    private enum PlayState
    {
      Play,
      Pause,
      Aborted,
    }
  }
}
