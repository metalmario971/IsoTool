// Decompiled with JetBrains decompiler
// Type: IsoPack.Box2i
// Assembly: IsoTool, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A40E7877-59D4-416C-9526-ACFD66F37CC4
// Assembly location: C:\Program Files\Iso Tool\IsoTool.exe

namespace IsoPack
{
  public class Box2i
  {
    public ivec2 p0 { get; set; } = new ivec2();

    public ivec2 p1 { get; set; } = new ivec2();

    public int Left()
    {
      return this.p0.x;
    }

    public int Top()
    {
      return this.p0.y;
    }

    public int Right()
    {
      return this.p1.x;
    }

    public int Bottom()
    {
      return this.p1.y;
    }

    public int Width()
    {
      return this.p1.x - this.p0.x;
    }

    public int Height()
    {
      return this.p1.y - this.p0.y;
    }

    public void Construct(int left, int top, int right, int bottom)
    {
      this.p0.x = left;
      this.p0.y = top;
      this.p1.x = right;
      this.p1.y = bottom;
    }
  }
}
