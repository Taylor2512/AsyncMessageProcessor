using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AsyncMessageProcessor.Application.Interfaces
{
    public interface ISendEmailService
    {
        Task SendEmailAsync(CancellationToken cancellationToken);
    }
}
