using BookingApp.Models;

namespace BookingApp.Migrations
{
    using Microsoft.AspNet.Identity;
    using Microsoft.AspNet.Identity.EntityFramework;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<BookingApp.Models.BAContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(BookingApp.Models.BAContext context)
        {
            //  This method will be called after migrating to the latest version.

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method
            //  to avoid creating duplicate seed data. E.g.
            //
            //    context.People.AddOrUpdate(
            //      p => p.FullName,
            //      new Person { FullName = "Andrew Peters" },
            //      new Person { FullName = "Brice Lambson" },
            //      new Person { FullName = "Rowan Miller" }
            //    );
            //

            if (!context.Roles.Any(r => r.Name == "Admin"))
            {
                var store = new RoleStore<IdentityRole>(context);
                var manager = new RoleManager<IdentityRole>(store);
                var role = new IdentityRole { Name = "Admin" };

                manager.Create(role);
            }

            if (!context.Roles.Any(r => r.Name == "Manager"))
            {
                var store = new RoleStore<IdentityRole>(context);
                var manager = new RoleManager<IdentityRole>(store);
                var role = new IdentityRole { Name = "Manager" };

                manager.Create(role);
            }

            if (!context.Roles.Any(r => r.Name == "AppUser"))
            {
                var store = new RoleStore<IdentityRole>(context);
                var manager = new RoleManager<IdentityRole>(store);
                var role = new IdentityRole { Name = "AppUser" };

                manager.Create(role);
            }

            var userStore = new UserStore<BAIdentityUser>(context);
            var userManager = new UserManager<BAIdentityUser>(userStore);


            context.AppUsers.AddOrUpdate(
                       p => p.Id,
                            new AppUser { Id = 1, UserName = "adminn", FullName = "adminn" },
                            new AppUser { Id = 2, UserName = "maki", FullName = "maki" },
                            new AppUser { Id = 3, UserName = "dukica", FullName = "dukica" }
                     );




            if (!context.Users.Any(u => u.UserName == "adminn"))
            {
                var appUser = context.AppUsers.FirstOrDefault(p => p.FullName == "adminn");

                var user = new BAIdentityUser() { Id = "adminn", UserName = "adminn", Email = "admin@yahoo.com", PasswordHash = BAIdentityUser.HashPassword("adminn"), appUserId = 1 };
                userManager.Create(user);
                userManager.AddToRole(user.Id, "Admin");
            }

            if (!context.Users.Any(u => u.UserName == "maki"))
            {
                var appUser = context.AppUsers.FirstOrDefault(p => p.FullName == "maki");

                var user = new BAIdentityUser() { Id = "maki", UserName = "maki", Email = "maki@yahoo.com", PasswordHash = BAIdentityUser.HashPassword("maki"), appUserId = 2 };
                userManager.Create(user);
                userManager.AddToRole(user.Id, "Admin");
            }

            if (!context.Users.Any(u => u.UserName == "dukica"))
            {
                var appUser = context.AppUsers.FirstOrDefault(p => p.FullName == "dukica");

                var user = new BAIdentityUser() { Id = "dukica", UserName = "dukica", Email = "dukica@yahoo.com", PasswordHash = BAIdentityUser.HashPassword("dukica"), appUserId = 3 };
                userManager.Create(user);
                userManager.AddToRole(user.Id, "Manager");
            }

            //dodavanje drzava
            context.AppCountries.AddOrUpdate(
                       p => p.Id,
                            new Country { Id = 1, Name = "Srbija", Code = "RS" },
                            new Country { Id = 2, Name = "Bosna i Hercegovina", Code = "BiH" },
                            new Country { Id = 3, Name = "Makedonija", Code = "MKD" }
                     );
        }
    }
}