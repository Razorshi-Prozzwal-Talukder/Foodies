//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Foodies.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class User_tbl
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public User_tbl()
        {
            this.tbl_restaurent = new HashSet<tbl_restaurent>();
        }
    
        public int user_id { get; set; }
        public string user_name { get; set; }
        public string user_email { get; set; }
        public string user_password { get; set; }
        public Nullable<int> Role_Id { get; set; }
        public Nullable<bool> IsValid { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<tbl_restaurent> tbl_restaurent { get; set; }
        public virtual User_Role User_Role { get; set; }
    }
}