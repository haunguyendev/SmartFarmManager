using Microsoft.Extensions.DependencyInjection;
using Quartz;
using SmartFarmManager.Service.Helpers;
using SmartFarmManager.Service.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartFarmManager.Service.Jobs
{
    public class UpcomingTaskReminderJob : IJob
    {
        private readonly IServiceScopeFactory _serviceScopeFactory;

        public UpcomingTaskReminderJob(IServiceScopeFactory serviceScopeFactory)
        {
            _serviceScopeFactory = serviceScopeFactory;
        }

        public async Task Execute(IJobExecutionContext context)
        {
            using (var scope = _serviceScopeFactory.CreateScope())
            {
                var taskService = scope.ServiceProvider.GetRequiredService<ITaskService>();

                Console.WriteLine($"Upcoming Task Reminder Job started at: {DateTimeUtils.GetServerTimeInVietnamTime()}");

                await taskService.ProcessUpcomingTaskNotificationAsync();

                Console.WriteLine($"Upcoming Task Reminder Job completed at: {DateTimeUtils.GetServerTimeInVietnamTime()}");
            }
        }
    }
}
