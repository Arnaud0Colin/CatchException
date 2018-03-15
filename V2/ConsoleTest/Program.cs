using CatchException;

using System;
using System.Collections.Generic;
using System.Collections.Specialized;
//using System.Data.Entity.Validation;
using System.Linq;
using System.Net.Mail;
using System.Text;



namespace ConsoleTest
{
    class Program
    {
        class ddd
        {
           public DateTime dd = DateTime.Now;
            public string ff = "string";
            public static string ddff = "string static";

            public static decimal xc => 58;

            public static decimal sw() => 999;
            protected static double wsq => 14.23;


            public Array rty = null;

            public IEnumerable<byte[]> pol = null;

            double[][] x = new double[5][];

          

            public string fr()
            {
                return "";
            }

        }

        //public static IDictionary<string, object> ObjectToDictionary(object value)
        //{
        //    Dictionary<string, object> ValueDictionary = new Dictionary<string, object>();
        //    if (value != null)
        //    {
        //        foreach (PropertyHelper property in PropertyHelper.GetProperties(value))
        //            ValueDictionary.Add(property.Name, property.GetValue(value));
        //    }
        //    return ValueDictionary;
        //}

        static void Main(string[] args)
        {
            CatchMe.SetOutPut(RenderType.File);

            List<Exception> rr = new List<Exception>();

            for (int i = 0; i < 3; i++)
            {
                rr.Add(new Exception($"test{i}"));
            }


            CatchMe.WriteException("Test").Variable("", rr).Where().Write();


            return;

            Smtp.ServeurInfo s = new Smtp.ServeurInfo();

            Smtp.MessageSmtp mInfo = new Smtp.MessageSmtp();

            //  mInfo.To.Add("AgentSql@fransbonhomme.fr");
            mInfo.To.Add("mcoquil@fransbonhomme.fr");
            mInfo.Reply = "test";
            mInfo.From = "mcoquil@fransbonhomme.eu";
            //
            //mInfo.Headers.Add("Disposition-Notification-To", "ANNAT@fransbonhomme.fr");
            //   mInfo.Headers.Add("X-MC-SendAt", "2017-03-24 10:00:00");
            //X-MC-SendAt


             mInfo.DeliveryNotificationOptions = DeliveryNotificationOptions.OnSuccess | DeliveryNotificationOptions.OnFailure | DeliveryNotificationOptions.Delay;



            mInfo.Subject = $"test Accusé Réception ";

            string tyr = null;

            tyr = $@" Bonjour,<br />
               Test <br />
                Cordialement.
            ";

            //   mInfo.Attachments.AddRange(file);

            if (tyr != null && mInfo.To.Any())
            {
                Smtp.NotificationSmtp.ServeurInfo = s;
                Smtp.Mesext.BuildBody(mInfo, tyr, "Site Intranet Traçabilité ANC", "Le service Achat");
               Exception ex = Smtp.NotificationSmtp.Send(mInfo);
            }






            //var filename = Path.Combine("", $"{DateTime.Now:yyyyMMddHHmmssfff}_{exception?.ComputerName}.ErDump");
            //System.Web.Script.Serialization.JavaScriptSerializer scriptSerializer = new System.Web.Script.Serialization.JavaScriptSerializer();
            //File.WriteAllText(filename, scriptSerializer.Serialize(exception));


            //System.DirectoryServices.AccountManagement.UserPrincipal user = System.DirectoryServices.AccountManagement.UserPrincipal.FindByIdentity(
            //   new System.DirectoryServices.AccountManagement.PrincipalContext(System.DirectoryServices.AccountManagement.ContextType.Domain),
            //   @"DOMTSE\ACOLIN");

            //foreach (System.DirectoryServices.AccountManagement.Principal result in user.GetAuthorizationGroups())
            //{


            //}


            //CatchMe.WriteException("Test").Where().Write();

            //   string hhh =  Dump.GetValue(zzz);

            //  eeee.GetObjectData(new System.Runtime.Serialization.SerializationInfo(), new System.Runtime.Serialization.StreamingContext());

            //CatchMe.WriteException("fff").Where()

            //    //    .Variable("d", new { r= "r", o= "o", rr= "r"})
            ////    .Variable("d", new string[] {  "r", "o", "r" })
            //  .Variable("QueryString", zzz)

            //  .Write();


            //WebCatchException.CatchException dd = new WebCatchException.CatchException();

            //var filename = @"C:\SourceCode\Projet\TFS\Bibliothèque\CatchException\CatchException_V2.dev\WebCatchException\WebCatchException\App_Data\20170322083050513_SRVINTRANET.ErDump";
            //System.Web.Script.Serialization.JavaScriptSerializer scriptSerializer = new System.Web.Script.Serialization.JavaScriptSerializer();
            //var MyEx = scriptSerializer.Deserialize<WebCatchException.MyException>(System.IO.File.ReadAllText(filename));

            //dd.CatchMe(MyEx);


        }

    
    }
}
