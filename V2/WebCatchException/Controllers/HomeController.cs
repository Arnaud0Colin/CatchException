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
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.UI;

namespace WebCatchException.Controllers
{
 //   [MyAuthorize]
    public class HomeController : Controller
    {

        public ActionResult Index()
        {
            return View();
        }


        //sql.CatchMe_Exception.Where(x => x.Supprimer == false && x.Masquer == false).GroupBy(x => x.ComputerName).Select(x => new { ComputerName = x.Key, Count = x.Count() }).OrderByDescending(x => x.Count).Take(5);
        //sql.CatchMe_Exception.Where(x => x.Supprimer == false && x.Masquer == false).GroupBy(x => x.Program).Select(x => new { Program = x.Key, Count = x.Count() }).OrderByDescending(x => x.Count).Take(5);
        //sql.CatchMe_Exception.Where(x => x.Supprimer == false && x.Masquer == false).GroupBy(x => x.Login).Select(x => new { Login = x.Key, Count = x.Count() }).OrderByDescending(x => x.Count).Take(5);
        //sql.CatchMe_Exception.Where(x => x.Supprimer == false && x.Masquer == false).GroupBy( x=> x.CatchMe_Exception_Detail. )


        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}