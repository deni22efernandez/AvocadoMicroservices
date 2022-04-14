using Avocado.Services.IdentityServer.DbContexts;
using Avocado.Services.IdentityServer.Models;
using IdentityModel;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Avocado.Services.IdentityServer.Initializer
{
	public class DbInitializer : IDbInitializer
	{
		private readonly ApplicationDbContext _dbContext;
		private readonly UserManager<ApplicationUser> _userManager;
		private readonly RoleManager<IdentityRole> _roleManager;
		public DbInitializer(ApplicationDbContext dbContext,
							UserManager<ApplicationUser> userManager,
							 RoleManager<IdentityRole> roleManager)
		{
			_dbContext = dbContext;
			_roleManager = roleManager;
			_userManager = userManager;
		}
		public void Initialize()
		{
			if (!_roleManager.RoleExistsAsync(Common.admin).GetAwaiter().GetResult())
			{
				 _roleManager.CreateAsync(new IdentityRole(Common.admin)).GetAwaiter().GetResult();
				 _roleManager.CreateAsync(new IdentityRole(Common.customer)).GetAwaiter().GetResult();
			}
			else
			{
				return;
			}
			ApplicationUser adminUser = new ApplicationUser
			{
				Id = Guid.NewGuid().ToString(),
				Email = "admin@avocado.com",
				EmailConfirmed = false,
				NormalizedEmail = "ADMIN@AVOCADO.COM",
				Name = "Admin",
				LastName = "admin",
				UserName = "admin@avocado.com"

			};
			_userManager.CreateAsync(adminUser,"Password123*").GetAwaiter().GetResult();
			_userManager.AddToRoleAsync(adminUser, Common.admin).GetAwaiter().GetResult();
			_userManager.AddClaimsAsync(adminUser, new System.Security.Claims.Claim[]
			{
				new System.Security.Claims.Claim(JwtClaimTypes.Subject, adminUser.Id),
				new System.Security.Claims.Claim(JwtClaimTypes.Name, adminUser.Name),
				new System.Security.Claims.Claim(JwtClaimTypes.Role, Common.admin),
				new System.Security.Claims.Claim(JwtClaimTypes.Email, adminUser.Email)
			}).GetAwaiter().GetResult();

			//customer user
			ApplicationUser customerUser = new ApplicationUser
			{
				Id = Guid.NewGuid().ToString(),
				Email = "customer@avocado.com",
				EmailConfirmed = false,
				NormalizedEmail = "CUSTOMER@AVOCADO.COM",
				Name = "Customer",
				LastName = "customer",
				UserName = "customer@avocado.com"

			};
			_userManager.CreateAsync(customerUser, "Password123*").GetAwaiter().GetResult();
			_userManager.AddToRoleAsync(customerUser, Common.customer).GetAwaiter().GetResult();
			_userManager.AddClaimsAsync(customerUser, new System.Security.Claims.Claim[]
			{
				new System.Security.Claims.Claim(JwtClaimTypes.Name, customerUser.Name),
				new System.Security.Claims.Claim(JwtClaimTypes.Role, Common.customer),
				new System.Security.Claims.Claim(JwtClaimTypes.Subject, customerUser.Id),
				new System.Security.Claims.Claim(JwtClaimTypes.Email, customerUser.Email)
			}).GetAwaiter().GetResult();
		}
	}
}
