using System;
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
using MuQuiz.Models.Entities;

namespace MuQuiz
{
    public class Startup
    {
        public IConfiguration Configuration { get; }
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            var connString = Configuration.GetConnectionString("MuQuizConnString");
            services.AddDbContext<MuquizContext>(options => options.UseSqlServer(connString));
            services.AddDbContext<MyIdentityContext>(options => options.UseSqlServer(connString));
            services.AddIdentity<MyIdentityUser, IdentityRole>(o =>
                {
                    o.Password.RequireNonAlphanumeric = true;
                    o.Password.RequiredLength = 6;
                })
                .AddEntityFrameworkStores<MyIdentityContext>()
                .AddDefaultTokenProviders();
            //Standard inloggningssida är nu /account/login

            services.AddMvc();
            services.AddSignalR();

            services.AddSession();
            services.AddHttpContextAccessor();
            services.AddAuthentication(
                CookieAuthenticationDefaults.AuthenticationScheme)
                .AddCookie(o => o.LoginPath = "/Account/Login");

            services.AddSingleton<SpotifyService>();
            services.AddSingleton<IConfiguration>(Configuration);
            services.AddTransient<AccountService>();
            services.AddTransient<SessionStorageService>();
            services.AddTransient<QuestionService>();
            services.AddTransient<GameService>();
            services.AddTransient<AdminService>();
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
                app.UseDeveloperExceptionPage();
            else
            {
                app.UseExceptionHandler("/Error/ServerError");
                //app.UseHsts();
            }

            app.UseStatusCodePagesWithReExecute("/Error/HttpError/{0}");
            //app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseAuthentication();
            app.UseSession();
            app.UseSignalR(routes => routes.MapHub<GameHub>("/gamehub"));
            app.UseMvcWithDefaultRoute();
        }
    }
}
