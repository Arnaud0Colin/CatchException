using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Web;

namespace CatchException.mvc.Models
{
    public partial class Journal
    {
        public int DataType { get; set; }
        public byte[] Exception { get; set; }

        public enum ExceptionType
        {
            String = 1,
            Zip = 2
        }

#if  NET45
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
        public string GetException()
        {
            switch (DataType)
            {
                case 2:
                    return Unzip(Exception.ToArray());
                default:
                    return Encoding.UTF8.GetString(Exception.ToArray());
            }
        }


        public string DecodeException
        {
            get
            {
                return GetException();
            }
        }

#if  NET45
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
        public void SetException(string str, ExceptionType tp)
        {

            DataType = (int)tp;
            switch (tp)
            {
                case ExceptionType.Zip:
                    Exception = Zip(str);
                    break;
                default:
                    Exception = Encoding.UTF8.GetBytes(str);
                    break;
            }
        }


#if  NET45
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
        public void ZipException(string str)
        {
            SetException(str, ExceptionType.Zip);
        }


#if  NET45
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
        public string UnZipException()
        {
            return GetException();
        }




        private static void CopyTo(Stream src, Stream dest)
        {
            byte[] bytes = new byte[4096];

            int cnt;

            while ((cnt = src.Read(bytes, 0, bytes.Length)) != 0)
            {
                dest.Write(bytes, 0, cnt);
            }
        }

        public static byte[] Zip(string str)
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

        public static string Unzip(byte[] bytes)
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