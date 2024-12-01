using CommunityToolkit.Mvvm.ComponentModel;
using System.Collections.ObjectModel;
using System.Windows.Input;
using ToDoApp.Interfaces;

namespace ToDoApp.ViewModels
{
    public class AppShellViewModel : ObservableObject
    {
        private readonly IResourceProviderService _resourceProvider;
        private readonly ILocalStorageService _localStorageService;
        private readonly IValidationService _validationService;
        private readonly IThemeService _themeService;


        public string AppVersion => $"Version: {AppInfo.VersionString} (Build {AppInfo.BuildString})";
        public string Copyright => $"© {DateTime.Now.Year} Lauri Saarenpää. All rights reserved.";


        private bool _isDarkMode;
        public bool IsDarkMode
        {
            get => _isDarkMode;
            set => SetProperty(ref _isDarkMode, value);
        }


        private string _validationMessage;
        public string ValidationMessage
        {
            get => _validationMessage;
            set => SetProperty(ref _validationMessage, value);
        }


        private ObservableCollection<Tuple<string, Color>> _colorThemes;
        public ObservableCollection<Tuple<string, Color>> ColorThemes
        {
            get => _colorThemes;
            set => SetProperty(ref _colorThemes, value);
        }


        private FlyoutBehavior _flyoutBehavior;
        public FlyoutBehavior FlyoutBehavior
        {
            get => _flyoutBehavior;
            set => SetProperty(ref _flyoutBehavior, value);
        }


        private bool _isFlyoutOpen;
        public bool IsFlyoutOpen
        {
            get => _isFlyoutOpen;
            set
            {
                FlyoutBehavior = value ? FlyoutBehavior.Flyout : FlyoutBehavior.Disabled;
                SetProperty(ref _isFlyoutOpen, value);
            }
        }


        public AppShellViewModel(IResourceProviderService resourceProvider, ILocalStorageService localStorageService,
                                IThemeService themeService, IValidationService validationService)
        {
            _resourceProvider = resourceProvider;
            _localStorageService = localStorageService;
            _themeService = themeService;
            _validationService = validationService;
            ValidationMessage = string.Empty;
            FlyoutBehavior = FlyoutBehavior.Disabled;
            _validationService.PropertyChanged += OnValidationMessageChanged;
        }


        public ICommand InitializeThemesCommand => new Command(() =>
        {
            var loadedThemeColors = LoadThemeColors();
            var parsedThemeColors = ParseLoadedThemeColors(loadedThemeColors);
            ColorThemes = new(parsedThemeColors);
            SetThemeColor();
            SetAppTheme();
        });


        private void OnValidationMessageChanged(object? sender, EventArgs e)
        {
            ValidationMessage = _validationService.ValidationMessage;
        }


        public ICommand ChangeThemeCommand => new Command<Color>((themeColor) =>
        {
            SaveThemeColor(themeColor);
            SetThemeColor();
        });


        public ICommand ToggleDarkModeCommand => new Command(() =>
        {
            IsDarkMode = !IsDarkMode;
            SaveAppTheme(IsDarkMode);
            SetAppTheme();
        });


        public ICommand ToggleFlyoutCommand => new Command(() =>
        {
            if (IsDarkMode)
            {
                SetWarningMessage("Themes are not available in dark mode");
            }
            else
            {
                OpenFlyout();
            }
        });


        private void SetWarningMessage(string message)
        {
            _validationService.ValidationMessage = message;
        }


        private void OpenFlyout()
        {
            IsFlyoutOpen = true;
        }


        private void SaveThemeColor(Color themeColor)
        {
            _localStorageService.Save("ThemeColor", themeColor.ToArgbHex());
        }


        private void SaveAppTheme(bool isDarkMode)
        {
            _localStorageService.Save("AppTheme", isDarkMode);
        }


        private Dictionary<string, Color> LoadThemeColors()
        {
            return _resourceProvider.GetResourcesWithPrefix<Color>("ThemeColor");
        }


        private T LoadStoredValue<T>(string resourceName)
        {
            return _localStorageService.Get<T>(resourceName);
        }


        private T LoadResource<T>(string resourceName)
        {
            return _resourceProvider.GetResource<T>(resourceName);
        }


        private static List<Tuple<string, Color>> ParseLoadedThemeColors(Dictionary<string, Color> colors)
        {
            return colors.Select(x => new Tuple<string, Color>(x.Key, x.Value)).ToList();
        }


        private Color ProcessLoadedThemeColor(string colorHex)
        {
            var themeColor = string.IsNullOrEmpty(colorHex) ? ColorThemes?.First().Item2 : Color.FromArgb(colorHex);
            return themeColor ?? LoadResource<Color>("Primary"); // Fallback to default color
        }


        private void SetThemeColor()
        {
            var loadedColorHex = LoadStoredValue<string>("ThemeColor");
            var themeColor = ProcessLoadedThemeColor(loadedColorHex);
            _themeService.SetThemeColor(themeColor);
        }


        private void SetAppTheme()
        {
            IsDarkMode = LoadStoredValue<bool>("AppTheme");
            var appTheme = IdentifyTheme(IsDarkMode);
            _themeService.SetAppTheme(appTheme);
        }


        private static AppTheme IdentifyTheme(bool isDarkMode)
        {
            return isDarkMode ? AppTheme.Dark : AppTheme.Light;
        }
    }
}