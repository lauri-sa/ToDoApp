using ToDoApp.ViewModels;

namespace ToDoApp.Views;

public partial class MainPageView : ContentPage
{
    public MainPageView(MainPageViewModel viewModel)
    {
        InitializeComponent();

        BindingContext = viewModel;
    }
}