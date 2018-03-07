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

using System.Text;
using System.Threading;


namespace LogTrace.CatchException
{
    public partial class CatchMe
    {

        public static string ProcessName
        {
            get
            {
                return System.Diagnostics.Process.GetCurrentProcess().ProcessName.Trim(Path.GetInvalidFileNameChars());
            }
        }

        public static string ApplicationPath
        {
            get
            {
                return Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().ManifestModule.FullyQualifiedName);
            }
        }

    }

  
}
