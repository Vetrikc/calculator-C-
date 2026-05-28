using SimpleCalculatorMVVM_4.Models;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace SimpleCalculatorMVVM_4.Services
{
    public class ThemeManager
    {
        private readonly ConfigurationService _configService;

        public ThemeManager(ConfigurationService configService)
        {
            _configService = configService;
        }

        public void ApplyTheme(string themeName)
        {
            ThemeColors? theme = null;

            // Получаем тему из конфига
            if (_configService.Config.Themes != null && _configService.Config.Themes.ContainsKey(themeName))
            {
                theme = _configService.Config.Themes[themeName];
            }

            if (theme == null) return;

            ApplyColorsToApp(theme.BackgroundColor, theme.ButtonColor, theme.ButtonSpecialColor,
                           theme.ButtonEqualColor, theme.TextColor, theme.DisplayBackground);
        }

        public void ApplyAgeStyle(string ageGroup)
        {
            AgeStyle? style = null;

            if (_configService.Config.AgeStyles != null && _configService.Config.AgeStyles.ContainsKey(ageGroup))
            {
                style = _configService.Config.AgeStyles[ageGroup];
            }

            if (style == null) return;

            ApplyColorsToApp(style.BackgroundColor, style.ButtonColor, style.ButtonSpecialColor,
                           "#00C853", style.TextColor, style.BackgroundColor);

            // Применяем размер шрифта
            if (Application.Current.MainWindow != null)
            {
                var fontSize = (double)style.FontSize;
                Application.Current.Resources["FontSize"] = fontSize;

                // Меняем размер шрифта у всех кнопок через стиль
                var styleDict = new Style(typeof(Button));
                styleDict.Setters.Add(new Setter(Control.FontSizeProperty, fontSize));
                Application.Current.Resources.Add("DynamicFontSize", fontSize);
            }
        }

        public void ApplyGenderStyle(string gender)
        {
            GenderStyle? style = null;

            if (_configService.Config.GenderStyles != null && _configService.Config.GenderStyles.ContainsKey(gender))
            {
                style = _configService.Config.GenderStyles[gender];
            }

            if (style == null) return;

            ApplyColorsToApp(style.BackgroundColor, style.ButtonColor, style.ButtonSpecialColor,
                           "#00C853", style.TextColor, style.BackgroundColor);
        }

        private void ApplyColorsToApp(string backgroundColor, string buttonColor, string buttonSpecialColor,
                             string buttonEqualColor, string textColor, string displayBackground)
        {
            System.Diagnostics.Debug.WriteLine($"=== APPLYING THEME ===");
            System.Diagnostics.Debug.WriteLine($"BG: {backgroundColor}");
            System.Diagnostics.Debug.WriteLine($"Button: {buttonColor}");
            System.Diagnostics.Debug.WriteLine($"Special: {buttonSpecialColor}");
            System.Diagnostics.Debug.WriteLine($"Equal: {buttonEqualColor}");
            System.Diagnostics.Debug.WriteLine($"Text: {textColor}");

            var resources = Application.Current.Resources;

            resources["BackgroundColor"] = ConvertHexToBrush(backgroundColor);
            resources["ButtonColor"] = ConvertHexToBrush(buttonColor);
            resources["ButtonSpecialColor"] = ConvertHexToBrush(buttonSpecialColor);
            resources["ButtonEqualColor"] = ConvertHexToBrush(buttonEqualColor);
            resources["TextColor"] = ConvertHexToBrush(textColor);
            resources["DisplayBackground"] = ConvertHexToBrush(displayBackground);

            if (Application.Current.MainWindow != null)
            {
                Application.Current.MainWindow.Background = ConvertHexToBrush(backgroundColor);
                System.Diagnostics.Debug.WriteLine($"Window background set to: {backgroundColor}");
            }

            System.Diagnostics.Debug.WriteLine($"=== THEME APPLIED ===");
        }

        private Brush ConvertHexToBrush(string hex)
        {
            try
            {
                if (string.IsNullOrEmpty(hex))
                    return Brushes.Black;

                var converter = new BrushConverter();
                return (Brush)converter.ConvertFromString(hex);
            }
            catch
            {
                return Brushes.Black;
            }
        }

        private T? FindVisualChild<T>(DependencyObject parent, string name = "") where T : DependencyObject
        {
            for (int i = 0; i < VisualTreeHelper.GetChildrenCount(parent); i++)
            {
                var child = VisualTreeHelper.GetChild(parent, i);
                if (child != null)
                {
                    if (child is T typedChild && (string.IsNullOrEmpty(name) || (child as FrameworkElement)?.Name == name))
                        return typedChild;

                    var result = FindVisualChild<T>(child, name);
                    if (result != null)
                        return result;
                }
            }
            return null;
        }
    }
}