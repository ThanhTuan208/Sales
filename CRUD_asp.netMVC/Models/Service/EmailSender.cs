using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.VisualBasic.FileIO;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.MinimalApi;
using MimeKit;
using SixLabors.ImageSharp.Processing.Processors;
using System.Configuration;
using System.Net.NetworkInformation;

namespace CRUD_asp.netMVC.Models.Service
{
    public class EmailSender : IEmailSender
    {
        private readonly string _smptServer;
        private readonly int _smptPort;
        private readonly string _smptUser;
        private readonly string _smptPass;

        /// Cau hinh smtp trong appsetting.json
        public EmailSender(IConfiguration configuration)
        {
            _smptServer = configuration["Smtp:Server"];
            _smptPort = int.Parse(configuration["Smtp:Port"]);
            _smptUser = configuration["Smtp:User"];
            _smptPass = configuration["Smtp:Pass"];
        }

        /// <summary>
        /// Phuong thuc gui email
        /// </summary>
        /// <param name="email"></param>
        /// <param name="subject"></param>
        /// <param name="message"></param>
        /// <returns></returns>
        public async Task SendEmailAsync(string email, string subject, string message)
        {
            var mailMessage = new MimeMessage();
            mailMessage.From.Add(new MailboxAddress("Admin Sales ", _smptUser));
            mailMessage.To.Add(new MailboxAddress("", email));
            mailMessage.Subject = subject;
            mailMessage.Body = new TextPart("html") { Text = message };

            // _smtpPort: Cổng SMTP(thường là 587 cho Gmail với TLS).
            // SecureSocketOptions.StartTls: Sử dụng kết nối bảo mật TLS để mã hóa dữ liệu khi gửi
            using (var client = new SmtpClient())
            {
                await client.ConnectAsync(_smptServer, _smptPort, SecureSocketOptions.StartTls);
                await client.AuthenticateAsync(_smptUser, _smptPass);
                await client.SendAsync(mailMessage);
                await client.DisconnectAsync(true);
            }
        }
    }
}
