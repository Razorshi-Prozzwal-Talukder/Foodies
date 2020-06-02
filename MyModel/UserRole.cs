using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace Foodies.MyModel
{
    public class UserRole
    {

        public int Role_Id { get; set; }
        public string role_name { get; set; }
    }


    [MetadataType(typeof(UserRole))]

    public partial class User_Role
    {
    }
}