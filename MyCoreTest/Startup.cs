using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace MyCoreTest
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
            InMemoryConfiguration.Configuration = this.Configuration;

            services.AddIdentityServer()
                .AddDeveloperSigningCredential()
                .AddInMemoryIdentityResources(InMemoryConfiguration.GetIdentityResources())
                .AddTestUsers(InMemoryConfiguration.GetUsers().ToList())
                .AddInMemoryClients(InMemoryConfiguration.GetClients())
                .AddInMemoryApiResources(InMemoryConfiguration.GetApiResources());


            services.Configure<CookiePolicyOptions>(options =>
            {
                // This lambda determines whether user consent for non-essential cookies is needed for a given request.
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });

            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            #region MyRegion
            //// 登录
            //app.Map("/login", builder => builder.Use(next =>
            //{
            //    return async (context) =>
            //    {
            //        var claimIdentity = new ClaimsIdentity();
            //        claimIdentity.AddClaim(new Claim(ClaimTypes.Name, "jim"));
            //        await context.SignInAsync("myScheme", new ClaimsPrincipal(claimIdentity));
            //    };
            //}));

            //// 退出
            //app.Map("/logout", builder => builder.Use(next =>
            //{
            //    return async (context) =>
            //    {
            //        await context.SignOutAsync("myScheme");
            //    };
            //}));

            //// 认证
            //app.Use(next =>
            //{
            //    return async (context) =>
            //    {
            //        var result = await context.AuthenticateAsync("myScheme");
            //        if (result?.Principal != null) context.User = result.Principal;
            //        await next(context);
            //    };
            //});

            //// 授权
            //app.Use(async (context, next) =>
            //{
            //    var user = context.User;
            //    if (user?.Identity?.IsAuthenticated ?? false)
            //    {
            //        if (user.Identity.Name != "jim") await context.ForbidAsync("myScheme");
            //        else await next();
            //    }
            //    else
            //    {
            //        await context.ChallengeAsync("myScheme");
            //    }
            //});

            //// 访问受保护资源
            //app.Map("/resource", builder => builder.Run(async (context) => await context.Response.WriteAsync("Hello, ASP.NET Core!")));

            #endregion

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseCookiePolicy();

            //app.UseAuthentication();
            app.UseIdentityServer();
            

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
