// Decompiled with JetBrains decompiler
// Type: IsoPack.ivec2
// Assembly: IsoTool, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A40E7877-59D4-416C-9526-ACFD66F37CC4
// Assembly location: C:\Program Files\Iso Tool\IsoTool.exe

namespace IsoPack
{
  public class ivec2
  {
    public int x { get; set; }

    public int y { get; set; }

    public void Construct(int dx, int dy)
    {
      this.x = dx;
      this.y = dy;
    }
  }
}
