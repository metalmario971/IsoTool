// Decompiled with JetBrains decompiler
// Type: IsoPack.SpriteListViewItem
// Assembly: IsoTool, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A40E7877-59D4-416C-9526-ACFD66F37CC4
// Assembly location: C:\Program Files\Iso Tool\IsoTool.exe

namespace IsoPack
{
  public class SpriteListViewItem
  {
    public object Object { get; set; } = (object) null;

    public Frame Frame { get; set; } = (Frame) null;

    public SpriteListViewItem(object tag, Frame frame)
    {
      this.Object = tag;
      this.Frame = frame;
    }
  }
}
