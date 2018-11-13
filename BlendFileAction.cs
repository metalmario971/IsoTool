// Decompiled with JetBrains decompiler
// Type: IsoPack.BlendFileAction
// Assembly: IsoTool, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A40E7877-59D4-416C-9526-ACFD66F37CC4
// Assembly location: C:\Program Files\Iso Tool\IsoTool.exe

using System.IO;

namespace IsoPack
{
  public class BlendFileAction
  {
    public string Name { get; set; }

    public bool Checked { get; set; } = false;

    public BlendFileAction CreateCopy()
    {
      return new BlendFileAction()
      {
        Name = this.Name,
        Checked = this.Checked
      };
    }

    public void Serialize(BinaryWriter stream)
    {
      BinUtils.WriteString(this.Name, stream);
      BinUtils.WriteBool(this.Checked, stream);
    }

    public void Deserialize(BinaryReader stream)
    {
      this.Name = BinUtils.ReadString(stream);
      this.Checked = BinUtils.ReadBool(stream);
    }
  }
}
