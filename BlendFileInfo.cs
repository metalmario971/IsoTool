// Decompiled with JetBrains decompiler
// Type: IsoPack.BlendFileInfo
// Assembly: IsoTool, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A40E7877-59D4-416C-9526-ACFD66F37CC4
// Assembly location: C:\Program Files\Iso Tool\IsoTool.exe

using System.Collections.Generic;
using System.IO;

namespace IsoPack
{
  public class BlendFileInfo
  {
    public List<BlendFileObject> Objects { get; set; } = new List<BlendFileObject>();

    public void Serialize(BinaryWriter stream)
    {
      BinUtils.WriteInt32(this.Objects.Count, stream);
      foreach (BlendFileObject blendFileObject in this.Objects)
        blendFileObject.Serialize(stream);
    }

    public void Deserialize(BinaryReader stream)
    {
      int num = BinUtils.ReadInt32(stream);
      for (int index = 0; index < num; ++index)
      {
        BlendFileObject blendFileObject = new BlendFileObject();
        blendFileObject.Deserialize(stream);
        this.Objects.Add(blendFileObject);
      }
    }

    public BlendFileInfo CreateCopy()
    {
      BlendFileInfo blendFileInfo = new BlendFileInfo();
      foreach (BlendFileObject blendFileObject in this.Objects)
        blendFileInfo.Objects.Add(blendFileObject.CreateCopy());
      return blendFileInfo;
    }
  }
}
