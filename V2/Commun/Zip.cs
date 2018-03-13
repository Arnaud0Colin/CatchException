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
using System.IO.Compression;
#if NET35
using System.Linq;
#endif
using System.Text;


namespace CatchException
{
    public class Zip
    {
        static private  void CopyTo(Stream src, Stream dest)
        {
            byte[] bytes = new byte[4096];

            int cnt;

            while ((cnt = src.Read(bytes, 0, bytes.Length)) != 0)
            {
                dest.Write(bytes, 0, cnt);
            }
        }

        static public string CompressToBase64(string str)
        {
            return GetBase64(Compress(str));
        }

        static public string Base64ToUncompress(string str)
        {
            return Uncompress(SetBase64(str));
        }


        static public string GetBase64(byte[] Array)
        {
            return Convert.ToBase64String(Array);
        }

        static public byte[] SetBase64(string str)
        {
            return Convert.FromBase64String(str);
        }

        public static byte[] Compress(string str)
        {
            var bytes = Encoding.UTF8.GetBytes(str);

            using (var msi = new MemoryStream(bytes))
            using (var mso = new MemoryStream())
            {
#if NET45
                using (var gs = new GZipStream(mso, CompressionLevel.Optimal, false))
#else
                using (var gs = new GZipStream(mso, CompressionMode.Compress))
#endif
                {
                    CopyTo(msi, gs);
                }

                return mso.ToArray();
            }
        }

        public static string Uncompress(byte[] bytes)
        {
            using (var msi = new MemoryStream(bytes))
            using (var mso = new MemoryStream())
            {
                using (var gs = new GZipStream(msi, CompressionMode.Decompress))
                {
                    CopyTo(gs, mso);
                }

                return Encoding.UTF8.GetString(mso.ToArray());
            }
        }
    }
}
