using Foodies.Models;
using System;
using System.Web.Mvc;
using System.Linq;

namespace Foodies.Controllers
{


    public class HomeController : Controller
    {
        FoodieEntities2 db = new FoodieEntities2();

        public ActionResult Index()
        {
            
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }


        [HttpPost]
        //Leave Review
        public JsonResult LeaveReview(Rating_tbl rating)
        {
            try
            {
                rating.Time = DateTime.Now;

                db.Rating_tbl.Add(rating);

                var totalrate = (from p in db.tbl_platter
                                 where (p.platter_id == rating.Platter_id)
                                 select p).FirstOrDefault();


                totalrate.rating_total = totalrate.rating_total + rating.Rate;
                totalrate.rating_count++;
                if (totalrate.rating == null)
                {
                    totalrate.rating = 0;
                }
                if (totalrate.rating_count != 0)
                {
                    totalrate.rating = totalrate.rating_total / totalrate.rating_count;
                }
                db.SaveChanges();
                
                return Json(new { Success = true, msg = "Thank You for your time" }, JsonRequestBehavior.AllowGet);
            }
            catch(Exception e)
            {
                return Json(new { Success = false, msg = e.Message }, JsonRequestBehavior.AllowGet);
            }
        }


        //Update review
        public ActionResult UpdateComment(int id)
        {
            if (Session["User_id"] != null && Session["User_name"] != null)
            {
                Session["User_id"].ToString();
                Session["User_name"].ToString();
            }
           
            var review = db.Rating_tbl.Find(id);

            return View(review);

        }


        [HttpPost]
     
        public ActionResult UpdateComment(Rating_tbl reviewmodel)
        {
            if (Session["User_id"] != null && Session["User_name"] != null)
            {
                Session["User_id"].ToString();
                Session["User_name"].ToString();
            }

            reviewmodel.Time = DateTime.Now;
            db.Entry(reviewmodel).State = System.Data.Entity.EntityState.Modified;

            var totalrate = (from p in db.tbl_platter
                             where (p.platter_id == reviewmodel.Platter_id)
                             select p).FirstOrDefault();


            totalrate.rating_total = totalrate.rating_total + reviewmodel.Rate;
            totalrate.rating_count++;

            db.SaveChanges();
            return RedirectToAction("Index","Home");
            
        }

      
        public ActionResult DeleteComment(int id)
        {

            var rate = db.Rating_tbl.Find(id);
            db.Rating_tbl.Remove(rate);
            db.SaveChanges();
            return RedirectToAction("Index", "Home");

        }

    }
}