//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace MuzikosSistema.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class ListenerProfile
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public ListenerProfile()
        {
            this.ListenerStyles = new HashSet<ListenerStyles>();
        }
    
        public int Id { get; set; }
        public string Username { get; set; }
        public string Nationality { get; set; }
        public Nullable<System.DateTime> BirthDate { get; set; }
        public string Email { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ListenerStyles> ListenerStyles { get; set; }
    }
}
