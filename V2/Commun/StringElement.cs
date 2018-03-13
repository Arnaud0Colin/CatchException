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
    public static class ConvertString
    {
#if  NET45
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
        public static string GetDataSize(long size)
        {
            string[] nom = { " Octect", " Ko", " Mo", " Go", " To" };

            decimal p = (decimal)size;
            int i = 0;
            while (p > 1024 && i < 4 && (p = p / 1024) > 1) {; i++; }
            return (i == 0 ? p.ToString("n0") : p.ToString("n2")) + nom[i];
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




#if NET45
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
        public static string Format(this TimeSpan tmsp, String format)
        {

          //  string[] nom = { "Semaine", "Jour", "Heure", "Minute", "Second" };
            StringBuilder str = new StringBuilder();

            int Days = tmsp.Days;
            int Hours = tmsp.Hours;
            int Minutes = tmsp.Minutes;
            int Seconds = tmsp.Seconds;

            int i = 0;
            while (i < format.Length)
            {
                char ch = format[i++];
                switch (ch)
                {
                    case 'w':
                    case 'W':
                        if (Days > 0)
                        {
                            int week = Days / 7;
                            Days = Days % 7;
                            if(ch == 'W')
                             str.Append(string.Format("{0} Semaine{1}", week, week > 1 ? "s" : null));
                            else
                             str.Append(string.Format("{0}W", week));

                            week = 0;
                        }
                        break;                   
                    case 'd':
                    case 'D':
                        if (Days > 0)
                        {
                            if (ch == 'D')
                                str.Append(string.Format("{0} Jour{1}", Days, Days > 1 ? "s" : null));
                            else
                                str.Append(string.Format("{0}J", Days));

                            Days = 0;
                        }
                        break;
                    case 'h':
                    case 'H':
                        if (Days > 0 || Hours>0)
                        {
                            Hours += Days * 24;

                            if (ch == 'H')
                                str.Append(string.Format("{0} Heure{1}", Hours, Hours > 1 ? "s" : null));
                            else
                                str.Append(string.Format("{0}H", Hours));

                            Days = 0;
                            Hours = 0;
                        }
                        break;
                    case 'M':
                    case 'm':

                        if (Days > 0 || Hours > 0 || Minutes > 0)
                        {
                            Minutes += ((Days * 24) + Hours) * 60;

                            if (ch == 'M')
                                str.Append(string.Format("{0} Minute{1}", Minutes, Minutes > 1 ? "s" : null));
                            else
                                str.Append(string.Format("{0}M", Minutes));

                            Days = 0;
                            Hours = 0;
                            Minutes = 0;
                        }
                        break;
                    case 'S':
                    case 's':
                        if (Days > 0 || Hours > 0 || Minutes > 0 || Seconds > 0)
                        {
                            Seconds += ((((Days * 24) + Hours) * 60) + Minutes) *60;

                            if (ch == 'S')
                                str.Append(string.Format("{0} Second{1}", Seconds, Seconds > 1 ? "s" : null));
                            else
                                str.Append(string.Format("{0}S", Seconds));

                            Days = 0;
                            Hours = 0;
                            Minutes = 0;
                            Seconds = 0;
                        }
                        break;
                    case ' ':
                        str.Append(' ');
                        break;

                }
            }

            return str.ToString();
        }



    }
}
