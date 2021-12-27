using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace artiktamam.Models
{
    public class DatabaseViews
    {
        public virtual ICollection<Car_Tablo> Car_Tablo { get; set; }
    }
}