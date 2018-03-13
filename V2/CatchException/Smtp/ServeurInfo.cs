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
using System.Configuration;
using System.Data.SqlClient;
using System.Net.Mail;
using System.Text;
using CatchException;
#if NET35
using System.ServiceModel.Channels;
using System.ServiceModel;
using WebCatchException;
#endif
namespace Smtp
{



    public class ServeurInfo
    {
        /// <summary>
        /// Server Smtp
        /// </summary>
        public string server;


        /// <summary>
        /// Port Server Smtp
        /// </summary>
        public int port = 25;

        /// <summary>
        /// NetworkCredential
        /// </summary>
        /// <example> Info.Credential = new System.Net.NetworkCredential("test@MyTest.fr", "01234");; </example>/// 
        public System.Net.NetworkCredential Credential;

        public static string from = null;

        public ServeurInfo()
        {

            this.server = "smtp.tse.fransbonhomme.fr";

#if NET35
 BasicHttpBinding customBinding = new BasicHttpBinding();

            string env = null;
#if DEBUG
            env = "localhost:2010";
#else
            env = "catchexception";
#endif

            string url = $"http://{env}/ParamMail.svc";

            EndpointAddress endpointAddress = new EndpointAddress(url);
            IParamMail ws = new ChannelFactory<IParamMail>(customBinding, endpointAddress).CreateChannel();

            ParamServerMail pm = ws.GetParamServerMail();
            server = pm.Smtp;
            from = pm.Default_exp;
#endif

        }


        public static explicit operator SmtpClient(ServeurInfo sinfo)
        {
            var ret = new SmtpClient(sinfo != null ? sinfo.server : null, sinfo != null ? sinfo.port : 25);
            ret.EnableSsl = (sinfo != null ? sinfo.port : 25) != 25;
            ret.Credentials = sinfo != null ? sinfo.Credential : null;
            return ret;
        }

        private static byte[] BytesFromString(string str)
        {
            return Encoding.ASCII.GetBytes(str);
        }
        private static int GetResponseCode(string ResponseString)
        {
            int ival = 0;
            if (ResponseString.Length > 2 && int.TryParse(ResponseString.Substring(0, 3), out ival))
                return ival;
            else
                return 0;
        }

    }

}
public class ParamServerMail
{
    private string smtp;
    private string default_exp;

    public string Smtp
    {
        get
        {
            return smtp;
        }

        set
        {
            smtp = value;
        }
    }

    public string Default_exp
    {
        get
        {
            return default_exp;
        }

        set
        {
            default_exp = value;
        }
    }
}