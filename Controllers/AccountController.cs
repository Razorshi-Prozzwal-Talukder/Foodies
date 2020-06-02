using Foodies.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Web;
using System.Web.Hosting;
using System.Web.Mvc;
using System.Web.Security;


namespace Foodies.Controllers
{

    public class AccountController : Controller
    {
        // GET: Account
      

        //Log in
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]

        public ActionResult Login(User_tbl model)
        {
            using (var context = new FoodieEntities2())
            {
               
                bool isValid = context.User_tbl.Any(x => x.user_email == model.user_email && x.user_name == model.user_name &&
                x.user_password == model.user_password);

                var active =( from x in context.User_tbl
                              where (x.user_email == model.user_email && x.user_name == model.user_name)
                              select x.IsValid).Any();

                if (isValid)
                {

                    if(active == true)
                    {

                        FormsAuthentication.SetAuthCookie(model.user_name, true);
                        var role = (from x in context.User_tbl
                                    where (x.user_email == model.user_email && x.user_name == model.user_name)
                                    select x.Role_Id).FirstOrDefault();
                        Session["manager"] = null;
                        Session["admin"] = null;
                        Session["Rest_id"] = null;
                        Session["role"] = role;
                        if (role == 1)
                        {
                            Session["admin"] = "Admin";
                        }

                        model.user_id = (from x in context.User_tbl
                                         where (x.user_email == model.user_email
                                         && x.user_name == model.user_name)
                                         select x.user_id).FirstOrDefault();

                        Session["User_id"] = model.user_id;
                        Session["User_name"] = model.user_name;

                        if (role == 2)
                        {
                            Session["manager"] = "manager";
                            var restId = (from r in context.tbl_restaurent
                                          where (r.User_id == model.user_id)
                                          select r.Rest_id).FirstOrDefault();

                            if (context.tbl_restaurent.Any(x => x.User_id == model.user_id))
                            {
                                Session["Rest_id"] = restId;

                                if (context.tbl_platter.Any(x => x.Rest_id == restId))
                                {
                                    var Code = context.tbl_platter.Where(x => x.Rest_id == restId).Select(x => x.secret_code).FirstOrDefault();
                                    Session["scode"] = Code;
                                   
                                    return RedirectToAction("Index", "Platter", new { code = Code });
                                }
                                else
                                {
                                    //Session["Rest_id"] = restId;
                                    return RedirectToAction("Addplatter", "Platter");
                                }


                            }
                            else
                            {
                                return RedirectToAction("Restautrant");
                            }
                        }

                        else
                        {

                            return RedirectToAction("Index", "Home");
                        }
                    
                    }
                    else
                    {
                        ModelState.AddModelError("user_name", "Checck your input data");
                    }
                }
                ModelState.AddModelError("user_name", "Please, Registration First or check your data");
                return View();
            }                
        }






        //sign up
        public ActionResult Signup()
        {
            return View();
        }



        [HttpPost]
        public JsonResult Sign(User_tbl model)
        {
            using (var context = new FoodieEntities2())
            {

                var active = (from x in context.User_tbl
                              where (x.user_email == model.user_email && x.user_name == model.user_name)
                              select x.IsValid).FirstOrDefault();


                if (context.User_tbl.Any(x => x.user_email == model.user_email) && active == true)
                {

                    return Json(new { Success = false, LoginError = "This Email is already in use" }, JsonRequestBehavior.AllowGet);
               
                }

                if (context.User_tbl.Any(x => x.user_name == model.user_name) && active == true)
                {
                    return Json(new { Success = false, LoginError = "This Name is already in use" }, JsonRequestBehavior.AllowGet);

                }

                if (context.User_tbl.Any(x => x.user_email == model.user_email) && active == false)
                {
                    var id = (from s in context.User_tbl
                              where (s.user_email == model.user_email)
                              select s.user_id).FirstOrDefault();
                     var remove = context.User_tbl.Find(id);

                    context.User_tbl.Remove(remove);
                    context.SaveChanges();
                }
                
                model.IsValid = false;
                context.User_tbl.Add(model);
                context.SaveChanges();
                BuildEmailTemplate(model.user_id, model.user_name);

                return Json(new { Success = true, msg = "Registration Successful" }, JsonRequestBehavior.AllowGet);
                //return Json("Registration Successful", JsonRequestBehavior.AllowGet);
             

            }
            
        }




