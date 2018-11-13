// Decompiled with JetBrains decompiler
// Type: IsoPack.MtTex
// Assembly: IsoTool, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A40E7877-59D4-416C-9526-ACFD66F37CC4
// Assembly location: C:\Program Files\Iso Tool\IsoTool.exe

using System.Drawing;

namespace IsoPack
{
  public class MtTex
  {
    public Frame Frame { get; set; } = (Frame) null;

    public MtNode Node { get; set; } = (MtNode) null;

    public Bitmap Image { get; set; } = (Bitmap) null;

    public MtTex(Bitmap bmp, Frame fr)
    {
      this.Image = bmp;
      this.Frame = fr;
    }

    public MtTex CreateCopy(Frame newFrame)
    {
      return new MtTex(new Bitmap((System.Drawing.Image) this.Image), newFrame);
    }
  }
}
