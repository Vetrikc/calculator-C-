using System;
using System.IO;
using System.Text.Json;
using System.Windows;
using SimpleCalculatorMVVM_4.Models;

namespace SimpleCalculatorMVVM_4.Services
{
    public class ConfigurationService
    {
        private const string ConfigFileName = "appsettings.json";
        private AppConfig _config;
        private readonly string _configPath;
        private string _lastError = "";

        public event EventHandler<AppConfig>? ConfigChanged;
        public event EventHandler<string>? ErrorOccurred;

        public ConfigurationService()
        {
            _configPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, ConfigFileName);
            LoadConfiguration();
        }

        public AppConfig Config => _config;
        public string LastError => _lastError;
        public bool IsConfigValid => _config != null;

        public void LoadConfiguration()
        {
            try
            {
                if (!File.Exists(_configPath))
                {
                    _lastError = $"Файл конфигурации не найден: {_configPath}. Создан файл с настройками по умолчанию.";
                    OnErrorOccurred(_lastError);
                    CreateDefaultConfiguration();
                    return;
                }

                string jsonContent = File.ReadAllText(_configPath);

                if (string.IsNullOrWhiteSpace(jsonContent))
                {
                    throw new JsonException("Файл конфигурации пуст");
                }

                _config = JsonSerializer.Deserialize<AppConfig>(jsonContent);

                if (_config == null)
                {
                    throw new InvalidOperationException("Не удалось десериализовать конфигурацию");
                }

                // Валидация параметров
                ValidateConfiguration();

                ApplyConfiguration();

                System.Diagnostics.Debug.WriteLine("Configuration loaded successfully");
                _lastError = "";
            }
            catch (FileNotFoundException ex)
            {
                _lastError = $"Файл конфигурации не найден: {ex.Message}";
                OnErrorOccurred(_lastError);
                CreateDefaultConfiguration();
            }
            catch (JsonException ex)
            {
                _lastError = $"Неверный формат конфигурационного файла: {ex.Message}";
                OnErrorOccurred(_lastError);
                CreateDefaultConfiguration();
            }
            catch (Exception ex)
            {
                _lastError = $"Ошибка загрузки конфигурации: {ex.Message}";
                OnErrorOccurred(_lastError);
                CreateDefaultConfiguration();
            }
        }

        private void ValidateConfiguration()
        {
            if (_config.WindowSettings.Width < 200 || _config.WindowSettings.Width > 1920)
            {
                _config.WindowSettings.Width = 300;
                _lastError = "Некорректная ширина окна, установлено значение по умолчанию";
            }

            if (_config.WindowSettings.Height < 200 || _config.WindowSettings.Height > 1080)
            {
                _config.WindowSettings.Height = 640;
                _lastError = "Некорректная высота окна, установлено значение по умолчанию";
            }

            if (string.IsNullOrEmpty(_config.Theme))
            {
                _config.Theme = "dark";
            }
        }

        private void CreateDefaultConfiguration()
        {
            _config = new AppConfig();
            SaveConfiguration();
            OnErrorOccurred("Создан файл конфигурации с настройками по умолчанию");
        }

        public void SaveConfiguration()
        {
            try
            {
                var options = new JsonSerializerOptions { WriteIndented = true };
                string jsonContent = JsonSerializer.Serialize(_config, options);
                File.WriteAllText(_configPath, jsonContent);
                System.Diagnostics.Debug.WriteLine($"Configuration saved to: {_configPath}");
            }
            catch (Exception ex)
            {
                _lastError = $"Ошибка сохранения конфигурации: {ex.Message}";
                OnErrorOccurred(_lastError);
            }
        }

        public void ApplyConfiguration()
        {
            try
            {
                if (Application.Current.MainWindow != null)
                {
                    var window = Application.Current.MainWindow;
                    window.Width = _config.WindowSettings.Width;
                    window.Height = _config.WindowSettings.Height;
                }

                ConfigChanged?.Invoke(this, _config);
            }
            catch (Exception ex)
            {
                _lastError = $"Ошибка применения конфигурации: {ex.Message}";
                OnErrorOccurred(_lastError);
            }
        }

