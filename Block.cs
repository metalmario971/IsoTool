// Decompiled with JetBrains decompiler
// Type: IsoPack.Block
// Assembly: IsoTool, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A40E7877-59D4-416C-9526-ACFD66F37CC4
// Assembly location: C:\Program Files\Iso Tool\IsoTool.exe

using System.IO;

namespace IsoPack
{
  public class Block
  {
    public int Size { get; set; } = 0;

    public BinaryWriter OutStream { get; set; } = (BinaryWriter) null;

    public BinaryReader InStream { get; set; } = (BinaryReader) null;

    public Block(BinaryWriter o)
    {
      this.OutStream = o;
    }

    public Block(BinaryReader i)
    {
      this.InStream = i;
    }
  }
}
