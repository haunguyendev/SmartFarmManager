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
    public class CheckAndNotifyAdminForEndingFarmingBatchesJob:IJob
    {
        private readonly IServiceScopeFactory _serviceScopeFactory;

        public CheckAndNotifyAdminForEndingFarmingBatchesJob(IServiceScopeFactory serviceScopeFactory)
        {
            _serviceScopeFactory = serviceScopeFactory;
        }

        public async Task Execute(IJobExecutionContext context)
        {
            using (var scope = _serviceScopeFactory.CreateScope())
            {
                var farmingBatchService = scope.ServiceProvider.GetRequiredService<IFarmingBatchService>();

                Console.WriteLine($"Job started at: {DateTimeUtils.GetServerTimeInVietnamTime()}");
                await farmingBatchService.CheckAndNotifyAdminForEndingFarmingBatchesAsync();
                Console.WriteLine($"Job completed  at: {DateTimeUtils.GetServerTimeInVietnamTime()}");
            }
        }
    }
}
