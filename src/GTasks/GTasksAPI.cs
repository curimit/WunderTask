using Google.Apis.Auth.OAuth2;
using Google.Apis.Services;
using Google.Apis.Tasks.v1;
using Google.Apis.Tasks.v1.Data;
using Google.Apis.Util.Store;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace WunderTask
{
    class GTasksAPI
    {
        /// <summary>The remote service on which all the requests are executed.</summary>
        private TasksService _service = new TasksService();
        private List<TaskList> _tasklists { get; set; }

        public GTasksAPI()
        {
            Init().Wait();
        }

        /// <summary>
        /// Initialize GTasks Service.
        /// </summary>
        /// <returns></returns>
        public async System.Threading.Tasks.Task Init()
        {
            using (var stream = new FileStream("client_secrets.json", FileMode.Open, FileAccess.Read))
            {
                var credential = await GoogleWebAuthorizationBroker.AuthorizeAsync(
                                    GoogleClientSecrets.Load(stream).Secrets,
                                    new[] { TasksService.Scope.Tasks },
                                    "user", CancellationToken.None, new FileDataStore("Tasks.Auth.Store"));

                _service = new TasksService(new BaseClientService.Initializer()
                {
                    HttpClientInitializer = credential,
                    ApplicationName = "Tasks API Sample",
                });
            }
        }


        public IEnumerable<Google.Apis.Tasks.v1.Data.TaskList> GetTaskLists()
        {
            return this._tasklists;
        }

        /// <summary>
        /// Synchronize TaskList With GTasks Server.
        /// </summary>
        /// <returns></returns>
        public async System.Threading.Tasks.Task Synchronize()
        {
            _tasklists = new List<TaskList>();
            var taskLists = await _service.Tasklists.List().ExecuteAsync();
            foreach (TaskList list in taskLists.Items)
            {
                var tasks = await _service.Tasks.List(list.Id).ExecuteAsync();
                this._tasklists.Add(list);
            }
        }

        public async System.Threading.Tasks.Task CreateTaskListByTitle(string tasklist_title)
        {
            var tasklist = new TaskList();
            tasklist.Title = tasklist_title;
            _service.Tasklists.Insert(tasklist);
        }

        public Google.Apis.Tasks.v1.Data.TaskList GetTaskListByTitle(string tasklist_title)
        {
            foreach (var list in this._tasklists)
                if (list.Title == tasklist_title)
                    return list;
            return null;
        }

        public async System.Threading.Tasks.Task<Google.Apis.Tasks.v1.Data.Tasks> GetTasksByTitle(string tasklist_title)
        {
            TaskList list = GetTaskListByTitle(tasklist_title);
            if (list.Id == null)
            {
                return null;
            }
            return await _service.Tasks.List(list.Id).ExecuteAsync();
        }

        public async System.Threading.Tasks.Task DeleteTaskList(string tasklist_id)
        {
            _service.Tasklists.Delete(tasklist_id);
        }
    }
}
