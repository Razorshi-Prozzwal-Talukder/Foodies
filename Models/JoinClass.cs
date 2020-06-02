using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Foodies.Models
{
    public class JoinClass
    {
       public tbl_platter platterdetail { get; set; }
       public tbl_restaurent restaurentdetail {get; set;}
       public Rating_tbl ratingdetail { get; set; }

      public List<Rating_tbl> ratingdetails { get; set; }
      public List<tbl_platter> platterdetails { get; set; }
      public List<tbl_restaurent> restaurentdetails { get; set; }

    }
}