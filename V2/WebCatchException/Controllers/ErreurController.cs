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
using WebCatchException.Models;

namespace WebCatchException.Controllers
{
    public class ErreurController : Controller
    {

        [OutputCache(Duration = 10, VaryByParam = "none", Location = OutputCacheLocation.Client, NoStore = true)]
        public ActionResult Index(int statusCode, Exception exception, bool isAjaxRequet)
        {
            Response.StatusCode = statusCode;

            ViewBag.ReturnUrl = Request.UrlReferrer;

            // If it's not an AJAX request that triggered this action then just retun the view
            if (!isAjaxRequet)
            {
                ErreurModel model = new ErreurModel { HttpStatusCode = statusCode, Exception = exception };  
                return View(model);
            }
            else
            {
                // Otherwise, if it was an AJAX request, return an anon type with the message from the exception
                var errorObjet = new { message = exception.Message };
                return Json(errorObjet, JsonRequestBehavior.AllowGet);
            }
        }

        [OutputCache(Duration = 10, VaryByParam = "id", Location = OutputCacheLocation.Client, NoStore = true)]
        public ActionResult Code(int? id)
        {

            int statusCode = id.HasValue ? id.Value : 500;

            ViewBag.ReturnUrl = Request.UrlReferrer;

            ErreurModel model = new ErreurModel { HttpStatusCode = statusCode };
            return View(model);
        }

    }
}