using Quartz;
using System.Net.Mail;
using System.Net;
using NotificationService.DataAccess.Http.Interfaces;
using Microsoft.Extensions.Configuration;

namespace NotificationService.Notification.Jobs
{
    public class EmailSender : IJob
    {
        private readonly string _senderEmail;
        private readonly string _senderPassword;
        private readonly string _host;

        private readonly ITaskAccess _taskRepository;
        private readonly IManagementAccess _managementRepository;

        public EmailSender(ITaskAccess taskRepository, IManagementAccess managementRepository, IConfiguration configuration)
        {
            _taskRepository = taskRepository ?? throw new ArgumentNullException(nameof(taskRepository));
            _managementRepository = managementRepository ?? throw new ArgumentNullException(nameof(managementRepository));

            _senderEmail = configuration["EmailSettings:Email"];
            _senderPassword = configuration["EmailSettings:Password"];
            _host = configuration["EmailSettings:Host"];
        }

        public async Task Execute(IJobExecutionContext context)
        {
            var tasks = (await _taskRepository.GetTasksAsync())
                            .Where(task => (task.DeadLine - DateTime.UtcNow) <= TimeSpan.FromDays(1));

            foreach (var task in tasks)
            {
                var user = await _managementRepository.GetUserInfoAsync(task.OwnerUserId);

                using var message = new MailMessage(_senderEmail, user.Email);

                message.Subject = $"Task \"{task.Header}\"";
                message.Body = $"You need to complete task \"{task.Header}\" before {task.DeadLine:dd.MM.yyyy}";

                using var smtpClient = new SmtpClient()
                {
                    EnableSsl = true,
                    Host = _host,
                    Port = 25,
                    Credentials = new NetworkCredential(_senderEmail, _senderPassword)
                };

                await smtpClient.SendMailAsync(message);
            }
        }
    }
}
