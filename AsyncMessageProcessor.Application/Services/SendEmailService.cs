using AsyncMessageProcessor.Application.Interfaces;
using AsyncMessageProcessor.Domain.Entities;

using System.Text.Json;


namespace AsyncMessageProcessor.Application.Services
{
    internal class SendEmailService : ISendEmailService
    {
        private readonly IRabbitMQManager _rabbitMQManager;
        private readonly IEmailService _emailService;

        public SendEmailService(IRabbitMQManager rabbitMQManager, IEmailService emailService)
        {
            _rabbitMQManager = rabbitMQManager;
            _emailService = emailService;
        }

        public   Task SendEmailAsync(CancellationToken cancellationToken)
        {
            _rabbitMQManager.ConsumeMessages("email_queue", async (mensaje) =>
            {
                var email = JsonSerializer.Deserialize<EmailRequest>(mensaje);
                if (email == null)
                {
                    return;
                }

                try
                {
                    await _emailService.SendEmailAsync(email);
                    // Aquí puedes procesar el objeto de email y enviar el correo electrónico
                }
                catch (Exception ex)
                {
                    // Loguear el error
                    Console.WriteLine($"Error al enviar correo electrónico: {ex.Message}");

                    // Devolver el mensaje a la cola
                    _rabbitMQManager.SendMessage("email_queue", mensaje);
                }
            });
            return Task.CompletedTask;
        }
    }

}
