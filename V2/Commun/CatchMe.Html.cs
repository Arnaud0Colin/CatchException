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
using System.Linq;
#if NET35
using System.Xml.Linq;
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
        private string cache = null;
        private static Mutex mut = new Mutex();

        HtmlAttribute[] CreateAttrib( int color, int colspan )
        {
            return new HtmlAttribute[3] {
            new HtmlAttribute("align","left"),
                new HtmlAttribute("bgcolor", "#" + color.ToString("x6")),
              new HtmlAttribute("colspan", colspan.ToString())
              };
        }

#if NET35
        public string ToXml()
        {

            WebCatchException.MyException rtt = (WebCatchException.MyException)(this);

            System.Runtime.Serialization.DataContractSerializer ser =
           new System.Runtime.Serialization.DataContractSerializer(typeof(WebCatchException.MyException));

            using (var mso = new System.IO.MemoryStream())
            {
                ser.WriteObject(mso, rtt);
                return System.Text.Encoding.UTF8.GetString(mso.ToArray());
            }
  
        }
#endif


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

                string AppIdStr =  string.Format("({0})", ApplicationId.ToString());
                int color = 0x0;
                switch (UrgenceLevel)
                {
                    case 2:
                        color = 0x00CC00;
                        break;
                    case 1:
                        color = 0x0000FF;
                        break;
                    default:
                        color = 0xcc0000;
                        break;
                }

                HtmlElement html = new HtmlElement();

                html.Child(new HtmlElement() { Txt = "<META CHARSET=\"UTF-8\" />" });

                html.Child(new HtmlElement("font", new HtmlAttribute("size", "1")));

                var table = new HtmlElement("table",
                    new HtmlAttribute("class", "xdebug-error xe-notice"),
                    new HtmlAttribute("dir", "ltr"),
                    new HtmlAttribute("border", "1"),
                    new HtmlAttribute("cellspacing", "0"),
                    new HtmlAttribute("cellpadding", "1"));

                html.Child(table);

                table.Child(new HtmlElement("tr")
                   .Child(
                        new HtmlElement("th", CreateAttrib(0xf57900, 3))
                        .Child( new HtmlElement("span",
                            new HtmlStyles(
                             color != 0 ?  new HtmlStyle("background-color", "#" + color.ToString("x6")) : null,
                                new HtmlStyle("color", "#fce94f"),
                                new HtmlStyle("font-size", "x-large")))
                    .Text(AppIdStr))
#if !WindowsCE
                    .Child(new HtmlElement() { Txt = _Excep.Any() ? _Excep.FirstOrDefault()?.Message : string.Empty })));
#else
                 .Child(new HtmlElement() { Txt = Any(_Excep) ? FirstOrDefault(_Excep)?.Message : string.Empty })));
#endif

                table.Child(new HtmlElement("tr").Child(
                   new HtmlElement("th", CreateAttrib(thColor, 1)).Text("Machine"),
                   new HtmlElement("th", CreateAttrib(thColor, 1)).Text("Os Version"),
                   new HtmlElement("th", CreateAttrib(thColor, 1)).Text("Platform"))                   
                   );

                OperatingSystem os = Environment.OSVersion;

                table.Child(new HtmlElement("tr")
                  .Child(new HtmlElement("td", CreateAttrib(tdColor, 1))                      
#if WindowsCE
                    .Text(SerialNumber))
#else
                    .Text(SystemInformation.ComputerName))
#endif
.Child(new HtmlElement("td", CreateAttrib(tdColor, 1))    
#if !WindowsCE
                    .Text( os.VersionString))
#else
                    .Text( os.Version.ToString()))
#endif
.Child(new HtmlElement("td", CreateAttrib(tdColor, 1))
                 .Text( os.Platform.ToString())));
#if !WindowsCE

                table.Child(new HtmlElement("tr").Child(
                    new HtmlElement("th", CreateAttrib(thColor, 1)).Text("Login"),
                    new HtmlElement("th", CreateAttrib(thColor, 1)).Text("Date"),
                    new HtmlElement("th", CreateAttrib(thColor, 1)).Text("TerminalServer"))
                );
            
                table.Child(new HtmlElement("tr").Child(
                    new HtmlElement("td", CreateAttrib(tdColor, 1)).Text(Login),
                new HtmlElement("td", CreateAttrib(tdColor, 1)).Text(DateTime.Now.ToString()),
                new HtmlElement("td", CreateAttrib(tdColor, 1)).Text(System.Windows.Forms.SystemInformation.TerminalServerSession.ToString()))
                );
#else
                 table.Child(new HtmlElement("tr")
                .Child(new HtmlElement("th", CreateAttrib(thColor, 3)).Text("Date"))
                );

                table.Child(new HtmlElement("tr")
                .Child(new HtmlElement("td", CreateAttrib(tdColor, 3)).Text(DateTime.Now.ToString()))
                );
