//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace artiktamam.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class UserRoleMapping
    {
        public int Id { get; set; }
        public Nullable<int> UserId { get; set; }
        public Nullable<int> RoleId { get; set; }
    
        public virtual Roles_Tablo Roles_Tablo { get; set; }
        public virtual Users_Tablo Users_Tablo { get; set; }
        public virtual Users_Tablo Users_Tablo1 { get; set; }
    }
}
