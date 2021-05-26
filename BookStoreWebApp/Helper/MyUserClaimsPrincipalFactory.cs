using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace BookStoreWebApp.Helper
{
    //继承的类，第一个参数为User类型，第二个参数为Role类型
    public class MyUserClaimsPrincipalFactory : UserClaimsPrincipalFactory<IdentityUser, IdentityRole>
    {
        //根据代码提示，自动创建构造函数。
        //父类的构造函数需要三个参数，子类也必须实现。
        public MyUserClaimsPrincipalFactory(UserManager<IdentityUser> userManager, 
            RoleManager<IdentityRole> roleManager, 
            IOptions<IdentityOptions> options) 
            : base(userManager, roleManager, options)
        {
        }

        //重写父类中的该方法
        protected override async Task<ClaimsIdentity> GenerateClaimsAsync(IdentityUser user)
        {
            //ClaimsIdentity相当于一张身份证
            ClaimsIdentity claimsIdentity = await base.GenerateClaimsAsync(user);
            //相当于身份证中的一条信息（本质上可以理解为一组键值对）
            Claim claim = new Claim("PhoneNumber", user.PhoneNumber ?? "");
            //向身份证中添加信息
            claimsIdentity.AddClaim(claim);
            return claimsIdentity;
        }
    }
}
