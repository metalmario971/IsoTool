// Decompiled with JetBrains decompiler
// Type: IsoPack.MainForm
// Assembly: IsoTool, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A40E7877-59D4-416C-9526-ACFD66F37CC4
// Assembly location: C:\Program Files\Iso Tool\IsoTool.exe

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace IsoPack
{
  public class MainForm : Form
  {
    private int c_iProgramVersion = 6;
    private List<AddEditForm> AddEditForms = new List<AddEditForm>();
    private IContainer components = (IContainer) null;
    private MenuStrip menuStrip1;
    private ToolStripMenuItem fileToolStripMenuItem;
    private ToolStripMenuItem exitToolStripMenuItem;
    private ToolStripMenuItem helpToolStripMenuItem;
    private ToolStripMenuItem aboutToolStripMenuItem;
    private ToolStripMenuItem openToolStripMenuItem;
    private StatusStrip statusStrip1;
    private ToolStripStatusLabel _lblInfo;
    private Button _btnAddObject;
    private Button _btnRemoveObject;
    private ListView _lsvModels;
    private ToolStripMenuItem newToolStripMenuItem;
    private ToolStripMenuItem saveToolStripMenuItem;
    private ToolStripMenuItem viewToolStripMenuItem;
    private ToolStripMenuItem textureToolStripMenuItem;
    private TabControl _objMainTabControl;
    private TabPage _tabModels;
    private Button _btnPackTexture;
    private ToolStripMenuItem saveToolStripMenuItem1;
    private ToolStripMenuItem helpToolStripMenuItem1;
    private ToolStripMenuItem quickstartToolStripMenuItem;

    public IsoPackFile IsoPackFile { get; set; } = (IsoPackFile) null;

    public Settings Settings { get; set; } = (Settings) null;

    public TextureInfo TextureInfo { get; set; } = (TextureInfo) null;

    public SpriteListView ModelFrames { get; set; } = (SpriteListView) null;

    public ProgressWindow Blender { get; set; } = (ProgressWindow) null;

    public void UpdateWindowUI(Model m, bool bDirty)
    {
      foreach (AddEditForm addEditForm in this.AddEditForms)
      {
        AddEditModel addEditModel = addEditForm as AddEditModel;
        if (addEditModel != null && addEditModel.Model == m)
        {
          addEditModel.UpdateUI(bDirty);
          break;
        }
      }
    }

    public void UpdateWindowUI(Sprite s, bool bDirty)
    {
      foreach (AddEditForm addEditForm in this.AddEditForms)
      {
        AddEditSprite addEditSprite = addEditForm as AddEditSprite;
        if (addEditSprite != null && addEditSprite.Sprite == s)
        {
          addEditSprite.UpdateUI(bDirty);
          break;
        }
      }
    }

    public void AddEditObject(Model m, bool bClose)
    {
      AddEditModel addEditModel1 = (AddEditModel) null;
      foreach (AddEditForm addEditForm in this.AddEditForms)
      {
        if (addEditForm is AddEditModel && (addEditForm as AddEditModel).Model == m)
        {
          addEditModel1 = addEditForm as AddEditModel;
          break;
        }
      }
      if (addEditModel1 == null && !bClose)
      {
        AddEditModel addEditModel2 = new AddEditModel(this);
        this.AddEditForms.Add((AddEditForm) addEditModel2);
        addEditModel2.Show(m);
      }
      else if (bClose)
        this.AddEditForms.Remove((AddEditForm) addEditModel1);
      else
        addEditModel1.BringToFront();
    }

    public void AddEditObject(Sprite s, Model m, bool bClose)
    {
      AddEditSprite addEditSprite1 = (AddEditSprite) null;
      foreach (AddEditForm addEditForm in this.AddEditForms)
      {
        if (addEditForm is AddEditSprite && (addEditForm as AddEditSprite).Sprite == s)
        {
          addEditSprite1 = addEditForm as AddEditSprite;
          break;
        }
      }
      if (addEditSprite1 == null && !bClose)
      {
        AddEditSprite addEditSprite2 = new AddEditSprite(this);
        this.AddEditForms.Add((AddEditForm) addEditSprite2);
        addEditSprite2.Show(s, m);
      }
      else if (bClose)
        this.AddEditForms.Remove((AddEditForm) addEditSprite1);
      else
        addEditSprite1.BringToFront();
    }

    public void AddEditObject(Frame fr, Sprite s, bool bClose)
    {
      AddEditFrame addEditFrame1 = (AddEditFrame) null;
      foreach (AddEditForm addEditForm in this.AddEditForms)
      {
        if (addEditForm is AddEditFrame && (addEditForm as AddEditFrame).Frame == fr)
        {
          addEditFrame1 = addEditForm as AddEditFrame;
          break;
        }
      }
      if (addEditFrame1 == null && !bClose)
      {
        AddEditFrame addEditFrame2 = new AddEditFrame(this);
        this.AddEditForms.Add((AddEditForm) addEditFrame2);
        addEditFrame2.Show(fr, s);
      }
      else if (bClose)
        this.AddEditForms.Remove((AddEditForm) addEditFrame1);
      else
        addEditFrame1.BringToFront();
    }

    public MainForm()
    {
      this.InitializeComponent();
    }

    private void MainForm_Load(object sender, EventArgs e)
    {
      this.Text = "Iso Tool v" + (object) ((double) this.c_iProgramVersion * 0.01);
      this.IsoPackFile = new IsoPackFile(this);
      this.TextureInfo = new TextureInfo(this);
      this.Settings = new Settings(this);
      this.ModelFrames = new SpriteListView(this, (Func<List<SpriteListViewItem>>) (() =>
      {
        List<SpriteListViewItem> spriteListViewItemList = new List<SpriteListViewItem>();
        foreach (Model model in this.IsoPackFile.Models)
        {
          Frame frame = model.Sprites.Count <= 0 || model.Sprites[0].Frames.Count <= 0 ? (Frame) null : model.Sprites[0].Frames[0];
          spriteListViewItemList.Add(new SpriteListViewItem((object) model, frame));
        }
        return spriteListViewItemList;
      }), (Action<List<object>>) (x =>
      {
        foreach (object obj in x)
        {
          if (obj is Model)
            this.IsoPackFile.Models.Remove(obj as Model);
        }
        this.UpdateUI();
      }));
      Globals.SwapControl((Control) this._lsvModels, (Control) this.ModelFrames);
      this.ModelFrames.MouseDoubleClick += new MouseEventHandler(this._lsvModels_MouseDoubleClick);
      this.Blender = new ProgressWindow(this);
      this.ClearUI();
      this.SetTooltips();
      this.Log("Here we go!");
    }

    private void SetTooltips()
    {
      Globals.SetToolTip(new List<Control>()
      {
        (Control) this._btnAddObject
      }, "Add a new sprite or model, depending on which tab you have selected.");
      Globals.SetToolTip(new List<Control>()
      {
        (Control) this._btnRemoveObject
      }, "Remove a sprite or model, depending on which tab you have selected.");
    }

    public int GetProgramVersion()
    {
      return this.c_iProgramVersion;
    }

    private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
    {
      this.Settings.Show();
      this.Settings.BringToFront();
    }

    private void helpToolStripMenuItem_Click(object sender, EventArgs e)
    {
    }

    public void Log(string str)
    {
      this.BeginInvoke((Delegate) (() =>
      {
        this.Settings.Log(str);
        this._lblInfo.Text = str;
      }));
    }

    private void openToolStripMenuItem_Click(object sender, EventArgs e)
    {
      this.ClearUI();
      if (!this.IsoPackFile.Load(this.GetValidOpenSaveUserFile(false)))
        this.ClearUI();
      this.UpdateUI();
    }

    private void CheckEnableSaveMenu()
    {
      if (File.Exists(this.IsoPackFile.SavedFile))
        this.saveToolStripMenuItem1.Enabled = true;
      else
        this.saveToolStripMenuItem1.Enabled = false;
    }

    private void ClearUI()
    {
      this.IsoPackFile.ClearUI();
      this.Settings.ClearUI();
      this.TextureInfo.ClearUI();
      this.UpdateUI();
    }

    public void UpdateUI()
    {
      this.ModelFrames.UpdateListView();
      this.CheckEnableSaveMenu();
    }

    private string GetValidOpenSaveUserFile(bool bSave)
    {
      string[] openSaveUserFile = Globals.GetValidOpenSaveUserFile(bSave, "Isopack File (*.ip)|*.ip|All files (*.*)|*.*", "Isopack File (*.ip)|*.ip|All files (*.*)|*.*", "ip", Environment.GetFolderPath(Environment.SpecialFolder.Desktop), false);
      if ((uint) openSaveUserFile.Length > 0U)
        return openSaveUserFile[0];
      return "";
    }

    private Model GetSelectedModel()
    {
      return this.ModelFrames.GetSelectedObject() as Model;
    }

    private void _btnRemoveObject_Click(object sender, EventArgs e)
    {
      this.RemoveSelectedModel();
    }

    private void RemoveSelectedModel()
    {
      this.IsoPackFile.Models.Remove(this.GetSelectedModel());
      this.UpdateUI();
    }

    private void newToolStripMenuItem_Click(object sender, EventArgs e)
    {
      this.ClearUI();
    }

    private void saveToolStripMenuItem_Click(object sender, EventArgs e)
    {
      if (this.IsoPackFile == null)
        return;
      this.IsoPackFile.SaveAs(this.GetValidOpenSaveUserFile(true));
      this.UpdateUI();
    }

    private void _lsvModels_SelectedIndexChanged(object sender, EventArgs e)
    {
    }

    private void _lsvModels_MouseDoubleClick(object sender, MouseEventArgs e)
    {
      this.AddEditObject(this.GetSelectedModel(), false);
    }

    private void textureToolStripMenuItem_Click(object sender, EventArgs e)
    {
      this.TextureInfo.Show();
      this.TextureInfo.BringToFront();
    }

    private void _btnAddObject_Click(object sender, EventArgs e)
    {
      this.AddModel();
    }

    private void AddModel()
    {
      this.AddEditObject((Model) null, false);
      this.UpdateUI();
    }

    private void _btnPackTexture_Click(object sender, EventArgs e)
    {
      this.TextureInfo.PackedTexture.Pack();
    }

    private void saveToolStripMenuItem1_Click(object sender, EventArgs e)
    {
      this.IsoPackFile.Save();
    }

    private void quickstartToolStripMenuItem_Click(object sender, EventArgs e)
    {
      Process.Start("http://www.isotool.net?p=1");
    }

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof (MainForm));
      this.menuStrip1 = new MenuStrip();
      this.fileToolStripMenuItem = new ToolStripMenuItem();
      this.newToolStripMenuItem = new ToolStripMenuItem();
      this.saveToolStripMenuItem = new ToolStripMenuItem();
      this.saveToolStripMenuItem1 = new ToolStripMenuItem();
      this.openToolStripMenuItem = new ToolStripMenuItem();
      this.exitToolStripMenuItem = new ToolStripMenuItem();
      this.helpToolStripMenuItem = new ToolStripMenuItem();
      this.aboutToolStripMenuItem = new ToolStripMenuItem();
      this.viewToolStripMenuItem = new ToolStripMenuItem();
      this.textureToolStripMenuItem = new ToolStripMenuItem();
      this.statusStrip1 = new StatusStrip();
      this._lblInfo = new ToolStripStatusLabel();
      this._btnAddObject = new Button();
      this._btnRemoveObject = new Button();
      this._lsvModels = new ListView();
      this._objMainTabControl = new TabControl();
      this._tabModels = new TabPage();
      this._btnPackTexture = new Button();
      this.helpToolStripMenuItem1 = new ToolStripMenuItem();
      this.quickstartToolStripMenuItem = new ToolStripMenuItem();
      this.menuStrip1.SuspendLayout();
      this.statusStrip1.SuspendLayout();
      this._objMainTabControl.SuspendLayout();
      this._tabModels.SuspendLayout();
      this.SuspendLayout();
      this.menuStrip1.Font = new Font("Segoe UI", 9.75f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.menuStrip1.Items.AddRange(new ToolStripItem[4]
      {
        (ToolStripItem) this.fileToolStripMenuItem,
        (ToolStripItem) this.helpToolStripMenuItem,
        (ToolStripItem) this.viewToolStripMenuItem,
        (ToolStripItem) this.helpToolStripMenuItem1
      });
      this.menuStrip1.Location = new Point(0, 0);
      this.menuStrip1.Name = "menuStrip1";
      this.menuStrip1.Size = new Size(331, 25);
      this.menuStrip1.TabIndex = 4;
      this.menuStrip1.Text = "menuStrip1";
      this.fileToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[5]
      {
        (ToolStripItem) this.newToolStripMenuItem,
        (ToolStripItem) this.saveToolStripMenuItem,
        (ToolStripItem) this.saveToolStripMenuItem1,
        (ToolStripItem) this.openToolStripMenuItem,
        (ToolStripItem) this.exitToolStripMenuItem
      });
      this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
      this.fileToolStripMenuItem.Size = new Size(39, 21);
      this.fileToolStripMenuItem.Text = "File";
      this.newToolStripMenuItem.Name = "newToolStripMenuItem";
      this.newToolStripMenuItem.Size = new Size(208, 22);
      this.newToolStripMenuItem.Text = "New";
      this.newToolStripMenuItem.Click += new EventHandler(this.newToolStripMenuItem_Click);
      this.saveToolStripMenuItem.Name = "saveToolStripMenuItem";
      this.saveToolStripMenuItem.ShortcutKeys = Keys.S | Keys.Shift | Keys.Control;
      this.saveToolStripMenuItem.Size = new Size(208, 22);
      this.saveToolStripMenuItem.Text = "Save As...";
      this.saveToolStripMenuItem.Click += new EventHandler(this.saveToolStripMenuItem_Click);
      this.saveToolStripMenuItem1.Enabled = false;
      this.saveToolStripMenuItem1.Name = "saveToolStripMenuItem1";
      this.saveToolStripMenuItem1.Size = new Size(208, 22);
      this.saveToolStripMenuItem1.Text = "Save";
      this.saveToolStripMenuItem1.Click += new EventHandler(this.saveToolStripMenuItem1_Click);
      this.openToolStripMenuItem.Name = "openToolStripMenuItem";
      this.openToolStripMenuItem.Size = new Size(208, 22);
      this.openToolStripMenuItem.Text = "Open";
      this.openToolStripMenuItem.Click += new EventHandler(this.openToolStripMenuItem_Click);
      this.exitToolStripMenuItem.Name = "exitToolStripMenuItem";
      this.exitToolStripMenuItem.Size = new Size(208, 22);
      this.exitToolStripMenuItem.Text = "Exit";
      this.helpToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[1]
      {
        (ToolStripItem) this.aboutToolStripMenuItem
      });
      this.helpToolStripMenuItem.Name = "helpToolStripMenuItem";
      this.helpToolStripMenuItem.Size = new Size(51, 21);
      this.helpToolStripMenuItem.Text = "Tools";
      this.helpToolStripMenuItem.Click += new EventHandler(this.helpToolStripMenuItem_Click);
      this.aboutToolStripMenuItem.Name = "aboutToolStripMenuItem";
      this.aboutToolStripMenuItem.Size = new Size(122, 22);
      this.aboutToolStripMenuItem.Text = "Settings";
      this.aboutToolStripMenuItem.Click += new EventHandler(this.aboutToolStripMenuItem_Click);
      this.viewToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[1]
      {
        (ToolStripItem) this.textureToolStripMenuItem
      });
      this.viewToolStripMenuItem.Name = "viewToolStripMenuItem";
      this.viewToolStripMenuItem.Size = new Size(47, 21);
      this.viewToolStripMenuItem.Text = "View";
      this.textureToolStripMenuItem.Name = "textureToolStripMenuItem";
      this.textureToolStripMenuItem.Size = new Size(124, 22);
      this.textureToolStripMenuItem.Text = "Textures";
      this.textureToolStripMenuItem.Click += new EventHandler(this.textureToolStripMenuItem_Click);
      this.statusStrip1.AutoSize = false;
      this.statusStrip1.Items.AddRange(new ToolStripItem[1]
      {
        (ToolStripItem) this._lblInfo
      });
      this.statusStrip1.Location = new Point(0, 398);
      this.statusStrip1.Name = "statusStrip1";
      this.statusStrip1.Size = new Size(331, 22);
      this.statusStrip1.TabIndex = 9;
      this.statusStrip1.Text = "statusStrip1";
      this._lblInfo.Name = "_lblInfo";
      this._lblInfo.Size = new Size(45, 17);
      this._lblInfo.Text = "Ready..";
      this._btnAddObject.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
      this._btnAddObject.Location = new Point(217, 304);
      this._btnAddObject.Name = "_btnAddObject";
      this._btnAddObject.Size = new Size(46, 23);
      this._btnAddObject.TabIndex = 10;
      this._btnAddObject.Text = "+";
      this._btnAddObject.UseVisualStyleBackColor = true;
      this._btnAddObject.Click += new EventHandler(this._btnAddObject_Click);
      this._btnRemoveObject.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
      this._btnRemoveObject.Location = new Point(269, 304);
      this._btnRemoveObject.Name = "_btnRemoveObject";
      this._btnRemoveObject.Size = new Size(46, 23);
      this._btnRemoveObject.TabIndex = 10;
      this._btnRemoveObject.Text = "-";
      this._btnRemoveObject.UseVisualStyleBackColor = true;
      this._btnRemoveObject.Click += new EventHandler(this._btnRemoveObject_Click);
      this._lsvModels.Dock = DockStyle.Fill;
      this._lsvModels.Location = new Point(0, 0);
      this._lsvModels.Name = "_lsvModels";
      this._lsvModels.Size = new Size(299, 241);
      this._lsvModels.TabIndex = 11;
      this._lsvModels.UseCompatibleStateImageBehavior = false;
      this._lsvModels.SelectedIndexChanged += new EventHandler(this._lsvModels_SelectedIndexChanged);
      this._lsvModels.MouseDoubleClick += new MouseEventHandler(this._lsvModels_MouseDoubleClick);
      this._objMainTabControl.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
      this._objMainTabControl.Controls.Add((Control) this._tabModels);
      this._objMainTabControl.Font = new Font("Microsoft Sans Serif", 9.75f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this._objMainTabControl.Location = new Point(12, 28);
      this._objMainTabControl.Name = "_objMainTabControl";
      this._objMainTabControl.SelectedIndex = 0;
      this._objMainTabControl.Size = new Size(307, 270);
      this._objMainTabControl.TabIndex = 12;
      this._tabModels.Controls.Add((Control) this._lsvModels);
      this._tabModels.Location = new Point(4, 25);
      this._tabModels.Name = "_tabModels";
      this._tabModels.Size = new Size(299, 241);
      this._tabModels.TabIndex = 0;
      this._tabModels.Text = "Models";
      this._tabModels.UseVisualStyleBackColor = true;
      this._btnPackTexture.Anchor = AnchorStyles.Bottom;
      this._btnPackTexture.Font = new Font("Microsoft Sans Serif", 9.75f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this._btnPackTexture.Location = new Point(92, 347);
      this._btnPackTexture.Name = "_btnPackTexture";
      this._btnPackTexture.Size = new Size(227, 37);
      this._btnPackTexture.TabIndex = 3;
      this._btnPackTexture.Text = "Pack Texture (happens on save)";
      this._btnPackTexture.UseVisualStyleBackColor = true;
      this._btnPackTexture.Click += new EventHandler(this._btnPackTexture_Click);
      this.helpToolStripMenuItem1.DropDownItems.AddRange(new ToolStripItem[1]
      {
        (ToolStripItem) this.quickstartToolStripMenuItem
      });
      this.helpToolStripMenuItem1.Name = "helpToolStripMenuItem1";
      this.helpToolStripMenuItem1.Size = new Size(47, 21);
      this.helpToolStripMenuItem1.Text = "Help";
      this.quickstartToolStripMenuItem.Name = "quickstartToolStripMenuItem";
      this.quickstartToolStripMenuItem.Size = new Size(152, 22);
      this.quickstartToolStripMenuItem.Text = "Quickstart";
      this.quickstartToolStripMenuItem.Click += new EventHandler(this.quickstartToolStripMenuItem_Click);
      this.AutoScaleDimensions = new SizeF(6f, 13f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.ClientSize = new Size(331, 420);
      this.Controls.Add((Control) this._objMainTabControl);
      this.Controls.Add((Control) this._btnRemoveObject);
      this.Controls.Add((Control) this._btnAddObject);
      this.Controls.Add((Control) this.statusStrip1);
      this.Controls.Add((Control) this._btnPackTexture);
      this.Controls.Add((Control) this.menuStrip1);
      this.Icon = (Icon) componentResourceManager.GetObject("$this.Icon");
      this.MainMenuStrip = this.menuStrip1;
      this.Name = nameof (MainForm);
      this.Text = "Iso Tool";
      this.Load += new EventHandler(this.MainForm_Load);
      this.menuStrip1.ResumeLayout(false);
      this.menuStrip1.PerformLayout();
      this.statusStrip1.ResumeLayout(false);
      this.statusStrip1.PerformLayout();
      this._objMainTabControl.ResumeLayout(false);
      this._tabModels.ResumeLayout(false);
      this.ResumeLayout(false);
      this.PerformLayout();
    }
  }
}
