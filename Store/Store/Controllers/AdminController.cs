using Store.DAL;
using Store.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Store.Controllers
{
    public class AdminController : Controller
    {
        //
        // GET: /Admin/
        public ActionResult Admin()
        {
            //! Reffering to login page
            return View(new LoginModel());
        }

        public ActionResult AdminArea(LoginModel currentLogin)
        {
            //! Verifying the currentLogin details.
            LoginDAL logDAL = new LoginDAL();

            //! Testing in DB if has a match
            var testLogin = logDAL.Logins.FirstOrDefault(
                user => (user.UserName.Equals(currentLogin.UserName)) &&
                (user.Password.Equals(currentLogin.Password))
                );


            if (testLogin != null)
                return View("AdminArea", currentLogin); //! Reffering to AdminArea page
            else
                return View("Admin", currentLogin);
            
        }

        [HttpPost]
        public ActionResult Logon(LoginModel currentLogin)
        {
            if (ModelState.IsValid)
                return AdminArea(currentLogin);
            else return View("Admin", currentLogin);
        }
	}
}