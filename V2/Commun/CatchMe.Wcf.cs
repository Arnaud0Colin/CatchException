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
using System.ServiceModel;
#endif
#if !WindowsCE
using System.Security.Principal;

#endif
using System.Text;
using System.Threading;
using System.Windows.Forms;
using System.Xml;


namespace CatchException
{
 
    public partial class CatchMe 
    {

#if DEBUG
        string Serveur = "localhost:2010";
#else
        string Serveur = "catchexception";
#endif
        bool ssl = false;

        string ServeurWcf => $"http{(ssl ? "s" : "" )}://{Serveur}/CatchException.svc";




#if !WindowsCE && NET35
        private static WebCatchException.CatchExceptionClient _fs = null;
#else
          private static WebCatchException.CatchException _fs = null;
#endif


#if NET45
        public void WriteWcfAsync()
        {
            try
            {

                if (_fs == null)
                _fs = new WebCatchException.CatchExceptionClient(new BasicHttpBinding(), new EndpointAddress(ServeurWcf));

                _fs.CatchMeAsync((WebCatchException.MyException)this);

                _fs.Close();
            }
            catch (Exception ex)
            {
                CatchMe.WriteException(ex).Where("LogWcf::Write").WriteFile();
            }
        }
#elif NET35
        public void WriteWcfAsync()
        {
            try
            {

                if (_fs == null)
                    _fs = new WebCatchException.CatchExceptionClient(new BasicHttpBinding(), new EndpointAddress(ServeurWcf));

                _fs.CatchMe((WebCatchException.MyException)this);

                _fs.Close();
            }
            catch (Exception ex)
            {
                CatchMe.WriteException(ex).Where("LogWcf::Write").WriteFile();
            }
        }
#else
        public void WriteWcfAsync()
        {

         try
            {

            if (_fs == null)
                _fs = new WebCatchException.CatchException(ServeurWcf);

            _fs.CatchMeAsync((WebCatchException.MyException)this);
           }
            catch (Exception ex)
            {
                CatchMe.WriteException(ex).Where("LogWcf::Write").WriteFile();
            }
        }
#endif


        public void WriteWcf()
        {

            try
            {
#if !NET35
            if (_fs == null)
                _fs = new WebCatchException.CatchException(ServeurWcf);
                var tt = (WebCatchException.MyException)this;
            _fs.CatchMe(tt);
#else
#if !WindowsCE

                if (_fs == null)
                    _fs = new WebCatchException.CatchExceptionClient(new BasicHttpBinding(), new EndpointAddress(ServeurWcf));
              
                _fs.CatchMe((WebCatchException.MyException)this);

                _fs.Close();

#else
            if (_fs == null)
                _fs = new WcfException.CatchException();

            AsyncCallback cb = new AsyncCallback(OnAsyncWifiLog);
            _fs.BeginWriteInJournal(Entry.ComputerName,Entry.GetApplicationId(), true, Entry.ToHtml(), cb, null);
#endif
            //     }
            //catch (ProtocolException ex)
            //{
            //    CatchMe.WriteException(ex).Where("LogWcf::Write").WriteFile();
#endif


            }
            catch (Exception ex)
            {
                CatchMe.WriteException(ex).Where("LogWcf::Write").WriteFile();
            }

        }

#if WindowsCE
        public void OnAsyncWifiLog(IAsyncResult result)
        {
            try
            {               
              _fs.EndWriteInJournal(result);
            }
            catch (Exception ex)
            {
                CatchMe.WriteException(ex).Where("cWifi::OnAsyncWifiLog").WriteOnly < LogFile>();
            }
        }
#endif


    }
}
