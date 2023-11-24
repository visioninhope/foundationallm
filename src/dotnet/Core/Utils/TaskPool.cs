using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FoundationaLLM.Core.Utils
{
    public class TaskPool
    {
        private readonly int _maxConcurrentTasks;
        private readonly List<TaskStatus> _runningStates = new List<TaskStatus>
        {
            TaskStatus.Running,
            TaskStatus.WaitingForActivation,
            TaskStatus.WaitingToRun
        };

        private Task[] _tasks;

        public bool IsAtCapacity => _tasks.Count(t => _runningStates.Contains(t.Status)) == _maxConcurrentTasks;

        public TaskPool(int maxConcurrentTasks)
        {
            _maxConcurrentTasks = maxConcurrentTasks;
            _tasks = new Task[maxConcurrentTasks];
        }
    }
}
