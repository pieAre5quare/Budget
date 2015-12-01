namespace Budget.Migrations
{
    using Models;
    using Microsoft.AspNet.Identity;
    using Microsoft.AspNet.Identity.EntityFramework;
    using System;
    using System.Configuration;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<ApplicationDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
        }

        protected override void Seed(ApplicationDbContext context)
        {
            var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(context));

            if (!context.Roles.Any(r => r.Name == "Admin"))
            {
                roleManager.Create(new IdentityRole { Name = "Admin" });
            }

            var userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(context));
            var AdminUser = ConfigurationManager.AppSettings["AdminUser"];
            var Passwd = ConfigurationManager.AppSettings["AdminPassword"];
            if (!context.Users.Any(u => u.Email == AdminUser))
            {
                userManager.Create(new ApplicationUser
                {
                    UserName = AdminUser,
                    Email = AdminUser,
                    FirstName = "Allan",
                    LastName = "Clark",
                    DisplayName = "aclark"
                }, Passwd);
            }

            var userId = userManager.FindByEmail(AdminUser).Id;
            userManager.AddToRole(userId, "Admin");
        }
    }
}
