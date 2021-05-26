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
            //�������ݿ�����
            services.AddDbContextPool<MyDbContext>(
                options => options.UseSqlServer(Configuration.GetConnectionString("TestDbConnection")));
            //����Identity�Ĺ���
            //���Ĭ�ϵ�Token����
            services.AddIdentity<IdentityUser, IdentityRole>().AddEntityFrameworkStores <MyDbContext> ().AddDefaultTokenProviders();


            services.AddScoped<IAccountRepository, AccountRepository>();
            services.AddScoped<IUserService, UserService>();
            //services.AddHttpContextAccessor();
            //services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            //����δ��¼ʱ�޷����ʵ�ҳ�棬�ض���ĵ�ַ
            services.ConfigureApplicationCookie(config =>
            {
                config.LoginPath = "/login";
            });
            //����ClaimsIdentity���õ�¼����ʾ�ض�����Ϣ�������ֻ��ţ��û����ȣ�
            services.AddScoped<IUserClaimsPrincipalFactory<IdentityUser>, MyUserClaimsPrincipalFactory>();
            //����SMTP�ʼ�����
            services.Configure<SMTPConfigModel>(Configuration.GetSection("SMTPConfig"));
            services.AddScoped<IEmailService, EmailService>();
            //�����û�����ǿ�Ⱥ͵�¼���
            services.Configure<IdentityOptions>(options=> 
            {
                options.Password.RequiredLength = 1;
                options.Password.RequiredUniqueChars = 1;
                //�Ƿ�Ҫ������
                options.Password.RequireDigit = false;
                //�Ƿ�Ҫ��Сд��ĸ
                options.Password.RequireLowercase = false;
                //�Ƿ�Ҫ���������
                options.Password.RequireNonAlphanumeric = false;
                //�Ƿ�Ҫ���д��ĸ
                options.Password.RequireUppercase = false;

                //���õ�¼���
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
