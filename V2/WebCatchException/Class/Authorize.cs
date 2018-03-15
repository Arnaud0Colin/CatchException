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

namespace WebCatchException
{
    public class MyAuthorizeAttribute : AuthorizeAttribute
    {
        /*  protected override void HandleUnauthorizedRequest(AuthorizationContext filterContext)
          {
              throw new NotAuthorizedHttpException(Roles);
          }*/
    }

    public class NotAuthorizedHttpException : HttpException
    {
        public NotAuthorizedHttpException(string missingRoles)
            : base(401, string.Format("You do not have the required role(s) '{0}'.", string.Join(", ", missingRoles)))
        {
        }
    }
}