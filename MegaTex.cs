// Decompiled with JetBrains decompiler
// Type: IsoPack.MegaTex
// Assembly: IsoTool, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A40E7877-59D4-416C-9526-ACFD66F37CC4
// Assembly location: C:\Program Files\Iso Tool\IsoTool.exe

using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;

namespace IsoPack
{
  public class MegaTex
  {
    private MtNode _pRoot = (MtNode) null;
    public Bitmap MasterImage = (Bitmap) null;

    public int Id { get; set; } = -1;

    public string Name { get; set; }

    public MegaTex()
    {
    }

    public MegaTex(string name, int id)
    {
      this.Name = name;
      this.Id = id;
    }

    public void Serialize(BinaryWriter stream, bool packImages)
    {
      if (this.MasterImage == null)
        throw new Exception("The master image was null for the given MegaTex " + this.Name);
      ImageConverter imageConverter = new ImageConverter();
      BinUtils.WriteString(this.Name, stream);
      BinUtils.WriteInt32(this.Id, stream);
      if (packImages)
      {
        byte[] b = (byte[]) imageConverter.ConvertTo((object) this.MasterImage, typeof (byte[]));
        BinUtils.WriteInt32(b.Length, stream);
        BinUtils.WriteBlock(b, stream);
      }
      else
        BinUtils.WriteInt32(0, stream);
    }

    public void Deserialize(BinaryReader stream, bool packedTextures)
    {
      this.Name = BinUtils.ReadString(stream);
      this.Id = BinUtils.ReadInt32(stream);
      int count = BinUtils.ReadInt32(stream);
      if (count <= 0)
        return;
      if (packedTextures)
        ;
      using (MemoryStream memoryStream = new MemoryStream(BinUtils.ReadBlock(count, stream)))
        this.MasterImage = new Bitmap((Stream) memoryStream);
    }

    public void Compile(List<MtTex> Texs, int iMinTexSize, int iMaxTexSize, int iGrowBy)
    {
      if (iMinTexSize > iMaxTexSize)
        throw new Exception("Min texture size is greater than max texture size.");
      Texs.Sort((Comparison<MtTex>) ((a, b) =>
      {
        int num1 = a.Image.Width * a.Image.Height;
        int num2 = b.Image.Width * b.Image.Height;
        if (num1 > num2)
          return 1;
        return num1 < num2 ? -1 : 0;
      }));
      int num3 = iMinTexSize;
      int num4 = 0;
      List<MtTex> mtTexList = new List<MtTex>();
      while (num3 <= iMaxTexSize)
      {
        this._pRoot = new MtNode();
        this._pRoot.Rect.p0.Construct(0, 0);
        this._pRoot.Rect.p1.Construct(num3, num3);
        bool flag = true;
        mtTexList.Clear();
        foreach (MtTex mtTex in Texs)
          mtTex.Node = (MtNode) null;
        foreach (MtTex tex in Texs)
        {
          if (tex.Image != null)
          {
            MtNode mtNode = this._pRoot.Plop(tex);
            if (mtNode != null)
            {
              if (mtNode.Tex != null)
              {
                int num1 = 0 + 1;
              }
              else
                mtNode.Tex = tex;
              tex.Node = mtNode;
              mtTexList.Add(tex);
            }
            else
            {
              num3 += iGrowBy;
              ++num4;
              flag = false;
              break;
            }
          }
          else
          {
            int num2 = 0 + 1;
          }
        }
        if (flag)
          break;
      }
      foreach (MtTex mtTex in mtTexList)
        Texs.Remove(mtTex);
      if (num3 <= iMaxTexSize)
        ;
      this.MasterImage = new Bitmap(num3, num3);
      foreach (MtTex mtTex1 in mtTexList)
      {
        foreach (MtTex mtTex2 in mtTexList)
        {
          if (mtTex1 != mtTex2)
          {
            if (mtTex1.Node == mtTex2.Node)
            {
              int num1 = 0 + 1;
            }
            if (mtTex1.Node.Rect.p0.x == mtTex2.Node.Rect.p0.x && mtTex1.Node.Rect.p0.y == mtTex2.Node.Rect.p0.y)
            {
              int num2 = 0 + 1;
            }
          }
        }
      }
      foreach (MtTex mtTex in mtTexList)
      {
        GraphicsUnit pageUnit = GraphicsUnit.Pixel;
        Rectangle destRegion = new Rectangle(mtTex.Node.Rect.p0.x, mtTex.Node.Rect.p0.y, mtTex.Node.Rect.Width(), mtTex.Node.Rect.Height());
        Rectangle srcRegion = Rectangle.Round(mtTex.Image.GetBounds(ref pageUnit));
        Globals.CopyRegionIntoImage(mtTex.Image, srcRegion, ref this.MasterImage, destRegion);
        mtTex.Frame.x = mtTex.Node.Rect.p0.x;
        mtTex.Frame.y = mtTex.Node.Rect.p0.y;
        mtTex.Frame.w = mtTex.Node.Rect.Width();
        mtTex.Frame.h = mtTex.Node.Rect.Height();
        mtTex.Frame.texid = this.Id;
      }
    }
  }
}
