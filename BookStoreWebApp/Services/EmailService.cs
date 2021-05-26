using BookStoreWebApp.Models;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace BookStoreWebApp.Services
{
    public class EmailService : IEmailService
    {
        private const string templatePath = @"EmailTemplates/{0}.html";
        private readonly SMTPConfigModel _smtpConfig;

        public EmailService(IOptions<SMTPConfigModel> smtpConfig)
        {
            _smtpConfig = smtpConfig.Value;
        }

        //public async Task SendTestEmail(UserEmailOptions userEmailOptions)
        //{
        //    userEmailOptions.Subject = "This is test email subject.";
        //    userEmailOptions.Body = GetEmailBody("EmailTemplate1");
        //    await SendEmail(userEmailOptions);
        //}

        //发送自定义模板的邮件
        public async Task SendTestEmail()
        {
            List<KeyValuePair<string, string>> placeHolders = new List<KeyValuePair<string, string>>()
            {
                new KeyValuePair<string, string>("{{UserName}}","Tomson"),
                new KeyValuePair<string, string>("{{UserEmail}}","tomson@gmail.com"),
            };
            //配置邮件发送
            UserEmailOptions userEmailOptions = new UserEmailOptions()
            {
                Subject = "This is test email subject.",
                //Body = GetEmailBody("EmailTemplate1"),
                Body = updatePlaceHolder(GetEmailBody("EmailTemplate1"), placeHolders),
                ToEmails = new List<string>() { "test@hotmail.com" }
            };

            await SendEmail(userEmailOptions);
        }

        //发送邮箱验证邮件
        public async Task SendEmailConfirmationEmail(string username, string userEmail, string confirmLink)
        {
            List<KeyValuePair<string, string>> placeHolders = new List<KeyValuePair<string, string>>()
            {
                new KeyValuePair<string, string>("{{UserName}}",username),
                new KeyValuePair<string, string>("{{ConfirmationLink}}",confirmLink),
            };
            UserEmailOptions userEmailOptions = new UserEmailOptions
            {
                Subject="Email Confirmation",
                Body=updatePlaceHolder(GetEmailBody("EmailConfirm"),placeHolders),
                ToEmails=new List<string>() {userEmail}
            };
            await SendEmail(userEmailOptions);
        }

        //发送密码重置的链接邮件
        public async Task SendForgotPasswordEmail(string username, string userEmail, string confirmLink)
        {
            List<KeyValuePair<string, string>> placeHolders = new List<KeyValuePair<string, string>>()
            {
                new KeyValuePair<string, string>("{{UserName}}",username),
                new KeyValuePair<string, string>("{{Link}}",confirmLink),
            };
            UserEmailOptions userEmailOptions = new UserEmailOptions
            {
                Subject = "Reset Password",
                Body = updatePlaceHolder(GetEmailBody("EmailForgotPassword"), placeHolders),
                ToEmails = new List<string>() { userEmail }
            };
            await SendEmail(userEmailOptions);
        }

        //发送邮件
        private async Task SendEmail(UserEmailOptions userEmailOptions)
        {
            MailMessage mail = new MailMessage
            {
                Subject = userEmailOptions.Subject,
                Body = userEmailOptions.Body,
                From = new MailAddress(_smtpConfig.SenderAddress, _smtpConfig.SenderDisplayName),
                IsBodyHtml = _smtpConfig.IsBodyHTML,
                BodyEncoding = Encoding.UTF8
            };
            //向邮件中添加发送对象
            foreach (string toEmail in userEmailOptions.ToEmails)
            {
                mail.To.Add(toEmail);
            }
            //邮件凭证
            NetworkCredential networkCredential = new NetworkCredential(_smtpConfig.UserName, _smtpConfig.Password);

            SmtpClient smtpClient = new SmtpClient
            {
                Host = _smtpConfig.Host,
                Port = _smtpConfig.Port,
                EnableSsl = _smtpConfig.EnableSSL,
                UseDefaultCredentials = _smtpConfig.UseDefualtCredentials,
                Credentials = networkCredential
            };

            await smtpClient.SendMailAsync(mail);
        }

        //获取邮件的Body，本地的一个html文件
        private string GetEmailBody(string templateName)
        {
            string path = string.Format(templatePath, templateName);
            string body = File.ReadAllText(path);
            return body;
        }

        //替换邮件模板中的内容
        private string updatePlaceHolder(string emailBody, List<KeyValuePair<string, string>> keyValuePairs)
        {
            if (!string.IsNullOrEmpty(emailBody) && keyValuePairs != null)
            {
                foreach (KeyValuePair<string, string> pair in keyValuePairs)
                {
                    if (emailBody.Contains(pair.Key))
                    {
                        emailBody = emailBody.Replace(pair.Key, pair.Value);
                    }
                }
            }
            return emailBody;
        }
    }
}
