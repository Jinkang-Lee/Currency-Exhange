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
    public class AdminAccountController : Controller
    {
        
        
        private const string LOGIN_VIEW = "Login";

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

        [Authorize]
        public IActionResult Logoff(string returnUrl = null)
        {
            HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            if (Url.IsLocalUrl(returnUrl))
                return Redirect(returnUrl);
            return RedirectToAction("Login", "Account");
        }


        //Checking if user enter the correct credentials
        [AllowAnonymous]
        [HttpPost]
        public IActionResult Login(Login user)
        {
            //If user enter wrongly, give error message and return view on the same login page
            if (!AuthenticateUser(user.UserID, user.Password,
                                  out ClaimsPrincipal principal))
            {
                ViewData["Message"] = "Incorrect User ID or Password";
                return View();
            }

            //If user enter correctly, sign them in
            else
            {
                HttpContext.SignInAsync(
                   CookieAuthenticationDefaults.AuthenticationScheme,
                   principal);
                if (TempData["returnUrl"] != null)
                {
                    string returnUrl = TempData["returnUrl"].ToString();
                    if (Url.IsLocalUrl(returnUrl))
                        return Redirect(returnUrl);
                }
                //Redirect user to Staff/Index after successful login
                return RedirectToAction(REDIRECT_ACTN, REDIRECT_CNTR);
            }
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

            string sql = @"SELECT * FROM CurrencyRate
                         WHERE UserId = '{0}' AND UserPw = HASHBYTES('SHA1', '{1}')";
            ViewData["sql"] = sql;

            //Checking in the database if such user exist
            DataTable ds = DBUtl.GetTable(sql, uid, pw);
            if (ds.Rows.Count == 1)
            {
                principal =
                   new ClaimsPrincipal(
                      new ClaimsIdentity(
                         new Claim[] {
                        new Claim(ClaimTypes.NameIdentifier, uid),
                        new Claim(ClaimTypes.Name, ds.Rows[0]["FullName"].ToString()),
                        new Claim(ClaimTypes.Role, ds.Rows[0]["UserRole"].ToString())
                         },
                         CookieAuthenticationDefaults.AuthenticationScheme));
                return true;
            }
            return false;
        }
    }
}
