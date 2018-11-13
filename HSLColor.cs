// Decompiled with JetBrains decompiler
// Type: IsoPack.HSLColor
// Assembly: IsoTool, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A40E7877-59D4-416C-9526-ACFD66F37CC4
// Assembly location: C:\Program Files\Iso Tool\IsoTool.exe

using System.Drawing;

namespace IsoPack
{
  public class HSLColor
  {
    private double hue = 1.0;
    private double saturation = 1.0;
    private double luminosity = 1.0;
    private const double scale = 240.0;

    public double Hue
    {
      get
      {
        return this.hue * 240.0;
      }
      set
      {
        this.hue = this.CheckRange(value / 240.0);
      }
    }

    public double Saturation
    {
      get
      {
        return this.saturation * 240.0;
      }
      set
      {
        this.saturation = this.CheckRange(value / 240.0);
      }
    }

    public double Luminosity
    {
      get
      {
        return this.luminosity * 240.0;
      }
      set
      {
        this.luminosity = this.CheckRange(value / 240.0);
      }
    }

    private double CheckRange(double value)
    {
      if (value < 0.0)
        value = 0.0;
      else if (value > 1.0)
        value = 1.0;
      return value;
    }

    public override string ToString()
    {
      return string.Format("H: {0:#0.##} S: {1:#0.##} L: {2:#0.##}", (object) this.Hue, (object) this.Saturation, (object) this.Luminosity);
    }

    public string ToRGBString()
    {
      Color color = (Color) this;
      return string.Format("R: {0:#0.##} G: {1:#0.##} B: {2:#0.##}", (object) color.R, (object) color.G, (object) color.B);
    }

    public static implicit operator Color(HSLColor hslColor)
    {
      double num1 = 0.0;
      double num2 = 0.0;
      double num3 = 0.0;
      if (hslColor.luminosity != 0.0)
      {
        if (hslColor.saturation == 0.0)
        {
          double luminosity;
          num3 = luminosity = hslColor.luminosity;
          num2 = luminosity;
          num1 = luminosity;
        }
        else
        {
          double temp2 = HSLColor.GetTemp2(hslColor);
          double temp1 = 2.0 * hslColor.luminosity - temp2;
          num1 = HSLColor.GetColorComponent(temp1, temp2, hslColor.hue + 1.0 / 3.0);
          num2 = HSLColor.GetColorComponent(temp1, temp2, hslColor.hue);
          num3 = HSLColor.GetColorComponent(temp1, temp2, hslColor.hue - 1.0 / 3.0);
        }
      }
      return Color.FromArgb((int) ((double) byte.MaxValue * num1), (int) ((double) byte.MaxValue * num2), (int) ((double) byte.MaxValue * num3));
    }

    private static double GetColorComponent(double temp1, double temp2, double temp3)
    {
      temp3 = HSLColor.MoveIntoRange(temp3);
      if (temp3 < 1.0 / 6.0)
        return temp1 + (temp2 - temp1) * 6.0 * temp3;
      if (temp3 < 0.5)
        return temp2;
      if (temp3 < 2.0 / 3.0)
        return temp1 + (temp2 - temp1) * (2.0 / 3.0 - temp3) * 6.0;
      return temp1;
    }

    private static double MoveIntoRange(double temp3)
    {
      if (temp3 < 0.0)
        ++temp3;
      else if (temp3 > 1.0)
        --temp3;
      return temp3;
    }

    private static double GetTemp2(HSLColor hslColor)
    {
      return hslColor.luminosity >= 0.5 ? hslColor.luminosity + hslColor.saturation - hslColor.luminosity * hslColor.saturation : hslColor.luminosity * (1.0 + hslColor.saturation);
    }

    public static implicit operator HSLColor(Color color)
    {
      return new HSLColor()
      {
        hue = (double) color.GetHue() / 360.0,
        luminosity = (double) color.GetBrightness(),
        saturation = (double) color.GetSaturation()
      };
    }

    public void SetRGB(int red, int green, int blue)
    {
      HSLColor hslColor = (HSLColor) Color.FromArgb(red, green, blue);
      this.hue = hslColor.hue;
      this.saturation = hslColor.saturation;
      this.luminosity = hslColor.luminosity;
    }

    public HSLColor()
    {
    }

    public HSLColor(Color color)
    {
      this.SetRGB((int) color.R, (int) color.G, (int) color.B);
    }

    public HSLColor(int red, int green, int blue)
    {
      this.SetRGB(red, green, blue);
    }

    public HSLColor(double hue, double saturation, double luminosity)
    {
      this.Hue = hue;
      this.Saturation = saturation;
      this.Luminosity = luminosity;
    }
  }
}
