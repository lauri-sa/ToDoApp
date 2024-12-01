using CommunityToolkit.Mvvm.ComponentModel;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows.Input;
using ToDoApp.Interfaces;

namespace ToDoApp.ViewModels
{
    public class TaskListPageViewModel : ObservableObject
    {
        private readonly IDialogService _dialogService;
        private readonly IDatabaseService _databaseService;
        private readonly IValidationService _validationService;
        private readonly INavigationService _navigationService;

        private ObservableCollection<TaskItemViewModel> _tasks = [];

        private string _headerText;
        public string HeaderText
        {
            get => _headerText;
            set => SetProperty(ref _headerText, value);
        }

        private string _emptyListText;
        public string EmptyListText
        {
            get => _emptyListText;
            set
            {
                SetProperty(ref _emptyListText, value);
            }
        }

        private bool _completedListSelected;
        public bool CompletedListSelected
        {
            get => _completedListSelected;
            set => SetProperty(ref _completedListSelected, value);
        }

        private bool _sortedByDueDate;
        public bool SortedByDueDate
        {
            get => _sortedByDueDate;
            set => SetProperty(ref _sortedByDueDate, value);
        }

        private bool _showTaskSelectors;
        public bool ShowTaskSelectors
        {
            get => _showTaskSelectors;
            set => SetProperty(ref _showTaskSelectors, value);
        }

        private TaskItemViewModel? _selectedTask;
        public TaskItemViewModel? SelectedTask
        {
            get => _selectedTask;
            set => SetProperty(ref _selectedTask, value);
        }

        private ObservableCollection<TaskItemViewModel> _filteredTasks;
        public ObservableCollection<TaskItemViewModel> FilteredTasks
        {
            get => _filteredTasks;
            set
            {
                SetProperty(ref _filteredTasks, value);
            }
        }

        public bool IsTaskListEmpty => FilteredTasks?.Count < 1;

        public ICommand ChangeListCommand => new Command(() =>
        {
            CompletedListSelected = !CompletedListSelected;
            SelectedTask = null;
            SetSelectedListItem();
            ChangeHeader();
            FilterTasks();
            SortTasks();
        });

        public ICommand ChangeSortCommand => new Command(() =>
        {
            SortedByDueDate = !SortedByDueDate;
            SelectedTask = null;
            SetSelectedListItem();
            SortTasks();
        });

        public ICommand ShowTaskSelectorsCommand => new Command(() =>
        {
            SetSelectedListItem();
        });

        public ICommand HideTaskSelectorsCommand => new Command(() =>
        {
            SelectedTask = null;
            SetSelectedListItem();
        });

        public ICommand OpenDetailsPageCommand => new Command<TaskItemViewModel>((task) =>
        {
            _navigationService.NavigateToAsync("TaskDetailPage", task.Id);
        });

        public ICommand SetIsCompletedCommand => new Command<TaskItemViewModel>((task) =>
        {
            SetIsCompleted(task);
        });

        public ICommand DeleteTaskCommand => new Command<TaskItemViewModel>(async(task) =>
        {
            bool isConfirmed = await _dialogService.ShowConfirmation(
                "Delete Task",
                $"Are you sure you want to delete task no. {task.Id}: '{task.Title}'?",
                "Yes",
                "No");

            if (isConfirmed)
            {
                await DeleteTask(task);
            }
        });

        public TaskListPageViewModel(IDialogService dialogService, IDatabaseService databaseService, IValidationService validationService, IResourceProviderService rps, INavigationService navigationService)
        {
            _dialogService = dialogService;
            _databaseService = databaseService;
            _validationService = validationService;
            _navigationService = navigationService;
            PopulateTasks(rps);
            ChangeHeader();
            _navigationService = navigationService;
        }


        private async void SetIsCompleted(TaskItemViewModel task)
        {
            var success = await _databaseService.SetIsCompleted(task.Id);

            if (!success)
            {
                SetWarningMessage("Setting task as completed failed");
            }
            else
            {
                task.IsCompleted = !task.IsCompleted;
                task.IsSelected = false;
                FilteredTasks.Remove(task);
                SelectedTask = null;
                OnPropertyChanged(nameof(IsTaskListEmpty));
            }
        }


        private async void PopulateTasks(IResourceProviderService rps)
        {
            var tasks = await _databaseService.GetAllTasks();
            foreach (var task in tasks)
            {
                _tasks.Add(new TaskItemViewModel(task, rps));
            }
            FilterTasks();
            SortTasks();
        }

        private async Task DeleteTask(TaskItemViewModel task)
        {
            var success = await _databaseService.DeleteTask(task.Id);
            if (success)
            {
                _tasks.Remove(task);
                ReorganizeTasks();
                FilterTasks();
                SortTasks();
            }
            else
            {
                SetWarningMessage("Deleting task failed");
            }
        }

        private void ReorganizeTasks()
        {
            var reorganizedTasks = _tasks.Select((t, index) =>
            {
                t.Id = index + 1;
                return t;
            }).ToList();
            _tasks = new(reorganizedTasks);
        }

        private void ChangeHeader()
        {
            HeaderText = CompletedListSelected
                ? "Completed Tasks"
                : "Incompleted Tasks";
        }

        private void ChangeEmptyListText()
        {
            EmptyListText = CompletedListSelected
                ? "No tasks completed yet"
                : "All tasks are completed";
        }

        private void FilterTasks()
        {
            if (CompletedListSelected)
            {
                FilteredTasks = new ObservableCollection<TaskItemViewModel>(
                    _tasks.Where(task => task.IsCompleted));
            }
            else
            {
                FilteredTasks = new ObservableCollection<TaskItemViewModel>(
                    _tasks.Where(task => !task.IsCompleted));
            }

            ChangeEmptyListText();

            OnPropertyChanged(nameof(IsTaskListEmpty));
        }

        private void SortTasks()
        {
            var priorityOrder = new List<string> { "High", "Medium", "Low" };

            if (SortedByDueDate)
            {
                FilteredTasks = new ObservableCollection<TaskItemViewModel>(
                    FilteredTasks
                        .OrderBy(task => task.DueDate)
                        .ThenBy(task => priorityOrder.IndexOf(task.Priority)));
            }
            else
            {
                FilteredTasks = new ObservableCollection<TaskItemViewModel>(
                    FilteredTasks
                        .OrderBy(task => priorityOrder.IndexOf(task.Priority))
                        .ThenBy(task => task.DueDate));
            }
        }

        private void SetSelectedListItem()
        {
            foreach (var task in FilteredTasks)
            {
                task.IsSelected = task == SelectedTask;
            }

            OnPropertyChanged(nameof(FilteredTasks));
        }

        private void SetWarningMessage(string message)
        {
            _validationService.ValidationMessage = message;
        }
    }
}
