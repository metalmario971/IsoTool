// Decompiled with JetBrains decompiler
// Type: IsoPack.BlenderScriptTask_Render
// Assembly: IsoTool, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A40E7877-59D4-416C-9526-ACFD66F37CC4
// Assembly location: C:\Program Files\Iso Tool\IsoTool.exe

namespace IsoPack
{
  public class BlenderScriptTask_Render : BlenderScriptTask
  {
    public string strModName { get; set; }

    public int iDirections { get; set; }

    public int iWidth { get; set; }

    public int iHeight { get; set; }

    public float fDistance { get; set; }

    public int iAASamples { get; set; }

    public int iKeyframeGrain { get; set; }

    public string blendFile { get; set; }

    public string strObjName { get; set; }

    public string strActName { get; set; }

    public bool bFitModel { get; set; }

    public override void Execute(ProgressWindow sc)
    {
      sc.RenderModelAction(this.strModName, this.iDirections, this.iWidth, this.iHeight, this.fDistance, this.iAASamples, this.iKeyframeGrain, this.blendFile, this.strObjName, this.strActName, this.bFitModel);
    }
  }
}
