namespace myoungblog.Migrations
{
	using System;
	using System.Data.Entity;
	using System.Data.Entity.Migrations;
	using System.Linq;
	using myoungblog.Models;
	using Microsoft.AspNet.Identity;
	using Microsoft.AspNet.Identity.EntityFramework;

	internal sealed class Configuration : DbMigrationsConfiguration<ApplicationDbContext>
	{
		public Configuration()
		{
			AutomaticMigrationsEnabled = true;
			AutomaticMigrationDataLossAllowed = true;
		}



		protected override void Seed(ApplicationDbContext context)
		{
			var roleManager = new RoleManager<IdentityRole>(
				new RoleStore<IdentityRole>(context));
			if (!context.Roles.Any(r => r.Name == "Admin"))
			{
				roleManager.Create(new IdentityRole { Name = "Admin" });
			}
			if (!context.Roles.Any(r => r.Name == "Moderator"))
			{
				roleManager.Create(new IdentityRole { Name = "Moderator" });
			}

			var userManager = new UserManager<ApplicationUser>(
				new UserStore<ApplicationUser>(context));
			if (!context.Users.Any(u => u.Email == "martingyoung22@gmail.com"))     //All this code(seed method) is to configuare the program we are writing
			{                                                                       //to pre-existing external frameworks that will take our input DATA
				userManager.Create(new ApplicationUser                              //and set it to pre-defined web structures like tables.
				{
					UserName = "martingyoung22@gmail.com",
					Email = "martingyoung22@gmail.com",
					FirstName = "Martin",
					LastName = "Young",
				}, "VaginaDentata22");
			}
			if (!context.Users.Any(u => u.Email == "mjaang@coderfoundry.com"))     //All this code(seed method) is to configuare the program we are writing
			{                                                                       //to pre-existing external frameworks that will take our input DATA
				userManager.Create(new ApplicationUser                              //and set it to pre-defined web structures like tables.
				{
					UserName = "mjaang@coderfoundry.com",
					Email = "mjaang@coderfoundry.com",
					FirstName = "Mark",
					LastName = "Jaang",
				}, "Abc@123");
			}
			if (!context.Users.Any(u => u.Email == "rchapman@coderfoundry.com"))     //All this code(seed method) is to configuare the program we are writing
			{                                                                       //to pre-existing external frameworks that will take our input DATA
				userManager.Create(new ApplicationUser                              //and set it to pre-defined web structures like tables.
				{
					UserName = "rchapman@coderfoundry.com",
					Email = "rchapman@coderfoundry.com",
					FirstName = "Ryan",
					LastName = "Chapman",
				}, "Abc@123");
			}

			var userId = userManager.FindByEmail("martingyoung22@gmail.com").Id;
			userManager.AddToRole(userId, "Admin");
			var userId1 = userManager.FindByEmail("mjaang@coderfoundry.com").Id;
			userManager.AddToRole(userId1, "Moderator");
			var userId2 = userManager.FindByEmail("rchapman@coderfoundry.com").Id;
			userManager.AddToRole(userId2, "Moderator");
		}
		
	}
}
