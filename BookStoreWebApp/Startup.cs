using BookStoreWebApp.Helper;
using BookStoreWebApp.Models;
using BookStoreWebApp.Repository;
using BookStoreWebApp.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookStoreWebApp
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
            services.AddControllersWithViews();
            //配置数据库链接
            services.AddDbContextPool<MyDbContext>(
                options => options.UseSqlServer(Configuration.GetConnectionString("TestDbConnection")));
            //配置Identity的功能
            //添加默认的Token功能
            services.AddIdentity<IdentityUser, IdentityRole>().AddEntityFrameworkStores <MyDbContext> ().AddDefaultTokenProviders();


            services.AddScoped<IAccountRepository, AccountRepository>();
            services.AddScoped<IUserService, UserService>();
            //services.AddHttpContextAccessor();
            //services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            //配置未登录时无法访问的页面，重定向的地址
            services.ConfigureApplicationCookie(config =>
            {
                config.LoginPath = "/login";
            });
            //配置ClaimsIdentity，让登录后显示特定的信息（比如手机号，用户名等）
            services.AddScoped<IUserClaimsPrincipalFactory<IdentityUser>, MyUserClaimsPrincipalFactory>();
            //配置SMTP邮件服务
            services.Configure<SMTPConfigModel>(Configuration.GetSection("SMTPConfig"));
            services.AddScoped<IEmailService, EmailService>();
            //配置用户密码强度和登录许可
            services.Configure<IdentityOptions>(options=> 
            {
                options.Password.RequiredLength = 1;
                options.Password.RequiredUniqueChars = 1;
                //是否要求数字
                options.Password.RequireDigit = false;
                //是否要求小写字母
                options.Password.RequireLowercase = false;
                //是否要求特殊符号
                options.Password.RequireNonAlphanumeric = false;
                //是否要求大写字母
                options.Password.RequireUppercase = false;

                //设置登录许可
                options.SignIn.RequireConfirmedEmail = true;
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
