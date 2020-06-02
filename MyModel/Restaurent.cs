using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace Foodies.MyModel
{
    public class Restaurent
    {

        public int Rest_id { get; set; }



        [Required(ErrorMessage ="Your Restautant Name is required")]
        public string Rest_name { get; set; }

        [Required(ErrorMessage ="Enter your restaurant location")]
        public string Rest_location { get; set; }

        [Required]
        [DataType(DataType.PhoneNumber)]
        [RegularExpression(@"^(?:\+? 88 | 0088)?01[15-9]\d{8}$", ErrorMessage = "Enter a valid phone number")]
        public string Rest_phone { get; set; }


        [Required(ErrorMessage = "Enter Code")]
        [StringLength(4, MinimumLength = 1, ErrorMessage = "Code length should be 1 to 4 length digit")]
        public int? secret_code { get; set; }
        public int? User_id { get; set; }

        public User_tbl user_Tbl { get; set; }

    }


    [MetadataType(typeof(Restaurent))]

    public partial class tbl_restaurent
    {
    }
}