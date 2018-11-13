// Decompiled with JetBrains decompiler
// Type: IsoPack.ModelRenderParameters
// Assembly: IsoTool, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A40E7877-59D4-416C-9526-ACFD66F37CC4
// Assembly location: C:\Program Files\Iso Tool\IsoTool.exe

using System;
using System.IO;

namespace IsoPack
{
  public class ModelRenderParameters
  {
    public float RenderDistance { get; set; } = 5f;

    public int RenderWidth { get; set; } = 128;

    public int RenderHeight { get; set; } = 128;

    public int Directions { get; set; } = 4;

    public int AASamples { get; set; } = 0;

    public int KeyframeGrain { get; set; } = 1;

    public bool FitModel { get; set; } = false;

    public ObjectInfoType InfoType { get; set; } = ObjectInfoType.Both;

    public float IsoHeight { get; set; } = 1f;

    public void Serialize(BinaryWriter stream, float version)
    {
      BinUtils.WriteFloat(this.RenderDistance, stream);
      BinUtils.WriteInt32(this.RenderWidth, stream);
      BinUtils.WriteInt32(this.RenderHeight, stream);
      BinUtils.WriteInt32(this.Directions, stream);
      BinUtils.WriteInt32(this.AASamples, stream);
      BinUtils.WriteInt32(this.KeyframeGrain, stream);
      BinUtils.WriteString(this.InfoType.ToString(), stream);
      BinUtils.WriteBool(this.FitModel, stream);
      BinUtils.WriteFloat(this.IsoHeight, stream);
    }

    public void Deserialize(BinaryReader stream, float version)
    {
      this.RenderDistance = BinUtils.ReadFloat(stream);
      this.RenderWidth = BinUtils.ReadInt32(stream);
      this.RenderHeight = BinUtils.ReadInt32(stream);
      this.Directions = BinUtils.ReadInt32(stream);
      this.AASamples = BinUtils.ReadInt32(stream);
      this.KeyframeGrain = BinUtils.ReadInt32(stream);
      this.InfoType = (ObjectInfoType) Enum.Parse(typeof (ObjectInfoType), BinUtils.ReadString(stream));
      if (!Enum.IsDefined(typeof (ObjectInfoType), (object) this.InfoType) && !this.InfoType.ToString().Contains(","))
        throw new InvalidOperationException("Enum Cast To ObjectInfoType Failed.");
      this.FitModel = BinUtils.ReadBool(stream);
      this.IsoHeight = BinUtils.ReadFloat(stream);
    }
  }
}
