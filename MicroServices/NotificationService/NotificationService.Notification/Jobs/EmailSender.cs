using Quartz;
using System.Net.Mail;
using System.Net;
using NotificationService.DataAccess.Http.Interfaces;
using Microsoft.Extensions.Configuration;
using NotificationService.DataAccess.DataBase;
using NotificationService.DataAccess.DataBase.Entity;
using System.Text;

namespace NotificationService.Notification.Jobs
{
    public class EmailSender : IJob
    {
        private readonly string _senderEmail;
        private readonly string _senderPassword;
        private readonly string _host;

        private readonly ITaskAccess _taskAccess;
        private readonly IManagementAccess _managementAccess;
        private readonly IMessageRepository _messageRepository;

        public EmailSender(ITaskAccess taskAccess, IManagementAccess managementAccess, IMessageRepository messageRepository, IConfiguration configuration)
        {
            _taskAccess = taskAccess ?? throw new ArgumentNullException(nameof(taskAccess));
            _managementAccess = managementAccess ?? throw new ArgumentNullException(nameof(managementAccess));
            _messageRepository = messageRepository ?? throw new ArgumentNullException(nameof(messageRepository));

            _senderEmail = configuration["EmailSettings:Email"];
            _senderPassword = configuration["EmailSettings:Password"];
            _host = configuration["EmailSettings:Host"];
        }

        public async Task Execute(IJobExecutionContext context)
        {
            var tasks = (await _taskAccess.GetTasksAsync())
                            .Where(task => (task.DeadLine - DateTime.Now) <= TimeSpan.FromDays(1))
                            .GroupBy(task => task.OwnerUserId);

            foreach (var task in tasks)
            {
                var user = await _managementAccess.GetUserInfoAsync(task.Key);

                using var message = new MailMessage(_senderEmail, user.Email);

                message.Subject = $"Task deadlines";

                var body = new StringBuilder();
                body.AppendJoin('\n', task.Select(ta => $"You need to complete task <{ta.Header}> before {ta.DeadLine:dd.MM.yyyy}"));

                message.Body = body.ToString();

                using var smtpClient = new SmtpClient()
                {
                    EnableSsl = true,
                    Host = _host,
                    Port = 25,
                    Credentials = new NetworkCredential(_senderEmail, _senderPassword)
                };

                await smtpClient.SendMailAsync(message);

                var messageEntity = new MessageEntity
                {
                    RecipientId = user.Id,
                    Subject = message.Subject,
                    Body = message.Body,
                };

                await _messageRepository.AddAsync(messageEntity);
            }
        }
    }
}
