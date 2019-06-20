using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using pickupsv2.Data;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using pickupsv2.Hubs;
using pickupsv2.Models;
using AspNet.Security.OpenId.Steam;
using Microsoft.AspNetCore.HttpOverrides;

namespace pickupsv2
{
    public class Startup
    {

        private readonly IHostingEnvironment _env;
        public Startup(IConfiguration configuration, IHostingEnvironment __env)
        {
            Configuration = configuration;
            _env = __env;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.Configure<CookiePolicyOptions>(options =>
            {
                // This lambda determines whether user consent for non-essential cookies is needed for a given request.
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });

            services
                .AddDbContext<PickupContext>
                    (item => item.UseSqlServer(Configuration.GetConnectionString("DefaultConnection"))
            );

            services.AddDefaultIdentity<IdentityUser>()
                .AddRoles<IdentityRole>()
                .AddDefaultUI(UIFramework.Bootstrap4)
                .AddEntityFrameworkStores<PickupContext>();
            services.AddTransient<UserManager<Player>>();
            services.AddTransient<PickupContext>();
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
            services.AddSignalR();
            services
                .AddAuthentication()
                .AddSteam( //TODO, implement this once the site is live
                    options =>
                        {
                            options.ApplicationKey = "99219D4659300FAE38AC15F0071C72AF";
                            options.UserInformationEndpoint = "https://zapickups.dedicated.co.za/Steam/SaveSteamDetails";
                            //options.UserInformationEndpoint = "http://requestbin.fullcontact.com/19vp4c61";
                        }
                );            
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseDatabaseErrorPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
		app.UseForwardedHeaders(new ForwardedHeadersOptions
    		{
	        	ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto
	    	});
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseCookiePolicy();

            app.UseAuthentication();
            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
            app.UseSignalR(routes =>
            {
                routes.MapHub<PickupHub>("/PickupHub");
            });
        }
    }
}
