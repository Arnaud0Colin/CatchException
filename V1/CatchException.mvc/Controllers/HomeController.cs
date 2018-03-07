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

using CatchException.mvc.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CatchException.mvc.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            CatchExceptionEntities db = new CatchExceptionEntities();
            return View(db.Journals);
        }

        public ActionResult Decode()
        {
            
            return View(new DecodeModel());
        }

        [HttpPost]
        public ActionResult Decode(DecodeModel form)
        {
            form.Decode = LogTrace.Zip.Base64ToUncompress(form.Code);
           return View(form);
        }
        
     
    }
}