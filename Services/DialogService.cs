using ToDoApp.Interfaces;

namespace ToDoApp.Services
{
    class DialogService : IDialogService
    {
        public Task<bool> ShowConfirmation(string title, string message, string accept, string cancel)
        {
            return Application.Current.MainPage.DisplayAlert(title, message, accept, cancel);
        }
    }
}
