namespace ToDoApp.Interfaces
{
    public interface INavigationService
    {
        void RegisterRoutes();
        Task NavigateToAsync(string route);
        Task NavigateToAsync(string route, int? id);
        Task GoBackAsync();
    }
}