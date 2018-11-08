namespace Finance101.Migrations
{
    using Finance101.Models;
    using Microsoft.AspNet.Identity;
    using Microsoft.AspNet.Identity.EntityFramework;
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<Finance101.Models.ApplicationDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
        }

        protected override void Seed(Finance101.Models.ApplicationDbContext context)
        {
            //  This method will be called after migrating to the latest version.
            var roleManager = new RoleManager<IdentityRole>(
                new RoleStore<IdentityRole>(context));
            if (!context.Roles.Any(r => r.Name == "Admin"))
            {
                roleManager.Create(new IdentityRole { Name = "Admin" });
            }
            if (!context.Roles.Any(r => r.Name == "HeadOfHouseHold"))
            {
                roleManager.Create(new IdentityRole { Name = "HeadOfHouseHold" });
            }
            if (!context.Roles.Any(r => r.Name == "Member"))
            {
                roleManager.Create(new IdentityRole { Name = "Member" });
            }
            //  You can use the DbSet<T>.AddOrUpdate() helper extension method 
            //  to avoid creating duplicate seed data.
            var userManager = new UserManager<ApplicationUser>(
                new UserStore<ApplicationUser>(context));


            if (!context.Users.Any(u => u.Email == "CMPH@Mailinator.com"))
            {
                userManager.Create(new ApplicationUser
                {
                    UserName = "CMPH@Mailinator.com",
                    Email = "CMPH@Mailinator.com",
                    FirstName = "C",
                    LastName = "H",
                    DisplayName = "CMPH",
                }, "Abcd1234!");
            }

            var CMPHId = userManager.FindByEmail("CMPH@Mailinator.com").Id;
            userManager.AddToRole(CMPHId, "Admin");


            //First seed a Demo House
            context.Households.AddOrUpdate(h => h.Name,
                new Household { Id = 999, Name = "House 1", Created = DateTimeOffset.Now, HouseholdCreatorId = CMPHId }
            );



            ////*****************************************//
            //if (!System.Diagnostics.Debugger.IsAttached)
            //    System.Diagnostics.Debugger.Launch();
            ////*****************************************//



            //create user to assign to admin role
            if (!context.Users.Any(u => u.Email == "HeadOfHouseHold@Mailinator.com"))
            {
                userManager.Create(new ApplicationUser
                {
                    UserName = "HeadOfHouseHold@Mailinator.com",
                    Email = "HeadOfHouseHold@Mailinator.com",
                    FirstName = "Head",
                    LastName = "OfHouseHold",
                    DisplayName = "HeadOfHousehold1",
                    HouseholdId = 999,
                }, "Abcd1234!");
            }

            if (!context.Users.Any(u => u.Email == "Member1@Mailinator.com"))
            {
                userManager.Create(new ApplicationUser
                {
                    UserName = "Member1@Mailinator.com",
                    Email = "Member1@Mailinator.com",
                    FirstName = "Member1FirstName",
                    LastName = "Member1LastName",
                    DisplayName = "Member1",
                    HouseholdId = 1,
                }, "Abcd1234!");
            }

            if (!context.Users.Any(u => u.Email == "Member2@Mailinator.com"))
            {
                userManager.Create(new ApplicationUser
                {
                    UserName = "Member2@Mailinator.com",
                    Email = "Member2@Mailinator.com",
                    FirstName = "Member2FirstName",
                    LastName = "Member2LastName",
                    DisplayName = "Member2",
                    HouseholdId = 1,
                }, "Abcd1234!");
            }



            //assign users to roles            
            var HeadOfHouseHoldId = userManager.FindByEmail("HeadOfHouseHold@Mailinator.com").Id;
            userManager.AddToRole(HeadOfHouseHoldId, "HeadOfHouseHold");

            var Member1Id = userManager.FindByEmail("Member1@Mailinator.com").Id;
            userManager.AddToRole(Member1Id, "Member");

            var Member2Id = userManager.FindByEmail("Member2@Mailinator.com").Id;
            userManager.AddToRole(Member2Id, "Member");

            //Categories
            context.Categories.AddOrUpdate(tt => tt.Name,
            new Models.Category { Name = "Food" },
            new Models.Category { Name = "Clothing" },
            new Models.Category { Name = "Rent" },
            new Models.Category { Name = "Bills" }
            );
        }
    }
}
