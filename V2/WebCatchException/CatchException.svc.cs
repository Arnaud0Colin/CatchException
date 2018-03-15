using System;
using System.Collections.Generic;
using System.Data.Entity.Core.EntityClient;
using System.Data.Entity.Validation;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using System.Web;
using System.Web.Script.Serialization;
using WebCatchException.Models;
using Zip = CatchException.Zip;

namespace WebCatchException
{

internal static class  rre
    {
       public static CatchMe_Exception EEE(this MyException exception)
        {
            CatchMe_Exception Except = new CatchMe_Exception();

            Except.ApplicationId = exception.ApplicationId;

            Except.ComputerName = exception.ComputerName;
            Except.CurrentPath = exception.CurrentPath;
            Except.Date = exception.Date;
            Except.Login = exception.Login;
            Except.Method = exception.Method;
            Except.OsPlatform = exception.OsPlatform;
            Except.OsServicePack = exception.OsServicePack;
            Except.OsVersion = exception.OsVersion;
            Except.Path = exception.Path;
            Except.ProcessName = exception.ProcessName;
            Except.Program = exception.Program;
            Except.sourceFilePath = exception.sourceFilePath;
            Except.sourceLineNumber = exception.sourceLineNumber;
            Except.UrgenceLevel = (short?)exception.UrgenceLevel;
            Except.Version = exception.Version;
            Except.Debug = exception.Debug;
            Except.CodeStatus = 0;
            Except.SID = exception.SID;

            if (!string.IsNullOrWhiteSpace(exception.bitmap))
            {
                var scree = new CatchMe_Exception_Screen();
                Except.CatchMe_Exception_Screen.Add(scree);
                scree.Screen = Zip.SetBase64(exception.bitmap);
                scree.Ext = ".Jpeg";
                scree.Mime = "image/jpeg";
            }


            int i = 0;
            if (exception._Excep != null)
                foreach (var ex in exception._Excep)
                {
                    var dexp = new CatchMe_Exception_Detail();

                    Except.CatchMe_Exception_Detail.Add(dexp);

                    dexp.Code = i;
                    dexp.Message = ex.Message;
                    dexp.HelpLink = ex.HelpLink;
                    dexp.Exception = ex.Exception;
                    dexp.Source = ex.Source;
                    dexp.StackTrace = ex.StackTrace;
                    dexp.TargetSite = ex.TargetSite;
                    dexp.HResult = ex.HResult;

                    if (ex.Data != null)
                        foreach (var dta in ex.Data)
                        {
                            var ddta = new CatchMe_Exception_Detail_Data();
                            dexp.CatchMe_Exception_Detail_Data.Add(ddta);
                            ddta.Name = dta.Name;
                            ddta.Type = dta.Type;
                            ddta.Value = Zip.Base64ToUncompress(dta.Value);
                        }
                    i++;
                }

            if (exception._Var != null)
                foreach (var obj in exception._Var)
                {
                    var vr = new CatchMe_Exception_Variable();
                    Except.CatchMe_Exception_Variable.Add(vr);
                    vr.Name = obj.Name;
                    vr.Type = obj.Type;
                    vr.Value = Zip.Base64ToUncompress(obj.Value);
                }
            return Except;
        }
    }


