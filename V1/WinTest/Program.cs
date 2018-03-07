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

using LogTrace.CatchException;
using System;
using System.Collections.Generic;
using System.IO;
#if NET35
using System.Linq;
#endif
using System.Windows.Forms;

namespace WinTest
{
    static class Program
    {
        public static void InitSmtp()
        {
            SmtpInfo smtpInfo = new SmtpInfo();
            smtpInfo.from = @"CatchException@LogTrace.net";
            smtpInfo.to.Add(@"Administrateur@YourCompany.fr");
            smtpInfo.subject = "Test Sur Windows";
            smtpInfo.server = "srvmes01" /* Server Smtp  */;
            smtpInfo.Credential = new System.Net.NetworkCredential(@"Administrateur@YourCompany.fr", "YourCompany");
            LogSmtp.Info = smtpInfo;
        }
/*
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
        }*/

        /// <summary>
        /// Point d'entrée principal de l'application.
        /// </summary>
        [STAThread]
        static void Main()
        {

            LogFile.Fichier = CatchMe.ApplicationPath + @"\" + CatchMe.ProcessName + @".html";
            CatchMe.Bind<LogFile>();
           

            InitSmtp();

            CatchMe.ApplicationId = 1;

#if !DEBUG
            CatchMe.CatchUnhandled();
#endif
            CatchMe.Bind(SendWcfLog);
           

            TraceFile.Fichier = CatchMe.ApplicationPath + @"\Temp\" + CatchMe.ProcessName + @"Trace.html";
            TraceFile.Level =0;

          /*  ExceptionDialog d = new ExceptionDialog();
            d.ShowDialog();*/
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Form1());

          
        }

        private static void SendWcfLog(CatchMe me)
        {
          //  new LogWcf().Write(me);
            //new LogSmtp().Write(me);

           
        }
    }
}
