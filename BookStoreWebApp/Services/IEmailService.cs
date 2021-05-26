using BookStoreWebApp.Models;
using System.Threading.Tasks;

namespace BookStoreWebApp.Services
{
    public interface IEmailService
    {
        Task SendEmailConfirmationEmail(string username, string userEmail, string confirmLink);
        Task SendForgotPasswordEmail(string username, string userEmail, string confirmLink);
        Task SendTestEmail();
    }
}