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

        public virtual DbSet<Country> AppCountries { get; set; }

        public virtual DbSet<Region> AppRegions { get; set; }

        public virtual DbSet<Place> AppPlaces { get; set; }

        public virtual DbSet<Accommodation> AppAccommodations { get; set; }

        public virtual DbSet<AccommodationType> AppAccommodationTypes { get; set; }

        public virtual DbSet<Room> AppRooms { get; set; }

        public virtual DbSet<RoomReservations> AppRoomReservations { get; set; }

        public virtual DbSet<Comment> AppComments { get; set; }

        public BAContext() : base("name=BADB")
        {            
        }

        public static BAContext Create()
        {
            return new BAContext();
        }
    }
}