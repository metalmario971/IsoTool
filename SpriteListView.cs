// Decompiled with JetBrains decompiler
// Type: IsoPack.SpriteListView
// Assembly: IsoTool, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A40E7877-59D4-416C-9526-ACFD66F37CC4
// Assembly location: C:\Program Files\Iso Tool\IsoTool.exe

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace IsoPack
{
  public class SpriteListView : ListView
  {
    private int iIconSize = 32;
    private MainForm _objMainForm = (MainForm) null;
    private Func<List<SpriteListViewItem>> GetFrames;
    private Action<List<object>> DeleteFunc;

    public SpriteListView(MainForm mf, Func<List<SpriteListViewItem>> GetFramesFunc, Action<List<object>> deleteFunc)
    {
      this.GetFrames = GetFramesFunc;
      this.DeleteFunc = deleteFunc;
      this._objMainForm = mf;
      this.DoubleClick += new EventHandler(this.SpriteListView_DoubleClick);
      this.MouseClick += new MouseEventHandler(this.SpriteListView_Click);
      this.KeyDown += new KeyEventHandler(this.SpriteListView_KeyDown);
      this.ContextMenu = new ContextMenu(new MenuItem[4]
      {
        new MenuItem("Small Icons", new EventHandler(this.ContextMenu_Click)),
        new MenuItem("Medium Icons", new EventHandler(this.ContextMenu_Click)),
        new MenuItem("Large Icons", new EventHandler(this.ContextMenu_Click)),
        new MenuItem("Huge Icons", new EventHandler(this.ContextMenu_Click))
      });
    }

    private void SpriteListView_KeyDown(object sender, KeyEventArgs e)
    {
      if (e.KeyCode != Keys.Delete)
        return;
      List<object> objectList = new List<object>();
      foreach (ListViewItem selectedItem in this.SelectedItems)
        objectList.Add(selectedItem.Tag);
      this.DeleteFunc(objectList);
    }

    private void ContextMenu_Click(object sender, EventArgs e)
    {
      MenuItem menuItem = (MenuItem) sender;
      if (menuItem != null)
      {
        if (menuItem.Text == "Small Icons")
          this.iIconSize = 32;
        if (menuItem.Text == "Medium Icons")
          this.iIconSize = 64;
        if (menuItem.Text == "Large Icons")
          this.iIconSize = 128;
        if (menuItem.Text == "Huge Icons")
          this.iIconSize = 256;
        this.UpdateListView();
      }
      int num = 0 + 1;
    }

    private void SpriteListView_Click(object sender, MouseEventArgs e)
    {
      if (e.Button != MouseButtons.Right || !this.FocusedItem.Bounds.Contains(e.Location))
        return;
      this.ContextMenu.Show((Control) this, new Point(e.X, e.Y));
    }

    public object GetSelectedObject()
    {
      if (this.SelectedItems.Count == 0)
        return (object) null;
      return this.SelectedItems[0].Tag;
    }

    private string GetObjectName(object f)
    {
      if (f != null)
      {
        if (f is Model)
          return (f as Model).Name;
        if (f is Sprite)
          return (f as Sprite).Name;
        if (f is Frame)
          return (f as Frame).Name;
      }
      return "Unknown-Name*";
    }

    private void SpriteListView_DoubleClick(object sender, EventArgs e)
    {
      object selectedObject = this.GetSelectedObject();
      if (selectedObject == null)
        return;
      if (selectedObject is Model)
        this._objMainForm.AddEditObject(selectedObject as Model, false);
      else if (selectedObject is Sprite)
        this._objMainForm.AddEditObject(selectedObject as Sprite, (selectedObject as Sprite).Model, false);
      else if (selectedObject is Frame)
        this._objMainForm.AddEditObject(selectedObject as Frame, (selectedObject as Frame).Sprite, false);
    }

    public void UpdateListView()
    {
      if (this._objMainForm.TextureInfo.PackedTexture == null)
      {
        this._objMainForm.Log("File was not loaded. or texture was null.");
      }
      else
      {
        object selectedObject = this.GetSelectedObject();
        this.Items.Clear();
        this.View = View.LargeIcon;
        this.Alignment = ListViewAlignment.Top;
        this.HideSelection = false;
        ImageList imageList = new ImageList();
        imageList.ImageSize = new Size(this.iIconSize, this.iIconSize);
        List<SpriteListViewItem> spriteListViewItemList = this.GetFrames();
        for (int index = 0; index < spriteListViewItemList.Count; ++index)
        {
          Frame frame = spriteListViewItemList[index].Frame;
          if (frame == null)
          {
            imageList.Images.Add((Image) this._objMainForm.TextureInfo.PackedTexture.GetDefaultXImage());
          }
          else
          {
            Bitmap imageForFrame = this._objMainForm.TextureInfo.PackedTexture.GetImageForFrame(frame);
            imageList.Images.Add((Image) imageForFrame);
          }
          this.Items.Add(new ListViewItem()
          {
            ImageIndex = index,
            Text = this.GetObjectName(spriteListViewItemList[index].Object),
            Tag = spriteListViewItemList[index].Object
          });
        }
        if (this.View == View.SmallIcon)
          this.SmallImageList = imageList;
        if (this.View == View.LargeIcon)
          this.LargeImageList = imageList;
        if (this.View == View.Tile)
        {
          this.TileSize = new Size(64, 64);
          this.LargeImageList = imageList;
        }
        Globals.SetListViewToSelectedObjectTag((ListView) this, selectedObject);
      }
    }
  }
}
