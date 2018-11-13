// Decompiled with JetBrains decompiler
// Type: IsoPack.PictureBoxWithInterpolationMode
// Assembly: IsoTool, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A40E7877-59D4-416C-9526-ACFD66F37CC4
// Assembly location: C:\Program Files\Iso Tool\IsoTool.exe

using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace IsoPack
{
  public class PictureBoxWithInterpolationMode : PictureBox
  {
    public InterpolationMode InterpolationMode { get; set; }

    protected override void OnPaint(PaintEventArgs paintEventArgs)
    {
      paintEventArgs.Graphics.InterpolationMode = this.InterpolationMode;
      base.OnPaint(paintEventArgs);
    }
  }
}
