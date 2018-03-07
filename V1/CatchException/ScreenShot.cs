#region Copyright(c) 1998-2014, Arnaud Colin Licence GNU GPL version 3
/* Copyright(c) 1998-2014, Arnaud Colin
 * All rights reserved.
 *
 * Licence GNU GPL version 3
 * 
 * Redistribution and use in source and binary forms, with or without
 * modification, are permitted provided that the following conditions are met:
 *
 *   -> Redistributions of source code must retain the above copyright
 *      notice, this list of conditions and the following disclaimer.
 *
 *   -> Redistributions in binary form must reproduce the above copyright
 *      notice, this list of conditions and the following disclaimer in the
 *      documentation and/or other materials provided with the distribution.
 */
#endregion

using System;
using System.Text;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using System.Drawing.Imaging;
using System.Drawing.Drawing2D;
using System.IO;
using System.IO.Compression;

namespace LogTrace
{

    /// <summary>
    /// <example> ScreenShot.  </example>
    /// ScreenShot class
    /// </summary>
    public class ScreenShot
    {
        enum RasterOperation : uint { SRC_COPY = 0x00CC0020 }

#if WindowsCE
        [DllImport("coredll.dll")]
#else
        [DllImport("Gdi32.dll")]
#endif      
        private static extern int BitBlt(IntPtr hdcDest, int nXDest, int nYDest, int nWidth, int nHeight, IntPtr hdcSrc, int nXSrc, int nYSrc, RasterOperation rasterOperation);


#if WindowsCE
        [DllImport("coredll.dll")]
#else
        [DllImport("User32.dll")]
#endif      
        private static extern IntPtr GetDC(IntPtr hwnd);

#if WindowsCE
        [DllImport("coredll.dll")]
#else
        [DllImport("User32.dll")]
#endif
        private static extern int ReleaseDC(IntPtr hwnd, IntPtr hdc);


        private Bitmap _Screen = null;


        /// <summary>
        /// Take the screen shot
        ///  <example> ScreenShot.Take()  </example>
        ///  <returns>Bitmap</returns>
        /// </summary>
        static public Bitmap TakeScreen()
        {
            Rectangle bounds = Screen.PrimaryScreen.Bounds;
            IntPtr hdc = GetDC(IntPtr.Zero);
            Bitmap bitmap = new Bitmap(bounds.Width, bounds.Height, PixelFormat.Format16bppRgb565);
            using (Graphics graphics = Graphics.FromImage(bitmap))
            {
                IntPtr dstHdc = graphics.GetHdc();
                BitBlt(dstHdc, 0, 0, bounds.Width, bounds.Height, hdc, 0, 0,
                RasterOperation.SRC_COPY);
                graphics.ReleaseHdc(dstHdc);
            }            
            ReleaseDC(IntPtr.Zero, hdc);
            return bitmap;
        }

        static public string GetBase64(Bitmap map, ImageFormat format)
        {
            MemoryStream ms = new MemoryStream();
            map.Save(ms, format);
            return Convert.ToBase64String(ms.ToArray());
        }


        public ScreenShot Take()
        {
            _Screen = ScreenShot.TakeScreen();
            return this;
        }

        public string GetBase64(ImageFormat format)
        {
            return ScreenShot.GetBase64(_Screen, format);
        }

        public void Save(string path, ImageFormat format)
        {
            _Screen.Save(path, format);
        }

      
    }



    public static class BitmapFunction
    {
#if !WindowsCE
#if NET35
        public static Bitmap ResizeImage(this Bitmap imgToResize, float nPercent)
#else
        public static Bitmap ResizeImage(Bitmap imgToResize, float nPercent)
#endif
        {
            int sourceWidth = imgToResize.Width;
            int sourceHeight = imgToResize.Height;

            int destWidth = (int)(sourceWidth * nPercent);
            int destHeight = (int)(sourceHeight * nPercent);

            Bitmap b = new Bitmap(destWidth, destHeight);
            Graphics g = Graphics.FromImage((Image)b);
            g.InterpolationMode = InterpolationMode.HighQualityBicubic;

            g.DrawImage(imgToResize, 0, 0, destWidth, destHeight);
            g.Dispose();

            return b;
        }

#if NET35
        public static Bitmap ResizeImage(this Bitmap imgToResize, Size size)
        #else
        public static Bitmap ResizeImage( Bitmap imgToResize, Size size)
        #endif
        {
            int sourceWidth = imgToResize.Width;
            int sourceHeight = imgToResize.Height;

            float nPercent = 0;
            float nPercentW = 0;
            float nPercentH = 0;

            nPercentW = ((float)size.Width / (float)sourceWidth);
            nPercentH = ((float)size.Height / (float)sourceHeight);

            if (nPercentH < nPercentW)
                nPercent = nPercentH;
            else
                nPercent = nPercentW;



            return ResizeImage(imgToResize, nPercent);
        }

#endif
    }

}
