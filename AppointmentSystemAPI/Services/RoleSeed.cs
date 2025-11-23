using EntityLayer.Concrete;
using Microsoft.AspNetCore.Identity;

namespace AppointmentSystemAPI.Services
{
    public class RoleSeed
    {

        public static async Task SeedAsync(IServiceProvider serviceProvider)
        {
            var roleManager = serviceProvider.GetRequiredService<RoleManager<Dt_ApplicationRole>>();

            string[] roles = { "Admin", "Editor", "User" };

            foreach (var role in roles)
            {
                if (!await roleManager.RoleExistsAsync(role))
                {
                    await roleManager.CreateAsync(new Dt_ApplicationRole { Name = role });
                }
            }
        }

    }
}
