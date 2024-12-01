using CommunityToolkit.Mvvm.ComponentModel;
using System.Windows.Input;
using ToDoApp.Interfaces;
using ToDoApp.Models;

namespace ToDoApp.ViewModels
{
    public class CreateTaskPageViewModel : ObservableObject
    {
        private readonly INavigationService _navigationService;
        private readonly IValidationService _validationService;
        private readonly IDatabaseService _databaseService;

        private string? _imagePath = null;

        private ImageSource _photo;
        public ImageSource Photo
        {
            get => _photo;
            set => SetProperty(ref _photo, value);
        }


        private string _priority = string.Empty;
        public string Priority
        {
            get => _priority;
            set => SetProperty(ref _priority, value);
        }


        private string _taskTitle = string.Empty;
        public string TaskTitle
        {
            get => _taskTitle;
            set => SetProperty(ref _taskTitle, value);
        }


        private string _taskDescription = string.Empty;
        public string TaskDescription
        {
            get => _taskDescription;
            set => SetProperty(ref _taskDescription, value);
        }


        private DateTime _dueDate;
        public DateTime DueDate
        {
            get => _dueDate;
            set => SetProperty(ref _dueDate, value);
        }


        private DateTime _minimumDate;
        public DateTime MinimumDate
        {
            get => _minimumDate;
            set => SetProperty(ref _minimumDate, value.Date);
        }


        private List<string> _priorities = [];
        public List<string> Priorities
        {
            get => _priorities;
            set => SetProperty(ref _priorities, value);
        }


        public CreateTaskPageViewModel(INavigationService navigationService, IValidationService validationService, IDatabaseService databaseService)
        {
            _navigationService = navigationService;
            _validationService = validationService;
            _databaseService = databaseService;
            Priorities = ["Low", "Medium", "High"];
            Priority = Priorities.First();
            SetCurrentDate();
        }


        public ICommand TakePhotoCommand => new Command(async () =>
        {
            await TakePhotoAsync();
        });


        public ICommand CreateTaskCommand => new Command(async () =>
        {
            var (isInputValid, message) = _validationService.ValidateInputs(TaskTitle, TaskDescription, DueDate);

            if (!isInputValid)
            {
                SetWarningMessage(message);
            }
            else
            {
                await AddTaskAsync();
            }
        });


        private async Task AddTaskAsync()
        {
            var task = CreateTask();
            var success = await _databaseService.SaveTask(task);
            if (success)
            {
                await _navigationService.GoBackAsync();
            }
            else
            {
                SetWarningMessage("Saving task failed");
            }
        }


        private async Task TakePhotoAsync()
        {
            if (!MediaPicker.Default.IsCaptureSupported)
            {
                _validationService.ValidationMessage = "Taking photo is not supported on this device";
                return;
            }
            
            try
            {
                var photoResult = await MediaPicker.Default.CapturePhotoAsync();
                if (photoResult != null)
                {
                    var newImagePath = Path.Combine(FileSystem.AppDataDirectory, photoResult.FileName);
                    var success = await _databaseService.SaveImage(photoResult, newImagePath);
                    if (success)
                    {
                        _imagePath = newImagePath;
                        Photo = ImageSource.FromFile(_imagePath);
                    }
                    else
                    {
                        _validationService.ValidationMessage = "Saving photo failed";
                    }
                }
            }
            catch (Exception)
            {
                _validationService.ValidationMessage = "Taking photo failed";
            }

        }


        private void SetWarningMessage(string message)
        {
            _validationService.ValidationMessage = message;
        }


        private void SetCurrentDate()
        {
            MinimumDate = DateTime.Today;
        }


        private TaskItemModel CreateTask()
        {
            return new TaskItemModel(null, _imagePath, Priority, TaskTitle, TaskDescription, DueDate, false);
        }
    }
}