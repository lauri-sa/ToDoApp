using ToDoApp.ViewModels;

namespace ToDoApp.Views;

public partial class TaskListPageView : ContentPage
{
    public TaskListPageView(TaskListPageViewModel viewModel)
    {
        InitializeComponent();

        BindingContext = viewModel;
    }
}