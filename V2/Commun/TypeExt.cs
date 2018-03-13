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
#if NET35
using System.Linq;
#endif
using System.Text;




namespace CatchException
{
   internal static class TypeExt
    {
      //private  class HResultException : Exception
      //  {
       
      //      internal int GetHResult
      //      {
      //          get
      //          {
      //              return this.HResult;
      //          }
      //      }
      //  }


     


        //internal static int GetHResult(this Exception ex)
        //{
        //    if (typeof(HResultException).IsAssignableFrom(typeof(Exception)))

        //        return ((HResultException)(ex as object)).GetHResult;
        //   // return ((HResultException)ex).GetHResult;
        //    else
        //        return 0;

        //    //return (int)ex.GetType().GetField("HResult").GetValue(ex);

        //   // return ex.HResult;
        //  //  
        //}


        internal static string ValueToHtml(this WebCatchException.ElVar variable)
            => System.Web.HttpUtility.HtmlEncode(variable.Value).Replace("\t", "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;").Replace("\n", "<br/>");


      
    }
}