        public void ApplyThemeToUI(string themeName)
        {
            var resources = Application.Current.Resources;
            ThemeColors? theme = null;

            if (_config.Themes != null && _config.Themes.ContainsKey(themeName))
            {
                theme = _config.Themes[themeName];
            }

            if (theme != null)
            {
                resources["BackgroundColor"] = ConvertHexToBrush(theme.BackgroundColor);
                resources["ButtonColor"] = ConvertHexToBrush(theme.ButtonColor);
                resources["ButtonSpecialColor"] = ConvertHexToBrush(theme.ButtonSpecialColor);
                resources["ButtonEqualColor"] = ConvertHexToBrush(theme.ButtonEqualColor);
                resources["TextColor"] = ConvertHexToBrush(theme.TextColor);
                resources["DisplayBackground"] = ConvertHexToBrush(theme.DisplayBackground);

                if (Application.Current.MainWindow != null)
                {
                    Application.Current.MainWindow.Background = ConvertHexToBrush(theme.BackgroundColor);
                }
            }
        }

        public void ApplyAgeStyle(string ageGroup)
        {
            var resources = Application.Current.Resources;
            AgeStyle? style = null;

            if (_config.AgeStyles != null && _config.AgeStyles.ContainsKey(ageGroup))
            {
                style = _config.AgeStyles[ageGroup];
            }

            if (style != null)
            {
                resources["BackgroundColor"] = ConvertHexToBrush(style.BackgroundColor);
                resources["ButtonColor"] = ConvertHexToBrush(style.ButtonColor);
                resources["ButtonSpecialColor"] = ConvertHexToBrush(style.ButtonSpecialColor);
                resources["ButtonEqualColor"] = ConvertHexToBrush(style.ButtonEqualColor);
                resources["TextColor"] = ConvertHexToBrush(style.TextColor);
                resources["DisplayBackground"] = ConvertHexToBrush(style.DisplayBackground);
                resources["FontSize"] = (double)style.FontSize;

                if (Application.Current.MainWindow != null)
                {
                    Application.Current.MainWindow.Background = ConvertHexToBrush(style.BackgroundColor);
                    Application.Current.MainWindow.FontSize = style.FontSize;
                }
            }
        }

        public void ApplyGenderStyle(string gender)
        {
            var resources = Application.Current.Resources;
            GenderStyle? style = null;

            if (_config.GenderStyles != null && _config.GenderStyles.ContainsKey(gender))
            {
                style = _config.GenderStyles[gender];
            }

            if (style != null)
            {
                resources["BackgroundColor"] = ConvertHexToBrush(style.BackgroundColor);
                resources["ButtonColor"] = ConvertHexToBrush(style.ButtonColor);
                resources["ButtonSpecialColor"] = ConvertHexToBrush(style.ButtonSpecialColor);
                resources["TextColor"] = ConvertHexToBrush(style.TextColor);
                resources["DisplayBackground"] = ConvertHexToBrush(style.DisplayBackground);

                if (Application.Current.MainWindow != null)
                {
                    Application.Current.MainWindow.Background = ConvertHexToBrush(style.BackgroundColor);
                }
            }
        }

        private System.Windows.Media.Brush ConvertHexToBrush(string hex)
        {
            try
            {
                if (string.IsNullOrEmpty(hex))
                    return System.Windows.Media.Brushes.Black;

                var converter = new System.Windows.Media.BrushConverter();
                return (System.Windows.Media.Brush)converter.ConvertFromString(hex);
            }
            catch
            {
                return System.Windows.Media.Brushes.Black;
            }
        }

        private void OnErrorOccurred(string error)
        {
            ErrorOccurred?.Invoke(this, error);
            MessageBox.Show(error, "Ошибка конфигурации", MessageBoxButton.OK, MessageBoxImage.Warning);
        }

        public void ChangeTheme(string themeName)
        {
            _config.Theme = themeName;
            _config.AccessibilityMode = "none";
            SaveConfiguration();
            ApplyThemeToUI(themeName);
        }

        public void SetAccessibilityMode(string mode, string? styleName = null)
        {
            _config.AccessibilityMode = mode;
            SaveConfiguration();

            switch (mode)
            {
                case "highContrast":
                    ApplyThemeToUI("highContrast");
                    break;
                case "gender":
                    if (!string.IsNullOrEmpty(styleName))
                        ApplyGenderStyle(styleName);
                    break;
                case "age":
                    if (!string.IsNullOrEmpty(styleName))
                        ApplyAgeStyle(styleName);
                    break;
                default:
                    ApplyThemeToUI(_config.Theme);
                    break;
            }
        }

        public void ToggleSound()
        {
            _config.SoundEnabled = !_config.SoundEnabled;
            SaveConfiguration();
        }

        public void ToggleCursor()
        {
            _config.CursorEnabled = !_config.CursorEnabled;
            SaveConfiguration();

            if (_config.CursorEnabled)
                CursorService.Initialize();
            else
                CursorService.RestoreDefaultCursor();
        }
    }
}