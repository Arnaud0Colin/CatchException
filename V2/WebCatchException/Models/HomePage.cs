using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WebCatchException.Models
{
    public class HomePage
    {

        public IQueryable<CatchMe_Exception> ListEx { get; set; }

    }

    public class StatPage
    {


        public IQueryable<CatchMe_Exception> ListEx { get; set; }

    }


    public class wPage
    {

        public IQueryable<CatchMe_Exception> ListEx { get; set; }

    }

    public class ErrorFilePage
    {
        public string Fichier { get; set; }
       // public MyException MyEx { get; set; }

        public CatchMe_Exception MyEx { get; set; }


        [DisplayName("Lire un fichier sur le serveur ou uploader un fichier [*.erDump]")]
        public IEnumerable<HttpPostedFileBase> Fichiers { get; set; }
        

        public SelectList ChargeFichier
        {
            get
            {

               return new SelectList( System.IO.Directory.GetFiles(HttpContext.Current.Server.MapPath("~/App_Data"), "*.ERDUMP",
                                 System.IO.SearchOption.AllDirectories).Select(x=> new { Fichier = System.IO.Path.GetFileNameWithoutExtension(x) , Titre = System.IO.Path.GetFileNameWithoutExtension(x) } ), "Fichier", "Titre");

            }
        }


    }


}