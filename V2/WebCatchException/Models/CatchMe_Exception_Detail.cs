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
    
    public partial class CatchMe_Exception_Detail
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public CatchMe_Exception_Detail()
        {
            this.CatchMe_Exception_Detail_Data = new HashSet<CatchMe_Exception_Detail_Data>();
        }
    
        public long CodeCatch { get; set; }
        public long Code { get; set; }
        public string Message { get; set; }
        public string Source { get; set; }
        public string StackTrace { get; set; }
        public string HelpLink { get; set; }
        public string TargetSite { get; set; }
        public Nullable<int> HResult { get; set; }
        public string Exception { get; set; }
    
        public virtual CatchMe_Exception CatchMe_Exception { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<CatchMe_Exception_Detail_Data> CatchMe_Exception_Detail_Data { get; set; }
    }
}
