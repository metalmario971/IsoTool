// Decompiled with JetBrains decompiler
// Type: IsoPack.HSVDialog
// Assembly: IsoTool, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A40E7877-59D4-416C-9526-ACFD66F37CC4
// Assembly location: C:\Program Files\Iso Tool\IsoTool.exe

using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace IsoPack
{
  public class HSVDialog : Form
  {
    private Bitmap _saved = (Bitmap) null;
    private IContainer components = (IContainer) null;
    private TrackBar _tbValue;
    private TrackBar _tbSaturation;
    private TrackBar _tbHue;
    private PictureBox _pbHue;
    private Button _btnOk;
    private Button _btnCancel;
    private PictureBox _pbSaturation;
    private PictureBox _pbValue;

    public HSVDialog()
    {
      this.InitializeComponent();
    }

    public AddEditForm AddEditForm { get; set; } = (AddEditForm) null;

    public Frame Frame { get; set; } = (Frame) null;

    private void _btnCancel_Click(object sender, EventArgs e)
    {
      if (this._saved != null)
        this.Frame.ImageTemp.Image = this._saved;
      if (this.AddEditForm != null)
        this.AddEditForm.UpdateUI(false);
      this.Close();
    }

    private void _btnOk_Click(object sender, EventArgs e)
    {
      this._saved = (Bitmap) null;
      if (this.AddEditForm != null)
        this.AddEditForm.UpdateUI(true);
      this.Close();
    }

    private LinearGradientBrush HueBrush()
    {
      return new LinearGradientBrush(this.ClientRectangle, Color.Black, Color.Black, 0.0f, false)
      {
        InterpolationColors = new ColorBlend()
        {
          Positions = new float[7]
          {
            0.0f,
            0.1666667f,
            0.3333333f,
            0.5f,
            0.6666667f,
            0.8333333f,
            1f
          },
          Colors = new Color[7]
          {
            Color.FromArgb((int) byte.MaxValue, (int) byte.MaxValue, 0, 0),
            Color.FromArgb((int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue, 0),
            Color.FromArgb((int) byte.MaxValue, 0, (int) byte.MaxValue, 0),
            Color.FromArgb((int) byte.MaxValue, 0, (int) byte.MaxValue, (int) byte.MaxValue),
            Color.FromArgb((int) byte.MaxValue, 0, 0, (int) byte.MaxValue),
            Color.FromArgb((int) byte.MaxValue, (int) byte.MaxValue, 0, (int) byte.MaxValue),
            Color.FromArgb((int) byte.MaxValue, (int) byte.MaxValue, 0, 0)
          }
        }
      };
    }

    private void HSVDialog_Load(object sender, EventArgs e)
    {
      Bitmap bitmap1 = new Bitmap(this._pbHue.Width, this._pbHue.Height);
      using (Graphics graphics = Graphics.FromImage((Image) bitmap1))
      {
        LinearGradientBrush linearGradientBrush = this.HueBrush();
        Pen pen = new Pen((Brush) linearGradientBrush);
        graphics.FillRectangle((Brush) linearGradientBrush, 0, 0, bitmap1.Width, bitmap1.Height);
      }
      this._pbHue.Image = (Image) bitmap1;
      Bitmap bitmap2 = new Bitmap(this._pbSaturation.Width, this._pbSaturation.Height);
      using (Graphics graphics = Graphics.FromImage((Image) bitmap2))
      {
        LinearGradientBrush linearGradientBrush = new LinearGradientBrush(new Point(0, bitmap2.Height), new Point(bitmap2.Width, bitmap2.Height), Color.FromArgb((int) byte.MaxValue, 128, 128, 128), Color.FromArgb((int) byte.MaxValue, (int) byte.MaxValue, 0, 0));
        Pen pen = new Pen((Brush) linearGradientBrush);
        graphics.FillRectangle((Brush) linearGradientBrush, 0, 0, bitmap2.Width, bitmap2.Height);
      }
      this._pbSaturation.Image = (Image) bitmap2;
      Bitmap bitmap3 = new Bitmap(this._pbValue.Width, this._pbValue.Height);
      using (Graphics graphics = Graphics.FromImage((Image) bitmap3))
      {
        LinearGradientBrush linearGradientBrush = new LinearGradientBrush(new Point(0, bitmap3.Height), new Point(bitmap3.Width, bitmap3.Height), Color.FromArgb((int) byte.MaxValue, 0, 0, 0), Color.FromArgb((int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue));
        Pen pen = new Pen((Brush) linearGradientBrush);
        graphics.FillRectangle((Brush) linearGradientBrush, 0, 0, bitmap3.Width, bitmap3.Height);
      }
      this._pbValue.Image = (Image) bitmap3;
    }

    private float tb01(TrackBar tb)
    {
      return (float) tb.Value * 0.0001f;
    }

    private void _tbHue_Scroll(object sender, EventArgs e)
    {
      this.ModifyImage();
    }

    private void _tbSaturation_Scroll(object sender, EventArgs e)
    {
      this.ModifyImage();
    }

    private void _tbValue_Scroll(object sender, EventArgs e)
    {
      this.ModifyImage();
    }

    private void ModifyImage()
    {
      if (this._saved == null)
        this._saved = new Bitmap((Image) this.Frame.ImageTemp.Image);
      float num1 = (float) ((double) this.tb01(this._tbHue) * 720.0 - 360.0);
      float num2 = (float) ((double) this.tb01(this._tbSaturation) * 2.0 - 1.0);
      float num3 = (float) ((double) this.tb01(this._tbValue) * 2.0 - 1.0);
      for (int y = 0; y < this._saved.Height; ++y)
      {
        for (int x = 0; x < this._saved.Width; ++x)
        {
          Color color = this._saved.GetPixel(x, y);
          float hue = color.GetHue();
          float saturation = color.GetSaturation();
          float brightness = color.GetBrightness();
          int r;
          int g;
          int b;
          this.HsvToRgb((double) (hue + num1), (double) (saturation + num2), (double) (brightness + num3), out r, out g, out b);
          color = Color.FromArgb((int) color.A, r, g, b);
          this.Frame.ImageTemp.Image.SetPixel(x, y, color);
        }
      }
      if (this.AddEditForm == null)
        return;
      this.AddEditForm.UpdateUI(false);
    }

    private void HsvToRgb(double h, double S, double V, out int r, out int g, out int b)
    {
      double num1 = h;
      while (num1 < 0.0)
        num1 += 360.0;
      while (num1 >= 360.0)
        num1 -= 360.0;
      double num2;
      double num3;
      double num4;
      if (V <= 0.0)
      {
        double num5;
        num2 = num5 = 0.0;
        num3 = num5;
        num4 = num5;
      }
      else if (S <= 0.0)
      {
        double num5;
        num2 = num5 = V;
        num3 = num5;
        num4 = num5;
      }
      else
      {
        double d = num1 / 60.0;
        int num5 = (int) Math.Floor(d);
        double num6 = d - (double) num5;
        double num7 = V * (1.0 - S);
        double num8 = V * (1.0 - S * num6);
        double num9 = V * (1.0 - S * (1.0 - num6));
        switch (num5)
        {
          case -1:
            num4 = V;
            num3 = num7;
            num2 = num8;
            break;
          case 0:
            num4 = V;
            num3 = num9;
            num2 = num7;
            break;
          case 1:
            num4 = num8;
            num3 = V;
            num2 = num7;
            break;
          case 2:
            num4 = num7;
            num3 = V;
            num2 = num9;
            break;
          case 3:
            num4 = num7;
            num3 = num8;
            num2 = V;
            break;
          case 4:
            num4 = num9;
            num3 = num7;
            num2 = V;
            break;
          case 5:
            num4 = V;
            num3 = num7;
            num2 = num8;
            break;
          case 6:
            num4 = V;
            num3 = num9;
            num2 = num7;
            break;
          default:
            double num10;
            num2 = num10 = V;
            num3 = num10;
            num4 = num10;
            break;
        }
      }
      r = this.Clamp((int) (num4 * (double) byte.MaxValue));
      g = this.Clamp((int) (num3 * (double) byte.MaxValue));
      b = this.Clamp((int) (num2 * (double) byte.MaxValue));
    }

    private int Clamp(int i)
    {
      if (i < 0)
        return 0;
      if (i > (int) byte.MaxValue)
        return (int) byte.MaxValue;
      return i;
    }

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof (HSVDialog));
      this._tbValue = new TrackBar();
      this._tbSaturation = new TrackBar();
      this._tbHue = new TrackBar();
      this._pbHue = new PictureBox();
      this._btnOk = new Button();
      this._btnCancel = new Button();
      this._pbSaturation = new PictureBox();
      this._pbValue = new PictureBox();
      this._tbValue.BeginInit();
      this._tbSaturation.BeginInit();
      this._tbHue.BeginInit();
      ((ISupportInitialize) this._pbHue).BeginInit();
      ((ISupportInitialize) this._pbSaturation).BeginInit();
      ((ISupportInitialize) this._pbValue).BeginInit();
      this.SuspendLayout();
      this._tbValue.AutoSize = false;
      this._tbValue.Location = new Point(10, 105);
      this._tbValue.Maximum = 10000;
      this._tbValue.Name = "_tbValue";
      this._tbValue.Size = new Size(289, 17);
      this._tbValue.TabIndex = 0;
      this._tbValue.TickStyle = TickStyle.None;
      this._tbValue.Value = 5000;
      this._tbValue.Scroll += new EventHandler(this._tbValue_Scroll);
      this._tbSaturation.AutoSize = false;
      this._tbSaturation.Location = new Point(10, 61);
      this._tbSaturation.Maximum = 10000;
      this._tbSaturation.Name = "_tbSaturation";
      this._tbSaturation.Size = new Size(289, 17);
      this._tbSaturation.TabIndex = 0;
      this._tbSaturation.TickStyle = TickStyle.None;
      this._tbSaturation.Value = 5000;
      this._tbSaturation.Scroll += new EventHandler(this._tbSaturation_Scroll);
      this._tbHue.AutoSize = false;
      this._tbHue.Location = new Point(10, 24);
      this._tbHue.Maximum = 10000;
      this._tbHue.Name = "_tbHue";
      this._tbHue.Size = new Size(289, 17);
      this._tbHue.TabIndex = 0;
      this._tbHue.TickStyle = TickStyle.None;
      this._tbHue.Value = 5000;
      this._tbHue.Scroll += new EventHandler(this._tbHue_Scroll);
      this._pbHue.BorderStyle = BorderStyle.FixedSingle;
      this._pbHue.Location = new Point(21, 12);
      this._pbHue.Name = "_pbHue";
      this._pbHue.Size = new Size(273, 12);
      this._pbHue.SizeMode = PictureBoxSizeMode.StretchImage;
      this._pbHue.TabIndex = 1;
      this._pbHue.TabStop = false;
      this._btnOk.Font = new Font("Microsoft Sans Serif", 9.75f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this._btnOk.Location = new Point(70, 135);
      this._btnOk.Name = "_btnOk";
      this._btnOk.Size = new Size(87, 29);
      this._btnOk.TabIndex = 2;
      this._btnOk.Text = "OK";
      this._btnOk.UseVisualStyleBackColor = true;
      this._btnOk.Click += new EventHandler(this._btnOk_Click);
      this._btnCancel.Font = new Font("Microsoft Sans Serif", 9.75f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this._btnCancel.Location = new Point(163, 134);
      this._btnCancel.Name = "_btnCancel";
      this._btnCancel.Size = new Size(87, 29);
      this._btnCancel.TabIndex = 2;
      this._btnCancel.Text = "Cancel";
      this._btnCancel.UseVisualStyleBackColor = true;
      this._btnCancel.Click += new EventHandler(this._btnCancel_Click);
      this._pbSaturation.BorderStyle = BorderStyle.FixedSingle;
      this._pbSaturation.Location = new Point(21, 49);
      this._pbSaturation.Name = "_pbSaturation";
      this._pbSaturation.Size = new Size(273, 12);
      this._pbSaturation.SizeMode = PictureBoxSizeMode.StretchImage;
      this._pbSaturation.TabIndex = 1;
      this._pbSaturation.TabStop = false;
      this._pbValue.BorderStyle = BorderStyle.FixedSingle;
      this._pbValue.Location = new Point(21, 93);
      this._pbValue.Name = "_pbValue";
      this._pbValue.Size = new Size(273, 12);
      this._pbValue.SizeMode = PictureBoxSizeMode.StretchImage;
      this._pbValue.TabIndex = 1;
      this._pbValue.TabStop = false;
      this.AutoScaleDimensions = new SizeF(6f, 13f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.ClientSize = new Size(315, 176);
      this.Controls.Add((Control) this._btnCancel);
      this.Controls.Add((Control) this._btnOk);
      this.Controls.Add((Control) this._pbValue);
      this.Controls.Add((Control) this._pbSaturation);
      this.Controls.Add((Control) this._pbHue);
      this.Controls.Add((Control) this._tbHue);
      this.Controls.Add((Control) this._tbSaturation);
      this.Controls.Add((Control) this._tbValue);
      this.Icon = (Icon) componentResourceManager.GetObject("$this.Icon");
      this.Name = nameof (HSVDialog);
      this.Text = "Hue, Saturation, Value";
      this.Load += new EventHandler(this.HSVDialog_Load);
      this._tbValue.EndInit();
      this._tbSaturation.EndInit();
      this._tbHue.EndInit();
      ((ISupportInitialize) this._pbHue).EndInit();
      ((ISupportInitialize) this._pbSaturation).EndInit();
      ((ISupportInitialize) this._pbValue).EndInit();
      this.ResumeLayout(false);
    }
  }
}
