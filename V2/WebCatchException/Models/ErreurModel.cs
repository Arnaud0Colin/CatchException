using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebCatchException.Models
{
    public class ErreurModel
    {
        public int HttpStatusCode { get; set; }

        public Exception Exception { get; set; }
    }
}