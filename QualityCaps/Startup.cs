using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using QualityCaps.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.FileProviders;
using System.IO;
using QualityCaps.Models;
using QualityCaps.Services;
using Microsoft.AspNetCore.Rewrite;

namespace QualityCaps
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            //use for upload file
            services.AddSingleton<IFileProvider>(
                new PhysicalFileProvider(
                    Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\images\\upload")));
   
            //get connection string
          //  services.AddDbContext<CapContext>(options => options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));   

            services.AddDbContext<ApplicationDbContext>(options =>options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));

            services.AddIdentity<ApplicationUser, IdentityRole>(config =>
            {
                config.SignIn.RequireConfirmedEmail = true;
            })
            .AddEntityFrameworkStores<ApplicationDbContext>()
            .AddDefaultTokenProviders();

            // Add application services.
            services.AddTransient<IEmailSender, EmailSender>();

            services.AddMvc();

            //enable sessions
            services.AddSession(options => {
                //set a short timeout for testing
                options.IdleTimeout = TimeSpan.FromMinutes(5);
                options.CookieHttpOnly = true;
            });
        }

        //changed at week 7 This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public async void Configure(IApplicationBuilder app, IHostingEnvironment env, 
            IServiceProvider serviceProvider, ApplicationDbContext apContext, UserManager<ApplicationUser> userManager)
        {
          //  CreateRoles(serviceProvider,apContext);

            app.UseSession();
            if (env.IsDevelopment())
            {
                app.UseBrowserLink();
                app.UseDeveloperExceptionPage();
                app.UseDatabaseErrorPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }

            app.UseStaticFiles();

            app.UseAuthentication();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
            CreateRoles(serviceProvider, apContext).Wait();
        }

        public async Task CreateRoles(IServiceProvider serviceProvider, ApplicationDbContext apContext)
        {
            try
            {
                DbInitializer.Initialize(apContext);
            }
            catch (Exception ex)
            {
                Console.WriteLine("WARNING: Database initial failed: " + ex.Message);
                return;
            }
            Console.WriteLine("INFO: Database is ready.");
            var catRecordCount = -1;
            try
            {
                catRecordCount = apContext.Categories.Count();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Database not ready: " + ex.Message);
                catRecordCount = -1;
            }

            if (catRecordCount < 0)
            {

                Console.WriteLine("WARNING: Database not ready: " + "can not initial user roles.");
                return;
            }
            Console.WriteLine("Info: Database ready: " + "try to initial user roles.");

            string[] roleNames = { "Admin", "Member" };

            foreach (var roleName in roleNames)
            {
                try
                {
                    var _roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
                    bool roleExist = _roleManager.RoleExistsAsync(roleName).Result;
                    if (!roleExist)
                    {
                        Console.WriteLine("creating role: " + roleName);
                        // must use new  _roleManager
                        _roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
                        IdentityResult roleResult;
                        roleResult = await _roleManager.CreateAsync(new IdentityRole(roleName));
                        Console.WriteLine("created role: " + roleName);
                    }
                    else
                    {
                        Console.WriteLine("role: " + roleName + " already existed.");
                    }
                }
                catch (System.AggregateException ex)
                {
                    Console.WriteLine("create role: " + roleName + " failed, message: " + ex.Message);
                }
            }

            var poweruser = new ApplicationUser
            {
                UserName = Configuration.GetSection("UserSettings")["UserEmail"],
                Email = Configuration.GetSection("UserSettings")["UserEmail"],
                Address = "Admin Address",
                Enabled = true,
                EmailConfirmed = true
            };
            string UserPassword = Configuration.GetSection("UserSettings")["UserPassword"];

            var _userManager = serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();
            var userExisted = await _userManager.FindByEmailAsync(poweruser.Email);

            if (userExisted == null)
            {
                // must use new _userManager
                _userManager = serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();
                var createPowerUser = await _userManager.CreateAsync(poweruser, UserPassword);

                if (createPowerUser.Succeeded)
                {
                    //tie the new user to the "admin" role
                    await _userManager.AddToRoleAsync(poweruser, "Admin");
                    Console.WriteLine("create admin user: " + poweruser + " ok.");
                }
                else
                {
                    Console.WriteLine("create admin user: " + poweruser + ", failed.");
                }
            }
            else
            {
                Console.WriteLine("admin user: " + poweruser + " already existed.");
            }
        }
    }
}
