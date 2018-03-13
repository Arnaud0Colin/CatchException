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

using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;

namespace CatchException
{

    public delegate void UnhandledCatchMeEventHandler(CatchMe me);

    public partial class CatchMe
    {
        public static bool IsBatch = false;


        private static event UnhandledCatchMeEventHandler _CallBack;

        public static void CatchUnhandled()
        {
#if !WindowsCE
            System.Windows.Forms.Application.ThreadException -= ThreadExceptionHandler;
            System.Windows.Forms.Application.ThreadException += ThreadExceptionHandler;
            try
            {
               // System.Windows.Forms.Application.SetUnhandledExceptionMode(System.Windows.Forms.UnhandledExceptionMode.CatchException);
            }
#pragma warning disable 0168
            catch(Exception ex)
#pragma warning restore 0168
            {
                // new LogFile().Write((CatchMe.WriteException(ex).Level(0).Screen().Where()));
                throw new Exception("Manque la Declaration SecurityPermission");
            }
#endif

            System.AppDomain.CurrentDomain.UnhandledException -= UnhandledExceptionHandler;
            System.AppDomain.CurrentDomain.UnhandledException += UnhandledExceptionHandler;
            
        }


        public static void CallBack(UnhandledCatchMeEventHandler evt)
        {
            _CallBack -= evt;
            _CallBack += evt;
        }


#if !WindowsCE
        public static void ThreadExceptionHandler(object sender, ThreadExceptionEventArgs e)
       {
           ProcessException((Exception)e.Exception);
       }
#endif

        private static void UnhandledExceptionHandler(object sender, UnhandledExceptionEventArgs e)
        {
            ProcessException((Exception)e.ExceptionObject);
        }



        private static void ProcessException(Exception ex)
        {
            CatchMe me = null;
            try
            {
                
#if !WindowsCE
                (me = CatchMe.WriteException(ex).Level(1).Screen().Where()).Write();
#else
               (me = CatchMe.WriteException(ex).Level(1).Screen().Where("UnhandledExceptionHandler").To<LogFile>()).Write();
#endif



                if (_CallBack != null)
                    _CallBack(me);

            }
            catch (Exception ex2)
            {
#if !WindowsCE
                CatchMe.WriteException(ex2).Level(1).Screen().Where().WriteFile();
#else
                new LogFile().Write((CatchMe.WriteException(ex2).Level(1).Screen().Where("Double UnhandledExceptionHandler")));
#endif
                
            }
            finally
            {

                if (!IsBatch)
                {
                    ExceptionDialog d = new ExceptionDialog(me);
                    d.ShowDialog();
                }
                System.Diagnostics.Process.GetCurrentProcess().Kill();
            }
        }

    }
}
