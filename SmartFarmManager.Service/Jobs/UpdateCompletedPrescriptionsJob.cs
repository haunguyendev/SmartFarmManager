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
    public class UpdateCompletedPrescriptionsJob : IJob
    {
        private readonly IServiceScopeFactory _serviceScopeFactory;

        public UpdateCompletedPrescriptionsJob(IServiceScopeFactory serviceScopeFactory)
        {
            _serviceScopeFactory = serviceScopeFactory;
        }

        public async Task Execute(IJobExecutionContext context)
        {
            using (var scope = _serviceScopeFactory.CreateScope())
            {
                try
                {
                    var prescriptionService = scope.ServiceProvider.GetRequiredService<IPrescriptionService>();

                    Console.WriteLine($"Running UpdateCompletedPrescriptionsJob at: {DateTimeUtils.GetServerTimeInVietnamTime()}");
                    await prescriptionService.UpdateCompletedPrescriptionsAsync();
                    Console.WriteLine($"UpdateCompletedPrescriptionsJob finished.");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error in UpdateCompletedPrescriptionsJob: {ex.Message}");
                }
            }
        }
    }
}
