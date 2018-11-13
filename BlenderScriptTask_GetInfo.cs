// Decompiled with JetBrains decompiler
// Type: IsoPack.BlenderScriptTask_GetInfo
// Assembly: IsoTool, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A40E7877-59D4-416C-9526-ACFD66F37CC4
// Assembly location: C:\Program Files\Iso Tool\IsoTool.exe

namespace IsoPack
{
  public class BlenderScriptTask_GetInfo : BlenderScriptTask
  {
    public BlendFileInfo BlendFileInfo = (BlendFileInfo) null;

    public string strFile { get; set; } = "";

    public override void Execute(ProgressWindow sc)
    {
      this.BlendFileInfo = sc.GetBlendFileInfo(this.strFile);
    }
  }
}
