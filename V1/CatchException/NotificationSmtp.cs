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
#if NET35
using System.Linq;
#endif
using System.Text;
#if !WindowsCE
using System.Net.Mail;
#endif

namespace LogTrace.CatchException
{

    public class SmtpInfo
    {
        /// <summary>
        /// Server Smtp
        /// </summary>
        public string server;

        /// <summary>
        /// From Address
        /// </summary>
        public string from;

        /// <summary>
        /// To Address
        /// </summary>
        /// <example> Info.to.Add("Test@test.fg"); </example>/// 
        public List<string> to = new List<string>();

        /// <summary>
        /// Subject of the message
        /// </summary>        
        public string subject;

        /// <summary>
        /// NetworkCredential
        /// </summary>
        /// <example> Info.Credential = new System.Net.NetworkCredential("test@MyTest.fr", "01234");; </example>/// 
        public System.Net.NetworkCredential Credential;
    }


   public class NotificationSmtp
    {
       protected static SmtpInfo _Info = null;
        /// <summary>
        /// Set StmpInfo; 
        /// </summary>   
        public static SmtpInfo Info
        {
            set
            {
                _Info = value;
            }
        }

       protected void Send(ICatchMe Entry)
       {
#if !WindowsCE
           MailMessage message = new MailMessage();
           message.From = new MailAddress(_Info.from);
           foreach (string s in _Info.to)
               message.To.Add(s);
           message.Subject = _Info.subject;
           message.Body = Entry.ToHtml();
           message.IsBodyHtml = true;

           SmtpClient client = new SmtpClient(_Info.server, 25);
           client.Credentials = _Info.Credential;
           try
           {
               client.Send(message);
           }
           catch (Exception ex)
           {
               LogTrace.CatchException.CatchMe.WriteException(ex).WriteOnly<LogFile>();
           }
#if NET45
           client.Dispose();
#endif
#else
            LogTrace.CatchException.CatchMe.WriteMessage("LogSmtp Not Working in WindowsCE").Write<LogFile>();
#endif
       }




    }
}
