using Foodies.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Rotativa;
using System.Drawing.Printing;

namespace Foodies.Controllers
{
    
    public class PlatterController : Controller
    {
        FoodieEntities2 db = new FoodieEntities2();
     
  
        public ActionResult Index(int code)
        {
            //if (Session["Rest_id"] != null)
            //{
            //    Session["Rest_id"].ToString();
            //}
            //if (Session["scode"] != null)
            //{
            //    Session["scode"].ToString();
            //}
            if (code != 0)
            {
                var item = (from d in db.tbl_platter
                            select d).ToList();

                var userid = (from d in db.tbl_restaurent
                                where d.secret_code == code
                                select d.User_id).FirstOrDefault();

                var user_role = (from u in db.User_tbl
                                where u.user_id == userid
                                select u.Role_Id).FirstOrDefault().ToString();



                if (db.tbl_platter.Any(x => x.secret_code == code))
                {
                    item = item.Where(i => i.secret_code == code).ToList();

                    return View(item.ToList());
                }
            }
         

            ViewBag.Msg = "No Record Found";
            return View();
           
                      
        }



        //[Authorize(Roles = "Admin")]
        public ActionResult List()
        {
            //if (Session["scode"] != null)
            //{
            //    Session["scode"].ToString();
            //}
            if (Session["admin"] != null)
            {
                List<tbl_platter> platternames = db.tbl_platter.ToList();
                List<tbl_restaurent> restaurentnames = db.tbl_restaurent.ToList();
                List<Rating_tbl> rating = db.Rating_tbl.ToList();

                var item = from p in platternames
                           join r in restaurentnames on p.Rest_id equals r.Rest_id into table1
                           from r in table1.DefaultIfEmpty()
                           join rate in rating on p.platter_id equals rate.Platter_id into table2
                           from rate in table2.DefaultIfEmpty()
                           select new JoinClass { platterdetail = p, restaurentdetail = r, ratingdetail = rate };

                if (item.ToList().Count == 0)
                {
                    ViewBag.Msg = "No Record Found";
                    return View();
                }

                return View(item.ToList());
            }
            else
            {
             return new HttpStatusCodeResult(HttpStatusCode.BadRequest, "Not Found");
            }

        }



