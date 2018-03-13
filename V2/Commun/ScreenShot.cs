#region Copyright(c) 1998-2018, Arnaud Colin Licence GNU GPL version 3
/* Copyright(c) 1998-2018, Arnaud Colin
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

namespace CatchException.Tools
{

    /// <summary>
    /// <example> ScreenShot.  </example>
    /// ScreenShot class
    /// </summary>

    public static class ScreenShot
    {


        [StructLayout(LayoutKind.Sequential)]
        public struct RECT
        {
            public int left;
            public int top;
            public int right;
            public int bottom;
        }

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


#if WindowsCE
        [DllImport("coredll.dll")]
#else
        [DllImport("User32.dll")]
#endif
        public static extern IntPtr GetDesktopWindow();


#if WindowsCE
        [DllImport("coredll.dll")]
#else
        [DllImport("User32.dll")]
#endif
        public static extern IntPtr GetWindowDC(IntPtr hWnd);




#if WindowsCE
        [DllImport("coredll.dll")]
#else
        [DllImport("User32.dll")]
#endif
        public static extern IntPtr GetWindowRect(IntPtr hWnd, ref RECT rect);



#if WindowsCE
        [DllImport("coredll.dll")]
#else
        [DllImport("Gdi32.dll")]
#endif      
        public static extern IntPtr CreateCompatibleBitmap(IntPtr hDC, int nWidth,
               int nHeight);
#if WindowsCE
        [DllImport("coredll.dll")]
#else
        [DllImport("Gdi32.dll")]
#endif      
        public static extern IntPtr CreateCompatibleDC(IntPtr hDC);
#if WindowsCE
        [DllImport("coredll.dll")]
#else
        [DllImport("Gdi32.dll")]
#endif      
        public static extern bool DeleteObject(IntPtr hObject);
#if WindowsCE
        [DllImport("coredll.dll")]
#else
        [DllImport("Gdi32.dll")]
#endif      
        public static extern IntPtr SelectObject(IntPtr hDC, IntPtr hObject);

#if WindowsCE
        [DllImport("coredll.dll")]
#else
        [DllImport("User32.dll")]
#endif
        private static extern int SetForegroundWindow(IntPtr hWnd);

        private const int SW_RESTORE = 9;

#if WindowsCE
        [DllImport("coredll.dll")]
#else
        [DllImport("User32.dll")]
#endif
        private static extern IntPtr ShowWindow(IntPtr hWnd, int nCmdShow);


#if WindowsCE
        [DllImport("coredll.dll")]
#else
        [DllImport("gdi32.dll")]
#endif
        public static extern bool DeleteDC(IntPtr hDC);

        public static Bitmap TakeCaptureApplication()
        {
            System.Diagnostics.Process proc = System.Diagnostics.Process.GetCurrentProcess();

            // You need to focus on the application
            SetForegroundWindow(proc.MainWindowHandle);
            ShowWindow(proc.MainWindowHandle, SW_RESTORE);

            // You need some amount of delay, but 1 second may be overkill
           // Thread.Sleep(1000);

            RECT rect = new RECT();
            IntPtr error = GetWindowRect(proc.MainWindowHandle, ref rect);

            // sometimes it gives error.
            while (error == (IntPtr)0)
            {
                error = GetWindowRect(proc.MainWindowHandle, ref rect);
            }

            int width = rect.right - rect.left;
            int height = rect.bottom - rect.top;

            Bitmap bmp = new Bitmap(width, height, PixelFormat.Format32bppArgb);
            Graphics.FromImage(bmp).CopyFromScreen(rect.left,
                                                   rect.top,
                                                   0,
                                                   0,
                                                   new Size(width, height),
                                                   CopyPixelOperation.SourceCopy);

            return bmp;
        }

        public static Bitmap TakeCaptureWindow()
        {
            System.Diagnostics.Process proc = System.Diagnostics.Process.GetCurrentProcess();
            

            // get te hDC of the target window
            IntPtr hdcSrc = GetWindowDC(proc.MainWindowHandle);
            // get the size
            RECT windowRect = new RECT();
            GetWindowRect(proc.MainWindowHandle, ref windowRect);
            int width = windowRect.right - windowRect.left;
            int height = windowRect.bottom - windowRect.top;
            // create a device context we can copy to
            IntPtr hdcDest = CreateCompatibleDC(hdcSrc);
            // create a bitmap we can copy it to,
            // using GetDeviceCaps to get the width/height
            IntPtr hBitmap = CreateCompatibleBitmap(hdcSrc, width, height);
            // select the bitmap object
            IntPtr hOld = SelectObject(hdcDest, hBitmap);
            // bitblt over
            BitBlt(hdcDest, 0, 0, width, height, hdcSrc, 0, 0, RasterOperation.SRC_COPY);
            // restore selection
            SelectObject(hdcDest, hOld);
            // clean up 
            DeleteDC(hdcDest);
            ReleaseDC(proc.MainWindowHandle, hdcSrc);
            // get a .NET image object for it
            Bitmap img = Image.FromHbitmap(hBitmap);
            // free up the Bitmap object
            DeleteObject(hBitmap);
            return img;
        }

      
      


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
