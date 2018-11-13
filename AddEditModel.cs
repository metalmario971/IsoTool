// Decompiled with JetBrains decompiler
// Type: IsoPack.AddEditModel
// Assembly: IsoTool, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A40E7877-59D4-416C-9526-ACFD66F37CC4
// Assembly location: C:\Program Files\Iso Tool\IsoTool.exe

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace IsoPack
{
  public class AddEditModel : AddEditForm
  {
    private MainForm _objMainForm = (MainForm) null;
    private bool _bChanged = false;
    private SpriteListView SpriteListView = (SpriteListView) null;
    private BlendFileInfo _objBlendFileInfo = (BlendFileInfo) null;
    private List<int> _lstDirs = new List<int>();
    private IContainer components = (IContainer) null;
    private Button _btnBlendFile;
    private TextBox _txtBlendFile;
    private Label _lblBlendFile;
    private NumericUpDown _nudRenderDistance;
    private Label _lblRenderDistance;
    private Label label3;
    private TextBox _txtName;
    private Label _lblName;
    private Label _lblDirections;
    private NumericUpDown _nudDirections;
    private Button _btnSave;
    private Button _btnCancel;
    private Label _lblRenderWidth;
    private NumericUpDown _nudRenderWidth;
    private NumericUpDown _nudRenderHeight;
    private CheckBox _chkEnableAA;
    private NumericUpDown _nudAASamples;
    private Label _lblAASamples;
    private Button _btnApply;
    private Label _lblKeyframeGrain;
    private NumericUpDown _nudKeyframeGrain;
    private Label _lblRenderHeight;
    private ListView _lsvSprites;
    private GroupBox _gbRenderParameters;
    private Button _btnGetInfo;
    private Label _lblActions;
    private ComboBox _cboObjectInfoType;
    private Button _btnMakeDefault;
    private Button _btnRemoveAction;
    private Button button1;
    private Button _btnRenderSelected;
    private CheckBox _chkFitModel;
    private Button button2;
    private TreeView _tvActions;
    private Label _lblModelHeight;
    private NumericUpDown _nudModelHeight;
    private Label label2;
    private Label label4;
    private Button _btnAddSprite;
    private Button _btnRemoveSprite;
    private ComboBox _cboAngle;
    private Label label1;

    public Model Model { get; private set; } = (Model) null;

    public AddEditModel(MainForm mf)
    {
      this.InitializeComponent();
      this._objMainForm = mf;
      this.SpriteListView = new SpriteListView(this._objMainForm, (Func<List<SpriteListViewItem>>) (() =>
      {
        int spriteAngleFilter = this.GetSelectedSpriteAngleFilter();
        List<SpriteListViewItem> spriteListViewItemList = new List<SpriteListViewItem>();
        foreach (Sprite sprite in this.Model.Sprites)
        {
          if (spriteAngleFilter == -1 || sprite.Direction == spriteAngleFilter)
          {
            Frame frame = (Frame) null;
            if (sprite.Frames.Count > 0)
              frame = sprite.Frames[0];
            spriteListViewItemList.Add(new SpriteListViewItem((object) sprite, frame));
          }
        }
        return spriteListViewItemList;
      }), (Action<List<object>>) (x =>
      {
        foreach (object obj in x)
        {
          if (obj is Sprite)
            this.Model.Sprites.Remove(obj as Sprite);
        }
        this.UpdateUI(true);
      }));
      Globals.SwapControl((Control) this._lsvSprites, (Control) this.SpriteListView);
      Globals.SetToolTip(new List<Control>()
      {
        (Control) this._lblKeyframeGrain,
        (Control) this._nudKeyframeGrain
      }, "The number of sub-frames that are rendered when rendering animation.  This adds more frames for smoother animation, but results in a bigger texture.");
      Globals.SetToolTip(new List<Control>()
      {
        (Control) this._txtBlendFile,
        (Control) this._btnBlendFile,
        (Control) this._lblBlendFile
      }, "(optional) The location of the .blend file to process animations  The Render Parameters below control how the model is processed.  1.Select a .blend file by clicking the ellipsis.\n2. Click 'Load'\n3. Click 'Get Info' below.\n4. Select the actions (checkboxes) you want to render.\n5. Click 'Render' to create sprites.");
      Globals.SetToolTip(new List<Control>()
      {
        (Control) this._lblRenderDistance,
        (Control) this._nudRenderDistance
      }, "The isometric distance to render.  Play with this value to get a good result.");
      Globals.SetToolTip(new List<Control>()
      {
        (Control) this._lblDirections,
        (Control) this._nudDirections
      }, "The number of angles to render the model.  4, or 8 usually.");
      Globals.SetToolTip(new List<Control>()
      {
        (Control) this._lblRenderWidth,
        (Control) this._nudRenderWidth
      }, "The width of the sprite after rendering.");
      Globals.SetToolTip(new List<Control>()
      {
        (Control) this._lblRenderHeight,
        (Control) this._nudRenderHeight
      }, "The height of the sprite after rendering.");
      Globals.SetToolTip(new List<Control>()
      {
        (Control) this._chkEnableAA,
        (Control) this._nudAASamples,
        (Control) this._lblAASamples
      }, "Enable for anti-aliasing. (I don't recommend this if your game needs to be pixel perfect).  Blender hard-codes AA samples to 5, 8, 11, 16, so we round that value down to the closest value blender can handle.");
      Globals.SetToolTip(new List<Control>()
      {
        (Control) this._txtName,
        (Control) this._lblName
      }, "Model Name (referended in your code)");
      Globals.SetToolTip(new List<Control>()
      {
        (Control) this._cboObjectInfoType
      }, "Whether to animate Meshes only, Armatures only, or Both Meshes and Armatures");
      Globals.SetToolTip(new List<Control>()
      {
        (Control) this._chkFitModel
      }, "Zoom the camera in in order to fit the model exactly to the camera's viewport.");
      Globals.SetToolTip(new List<Control>()
      {
        (Control) this._tvActions,
        (Control) this._lblActions
      }, "Objects and Actions in the .blend file.  Check objects to render the object statically with 1 frame (no animation).  Check an action to render as an animation.");
      Globals.SetToolTip(new List<Control>()
      {
        (Control) this._lblModelHeight,
        (Control) this._nudModelHeight
      }, "Height of the model in pixels.  This is the isometric stacking height.");
      this.SetCollapsibleRenderParameters();
    }

    private int GetSelectedSpriteAngleFilter()
    {
      int num = -1;
      if (this._cboAngle.SelectedText == "All")
      {
        num = -1;
      }
      else
      {
        int index = this._cboAngle.SelectedIndex - 1;
        if (index >= 0 && index < this._lstDirs.Count)
          num = this._lstDirs[index];
      }
      return num;
    }

    private void SetCollapsibleRenderParameters()
    {
      this._gbRenderParameters.Text = "[ - ] Render Parameters";
      RECT rc;
      PInvoke.GetClientRect(this._gbRenderParameters.Handle, out rc);
      int minsize = 0;
      this._gbRenderParameters.MouseClick += (MouseEventHandler) ((s, e) =>
      {
        RECT lpRect1;
        PInvoke.GetClientRect(this._gbRenderParameters.Handle, out lpRect1);
        if (lpRect1.Height == minsize)
        {
          this._gbRenderParameters.Height = rc.Height;
          this._gbRenderParameters.Text = "[ - ] Render Parameters";
        }
        else
        {
          this._gbRenderParameters.Height = 20;
          this._gbRenderParameters.Text = "[ + ]Render Parameters";
          RECT lpRect2;
          PInvoke.GetClientRect(this._gbRenderParameters.Handle, out lpRect2);
          minsize = lpRect2.Height;
        }
      });
    }

    private void MarkChanged()
    {
      this._bChanged = true;
      this._btnApply.Enabled = true;
      this.CheckModelExists();
      this.CheckDefault();
    }

    private void ModelInfo_Load(object sender, EventArgs e)
    {
    }

    public void Show(Model m)
    {
      this._tvActions.Nodes.Clear();
      this._bChanged = false;
      this._btnApply.Enabled = false;
      if (m == null)
      {
        this.FormMode = FormMode.Add;
        this.Text = "Add Model";
        this.Model = new Model();
        this.Model.RenderParams = this._objMainForm.IsoPackFile.DefaultRenderParams;
        this.Model.Name = this.GetNewModelName();
      }
      else
      {
        this.FormMode = FormMode.Edit;
        this.Text = "Edit Model";
        this.Model = m;
      }
      this.LoadData();
      if (this._txtBlendFile.Text != "")
        this.CheckModelExists();
      this.Show();
      this.BringToFront();
    }

    private string GetNewModelName()
    {
      string str = "NewModel-0";
      int num = 0;
      bool flag = true;
      while (flag)
      {
        str = string.Format("NewModel-{0}", (object) num);
        flag = false;
        foreach (Model model in this._objMainForm.IsoPackFile.Models)
        {
          if (model.Name.Equals(str))
          {
            flag = true;
            break;
          }
        }
        if (flag)
          ++num;
        else
          break;
      }
      return str;
    }

    private void _btnSave_Click(object sender, EventArgs e)
    {
      this.SaveData();
      this.Close();
    }

    private void SaveData()
    {
      this.Model.Name = this._txtName.Text;
      this.Model.BlendFile = this._txtBlendFile.Text;
      this.Model.RenderParams.Directions = (int) this._nudDirections.Value;
      this.Model.RenderParams.RenderDistance = (float) this._nudRenderDistance.Value;
      this.Model.RenderParams.RenderHeight = (int) this._nudRenderHeight.Value;
      this.Model.RenderParams.RenderWidth = (int) this._nudRenderWidth.Value;
      this.Model.RenderParams.AASamples = this._chkEnableAA.Checked ? (int) this._nudAASamples.Value : 0;
      this.Model.RenderParams.KeyframeGrain = (int) this._nudKeyframeGrain.Value;
      this.Model.RenderParams.InfoType = (ObjectInfoType) Enum.Parse(typeof (ObjectInfoType), (string) this._cboObjectInfoType.SelectedItem);
      this.Model.RenderParams.FitModel = this._chkFitModel.Checked;
      this.Model.RenderParams.IsoHeight = (float) this._nudModelHeight.Value;
      if (this._objBlendFileInfo != null)
        this.Model.BlendFileInfo = this._objBlendFileInfo.CreateCopy();
      this._bChanged = false;
      this._btnApply.Enabled = false;
      if (this.FormMode == FormMode.Add)
      {
        this._objMainForm.IsoPackFile.Models.Add(this.Model);
        this.FormMode = FormMode.Edit;
        this.Text = "Edit Model";
      }
      this._objMainForm.UpdateUI();
    }

    private void LoadData()
    {
      this._txtName.Text = this.Model.Name;
      this._txtBlendFile.Text = this.Model.BlendFile;
      this._nudDirections.Value = (Decimal) this.Model.RenderParams.Directions;
      this._nudRenderDistance.Value = (Decimal) this.Model.RenderParams.RenderDistance;
      this._nudRenderHeight.Value = (Decimal) this.Model.RenderParams.RenderHeight;
      this._nudRenderWidth.Value = (Decimal) this.Model.RenderParams.RenderWidth;
      this._chkEnableAA.Checked = this.Model.RenderParams.AASamples > 0;
      this._nudAASamples.Value = (Decimal) this.Model.RenderParams.AASamples;
      this._nudKeyframeGrain.Value = (Decimal) this.Model.RenderParams.KeyframeGrain;
      this._cboObjectInfoType.SelectedItem = (object) this.Model.RenderParams.InfoType.ToString();
      this._objBlendFileInfo = this.Model.BlendFileInfo.CreateCopy();
      this._chkFitModel.Checked = this.Model.RenderParams.FitModel;
      this._nudModelHeight.Value = (Decimal) this.Model.RenderParams.IsoHeight;
      this.UpdateUI(false);
    }

    public override void UpdateUI(bool bDirty)
    {
      this.SpriteListView.UpdateListView();
      this.UpdateInfoBox();
      this.CheckDefault();
      if (bDirty)
        this.MarkChanged();
      this.UpdateAngleComboBox();
    }

    private void UpdateAngleComboBox()
    {
      this._cboAngle.SelectedIndexChanged -= new EventHandler(this._cboAngle_SelectedIndexChanged);
      int num = 0;
      foreach (Sprite sprite in this.Model.Sprites)
      {
        if (!this._lstDirs.Contains(sprite.Direction))
          this._lstDirs.Add(sprite.Direction);
        num = sprite.Direction > num ? sprite.Direction : num;
      }
      this._lstDirs.Sort((Comparison<int>) ((x, y) => x.CompareTo(y)));
      this._cboAngle.Items.Clear();
      this._cboAngle.Items.Add((object) "All");
      foreach (int lstDir in this._lstDirs)
        this._cboAngle.Items.Add((object) (((int) (360.0 / (double) num * (double) lstDir)).ToString() + "º"));
      this._cboAngle.SelectedIndex = 0;
      this._cboAngle.SelectedIndexChanged += new EventHandler(this._cboAngle_SelectedIndexChanged);
    }

    private void CheckDefault()
    {
      this._btnMakeDefault.Enabled = !true || !(this._nudDirections.Value == (Decimal) this._objMainForm.IsoPackFile.DefaultRenderParams.Directions) || !(this._nudRenderDistance.Value == (Decimal) this._objMainForm.IsoPackFile.DefaultRenderParams.RenderDistance) || !(this._nudRenderHeight.Value == (Decimal) this._objMainForm.IsoPackFile.DefaultRenderParams.RenderHeight) || !(this._nudRenderWidth.Value == (Decimal) this._objMainForm.IsoPackFile.DefaultRenderParams.RenderWidth) || this._chkEnableAA.Checked != this._objMainForm.IsoPackFile.DefaultRenderParams.AASamples > 0 || !(this._nudAASamples.Value == (Decimal) this._objMainForm.IsoPackFile.DefaultRenderParams.AASamples) || !(this._nudKeyframeGrain.Value == (Decimal) this._objMainForm.IsoPackFile.DefaultRenderParams.KeyframeGrain) || !((string) this._cboObjectInfoType.SelectedItem == this._objMainForm.IsoPackFile.DefaultRenderParams.InfoType.ToString()) || this._chkFitModel.Checked != this._objMainForm.IsoPackFile.DefaultRenderParams.FitModel || !(this._nudModelHeight.Value == (Decimal) this._objMainForm.IsoPackFile.DefaultRenderParams.IsoHeight);
    }

    private void _btnMakeDefault_Click(object sender, EventArgs e)
    {
      this._objMainForm.IsoPackFile.DefaultRenderParams.Directions = (int) this._nudDirections.Value;
      this._objMainForm.IsoPackFile.DefaultRenderParams.RenderDistance = (float) this._nudRenderDistance.Value;
      this._objMainForm.IsoPackFile.DefaultRenderParams.RenderHeight = (int) this._nudRenderHeight.Value;
      this._objMainForm.IsoPackFile.DefaultRenderParams.RenderWidth = (int) this._nudRenderWidth.Value;
      this._objMainForm.IsoPackFile.DefaultRenderParams.AASamples = (int) this._nudAASamples.Value;
      this._objMainForm.IsoPackFile.DefaultRenderParams.KeyframeGrain = (int) this._nudKeyframeGrain.Value;
      this._objMainForm.IsoPackFile.DefaultRenderParams.InfoType = (ObjectInfoType) Enum.Parse(typeof (ObjectInfoType), (string) this._cboObjectInfoType.SelectedItem);
      this._objMainForm.IsoPackFile.DefaultRenderParams.FitModel = this._chkFitModel.Checked;
      this.CheckDefault();
    }

    private void button1_Click_1(object sender, EventArgs e)
    {
      this._txtBlendFile.Text = "hell_bovine/hell_bovine.blend";
      this._chkFitModel.Checked = false;
      this._chkEnableAA.Checked = false;
      this.MarkChanged();
    }

    private void button2_Click(object sender, EventArgs e)
    {
      this._txtBlendFile.Text = "tile_0/tile_0.blend";
      this._chkFitModel.Checked = true;
      this._chkEnableAA.Checked = false;
      this.MarkChanged();
    }

    private void button1_Click(object sender, EventArgs e)
    {
    }

    private void _nudRenderDistance_ValueChanged(object sender, EventArgs e)
    {
      if ((double) (float) this._nudRenderDistance.Value == (double) this.Model.RenderParams.RenderDistance)
        return;
      this.MarkChanged();
    }

    private void _nudDirections_ValueChanged(object sender, EventArgs e)
    {
      if ((int) this._nudDirections.Value == this.Model.RenderParams.Directions)
        return;
      this.MarkChanged();
    }

    private void _txtBlendFile_TextChanged(object sender, EventArgs e)
    {
      if (!(this._txtBlendFile.Text != this.Model.BlendFile))
        return;
      this.MarkChanged();
    }

    private bool CheckModelExists()
    {
      if (!File.Exists(this._txtBlendFile.Text))
      {
        this._txtBlendFile.BackColor = Color.LightCoral;
        return false;
      }
      this._txtBlendFile.BackColor = Color.White;
      return true;
    }

    private void _btnCancel_Click(object sender, EventArgs e)
    {
      if (this._bChanged)
      {
        if (MessageBox.Show("Changes have been made. Discard?", "Invalid", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation) != DialogResult.Yes)
          return;
        this.Close();
      }
      else
        this.Close();
    }

    private void checkBox1_CheckedChanged(object sender, EventArgs e)
    {
      if (this._chkEnableAA.Checked)
        this._nudAASamples.Enabled = true;
      else
        this._nudAASamples.Enabled = false;
      if (this._chkEnableAA.Checked == this.Model.RenderParams.AASamples > 0)
        return;
      this.MarkChanged();
    }

    private void ModelInfo_FormClosing(object sender, FormClosingEventArgs e)
    {
      this._objMainForm.AddEditObject(this.Model, true);
    }

    private void _btnApply_Click(object sender, EventArgs e)
    {
      this.SaveData();
    }

    private void _lblDirections_Click(object sender, EventArgs e)
    {
    }

    private void _lblRenderWidth_Click(object sender, EventArgs e)
    {
    }

    private void _lblRenderHeight_Click(object sender, EventArgs e)
    {
    }

    private void groupBox1_Enter(object sender, EventArgs e)
    {
    }

    private void _getInfo_Click(object sender, EventArgs e)
    {
      if (!this.CheckModelExists())
        return;
      BlenderScriptTask_GetInfo info = new BlenderScriptTask_GetInfo();
      info.strFile = this._txtBlendFile.Text;
      info.Text = "Analyzing '" + this._txtBlendFile.Text + "'";
      info.After = (Action) (() => {});
      this._objMainForm.Blender.Scripts.Add((BlenderScriptTask) info);
      this._objMainForm.Blender.RunScripts((Action) (() =>
      {
        this._objBlendFileInfo = info.BlendFileInfo;
        this.UpdateInfoBox();
        this.MarkChanged();
      }));
    }

    private void UpdateInfoBox()
    {
      this._tvActions.AfterCheck -= new TreeViewEventHandler(this._tvActions_AfterCheck);
      this._tvActions.Nodes.Clear();
      if (this._objBlendFileInfo != null)
      {
        foreach (BlendFileObject blendFileObject in this._objBlendFileInfo.Objects)
        {
          TreeNode treeNode1 = this._tvActions.Nodes.Add(blendFileObject.Name);
          treeNode1.Name = blendFileObject.Name;
          treeNode1.Checked = blendFileObject.Checked;
          foreach (BlendFileAction action in blendFileObject.Actions)
          {
            TreeNode treeNode2 = treeNode1.Nodes.Add(action.Name);
            treeNode2.Name = action.Name;
            treeNode2.Checked = action.Checked;
          }
        }
      }
      this._tvActions.AfterCheck += new TreeViewEventHandler(this._tvActions_AfterCheck);
    }

    private void _lblAASamples_Click(object sender, EventArgs e)
    {
    }

    private void _cboObjectInfoType_SelectedIndexChanged(object sender, EventArgs e)
    {
      if (this.Model.RenderParams.InfoType == (ObjectInfoType) Enum.Parse(typeof (ObjectInfoType), (string) this._cboObjectInfoType.SelectedItem))
        return;
      this.MarkChanged();
    }

    private void _btnRemoveAction_Click(object sender, EventArgs e)
    {
      if (this._tvActions.SelectedNode == null)
        return;
      string name1 = this._tvActions.SelectedNode.Name;
      if (this._tvActions.SelectedNode.Parent == null)
      {
        for (int index = 0; index < this._objBlendFileInfo.Objects.Count; ++index)
        {
          if (this._objBlendFileInfo.Objects[index].Name == name1)
          {
            this._objBlendFileInfo.Objects.RemoveAt(index);
            break;
          }
        }
      }
      else
      {
        string str = name1;
        string name2 = this._tvActions.SelectedNode.Parent.Name;
        for (int index1 = 0; index1 < this._objBlendFileInfo.Objects.Count; ++index1)
        {
          if (this._objBlendFileInfo.Objects[index1].Name == name2)
          {
            for (int index2 = 0; index2 < this._objBlendFileInfo.Objects[index1].Actions.Count; ++index2)
            {
              if (this._objBlendFileInfo.Objects[index1].Actions[index2].Name == str)
              {
                this._objBlendFileInfo.Objects[index1].Actions.RemoveAt(index2);
                break;
              }
            }
            break;
          }
        }
      }
      this.UpdateInfoBox();
      this.MarkChanged();
    }

    private void _btnRenderSelected_Click(object sender, EventArgs e)
    {
      this._objMainForm.Blender.Scripts.Clear();
      foreach (BlendFileObject blendFileObject in this._objBlendFileInfo.Objects)
      {
        BlendFileObject ob = blendFileObject;
        if (ob.Checked)
        {
          string strAct = "__bind";
          this._objMainForm.Log("Rendering " + ob.Name);
          BlenderScriptTask_Render scriptTaskRender = new BlenderScriptTask_Render();
          scriptTaskRender.strModName = this._txtName.Text;
          scriptTaskRender.iDirections = (int) this._nudDirections.Value;
          scriptTaskRender.iWidth = (int) this._nudRenderWidth.Value;
          scriptTaskRender.iHeight = (int) this._nudRenderHeight.Value;
          scriptTaskRender.fDistance = (float) this._nudRenderDistance.Value;
          scriptTaskRender.iAASamples = (int) this._nudAASamples.Value;
          scriptTaskRender.iKeyframeGrain = (int) this._nudKeyframeGrain.Value;
          scriptTaskRender.blendFile = this._txtBlendFile.Text;
          scriptTaskRender.strObjName = ob.Name;
          scriptTaskRender.strActName = strAct;
          scriptTaskRender.bFitModel = this._chkFitModel.Checked;
          scriptTaskRender.Text = "Rendering " + ob.Name + ", " + strAct;
          scriptTaskRender.After = (Action) (() => this.Model.UpdateRenderedSprite(ob.Name, strAct, this._objMainForm));
          this._objMainForm.Blender.Scripts.Add((BlenderScriptTask) scriptTaskRender);
        }
        foreach (BlendFileAction action in ob.Actions)
        {
          BlendFileAction act = action;
          if (act.Checked)
          {
            BlenderScriptTask_Render scriptTaskRender = new BlenderScriptTask_Render();
            scriptTaskRender.strModName = this._txtName.Text;
            scriptTaskRender.iDirections = (int) this._nudDirections.Value;
            scriptTaskRender.iWidth = (int) this._nudRenderWidth.Value;
            scriptTaskRender.iHeight = (int) this._nudRenderHeight.Value;
            scriptTaskRender.fDistance = (float) this._nudRenderDistance.Value;
            scriptTaskRender.iAASamples = (int) this._nudAASamples.Value;
            scriptTaskRender.iKeyframeGrain = (int) this._nudKeyframeGrain.Value;
            scriptTaskRender.blendFile = this._txtBlendFile.Text;
            scriptTaskRender.strObjName = ob.Name;
            scriptTaskRender.strActName = act.Name;
            scriptTaskRender.bFitModel = this._chkFitModel.Checked;
            scriptTaskRender.Text = "Rendering " + ob.Name + ", " + act.Name;
            scriptTaskRender.After = (Action) (() => this.Model.UpdateRenderedSprite(ob.Name, act.Name, this._objMainForm));
            this._objMainForm.Blender.Scripts.Add((BlenderScriptTask) scriptTaskRender);
          }
        }
      }
      this._objMainForm.Blender.RunScripts((Action) (() => this.SpriteListView.UpdateListView()));
    }

    private void _chkFitModel_CheckedChanged(object sender, EventArgs e)
    {
      if (this._chkFitModel.Checked == this.Model.RenderParams.FitModel)
        return;
      this.MarkChanged();
    }

    private void _tvActions_AfterSelect(object sender, TreeViewEventArgs e)
    {
      if (this._tvActions.SelectedNode != null)
        this._btnRenderSelected.Enabled = true;
      else
        this._btnRenderSelected.Enabled = false;
    }

    private void _tvActions_AfterCheck(object sender, TreeViewEventArgs e)
    {
      if (this._objBlendFileInfo != null)
      {
        if (e.Node.Parent == null)
        {
          foreach (BlendFileObject blendFileObject in this._objBlendFileInfo.Objects)
          {
            if (blendFileObject.Name == e.Node.Name)
              blendFileObject.Checked = e.Node.Checked;
          }
        }
        else
        {
          foreach (BlendFileObject blendFileObject in this._objBlendFileInfo.Objects)
          {
            if (blendFileObject.Name == e.Node.Parent.Name)
            {
              using (List<BlendFileAction>.Enumerator enumerator = blendFileObject.Actions.GetEnumerator())
              {
                while (enumerator.MoveNext())
                {
                  BlendFileAction current = enumerator.Current;
                  if (current.Name == e.Node.Name)
                    current.Checked = e.Node.Checked;
                }
                break;
              }
            }
          }
        }
      }
      this.MarkChanged();
    }

    private void _lsvSprites_SelectedIndexChanged(object sender, EventArgs e)
    {
    }

    private void _btnBlendFile_Click(object sender, EventArgs e)
    {
      string[] openSaveUserFile = Globals.GetValidOpenSaveUserFile(false, "Blend File (*.blend)|*.blend|All files (*.*)|*.*", "Blend File (*.blend)|*.blend|All files (*.*)|*.*", "blend", this._objMainForm.IsoPackFile.AppSettings.ProjectRoot, false);
      if ((uint) openSaveUserFile.Length <= 0U)
        return;
      this._txtBlendFile.Text = openSaveUserFile[0];
    }

    private void _txtName_TextChanged(object sender, EventArgs e)
    {
      if (!(this._txtName.Text != this.Model.Name))
        return;
      this.MarkChanged();
    }

    private void _nudRenderHeight_ValueChanged(object sender, EventArgs e)
    {
      if ((int) this._nudRenderHeight.Value == this.Model.RenderParams.RenderHeight)
        return;
      this.MarkChanged();
    }

    private void _nudRenderWidth_ValueChanged(object sender, EventArgs e)
    {
      if ((int) this._nudRenderWidth.Value == this.Model.RenderParams.RenderWidth)
        return;
      this.MarkChanged();
    }

    private void _nudKeyframeGrain_ValueChanged(object sender, EventArgs e)
    {
      if ((int) this._nudKeyframeGrain.Value == this.Model.RenderParams.KeyframeGrain)
        return;
      this.MarkChanged();
    }

    private void _nudAASamples_ValueChanged(object sender, EventArgs e)
    {
      if ((int) this._nudAASamples.Value == this.Model.RenderParams.AASamples)
        return;
      this.MarkChanged();
    }

    private void _nudModelHeight_ValueChanged(object sender, EventArgs e)
    {
      if ((double) (int) this._nudModelHeight.Value == (double) this.Model.RenderParams.IsoHeight)
        return;
      this.MarkChanged();
    }

    private void _btnAddSprite_Click(object sender, EventArgs e)
    {
      this._objMainForm.AddEditObject((Sprite) null, this.Model, false);
    }

    private void _btnRemoveSprite_Click(object sender, EventArgs e)
    {
      Sprite selectedObject = this.SpriteListView.GetSelectedObject() as Sprite;
      if (selectedObject != null)
        this.Model.Sprites.Remove(selectedObject);
      this.UpdateUI(true);
    }

    private void _cboAngle_SelectedIndexChanged(object sender, EventArgs e)
    {
      this.UpdateUI(false);
    }

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof (AddEditModel));
      this._btnBlendFile = new Button();
      this._txtBlendFile = new TextBox();
      this._lblBlendFile = new Label();
      this._nudRenderDistance = new NumericUpDown();
      this._lblRenderDistance = new Label();
      this.label3 = new Label();
      this._txtName = new TextBox();
      this._lblName = new Label();
      this._lblDirections = new Label();
      this._nudDirections = new NumericUpDown();
      this._btnSave = new Button();
      this._btnCancel = new Button();
      this._lblRenderWidth = new Label();
      this._nudRenderWidth = new NumericUpDown();
      this._nudRenderHeight = new NumericUpDown();
      this._chkEnableAA = new CheckBox();
      this._nudAASamples = new NumericUpDown();
      this._lblAASamples = new Label();
      this._btnApply = new Button();
      this._lblKeyframeGrain = new Label();
      this._nudKeyframeGrain = new NumericUpDown();
      this._lblRenderHeight = new Label();
      this._lsvSprites = new ListView();
      this._gbRenderParameters = new GroupBox();
      this._btnMakeDefault = new Button();
      this._cboObjectInfoType = new ComboBox();
      this._chkFitModel = new CheckBox();
      this.label4 = new Label();
      this.label2 = new Label();
      this._lblModelHeight = new Label();
      this._nudModelHeight = new NumericUpDown();
      this._btnGetInfo = new Button();
      this._lblActions = new Label();
      this._btnRemoveAction = new Button();
      this.button1 = new Button();
      this._btnRenderSelected = new Button();
      this.button2 = new Button();
      this._tvActions = new TreeView();
      this._btnAddSprite = new Button();
      this._btnRemoveSprite = new Button();
      this._cboAngle = new ComboBox();
      this.label1 = new Label();
      this._nudRenderDistance.BeginInit();
      this._nudDirections.BeginInit();
      this._nudRenderWidth.BeginInit();
      this._nudRenderHeight.BeginInit();
      this._nudAASamples.BeginInit();
      this._nudKeyframeGrain.BeginInit();
      this._gbRenderParameters.SuspendLayout();
      this._nudModelHeight.BeginInit();
      this.SuspendLayout();
      this._btnBlendFile.Font = new Font("Microsoft Sans Serif", 9.75f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this._btnBlendFile.Location = new Point(15, 66);
      this._btnBlendFile.Name = "_btnBlendFile";
      this._btnBlendFile.Size = new Size(31, 23);
      this._btnBlendFile.TabIndex = 0;
      this._btnBlendFile.Text = "...";
      this._btnBlendFile.UseVisualStyleBackColor = true;
      this._btnBlendFile.Click += new EventHandler(this._btnBlendFile_Click);
      this._txtBlendFile.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
      this._txtBlendFile.Font = new Font("Microsoft Sans Serif", 9.75f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this._txtBlendFile.ForeColor = Color.Black;
      this._txtBlendFile.Location = new Point(52, 67);
      this._txtBlendFile.Name = "_txtBlendFile";
      this._txtBlendFile.Size = new Size(475, 22);
      this._txtBlendFile.TabIndex = 1;
      this._txtBlendFile.TextChanged += new EventHandler(this._txtBlendFile_TextChanged);
      this._lblBlendFile.AutoSize = true;
      this._lblBlendFile.Font = new Font("Microsoft Sans Serif", 9.75f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this._lblBlendFile.Location = new Point(12, 48);
      this._lblBlendFile.Name = "_lblBlendFile";
      this._lblBlendFile.Size = new Size(123, 16);
      this._lblBlendFile.TabIndex = 2;
      this._lblBlendFile.Text = "Blender (.blend) file";
      this._nudRenderDistance.DecimalPlaces = 2;
      this._nudRenderDistance.Font = new Font("Microsoft Sans Serif", 9.75f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this._nudRenderDistance.Location = new Point(83, 59);
      this._nudRenderDistance.Maximum = new Decimal(new int[4]
      {
        999999,
        0,
        0,
        0
      });
      this._nudRenderDistance.Name = "_nudRenderDistance";
      this._nudRenderDistance.Size = new Size(76, 22);
      this._nudRenderDistance.TabIndex = 3;
      this._nudRenderDistance.Value = new Decimal(new int[4]
      {
        50,
        0,
        0,
        65536
      });
      this._nudRenderDistance.ValueChanged += new EventHandler(this._nudRenderDistance_ValueChanged);
      this._lblRenderDistance.AutoSize = true;
      this._lblRenderDistance.Font = new Font("Microsoft Sans Serif", 9.75f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this._lblRenderDistance.Location = new Point(16, 61);
      this._lblRenderDistance.Name = "_lblRenderDistance";
      this._lblRenderDistance.Size = new Size(61, 16);
      this._lblRenderDistance.TabIndex = 2;
      this._lblRenderDistance.Text = "Distance";
      this.label3.AutoSize = true;
      this.label3.Font = new Font("Microsoft Sans Serif", 9.75f, FontStyle.Bold, GraphicsUnit.Point, (byte) 0);
      this.label3.Location = new Point(258, 285);
      this.label3.Name = "label3";
      this.label3.Size = new Size(57, 16);
      this.label3.TabIndex = 6;
      this.label3.Text = "Sprites";
      this._txtName.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
      this._txtName.Font = new Font("Microsoft Sans Serif", 9.75f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this._txtName.Location = new Point(68, 12);
      this._txtName.Name = "_txtName";
      this._txtName.Size = new Size(208, 22);
      this._txtName.TabIndex = 1;
      this._txtName.TextChanged += new EventHandler(this._txtName_TextChanged);
      this._lblName.AutoSize = true;
      this._lblName.Font = new Font("Microsoft Sans Serif", 9.75f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this._lblName.Location = new Point(12, 15);
      this._lblName.Name = "_lblName";
      this._lblName.Size = new Size(50, 16);
      this._lblName.TabIndex = 2;
      this._lblName.Text = "*Name";
      this._lblDirections.AutoSize = true;
      this._lblDirections.Font = new Font("Microsoft Sans Serif", 9.75f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this._lblDirections.Location = new Point(16, 88);
      this._lblDirections.Name = "_lblDirections";
      this._lblDirections.Size = new Size(74, 16);
      this._lblDirections.TabIndex = 2;
      this._lblDirections.Text = "# of Angles";
      this._lblDirections.Click += new EventHandler(this._lblDirections_Click);
      this._nudDirections.Font = new Font("Microsoft Sans Serif", 9.75f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this._nudDirections.Location = new Point(96, 88);
      this._nudDirections.Maximum = new Decimal(new int[4]
      {
        99999,
        0,
        0,
        0
      });
      this._nudDirections.Minimum = new Decimal(new int[4]
      {
        1,
        0,
        0,
        0
      });
      this._nudDirections.Name = "_nudDirections";
      this._nudDirections.Size = new Size(63, 22);
      this._nudDirections.TabIndex = 3;
      this._nudDirections.Value = new Decimal(new int[4]
      {
        8,
        0,
        0,
        0
      });
      this._nudDirections.ValueChanged += new EventHandler(this._nudDirections_ValueChanged);
      this._btnSave.Anchor = AnchorStyles.Bottom;
      this._btnSave.Font = new Font("Microsoft Sans Serif", 9.75f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this._btnSave.Location = new Point(121, 593);
      this._btnSave.Name = "_btnSave";
      this._btnSave.Size = new Size(94, 36);
      this._btnSave.TabIndex = 4;
      this._btnSave.Text = "Ok";
      this._btnSave.UseVisualStyleBackColor = true;
      this._btnSave.Click += new EventHandler(this._btnSave_Click);
      this._btnCancel.Anchor = AnchorStyles.Bottom;
      this._btnCancel.Font = new Font("Microsoft Sans Serif", 9.75f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this._btnCancel.Location = new Point(321, 593);
      this._btnCancel.Name = "_btnCancel";
      this._btnCancel.Size = new Size(94, 36);
      this._btnCancel.TabIndex = 4;
      this._btnCancel.Text = "Cancel";
      this._btnCancel.UseVisualStyleBackColor = true;
      this._btnCancel.Click += new EventHandler(this._btnCancel_Click);
      this._lblRenderWidth.AutoSize = true;
      this._lblRenderWidth.Font = new Font("Microsoft Sans Serif", 9.75f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this._lblRenderWidth.Location = new Point(16, 29);
      this._lblRenderWidth.Name = "_lblRenderWidth";
      this._lblRenderWidth.Size = new Size(42, 16);
      this._lblRenderWidth.TabIndex = 2;
      this._lblRenderWidth.Text = "Width";
      this._lblRenderWidth.Click += new EventHandler(this._lblRenderWidth_Click);
      this._nudRenderWidth.Font = new Font("Microsoft Sans Serif", 9.75f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this._nudRenderWidth.Location = new Point(64, 27);
      this._nudRenderWidth.Maximum = new Decimal(new int[4]
      {
        99999,
        0,
        0,
        0
      });
      this._nudRenderWidth.Minimum = new Decimal(new int[4]
      {
        1,
        0,
        0,
        0
      });
      this._nudRenderWidth.Name = "_nudRenderWidth";
      this._nudRenderWidth.Size = new Size(45, 22);
      this._nudRenderWidth.TabIndex = 3;
      this._nudRenderWidth.Value = new Decimal(new int[4]
      {
        128,
        0,
        0,
        0
      });
      this._nudRenderWidth.ValueChanged += new EventHandler(this._nudRenderWidth_ValueChanged);
      this._nudRenderHeight.Font = new Font("Microsoft Sans Serif", 9.75f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this._nudRenderHeight.Location = new Point(168, 28);
      this._nudRenderHeight.Maximum = new Decimal(new int[4]
      {
        99999,
        0,
        0,
        0
      });
      this._nudRenderHeight.Minimum = new Decimal(new int[4]
      {
        1,
        0,
        0,
        0
      });
      this._nudRenderHeight.Name = "_nudRenderHeight";
      this._nudRenderHeight.Size = new Size(45, 22);
      this._nudRenderHeight.TabIndex = 3;
      this._nudRenderHeight.Value = new Decimal(new int[4]
      {
        128,
        0,
        0,
        0
      });
      this._nudRenderHeight.ValueChanged += new EventHandler(this._nudRenderHeight_ValueChanged);
      this._chkEnableAA.AutoSize = true;
      this._chkEnableAA.Font = new Font("Microsoft Sans Serif", 9.75f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this._chkEnableAA.Location = new Point(203, 60);
      this._chkEnableAA.Name = "_chkEnableAA";
      this._chkEnableAA.Size = new Size(79, 20);
      this._chkEnableAA.TabIndex = 7;
      this._chkEnableAA.Text = "AntiAlias";
      this._chkEnableAA.UseVisualStyleBackColor = true;
      this._chkEnableAA.CheckedChanged += new EventHandler(this.checkBox1_CheckedChanged);
      this._nudAASamples.Enabled = false;
      this._nudAASamples.Font = new Font("Microsoft Sans Serif", 9.75f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this._nudAASamples.Location = new Point(288, 58);
      this._nudAASamples.Maximum = new Decimal(new int[4]
      {
        99999,
        0,
        0,
        0
      });
      this._nudAASamples.Name = "_nudAASamples";
      this._nudAASamples.Size = new Size(45, 22);
      this._nudAASamples.TabIndex = 3;
      this._nudAASamples.Value = new Decimal(new int[4]
      {
        5,
        0,
        0,
        0
      });
      this._nudAASamples.ValueChanged += new EventHandler(this._nudAASamples_ValueChanged);
      this._lblAASamples.AutoSize = true;
      this._lblAASamples.Font = new Font("Microsoft Sans Serif", 9.75f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this._lblAASamples.Location = new Point(339, 60);
      this._lblAASamples.Name = "_lblAASamples";
      this._lblAASamples.Size = new Size(62, 16);
      this._lblAASamples.TabIndex = 2;
      this._lblAASamples.Text = "Samples";
      this._lblAASamples.Click += new EventHandler(this._lblAASamples_Click);
      this._btnApply.Anchor = AnchorStyles.Bottom;
      this._btnApply.Enabled = false;
      this._btnApply.Font = new Font("Microsoft Sans Serif", 9.75f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this._btnApply.Location = new Point(221, 593);
      this._btnApply.Name = "_btnApply";
      this._btnApply.Size = new Size(94, 36);
      this._btnApply.TabIndex = 4;
      this._btnApply.Text = "Apply";
      this._btnApply.UseVisualStyleBackColor = true;
      this._btnApply.Click += new EventHandler(this._btnApply_Click);
      this._lblKeyframeGrain.AutoSize = true;
      this._lblKeyframeGrain.Font = new Font("Microsoft Sans Serif", 9.75f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this._lblKeyframeGrain.Location = new Point(182, 90);
      this._lblKeyframeGrain.Name = "_lblKeyframeGrain";
      this._lblKeyframeGrain.Size = new Size(100, 16);
      this._lblKeyframeGrain.TabIndex = 2;
      this._lblKeyframeGrain.Text = "Keyframe Grain";
      this._nudKeyframeGrain.Font = new Font("Microsoft Sans Serif", 9.75f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this._nudKeyframeGrain.Location = new Point(288, 88);
      this._nudKeyframeGrain.Minimum = new Decimal(new int[4]
      {
        1,
        0,
        0,
        0
      });
      this._nudKeyframeGrain.Name = "_nudKeyframeGrain";
      this._nudKeyframeGrain.Size = new Size(45, 22);
      this._nudKeyframeGrain.TabIndex = 3;
      this._nudKeyframeGrain.Value = new Decimal(new int[4]
      {
        1,
        0,
        0,
        0
      });
      this._nudKeyframeGrain.ValueChanged += new EventHandler(this._nudKeyframeGrain_ValueChanged);
      this._lblRenderHeight.AutoSize = true;
      this._lblRenderHeight.Font = new Font("Microsoft Sans Serif", 9.75f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this._lblRenderHeight.Location = new Point(115, 30);
      this._lblRenderHeight.Name = "_lblRenderHeight";
      this._lblRenderHeight.Size = new Size(47, 16);
      this._lblRenderHeight.TabIndex = 2;
      this._lblRenderHeight.Text = "Height";
      this._lblRenderHeight.Click += new EventHandler(this._lblRenderHeight_Click);
      this._lsvSprites.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
      this._lsvSprites.Location = new Point(261, 338);
      this._lsvSprites.Name = "_lsvSprites";
      this._lsvSprites.Size = new Size(282, 218);
      this._lsvSprites.TabIndex = 8;
      this._lsvSprites.UseCompatibleStateImageBehavior = false;
      this._lsvSprites.SelectedIndexChanged += new EventHandler(this._lsvSprites_SelectedIndexChanged);
      this._gbRenderParameters.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
      this._gbRenderParameters.Controls.Add((Control) this._btnMakeDefault);
      this._gbRenderParameters.Controls.Add((Control) this._cboObjectInfoType);
      this._gbRenderParameters.Controls.Add((Control) this._lblRenderDistance);
      this._gbRenderParameters.Controls.Add((Control) this._nudRenderDistance);
      this._gbRenderParameters.Controls.Add((Control) this._chkFitModel);
      this._gbRenderParameters.Controls.Add((Control) this._chkEnableAA);
      this._gbRenderParameters.Controls.Add((Control) this.label4);
      this._gbRenderParameters.Controls.Add((Control) this.label2);
      this._gbRenderParameters.Controls.Add((Control) this._lblModelHeight);
      this._gbRenderParameters.Controls.Add((Control) this._lblDirections);
      this._gbRenderParameters.Controls.Add((Control) this._nudModelHeight);
      this._gbRenderParameters.Controls.Add((Control) this._nudDirections);
      this._gbRenderParameters.Controls.Add((Control) this._lblRenderWidth);
      this._gbRenderParameters.Controls.Add((Control) this._nudRenderWidth);
      this._gbRenderParameters.Controls.Add((Control) this._lblRenderHeight);
      this._gbRenderParameters.Controls.Add((Control) this._lblAASamples);
      this._gbRenderParameters.Controls.Add((Control) this._nudAASamples);
      this._gbRenderParameters.Controls.Add((Control) this._lblKeyframeGrain);
      this._gbRenderParameters.Controls.Add((Control) this._nudKeyframeGrain);
      this._gbRenderParameters.Controls.Add((Control) this._nudRenderHeight);
      this._gbRenderParameters.Font = new Font("Microsoft Sans Serif", 9.75f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this._gbRenderParameters.ForeColor = Color.Black;
      this._gbRenderParameters.Location = new Point(15, 120);
      this._gbRenderParameters.Name = "_gbRenderParameters";
      this._gbRenderParameters.Size = new Size(534, 146);
      this._gbRenderParameters.TabIndex = 9;
      this._gbRenderParameters.TabStop = false;
      this._gbRenderParameters.Text = "Render Parameters";
      this._gbRenderParameters.Enter += new EventHandler(this.groupBox1_Enter);
      this._btnMakeDefault.Location = new Point(432, 104);
      this._btnMakeDefault.Name = "_btnMakeDefault";
      this._btnMakeDefault.Size = new Size(96, 23);
      this._btnMakeDefault.TabIndex = 9;
      this._btnMakeDefault.Text = "Make Default";
      this._btnMakeDefault.UseVisualStyleBackColor = true;
      this._btnMakeDefault.Click += new EventHandler(this._btnMakeDefault_Click);
      this._cboObjectInfoType.Font = new Font("Microsoft Sans Serif", 9.75f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this._cboObjectInfoType.FormattingEnabled = true;
      this._cboObjectInfoType.Items.AddRange(new object[3]
      {
        (object) "Armature",
        (object) "Mesh",
        (object) "Both"
      });
      this._cboObjectInfoType.Location = new Point(243, 27);
      this._cboObjectInfoType.Name = "_cboObjectInfoType";
      this._cboObjectInfoType.Size = new Size(90, 24);
      this._cboObjectInfoType.TabIndex = 8;
      this._cboObjectInfoType.Text = "Armature";
      this._cboObjectInfoType.SelectedIndexChanged += new EventHandler(this._cboObjectInfoType_SelectedIndexChanged);
      this._chkFitModel.AutoSize = true;
      this._chkFitModel.Enabled = false;
      this._chkFitModel.Font = new Font("Microsoft Sans Serif", 9.75f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this._chkFitModel.Location = new Point(420, 29);
      this._chkFitModel.Name = "_chkFitModel";
      this._chkFitModel.Size = new Size(82, 20);
      this._chkFitModel.TabIndex = 7;
      this._chkFitModel.Text = "Fit Model";
      this._chkFitModel.UseVisualStyleBackColor = true;
      this._chkFitModel.CheckedChanged += new EventHandler(this._chkFitModel_CheckedChanged);
      this.label4.AutoSize = true;
      this.label4.Font = new Font("Microsoft Sans Serif", 9.75f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.label4.Location = new Point(156, 64);
      this.label4.Name = "label4";
      this.label4.Size = new Size(19, 16);
      this.label4.TabIndex = 2;
      this.label4.Text = "m";
      this.label4.Click += new EventHandler(this._lblDirections_Click);
      this.label2.AutoSize = true;
      this.label2.Font = new Font("Microsoft Sans Serif", 9.75f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.label2.Location = new Point(165, 119);
      this.label2.Name = "label2";
      this.label2.Size = new Size(22, 16);
      this.label2.TabIndex = 2;
      this.label2.Text = "px";
      this.label2.Click += new EventHandler(this._lblDirections_Click);
      this._lblModelHeight.AutoSize = true;
      this._lblModelHeight.Font = new Font("Microsoft Sans Serif", 9.75f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this._lblModelHeight.Location = new Point(-3, 119);
      this._lblModelHeight.Name = "_lblModelHeight";
      this._lblModelHeight.Size = new Size(84, 16);
      this._lblModelHeight.TabIndex = 2;
      this._lblModelHeight.Text = "Stack Height";
      this._lblModelHeight.Click += new EventHandler(this._lblDirections_Click);
      this._nudModelHeight.DecimalPlaces = 2;
      this._nudModelHeight.Font = new Font("Microsoft Sans Serif", 9.75f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this._nudModelHeight.Location = new Point(83, 116);
      this._nudModelHeight.Maximum = new Decimal(new int[4]
      {
        99999,
        0,
        0,
        0
      });
      this._nudModelHeight.Name = "_nudModelHeight";
      this._nudModelHeight.Size = new Size(76, 22);
      this._nudModelHeight.TabIndex = 3;
      this._nudModelHeight.Value = new Decimal(new int[4]
      {
        10,
        0,
        0,
        65536
      });
      this._nudModelHeight.ValueChanged += new EventHandler(this._nudModelHeight_ValueChanged);
      this._btnGetInfo.Font = new Font("Microsoft Sans Serif", 9.75f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this._btnGetInfo.Location = new Point(12, 309);
      this._btnGetInfo.Name = "_btnGetInfo";
      this._btnGetInfo.Size = new Size(67, 23);
      this._btnGetInfo.TabIndex = 4;
      this._btnGetInfo.Text = "Get Info";
      this._btnGetInfo.UseVisualStyleBackColor = true;
      this._btnGetInfo.Click += new EventHandler(this._getInfo_Click);
      this._lblActions.AutoSize = true;
      this._lblActions.Font = new Font("Microsoft Sans Serif", 9.75f, FontStyle.Bold, GraphicsUnit.Point, (byte) 0);
      this._lblActions.Location = new Point(10, 285);
      this._lblActions.Name = "_lblActions";
      this._lblActions.Size = new Size(59, 16);
      this._lblActions.TabIndex = 6;
      this._lblActions.Text = "Actions";
      this._btnRemoveAction.Font = new Font("Microsoft Sans Serif", 9.75f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this._btnRemoveAction.Location = new Point(221, 309);
      this._btnRemoveAction.Name = "_btnRemoveAction";
      this._btnRemoveAction.Size = new Size(32, 23);
      this._btnRemoveAction.TabIndex = 4;
      this._btnRemoveAction.Text = "-";
      this._btnRemoveAction.UseVisualStyleBackColor = true;
      this._btnRemoveAction.Click += new EventHandler(this._btnRemoveAction_Click);
      this.button1.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.button1.Location = new Point(357, 12);
      this.button1.Name = "button1";
      this.button1.Size = new Size(65, 49);
      this.button1.TabIndex = 9;
      this.button1.Text = "Test HB";
      this.button1.UseVisualStyleBackColor = true;
      this.button1.Visible = false;
      this.button1.Click += new EventHandler(this.button1_Click_1);
      this._btnRenderSelected.Font = new Font("Microsoft Sans Serif", 9.75f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this._btnRenderSelected.Location = new Point(85, 309);
      this._btnRenderSelected.Name = "_btnRenderSelected";
      this._btnRenderSelected.Size = new Size(71, 23);
      this._btnRenderSelected.TabIndex = 4;
      this._btnRenderSelected.Text = "Render";
      this._btnRenderSelected.UseVisualStyleBackColor = true;
      this._btnRenderSelected.Click += new EventHandler(this._btnRenderSelected_Click);
      this.button2.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.button2.Location = new Point(462, 12);
      this.button2.Name = "button2";
      this.button2.Size = new Size(65, 49);
      this.button2.TabIndex = 11;
      this.button2.Text = "Test Tile";
      this.button2.UseVisualStyleBackColor = true;
      this.button2.Visible = false;
      this.button2.Click += new EventHandler(this.button2_Click);
      this._tvActions.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left;
      this._tvActions.CheckBoxes = true;
      this._tvActions.HideSelection = false;
      this._tvActions.Location = new Point(12, 338);
      this._tvActions.Name = "_tvActions";
      this._tvActions.Size = new Size(243, 218);
      this._tvActions.TabIndex = 13;
      this._tvActions.AfterCheck += new TreeViewEventHandler(this._tvActions_AfterCheck);
      this._tvActions.AfterSelect += new TreeViewEventHandler(this._tvActions_AfterSelect);
      this._btnAddSprite.Font = new Font("Microsoft Sans Serif", 9.75f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this._btnAddSprite.Location = new Point(473, 285);
      this._btnAddSprite.Name = "_btnAddSprite";
      this._btnAddSprite.Size = new Size(32, 23);
      this._btnAddSprite.TabIndex = 4;
      this._btnAddSprite.Text = "+";
      this._btnAddSprite.UseVisualStyleBackColor = true;
      this._btnAddSprite.Click += new EventHandler(this._btnAddSprite_Click);
      this._btnRemoveSprite.Font = new Font("Microsoft Sans Serif", 9.75f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this._btnRemoveSprite.Location = new Point(511, 285);
      this._btnRemoveSprite.Name = "_btnRemoveSprite";
      this._btnRemoveSprite.Size = new Size(32, 23);
      this._btnRemoveSprite.TabIndex = 4;
      this._btnRemoveSprite.Text = "-";
      this._btnRemoveSprite.UseVisualStyleBackColor = true;
      this._btnRemoveSprite.Click += new EventHandler(this._btnRemoveSprite_Click);
      this._cboAngle.FormattingEnabled = true;
      this._cboAngle.Location = new Point(314, 315);
      this._cboAngle.Name = "_cboAngle";
      this._cboAngle.Size = new Size(121, 21);
      this._cboAngle.TabIndex = 14;
      this._cboAngle.SelectedIndexChanged += new EventHandler(this._cboAngle_SelectedIndexChanged);
      this.label1.AutoSize = true;
      this.label1.Font = new Font("Microsoft Sans Serif", 9.75f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.label1.Location = new Point(265, 315);
      this.label1.Name = "label1";
      this.label1.Size = new Size(43, 16);
      this.label1.TabIndex = 6;
      this.label1.Text = "Angle";
      this.AutoScaleDimensions = new SizeF(6f, 13f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.ClientSize = new Size(561, 640);
      this.Controls.Add((Control) this._cboAngle);
      this.Controls.Add((Control) this._tvActions);
      this.Controls.Add((Control) this.button2);
      this.Controls.Add((Control) this.button1);
      this.Controls.Add((Control) this._gbRenderParameters);
      this.Controls.Add((Control) this._lsvSprites);
      this.Controls.Add((Control) this._lblActions);
      this.Controls.Add((Control) this.label1);
      this.Controls.Add((Control) this.label3);
      this.Controls.Add((Control) this._btnCancel);
      this.Controls.Add((Control) this._btnApply);
      this.Controls.Add((Control) this._btnSave);
      this.Controls.Add((Control) this._btnRemoveSprite);
      this.Controls.Add((Control) this._btnAddSprite);
      this.Controls.Add((Control) this._btnRemoveAction);
      this.Controls.Add((Control) this._btnRenderSelected);
      this.Controls.Add((Control) this._btnGetInfo);
      this.Controls.Add((Control) this._lblName);
      this.Controls.Add((Control) this._lblBlendFile);
      this.Controls.Add((Control) this._txtName);
      this.Controls.Add((Control) this._txtBlendFile);
      this.Controls.Add((Control) this._btnBlendFile);
      this.Icon = (Icon) componentResourceManager.GetObject("$this.Icon");
      this.Name = nameof (AddEditModel);
      this.Text = "Add / Edit Model";
      this.FormClosing += new FormClosingEventHandler(this.ModelInfo_FormClosing);
      this.Load += new EventHandler(this.ModelInfo_Load);
      this._nudRenderDistance.EndInit();
      this._nudDirections.EndInit();
      this._nudRenderWidth.EndInit();
      this._nudRenderHeight.EndInit();
      this._nudAASamples.EndInit();
      this._nudKeyframeGrain.EndInit();
      this._gbRenderParameters.ResumeLayout(false);
      this._gbRenderParameters.PerformLayout();
      this._nudModelHeight.EndInit();
      this.ResumeLayout(false);
      this.PerformLayout();
    }
  }
}
