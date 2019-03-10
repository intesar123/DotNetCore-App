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
using MyCoreAppAuth.Data;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MySql.Data.EntityFrameworkCore.Extensions;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System.IdentityModel.Tokens.Jwt;

namespace MyCoreAppAuth
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
            #region Authentication
            services.AddIdentity<IdentityUser, IdentityRole>().AddEntityFrameworkStores<ApplicationDbContext>().AddDefaultTokenProviders();

            //jwt
            JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear(); // => remove default claims
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = Configuration["Jwt:Issuer"],
                    ValidAudience = Configuration["Jwt:Issuer"],
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["Jwt:Key"]))
                };
               });
                ////////////services.AddAuthentication(options =>
                ////////////    {
                ////////////        options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                ////////////        options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
                ////////////        options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;

                ////////////    }).AddJwtBearer(cfg =>
                ////////////    {
                ////////////        cfg.RequireHttpsMetadata = false;
                ////////////        cfg.SaveToken = true;
                ////////////        cfg.TokenValidationParameters = new TokenValidationParameters
                ////////////        {
                ////////////            ValidIssuer = Configuration["JwtIssuer"],
                ////////////            ValidAudience = Configuration["JwtIssuer"],
                ////////////            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["JwtKey"])),
                ////////////            ClockSkew = TimeSpan.Zero // remove delay of token when expire
                ////////////        };
                ////////////    });
                #endregion
                services.Configure<CookiePolicyOptions>(options =>
            {
                // This lambda determines whether user consent for non-essential cookies is needed for a given request.
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });
            /**************************Sql ServerConnection **************************/
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(
                    Configuration.GetConnectionString("AppConn")));

            /**************************MySql Server Connection **************************/
            //Install-Package MySql.Data.EntityFrameworkCore -Version 8.0.13
            //services.AddDbContext<ApplicationDbContext>(options =>
            //    options.UseMySQL(
            //        Configuration.GetConnectionString("DefaultConnection")));


            //////////////services.AddDefaultIdentity<IdentityUser>()
            //////////////    .AddDefaultUI(UIFramework.Bootstrap4)
            //////////////    .AddEntityFrameworkStores<ApplicationDbContext>();
            

            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
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
                app.UseHsts();
            }
           /*********************************************Custom Middlewares****************************************************/
            //app.Use(async (context, next) => {
            //    //login to perform on request
            //    await context.Response.WriteAsync("<p>Hello Middleware 1</p>");
            //    await next();//--------------- it is used to transfer request control to next middlewares
            //    //login to perform on response
            //});
            //app.Use(async (context, next) => {
            //    //login to perform on request
            //    await context.Response.WriteAsync("<p>Hello Middleware 2</p>");
            //    await next();
            //    //login to perform on response
            //});
            /*********************************************End Custom Middlewares****************************************************/
            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseCookiePolicy();
            app.UseAuthentication();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
                routes.MapRoute(
                    name: "Login",
                    template: "{controller=Account}/{action=Login}");
                routes.MapRoute(
                    name: "Register",
                    template: "{controller=Account}/{action=Register}");
                routes.MapRoute(
                    name: "Logout",
                    template: "{controller=Account}/{action=Logout}");
            });

        }
    }
}
