// Decompiled with JetBrains decompiler
// Type: IsoPack.PackedTexture
// Assembly: IsoTool, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A40E7877-59D4-416C-9526-ACFD66F37CC4
// Assembly location: C:\Program Files\Iso Tool\IsoTool.exe

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Reflection;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace IsoPack
{
  public class PackedTexture
  {
    private MainForm _objMainForm = (MainForm) null;
    private Bitmap DefaultImage = (Bitmap) null;

    public List<MegaTex> Images { get; set; } = new List<MegaTex>();

    public PackedTexture(MainForm mf)
    {
      this._objMainForm = mf;
    }

    public Bitmap GetImageForFrame(Frame f)
    {
      if (f == null)
      {
        this._objMainForm.Log("Given Frame was null");
        return (Bitmap) null;
      }
      if (f.ImageTemp != null)
      {
        if (f.ImageTemp.Image != null)
          return f.ImageTemp.Image;
        Debugger.Break();
      }
      return this.GetSubImageForRect(this.GetRectForFrame(f), f.texid);
    }

    private Rectangle GetRectForFrame(Frame f)
    {
      return new Rectangle(f.x, f.y, f.w, f.h);
    }

    private MegaTex GetTexById(int texid)
    {
      foreach (MegaTex image in this.Images)
      {
        if (image.Id == texid)
          return image;
      }
      return (MegaTex) null;
    }

    private Bitmap GetSubImageForRect(Rectangle r, int texid)
    {
      MegaTex texById = this.GetTexById(texid);
      if (texById == null || texById.MasterImage == null)
        return this.GetDefaultXImage();
      Rectangle destRegion = new Rectangle(0, 0, r.Width, r.Height);
      Bitmap destBitmap = new Bitmap(r.Width, r.Height);
      Globals.CopyRegionIntoImage(texById.MasterImage, r, ref destBitmap, destRegion);
      return destBitmap;
    }

    public Bitmap GetDefaultXImage()
    {
      if (this.DefaultImage == null)
      {
        string str = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "no.png");
        if (File.Exists(str))
        {
          this.DefaultImage = new Bitmap(str);
        }
        else
        {
          Bitmap bitmap = new Bitmap(64, 64, PixelFormat.Format24bppRgb);
        }
      }
      return this.DefaultImage;
    }

    public void Serialize(BinaryWriter stream, bool packImages)
    {
      BinUtils.WriteInt32(this.Images.Count, stream);
      foreach (MegaTex image in this.Images)
        image.Serialize(stream, packImages);
    }

    public void Deserialize(BinaryReader stream, bool packedTextures)
    {
      int num = BinUtils.ReadInt32(stream);
      for (int index = 0; index < num; ++index)
      {
        MegaTex megaTex = new MegaTex();
        megaTex.Deserialize(stream, packedTextures);
        this.Images.Add(megaTex);
      }
    }

    public Task Pack()
    {
      bool valid = true;
      List<MtTex> texs = new List<MtTex>();
      int maxsize = this._objMainForm.IsoPackFile.AppSettings.MaxSize;
      this._objMainForm.Blender.Scripts.Add(new BlenderScriptTask("Gathering Sprites", "Pack Texture", (Action) (() =>
      {
        this.GatherModelSprites(texs);
        maxsize = this.CheckMaxSize(texs);
        if (maxsize != -1)
          return;
        valid = false;
      })));
      BlenderScriptTask packTask = new BlenderScriptTask("Packing", "Pack Texture", (Action) null);
      this._objMainForm.Blender.Scripts.Add(packTask);
      Action action = (Action) (() =>
      {
        try
        {
          if (!valid)
            return;
          int id = 0;
          int count = texs.Count;
          List<MegaTex> megaTexList = new List<MegaTex>();
          while (texs.Count > 0)
          {
            MegaTex megaTex = new MegaTex(this._objMainForm.IsoPackFile.GetAtlasName(id), id++);
            megaTex.Compile(texs, this._objMainForm.IsoPackFile.AppSettings.MinSize, maxsize, this._objMainForm.IsoPackFile.AppSettings.GrowBy);
            megaTexList.Add(megaTex);
            if (packTask.Progress != null)
              packTask.Progress((float) texs.Count / (float) count, "Created MegaTexture " + id.ToString());
          }
          lock (this.Images)
          {
            this.Images.Clear();
            this.Images = megaTexList;
          }
        }
        catch (Exception ex)
        {
          this._objMainForm.BeginInvoke((Delegate) (() => this._objMainForm.Log("Failed to pack texture. \r\n" + ex.ToString())));
        }
      });
      packTask.ExecAsync = action;
      return this._objMainForm.Blender.RunScripts((Action) (() => {}));
    }

    private void GatherModelSprites(List<MtTex> texs)
    {
      foreach (Model model in this._objMainForm.IsoPackFile.Models)
      {
        foreach (Sprite sprite in model.Sprites)
          this.GatherSprite(texs, sprite);
      }
    }

    private void GatherSprite(List<MtTex> texs, Sprite s)
    {
      foreach (Frame frame in s.Frames)
      {
        if (frame.ImageTemp != null)
        {
          texs.Add(frame.ImageTemp);
        }
        else
        {
          Bitmap imageForFrame = this._objMainForm.TextureInfo.PackedTexture.GetImageForFrame(frame);
          frame.ImageTemp = new MtTex(imageForFrame, frame);
          Debugger.Break();
        }
      }
    }

    private int CheckMaxSize(List<MtTex> texs)
    {
      bool flag1 = false;
      bool flag2 = false;
      int num1 = this._objMainForm.IsoPackFile.AppSettings.MaxSize;
      int num2 = 16384;
      foreach (MtTex mtTex in texs)
      {
        if (mtTex.Image.Width > this._objMainForm.IsoPackFile.AppSettings.MaxSize)
        {
          string str1 = "Error: The Max Texture Size '" + (object) num1 + "' is smaller than the size of one or more textures' width '" + (object) mtTex.Image.Width + "'.";
          string str2;
          if (mtTex.Image.Width <= num2)
          {
            str2 = str1 + "Increasing texture size to '" + (object) mtTex.Image.Width + "'";
            num1 = mtTex.Image.Width;
          }
          else
          {
            str2 = str1 + "Failed to pack texture.  Consider making the image smaller.";
            flag2 = true;
          }
          if (!flag1)
          {
            int num3 = (int) MessageBox.Show(str2);
            this._objMainForm.Log(str2);
            flag1 = true;
          }
        }
        if (mtTex.Image.Height > this._objMainForm.IsoPackFile.AppSettings.MaxSize)
        {
          string str1 = "Error: The Max Texture Size '" + (object) num1 + "' is smaller than the size of one or more textures' height '" + (object) mtTex.Image.Height + "'.";
          string str2;
          if (mtTex.Image.Height <= num2)
          {
            str2 = str1 + "Increasing texture size to '" + (object) mtTex.Image.Height + "'";
            num1 = mtTex.Image.Height;
          }
          else
          {
            str2 = str1 + "Failed to pack texture.  Consider making the image smaller.";
            flag2 = true;
          }
          if (!flag1)
          {
            int num3 = (int) MessageBox.Show(str2);
            this._objMainForm.Log(str2);
            flag1 = true;
          }
        }
        if (flag2)
          return -1;
      }
      return num1;
    }
  }
}
