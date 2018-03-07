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
using System.Collections.Generic;
using System.IO;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
 
namespace LogTrace.CatchException
{

    internal static class ConvertString
    {
#if  NET45
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
        private static string GetDataSize(long size)
        {
            string[] nom = { " Octect", " Ko", " Mo", " Go", " To" };

            decimal p = (decimal)size;
            int i = 0;
            while (p > 1024 && i < 4 && (p = p / 1024) > 1) { ; i++; }
            return p.ToString("n2") + nom[i];
        }

#if  NET45
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
        public static string GetFileSize(string File)
        {
            FileInfo f = new FileInfo(File);
            if (f.Exists)
                return GetDataSize(f.Length);
            else
                return "Non present";
        }


    }

    

    public class LogFile : ICatchAction
    {
        /// <summary>
        /// Urgence level of the message
        /// </summary>
        public static ushort Level = 0;

        /// <summary>
        /// Path and name of the file
       /// </summary>

        private static string _Path = null;
        public static string  FilePath
        {
            get { return string.IsNullOrEmpty(_Path) ? 
#if !WindowsCE
                Directory.GetCurrentDirectory()
#else
                    @""
#endif
             : _Path; }
            set { _Path = value.Trim(Path.GetInvalidPathChars()); }
        }

        private static string _File = null;
        public static string FileName
        {
            get { return string.IsNullOrEmpty(_File) ?
#if !WindowsCE
                    System.Diagnostics.Process.GetCurrentProcess().ProcessName.Trim(Path.GetInvalidFileNameChars()) + @".html" 
#else
                    @"LogFile.html"
#endif
                    : _File; }

#if !WindowsCE
            set { _File = value.Trim(Path.GetInvalidFileNameChars()); }
#else
            set { _File = value.Trim(GetInvalidFileNameChars()); }
#endif
        }

        private static string _Fichier = null;        
         public static string Fichier
         {
             get { return string.IsNullOrEmpty(_Fichier) ? Path.Combine(FilePath, FileName) : _Fichier; }
#if !WindowsCE
             set { _Fichier = value.Trim(Path.GetInvalidFileNameChars()).Trim(Path.GetInvalidPathChars()); }
#else
             set { _Fichier = value.Trim(GetInvalidFileNameChars()).Trim(Path.GetInvalidPathChars()); }
#endif
         }

        private static Mutex mut = new Mutex();
       // private static StreamWriter _fs = null;

        /// <summary>
        /// Constructor
        /// </summary>
        public LogFile()
        {
        }

        /// <summary>
        /// <para>
        /// <param name="Entry">LogEntry</param>
        /// </para>
        /// Call by the .Write(); 
        /// </summary>
        public void Write(ICatchMe Entry)
        {
            if (Entry.UrgenceLevel.HasValue && (Level != 0))
                if (Entry.UrgenceLevel.Value > Level)
                    return;

            WriteLine(Entry.ToHtml());
        }

        /// <summary>
        /// <para>
        /// <param name="s">string</param>
        /// </para>
        /// Write OuterXml in the file
        /// </summary>
        private static void WriteLine(string s)
        {
            try
            {
                mut.WaitOne();

                using (StreamWriter fs = new StreamWriter(Fichier, true))
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


#if  NET45
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

    }
}
