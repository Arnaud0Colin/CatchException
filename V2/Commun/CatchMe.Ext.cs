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

    public class FransBonhommeException : Exception
    {
        public FransBonhommeException()
        {
        }

        public FransBonhommeException(string message)
            : base(message)
        {
        }

        public FransBonhommeException(string message, Exception inner)
            : base(message, inner)
        {
        }

        internal int GetHResult()
        {
            return this.HResult;
        }
    }


    public class MessageInfo : Exception
    {
        public MessageInfo()
        {
        }

        public MessageInfo(string message)
            : base(message)
        {
        }

        public MessageInfo(string message, Exception inner)
            : base(message, inner)
        {
        }

        internal int GetHResult()
        {
            return this.HResult;
        }
    }



    public static class ExtentionException
    {
        public static string GetMessage(this IEnumerable<CatchException.WebCatchException.MyExceptionDetail> exArray)
        {
            string message = string.Empty;

            if (exArray != null)
                foreach (var ex in exArray)
                {
                    message += !string.IsNullOrEmpty(message) ? "=>" : string.Empty + ex.Message;
                }

            return message;
        }



#if NET35
        public static string GetMessage(this Exception ex)
#else
            public static string GetMessage(Exception ex )
#endif
        {
            string message = string.Empty;
            while (ex != null)
            {
                message += !string.IsNullOrEmpty(message) ? "=>" : string.Empty + ex.Message;
                ex = ex.InnerException;
            }
            return message;
        }


    }
}
