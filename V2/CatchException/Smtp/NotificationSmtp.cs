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
using CatchException;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.ComponentModel;
#if  NET45
using System.Threading.Tasks;
#endif

namespace Smtp
{
    public class NotificationSmtp : IDisposable
    {
        protected static ServeurInfo _ServeurInfo = null;
        public static ServeurInfo ServeurInfo
        {
            set
            {
                _ServeurInfo = value;
            }
        }

        protected static MessageSmtp _MessageSmtp = null;
        public static MessageSmtp MessageSmtp
        {
            set
            {
                _MessageSmtp = value;
            }
            get
            {
                if (_MessageSmtp != null)
                    return _MessageSmtp.Copy();
                else
                    return new MessageSmtp();
            }


        }


        public static Exception Send(MessageSmtp messagesmtp, bool Trace = false)
        {
            Exception Result = null;

            SmtpClient client = (SmtpClient)_ServeurInfo;
            MailMessage message = (MailMessage)messagesmtp;

            if (Trace)
            {
                TraceFile.WriteLine("<----------");
                TraceFile.WriteLine($" Subjet {message.Subject}");
                TraceFile.WriteLine($" To {messagesmtp.To.Aggregate((a, b) => a + ";" + b)}");
            }

            try
            {
                client.Send(message);
            }
            catch (Exception ex)
            {
                Result = new Exception($"Echec du message {message.Subject}", ex); ;
            }

#if  NET45
            client.Dispose();
#endif

            if (Trace)
                TraceFile.WriteLine("---------->");


            return Result;
        }

#if NET45
        public static void SendAsync(MessageSmtp messagesmtp)
        {
            Task.Factory.StartNew(() => Send(messagesmtp), TaskCreationOptions.LongRunning);
        }
#endif

#if NET45
        public static void SendMailAsync(MessageSmtp messagesmtp)
        {
            SmtpClient client = (SmtpClient)_ServeurInfo;
            client.SendCompleted += Client_SendCompleted;
            MailMessage message = (MailMessage)messagesmtp;

            client.SendMailAsync(message);
        }

        private static void Client_SendCompleted(object sender, AsyncCompletedEventArgs e)
        {
            var smtpClient = (SmtpClient)sender;

            smtpClient.SendCompleted -= Client_SendCompleted;

            if (e.Error != null)
            {
                CatchMe.WriteException(e.Error.InnerException).Where().Write();
            }

        }

#endif



        //        MailMessage CreateMessage(MailInfo mInfo, List<Attachment> Attas = null)
        //        {
        //            MailMessage message = new MailMessage();
        //            message.From = new MailAddress(mInfo.from);
        //            foreach (string s in mInfo.to)
        //                message.To.Add(s);
        //            foreach (string s in mInfo.cc)
        //                message.CC.Add(s);
        //            foreach (string s in mInfo.ci)
        //                message.Bcc.Add(s);
        //            message.Subject = mInfo.subject;
        //            message.Priority = mInfo.Priority;
        //            message.BodyEncoding = mInfo.Encoding;
        //            message.SubjectEncoding = mInfo.Encoding;

        //           message.HeadersEncoding = mInfo.HeadersEncoding;
        //           message.BodyTransferEncoding = mInfo.BodyTransferEncoding;

        //            if (Attas != null)
        //            {
        //                foreach (var atch in Attas)
        //                    message.Attachments.Add(atch);
        //            }


        //            foreach (var aazz in mInfo.Headers)
        //            {
        //                message.Headers.Add(aazz.Key, aazz.Value);
        //            }


        //#if NET45
        //           if (mInfo.reply != null)
        //                message.ReplyToList.Add(new MailAddress(mInfo.reply));
        //#else
        //            if (mInfo.reply != null)
        //                message.ReplyTo = new MailAddress(mInfo.reply);
        //#endif
        //            message.DeliveryNotificationOptions = mInfo.DeliveryNotificationOptions;
        //            message.IsBodyHtml = mInfo.IsBodyHtml;


        //            return message;
        //        }


        //public void SendAsync(IMessage Entry,  List<Attachment> Attas = null)
        //{
        //    Task.Factory.StartNew(() => Send(Entry, _SmtpMailInfo, Attas), TaskCreationOptions.LongRunning);
        //}





        //public Exception Send(IMessage Entry, MailInfo mInfo,  List<Attachment> Attas = null)
        //{
        //    Exception Result = null;
        //    var message = CreateMessage(mInfo, Attas);
        //    message.Body = Entry.ToHtml();
        //    SmtpClient client = (SmtpClient)_sInfo;
        //    try
        //    {
        //        client.Send(message);
        //    }
        //    catch (Exception ex)
        //    {
        //        Result = new Exception($"Echec du message {message.Subject}", ex);
        //    }
        //   client.Dispose();

        //    return Result;
        //}



        //public Exception Send2(IMessage Entry, MailInfo mInfo, List<Attachment> Attas = null)
        //{

        //    Exception Result = null;

        //    var message = CreateMessage(mInfo, Attas);


        //    message.Body = Entry.ToHtml();

        //    SmtpClient client = (SmtpClient)_sInfo;

        //    client.SendCompleted += client_SendCompleted;

        //    try
        //    {
        //        client.SendAsync(message, Tuple.Create(client, message));
        //    }
        //    catch (Exception ex)
        //    {
        //        Result = new Exception($"Echec du message {message.Subject}", ex);
        //    }
        //    client.Dispose();

        //    return Result;
        //}


        //private void client_SendCompleted(object sender, AsyncCompletedEventArgs e)
        //{
        //    throw new NotImplementedException();
        //}

        //protected void Send(IMessage Entry)
        //{
        //    Send(Entry, _SmtpMailInfo);
        //}


        public void Dispose()
        {

        }
    }
}
