using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DeigCrud.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace DeigCrud.Controllers
{
   
    public class HomeController : Controller
    {
        private string message = "";
        private UserManager<AppUser> userManager;
        public HomeController(UserManager<AppUser> userMgr)
        {
            userManager = userMgr;
        }

#nullable enable
        [Authorize(Roles = "Admin, List")]
        public async Task<IActionResult> Index()
        {
         
            AppUser user = await userManager.GetUserAsync(HttpContext.User);
            if (user != null)
            {
                message = "Hello " + user.UserName;
            }
            else
            {
                message = "Hello Cruel World!";
            }
            
            return View((object)message);
        }
    }
}
