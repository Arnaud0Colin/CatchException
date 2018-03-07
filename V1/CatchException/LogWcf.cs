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
#if NET32
using System.Linq;
#endif
using System.Collections.Generic;
using System.Text;
using System.Threading;
#if !WindowsCE && NET35
using System.ServiceModel.Description;
using System.ServiceModel;
#endif


namespace LogTrace.CatchException
{
    public class LogWcf : ICatchAction
    {

        public static ushort Level = 0;
        public const string ServiceUrl = "http://{1}/CatchException.svc";

        public string IpAddress = null;

        public string FullUrl
        {
            get
            {
                return string.Format(ServiceUrl, IpAddress);
            }
        }

        //  private static Mutex mut = new Mutex();
#if !WindowsCE && NET35
        private static CatchException.Service.CatchExceptionClient _fs = null;
#else
        private static CatchException.Service.CatchException _fs = null;
#endif

        public void Write(ICatchMe Entry)
        {
            if (IpAddress != null)
                return;

            if (Entry.UrgenceLevel.HasValue && (Level != 0))
                if (Entry.UrgenceLevel.Value > Level)
                    return;

            try
            {
#if !NET35
            if (_fs == null)
                _fs = new CatchException.Service.CatchException();
            _fs.WriteInJournal(Entry.ComputerName, Entry.GetApplicationId(), true, Entry.ToHtml());
#else
#if !WindowsCE

                if (_fs == null)
                    _fs = new CatchException.Service.CatchExceptionClient(new BasicHttpBinding(), new EndpointAddress(FullUrl));
                _fs.WriteInJournal(Entry.ComputerName, Entry.GetApplicationId(), Entry.ToHtml());

                _fs.Close();
#else
            if (_fs == null)
                _fs = new WcfException.CatchException();

            AsyncCallback cb = new AsyncCallback(OnAsyncWifiLog);
            _fs.BeginWriteInJournal(Entry.ComputerName,Entry.GetApplicationId(), true, Entry.ToHtml(), cb, null);
#endif
#endif

            }
            catch(Exception ex)
            {
                CatchMe.WriteException(ex).Where("LogWcf::Write").WriteOnly<LogFile>();
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
