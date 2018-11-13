// Decompiled with JetBrains decompiler
// Type: IsoPack.GifWriter
// Assembly: IsoTool, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A40E7877-59D4-416C-9526-ACFD66F37CC4
// Assembly location: C:\Program Files\Iso Tool\IsoTool.exe

using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;

namespace IsoPack
{
  public class GifWriter : IDisposable
  {
    private bool _firstFrame = true;
    private readonly object _syncLock = new object();
    private const long SourceGlobalColorInfoPosition = 10;
    private const long SourceImageBlockPosition = 789;
    private readonly BinaryWriter _writer;

    public GifWriter(Stream OutStream, int DefaultFrameDelay = 500, int Repeat = -1)
    {
      if (OutStream == null)
        throw new ArgumentNullException(nameof (OutStream));
      if (DefaultFrameDelay <= 0)
        throw new ArgumentOutOfRangeException(nameof (DefaultFrameDelay));
      if (Repeat < -1)
        throw new ArgumentOutOfRangeException(nameof (Repeat));
      this._writer = new BinaryWriter(OutStream);
      this.DefaultFrameDelay = DefaultFrameDelay;
      this.Repeat = Repeat;
    }

    public GifWriter(string FileName, int DefaultFrameDelay = 500, int Repeat = -1)
      : this((Stream) new FileStream(FileName, FileMode.OpenOrCreate, FileAccess.Write, FileShare.Read), DefaultFrameDelay, Repeat)
    {
    }

    public int DefaultWidth { get; set; }

    public int DefaultHeight { get; set; }

    public int DefaultFrameDelay { get; set; }

    public int Repeat { get; }

    public void WriteFrame(Image Image, int Delay = 0)
    {
      lock (this._syncLock)
      {
        using (MemoryStream memoryStream = new MemoryStream())
        {
          Image.Save((Stream) memoryStream, ImageFormat.Gif);
          if (this._firstFrame)
            this.InitHeader((Stream) memoryStream, this._writer, Image.Width, Image.Height);
          GifWriter.WriteGraphicControlBlock((Stream) memoryStream, this._writer, Delay == 0 ? this.DefaultFrameDelay : Delay);
          GifWriter.WriteImageBlock((Stream) memoryStream, this._writer, !this._firstFrame, 0, 0, Image.Width, Image.Height);
        }
      }
      if (!this._firstFrame)
        return;
      this._firstFrame = false;
    }

    private void InitHeader(Stream SourceGif, BinaryWriter Writer, int Width, int Height)
    {
      Writer.Write("GIF".ToCharArray());
      Writer.Write("89a".ToCharArray());
      Writer.Write(this.DefaultWidth == 0 ? (short) Width : (short) this.DefaultWidth);
      Writer.Write(this.DefaultHeight == 0 ? (short) Height : (short) this.DefaultHeight);
      SourceGif.Position = 10L;
      Writer.Write((byte) SourceGif.ReadByte());
      Writer.Write((byte) 0);
      Writer.Write((byte) 0);
      GifWriter.WriteColorTable(SourceGif, Writer);
      Writer.Write((short) -223);
      Writer.Write((byte) 11);
      Writer.Write("NETSCAPE2.0".ToCharArray());
      Writer.Write((byte) 3);
      Writer.Write((byte) 1);
      Writer.Write((short) this.Repeat);
      Writer.Write((byte) 0);
    }

    private static void WriteColorTable(Stream SourceGif, BinaryWriter Writer)
    {
      SourceGif.Position = 13L;
      byte[] buffer = new byte[768];
      SourceGif.Read(buffer, 0, buffer.Length);
      Writer.Write(buffer, 0, buffer.Length);
    }

    private static void WriteGraphicControlBlock(Stream SourceGif, BinaryWriter Writer, int FrameDelay)
    {
      SourceGif.Position = 781L;
      byte[] buffer = new byte[8];
      SourceGif.Read(buffer, 0, buffer.Length);
      Writer.Write((short) -1759);
      Writer.Write((byte) 4);
      Writer.Write((byte) ((int) buffer[3] & 247 | 8));
      Writer.Write((short) (FrameDelay / 10));
      Writer.Write(buffer[6]);
      Writer.Write((byte) 0);
    }

    private static void WriteImageBlock(Stream SourceGif, BinaryWriter Writer, bool IncludeColorTable, int X, int Y, int Width, int Height)
    {
      SourceGif.Position = 789L;
      byte[] buffer1 = new byte[11];
      SourceGif.Read(buffer1, 0, buffer1.Length);
      Writer.Write(buffer1[0]);
      Writer.Write((short) X);
      Writer.Write((short) Y);
      Writer.Write((short) Width);
      Writer.Write((short) Height);
      if (IncludeColorTable)
      {
        SourceGif.Position = 10L;
        Writer.Write((byte) (SourceGif.ReadByte() & 63 | 128));
        GifWriter.WriteColorTable(SourceGif, Writer);
      }
      else
        Writer.Write((byte) ((int) buffer1[9] & 7 | 7));
      Writer.Write(buffer1[10]);
      SourceGif.Position = 789L + (long) buffer1.Length;
      for (int count = SourceGif.ReadByte(); count > 0; count = SourceGif.ReadByte())
      {
        byte[] buffer2 = new byte[count];
        SourceGif.Read(buffer2, 0, count);
        Writer.Write((byte) count);
        Writer.Write(buffer2, 0, count);
      }
      Writer.Write((byte) 0);
    }

    public void Dispose()
    {
      this._writer.Write((byte) 59);
      this._writer.BaseStream.Dispose();
      this._writer.Dispose();
    }
  }
}