        public ActionResult HomePage(string search)
        {
            //if(Session["User_id"] != null && Session["User_name"] != null)
            //{
            //    Session["User_id"].ToString();
            //    Session["User_name"].ToString();
            //}
            //if (Session["Rest_id"] != null)
            //{
            //    Session["Rest_id"].ToString();
            //}
            //if (Session["scode"] != null)
            //{
            //    Session["scode"].ToString();
            //}
            var rate = db.Rating_tbl.ToList();
            var platter = db.tbl_platter.OrderByDescending(r => r.rating).ToList();
            var rest = db.tbl_restaurent.ToList();

            JoinClass item = new JoinClass();
            item.platterdetails = platter;
            item.restaurentdetails = rest;
            item.ratingdetails = rate;

          

            return PartialView(item);

        }

  
        [HttpGet]//Add
        //[Authorize(Roles = "Manager")]
        public ActionResult Addplatter()
        {
            if (Session["manager"] != null)
            {
                return View();
            }
            
            else
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest, "Not Found");
            }
        }


        
        [HttpPost]
        //[Authorize(Roles = "Manager")]
        public ActionResult Addplatter(tbl_platter model1, HttpPostedFileBase image1)
        {

         
            //if (Session["scode"] != null)
            //{
            //    Session["scode"].ToString();
            //}

            var rest_id = (from x in db.tbl_restaurent
                      where x.secret_code == model1.secret_code
                      select x.Rest_id).FirstOrDefault();

         

            if ( rest_id.ToString() != null)
            {
                

                if (db.tbl_restaurent.Any( x => x.secret_code == model1.secret_code))

                {
                    if (image1 != null)
                    {
                        model1.platter_image = new byte[image1.ContentLength];
                        image1.InputStream.Read(model1.platter_image, 0, image1.ContentLength);
                    }
                    model1.Rest_id = rest_id;


                    db.tbl_platter.Add(model1);

                    try
                    {

                        db.SaveChanges();
                    }
                    catch (DbEntityValidationException e)
                    {
                        foreach (var eve in e.EntityValidationErrors)
                        {
                            Console.WriteLine("Entity of type \"{0}\" in state \"{1}\" has the following validation errors:",
                                eve.Entry.Entity.GetType().Name, eve.Entry.State);
                            foreach (var ve in eve.ValidationErrors)
                            {
                                Console.WriteLine("- Property: \"{0}\", Error: \"{1}\"",
                                    ve.PropertyName, ve.ErrorMessage);
                            }
                        }
                        throw;
                    }
                    Session["scode"] = model1.secret_code;
                    ViewBag.Msg = "Data Added Successfully.";
                    ModelState.Clear();
                    return View();

                    //return RedirectToAction("Index", new { id = model1.secret_code }); 
                }

                ModelState.AddModelError("secret_code", "Code doesn't match.");
            }

            else
            {
                return RedirectToAction("Restautrant", "Account");
            }
           
            return View();
        }








        // GET: Details/5
      
        public ActionResult Details(int? id)
        {
            //if (Session["scode"] != null)
            //{
            //    Session["scode"].ToString();
            //}
            if ((id == null ||  Session["manager"] == null) && Session["role"].ToString() == "2")
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
          
            if ((id == null || Session["admin"] == null)  && Session["role"].ToString() == "1")
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            tbl_platter platter_tbl = db.tbl_platter.Find(id);
            if (platter_tbl == null)
            {
                return HttpNotFound();
            }
            return View(platter_tbl);
        }


        //[Authorize(Roles ="Manager")]
        public ActionResult Edit(int id)
        {
            if (Session["manager"] != null)
            {
                var platter = db.tbl_platter.Find(id);

                return View(platter);
            }
            else
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest, "Not Found");
            }
        }


        [HttpPost]
        //[Authorize(Roles = "Manager")]
        public ActionResult Edit(tbl_platter platter, HttpPostedFileBase image1)
        {
            
            if (ModelState.IsValid)
            {
                //if (Session["scode"] != null)
                //{
                //    Session["scode"].ToString();
                //}
                if (image1 != null)
                {
                    platter.platter_image = new byte[image1.ContentLength];
                    image1.InputStream.Read(platter.platter_image, 0, image1.ContentLength);
                }
                
                db.Entry(platter).State = System.Data.Entity.EntityState.Modified;

                db.SaveChanges();
                return RedirectToAction("Index", new { code = platter.secret_code });
            }
            return View();
        }


        [HttpPost]

        public ActionResult Delete(int id)
        {
            if (Session["manager"] != null || Session["admin"] != null)
            {
                var secretCode = db.tbl_platter.Where(x => x.platter_id == id).Select(x => x.secret_code).FirstOrDefault();
                Session["scode"] = secretCode;

                var platter = db.tbl_platter.Find(id);
                var rate = db.Rating_tbl.Where(x => x.Platter_id == id).Select(x => x.Rating_id).ToList();
            
                for (var x = 0; x < rate.Count(); x++)
                {
                    var todo = db.Rating_tbl.Find(rate[x]);
                    db.Rating_tbl.Remove(todo);
                    db.SaveChanges();
                }
                db.tbl_platter.Remove(platter);
                db.SaveChanges();
                if(Session["role"].ToString() == "1")
                {
                    return RedirectToAction("List", "Platter");
                }
               
                return RedirectToAction("Index", "Platter", new { code = secretCode });               
             }
   
            else
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest, "Not Found");
            }   
        }



        //Print Out All Menu 
        public ActionResult Print()
        {
            var q = new ActionAsPdf("Get_All", new { code = Session["scode"] });
            return q;
        }

        public ActionResult Get_All(int code)
        {
            if (code != 0)
            {
                var item = (from d in db.tbl_platter
                            select d).ToList();

                var userid = (from d in db.tbl_restaurent
                              where d.secret_code == code
                              select d.User_id).FirstOrDefault();

                var user_role = (from u in db.User_tbl
                                 where u.user_id == userid
                                 select u.Role_Id).FirstOrDefault().ToString();



                if (db.tbl_platter.Any(x => x.secret_code == code))
                {
                    item = item.Where(i => i.secret_code == code).ToList();

                    return View(item.ToList());
                }
            }

            ViewBag.Msg = "No Record Found";
            return View();
        }

    }
}