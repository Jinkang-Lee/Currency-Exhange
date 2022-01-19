using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Currency_Exchange.Controllers
{
    public class AdminController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }


        //View all the staff in the database
        public IActionResult ViewStaff()
        {
            DataTable dt = DBUtl.GetTable("SELECT * FROM Staff");
            return View("ViewStaff", dt.Rows);
        }

        //HTTPGET
        //BRING YOU TO THE CREATE STAFF PAGE
        public IActionResult CreateStaff()
        {
            return View();
        }

        //SQL INSERT INTO STAFF DATABASE TO CREATE A NEW STAFF
        //This is what will happen when you click the button after creating new staff
        [HttpPost]
        [Authorize]
        //public IActionResult CreateStaff(Staff staff)
        //{
        //    string userid = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;
        //    if (ModelState.IsValid)
        //    {
        //        string sql = @"INSERT INTO Staff
        //                                    (staff_email, staff_name, ph_num)
        //                                    VALUES ('{0}', '{1}', {2})";

        //        if (DBUtl.ExecSQL(sql, staff.StudEmail, staff.StudName, staff.StudPhNum) == 1)
        //            TempData["Msg"] = "New staff Added!";
        //        return RedirectToAction("ViewStaff");
        //    }
        //    else
        //    {
        //        TempData["Msg"] = "Invalid information entered!";
        //        return RedirectToAction("Index");
        //    }
        //}



        //UPDATE STAFF - NOT DONE
        public IActionResult EditStaff()
        {
            return View();
        }

        //DELETE STAFF - NOT DONE
        public IActionResult DeleteStaff()
        {
            return View();
        }
    }
}
