using CommunityToolkit.Mvvm.ComponentModel;
using System.Windows.Input;
using ToDoApp.Interfaces;

namespace ToDoApp.ViewModels
{
    public class MainPageViewModel : ObservableObject
    {
        private int _completedTasks;
        private int _incompletedTasks;

        
        private readonly IDatabaseService _databaseService;
        private readonly INavigationService _navigationService;

        
        private string _tasksCompletionInfo;
        public string TasksCompletionInfo
        {
            get => _tasksCompletionInfo;
            set => SetProperty(ref _tasksCompletionInfo, value);
        }


        public ICommand NavigateToCreateTaskCommand => new Command(async () =>
        {
            await _navigationService.NavigateToAsync("CreateTaskPage");
        });

        
        public ICommand NavigateToTaskListCommand => new Command(async () =>
        {
            await _navigationService.NavigateToAsync("TaskListPage");
        });


        public ICommand OnNavigatedCommand => new Command(async () =>
        {
            await UpdateTasksCompletionInfo();
        });


        public MainPageViewModel(IDatabaseService databaseService, INavigationService navigationService)
        {
            _databaseService = databaseService;
            _navigationService = navigationService;
        }

        
        private async Task UpdateTasksCompletionInfo()
        {
            await _databaseService.GetAllTasks().ContinueWith(task =>
            {
                var tasks = task.Result;
                _completedTasks = tasks.Count(t => t.IsCompleted);
                _incompletedTasks = tasks.Count(t => !t.IsCompleted);
                SetInfoText();
            });
        }

        private void SetInfoText()
        {
            TasksCompletionInfo = $"Completed: {_completedTasks} | Incompleted: {_incompletedTasks}";
        }
    }
}
