// Decompiled with JetBrains decompiler
// Type: IsoPack.BlenderScriptTask
// Assembly: IsoTool, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A40E7877-59D4-416C-9526-ACFD66F37CC4
// Assembly location: C:\Program Files\Iso Tool\IsoTool.exe

using System;

namespace IsoPack
{
  public class BlenderScriptTask
  {
    public Action<float, string> Progress = (Action<float, string>) null;
    public Action After = (Action) null;
    public Action ExecAsync = (Action) null;
    public Action Before = (Action) null;
    public string Text = "";
    public string Title = "Blender Script";

    public virtual void Execute(ProgressWindow sc)
    {
    }

    public BlenderScriptTask()
    {
    }

    public BlenderScriptTask(string text, string title, Action execAsync)
    {
      this.Text = text;
      this.Title = title;
      this.ExecAsync = execAsync;
    }
  }
}
