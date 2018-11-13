// Decompiled with JetBrains decompiler
// Type: IsoPack.AppSettings
// Assembly: IsoTool, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A40E7877-59D4-416C-9526-ACFD66F37CC4
// Assembly location: C:\Program Files\Iso Tool\IsoTool.exe

using System;
using System.IO;

namespace IsoPack
{
  public class AppSettings
  {
    public string BlenderPath { get; set; } = "%AUTO%";

    public string TempFolder { get; set; } = "%AUTO%";

    public string ProjectRoot { get; set; } = ".\\";

    public string PythonScriptPath { get; set; } = ".\\iso_export.py";

    public int GrowBy { get; set; } = 128;

    public int MaxSize { get; set; } = 2048;

    public int MinSize { get; set; } = 256;

    public bool PackImages { get; set; } = false;

    public bool SaveImagesDebug { get; set; } = false;

    public TextureFileType TextureFileType { get; set; } = TextureFileType.Png;

    public string GetOrCreateTempFolder()
    {
      string str1;
      if (this.TempFolder.ToUpper() == "%AUTO%")
      {
        string str2 = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "IsoTool");
        if (!Directory.Exists(str2))
          Directory.CreateDirectory(str2);
        string path = Path.Combine(str2, "tmp");
        if (!Directory.Exists(path))
          Directory.CreateDirectory(path);
        str1 = path;
      }
      else
        str1 = this.TempFolder;
      return str1;
    }

    public string GetTemporaryOutputPathForModelName(string strModName)
    {
      return Path.Combine(Path.GetFullPath(this.GetOrCreateTempFolder()), strModName);
    }

    public string GetValidBlenderPath()
    {
      if (this.BlenderPath.ToUpper() == "%AUTO%")
      {
        string path1 = Path.Combine(Environment.ExpandEnvironmentVariables("%ProgramW6432%"), "Blender Foundation\\Blender\\blender.exe");
        if (File.Exists(path1))
          return path1;
        string path2 = Path.Combine(Environment.ExpandEnvironmentVariables("%ProgramFiles(x86)%"), "Blender Foundation\\Blender\\blender.exe");
        if (File.Exists(path2))
          return path2;
      }
      return Path.GetFullPath(this.BlenderPath);
    }

    public void Serialize(BinaryWriter stream)
    {
      byte[] numArray = new byte[0];
      BinUtils.WriteString(this.BlenderPath, stream);
      BinUtils.WriteString(this.TempFolder, stream);
      BinUtils.WriteString(this.ProjectRoot, stream);
      BinUtils.WriteString(this.PythonScriptPath, stream);
      BinUtils.WriteInt32(this.GrowBy, stream);
      BinUtils.WriteInt32(this.MaxSize, stream);
      BinUtils.WriteInt32(this.MinSize, stream);
      BinUtils.WriteBool(this.PackImages, stream);
      BinUtils.WriteBool(this.SaveImagesDebug, stream);
      BinUtils.WriteString(this.TextureFileType.ToString(), stream);
    }

    public void Deserialize(BinaryReader stream)
    {
      this.BlenderPath = BinUtils.ReadString(stream);
      this.TempFolder = BinUtils.ReadString(stream);
      this.ProjectRoot = BinUtils.ReadString(stream);
      this.PythonScriptPath = BinUtils.ReadString(stream);
      this.GrowBy = BinUtils.ReadInt32(stream);
      this.MaxSize = BinUtils.ReadInt32(stream);
      this.MinSize = BinUtils.ReadInt32(stream);
      this.PackImages = BinUtils.ReadBool(stream);
      this.SaveImagesDebug = BinUtils.ReadBool(stream);
      this.TextureFileType = (TextureFileType) Enum.Parse(typeof (TextureFileType), BinUtils.ReadString(stream));
      if (!Enum.IsDefined(typeof (TextureFileType), (object) this.TextureFileType) && !this.TextureFileType.ToString().Contains(","))
        throw new InvalidOperationException("Enum Cast To TextureFileType Failed.");
    }
  }
}
