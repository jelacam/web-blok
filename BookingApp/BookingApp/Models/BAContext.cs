using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Security.AccessControl;
using System.Web;
using Microsoft.AspNet.Identity.EntityFramework;

namespace BookingApp.Models
{
    public class BAContext: IdentityDbContext<BAIdentityUser>
    {   
        public virtual DbSet<AppUser> AppUsers { get; set; }

        public virtual DbSet<Country> AppCountrys { get; set; }

        public virtual DbSet<Region> AppRegions { get; set; }

        public virtual DbSet<Place> AppPlaces { get; set; }

        public BAContext() : base("name=BADB")
        {            
        }

        public static BAContext Create()
        {
            return new BAContext();
        }
    }
}