using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Prometheus;
using System.Threading.Tasks;
using ToDoList.Entities;
using ToDoList.Messaging;
using ToDoList.Messaging.Messages.Events;
using ToDoList.Model;

namespace ToDoList.Services
{
    public class ToDoService
    {
        private static Counter _NewTasksCounter;

        private readonly ToDoContext _context;
        private readonly MessageQueue _messageQueue;
        private readonly IConfiguration _config;

        public ToDoService(ToDoContext context, MessageQueue messageQueue, IConfiguration config)
        {
            _context = context;
            _messageQueue = messageQueue;
            _config = config;
            EnsureMetrics();
        }

        private void EnsureMetrics()
        {
            if (_NewTasksCounter == null && _config.GetValue<bool>("Metrics:Enabled"))
            {
                _NewTasksCounter = Metrics.CreateCounter("todo_tasks_created_total", "TODO List - Number of Tasks created");
            }
        }

        public async Task<ToDo[]> GetToDosAsync()
        {
            return await _context.ToDos.ToArrayAsync();
        }

        public async Task<int> GetToDoCountAsync()
        {
            return await _context.ToDos.CountAsync();
        }

        public void AddToDo(ToDo todo)
        {
            _messageQueue.Publish(new NewItemEvent(todo));
            if (_NewTasksCounter != null)
            {
                _NewTasksCounter.Inc();
            }
        }
    }
}
