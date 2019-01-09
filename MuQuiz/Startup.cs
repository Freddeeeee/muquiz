﻿using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MuQuiz.Hubs;
using MuQuiz.Models;

namespace MuQuiz
{
    public class Startup
    {
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public IConfiguration Configuration { get; }
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddTransient<HostService>();
            //Skapa connectionString med hjälp av lokal secret
            var builder = new SqlConnectionStringBuilder(
                Configuration.GetConnectionString("MuQuizConnString"));
            builder.Password = Configuration["MuQuizDbPw"];
            var connString = builder.ConnectionString;

            services.AddDbContext<MyIdentityContext>(options => options.UseSqlServer(connString));
            services.AddIdentity<MyIdentityUser, IdentityRole>(o => 
                {
                    o.Password.RequireNonAlphanumeric = true;
                    o.Password.RequiredLength = 6;
                })
                .AddEntityFrameworkStores<MyIdentityContext>()
                .AddDefaultTokenProviders();
            //Standard inloggningssida är nu /account/login

            services.AddTransient<AccountService>();
            services.AddSignalR();
            services.AddMvc();
            services.AddSession();

            services.AddAuthentication(
                CookieAuthenticationDefaults.AuthenticationScheme)
                .AddCookie(o => o.LoginPath = "/Host/Login");
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
                app.UseDeveloperExceptionPage();
            else
                app.UseExceptionHandler("/Error/ServerError");

            app.UseAuthentication();
            app.UseSignalR(routes => routes.MapHub<GameHub>("/gamehub"));
            app.UseSession();
            app.UseStaticFiles();
            app.UseAuthentication();
            app.UseMvcWithDefaultRoute();
            app.UseStatusCodePagesWithRedirects("/Error/HttpError/{0}");
        }
    }
}
