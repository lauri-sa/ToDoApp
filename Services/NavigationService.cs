using ToDoApp.Interfaces;
using ToDoApp.Views;

namespace ToDoApp.Services
{
    class NavigationService : INavigationService
    {
        public void RegisterRoutes()
        {
            Routing.RegisterRoute("CreateTaskPage", typeof(CreateTaskPageView));
            Routing.RegisterRoute("TaskListPage", typeof(TaskListPageView));
            Routing.RegisterRoute("TaskDetailPage", typeof(TaskDetailPageView));
        }

        public async Task NavigateToAsync(string route)
        {
            await Shell.Current.GoToAsync(route);
        }

        public async Task NavigateToAsync(string route, int? id)
        {
            await Shell.Current.GoToAsync($"{route}?Id={id}");
        }

        public async Task GoBackAsync()
        {
            await Shell.Current.GoToAsync("..");
        }
    }
}