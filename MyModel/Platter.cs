using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using Foodies.Models;

namespace Foodies.MyModel
{
    public class Platter
    {
        public int platter_id { get; set; }

        [Required(ErrorMessage ="Platter name is required")]
        public string platter_name { get; set; }


        [Required(ErrorMessage ="Platter Description is required")]
        public string platter_description { get; set; }

        [Required(ErrorMessage = "Platter Image is required")]
        public byte[] platter_image { get; set; }


        public decimal? platter_price { get; set; }

        
        public int? Rest_id { get; set; }

        [Required(ErrorMessage = "Please enter your code")]
        public int? secret_code { get; set; }


        public Restaurent restaurent { get; set; }



    }
    [MetadataType(typeof(Platter))]
    public partial class tbl_platter
    {


    }
}