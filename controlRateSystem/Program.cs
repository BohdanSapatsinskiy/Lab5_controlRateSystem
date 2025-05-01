using Microsoft.EntityFrameworkCore;
using controlRateSystem.Models;
using Microsoft.AspNetCore.Identity;


using controlRateSystem.Data.Models;
using controlRateSystem.Data.Data;
using controlRateSystem.Data.Repositories;

namespace controlRateSystem
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddDistributedMemoryCache();
            builder.Services.AddSession(options =>
            {
                options.Cookie.IsEssential = true;
                options.Cookie.HttpOnly = true;
                options.Cookie.SameSite = SameSiteMode.Lax;
                options.Cookie.MaxAge = null;
            });

            builder.Services.AddControllersWithViews();

			builder.Services.AddDbContext<RatesDbContext>(opts => {
				opts.UseSqlServer(builder.Configuration["ConnectionStrings:RateSystemConnection"]);
			});

			builder.Services.AddScoped<IRatesRepository, EFRatesRepository>();


            builder.Services.AddDbContext<AppIdentityDbContext>(options =>
                options.UseSqlServer(builder.Configuration["ConnectionStrings:IdentityConnection"])
            );
            builder.Services.AddIdentity<IdentityUser, IdentityRole>()
                .AddEntityFrameworkStores<AppIdentityDbContext>();
            
            builder.Services.Configure<IdentityOptions>(options =>
            {
                options.User.RequireUniqueEmail = true;

                options.Password.RequiredLength = 8;
                options.Password.RequireDigit = true;
                options.Password.RequireLowercase = true;
                options.Password.RequireUppercase = true;
                options.Password.RequireNonAlphanumeric = false;
            });



            var app = builder.Build();

            app.UseSession();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseStaticFiles();
            app.MapDefaultControllerRoute();

			SeedData.EnsurePopulated(app);

			app.Run();
        }
    }
}
