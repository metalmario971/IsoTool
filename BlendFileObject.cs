// Decompiled with JetBrains decompiler
// Type: IsoPack.BlendFileObject
// Assembly: IsoTool, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A40E7877-59D4-416C-9526-ACFD66F37CC4
// Assembly location: C:\Program Files\Iso Tool\IsoTool.exe

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace IsoPack
{
  public class BlendFileObject
  {
    public string Name { get; set; }

    public string Type { get; set; }

    public bool Checked { get; set; } = false;

    public List<BlendFileAction> Actions { get; set; } = new List<BlendFileAction>();

    public BlendFileObject CreateCopy()
    {
      return new BlendFileObject()
      {
        Name = this.Name,
        Type = this.Type,
        Checked = this.Checked,
        Actions = this.Actions.Select<BlendFileAction, BlendFileAction>((Func<BlendFileAction, BlendFileAction>) (x => x.CreateCopy())).ToList<BlendFileAction>()
      };
    }

    public void Serialize(BinaryWriter stream)
    {
      BinUtils.WriteString(this.Name, stream);
      BinUtils.WriteString(this.Type, stream);
      BinUtils.WriteBool(this.Checked, stream);
      BinUtils.WriteInt32(this.Actions.Count, stream);
      foreach (BlendFileAction action in this.Actions)
        action.Serialize(stream);
    }

    public void Deserialize(BinaryReader stream)
    {
      this.Name = BinUtils.ReadString(stream);
      this.Type = BinUtils.ReadString(stream);
      this.Checked = BinUtils.ReadBool(stream);
      int num = BinUtils.ReadInt32(stream);
      for (int index = 0; index < num; ++index)
      {
        BlendFileAction blendFileAction = new BlendFileAction();
        blendFileAction.Deserialize(stream);
        this.Actions.Add(blendFileAction);
      }
    }
  }
}
