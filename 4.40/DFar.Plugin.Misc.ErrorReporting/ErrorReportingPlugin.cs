using DFar.Plugin.Misc.ErrorReporting.Tasks;
using Nop.Core.Domain.Tasks;
using Nop.Services.Common;
using Nop.Services.Plugins;
using Nop.Services.Tasks;

namespace DFar.Plugin.Misc.ErrorReporting
{
    public class ErrorReportingPlugin : BasePlugin, IMiscPlugin
    {
        private readonly IScheduleTaskService _scheduleTaskService;

        private readonly string TaskType = $"{typeof(CheckLogsForErrorsTask).FullName}, {typeof(ErrorReportingPlugin).Namespace}";

        public ErrorReportingPlugin(
            IScheduleTaskService scheduleTaskService)
        {
            _scheduleTaskService = scheduleTaskService;
        }

        public override async System.Threading.Tasks.Task InstallAsync()
        {
            await RemoveTaskAsync();
            await AddTaskAsync();

            await base.InstallAsync();
        }

        public override async System.Threading.Tasks.Task UninstallAsync()
        {
            await RemoveTaskAsync();

            await base.UninstallAsync();
        }

        private async System.Threading.Tasks.Task AddTaskAsync()
        {
            ScheduleTask task = new ScheduleTask
            {
                Name = $"Check logs for errors",
                Seconds = 300,
                Type = TaskType,
                Enabled = true,
                StopOnError = false
            };

            await _scheduleTaskService.InsertTaskAsync(task);
        }

        private async System.Threading.Tasks.Task RemoveTaskAsync()
        {
            var task = await _scheduleTaskService.GetTaskByTypeAsync(TaskType);
            if (task != null)
            {
                await _scheduleTaskService.DeleteTaskAsync(task);
            }
        }
    }
}