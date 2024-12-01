using Android.App;
using Android.Content.Res;
using Android.Runtime;

namespace ToDoApp
{
    [Application]
    public class MainApplication : MauiApplication
    {
        public MainApplication(IntPtr handle, JniHandleOwnership ownership)
            : base(handle, ownership)
        {
            Microsoft.Maui.Handlers.PickerHandler.Mapper.AppendToMapping("NoUnderlines", (handler, view) =>
            {
                if (view is Picker)
                {
                    handler.PlatformView.BackgroundTintList = ColorStateList.ValueOf(Android.Graphics.Color.Transparent);
                }
            });

            Microsoft.Maui.Handlers.EntryHandler.Mapper.AppendToMapping("NoUnderlines", (handler, view) =>
            {
                if (view is Entry)
                {
                    handler.PlatformView.BackgroundTintList = ColorStateList.ValueOf(Android.Graphics.Color.Transparent);
                    handler.PlatformView.SetCursorVisible(false);
                }
            });

            Microsoft.Maui.Handlers.DatePickerHandler.Mapper.AppendToMapping("NoUnderlines", (handler, view) =>
            {
                if (view is DatePicker)
                {
                    handler.PlatformView.BackgroundTintList = ColorStateList.ValueOf(Android.Graphics.Color.Transparent);
                }
            });

            Microsoft.Maui.Handlers.ButtonHandler.Mapper.AppendToMapping("RemoveBtnRippleEffect", (handler, view) =>
            {
                if (handler.PlatformView.Background is Android.Graphics.Drawables.RippleDrawable ripple)
                {
                    ripple.SetColor(Android.Content.Res.ColorStateList.ValueOf(Android.Graphics.Color.Transparent));
                };
            });
        }

        protected override MauiApp CreateMauiApp() => MauiProgram.CreateMauiApp();
    }
}
