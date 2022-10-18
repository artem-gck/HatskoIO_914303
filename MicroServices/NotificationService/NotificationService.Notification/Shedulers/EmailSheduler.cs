using NotificationService.Notification.Jobs;
using Quartz.Impl;
using Quartz;

namespace NotificationService.Notification.Shedulers
{
    public class EmailSheduler
    {
        public static async void Start()
        {
            var scheduler = await StdSchedulerFactory.GetDefaultScheduler();
            await scheduler.Start();

            var job = JobBuilder.Create<EmailSender>().Build();

            ITrigger trigger = TriggerBuilder.Create()  
                .WithIdentity("taskReminder", "emailGroup")     
                .StartNow()                            
                .WithCronSchedule("0 0 0/6 ? * * *", x => x.InTimeZone(TimeZoneInfo.Local))
                .Build();

            await scheduler.ScheduleJob(job, trigger);
        }
    }
}
