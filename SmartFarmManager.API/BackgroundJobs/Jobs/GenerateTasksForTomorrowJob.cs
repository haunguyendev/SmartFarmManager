﻿namespace SmartFarmManager.API.BackgroundJobs.Jobs
{
    using Quartz;
    using SmartFarmManager.Service.Interfaces;

    public class GenerateTasksForTomorrowJob : IJob
    {
        private readonly ITaskService _taskService;

        public GenerateTasksForTomorrowJob(ITaskService farmingBatchService)
        {
            _taskService = farmingBatchService;
        }

        public async Task Execute(IJobExecutionContext context)
        {
            Console.WriteLine($"Generating tasks for tomorrow... {DateTime.Now}");
            await _taskService.GenerateTasksForTomorrowAsync();
            await _taskService.GenerateTreatmentTasksAsyncV2();
        }
    }

}
