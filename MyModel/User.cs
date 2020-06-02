using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace Foodies.MyModel
{
    public class User
    {
        public int user_id { get; set; }

        [Required(ErrorMessage ="Name is required")]
        //[RegularExpression(@"^(([A-za-z]+[\s]{1}[A-za-z]+)|([A-Za-z]+))$", ErrorMessage = "Name contains only alphabet")]
        public string user_name { get; set; }

        [Required(ErrorMessage ="Email is requied")]
        [EmailAddress(ErrorMessage = "Invalid Email")]
        public string user_email { get; set; }
        public string user_phone { get; set; }

        [Required(ErrorMessage ="Password is required")]
        public string user_password { get; set; }

        [Required(ErrorMessage ="Please select a button")]
        public int? Role_Id { get; set; }
        public bool? IsValid { get; set; }

        public UserRole userRole { get; set; }
    }
    [MetadataType(typeof(User))]

    public partial class User_tbl
    {

    }


}