// Decompiled with JetBrains decompiler
// Type: IsoPack.Frame
// Assembly: IsoTool, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A40E7877-59D4-416C-9526-ACFD66F37CC4
// Assembly location: C:\Program Files\Iso Tool\IsoTool.exe

using System.Drawing;
using System.IO;

namespace IsoPack
{
  public class Frame
  {
    public int Delay = 41;
    public int texid = -1;
    public string Location = "";
    public string Name = "";
    public int FrameId = -1;
    public int x;
    public int y;
    public int w;
    public int h;

    public MtTex ImageTemp { get; set; } = (MtTex) null;

    public Sprite Sprite { get; set; } = (Sprite) null;

    public void Serialize(BinaryWriter stream, float version)
    {
      BinUtils.WriteInt32(this.x, stream);
      BinUtils.WriteInt32(this.y, stream);
      BinUtils.WriteInt32(this.w, stream);
      BinUtils.WriteInt32(this.h, stream);
      BinUtils.WriteInt32(this.texid, stream);
      BinUtils.WriteInt32(this.Delay, stream);
      BinUtils.WriteString(this.Location, stream);
      BinUtils.WriteString(this.Name, stream);
      BinUtils.WriteInt32(this.FrameId, stream);
    }

    public void Deserialize(BinaryReader stream, float version)
    {
      Block block = new Block(stream);
      this.x = BinUtils.ReadInt32(stream);
      this.y = BinUtils.ReadInt32(stream);
      this.w = BinUtils.ReadInt32(stream);
      this.h = BinUtils.ReadInt32(stream);
      this.texid = BinUtils.ReadInt32(stream);
      this.Delay = BinUtils.ReadInt32(stream);
      this.Location = BinUtils.ReadString(stream);
      this.Name = BinUtils.ReadString(stream);
      this.FrameId = BinUtils.ReadInt32(stream);
    }

    public Frame CreateCopy()
    {
      Frame newFrame = new Frame()
      {
        Delay = this.Delay,
        x = this.x,
        y = this.y,
        w = this.w,
        h = this.h,
        texid = this.texid
      };
      newFrame.ImageTemp = this.ImageTemp.CreateCopy(newFrame);
      newFrame.Location = this.Location;
      newFrame.Name = this.Name;
      newFrame.Sprite = this.Sprite;
      newFrame.FrameId = this.FrameId;
      return newFrame;
    }

    public void LoadFrameImage(string file, MainForm mf)
    {
      Bitmap bmp;
      if (!File.Exists(file))
      {
        bmp = mf.TextureInfo.PackedTexture.GetDefaultXImage();
      }
      else
      {
        using (Bitmap bitmap = new Bitmap(file))
          bmp = new Bitmap((Image) bitmap);
      }
      this.ImageTemp = new MtTex(bmp, this);
    }
  }
}
