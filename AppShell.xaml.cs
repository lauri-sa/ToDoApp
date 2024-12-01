using ToDoApp.Interfaces;
using ToDoApp.ViewModels;

namespace ToDoApp
{
    public partial class AppShell : Shell
    {
        public AppShell(AppShellViewModel viewModel, INavigationService navigationService)
        {
            InitializeComponent();

            BindingContext = viewModel;

            navigationService.RegisterRoutes();
        }
    }
}