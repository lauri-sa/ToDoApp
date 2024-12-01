namespace ToDoApp.Controls;

public partial class CustomSwitchControl : ContentView
{
    private readonly Color _lightModeColor = Color.FromArgb("#FFD700");
    private readonly Color _darkModeColor = Color.FromArgb("#6200EE");

    private readonly double _trackWidth = 70;
    private readonly double _thumbWidth = 37;

    public static readonly BindableProperty IsToggledProperty =
    BindableProperty.Create(nameof(IsToggled), typeof(bool), typeof(CustomSwitchControl), false, propertyChanged: OnIsToggledChanged);

    public bool IsToggled
    {
        get => (bool)GetValue(IsToggledProperty);
        set => SetValue(IsToggledProperty, value);
    }

    public CustomSwitchControl()
	{
        InitializeComponent();
    }

    private static void OnIsToggledChanged(BindableObject bindable, object oldValue, object newValue)
    {
        var control = (CustomSwitchControl)bindable;
        control.UpdateThumbPosition();
        control.UpdateIconColors();
    }

    private void UpdateThumbPosition()
    {
        var targetPosition = IsToggled ? _trackWidth - _thumbWidth : 0;

        targetPosition += targetPosition < 1 ? 2.5 : 3.5;

        Thumb.TranslateTo(targetPosition, 0, 100);
    }

    private void UpdateIconColors()
    {
        LightModeIcon.Color = _lightModeColor;
        DarkModeIcon.Color = _darkModeColor;
    }

    private void ControlLoaded(object sender, EventArgs e)
    {
        UpdateThumbPosition();
        UpdateIconColors();
    }
}