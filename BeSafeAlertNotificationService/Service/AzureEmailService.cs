using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Azure;
using Azure.Communication.Email;
using Microsoft.Extensions.Configuration;

namespace AlertNotificationService.Services
{
    public class AzureEmailService
    {
        private readonly EmailClient _emailClient;
        private readonly string _senderAddress;

        public AzureEmailService(string connectionString, string senderAddress)
        {
            _emailClient = new EmailClient(connectionString);
            _senderAddress = senderAddress;
        }
        
        public async Task SendEmailsAsync(List<(string email, string name)> recipients, string subject, string htmlContent)
        {
            foreach (var (email, name) in recipients)
            {
                var emailMessage = new EmailMessage(
                    senderAddress: _senderAddress,
                    content: new EmailContent(subject)
                    {
                        Html = htmlContent
                    },
                    recipients: new EmailRecipients(new List<EmailAddress>
                    {
                        new EmailAddress(email, name) 
                    })
                );

                try
                {
                    EmailSendOperation operation = await _emailClient.SendAsync(WaitUntil.Completed, emailMessage);
                    Console.WriteLine($"✅ Email enviado para {name} ({email}) | Status: {operation.Value.Status}");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"❌ Falha ao enviar e-mail para {name} ({email}) | Erro: {ex.Message}");
                }
            }
        }
    }
}