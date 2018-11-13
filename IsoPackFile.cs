// Decompiled with JetBrains decompiler
// Type: IsoPack.IsoPackFile
// Assembly: IsoTool, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A40E7877-59D4-416C-9526-ACFD66F37CC4
// Assembly location: C:\Program Files\Iso Tool\IsoTool.exe

using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace IsoPack
{
  public class IsoPackFile
  {
    private MainForm _objMainForm;

    public List<Model> Models { get; set; } = new List<Model>();

    public ModelRenderParameters DefaultRenderParams { get; set; } = new ModelRenderParameters();

    public AppSettings AppSettings { get; set; } = new AppSettings();

    public string SavedFileLocation { get; private set; } = "";

    public string SavedFile { get; private set; } = "";

    public void ClearUI()
    {
      this.Models.Clear();
      this.SavedFileLocation = "";
      this.DefaultRenderParams = new ModelRenderParameters();
    }

    public IsoPackFile(MainForm mf)
    {
      this._objMainForm = mf;
    }

    public bool SaveAs(string loc)
    {
      if (string.IsNullOrEmpty(loc))
        return false;
      this.SavedFile = loc;
      this.SavedFileLocation = Path.GetDirectoryName(this.SavedFile);
      if (!Directory.Exists(this.SavedFileLocation))
        Directory.CreateDirectory(this.SavedFileLocation);
      this.RenameAtlasImages();
      return this.Save();
    }

    public bool Save()
    {
      if (!Directory.Exists(this.SavedFileLocation))
      {
        string str = "Failed to save, '" + this.SavedFileLocation + "' was not a valid directory.";
        int num = (int) MessageBox.Show(str, "Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
        this._objMainForm.Log(str);
        return false;
      }
      bool flag = false;
      try
      {
        this._objMainForm.Log("Packing Atlas Textures.");
        this._objMainForm.TextureInfo.PackedTexture.Pack().Wait();
        this._objMainForm.Log("Saving IP File.");
        this.SaveIpFile(this.SavedFile, this.AppSettings.PackImages);
        this._objMainForm.Log("Saved '" + this.SavedFile + "'");
        if (!this.AppSettings.PackImages)
        {
          this._objMainForm.Log("Saving Atlas Textures.");
          this.SaveAtlasImages(this.SavedFileLocation);
        }
        this.MarkSaved();
        flag = true;
        this._objMainForm.Log("Saved.");
      }
      catch (Exception ex)
      {
        int num = (int) MessageBox.Show("Failed to save file.\r\n " + ex.ToString());
      }
      return flag;
    }

    public bool Load(string loc)
    {
      try
      {
        this.Models.Clear();
        if (!File.Exists(loc))
        {
          this._objMainForm.Log("Error: file " + loc + " does not exist.");
          return false;
        }
        this.LoadIpFile(loc);
        this.LoadAtlasImages(Path.GetDirectoryName(loc));
        this.ParseSpriteImages();
        this.SavedFileLocation = Path.GetDirectoryName(loc);
        this.SavedFile = loc;
        return true;
      }
      catch (Exception ex)
      {
        string str = "Failed to load file:\r\n" + ex.ToString();
        int num = (int) MessageBox.Show(str, "Error", MessageBoxButtons.OK, MessageBoxIcon.Hand);
        this._objMainForm.Log(str);
        return false;
      }
    }

    private void MarkSaved()
    {
    }

    private void RenameAtlasImages()
    {
      foreach (MegaTex image in this._objMainForm.TextureInfo.PackedTexture.Images)
        image.Name = this.GetAtlasName(image.Id);
    }

    public string GetAtlasName(int id)
    {
      string str = this.AppSettings.TextureFileType == TextureFileType.Png ? ".png" : ".jpg";
      return Path.GetFileName(this.SavedFile) + "." + string.Format("{0:0000}", (object) id) + str;
    }

    private void LoadAtlasImages(string loc)
    {
      if (this._objMainForm.TextureInfo.PackedTexture == null)
        return;
      foreach (MegaTex image in this._objMainForm.TextureInfo.PackedTexture.Images)
      {
        Bitmap bitmap1;
        using (Bitmap bitmap2 = new Bitmap(Path.Combine(loc, image.Name)))
          bitmap1 = new Bitmap((Image) bitmap2);
        image.MasterImage = bitmap1;
      }
    }

    private void ParseSpriteImages()
    {
      foreach (Model model in this.Models)
      {
        foreach (Sprite sprite in model.Sprites)
        {
          foreach (Frame frame in sprite.Frames)
          {
            Bitmap imageForFrame = this._objMainForm.TextureInfo.PackedTexture.GetImageForFrame(frame);
            frame.ImageTemp = new MtTex(imageForFrame, frame);
          }
        }
      }
    }

    private void SaveAtlasImages(string loc)
    {
      if (this._objMainForm.TextureInfo.PackedTexture == null)
        return;
      foreach (MegaTex image in this._objMainForm.TextureInfo.PackedTexture.Images)
      {
        string filename = Path.Combine(loc, image.Name);
        image.MasterImage.Save(filename);
      }
    }

    private void SaveIpFile(string loc, bool textures)
    {
      using (BinaryWriter binaryWriter = new BinaryWriter((Stream) File.Open(loc, FileMode.Create)))
      {
        BinUtils.WriteString(this._objMainForm.GetProgramVersion().ToString(), binaryWriter);
        BinUtils.WriteInt32(80085, binaryWriter);
        BinUtils.WriteInt16((short) 8080, binaryWriter);
        BinUtils.WriteBlock(new byte[1]{ (byte) 1 }, binaryWriter);
        BinUtils.WriteFloat(3.14f, binaryWriter);
        BinUtils.WriteBool(true, binaryWriter);
        BinUtils.WriteBlock(new byte[5]
        {
          (byte) 5,
          (byte) 4,
          (byte) 3,
          (byte) 2,
          (byte) 1
        }, binaryWriter);
        this.AppSettings.Serialize(binaryWriter);
        this.DefaultRenderParams.Serialize(binaryWriter, (float) this._objMainForm.GetProgramVersion());
        BinUtils.WriteInt32(this.Models.Count, binaryWriter);
        foreach (Model model in this.Models)
          model.Serialize(binaryWriter, this._objMainForm.GetProgramVersion());
        this._objMainForm.TextureInfo.PackedTexture.Serialize(binaryWriter, textures);
      }
    }

    private void LoadIpFile(string loc)
    {
      using (BinaryReader binaryReader = new BinaryReader((Stream) File.Open(loc, FileMode.Open)))
      {
        string s = BinUtils.ReadString(binaryReader);
        int result;
        if (!int.TryParse(s, out result))
        {
          this._objMainForm.Log("Error: could not parse program version " + s);
        }
        else
        {
          if (result != this._objMainForm.GetProgramVersion())
          {
            string str = "Note: This was created with an older version. " + (object) result + ".  Press 'OK' to upgrade to version " + (object) this._objMainForm.GetProgramVersion() + ".";
            if (MessageBox.Show(str, "Upgrade Notification", MessageBoxButtons.OKCancel, MessageBoxIcon.Asterisk) == DialogResult.Cancel)
              return;
            this._objMainForm.Log(str);
          }
          BinUtils.ReadInt32(binaryReader);
          BinUtils.ReadInt16(binaryReader);
          BinUtils.ReadBlock(1, binaryReader);
          BinUtils.ReadFloat(binaryReader);
          BinUtils.ReadBool(binaryReader);
          BinUtils.ReadBlock(5, binaryReader);
          this.AppSettings.Deserialize(binaryReader);
          this.DefaultRenderParams.Deserialize(binaryReader, (float) result);
          int num = BinUtils.ReadInt32(binaryReader);
          for (int index = 0; index < num; ++index)
          {
            Model model = new Model();
            model.Deserialize(binaryReader, result);
            this.Models.Add(model);
          }
          this._objMainForm.TextureInfo.PackedTexture.Deserialize(binaryReader, this.AppSettings.PackImages);
        }
      }
    }
  }
}
