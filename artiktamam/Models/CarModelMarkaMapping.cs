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
    
    public partial class CarModelMarkaMapping
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public CarModelMarkaMapping()
        {
            this.Car_Tablo = new HashSet<Car_Tablo>();
        }
    
        public int CarKod { get; set; }
        public Nullable<int> CarModel { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Car_Tablo> Car_Tablo { get; set; }
        public virtual Model_Table Model_Table { get; set; }
    }
}