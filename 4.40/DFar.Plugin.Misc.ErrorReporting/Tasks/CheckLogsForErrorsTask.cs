using System;
using System.Linq;
using Nop.Core.Domain.Logging;
using Nop.Services.Logging;
using Nop.Services.Tasks;

namespace DFar.Plugin.Misc.ErrorReporting.Tasks
{
    public class CheckLogsForErrorsTask : IScheduleTask
    {
        private readonly ILogger _logger;

        public CheckLogsForErrorsTask(
            ILogger logger)
        {
            _logger = logger;
        }

        public async System.Threading.Tasks.Task ExecuteAsync()
        {
            var fiveMinutesAgo = DateTime.Now.AddMinutes(-5);
            var logs = await _logger.GetAllLogsAsync(fiveMinutesAgo);

            var errorLogs = logs.Where(l => l.LogLevel == LogLevel.Error);
            var warningLogs = logs.Where(l => l.LogLevel == LogLevel.Warning);

            if (errorLogs.Count() < 5 && warningLogs.Count() < 5) { return; }


        }
    }
}