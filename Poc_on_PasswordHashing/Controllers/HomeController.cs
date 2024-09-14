using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using System.Web.UI.WebControls;
using Poc_on_PasswordHashing.Data;
using Poc_on_PasswordHashing.Models;
using Poc_on_PasswordHashing.ViewModel;

namespace Poc_on_PasswordHashing.Controllers
{
    public class HomeController : Controller
    {
        [HttpGet]
        public ActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Login(LoginVM loginVm)
        {
            using (var session = NHibernateHelper.CreateSession())
            {
                using (var txn = session.BeginTransaction())
                {
                    var user = session.Query<User>().SingleOrDefault(u => u.Username == loginVm.Username);
                    if(user!=null)
                    {
                        bool hashedPassword = BCrypt.Net.BCrypt.EnhancedVerify(loginVm.Password,user.Password);
                        if(hashedPassword)
                        {
                            FormsAuthentication.SetAuthCookie(loginVm.Username, true);
                            return RedirectToAction("Welcome");
                        }
                    }
                }
                ModelState.AddModelError("", "UserName/Password doesn't match");
                return View();
            }
        }
        [HttpGet]
        public ActionResult Register()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Register(User user)
        {
            using (var session = NHibernateHelper.CreateSession())
            {
                using (var txn = session.BeginTransaction())
                {
                    string passwordHash = BCrypt.Net.BCrypt.EnhancedHashPassword(user.Password, 13);

                    User newUser = new User
                    {
                        Username = user.Username,
                        Password = passwordHash
                    };
                    session.Save(newUser);
                    txn.Commit();
                    return RedirectToAction("Login");
                }
            }
        }
        public ActionResult Welcome()
        {
            return View();
        }
    }
}