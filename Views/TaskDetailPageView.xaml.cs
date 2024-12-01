using ToDoApp.ViewModels;

namespace ToDoApp.Views;

public partial class TaskDetailPageView : ContentPage
{
	public TaskDetailPageView(TaskDetailPageViewModel viewModel)
	{
		InitializeComponent();

        BindingContext = viewModel;
    }
}