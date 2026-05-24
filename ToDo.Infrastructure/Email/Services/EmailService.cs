using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using MailKit.Security;
using MimeKit.Text;
using MimeKit;
using MailKit.Net.Smtp;
using ToDo.Application.Abstractions.Email.Services;
using ToDo.Application.Abstractions.Email.DTOs;

namespace ToDo.Infrastructure.Email.Services
{
    public class EmailService : IEmailService
    {
        private readonly IConfiguration _configuration;

        // Email Settings
        private readonly string _host;
        private readonly int _port;
        private readonly string _from;
        private readonly string _email;
        private readonly string _password;

        public EmailService(IConfiguration configuration)
        {
            _configuration = configuration;
            _host = _configuration["EmailSettings:Host"]!;
            _port = Convert.ToInt32(_configuration["EmailSettings:Port"]);
            _from = _configuration["EmailSettings:From"]!;
            _email = _configuration["EmailSettings:UserName"]!;
            _password = _configuration["EmailSettings:Password"]!;
        }

        public async Task SendAsync(EmailMessageDTO emailMessage)
        {
            var message = new MimeMessage
            {
                Subject = emailMessage.Subject,
                Body = new TextPart(emailMessage.IsHtml ? TextFormat.Html : TextFormat.Plain)
                {
                    Text = emailMessage.Body,
                }
            };
            message.To.Add(new MailboxAddress("", emailMessage.To));
            message.From.Add(new MailboxAddress("ToDo App", _from));

            using var smtpClient = new SmtpClient();
            await smtpClient.ConnectAsync(_host, _port, SecureSocketOptions.StartTls);

            await smtpClient.AuthenticateAsync(_email, _password);

            await smtpClient.SendAsync(message);
            await smtpClient.DisconnectAsync(true);
        }
    }
}
