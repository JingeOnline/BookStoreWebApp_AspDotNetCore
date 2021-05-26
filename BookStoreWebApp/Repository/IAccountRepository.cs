using BookStoreWebApp.Models;
using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;

namespace BookStoreWebApp.Repository
{
    public interface IAccountRepository
    {
        Task<IdentityResult> ChangePasswordAsync(ChangePasswordModel model);
        Task<IdentityResult> ConfirmEmailAsync(string uid, string token);
        Task<IdentityResult> CreateUserAsync(SignUpUserModel userModel);
        Task<IdentityUser> GetUserByEmail(string email);
        Task<IdentityResult> ResetPasswordAsync(ResetPasswordModel model);
        Task SendForgotPasswordEmail(IdentityUser user);
        Task<SignInResult> SignInAsync(SignInModel signInModel);
        Task SignOutAsync();
    }
}