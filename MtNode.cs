// Decompiled with JetBrains decompiler
// Type: IsoPack.MtNode
// Assembly: IsoTool, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A40E7877-59D4-416C-9526-ACFD66F37CC4
// Assembly location: C:\Program Files\Iso Tool\IsoTool.exe

namespace IsoPack
{
  public class MtNode
  {
    public MtTex Tex { get; set; } = (MtTex) null;

    public MtNode[] Child { get; set; } = new MtNode[2];

    public Box2i Rect { get; set; } = new Box2i();

    public MtNode Plop(MtTex tex)
    {
      if (this.Child[0] != null && this.Child[1] != null)
      {
        MtNode mtNode = this.Child[0].Plop(tex);
        if (mtNode != null)
          return mtNode;
        return this.Child[1].Plop(tex);
      }
      if (this.Tex != null)
        return (MtNode) null;
      int num1 = this.Rect.Width();
      int num2 = this.Rect.Height();
      if (tex.Image.Height > num2 || tex.Image.Width > num1)
        return (MtNode) null;
      if (tex.Image.Width == num1 && tex.Image.Height == num2)
      {
        if (this.Rect.p0.x == 0 && this.Rect.p0.y == 0)
        {
          int num3 = 0 + 1;
        }
        return this;
      }
      this.Child[0] = new MtNode();
      this.Child[1] = new MtNode();
      if (num1 - tex.Image.Width > num2 - tex.Image.Height)
      {
        this.Child[0].Rect.Construct(this.Rect.Left(), this.Rect.Top(), this.Rect.Left() + tex.Image.Width, this.Rect.Bottom());
        this.Child[1].Rect.Construct(this.Rect.Left() + tex.Image.Width, this.Rect.Top(), this.Rect.Right(), this.Rect.Bottom());
      }
      else
      {
        this.Child[0].Rect.Construct(this.Rect.Left(), this.Rect.Top(), this.Rect.Right(), this.Rect.Top() + tex.Image.Height);
        this.Child[1].Rect.Construct(this.Rect.Left(), this.Rect.Top() + tex.Image.Height, this.Rect.Right(), this.Rect.Bottom());
      }
      return this.Child[0].Plop(tex);
    }
  }
}
