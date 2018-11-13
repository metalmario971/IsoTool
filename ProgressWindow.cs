// Decompiled with JetBrains decompiler
// Type: IsoPack.ProgressWindow
// Assembly: IsoTool, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A40E7877-59D4-416C-9526-ACFD66F37CC4
// Assembly location: C:\Program Files\Iso Tool\IsoTool.exe

using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Reflection;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace IsoPack
{
  public class ProgressWindow : Form
  {
    private Process process = (Process) null;
    private MainForm _objMainForm = (MainForm) null;
    private IContainer components = (IContainer) null;
    private ProgressBar _pbProgress;
    private Label _lblStatus;

    public List<BlenderScriptTask> Scripts { get; set; } = new List<BlenderScriptTask>();

    public ProgressWindow(MainForm mf)
    {
      this.InitializeComponent();
      this._objMainForm = mf;
    }

    private string GetExportScript()
    {
      Assembly executingAssembly = Assembly.GetExecutingAssembly();
      string name = "IsoPack.iso_export.py";
      executingAssembly.GetManifestResourceNames();
      using (Stream manifestResourceStream = executingAssembly.GetManifestResourceStream(name))
      {
        using (StreamReader streamReader = new StreamReader(manifestResourceStream))
          return streamReader.ReadToEnd();
      }
    }

    private void BlenderScript_Load(object sender, EventArgs e)
    {
    }

    public Task RunScripts(Action after)
    {
      this.Show();
      this.BringToFront();
      Task task = new Task((Action) (() => this.RunScriptsSync(after)));
      task.Start();
      return task;
    }

    private void RunScriptsSync(Action after)
    {
      this._pbProgress.BeginInvoke((Delegate) (() => this._pbProgress.Value = 0));
      int i = 0;
      foreach (BlenderScriptTask script in this.Scripts)
      {
        BlenderScriptTask task = script;
        task.Progress = closure_0 ?? (closure_0 = (Action<float, string>) ((fProgress, strText) =>
        {
          int iCurProgress = (int) ((Decimal) i / (Decimal) this.Scripts.Count * new Decimal(100)) + (int) ((double) this.Scripts.Count / 100.0 * (double) fProgress);
          this._lblStatus.BeginInvoke((Delegate) (() => this._lblStatus.Text = strText));
          this._pbProgress.BeginInvoke((Delegate) (() => this._pbProgress.Value = iCurProgress));
        }));
        this._lblStatus.BeginInvoke((Delegate) (() => this._lblStatus.Text = task.Text));
        this.BeginInvoke((Delegate) (() => this.Text = task.Title));
        this.BeginInvoke((Delegate) (() =>
        {
          Action before = task.Before;
          if (before == null)
            return;
          before();
        }));
        if (task.ExecAsync != null)
          task.ExecAsync();
        task.Execute(this);
        this.BeginInvoke((Delegate) (() =>
        {
          Action after1 = task.After;
          if (after1 == null)
            return;
          after1();
        }));
        i++;
        this._pbProgress.BeginInvoke((Delegate) (closure_1 ?? (closure_1 = (Action) (() => this._pbProgress.Value = this.Scripts.Count > 0 ? (int) ((Decimal) i / (Decimal) this.Scripts.Count * new Decimal(100)) : 100))));
      }
      this._pbProgress.BeginInvoke((Delegate) (() => this._pbProgress.Value = 100));
      this.BeginInvoke((Delegate) (() =>
      {
        if (!this.IsHandleCreated)
          return;
        Action action = after;
        if (action != null)
          action();
        this.Scripts.Clear();
        this.Hide();
      }));
    }

    private bool ShouldConvert(Model m)
    {
      return true;
    }

    public void Start()
    {
      this.process = new Process();
      ProcessStartInfo processStartInfo = new ProcessStartInfo();
      processStartInfo.FileName = "cmd.exe";
      processStartInfo.RedirectStandardInput = true;
      processStartInfo.RedirectStandardOutput = true;
      processStartInfo.CreateNoWindow = true;
      processStartInfo.UseShellExecute = false;
      this._objMainForm.Log("Executing " + processStartInfo.Arguments);
      this.process.StartInfo = processStartInfo;
      this.process.Start();
    }

    public string GetOutputString(string output, string token)
    {
      string str = "";
      int num1 = output.IndexOf(token);
      int num2 = output.LastIndexOf(token);
      if (num1 > 0 && num2 > 0)
        str = output.Substring(num1 + 2, num2 - (num1 + 2));
      return str;
    }

    public string Execute(string fileArg, Model m)
    {
      this.process.StandardInput.WriteLine(fileArg);
      this.process.StandardInput.Flush();
      this.process.StandardInput.Close();
      if (m != null)
        m.ScriptStatus = ScriptStatus.Success;
      string end = this.process.StandardOutput.ReadToEnd();
      if (this.GetOutputString(end, "$2").ToLower() == "error" && m != null)
      {
        ++m.Errors;
        m.ScriptStatus = ScriptStatus.Error;
      }
      this.process.Close();
      this.process = (Process) null;
      this._objMainForm.Log(end);
      return end;
    }

    public BlendFileInfo GetBlendFileInfo(string file)
    {
      BlendFileInfo blendFileInfo = (BlendFileInfo) null;
      try
      {
        string validBlenderPath = this._objMainForm.IsoPackFile.AppSettings.GetValidBlenderPath();
        this._objMainForm.Log("Blender path " + validBlenderPath);
        string fullPath1 = Path.GetFullPath(this._objMainForm.IsoPackFile.AppSettings.PythonScriptPath);
        string fullPath2 = Path.GetFullPath(file);
        if (!File.Exists(validBlenderPath))
        {
          this._objMainForm.Log("Error: Blender Exe '" + validBlenderPath + "' did not exist");
          return (BlendFileInfo) null;
        }
        if (!File.Exists(fullPath1))
        {
          this._objMainForm.Log("Error: Blender Exe '" + fullPath1 + "' did not exist");
          return (BlendFileInfo) null;
        }
        if (!File.Exists(fullPath2))
        {
          this._objMainForm.Log("Error: Blender Exe '" + fullPath2 + "' did not exist");
          return (BlendFileInfo) null;
        }
        string fileArg = "\"" + validBlenderPath + "\" \"" + fullPath2 + "\" --background --python \"" + fullPath1 + "\" -- -fileinfo \"a\" ";
        this.Start();
        string outputString = this.GetOutputString(this.Execute(fileArg, (Model) null), "$3");
        if (string.IsNullOrEmpty(outputString))
          this._objMainForm.Log("Error: The python script returned no data.  There is probably a syntax error.");
        try
        {
          blendFileInfo = JsonConvert.DeserializeObject<BlendFileInfo>(outputString);
        }
        catch (JsonSerializationException ex)
        {
          this._objMainForm.Log("Error processing json:\r\n" + ex.ToString());
        }
      }
      catch (Exception ex)
      {
        this._objMainForm.Log("Error: " + ex.ToString());
        return (BlendFileInfo) null;
      }
      return blendFileInfo;
    }

    public void RenderModelAction(string strModName, int iDirections, int iWidth, int iHeight, float fDistance, int iAASamples, int iKeyframeGrain, string blendFile, string strObjName, string strActName, bool bFitModel)
    {
      try
      {
        string validBlenderPath = this._objMainForm.IsoPackFile.AppSettings.GetValidBlenderPath();
        this._objMainForm.Log("Blender path " + validBlenderPath);
        string fullPath1 = Path.GetFullPath(validBlenderPath);
        string fullPath2 = Path.GetFullPath(this._objMainForm.IsoPackFile.AppSettings.PythonScriptPath);
        string fullPath3 = Path.GetFullPath(blendFile);
        if (!File.Exists(fullPath1))
          this._objMainForm.Log("Error: Blender Exe '" + fullPath1 + "' did not exist");
        else if (!File.Exists(fullPath2))
          this._objMainForm.Log("Error: Blender Exe '" + fullPath2 + "' did not exist");
        else if (!File.Exists(fullPath3))
        {
          this._objMainForm.Log("Error: Blender Exe '" + fullPath3 + "' did not exist");
        }
        else
        {
          string pathForModelName = this._objMainForm.IsoPackFile.AppSettings.GetTemporaryOutputPathForModelName(strModName);
          string str1 = iDirections.ToString();
          string str2 = iWidth.ToString();
          string str3 = iHeight.ToString();
          string str4 = fDistance.ToString();
          string str5 = iAASamples.ToString();
          string str6 = iKeyframeGrain.ToString();
          string str7 = bFitModel ? "True" : "False";
          if (Directory.Exists(pathForModelName))
          {
            try
            {
              this._objMainForm.Log("Deleting the contents of '" + pathForModelName + "'.");
              Directory.Delete(pathForModelName, true);
            }
            catch (Exception ex)
            {
              this._objMainForm.Log("Warning: Failed to delete the contents of '" + pathForModelName + "'. You may get invalid sprites.  Make sure all files and folders are closed which reside in that directory.");
            }
          }
          if (!Directory.Exists(pathForModelName))
          {
            this._objMainForm.Log("Creating directory " + pathForModelName);
            Directory.CreateDirectory(pathForModelName);
          }
          this._objMainForm.Log("Converting \"" + fullPath3 + "\"");
          string fileArg = "\"" + fullPath1 + "\" \"" + fullPath3 + "\" --background --python \"" + fullPath2 + "\" -- -outpath \"" + pathForModelName + "\" -angles \"" + str1 + "\" -width \"" + str2 + "\" -ieight \"" + str3 + "\" -dist \"" + str4 + "\" -aasamples \"" + str5 + "\" -keyframegrain \"" + str6 + "\" -objname \"" + strObjName + "\" -actname \"" + strActName + "\" -fitmodel \"" + str7 + "\" ";
          this.Start();
          this.Execute(fileArg, (Model) null);
        }
      }
      catch (Exception ex)
      {
        this._objMainForm.Log("Error: " + ex.ToString());
      }
    }

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof (ProgressWindow));
      this._pbProgress = new ProgressBar();
      this._lblStatus = new Label();
      this.SuspendLayout();
      this._pbProgress.Location = new Point(13, 57);
      this._pbProgress.Name = "_pbProgress";
      this._pbProgress.Size = new Size(398, 23);
      this._pbProgress.TabIndex = 0;
      this._lblStatus.AutoSize = true;
      this._lblStatus.Font = new Font("Microsoft Sans Serif", 9.75f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this._lblStatus.Location = new Point(13, 26);
      this._lblStatus.Name = "_lblStatus";
      this._lblStatus.Size = new Size(48, 16);
      this._lblStatus.TabIndex = 1;
      this._lblStatus.Text = "Status:";
      this.AutoScaleDimensions = new SizeF(6f, 13f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.ClientSize = new Size(423, 92);
      this.Controls.Add((Control) this._lblStatus);
      this.Controls.Add((Control) this._pbProgress);
      this.FormBorderStyle = FormBorderStyle.FixedDialog;
      this.Icon = (Icon) componentResourceManager.GetObject("$this.Icon");
      this.Name = "BlenderScript";
      this.Text = "BlenderScript";
      this.Load += new EventHandler(this.BlenderScript_Load);
      this.ResumeLayout(false);
      this.PerformLayout();
    }
  }
}
