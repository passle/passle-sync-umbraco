using PassleDotCom.PasslePlugin.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Umbraco.Core.Logging;
using Umbraco.Web.Scheduling;

namespace PassleDotCom.PasslePlugin.Core
{
    public class TaskRunnerWorker : LatchedBackgroundTaskBase
    {
        private readonly IBackgroundTaskRunner<LatchedBackgroundTaskBase> _runner;
        private readonly ICommandQueue _commandQueue;
        private readonly ILogger _logger;

        // Add the services that you need to the constructor here.
        public TaskRunnerWorker(IBackgroundTaskRunner<LatchedBackgroundTaskBase> runner,
                                        ICommandQueue commandQueue,
                                        ILogger logger)
        {
            _runner = runner;
            _commandQueue = commandQueue;
            _logger = logger;
            _commandQueue.OnCommandAdded += Dispatch;
        }

        public override async Task RunAsync(CancellationToken token)
        {
            try
            {
                while (_commandQueue.TryGet(out var command))
                {
                    // configure await is false so we don't have to wait for the context to become available again.
                    //   NOTE: be aware when creating commands that one command won't necessarily be executed on the same thread as the previous.
                    // This is where you would perform your logic
                    await command.ExecuteAsync().ConfigureAwait(false);
                }
            }
            catch (Exception e)
            {
                _logger.Error<TaskRunnerWorker>(e, "An exception occurred while executing a command on the queue.");
            }

            Repeat();
        }

        private void Repeat()
        {
            if (_runner.IsCompleted) return;

            Reset();

            if (!_runner.TryAdd(this))
            {
                Dispose();
            }
        }

        public void Dispatch(object sender, EventArgs e)
        {
            try
            {
                Release();
            }
            catch // it's alright if this fails, that means that this task is still running and the message will be consumed
            { }
        }

        public override bool IsAsync => true;

        protected override void DisposeResources()
        {
            base.DisposeResources();

            _commandQueue.OnCommandAdded -= Dispatch;
        }
    }
}
