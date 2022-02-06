
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using UserLogin.Models;

namespace Currency_Exchange.Controllers
{
    [Authorize(AuthenticationSchemes = "StaffAccount")]
    public class StaffController : Controller
    {
        //For signing in
        private const string AUTHSCHEME = "StaffAccount";

        private const string LOGIN_VIEW = "Login";

        //To check sign in credentials
        private const string LOGIN_SQL =
           @"SELECT * FROM Staff
            WHERE UserId = '{0}' 
              AND UserPw = HASHBYTES('SHA1', '{1}')";

        //For checking last login 
        private const string LASTLOGIN_SQL =
           @"UPDATE SRUser SET LastLogin=GETDATE() 
                        WHERE Ph_Num='{0}'";

        private const string NAME_COL = "FullName";

        //Where to redirect to after sign in
        private const string REDIRECT_CNTR = "Staff";
        private const string REDIRECT_ACTN = "Index";



        //Direct the user to Staff Login Page after they click the button
        [AllowAnonymous]
        public IActionResult Login(string returnUrl = null)
        {
            TempData["ReturnUrl"] = returnUrl;
            return View(LOGIN_VIEW);
        }

        //Checking if user enter the correct credentials
        [AllowAnonymous]
        [HttpPost]
        public IActionResult Login(Login user)
        {
            //If user enter wrongly, give error message and return view on the same login page
            if (!AuthenticateUser(user.UserID, user.Password, out ClaimsPrincipal principal))
            {
                ViewData["Message"] = "Incorrect User ID or Password";
                ViewData["MsgType"] = "warning";
                return View(LOGIN_VIEW);
            }

            //If user enter correctly, sign them in
            else
            {
                HttpContext.SignInAsync(
                   AUTHSCHEME,
                   principal,
               new AuthenticationProperties
               {
                   IsPersistent = false
               });

                // Update the Last Login Timestamp of the User
                DBUtl.ExecSQL(LASTLOGIN_SQL, user.UserID);

                if (TempData["returnUrl"] != null)
                {
                    string returnUrl = TempData["returnUrl"].ToString();
                    if (Url.IsLocalUrl(returnUrl))
                        return Redirect(returnUrl);
                }

                return RedirectToAction(REDIRECT_ACTN, REDIRECT_CNTR);
            }
        }

        [Authorize]
        public IActionResult Logoff(string returnUrl = null)
        {
            HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            if (Url.IsLocalUrl(returnUrl))
                return Redirect(returnUrl);
            return RedirectToAction("Index", "HomePage");
        }

        //If user is not authorized to view this page, throw them to forbidden page
        [AllowAnonymous]
        public IActionResult Forbidden()
        {
            return View();
        }

        //Authenticating user by checking user ID and Password
        private bool AuthenticateUser(string uid, string pw,
                                         out ClaimsPrincipal principal)
        {
            principal = null;

            DataTable ds = DBUtl.GetTable(LOGIN_SQL, uid, pw);
            if (ds.Rows.Count == 1)
            {
                principal =
                   new ClaimsPrincipal(
                      new ClaimsIdentity(
                         new Claim[] {
                        new Claim(ClaimTypes.NameIdentifier, ds.Rows[0]["UserId"].ToString()),
                        new Claim(ClaimTypes.Name, ds.Rows[0][NAME_COL].ToString()),
                         }, "Basic"
                      )
                   );
                return true;
            }
            return false;
        }
    }
}
