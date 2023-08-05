using System.Reflection;

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;

using VAndDCargoes.Data.Models;
using static VAndDCargoes.Common.GeneralConstants;

namespace VAndDCargoes.Web.Infrastructure.Extentions;

public static class WebApplicationBuilderExtentions
{
    /// <summary>
    /// Adding services automatically via reflection.
    /// </summary>
    public static void AddApplicationServices(this IServiceCollection services, Type serviceType)
	{
		Assembly? serviceAssembly = Assembly.GetAssembly(serviceType);
		if (serviceAssembly == null)
		{
			throw new InvalidOperationException("Invalid service type provided!");
		}

		IEnumerable<Type> serviceTypes = serviceAssembly
			.GetTypes()
			.Where(x => x.Name.EndsWith("Service") && !x.IsInterface);

		foreach (var implementationType in serviceTypes)
		{
			Type? interfaceType = implementationType.GetInterface($"I{implementationType.Name}");
			if (interfaceType == null)
			{
				throw new InvalidOperationException("Invalid service contract type provided!");
			}

			services.AddScoped(interfaceType, implementationType);
		}
	}

    public static IApplicationBuilder SeedAdministrator(this IApplicationBuilder app, string email)
    {
        using IServiceScope scopedServices = app.ApplicationServices.CreateScope();

        IServiceProvider serviceProvider = scopedServices.ServiceProvider;

        UserManager<ApplicationUser> userManager = serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();

        RoleManager<IdentityRole<Guid>> roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole<Guid>>>();

        Task.Run(async () =>
        {
            if (await roleManager.RoleExistsAsync(AdminRoleName))
            {
                return;
            }

            IdentityRole<Guid> role =
                new IdentityRole<Guid>(AdminRoleName);

            await roleManager.CreateAsync(role);

            ApplicationUser adminUser =
                await userManager.FindByEmailAsync(email);

            await userManager.AddToRoleAsync(adminUser, AdminRoleName);
        })
        .GetAwaiter()
        .GetResult();

        return app;
    }

    public static IApplicationBuilder SeedSpecialist(this IApplicationBuilder app, string email)
    {
        using IServiceScope scopedServices = app.ApplicationServices.CreateScope();

        IServiceProvider serviceProvider = scopedServices.ServiceProvider;

        UserManager<ApplicationUser> userManager = serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();

        RoleManager<IdentityRole<Guid>> roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole<Guid>>>();

        Task.Run(async () =>
        {
            if (await roleManager.RoleExistsAsync(SpecialistRoleName))
            {
                return;
            }

            IdentityRole<Guid> role =
                new IdentityRole<Guid>(SpecialistRoleName);

            await roleManager.CreateAsync(role);

            ApplicationUser specialistUser =
                await userManager.FindByEmailAsync(email);

            await userManager.AddToRoleAsync(specialistUser, SpecialistRoleName);
        })
        .GetAwaiter()
        .GetResult();

        return app;
    }
}

 