    // REMARQUE : vous pouvez utiliser la commande Renommer du menu Refactoriser pour changer le nom de classe "CatchException" à la fois dans le code, le fichier svc et le fichier de configuration.
    // REMARQUE : pour lancer le client test WCF afin de tester ce service, sélectionnez CatchException.svc ou CatchException.svc.cs dans l'Explorateur de solutions et démarrez le débogage.
    public class CatchException : ICatchException
    {
        public void CatchMe(MyException exception)
        {          

            //  bool HasBug = false;
            CatchMe_Exception Except = null;

            try
            {

                var sql = new Entities();

                Except= exception.EEE();

              sql.CatchMe_Exception.Add(Except);

                sql.SaveChanges();
            }
            catch (DbEntityValidationException e)
            {

                SaveException(e, exception);

                //StringBuilder str = new StringBuilder();

                //str.AppendLine(new string('-', 20));
                //str.AppendLine($"----- { DateTime.Now.ToShortDateString()} { DateTime.Now.ToLongTimeString()}");

                //foreach (var eve in e.EntityValidationErrors)
                //{
                //    str.AppendLine($"Entity of type \"{eve.Entry.Entity.GetType().Name}\" in state \"{eve.Entry.State}\" has the following validation errors:");
                //    foreach (var ve in eve.ValidationErrors)
                //    {
                //        str.AppendLine($"- Property: \"{ve.PropertyName}\", Error: \"{ve.ErrorMessage}\"");
                //    }
                //}
                //str.AppendLine(new string('_', 20));

                //var filelog = Path.Combine(HttpContext.Current.ApplicationInstance.Server.MapPath("~/App_Data"), $"DbEntityValidation.log");
                //using (FileStream file = new FileStream(filelog, FileMode.Append, FileAccess.Write, FileShare.Read))
                //{
                //    using (var fs = new StreamWriter(file))
                //    {
                //        fs.AutoFlush = true;
                //        fs.Write(str);
                //    }
                //}


            }
            catch (Exception ex)
            {
                SaveException(ex, exception);

                //var filename = Path.Combine(HttpContext.Current.ApplicationInstance.Server.MapPath("~/App_Data"), $"{DateTime.Now:yyyyMMddHHmmssfff}_{exception?.ComputerName}.ErDump");
                //JavaScriptSerializer scriptSerializer = new JavaScriptSerializer();
                //File.WriteAllText(filename, scriptSerializer.Serialize(exception));

                //var filelog = Path.Combine(HttpContext.Current.ApplicationInstance.Server.MapPath("~/App_Data"), $"WebCatchException.log");
                //using (FileStream file = new FileStream(filelog, FileMode.Append, FileAccess.Write, FileShare.Read))
                //{
                //    using (var fs = new StreamWriter(file))
                //    {
                //        fs.AutoFlush = true;
                //        fs.WriteLine($"{DateTime.Now.ToShortDateString()} {DateTime.Now.ToLongTimeString()} :   {ex.Message} {ex.InnerException?.Message} ");
                //    }
                //}
            }
           


            try
            {
                if(!Except.Debug)
                    Smtp.NotificationSmtp.SendAsync((Smtp.MessageSmtp)Except);
            }
            catch/*(Exception ex)*/
            {

            }



            //catch (DbEntityValidationException e)
            //{
            //    foreach (var eve in e.EntityValidationErrors)
            //    {
            //        Console.WriteLine("Entity of type \"{0}\" in state \"{1}\" has the following validation errors:",
            //            eve.Entry.Entity.GetType().Name, eve.Entry.State);
            //        foreach (var ve in eve.ValidationErrors)
            //        {
            //            Console.WriteLine("- Property: \"{0}\", Error: \"{1}\"",
            //                ve.PropertyName, ve.ErrorMessage);
            //        }
            //    }
            //    throw;
            //}

        }



        public static StringBuilder Dump(DbEntityValidationException e)
        {
            StringBuilder str = new StringBuilder();

            str.AppendLine(new string('-', 20));
            str.AppendLine($"----- { DateTime.Now.ToShortDateString()} { DateTime.Now.ToLongTimeString()}");

            foreach (var eve in e.EntityValidationErrors)
            {
                str.AppendLine($"Entity of type \"{eve.Entry.Entity.GetType().Name}\" in state \"{eve.Entry.State}\" has the following validation errors:");
                foreach (var ve in eve.ValidationErrors)
                {
                    str.AppendLine($"- Property: \"{ve.PropertyName}\", Error: \"{ve.ErrorMessage}\"");
                }
            }
            str.AppendLine(new string('_', 20));

            return str;
        }





        public void SaveException( Exception ex,  MyException exception)
        {
            var filename = Path.Combine(HttpContext.Current.ApplicationInstance.Server.MapPath("~/App_Data"), $"{DateTime.Now:yyyyMMddHHmmssfff}_{exception?.ComputerName}.ErDump");
            JavaScriptSerializer scriptSerializer = new JavaScriptSerializer();
            File.WriteAllText(filename, scriptSerializer.Serialize(exception));

            var filelog = Path.Combine(HttpContext.Current.ApplicationInstance.Server.MapPath("~/App_Data"), $"WebCatchException.log");
            using (FileStream file = new FileStream(filelog, FileMode.Append, FileAccess.Write, FileShare.Read))
            {
                using (var fs = new StreamWriter(file))
                {
                    fs.AutoFlush = true;
                    fs.WriteLine($"{DateTime.Now.ToShortDateString()} {DateTime.Now.ToLongTimeString()} :   {ex.Message} {ex.InnerException?.Message} ");
                }
            }

            if(ex is DbEntityValidationException)
            {
                var e = ex as DbEntityValidationException;

                StringBuilder str = Dump(e);

                var filelog2 = Path.Combine(HttpContext.Current.ApplicationInstance.Server.MapPath("~/App_Data"), $"DbEntityValidation.log");
                using (FileStream file = new FileStream(filelog2, FileMode.Append, FileAccess.Write, FileShare.Read))
                {
                    using (var fs = new StreamWriter(file))
                    {
                        fs.AutoFlush = true;
                        fs.Write(str);
                    }
                }
            }
        }

        public void WriteInJournal(string Ordi, int App, string xml)
        {

            if (string.IsNullOrEmpty(Ordi))
                Ordi = HttpContext.Current.Request.UserHostAddress;

            var filename = Path.Combine(HttpContext.Current.ApplicationInstance.Server.MapPath("~/App_Data"), $"{DateTime.Now:yyyyMMddHHmmssfff}_{Ordi}.OldErDump");
            File.WriteAllText(filename, xml);
        }

    }
}
