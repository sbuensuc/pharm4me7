using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using pharm4me7.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;


namespace pharm4me7.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            

            if (User.Identity.IsAuthenticated)
            {
                var manager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(new ApplicationDbContext()));
                var context = new OrderContext();
                var currentUser = manager.FindById(User.Identity.GetUserId());

                if (currentUser.PatientId != null)
                {
                    
                    ViewBag.Message1 = "Welcome, " + currentUser.Patient.FirstName + " " + currentUser.Patient.LastName;

                    
                }

                else if (currentUser.DoctorId != null)
                {
                    ViewBag.Message1 = "Welcome, " + currentUser.Doctor.FirstName + " " + currentUser.Doctor.LastName;
                    ViewBag.Message2 = currentUser.Doctor.Clinic.Name;

                    
                }

                else if (currentUser.PharmacistId != null)
                {
                    ViewBag.Message1 = "Welcome, " + currentUser.Pharmacist.FirstName + " " + currentUser.Pharmacist.LastName;
                    ViewBag.Message2 = currentUser.Pharmacist.Pharmacy.Name ;

                    
                }



            }
            else
            {
                ViewBag.Message1 = "Welcome to Pharm4me";
            }

            return View();
        }

        public ActionResult PreRegister()
        {
            return View();
        }

        public ActionResult Config()
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
    }
}