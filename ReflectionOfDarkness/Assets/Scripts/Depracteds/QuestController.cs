using System.Collections.Generic;
using UnityEngine;

namespace Depracteds
{
    public class QuestController : MonoBehaviour
    {
        [SerializeField]
        [OnInspector(ReadOnly = true)]
        public Dictionary<string, Task> tasks;
        public void Render()
        {
            foreach (KeyValuePair<string, Task> task in tasks)
            {
                Canvas quest = new Canvas();
            }
        }
        private void Start()
        {
            tasks = new Dictionary<string, Task>();
            tasks.Add("test",
            new Task
            {
                Id = 1,
                Name = "Büyünün başlangıcı için gerekli eşyayı topla",
                TaskType = TaskType.Loot,
            //Interact = BirTaneTopladi,
            //Finish = TumunuTopladi,
            IsRequireTaskItem = true,
                RequireTaskItems = new TaskItem[] {
                new TaskItem
                {
                    Id = 1,
                    Name = "Temiz Orak",
                    ExposureRate = 1f,
                    Item = null,
                },
                new TaskItem
                {
                    Id = 2,
                    Name = "Paslanmış Orak",
                    ExposureRate = 0.5f,
                    Item = null,
                },
                new TaskItem
                {
                    Id = 3,
                    Name = "Kanlı Orak",
                    ExposureRate = -0.5f,
                    Item = null,
                }
                },
                SelectedRequireTaskItem = null,
                TaskItems = new TaskItem[] {
                new TaskItem
                {
                    Id = 1,
                    Name = "Gül",
                    ExposureRate = -1f,
                    Item = null,
                },
                new TaskItem
                {
                    Id = 2,
                    Name = "Kararmış Ot",
                    ExposureRate = 1f,
                    Item = null,
                }
                },
                SelectedTaskItem = null,
                TaskCurrentSteps = 0,
                TaskCompleteSteps = 10
            });
            Render();
        }
    }
}