#endif
                    table.Child(new HtmlElement("tr").Child(
                                  new HtmlElement("th", CreateAttrib(thColor, 1)).Text("Program"),
                                  new HtmlElement("th", CreateAttrib(thColor, 1)).Text("Version"),
                                  new HtmlElement("th", CreateAttrib(thColor, 1)).Text("Type"))
                                  );

                    table.Child(new HtmlElement("tr").Child(
                                       new HtmlElement("td", CreateAttrib(tdColor, 1)).Text(Program),
                                       new HtmlElement("td", CreateAttrib(tdColor, 1)).Text(Version),
                                       new HtmlElement("td", CreateAttrib(tdColor, 1)).Text(Type))
                                       );

                    table.Child(new HtmlElement("tr")
                                 .Child(new HtmlElement("th", CreateAttrib(thColor, 3)).Text("Path"))
                                 );

                    table.Child(new HtmlElement("tr")
                                .Child(new HtmlElement("td", CreateAttrib(tdColor, 3)).Text(Path))
                                );


                if (_Var != null && _Var.Count() > 0)
                {

                    table.Child(new HtmlElement("tr").Child(
                                new HtmlElement("th", CreateAttrib(thColor, 1)).Text("Variable"),
                                new HtmlElement("th", CreateAttrib(thColor, 1)).Text("Type"),
                                new HtmlElement("th", CreateAttrib(thColor, 1)).Text("Valeur"))
                                );

                    foreach (var o in _Var)
                    {
                       table.Child(new HtmlElement("tr").Child(
                                      new HtmlElement("td", CreateAttrib(tdColor, 1)).Text(o.Name),
                                      new HtmlElement("td", CreateAttrib(tdColor, 1)).Text(o.Type),
                                      new HtmlElement("td", CreateAttrib(tdColor, 1)).Text(o.ValueToHtml()))
                                      );
                    }
                }

                if (!string.IsNullOrEmpty(Method))
                {
                    table.Child(new HtmlElement("tr").Child(
                               new HtmlElement("th", CreateAttrib(thColor, 1)).Text("Method"),
                               new HtmlElement("th", CreateAttrib(thColor, 1)).Text("sourceFile"),
                               new HtmlElement("th", CreateAttrib(thColor, 1)).Text("LineNumber"))
                               );

                    table.Child(new HtmlElement("tr").Child(
                                        new HtmlElement("td", CreateAttrib(tdColor, 1)).Text(Method),
                                        new HtmlElement("td", CreateAttrib(tdColor, 1)).Text(sourceFilePath),
                                        new HtmlElement("td", CreateAttrib(tdColor, 1)).Text(sourceLineNumber.ToString()))
                                        );
                }

                WebCatchException.MyExceptionDetail ex = null;


#if !WindowsCE
                if (_Excep != null && _Excep.Count() >= 1)
                    ex = _Excep.FirstOrDefault();
#else
                  if (_Excep != null )
                    ex = FirstOrDefault(_Excep);
             
#endif

#if !WindowsCE
                if ( _Excep != null && _Excep.Count() > 1 )
#else
              if ( _Excep != null && Count(_Excep) > 1 )  
#endif
                {
                    table.Child(new HtmlElement("tr")
                                                   .Child(new HtmlElement("th", CreateAttrib(thColor, 3)).Text("InnerException"))
                                                   );

                    foreach(var _ex in _Excep)
//
                 //   for( int i = 1; i < _Excep.Count(); i++)
                    {
                        table.Child(new HtmlElement("tr")
                               .Child(new HtmlElement("td", CreateAttrib(tdColor, 3)).Text(_ex?.Message))
                              );
                    }

                    //while (ex != null)
                    //{
                    //    table.Child(new HtmlElement("tr")
                    //            .Child(new HtmlElement("td", CreateAttrib(tdColor, 3)).Text(ex.Message))
                    //           );
                    //    ex = ex.InnerException;
                    //}
                }

#if !WindowsCE

                if (ex.Data != null && ex.Data.Count() > 1)
                {

                    table.Child(new HtmlElement("tr").Child(
                                                new HtmlElement("th", CreateAttrib(thColor, 1)).Text("Data"),
                                                new HtmlElement("th", CreateAttrib(thColor, 1)).Text("Type"),
                                                new HtmlElement("th", CreateAttrib(thColor, 1)).Text("Valeur"))
                                                );

                       foreach (var fo in ex.Data)
                        {
                          

                            table.Child(new HtmlElement("tr").Child(
                                                 new HtmlElement("td", CreateAttrib(tdColor, 1)).Text(fo.Name),
                                                 new HtmlElement("td", CreateAttrib(tdColor, 1)).Text(fo.Type),
                                                 new HtmlElement("td", CreateAttrib(tdColor, 1)).Text(fo.ValueToHtml()))
                                                                  );
                        }

                }
#endif

                    if (!string.IsNullOrEmpty(ex.StackTrace))
                {

                    table.Child(new HtmlElement("tr")
                                                  .Child(new HtmlElement("th", CreateAttrib(thColor, 3)).Text("Call Stack"))
                                                  );

                    table.Child(new HtmlElement("tr")
                                                   .Child(new HtmlElement("td", CreateAttrib(tdColor, 3)).Text(ex.StackTrace.Replace("\n", "<br/>")))
                                                  );

                }

                if (Bitmap != null  )
                {

//#if !WindowsCE
//                    if (bitmap.Width > 1000)
//#if NET35
//                        bitmap = bitmap.ResizeImage((float)0.60);
//#else
//                    bitmap =  BitmapFunction.ResizeImage(bitmap,(float)0.60);
//#endif
//#endif

                    var base64Data = ScreenShot.GetBase64(Bitmap, ImageFormat.Jpeg);

                    table.Child(new HtmlElement("tr")
                                                .Child(new HtmlElement("th", CreateAttrib(thColor, 3)).Text("ScreenShot"))
                                                );

                    table.Child(new HtmlElement("tr")
                       .Child(new HtmlElement("td", new HtmlAttribute("align", "center"),
                    new HtmlAttribute("colspan", "3")).Text("<img alt=\"sample\" src=\"data:image/jpg;base64," + base64Data + "\"/>"))
                       );
                 
                }

                cache = html.ToString();
            }
            finally
            {
                mut.ReleaseMutex();
            }

            return cache;
        }
    }
}
