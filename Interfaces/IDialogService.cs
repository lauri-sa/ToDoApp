namespace ToDoApp.Interfaces
{
    public interface IDialogService
    {
        Task<bool> ShowConfirmation(string title, string message, string accept, string cancel);
    }
}