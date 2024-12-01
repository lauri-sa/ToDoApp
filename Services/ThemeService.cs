using CommunityToolkit.Maui.Core.Platform;
using ToDoApp.Interfaces;

namespace ToDoApp.Services
{
    class ThemeService(IResourceProviderService resourceProvider) : IThemeService
    {
        private readonly IResourceProviderService _resourceProvider = resourceProvider;

        public void SetThemeColor(Color themeColor)
        {
            Application.Current.Resources["Primary"] = themeColor;
            SetStatusBarColor();
        }

        public void SetAppTheme(AppTheme appTheme)
        {
            Application.Current.UserAppTheme = appTheme;
            SetStatusBarColor();
        }

        private static AppTheme GetAppTheme()
        {
            return Application.Current.UserAppTheme;
        }

        private void SetStatusBarColor()
        {
            var appTheme = GetAppTheme();

            if (appTheme == AppTheme.Dark)
            {
                var color = _resourceProvider.GetResource<Color>("OffBlack");
                StatusBar.SetColor(color);
            }
            else
            {
                var color = _resourceProvider.GetResource<Color>("Primary");
                StatusBar.SetColor(color);
            }
        }
    }
}