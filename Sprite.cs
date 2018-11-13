// Decompiled with JetBrains decompiler
// Type: IsoPack.Sprite
// Assembly: IsoTool, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A40E7877-59D4-416C-9526-ACFD66F37CC4
// Assembly location: C:\Program Files\Iso Tool\IsoTool.exe

using System;
using System.Collections.Generic;
using System.IO;

namespace IsoPack
{
  public class Sprite
  {
    public Model Model { get; set; } = (Model) null;

    public int Direction { get; set; } = -1;

    public string Name { get; set; } = "";

    public bool Loop { get; set; } = true;

    public List<Frame> Frames { get; set; } = new List<Frame>();

    public string FileLocation { get; set; } = "";

    public float Origin_X { get; set; } = 0.0f;

    public float Origin_Y { get; set; } = 0.0f;

    public void SortFrames()
    {
      this.Frames.Sort((Comparison<Frame>) ((x, y) => x.FrameId.CompareTo(y.FrameId)));
    }

    public void Serialize(BinaryWriter stream, int version)
    {
      BinUtils.WriteInt32(this.Direction, stream);
      BinUtils.WriteString(this.Name, stream);
      BinUtils.WriteBool(this.Loop, stream);
      BinUtils.WriteString(this.FileLocation, stream);
      if (version >= 6)
      {
        BinUtils.WriteFloat(this.Origin_X, stream);
        BinUtils.WriteFloat(this.Origin_Y, stream);
      }
      BinUtils.WriteInt32(this.Frames.Count, stream);
      foreach (Frame frame in this.Frames)
        frame.Serialize(stream, (float) version);
    }

    public void Deserialize(BinaryReader stream, int version)
    {
      this.Direction = BinUtils.ReadInt32(stream);
      this.Name = BinUtils.ReadString(stream);
      this.Loop = BinUtils.ReadBool(stream);
      this.FileLocation = BinUtils.ReadString(stream);
      if (version >= 6)
      {
        this.Origin_X = BinUtils.ReadFloat(stream);
        this.Origin_Y = BinUtils.ReadFloat(stream);
      }
      int num = BinUtils.ReadInt32(stream);
      for (int index = 0; index < num; ++index)
      {
        Frame frame = new Frame();
        frame.Deserialize(stream, (float) version);
        frame.Sprite = this;
        this.Frames.Add(frame);
      }
    }
  }
}
