//------------------------------------------------------------------------------
// <auto-generated>
//     Ce code a été généré à partir d'un modèle.
//
//     Des modifications manuelles apportées à ce fichier peuvent conduire à un comportement inattendu de votre application.
//     Les modifications manuelles apportées à ce fichier sont remplacées si le code est régénéré.
// </auto-generated>
//------------------------------------------------------------------------------

namespace WebCatchException.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class CatchMe_Exception_Detail_Data
    {
        public long CodeCatch { get; set; }
        public long Code { get; set; }
        public string Name { get; set; }
        public string Type { get; set; }
        public string Value { get; set; }
    
        public virtual CatchMe_Exception CatchMe_Exception { get; set; }
        public virtual CatchMe_Exception_Detail CatchMe_Exception_Detail { get; set; }
    }
}
