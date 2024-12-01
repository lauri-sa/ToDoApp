using CommunityToolkit.Maui;
using Microsoft.Extensions.Logging;
using ToDoApp.Interfaces;
using ToDoApp.Services;
using ToDoApp.ViewModels;
using ToDoApp.Views;

namespace ToDoApp
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .UseMauiCommunityToolkit()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("AfacadFlux-Regular.ttf", "AfacadFlux");
                    fonts.AddFont("MaterialIcons-Regular.ttf", "MaterialIcons");
                });

            builder.Services.AddSingleton<AppShell>();
            builder.Services.AddSingleton<AppShellViewModel>();

            builder.Services.AddSingleton<MainPageView>();
            builder.Services.AddTransient<CreateTaskPageView>();
            builder.Services.AddTransient<TaskListPageView>();
            builder.Services.AddTransient<TaskDetailPageView>();

            builder.Services.AddSingleton<MainPageViewModel>();
            builder.Services.AddTransient<CreateTaskPageViewModel>();
            builder.Services.AddTransient<TaskListPageViewModel>();
            builder.Services.AddTransient<TaskItemViewModel>();
            builder.Services.AddTransient<TaskDetailPageViewModel>();

            builder.Services.AddTransient<IThemeService, ThemeService>();
            builder.Services.AddSingleton<IDatabaseService, DatabaseService>();
            builder.Services.AddTransient<ILocalStorageService, LocalStorageService>();
            builder.Services.AddSingleton<IValidationService, ValidationService>();
            builder.Services.AddSingleton<INavigationService, NavigationService>();
            builder.Services.AddTransient<IResourceProviderService, ResourceProviderService>();
            builder.Services.AddTransient<IDialogService, DialogService>();

#if DEBUG
            builder.Logging.AddDebug();
#endif

            return builder.Build();
        }
    }
}
