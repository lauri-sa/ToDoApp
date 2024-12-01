namespace ToDoApp.Models
{
    public class TaskItemModel(int? id, string imagePath, string priority, string title, string description, DateTime dueDate, bool isCompleted)
    {
        public int? Id { get; set; } = id;
        public string ImagePath { get; set; } = imagePath;
        public string Priority { get; private set; } = priority;
        public string Title { get; private set; } = title;
        public string Description { get; private set; } = description;
        public DateTime DueDate { get; private set; } = dueDate;
        public bool IsCompleted { get; set; } = isCompleted;
    }
}
