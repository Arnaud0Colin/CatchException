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
using CatchException.Tools;
using System;
using System.Collections.Generic;
using System.Drawing.Imaging;
#if NET35
using System.Linq;
using System.Xml.Linq;
#endif
#if !WindowsCE
using System.Security.Principal;
#endif
using System.Text;
using System.Threading;
using System.Windows.Forms;
using System.Xml;
using System.IO;
using System.Runtime.CompilerServices;
using System.IO.Compression;

namespace CatchException
{
 
    public partial class CatchMe
    {

        public RenderType OutPut { get; private set; } =  RenderType.Wcf;

       public void SetOutPut(RenderType output)
        {
           this.OutPut = output;
        }


#if !WindowsCE
        private static long MaxFileSize = 10000000;
#else
        private static long MaxFileSize = 1000000;
#endif

        /// <summary>
        /// Path and name of the file
        /// </summary>
        private static string _Path = null;
        public static string FilePath
        {
            get
            {
                return string.IsNullOrEmpty(_Path) ?
#if !WindowsCE
                Directory.GetCurrentDirectory()
#else
                    @""
#endif
             : _Path;
            }
            set { _Path = value.Trim(System.IO.Path.GetInvalidPathChars()); }
        }

        static string  GetInFile(string ext) => $@"{GetProcessName}_{DateTime.Now:HHmmss}.{ext}";
        public static string GetDefaultFile => System.IO.Path.Combine( GetCurrentPath , FileName );

#if NET35
        static string Extension = @".zlog";
#else
         static string Extension = @".html";
#endif


        private static string _File = null;
        public static string FileName
        {
            get
            {
                return string.IsNullOrEmpty(_File) ?
#if !WindowsCE
                    GetProcessName + Extension
#else
                    @"LogFile" + Extension
#endif
                    : _File;
            }

#if !WindowsCE
            set { _File = value.Trim(System.IO.Path.GetInvalidFileNameChars()); }
#else
            set { _File = value.Trim(GetInvalidFileNameChars()); }
#endif
        }

        private static string _Fichier = null;
        public static string Fichier
        {
            get { return string.IsNullOrEmpty(_Fichier) ? System.IO.Path.Combine(FilePath, FileName) : _Fichier; }
#if !WindowsCE
            set { _Fichier = value.Trim(System.IO.Path.GetInvalidFileNameChars()).Trim(System.IO.Path.GetInvalidPathChars()); }
#else
             set { _Fichier = value.Trim(GetInvalidFileNameChars()).Trim(Path.GetInvalidPathChars()); }
#endif
        }



        public void WriteFile()
        {

#if NET45
            if (System.IO.Path.GetExtension(FileName) == ".html")
                WriteLinehtml(this.ToHtml());
            else if (System.IO.Path.GetExtension(FileName) == ".xml")
                WriteLinehtml(this.ToXml());
            else
                WriteLineZip(this.ToXml(),"xml");
#else
                WriteLinehtml(this.ToHtml());
#endif

        }


        /// <summary>
        /// <para>        
        /// </para>
        /// Write OuterXml in the file
        /// </summary>
        public static void CleanFile()
        {
            FileInfo fi = new FileInfo(Fichier);
            if (fi.Exists && fi.Length > MaxFileSize)
            {
                fi.Delete();
            }
        }


        /// <summary>
        /// <para>
        /// <param name="s">string</param>
        /// </para>
        /// Write OuterXml in the file
        /// </summary>
        private static void WriteLinehtml(string s)
        {
            try
            {
                mut.WaitOne();

                bool IsTobig = false;

                FileInfo fi = new FileInfo(Fichier);
                if (fi.Exists)
                {
                    IsTobig = fi.Length > MaxFileSize;
                }

                using (StreamWriter fs = new StreamWriter(Fichier, !IsTobig))
                {
                    fs.WriteLine(s);
                    fs.Flush();
                    fs.Close();
                }
            }
            catch { }
            finally
            {
                mut.ReleaseMutex();
            }
        }


#if NET45
        private static void WriteLineZip(string s, string ext = "xml")
        {
            try
            {
                mut.WaitOne();

                FileInfo fi = new FileInfo(Fichier);
                if (fi.Exists)
                {
                    if(fi.Length > MaxFileSize)
                        File.Delete(Fichier);
                }

                using (var archive = ZipFile.Open(Fichier, ZipArchiveMode.Update))
                {

                    string path = $"{DateTime.Now:yyyyMMdd}/{GetInFile(ext)}";

                    var demoFile = archive.CreateEntry(path, CompressionLevel.Optimal);
                    using (StreamWriter fs = new StreamWriter(demoFile.Open()))
                    {
                        fs.WriteLine(s);
                        fs.Flush();
                        fs.Close();
                    }
                }
            }
            catch { }
            finally
            {
                mut.ReleaseMutex();
            }
        }
#endif




#if NET45
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
        public static string GetFileSize()
        {
            return ConvertString.GetFileSize(Fichier);
        }



#if WindowsCE

      public static char[] GetInvalidFileNameChars()
      {
         return (char[])InvalidFileNameChars.Clone();
      }

      private static char[] InvalidFileNameChars = new char[41]
      { '"','<','>','|',char.MinValue,'\x0001','\x0002','\x0003','\x0004','\x0005','\x0006','\a','\b','\t','\n','\v','\f','\r','\x000E','\x000F','\x0010','\x0011','\x0012','\x0013','\x0014','\x0015','\x0016','\x0017','\x0018','\x0019','\x001A','\x001B','\x001C','\x001D','\x001E','\x001F',':','*','?','\\','/'  };
#endif



        public static bool FileExist
        {
            get
            {
                return File.Exists(Fichier);
            }
        }

        public static void DeleteFile()
        {
            File.Delete(Fichier);
        }

    }

    [Flags]
    public enum RenderType
    {
        None = 0x0,
        File = 0x1,
    //    Stmp = 0x2,
        Wcf = 0x4,
    }
}
