using System;

namespace Depracteds
{
    public class Task
    {
        public int Id;
        public string Name;
        public TaskType TaskType;
        public bool IsRequireTaskItem;
        public TaskItem[] RequireTaskItems;
        public TaskItem SelectedRequireTaskItem;
        public int TaskCompleteSteps;
        public int TaskCurrentSteps;
        public TaskItem[] TaskItems;
        public TaskItem SelectedTaskItem;
        public Func<Task, TaskItem, string, bool> Interact;
        public Action Finish;
    }
}
