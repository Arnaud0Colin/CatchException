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
using System.IO.Compression;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Web;

namespace CatcheException.Wcf
{
    enum DataType
    {
        String = 1,
        Zip = 2
    }

      public class CatchException : ICatchException
    {
        public void WriteInJournal(string Ordi, int App, string xml)
        {

            if (string.IsNullOrEmpty(Ordi))
                Ordi = HttpContext.Current.Request.UserHostAddress;

            try
            {
             
                /*  Your Code  */

            }
            catch (Exception ex)
            {
             /*  
              * LogFile.FilePath = @"c:\";
                CatchException.CatchMe.ApplicationId = 0;
                CatchException.CatchMe.WriteException(ex).Write<LogFile>();
              * */
            }

        }


     

    }
}
