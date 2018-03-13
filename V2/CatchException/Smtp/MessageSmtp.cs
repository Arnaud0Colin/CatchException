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
using System.Net.Mime;
using System.Text;
using System.Web;

namespace Smtp
{

    public class MessageSmtp : ICloneable
    {

        public MessageSmtp()
        {
            this.From = ServeurInfo.from;
        }

        public MessageSmtp(string From, string[] To,  string Subject, string Body = null )
        {
            this.To = new HashSet<string>(To);
            this.From = From;
            this.Subject = Subject;
            this.Body = Body;
        }

        /// <summary>
        /// Body of the message
        /// </summary>    
        public string Body { get; set; }
        
        /// <summary>
        /// Subject of the message
        /// </summary>    
        public string Subject { get; set; }

        /// <summary>
        /// From Address
        /// </summary>
        public string From { get; set; }

        /// <summary>
        /// From Address
        /// </summary>
        public string Reply { get; set; }

        /// <summary>
        /// To Address
        /// </summary>
        /// <example> Info.to.Add("Test@test.fg"); </example>/// 
        public HashSet<string> To { get; } = new HashSet<string>();
        public HashSet<string> Cc { get; } = new HashSet<string>();
        public HashSet<string> Ci { get; } = new HashSet<string>();

        public MailPriority Priority = MailPriority.Normal;
        public DeliveryNotificationOptions DeliveryNotificationOptions = DeliveryNotificationOptions.Never;
        public bool IsBodyHtml = true;
        public Dictionary<string, string> Headers { get; } = new Dictionary<string, string>();

        public Encoding Encoding { get; set; } = Encoding.UTF8;
        public TransferEncoding BodyTransferEncoding = TransferEncoding.Base64;

        public List<Attachment> Attachments { get; } = new List<Attachment>();

        public object Clone()
        {
            return base.MemberwiseClone();
        }

        public MessageSmtp Copy()
        {
           return (MessageSmtp)base.MemberwiseClone();
        }



        public static explicit operator MailMessage(MessageSmtp mess)
        {
            var ret = new MailMessage();

            ret.Subject = mess.Subject;
          

            foreach(var H in mess.Headers)
                ret.Headers.Add( H.Key, H.Value);

           
            ret.BodyEncoding = mess.Encoding;
            ret.SubjectEncoding = mess.Encoding;
#if  NET45
            ret.BodyTransferEncoding = mess.BodyTransferEncoding;
            ret.HeadersEncoding = mess.Encoding;
#endif

            ret.Body = mess.Body;

            foreach (var file in mess.Attachments)
                ret.Attachments.Add(file);

            foreach (var dest in mess.Ci)
                ret.Bcc.Add(dest);

            foreach (var dest in mess.Cc)
                ret.CC.Add(dest);

            foreach (var dest in mess.To)
                ret.To.Add(dest);

                ret.From = new MailAddress(mess.From);

            ret.Priority = mess.Priority;

#if NET45
            if (mess.Reply != null)
                ret.ReplyToList.Add(new MailAddress(mess.Reply));
#endif

            ret.DeliveryNotificationOptions = mess.DeliveryNotificationOptions;
            ret.IsBodyHtml = mess.IsBodyHtml;


            return ret;
        }

    }

    public static class Mesext
    {

        public static void BuildBody(this MessageSmtp Mess, string Message, string Service, string Signature)
        {

            HtmlElement html = new HtmlElement();

            html.Child(new HtmlElement() { Txt = "<META CHARSET=\"UTF-8\" />" });

            html.Child(new HtmlElement("font", new HtmlAttribute("size", "1")));

            html.Child(new HtmlElement("style").Text("div.MsoMessage { font-size:12.0pt;font-family:\"Garamond\",\"serif\"; } span.MsoNormal, p.MsoNormal, li.MsoNormal, div.MsoNormal {margin:0cm;	margin-bottom:.0001pt;	font-size:12.0pt; font-family:\"Garamond\",\"serif\"; color:#0094ff;   }"));

            var table = new HtmlElement("div");

            table.Child(new HtmlElement("div", new HtmlAttribute("class", "MsoMessage")).Text(Message));


            html.Child(table);
            table.Child(
                new HtmlElement("p", new HtmlAttribute("class", "MsoNormal")).Child(
                new HtmlElement("span", new HtmlAttribute("class", "MsoNormal"))
                    .Text(Service)));

            table.Child(
                          new HtmlElement("p", new HtmlAttribute("class", "MsoNormal")).Child(
                          new HtmlElement("span", new HtmlAttribute("class", "MsoNormal"))
                              .Text(Signature)));

            Mess.Body = html.ToString();

        }

    }




}

