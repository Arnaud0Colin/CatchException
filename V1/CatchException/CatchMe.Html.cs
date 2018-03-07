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
using System.Drawing.Imaging;
#if NET35
using System.Linq;
using System.Xml.Linq;
#endif
#if !WindowsCE
using System.Security.Principal;
#endif
using System.Text;
using System.Threading;
using System.Windows.Forms;
using System.Xml;
using System.Reflection;


namespace LogTrace.CatchException
{
    public partial class CatchMe
    {
        private string cache = null;
        private static Mutex mut = new Mutex();

        public static bool AllowDump = false;

        HtmlAttribute[] CreateAttrib( int color, int colspan )
        {
            return new HtmlAttribute[3] {
            new HtmlAttribute("align","left"),
                new HtmlAttribute("bgcolor", "#" + color.ToString("x6")),
              new HtmlAttribute("colspan", colspan.ToString())
              };
        }


        public string ToHtml()
        {
            if (cache != null)
            {
                return cache; // RELEASE MUTEX JAMAIS EFFECTUE
            }

            try
            {
                mut.WaitOne();

                const int thColor = 0xe9b96e;
                const int tdColor = 0xeeeeec;

                int color = 0xcc0000;
                switch (UrgenceLevel)
                {
                    case 3:
                        color = 0x0066CC;
                        break;
                    default:
                        color = 0xcc0000;
                        break;
                }

                HtmlBuilder s = new HtmlBuilder();
                s.Append("<META CHARSET=UTF-8\" />");
                s.Append("<font size='1'> \n <table class='xdebug-error xe-notice' dir='ltr' border='1' cellspacing='0' cellpadding='1'> \n");

                
               // string temp = "<span style='background-color: #" + color.ToString("x6") + "; color: #fce94f; font-size: x-large;'> ( " + ApplicationId.ToString() + " ) </span>" + _Excep.Message;
#if NET45
                string temp = string.Format(@"<span style='background-color: #{0}; color: #fce94f; font-size: x-large;'> ( {1} ) </span> {2}"  , color.ToString("x6"),ApplicationId.ToString(),System.Web.HttpUtility.HtmlEncode(_Excep.Message));
#else
                 string temp = string.Format(@"<span style='background-color: #{0}; color: #fce94f; font-size: x-large;'> ( {1} ) </span> {2}"  , color.ToString("x6"),ApplicationId.ToString(),HttpUtility.HtmlEncode(_Excep.Message));
#endif



                s.TableLigneTitre(new HtmlElement(CreateAttrib(0xf57900, 3)) { Text = temp });

                s.TableLigneTitre(
                new HtmlElement(CreateAttrib(thColor, 1)) { Text = "Machine" },
                new HtmlElement(CreateAttrib(thColor, 1)) { Text = "Os Version" },
                new HtmlElement(CreateAttrib(thColor, 1)) { Text = "Platform" });

                OperatingSystem os = Environment.OSVersion;


                s.TableLigne(
                new HtmlElement(CreateAttrib(tdColor, 1))
                {
#if WindowsCE
             Text = SerialNumber
#else
                    Text = SystemInformation.ComputerName
#endif
                },
                new HtmlElement(CreateAttrib(tdColor, 1))
                {
#if !WindowsCE
                    Text = os.VersionString
#else
             Text = os.Version.ToString()
#endif
                },
                new HtmlElement(CreateAttrib(tdColor, 1))
                {
                    Text = os.Platform.ToString()
                }

                );


#if !WindowsCE
                s.TableLigneTitre(
                    new HtmlElement(CreateAttrib(thColor, 1)) { Text = "Login" },
                    new HtmlElement(CreateAttrib(thColor, 1)) { Text = "Date" },
                    new HtmlElement(CreateAttrib(thColor, 1)) { Text = "TerminalServer" }
                    );

                WindowsIdentity login = WindowsIdentity.GetCurrent();

                s.TableLigne(
                    new HtmlElement(CreateAttrib(tdColor, 1)) { Text = login.Name },
                    new HtmlElement(CreateAttrib(tdColor, 1)) { Text = DateTime.Now.ToString() },
                    new HtmlElement(CreateAttrib(tdColor, 1)) { Text = System.Windows.Forms.SystemInformation.TerminalServerSession.ToString() }
                   );
#else

            s.TableLigneTitre(
                new HtmlElement(CreateAttrib(thColor, 3)) {  Text = "Date" }
                );

             s.TableLigne(                       
            new HtmlElement(CreateAttrib(tdColor, 3) ) { Text = DateTime.Now.ToString() }
                       );
#endif


                System.Reflection.Assembly TheExe = null;
#if !WindowsCE
                if ((TheExe = System.Reflection.Assembly.GetEntryAssembly()) != null)
#else
             if ( (TheExe = MyAssembly) != null)            
#endif
                {


                    s.TableLigneTitre(
                   new HtmlElement(CreateAttrib(thColor, 1)) { Text = "Program" },
                   new HtmlElement(CreateAttrib(thColor, 1)) { Text = "Version" },
                   new HtmlElement(CreateAttrib(thColor, 1)) { Text = "Type" }
                   );

                    s.TableLigne(
                          new HtmlElement(CreateAttrib(tdColor, 1)) { Text = TheExe.GetName().Name },
                          new HtmlElement(CreateAttrib(tdColor, 1)) { Text = TheExe.GetName().Version.ToString() },
                          new HtmlElement(CreateAttrib(tdColor, 1)) { Text = _Excep != null ? _Excep.GetType().Name : "" }
                         );


                    s.TableLigneTitre(
                   new HtmlElement(CreateAttrib(thColor, 3)) { Text = "Path" }
                   );


                    s.TableLigne(
                          new HtmlElement(CreateAttrib(tdColor, 3)) { Text = TheExe.ManifestModule.FullyQualifiedName }
                         );

                }

                if (_Var.Count > 0)
                {

                    s.TableLigneTitre(
                  new HtmlElement(CreateAttrib(thColor, 1)) { Text = "Variable" },
                  new HtmlElement(CreateAttrib(thColor, 1)) { Text = "Type" },
                  new HtmlElement(CreateAttrib(thColor, 1)) { Text = "Valeur" }
                  );

                    foreach (var o in _Var)
                    {
                        string dump = (o.Value.Valeur != null ? o.Value.Valeur.ToString() : "<i>NULL</i>");
                        if (AllowDump)
                        {
                           string RealDump = HttpUtility.HtmlEncode(new DumpObject().Dump(o.Value.Valeur, 0)).Replace("\r\n", "<br/>").Replace("\t", "&nbsp;");
                           dump += string.Format("<a href=\"data:application/zip;base64,{0}\"/>", Zip.CompressToBase64(RealDump));
                           dump = RealDump;
                        }
                       

                       s.TableLigne(
                       new HtmlElement(CreateAttrib(tdColor, 1)) { Text = o.Key },
                       new HtmlElement(CreateAttrib(tdColor, 1)) { Text = o.Value.Type },
                       new HtmlElement(CreateAttrib(tdColor, 1)) { Text = dump }
                                         );

                    }
                }

                if (!string.IsNullOrEmpty(Method))
                {

                    s.TableLigneTitre(
                      new HtmlElement(CreateAttrib(thColor, 1)) { Text = "Method" },
                      new HtmlElement(CreateAttrib(thColor, 1)) { Text = "sourceFile" },
                      new HtmlElement(CreateAttrib(thColor, 1)) { Text = "LineNumber" }
                      );

                    s.TableLigne(
                            new HtmlElement(CreateAttrib(tdColor, 1)) { Text = Method },
                            new HtmlElement(CreateAttrib(tdColor, 1)) { Text = sourceFilePath },
                            new HtmlElement(CreateAttrib(tdColor, 1)) { Text = sourceLineNumber.ToString() }
                           );
                }

                Exception ex = null;

                if ((ex = _Excep.InnerException) != null)
                {
                    s.TableLigneTitre(
                        new HtmlElement(CreateAttrib(thColor, 3)) { Text = "InnerException" }
                        );

                    while (ex != null)
                    {

                        s.TableLigne(
                           new HtmlElement(CreateAttrib(tdColor, 3)) { Text = ex.Message }
                           );
                        ex = ex.InnerException;
                    }
                }

#if !WindowsCE
                if (_Excep.Data != null && _Excep.Data.Count > 1)
                {
                            s.TableLigneTitre(
                      new HtmlElement(CreateAttrib(thColor, 1)) { Text = "Data" },
                      new HtmlElement(CreateAttrib(thColor, 1)) { Text = "Type" },
                      new HtmlElement(CreateAttrib(thColor, 1)) { Text = "Valeur" }
                      );

                       foreach (var fo in _Excep.Data.Keys)
                        {
                            var o = _Excep.Data[fo];

                            s.TableLigne(
                            new HtmlElement(CreateAttrib(tdColor, 1)) { Text = fo.ToString() },
                           new HtmlElement(CreateAttrib(tdColor, 1)) { Text = VariableType(o) },
                           new HtmlElement(CreateAttrib(tdColor, 1)) { Text = (o != null) ? o.ToString() : "<i>NULL</i>" }
                                             );
                        }

                }
#endif

                if (!string.IsNullOrEmpty(_Excep.StackTrace))
                {
                    s.TableLigneTitre(
                    new HtmlElement(CreateAttrib(thColor, 3)) { Text = "Call Stack" }
                    );

                    s.TableLigne(
                            new HtmlElement(CreateAttrib(tdColor, 3)) { Text = _Excep.StackTrace.Replace("\n", "<br/>") }
                           );
                }

                if (bitmap != null)
                {

#if !WindowsCE
                    if (bitmap.Width > 1000)
#if NET35
                        bitmap = bitmap.ResizeImage((float)0.60);
#else
                    bitmap =  BitmapFunction.ResizeImage(bitmap,(float)0.60);
#endif
#endif

                    var base64Data = ScreenShot.GetBase64(bitmap, ImageFormat.Jpeg);

                    s.TableLigneTitre(
                            new HtmlElement(CreateAttrib(thColor, 3)) { Text = "ScreenShot" }
                            );

                    temp = "<img alt=\"sample\" src=\"data:image/jpg;base64," + base64Data + "\"/>";

                    s.TableLigneTitre(
                            new HtmlElement(
                                 new HtmlAttribute("align", "center"),
                    new HtmlAttribute("colspan", "3")
                                ) { Text = temp }
                           );
                }

                s.Append("</table></font>");


                cache = s.ToString();
            }
            finally
            {
                mut.ReleaseMutex();
            }

            return cache;
        }
    }
}
