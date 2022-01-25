using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Currency_Exchange.Models;

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
            DataTable dt = DBUtl.GetTable("SELECT Employee_Id, UserId, FullName, Ph_Num FROM Staff");
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
        public IActionResult CreateStaff(Staff staff)
        {
            //string userid = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;

            //If everything goes right
            if (ModelState.IsValid)
            {
                //INSERT the new staff into database
                string sql = @"INSERT INTO Staff (UserId, UserPw, FullName, Ph_Num)
                                            VALUES ('{0}', HASHBYTES('SHA1', '{1}'), '{2}', {3})";

                if (DBUtl.ExecSQL(sql, staff.UserId, staff.UserPw, staff.FullName, staff.Ph_Num) == 1)
                    TempData["Msg"] = "New staff Added!";
                return RedirectToAction("Index");
            }
            //Else throw error message
            else
            {
                TempData["Msg"] = "Invalid information entered!";
                return RedirectToAction("CreateStaff");
            }
        }


        
        //HTTP GET UPDATE STAFF
        public IActionResult EditStaff(int id)
        {
            //SQL Statement to find the staff that is selected to edit
            List<Staff> list = DBUtl.GetList<Staff>("SELECT Employee_Id, UserId, FullName, Ph_Num FROM Staff WHERE Employee_Id = {0}", id);
            Staff model = null;
            
            //If the number of items in the list is 1
            if (list.Count == 1)
            {
                //Display the first item in the list
                model = list[0];
                return View("EditStaff", model);
            }
            //Else throw error message
            else
            {
                TempData["Message"] = "Staff Not Found!";
                TempData["MsgType"] = "warning";
                return RedirectToAction("Index");
            }
        }

        //HTTP POST FOR EDITING STAFF IN THE LIST
        [HttpPost]
        public IActionResult EditStaffPost(Staff staff)
        {
            //If something goes wrong, throw error message
            if (!ModelState.IsValid)
            {
                TempData["Message"] = "Invalid Input!";
                TempData["MsgType"] = "warning";
                return View("Index");
            }

            //Else execute the UPDATE statement to update staff details
            else
            {
                string update = @"UPDATE Staff SET UserId='{0}', FullName='{1}', ph_num={2} WHERE Employee_Id={3}";

                int res = DBUtl.ExecSQL(update, staff.UserId, staff.FullName, staff.Ph_Num, staff.Id);

                if (res == 1)
                {
                    TempData["Message"] = "Success!";
                    TempData["MsgType"] = "success";
                }

                else
                {
                    TempData["Message"] = DBUtl.DB_Message;
                    TempData["MsgType"] = "danger";
                }

                return RedirectToAction("ViewStaff");
            }
        }

//DELETE STAFF
        public IActionResult DeleteStaff(int id)
        {
            string select = @"SELECT Employee_Id, UserId, FullName, Ph_Num FROM Staff WHERE Employee_Id={0}";
            DataTable ds = DBUtl.GetTable(select, id);
            if (ds.Rows.Count != 1)
            {
                TempData["Message"] = "Staff does not exist";
                TempData["MsgType"] = "warning";
            }
            else
            {
                string delete = "DELETE FROM Staff WHERE Employee_Id={0}";
                int res = DBUtl.ExecSQL(delete, id);
                if (res == 1)
                {
                    TempData["Message"] = "Staff Deleted";
                    TempData["MsgType"] = "success";
                }
                else
                {
                    TempData["Message"] = DBUtl.DB_Message;
                    TempData["MsgType"] = "danger";
                }
            }
            return RedirectToAction("ViewStaff");
        }
    }
}