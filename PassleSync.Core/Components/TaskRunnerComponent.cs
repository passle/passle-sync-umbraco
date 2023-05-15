using PassleSync.Core.API.SyncHandlers;
using PassleSync.Core.Models.Content.PassleApi;
using PassleSync.Core.Services.API;
using PassleSync.Core.SyncHandlers;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using Umbraco.Core;
using Umbraco.Core.Composing;
using Umbraco.Core.Logging;
using Umbraco.Web;
using Umbraco.Web.Scheduling;

namespace PassleSync.Core.Components
{
    public class TaskRunnerComponent : IComponent
    {
        private IProfilingLogger _logger;
        private BackgroundTaskRunner<IBackgroundTask> _syncPostsRunner;
        private BackgroundTaskRunner<IBackgroundTask> _syncAuthorsRunner;
        private readonly IUmbracoContextFactory _umbracoContextFactory;

        const int START_DELAY = 10 * 1000;
        const int REPEAT_INTERVAL = 30 * 1000;

        public TaskRunnerComponent(IProfilingLogger logger, IUmbracoContextFactory umbracoContextFactory)
        {
            _logger = logger;
            _umbracoContextFactory = umbracoContextFactory;
            _syncPostsRunner = new BackgroundTaskRunner<IBackgroundTask>("CheckForPostsToSync", _logger);
            _syncAuthorsRunner = new BackgroundTaskRunner<IBackgroundTask>("CheckForAuthorsToSync", _logger);
        }

        public void Initialize()
        {
            Run<PasslePost>(_syncPostsRunner);
            Run<PassleAuthor>(_syncAuthorsRunner);
        }

        public void Run<T>(BackgroundTaskRunner<IBackgroundTask> runner) where T : class
        {
            var _backgroundSyncService = Current.Factory.GetInstance<BackgroundSyncServiceBase<T>>();
            var _syncHandler = Current.Factory.GetInstance<ISyncHandler<T>>();

            var task = new CheckForItemsToSync<T>(runner, START_DELAY, REPEAT_INTERVAL, _logger, _umbracoContextFactory, _backgroundSyncService, _syncHandler);

            //declare the events
            runner.TaskCompleted += Task_Completed<T>;
            runner.TaskStarting += Task_Starting<T>;
            runner.TaskCancelled += Task_Cancelled<T>;
            runner.TaskError += Task_Error<T>;

            //As soon as we add our task to the runner it will start to run (after its delay period)
            runner.TryAdd(task);
        }

        private void Task_Completed<T>(BackgroundTaskRunner<IBackgroundTask> sender, TaskEventArgs<IBackgroundTask> e)
        {
            _logger.Info<TaskRunnerComponent>(typeof(T).Name + " run finished");
        }

        private void Task_Starting<T>(BackgroundTaskRunner<IBackgroundTask> sender, TaskEventArgs<IBackgroundTask> e)
        {
            _logger.Info<TaskRunnerComponent>(typeof(T).Name + " starting");
        }

        private void Task_Cancelled<T>(BackgroundTaskRunner<IBackgroundTask> sender, TaskEventArgs<IBackgroundTask> e)
        {
            _logger.Info<TaskRunnerComponent>(typeof(T).Name + " cancelled");
        }

        private void Task_Error<T>(BackgroundTaskRunner<IBackgroundTask> sender, TaskEventArgs<IBackgroundTask> e)
        {
            _logger.Info<TaskRunnerComponent>(typeof(T).Name + " error");
        }

        public void Terminate()
        {
            Terminate<PasslePost>(_syncPostsRunner);
            Terminate<PassleAuthor>(_syncAuthorsRunner);
        }

        void Terminate<T>(BackgroundTaskRunner<IBackgroundTask> runner) where T : class
        {
            runner.TaskCompleted -= Task_Completed<T>;
            runner.TaskStarting -= Task_Starting<T>;
            runner.TaskCancelled -= Task_Cancelled<T>;
            runner.TaskError -= Task_Error<T>;
        }
    }

    public class CheckForItemsToSync<T> : RecurringTaskBase where T : class
    {
        private IProfilingLogger _logger;
        private BackgroundSyncServiceBase<T> _backgroundSyncService;
        private ISyncHandler<T> _syncHandler;
        private readonly IUmbracoContextFactory _umbracoContextFactory;

        public CheckForItemsToSync(
            IBackgroundTaskRunner<RecurringTaskBase> runner,
            int startDelay,
            int repeatInterval,
            IProfilingLogger logger,
            IUmbracoContextFactory umbracoContextFactory,
            BackgroundSyncServiceBase<T> backgroundSyncService,
            ISyncHandler<T> syncHandler
        ) : base(runner, startDelay, repeatInterval)
        {
            _logger = logger;
            _backgroundSyncService = backgroundSyncService;
            _syncHandler = syncHandler;
            _umbracoContextFactory = umbracoContextFactory;
        }

        public override bool IsAsync => false;

        public override bool PerformRun()
        {
            var itemsToSync = _backgroundSyncService.GetItemsToSync();
            var itemsToDelete = _backgroundSyncService.GetItemsToDelete();

            _logger.Info<CheckForItemsToSync<T>>(string.Format("You have {0} {1} to sync", itemsToSync.Count(), typeof(T).Name));
            _logger.Info<CheckForItemsToSync<T>>(string.Format("You have {0} {1} to delete", itemsToDelete.Count(), typeof(T).Name));

            if (itemsToSync.Count() > 0)
            {
                using (_logger.TraceDuration<CheckForItemsToSync<T>>("Syncing content...", "Finished syncing."))
                {
                    var syncResults = new List<SyncTaskResult>();
                    using (var reference = _umbracoContextFactory.EnsureUmbracoContext())
                    {
                        syncResults = _syncHandler.SyncMany(itemsToSync.ToArray()).ToList();
                    }

                    foreach (var syncResult in syncResults)
                    {
                        if (!syncResult.Success)
                        {
                            _logger.Info<CheckForItemsToSync<T>>(string.Format("Failed to sync {0} with shortcode {1}", typeof(T).Name, syncResult.Shortcode));
                        }
                    }
                }
            }
            _backgroundSyncService.RemoveItemsToSync(itemsToSync);

            if (itemsToDelete.Count() > 0)
            {
                using (_logger.TraceDuration<CheckForItemsToSync<T>>("Deleting content...", "Finished deleting."))
                {
                    var deleteResults = new List<SyncTaskResult>();
                    using (var reference = _umbracoContextFactory.EnsureUmbracoContext())
                    {
                        deleteResults = _syncHandler.DeleteMany(itemsToDelete.ToArray()).ToList();
                    }

                    foreach (var deleteResult in deleteResults)
                    {
                        if (!deleteResult.Success)
                        {
                            _logger.Info<CheckForItemsToSync<T>>(string.Format("Failed to delete {0} with shortcode {1}", typeof(T).Name, deleteResult.Shortcode));
                        }
                    }
                }
            }
            _backgroundSyncService.RemoveItemsToDelete(itemsToDelete);

            // If we want to keep repeating - we need to return true
            // But if we run into a problem/error & want to stop repeating - return false
            return true;
        }
    }
}
