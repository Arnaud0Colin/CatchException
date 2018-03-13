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
using System.Collections.Generic;
using System.IO;
#if NET35
using System.Linq;
#endif
using System.Runtime.CompilerServices;
using System.Text;


namespace CatchException
{

 
    public class TraceFile
    {
        public static UInt16 Level = 5;
        private const int Col = 80;


        public static bool Actived
        {
            get
            {
                return (_Fichier != null) && (Level > 0);
            }
        }

        private static bool _OnConsole = true;
        public static bool OnConsole { get { return _OnConsole; } set { _OnConsole = value; } }

        private static bool IsStart = false;


        static StreamWriter fs = null;

        private static string _Fichier = null;
        public static string Fichier
        {
            get { return  _Fichier; }
#if !WindowsCE
            set {

                try
                {

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
                catch
                {
                    _Fichier = null;
                }


                    //throw new Exception("Directory Not found");
            }
#else
             set {
                string file = value.Trim(Path.GetInvalidPathChars());
                _Fichier = Path.Combine(Path.GetDirectoryName(file), Path.GetFileName(file).Trim(GetInvalidFileNameChars()));
            
            }
#endif
        }

#if WindowsCE
        private static char[] GetInvalidFileNameChars()
        {
            return  "[\\~#%&*{}/:<>?|\"-]".ToCharArray();
        }
#endif


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
                    FileStream file = new FileStream(Fichier, FileMode.Append, FileAccess.Write, FileShare.Read);
                    fs =  new StreamWriter(file);
                    fs.AutoFlush = true;
                }

                return fs;
            }
        }

        private static string Text(string s, int tab , char ctab , bool horodate)
        {
             var date = DateTime.Now;

            string text = string.Empty;
            if (horodate)
                text += string.Format("{0} {1} : ", date.ToShortDateString(), date.ToLongTimeString());

            if (tab > 0)
                text += new string(ctab, tab);

            text += s;

            return text;
        }

        public static void WriteTitre(string text = null, char xc = '=')
        {
            if (text == null)
            {
                WriteLine(new String(xc, Col));
                return;
            }

            var rest = 80 - text.Length;

            if (rest < 0)
            {
                WriteLine(text);
            }
            else
            {
                rest /= 2;
                WriteLine(new String(xc, rest) + text + new String(xc, rest));
            }

        }


#if WindowsCE
        public static void WriteBegin(string s)
        {
            WriteBegin(s, 0,  ' ', false);
        }
#endif

#if !WindowsCE
#if  NET45
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
        public static void WriteBegin(string s, int tab = 0, char ctab = ' ', bool horodate = true)
#else
        public static void WriteBegin(string s, int tab , bool horodate )
#endif
        {

            if (IsStart)
                WriteClose();

            IsStart = true;

            if (Actived)            
                FileWrite.Write(Text( s,  tab, ctab, horodate));

            if (_OnConsole)
                Console.Write(Text(s, tab, ctab, false));
        }


#if WindowsCE
        public static void Write(string s)
        {
            Write(s, 0, false);
        }

#endif

#if !WindowsCE
#if  NET45
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
        public static void Write(string s, bool Close = false)
#else
        public static void Write(string s, bool Close )
#endif
        {
            if (Actived)            
                if( Close)
                    FileWrite.WriteLine(Text(s, 0, ' ' , false));
                else 
                    FileWrite.Write(Text(s, 0, ' ', false));

            if (_OnConsole)
                if (Close)
                {
                    IsStart = false;
                    Console.WriteLine(Text(s,0, ' ', false));
                }
                else
                    Console.Write(Text(s,0, ' ', false));
        }

#if  NET45
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
        public static void WriteClose()
        {
            IsStart = false;

            if (Actived)            
                FileWrite.WriteLine(string.Empty);

            if (_OnConsole)
                Console.WriteLine(string.Empty);
        }

         #if !WindowsCE
#if  NET45
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
        public static void WriteClose(string s, int tab = 0, char ctab = ' ')
#else
        public static void WriteClose(string s, int tab, ctab )
#endif
        {
            IsStart = false;

            if (Actived)
                FileWrite.WriteLine(Text(s, tab, ctab, false));

            if (_OnConsole)
                Console.WriteLine(Text(s, tab, ctab, false));

        }

#if WindowsCE
        public static void WriteLine(string s)
        {
            WriteLine(s, 0, false);
        }

#endif

#if !WindowsCE
#if  NET45
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
        public static void WriteLine(string s, int tab = 0, char ctab = ' ', bool horodate = true)
#else
        public static void WriteLine(string s, int tab,char ctab, bool horodate )
#endif
        {

            if (IsStart)
                WriteClose();

            if (Actived)            
                FileWrite.WriteLine(Text(s, tab, ctab, horodate));

            if (_OnConsole)
                Console.WriteLine(Text(s, tab, ctab, false));
        }


        #if !WindowsCE
#if  NET45
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
        public static void WriteLine(UInt16 level, string s, int tab = 0, char ctab = ' ', bool horodate = true)
#else
        public static void WriteLine(UInt16 level, string s, int tab, char ctab, bool horodate)
#endif
        {
            if (IsStart)
                WriteClose();


            if (Actived && TraceFile.Level >= level)
                FileWrite.WriteLine(Text(s, tab, ctab, horodate));

            if (_OnConsole && TraceFile.Level >= level)
                Console.WriteLine(Text(s, tab, ctab, false));
        }

    }
}
