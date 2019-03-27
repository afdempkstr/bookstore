using BookStore.Domain.Models;
using BookStore.Repositories;
using BookStore.ViewModels;
using Microsoft.AspNet.Identity;
using Microsoft.Owin.Security;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Web;
using System.Web.Mvc;

namespace BookStore.Controllers
{
    public class LoginController : Controller
    {
        // GET: Login
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login(Account account)
        {
            User user = null;
            bool verified = false;
            using (var db = new BookStoreDb())
            {
                user = db.Users.All().FirstOrDefault(u => u.Username == account.Username);
                if (user != null && db.CheckUserCredentials(account.Username, account.Password))
                {
                    verified = true;
                }
            }

            if (!verified)
            {
                ModelState.AddModelError("Username", "Invalid username or password");
                return RedirectToAction("Index");
            }

            var claims = new List<Claim>(new[]
            {
                // adding following 2 claim just for supporting default antiforgery provider
                new Claim(ClaimTypes.NameIdentifier, user.Username),
                new Claim(
                    "http://schemas.microsoft.com/accesscontrolservice/2010/07/claims/identityprovider",
                    "ASP.NET Identity", "http://www.w3.org/2001/XMLSchema#string"),
                new Claim(ClaimTypes.Name, user.Username),
                new Claim(ClaimTypes.UserData, user.Id.ToString())
            });

            foreach (var role in user.Roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role.Name));
            }

            var identity = new ClaimsIdentity(claims, DefaultAuthenticationTypes.ApplicationCookie);

            HttpContext.GetOwinContext().Authentication.SignIn(
                new AuthenticationProperties { IsPersistent = false }, identity);

            return RedirectToAction("Index", "Home");
        }

        [Authorize]
        public ActionResult Logout()
        {
            Session.Abandon();
            HttpContext.GetOwinContext().Authentication.SignOut(DefaultAuthenticationTypes.ApplicationCookie);
            return RedirectToAction("Index", "Home");
        }
    }
}