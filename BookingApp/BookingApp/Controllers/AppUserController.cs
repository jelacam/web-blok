using BookingApp.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace BookingApp.Controllers
{
    [RoutePrefix("api/AppUser")]
    public class AppUserController : ApiController
    {
        private BAContext db = new BAContext();

        //User manager -> We will use it to check role if needed.
        private ApplicationUserManager _userManager;

        public ApplicationUserManager UserManager
        {
            get
            {
                return _userManager ?? Request.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            private set
            {
                _userManager = value;
            }
        }

        
        [Authorize(Roles = "Manager")]
        [HttpGet]
        [Route("Users")]
        public IQueryable<AppUser> m1()
        {
            return db.AppUsers;
        }
        
        [Authorize(Roles = "Manager, Admin")]
        [HttpGet]
        [Route("Countires")]
        public IQueryable<Country> m2()
        {
            return db.AppCountries;
        }
    



    }
}
