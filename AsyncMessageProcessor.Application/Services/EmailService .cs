using Microsoft.Extensions.Configuration;
using System;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
using AsyncMessageProcessor.Domain.Entities;

namespace AsyncMessageProcessor.Application.Services
{
    public interface IEmailService
    {
        Task SendEmailAsync(EmailRequest emailRequest);
    }

    public class EmailService : IEmailService
    {
        private readonly IConfiguration _configuration;

        public EmailService(IConfiguration configuration)
        {
            _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
        }

        public async Task SendEmailAsync(EmailRequest emailRequest)
        {
            // Validar correo destinatario
            ValidEmailRequest(emailRequest);

            // Validar configuración SMTP
            string host;
            int port;
            ValidConfigurationSMTP(out host, out port);
            bool enableSsl = bool.Parse(_configuration["Smtp:EnableSsl"] ?? "false");
            var username = _configuration["Smtp:Username"] ?? throw new ArgumentException("Ingrese el Username");
            var password = _configuration["Smtp:Password"] ?? throw new ArgumentException("Ingrese el Password");
            try
            {
                // Crear cliente SMTP
                using var smtpClient = new SmtpClient
                {
                    Host = host,
                    Port = port,
                    EnableSsl = enableSsl,
                    UseDefaultCredentials = false,
                    Credentials = new NetworkCredential(
                      username,
                        password
                    )
                };

                // Crear correo electrónico
                using var mailMessage = new MailMessage
                {
                    From = new MailAddress(username),
                    Subject = emailRequest.Subject,
                    Body = emailRequest.Body,
                    IsBodyHtml = true
                };

                // Agregar destinatarios
                string[] toAddresses = emailRequest.To.Split(new[] { ',', ';' }, StringSplitOptions.RemoveEmptyEntries);
                foreach (var toAddress in toAddresses)
                {
                    mailMessage.To.Add(toAddress.Trim());
                }

                // Agregar destinatarios en copia
                if (emailRequest.CC != null)
                {
                    foreach (var ccAddress in emailRequest.CC)
                    {
                        mailMessage.CC.Add(ccAddress.Trim());
                    }
                }

                // Agregar adjuntos
                if (emailRequest.Attachment != null)
                {
                    foreach (var attachment in emailRequest.Attachment)
                    {
                        var attachmentStream = new System.IO.MemoryStream(attachment.File);
                        mailMessage.Attachments.Add(new Attachment(attachmentStream, attachment.FileName, attachment.ContentType));
                    }
                }

                // Enviar correo electrónico
                await smtpClient.SendMailAsync(mailMessage);
            }
            catch (Exception ex)
            {
                // Loguear el error
                Console.WriteLine($"Error al enviar correo electrónico: {ex.Message}");
                // Puedes relanzar la excepción si deseas propagarla más arriba
                throw;
            }
        }

        private void ValidConfigurationSMTP(out string host, out int port)
        {
           var hostname = _configuration["Smtp:Host"];
            if (string.IsNullOrEmpty(hostname) || !int.TryParse(_configuration["Smtp:Port"], out port))
            {
                throw new ArgumentException("La configuración SMTP es inválida.");
            }
            host = hostname;
        }

        private static void ValidEmailRequest(EmailRequest emailRequest)
        {
            if (string.IsNullOrEmpty(emailRequest.To))
            {
                throw new ArgumentException("El campo 'To' es obligatorio.");
            }
        }
    }
}
