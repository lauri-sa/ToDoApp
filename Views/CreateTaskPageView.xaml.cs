using ToDoApp.ViewModels;

namespace ToDoApp.Views;

public partial class CreateTaskPageView : ContentPage
{
    public CreateTaskPageView(CreateTaskPageViewModel viewModel)
    {
        InitializeComponent();

        BindingContext = viewModel;
    }
}