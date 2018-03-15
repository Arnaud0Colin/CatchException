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
   // [Authorize(Roles = @"GG_SI_Dev")]
    public class CatchMeController : Controller
    {

        Models.Entities sql = new Models.Entities();

        private string GetPreviousUrl() =>
              (System.Web.HttpContext.Current.Request.UrlReferrer != null &&
              System.Web.HttpContext.Current.Request.Url != System.Web.HttpContext.Current.Request.UrlReferrer)
              ? System.Web.HttpContext.Current.Request.UrlReferrer.ToString() : Url.Action("Index");


        // Post: CatchMe/GetStatus
        [HttpPost]
        public JsonResult GetStatus()
        {
            var allItems = sql.CatchMe_Status.Select(x => new { Text = x.Libelle, Value = x.CodeStatus });
            return Json(new { Items = allItems });
        }


        // Post: CatchMe/GetComputerName
        [HttpPost]
        public JsonResult GetComputerName()
        {

            var allItems = sql.CatchMe_Exception.Select(x => x.ComputerName).Distinct().Select(x => new { Text = x, Value = x });
            return Json(new { Items = allItems });
        }

        // Post: CatchMe/GetProgram
        [HttpPost]
        public JsonResult GetProgram()
        {

            var allItems = sql.CatchMe_Exception.Select(x => x.Program).Distinct().Select(x => new { Text = x, Value = x });
            return Json(new { Items = allItems });
        }

        // Post: CatchMe/GetExceptionType
        [HttpPost]
        public JsonResult GetExceptionType()
        {

            var allItems = sql.CatchMe_Exception_Detail.Select(x => x.Exception).Distinct().Select(x => new { Text = x, Value = x });
            return Json(new { Items = allItems });
        }


        // GET : CatchMe/
        public ActionResult Index()
        {
            var hp = new Models.HomePage();
            hp.ListEx = sql.CatchMe_Exception.Where(x => x.Masquer == false).OrderByDescending(x => x.Date);
            return View(hp);
        }

        public ActionResult Archive()
        {
            var hp = new Models.HomePage();
            hp.ListEx = sql.CatchMe_Exception.Where(x => x.CodeStatus < 90).OrderByDescending(x => x.Date);
            return View(hp);
        }


        // GET : CatchMe/Voir/32
        public ActionResult Erreur(long? id)
        {

            ViewBag.PreviousUrl = GetPreviousUrl();
            TempData["PreviousUrl"] = GetPreviousUrl();

            if (!id.HasValue)
                return RedirectToAction("Index");

            var ex = sql.CatchMe_Exception.Where(x => x.CodeCatch == id).FirstOrDefault();
            if (ex == null /*|| ex.Supprimer*/)
                return RedirectToAction("Index");


            ViewBag.Next = sql.CatchMe_Exception.Where(x => x.CodeCatch > id /*&& x.Masquer == false*/).Select(x => x.CodeCatch).FirstOrDefault();
            ViewBag.Previous = sql.CatchMe_Exception.Where(x => x.CodeCatch < id /*&& x.Masquer == false*/).OrderByDescending(x => x.CodeCatch).Select(x => x.CodeCatch).FirstOrDefault();
            return View("Erreur", ex);
        }

        // POST : CatchMe/Voir/32
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Erreur(long? id, Models.CatchMe_Exception row)
        {

            ViewBag.PreviousUrl = TempData["PreviousUrl"] != null ? TempData["PreviousUrl"] : GetPreviousUrl();

            if (!id.HasValue)
                return RedirectToAction("Index");

            var ex = sql.CatchMe_Exception.Where(x => x.CodeCatch == id).FirstOrDefault();

            if (ex != null)
            {
                if (ModelState.IsValid)
                {

                    ex.CodeStatus = row.CodeStatus;
                    ex.DateStatus = DateTime.Now;
                    ex.LoginStatus = User?.Identity?.Name;

                    //try
                    //{
                    //   ex.Masquer = true;
                    sql.SaveChanges();
                    //}
                    //catch (Exception rrr)
                    //{
                    //}


                    var Next = sql.CatchMe_Exception.Where(x => x.CodeCatch > id && x.Masquer == false).Select(x => x.CodeCatch).FirstOrDefault();
                    if (Next > 0)
                        return RedirectToAction(RouteData.Values["action"].ToString(), new { id = Next });
                    else
                        return Redirect(ViewBag.PreviousUrl);
                }
                else
                {
                    return View(RouteData.Values["action"].ToString(), ex);
                }
            }


            return View("Erreur", row);
        }


        // GET : CatchMe/Voir/32
        public ActionResult Nouvelle(long? id)
        {

            ViewBag.PreviousUrl = GetPreviousUrl();
            TempData["PreviousUrl"] = GetPreviousUrl();

            if (!id.HasValue)
                return RedirectToAction("Index");

            var ex = sql.CatchMe_Exception.Where(x => x.CodeCatch == id).FirstOrDefault();
            if (ex == null /*|| ex.Supprimer*/)
                return RedirectToAction("Index");


           ViewBag.Next = sql.CatchMe_Exception.Where(x => x.CodeCatch > id && x.Masquer == false).Select(x=> x.CodeCatch).FirstOrDefault();
           ViewBag.Previous = sql.CatchMe_Exception.Where(x => x.CodeCatch < id && x.Masquer == false).OrderByDescending(x => x.CodeCatch).Select(x => x.CodeCatch).FirstOrDefault();
            return View("Erreur", ex);
        }


        // POST : CatchMe/Voir/32
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Nouvelle(long? id, Models.CatchMe_Exception row)
        {       

                ViewBag.PreviousUrl = TempData["PreviousUrl"] != null ? TempData["PreviousUrl"] : GetPreviousUrl();

                if (!id.HasValue)
                    return RedirectToAction("Index");

                var ex = sql.CatchMe_Exception.Where(x => x.CodeCatch == id).FirstOrDefault();

                if (ex != null)
                {
                    if (ModelState.IsValid)
                    {

                        ex.CodeStatus = row.CodeStatus;
                        ex.DateStatus = DateTime.Now;
                        ex.LoginStatus = User?.Identity?.Name;

                    //try
                    //{
                        //   ex.Masquer = true;
                        sql.SaveChanges();
                    //}
                    //catch (Exception rrr)
                    //{
                    //}


                        var Next = sql.CatchMe_Exception.Where(x => x.CodeCatch > id && x.Masquer == false).Select(x => x.CodeCatch).FirstOrDefault();
                        if (Next > 0)
                            return RedirectToAction(RouteData.Values["action"].ToString(), new { id = Next });
                        else
                            return Redirect(ViewBag.PreviousUrl);
                    }
                    else
                    {
                        return View(RouteData.Values["action"].ToString(), ex);
                    }
                }
            

            return View("Erreur", row);
        }

        // GET : CatchMe/ErrorFile
        public ActionResult ErrorFile()
        {
            ViewBag.PreviousUrl = GetPreviousUrl();
            TempData["PreviousUrl"] = GetPreviousUrl();

            return View(new Models.ErrorFilePage());
        }


        // POST : CatchMe/ErrorFile
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ErrorFile(Models.ErrorFilePage row)
        {
            ViewBag.PreviousUrl = TempData["PreviousUrl"] != null ? TempData["PreviousUrl"] : GetPreviousUrl();

            bool Supprimer = false;
            bool Importer = false;


            if (Request.Form["Action"] != null)
            {
                if(Request.Form["Action"].IndexOf("Imp", StringComparison.OrdinalIgnoreCase) >= 0)
                {
                    Importer = true;
                }
                else if (Request.Form["Action"].IndexOf("Sup", StringComparison.OrdinalIgnoreCase) >= 0)
                {
                    Supprimer = true;
                }
            }



            System.Web.Script.Serialization.JavaScriptSerializer scriptSerializer = new System.Web.Script.Serialization.JavaScriptSerializer();
            var Fichier =  row.Fichiers.FirstOrDefault();

            if(Fichier != null)
            {
                byte[] input = null;
                int FileLen = Fichier.ContentLength;
                System.IO.Stream MyStream;
                input = new byte[FileLen];

                MyStream = Fichier.InputStream;
                MyStream.Read(input, 0, FileLen);

                row.MyEx = scriptSerializer.Deserialize<MyException>(System.Text.Encoding.UTF8.GetString(input)).EEE();

            }
            else if(!string.IsNullOrWhiteSpace( row.Fichier))
            {
                var filename = System.IO.Path.Combine(Server.MapPath("~/App_Data"), row.Fichier + ".ERDUMP");

                if(Supprimer)
                {
                    System.IO.File.Delete(filename);
                    row.MyEx = null;
                }
                else
                {
                    row.MyEx = scriptSerializer.Deserialize<MyException>(System.IO.File.ReadAllText(filename)).EEE();
                }                
            }
            else
            {
                row.MyEx = null;
            }

            //  Except = exception.EEE();

            if(Importer && row.MyEx != null)
            {
                try
                {
                    //throw new System.Data.Entity.Validation.DbEntityValidationException();

                    var sql = new Models.Entities();
                    sql.CatchMe_Exception.Add(row.MyEx);
                    sql.SaveChanges();
                }
                catch(System.Data.Entity.Validation.DbEntityValidationException e)
                {
                    ViewBag.ErrorMessage = CatchException.Dump(e).ToString().Replace("\n", "<br/>");
                    return View(row);
                }

                if(!string.IsNullOrWhiteSpace(row.Fichier))
                {
                    var filename = System.IO.Path.Combine(Server.MapPath("~/App_Data"), row.Fichier + ".ERDUMP");
                    System.IO.File.Delete(filename);
                }

                return RedirectToAction("Erreur", new { id = row.MyEx.CodeCatch });
            }

            

            return View(row);
        }



        // GET : CatchMe/Screen/13C51516-BF47-E411-80C2-0050568B0648
        [OutputCache(Duration = 3600, VaryByParam = "id", Location = OutputCacheLocation.Any, NoStore = true)]
        public FileResult Screen(string id)
        {
            if (!string.IsNullOrEmpty(id))
            {
                try
                {
                    var fileId = Guid.Parse(id);


                    var myFile = sql.CatchMe_Exception_Screen.Where(x => x.FileKey == fileId).FirstOrDefault();

                    if (myFile != null)
                    {
                        byte[] fileBytes = myFile.Screen;
                        return File(fileBytes, @"image/jpeg", myFile.FileKey + ".jpg");
                    }
                }
                catch
                {
                }
            }

            return null;
        }

    }
}