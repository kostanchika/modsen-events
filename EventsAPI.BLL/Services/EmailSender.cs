﻿using Microsoft.Extensions.Configuration;
using MimeKit;
using MailKit.Net.Smtp;

namespace EventsAPI.BLL.Services
{
    public class EmailSender
    {
        private readonly IConfiguration _configuration;

        public EmailSender(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task SendEmailAsync(string email, string subject, string message, CancellationToken ct = default)
        {
            var emailMessage = new MimeMessage();

            emailMessage.From.Add(new MailboxAddress(_configuration["Email:Name"], _configuration["Email:From"]));
            emailMessage.To.Add(new MailboxAddress("", email));
            emailMessage.Subject = subject;
            emailMessage.Body = new TextPart(MimeKit.Text.TextFormat.Text) { Text = message };

            using (var client = new SmtpClient())
            {
                await client.ConnectAsync(_configuration["Email:SmtpServer"], int.Parse(_configuration["Email:Port"]), false, ct);
                await client.AuthenticateAsync(_configuration["Email:Username"], _configuration["Email:Password"], ct);
                await client.SendAsync(emailMessage, ct);
                await client.DisconnectAsync(true, ct);
            }
        }
    }
}
