// Decompiled with JetBrains decompiler
// Type: IsoPack.RECT
// Assembly: IsoTool, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A40E7877-59D4-416C-9526-ACFD66F37CC4
// Assembly location: C:\Program Files\Iso Tool\IsoTool.exe

using System;
using System.Drawing;
using System.Globalization;

namespace IsoPack
{
  public struct RECT
  {
    public int Left;
    public int Top;
    public int Right;
    public int Bottom;

    public RECT(int left, int top, int right, int bottom)
    {
      this.Left = left;
      this.Top = top;
      this.Right = right;
      this.Bottom = bottom;
    }

    public RECT(Rectangle r)
    {
      this = new RECT(r.Left, r.Top, r.Right, r.Bottom);
    }

    public int X
    {
      get
      {
        return this.Left;
      }
      set
      {
        this.Right -= this.Left - value;
        this.Left = value;
      }
    }

    public int Y
    {
      get
      {
        return this.Top;
      }
      set
      {
        this.Bottom -= this.Top - value;
        this.Top = value;
      }
    }

    public int Height
    {
      get
      {
        return this.Bottom - this.Top;
      }
      set
      {
        this.Bottom = value + this.Top;
      }
    }

    public int Width
    {
      get
      {
        return this.Right - this.Left;
      }
      set
      {
        this.Right = value + this.Left;
      }
    }

    public Point Location
    {
      get
      {
        return new Point(this.Left, this.Top);
      }
      set
      {
        this.X = value.X;
        this.Y = value.Y;
      }
    }

    public Size Size
    {
      get
      {
        return new Size(this.Width, this.Height);
      }
      set
      {
        this.Width = value.Width;
        this.Height = value.Height;
      }
    }

    public static implicit operator Rectangle(RECT r)
    {
      return new Rectangle(r.Left, r.Top, r.Width, r.Height);
    }

    public static implicit operator RECT(Rectangle r)
    {
      return new RECT(r);
    }

    public static bool operator ==(RECT r1, RECT r2)
    {
      return r1.Equals(r2);
    }

    public static bool operator !=(RECT r1, RECT r2)
    {
      return !r1.Equals(r2);
    }

    public bool Equals(RECT r)
    {
      return r.Left == this.Left && r.Top == this.Top && r.Right == this.Right && r.Bottom == this.Bottom;
    }

    public override bool Equals(object obj)
    {
      if (obj is RECT)
        return this.Equals((RECT) obj);
      if (obj is Rectangle)
        return this.Equals(new RECT((Rectangle) obj));
      return false;
    }

    public override int GetHashCode()
    {
      return (Rectangle) this.GetHashCode();
    }

    public override string ToString()
    {
      return string.Format((IFormatProvider) CultureInfo.CurrentCulture, "{{Left={0},Top={1},Right={2},Bottom={3}}}", (object) this.Left, (object) this.Top, (object) this.Right, (object) this.Bottom);
    }
  }
}
