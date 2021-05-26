using BookStoreWebApp.Models;
using BookStoreWebApp.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookStoreWebApp.Repository
{
    public class AccountRepository : IAccountRepository
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly IUserService _userService;
        private readonly IConfiguration _configuration;
        private readonly IEmailService _emailService;

        public AccountRepository(UserManager<IdentityUser> userManager, 
            SignInManager<IdentityUser> signInManager,
            IUserService userService, 
            IConfiguration configuration,
            IEmailService emailService)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _userService = userService;
            _configuration = configuration;
            _emailService = emailService;
        }
        //创建用户
        public async Task<IdentityResult> CreateUserAsync(SignUpUserModel userModel)
        {

            IdentityUser user = new IdentityUser()
            {
                Email = userModel.Email,
                UserName = userModel.Email,
            };

            //该方法有两个重载
            //Task<IdentityResult>CreateAsync(TUser user)
            //Task<IdentityResult>CreateAsync(Tuser user, string password)
            IdentityResult result = await _userManager.CreateAsync(user, userModel.Password);

            //如果账户创建成功，生成token，让用户去验证邮件
            if (result.Succeeded)
            {
                string token =await _userManager.GenerateEmailConfirmationTokenAsync(user);
                if (!string.IsNullOrEmpty(token))
                {
                    await SendEmailConfirmationEmail(user, token);
                }
            }
            return result;
        }

        //当用户注册时，发送邮件，验证用户的邮箱地址
        private async Task SendEmailConfirmationEmail(IdentityUser user,string token)
        {
            string appDomain = _configuration.GetSection("Application:AppDomain").Value;
            string confirmLink = _configuration.GetSection("Application:EmailConfirmation").Value;
            string link = string.Format(appDomain + confirmLink, user.Id,token);
            await _emailService.SendEmailConfirmationEmail(user.UserName, user.Email, link);
        }

        //用户登录
        public async Task<SignInResult> SignInAsync(SignInModel signInModel)
        {
            //该方法有两种重载
            //第三个参数是是否允许用户关闭浏览器后保存登录状态的cookie
            //第四个参数是是否当用户登录失败的时候，锁定该账户
            //Task<SignInResult> PasswordSignInAsync(string userName, string password, bool isPersistent, bool lockoutOnFailure);
            //Task<SignInResult> PasswordSignInAsync(TUser user, string password, bool isPersistent, bool lockoutOnFailure);
            SignInResult result = await _signInManager.PasswordSignInAsync(signInModel.Email, signInModel.Password, signInModel.RememberMe, false);
            return result;
        }
        //用户登出
        public async Task SignOutAsync()
        {
            await _signInManager.SignOutAsync();
        }

        //用户修改密码
        public async Task<IdentityResult> ChangePasswordAsync(ChangePasswordModel model)
        {
            string userId = _userService.GetUserId();
            IdentityUser user =await _userManager.FindByIdAsync(userId);
            //第一个参数是user，第二个参数旧密码，第三个参数是新密码
            return await _userManager.ChangePasswordAsync(user,model.CurrentPassword,model.NewPassword);
        }

        //执行邮箱验证，验证UID和TOKEN是不是和发出去的链接一致
        public async Task<IdentityResult> ConfirmEmailAsync(string uid,string token)
        {
            IdentityUser user =await _userManager.FindByIdAsync(uid);
            return await _userManager.ConfirmEmailAsync(user, token);
        }

        //当用户忘记密码时，发送重置链接给用户
        public async Task SendForgotPasswordEmail(IdentityUser user)
        {
            string token = await _userManager.GeneratePasswordResetTokenAsync(user);
            if (string.IsNullOrEmpty(token))
            {
                return;
            }
            string appDomain = _configuration.GetSection("Application:AppDomain").Value;
            string confirmLink = _configuration.GetSection("Application:EmailForgotPassword").Value;
            string link = string.Format(appDomain + confirmLink, user.Id, token);
            await _emailService.SendForgotPasswordEmail(user.UserName, user.Email, link);
        }

        //根据用户邮箱返回用户
        public async Task<IdentityUser> GetUserByEmail(string email)
        {
            return await _userManager.FindByEmailAsync(email);
        }

        //验证用户提交的修改密码
        public async Task<IdentityResult> ResetPasswordAsync(ResetPasswordModel model)
        {
            IdentityUser user =await _userManager.FindByIdAsync(model.UserId);
            return await _userManager.ResetPasswordAsync(user, model.Token, model.NewPassword);
        }
    }
}
