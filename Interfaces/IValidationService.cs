using System.ComponentModel;

namespace ToDoApp.Interfaces
{
    public interface IValidationService
    {
        string ValidationMessage { get; set; }

        event PropertyChangedEventHandler? PropertyChanged;

        (bool IsInputValid, string Message) ValidateInputs(string taskTitle, string taskDescription, DateTime dueDate);
    }
}