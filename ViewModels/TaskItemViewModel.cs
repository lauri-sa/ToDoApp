using CommunityToolkit.Mvvm.ComponentModel;
using ToDoApp.Interfaces;
using ToDoApp.Models;

namespace ToDoApp.ViewModels
{
    public class TaskItemViewModel : ObservableObject
    {
        private readonly IResourceProviderService _rps;
        private readonly TaskItemModel _taskItem;

        private int? _id;
        public int? Id
        {
            get => _id;
            set => SetProperty(ref _id, value);
        }

        public string ImagePath => _taskItem.ImagePath;
        public string Priority => _taskItem.Priority;
        public string Title => _taskItem.Title;
        public string Description => _taskItem.Description;
        public string DueDate => _taskItem.DueDate.ToString("dd/MM/yyyy");
        private bool _isCompleted;
        public bool IsCompleted
        {
            get => _isCompleted;
            set => SetProperty(ref _isCompleted, value);
        }

        public Color DueDateColor => _taskItem.DueDate.Date < DateTime.Now.Date
            ? _rps.GetResource<Color>("Red")
            : _rps.GetResource<Color>("White");

        private bool _isSelected;
        public bool IsSelected
        {
            get => _isSelected;
            set => SetProperty(ref _isSelected, value);
        }

        public TaskItemViewModel(TaskItemModel taskItem, IResourceProviderService rps)
        {
            _rps = rps;
            _taskItem = taskItem;
            Id = taskItem.Id;
            IsCompleted = taskItem.IsCompleted;
        }
    }
}
