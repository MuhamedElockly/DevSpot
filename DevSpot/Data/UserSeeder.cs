using DevSpot.Constants;
using Microsoft.AspNetCore.Identity;

namespace DevSpot.Data
{
	public class UserSeeder
	{

		public static async Task SeedUserAsync(IServiceProvider serviceProvider)
		{
			var userManager = serviceProvider.GetRequiredService<UserManager<IdentityUser>>();


			await CreateUserWithRole(userManager, "eloclyahmed@gmail.com", "Mohamed#9475", Roles.Admin);
			await CreateUserWithRole(userManager, "Ahmed@gmail.com", "Mohamed#9475", Roles.JobSeeker);

			await CreateUserWithRole(userManager, "Ibrahim@gmail.com", "Mohamed#9475", Roles.Employeer);


		}
		private static async Task CreateUserWithRole(
			UserManager<IdentityUser> userManager,
			string email,
			string password,
			string role)
		{
			if (await userManager.FindByEmailAsync(email) == null)
			{
				var user = new IdentityUser
				{
					Email = email
					,
					EmailConfirmed = true,
					UserName = email
				};
				var result = await userManager.CreateAsync(user,password);
				if ( result.Succeeded)
				{
					await userManager.AddToRoleAsync(user, Roles.Admin);
				}
				else
				{
					
					throw new Exception($"Fialed To register user : {string.Join(",",result.Errors)}");
				}
			}
		}

	}
}
