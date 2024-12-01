using CommunityToolkit.Mvvm.ComponentModel;
using ToDoApp.Interfaces;
using ToDoApp.Models;

namespace ToDoApp.ViewModels
{
    public class TaskDetailPageViewModel(IDatabaseService databaseService) : ObservableObject, IQueryAttributable
    {
        private readonly IDatabaseService _databaseService = databaseService;


        private string? _taskImagePath;
        public string? TaskImagePath
        {
            get => _taskImagePath;
            set => SetProperty(ref _taskImagePath, value);
        }


        private string _taskTitle;
        public string TaskTitle
        {
            get => _taskTitle;
            set => SetProperty(ref _taskTitle, value);
        }


        private string _taskDescription;
        public string TaskDescription
        {
            get => _taskDescription;
            set => SetProperty(ref _taskDescription, value);
        }


        private string _taskPriority;
        public string TaskPriority
        {
            get => _taskPriority;
            set => SetProperty(ref _taskPriority, value);
        }


        private string _taskDueDate;
        public string TaskDueDate
        {
            get => _taskDueDate;
            set => SetProperty(ref _taskDueDate, value);
        }


        /// <summary>
        /// Gets called automatically when navigating to the page with query attributes
        /// </summary>
        public async void ApplyQueryAttributes(IDictionary<string, object> query)
        {
            if (query.ContainsKey("Id") && query["Id"] is string idString && int.TryParse(idString, out int id))
            {
                var task = await _databaseService.GetTask(id);
                InitPageContent(task);
            }
        }


        private void InitPageContent(TaskItemModel task)
        {
            TaskImagePath = task.ImagePath;
            TaskTitle = task.Title;
            TaskDescription = task.Description;
            TaskPriority = task.Priority;
            TaskDueDate = task.DueDate.ToString("dd.MM.yyyy");
        }
    }
}
