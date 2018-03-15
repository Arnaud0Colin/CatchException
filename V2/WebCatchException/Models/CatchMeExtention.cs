using CatchException;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WebCatchException.Models
{

    internal sealed class CatchMe_ExceptionMetadata
    {
        [DisplayName("Etat")]
        [Required]
         public int CodeStatus { get; set; }
    }


    //[DisplayName("Commentaire")]
    //[StringLength(3000)]
    //public string Commentaire { get; set; }

    [MetadataType(typeof(CatchMe_ExceptionMetadata))]
    public partial class CatchMe_Exception
    {

        //[DisplayName("Etat")]
        //[Required]
        //public byte? SaisieCodeStatus { get; set; }


        public static explicit operator Smtp.MessageSmtp(CatchMe_Exception ex)
        {
            if (ex == null)
                return null;

            var nc = Smtp.NotificationSmtp.MessageSmtp;

            string Exception = ex.GetExceptionType;

            nc.Subject = $"{Exception} - {ex.ProcessName}";

            nc.From = $"{ex.Program}@Exception.Test.zz";


            HtmlElement Message = new HtmlElement();

            Message.Child(new HtmlElement("div").Text(ex.GetMessage));


            if (ex.CodeCatch > 0)
            {
                var url = new System.Web.Mvc.UrlHelper(HttpContext.Current.Request.RequestContext);
                Message.Child(new HtmlElement("div").Child(new HtmlElement("a", new HtmlAttribute("href", url.Action("Nouvelle", "CatchMe", new { id = ex.CodeCatch }, HttpContext.Current.Request.Url.Scheme))).Text(Exception)));
            }
            else
            {
                Message.Child(new HtmlElement("div").Text($" Erreur d'ecriture dans la base SQL Voir le fichier Dump App_Data/ "));
            }



            HtmlElement html = new HtmlElement();
            html.Child(new HtmlElement() { Txt = "<META CHARSET=\"UTF-8\" />" });
            html.Child(new HtmlElement("font", new HtmlAttribute("size", "1")));
            html.Child(new HtmlElement("style").Text("div.MsoMessage { font-size:12.0pt;font-family:\"Garamond\",\"serif\"; } span.MsoNormal, p.MsoNormal, li.MsoNormal, div.MsoNormal {margin:0cm;	margin-bottom:.0001pt;	font-size:12.0pt; font-family:\"Garamond\",\"serif\"; color:#0094ff;   }"));
            var table = new HtmlElement("div");
            table.Child(new HtmlElement("div", new HtmlAttribute("class", "MsoMessage")).Text(Message.ToString()));

            

            html.Child(table);
            table.Child(
                new HtmlElement("p", new HtmlAttribute("class", "MsoNormal")).Child(
                new HtmlElement("span", new HtmlAttribute("class", "MsoNormal"))
                    .Text("Service Informatique")));

            table.Child(
                new HtmlElement("p", new HtmlAttribute("class", "MsoNormal")).Child(
                new HtmlElement("span", new HtmlAttribute("class", "MsoNormal"))
                    .Text("CatchException V2")));

            nc.Body = html.ToString();
     
            return nc;
        }





        public DateTime? GetBuildTime
        {
            get
            {
                if (!string.IsNullOrWhiteSpace(this.Version))
                {
                    var Ver = new Version(this.Version);

                    if (Ver.Build > 1000 && Ver.Revision > 1000)
                    {

                        return new DateTime(2000, 1, 1).Add(new TimeSpan(
                              TimeSpan.TicksPerDay * Ver.Build + // days since 1 January 2000
                              TimeSpan.TicksPerSecond * 2 * Ver.Revision)); // seconds since midnight, (multiply by 2 to get original)
                    }
                }

                return null;
            }
        }


        public string eGetrr => this.Debug ?  $@"<img src = '{HttpContext.Current.Request.Url.Scheme}://{HttpContext.Current.Request.Url.Authority + "/Images/glyphicons-361-bug.png"}' />" : null;


        public string Getrrz => $@" <g style='fill:{tt(GetColor)}' id='gb' transform='matrix(0.5,0,0,0.5,116.14271,108.39986)'>
                <path d='M451.383,247.54c-3.606-3.617-7.898-5.427-12.847-5.427h-63.953v-83.939l49.396-49.394c3.614-3.615,5.428-7.898,5.428-12.85c0-4.947-1.813-9.229-5.428-12.847c-3.614-3.616-7.898-5.424-12.847-5.424s-9.233,1.809-12.847,5.424l-49.396,49.394H107.923L58.529,83.083c-3.617-3.616-7.898-5.424-12.847-5.424c-4.952,0-9.233,1.809-12.85,5.424c-3.617,3.617-5.424,7.9-5.424,12.847c0,4.952,1.807,9.235,5.424,12.85l49.394,49.394v83.939H18.273c-4.949,0-9.231,1.81-12.847,5.427C1.809,251.154,0,255.442,0,260.387c0,4.949,1.809,9.237,5.426,12.848c3.616,3.617,7.898,5.431,12.847,5.431h63.953c0,30.447,5.522,56.53,16.56,78.224l-57.67,64.809c-3.237,3.81-4.712,8.234-4.425,13.275c0.284,5.037,2.235,9.273,5.852,12.703c3.617,3.045,7.707,4.571,12.275,4.571c5.33,0,9.897-1.991,13.706-5.995l52.246-59.102l4.285,4.004c2.664,2.479,6.801,5.564,12.419,9.274c5.617,3.71,11.897,7.423,18.842,11.143c6.95,3.71,15.23,6.852,24.84,9.418c9.614,2.573,19.273,3.86,28.98,3.86V169.034h36.547V424.85c9.134,0,18.363-1.239,27.688-3.717c9.328-2.471,17.135-5.232,23.418-8.278c6.275-3.049,12.47-6.519,18.555-10.42c6.092-3.901,10.089-6.612,11.991-8.138c1.909-1.526,3.333-2.762,4.284-3.71l56.534,56.243c3.433,3.617,7.707,5.424,12.847,5.424c5.141,0,9.422-1.807,12.854-5.424c3.607-3.617,5.421-7.902,5.421-12.851s-1.813-9.232-5.421-12.847l-59.388-59.669c12.755-22.651,19.13-50.251,19.13-82.796h63.953c4.949,0,9.236-1.81,12.847-5.427c3.614-3.614,5.432-7.898,5.432-12.847C456.828,255.445,455.011,251.158,451.383,247.54z' />
                <path d = 'M293.081,31.27c-17.795-17.795-39.352-26.696-64.667-26.696c-25.319,0-46.87,8.901-64.668,26.696c-17.795,17.797-26.691,39.353-26.691,64.667h182.716C319.771,70.627,310.876,49.067,293.081,31.27z' />
            </g>";

        public string Getrr =>

               $@"<svg width='30px' height='30px' viewBox='0 0 456.828 456.828' xmlns='http://www.w3.org/2000/svg'>
            <g style='fill:{GetColor}' id='gc'>
                <circle color='red' r='230' cx='228' cy='230'></circle>
            </g>
           {(this.Debug ? Getrrz: null )}

        </svg>";


        public string tt(string color)
        {
            var col = System.Drawing.ColorTranslator.FromHtml(color);
          var fff = new HSLColor(col);
            if( fff.luminosity <= 0.5)
            {
                return "#fff";
            }
            else
            {
                return "#000";
            }

        }

        public string GetColor
        {
            get
            {
                switch( this.UrgenceLevel)
                {
                    case -1:
                        return "#e3e3e3";
                    case 1:
                    case 0:
                        return "#cc0000";
                    case 2:
                        return "#00CC00";
                    case 3:
                    case 4:
                    case 5:
                    case 6:
                    case 7:
                    case 8:
                        return "#0000FF";
                    default:
                        return "#e3e3e3";
                }
            }
        }


        public string GetStatusColor
        {
            get
            {
                switch (this.CodeStatus)
                {
                    case 0:
                        return "progress-bar-danger";
                    case 1:
                        return "progress-bar-warning";
                    case 2:
                        return "progress-bar-success";
                    case 3:
                    case 4:
                    case 5:
                    case 6:
                    case 7:
                    case 8:
                        return "progress-bar-info";
                    default:
                        return "";
                }
            }
        }

        public IEnumerable<int> GetNextStatus
        {
            get
            {
                if (this.CatchMe_Status != null)
                    return this.CatchMe_Status?.GetNextStatus;
                else
                    return new List<int>();
            }
        }

        public SelectList ChargeStatus
        {
            get
            {
#pragma warning disable 652, 472
                Models.Entities sql = new Models.Entities();
                if (this.GetNextStatus != null)
                    return new SelectList(sql.CatchMe_Status.Where(r => this.GetNextStatus.Contains(r.CodeStatus) && r.Order >= 0).OrderBy(r => r.Order), "CodeStatus", "Libelle");
                else
                    return new SelectList(sql.CatchMe_Status.Where(r => r.CodeStatus == -1000), "CodeStatus", "Libelle");
#pragma warning disable 652,472
            }
        }


        public string GetSpamString => ConvertString.Format((DateTime.Now - this.Date), "d h m s");

        public string GetExceptionType => this.CatchMe_Exception_Detail?.FirstOrDefault()?.Exception;
        public string GetMessage => this.CatchMe_Exception_Detail?.FirstOrDefault()?.Message;

        public string GetLibelleStatus => this.CatchMe_Status?.Libelle;


    }


    public partial class CatchMe_Status
    {
        public IEnumerable<int> GetNextStatus
        {
            get
            {
                if (this.Suivant == null)
                    yield break;

                foreach (var s in this.Suivant.Split(','))
                {
                    int val = -1;
                    if (int.TryParse(s, out val))
                    {
                        yield return val;
                    }
                }
            }
            set
            {
                string ret = string.Empty;
                foreach (var d in value)
                {
                    if (!string.IsNullOrWhiteSpace(ret))
                        ret += ',';

                    ret += d.ToString();
                }
                this.Suivant = ret;
            }

        }

    }


    public partial class CatchMe_Exception_Detail
    {
        public string GetStackTrace => this.StackTrace?.Replace("\n", "<br/>");
        public string GetTargetSite => this.TargetSite?.Replace("\n", "<br/>");

    }

    
   public partial class CatchMe_Exception_Variable
    {

        public string GetValue =>
            this.Value.Replace("\t", "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;").Replace("\n", "<br/>");


    }

    public partial class CatchMe_Exception_Screen
    {

        public string GetScreen => Convert.ToBase64String(this.Screen);   

    }
}