        //For email confirm

        public ActionResult Confirm(int regId)
        {
            ViewBag.regID = regId;
            return View();
        }


        //Registration confirm
        public JsonResult RegConfirm(int regId)
        {
            var context = new FoodieEntities2();
            User_tbl Data = context.User_tbl.Where(x => x.user_id == regId).FirstOrDefault();
            Data.IsValid = true;
            context.SaveChanges();
            string result = "Your Email is successfully verified";

            return Json(result, JsonRequestBehavior.AllowGet);

        }


        //Email-message
        private void BuildEmailTemplate(int regId, string name)
        {

            var context = new FoodieEntities2();
            string body = System.IO.File.ReadAllText(HostingEnvironment.MapPath("~/EmailTemplate/") + "Text" + ".cshtml");
            var regInfo = context.User_tbl.Where(x => x.user_id == regId).FirstOrDefault();
            var url = "https://localhost:44362/" + "Account/Confirm?regId=" + regId;

            body = body.Replace("@Viewbag.ConfirmationLink", url);
            body = body.Replace("@Viewbag.Name", name);
            body = body.ToString();


            BuildEmailTemplate("Verify your email for Foodies", body, regInfo.user_email);
        }

        private static void BuildEmailTemplate(string subjectText, string bodyText, string sendTo)
        {
            string from, to, bcc, cc, subject, body;

            from = "tech5soft2020@gmail.com";
            to = sendTo.Trim();
            bcc = "";
            cc = "";
            subject = subjectText;
            StringBuilder sb = new StringBuilder();

            sb.Append(bodyText);
            body = sb.ToString();
            MailMessage mail = new MailMessage();
            mail.From = new MailAddress(from);
            mail.To.Add(new MailAddress(to));

            if (!string.IsNullOrEmpty(bcc))
            {
                mail.Bcc.Add(new MailAddress(bcc));
            }
            if (!string.IsNullOrEmpty(cc))
            {
                mail.CC.Add(new MailAddress(cc));
            }

            mail.Subject = subjectText;
            mail.Body = body;
            mail.IsBodyHtml = true;
            SendEmail(mail);


        }

        private static void SendEmail(MailMessage mail)
        {
            SmtpClient client = new SmtpClient();

            client.Host = "smtp.gmail.com";
            client.Port = 587;
            client.EnableSsl = true;
            client.UseDefaultCredentials = false;
            client.Credentials = new System.Net.NetworkCredential("tech5soft2020@gmail.com", "12345!@#$%");
            try
            {
                client.Send(mail);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    





    //Restaurant manager
    public ActionResult Restautrant()
        {
            return View();
        }

        [HttpPost]
        

        public ActionResult Restautrant(tbl_restaurent model)
        {
            using (var context = new FoodieEntities2())
            {

                if (context.tbl_restaurent.Any(x => x.secret_code == model.secret_code))

                {
                    ModelState.AddModelError("secret_code", "This code is already in use. Please choose another");
                    return View();
                }

                context.tbl_restaurent.Add(model);
               

                context.SaveChanges();
                //Session["Rest_id"] = model.Rest_id.ToString();
            }

            return RedirectToAction("Addplatter","Platter");
        }

  

        public ActionResult Logout()
        {
            Session.Remove("Rest_id");
            Session.Remove("code");
            FormsAuthentication.SignOut();
            return RedirectToAction("Login");
        }
    }
}