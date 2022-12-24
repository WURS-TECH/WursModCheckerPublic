using ControlzEx.Theming;
using System.Windows;

namespace WursModChecker.Utilities.Styles
{
    public static class ThemeSelector
    {
        public static void SetDefaultTheme(FrameworkElement frameworkElement, string themeName = "Dark.Mauve")
        {
            ThemeManager.Current.ChangeTheme(frameworkElement, themeName);
        }
    }
}
