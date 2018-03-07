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
#if NET35
using System.Linq;
#endif
using System.Runtime.CompilerServices;
using System.Text;


namespace LogTrace.CatchException
{
    public class TraceFile
    {
        public static UInt16 Level = 5;

        public static bool Actived
        {
            get
            {
                return (_Fichier != null) && (Level > 0);
            }
        }
        static StreamWriter fs = null;

        private static string _Fichier = null;
        public static string Fichier
        {
            get { return  _Fichier; }
#if !WindowsCE
            set {
                string file = value.Trim(Path.GetInvalidPathChars());
                file = Path.Combine(Path.GetDirectoryName(file), Path.GetFileName(file).Trim(Path.GetInvalidFileNameChars()));
                

                if (Directory.Exists(Path.GetDirectoryName(file)))
                    _Fichier = file;
                else
                {
                    _Fichier = null;
                    if (fs != null)
                    {
                        fs.Close();
                        fs = null;
                    }

                }
                   
            }
#else
             set {
                string file = value.Trim(Path.GetInvalidPathChars());
                _Fichier = Path.Combine(Path.GetDirectoryName(file), Path.GetFileName(file).Trim(GetInvalidFileNameChars()));
            
            }
#endif
        }


#if  NET45
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
        public static void Stop()
        {
            _Fichier = null;
            if (fs != null)
            {
                fs.Close();
                fs = null;
            }
        }


#if  NET45
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
        public static string GetFileSize()
        {
            return ConvertString.GetFileSize(_Fichier);
        }


        public static void DeleteFile()
        {
            if( _Fichier != null)
            {
                string tmp = _Fichier;
                _Fichier = null;
                if( fs != null)
                    fs.Close();
                fs = null;
                File.Delete(tmp);
                 _Fichier = tmp;
            }
        }


        private static StreamWriter FileWrite
        {
            get
            {
                if (fs == null)
                {
                    fs = new StreamWriter(Fichier, true);
                    fs.AutoFlush = true;
                }

                return fs;
            }
        }

        private static string Text(string s, int tab , bool horodate)
        {
             var date = DateTime.Now;

            string text = string.Empty;
            if (horodate)
                text += string.Format("{0} {1} : ", date.ToShortDateString(), date.ToLongTimeString());

            if (tab > 0)
                text += new string(' ', tab);

            text += s;

            return text;
        }

        public enum Option
       {
           Start,
           Continue,
           Stop,
       }

#if  NET45
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
        public static void Write(string s, int tab = 0, bool horodate = true, Option option = Option.Continue)
        {
            if (Actived)
                FileWrite.Write(Text(s, tab, horodate));
        }


#if  NET45
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
        public static void Start(string s, int tab = 0, bool horodate = true)
        {
            if(Actived)            
                FileWrite.Write(Text( s,  tab , horodate));
        }

#if  NET45
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
        public static void Middle(string s, int tab = 0, bool Close = false)
        {
            if (Actived)            
                if( Close)
                    FileWrite.WriteLine(Text(s, tab, false));
                else 
                    FileWrite.Write(Text(s, tab, false));
        }

#if  NET45
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
        public static void Close()
        {
            if (Actived)            
                FileWrite.WriteLine(string.Empty);
        }

#if  NET45
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
        public static void Close(string s, int tab = 0)
        {
            if (Actived)
                FileWrite.WriteLine(Text(s, tab, false));

        }

#if  NET45
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
        public static void StartClose(string s, int tab = 0, bool horodate = true)
        {

            if (Actived)            
                FileWrite.WriteLine(Text(s, tab, horodate));
        }

#if  NET45
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
        public static void StartClose(UInt16 level, string s, int tab = 0, bool horodate = true)
        {

            if (Actived && TraceFile.Level >= level)
                FileWrite.WriteLine(Text(s, tab, horodate));
        }

    }
}
