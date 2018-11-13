// Decompiled with JetBrains decompiler
// Type: IsoPack.Globals
// Assembly: IsoTool, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A40E7877-59D4-416C-9526-ACFD66F37CC4
// Assembly location: C:\Program Files\Iso Tool\IsoTool.exe

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Windows.Forms;

namespace IsoPack
{
  public class Globals
  {
    public static string SupportedLoadSpriteImageFilter = "Image Files (*.png;*.bmp;*.gif;*.jpg)|*.png;*.bmp;*.gif;*.jpg|All files (*.*)|*.*";
    public static string GifFilter = "Gif Files (*.gif)|*.gif;|All files (*.*)|*.*";
    public static string ExeFilter = "Exe Files (*.exe)|*.exe|All files (*.*)|*.*";
    public static string AllFilter = "All files (*.*)|*.*";

    public static string GetNewFrameName(MainForm mf)
    {
      string str = "NewFrame-0";
      int num = 0;
      foreach (Model model in mf.IsoPackFile.Models)
      {
        str = string.Format("NewFrame-{0}", (object) num);
        bool flag = false;
        foreach (Sprite sprite in model.Sprites)
        {
          foreach (Frame frame in sprite.Frames)
          {
            if (frame.Name.Equals(str))
            {
              flag = true;
              ++num;
            }
            if (flag)
              break;
          }
          if (flag)
            break;
        }
      }
      return str;
    }

    public static void CopyRegionIntoImage(Bitmap srcBitmap, Rectangle srcRegion, ref Bitmap destBitmap, Rectangle destRegion)
    {
      using (Graphics graphics = Graphics.FromImage((Image) destBitmap))
        graphics.DrawImage((Image) srcBitmap, destRegion, srcRegion, GraphicsUnit.Pixel);
    }

    public static void SwapControl(Control ctlToReplace, Control newCtl)
    {
      newCtl.Bounds = ctlToReplace.Bounds;
      Control parent = ctlToReplace.Parent;
      newCtl.Anchor = ctlToReplace.Anchor;
      newCtl.Dock = ctlToReplace.Dock;
      parent.Controls.Remove(ctlToReplace);
      parent.Controls.Add(newCtl);
    }

    public static void SwapSpriteControl(PictureBox ctlToReplace, PictureBoxWithInterpolationMode newCtl)
    {
      Globals.SwapControl((Control) ctlToReplace, (Control) newCtl);
      newCtl.SizeMode = ctlToReplace.SizeMode;
      newCtl.InterpolationMode = InterpolationMode.NearestNeighbor;
      newCtl.BorderStyle = ctlToReplace.BorderStyle;
      newCtl.BackColor = ctlToReplace.BackColor;
    }

    public static byte[] Combine(byte[] a, byte[] b)
    {
      byte[] numArray = new byte[a.Length + b.Length];
      Buffer.BlockCopy((Array) a, 0, (Array) numArray, 0, a.Length);
      Buffer.BlockCopy((Array) b, 0, (Array) numArray, a.Length, b.Length);
      return numArray;
    }

    public static void SetToolTip(List<Control> ctls, string text)
    {
      foreach (Control ctl in ctls)
        Globals.SetToolTip(ctl, text);
    }

    public static void SetToolTip(Control ctl, string text)
    {
      ToolTip toolTip = new ToolTip();
      toolTip.AutoPopDelay = (int) short.MaxValue;
      toolTip.InitialDelay = 1000;
      toolTip.ReshowDelay = 500;
      toolTip.ShowAlways = false;
      bool flag = false;
      int num1 = 0;
      int num2 = 40;
      string caption = "";
      for (int index = 0; index < text.Length; ++index)
      {
        ++num1;
        if (num1 > num2)
          flag = true;
        if (flag && char.IsWhiteSpace(text[index]))
        {
          caption += "\r\n";
          flag = false;
          num1 = 0;
        }
        caption += text[index].ToString();
      }
      toolTip.SetToolTip(ctl, caption);
    }

    public static void SetListViewToSelectedObjectTag(ListView lv, object selected)
    {
      if (lv.Items.Count > 0 && selected != null)
      {
        foreach (ListViewItem listViewItem in lv.Items)
          listViewItem.Selected = listViewItem.Tag == selected;
      }
      lv.Select();
    }

    public static string[] GetValidUserFolder(string initialdir, bool multiple)
    {
      using (FolderBrowserDialog folderBrowserDialog = new FolderBrowserDialog())
      {
        folderBrowserDialog.Description = "Select a folder";
        if (folderBrowserDialog.ShowDialog() == DialogResult.OK)
          return new string[1]
          {
            folderBrowserDialog.SelectedPath
          };
      }
      return new string[0];
    }

    public static string[] GetValidOpenSaveUserFile(bool bSave, string saveFilter, string loadFilter, string defaultext, string initialdir, bool multiple)
    {
      int num = 0;
      FileDialog fileDialog;
      string str;
      if (!bSave)
      {
        fileDialog = (FileDialog) new OpenFileDialog();
        str = loadFilter;
        (fileDialog as OpenFileDialog).Multiselect = multiple;
      }
      else
      {
        fileDialog = (FileDialog) new SaveFileDialog();
        str = saveFilter;
      }
      string fullPath = Path.GetFullPath(initialdir);
      fileDialog.InitialDirectory = fullPath;
      fileDialog.DefaultExt = defaultext;
      fileDialog.Filter = str;
      fileDialog.FilterIndex = num;
      if (fileDialog.ShowDialog() == DialogResult.Cancel)
        return new string[0];
      if (!bSave && !File.Exists(fileDialog.FileName))
        return new string[0];
      return fileDialog.FileNames;
    }
  }
}
