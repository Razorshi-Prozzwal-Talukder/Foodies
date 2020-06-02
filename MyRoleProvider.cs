using Foodies.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;

namespace Foodies
{
    public class MyRoleProvider : RoleProvider
    {



        public override string ApplicationName { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public override void AddUsersToRoles(string[] usernames, string[] roleNames)
        {
            throw new NotImplementedException();
        }

        public override void CreateRole(string roleName)
        {
            throw new NotImplementedException();
        }

        public override bool DeleteRole(string roleName, bool throwOnPopulatedRole)
        {
            throw new NotImplementedException();
        }

        public override string[] FindUsersInRole(string roleName, string usernameToMatch)
        {
            throw new NotImplementedException();
        }

        public override string[] GetAllRoles()
        {
            throw new NotImplementedException();
        }


        //Need
        public override string[] GetRolesForUser(string username)
        {
            using (FoodieEntities2 db = new FoodieEntities2())
            {
                var result = (from user in db.User_tbl
                              join role in db.User_Role on user.Role_Id equals role.Role_Id
                              where user.user_name == username
                              select role.role_name).ToArray();

                return result;



                //string[] role = {db.User_tbl.Where(x => x.user_name == username).
                  // FirstOrDefault().User_Role.role_name };
            }   
           
        }

        public override string[] GetUsersInRole(string roleName)
        {
            throw new NotImplementedException();
        }

        public override bool IsUserInRole(string username, string roleName)
        {
            throw new NotImplementedException();
        }


       

        public override void RemoveUsersFromRoles(string[] usernames, string[] roleNames)
        {
            throw new NotImplementedException();
        }

        public override bool RoleExists(string roleName)
        {
            throw new NotImplementedException();
        }
    }
}