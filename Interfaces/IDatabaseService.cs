using System.Collections.ObjectModel;
using ToDoApp.Models;

namespace ToDoApp.Interfaces
{
    public interface IDatabaseService
    {
        public Task<bool> SaveImage(FileResult image, string imagePath);
        public Task<bool> SaveTask(TaskItemModel task);
        public Task<TaskItemModel> GetTask(int? id);
        public Task<ObservableCollection<TaskItemModel>> GetAllTasks();
        public Task<bool> DeleteTask(int? id);
        public Task<bool> SetIsCompleted(int? id);
    }
}