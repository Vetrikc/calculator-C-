using System;
using SimpleCalculatorMVVM_4.Services;
using SimpleCalculatorMVVM_4.ViewModels;
using System.Windows;
using SimpleCalculatorMVVM_4.Views;

namespace SimpleCalculatorMVVM_4
{
    public partial class App : Application
    {
        private ConfigurationService _configService;

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            _configService = new ConfigurationService();

            // Подписываемся на ошибки конфигурации
            _configService.ErrorOccurred += OnConfigError;

            if (_configService.Config.CursorEnabled)
            {
                CursorService.Initialize();
            }

            // Применяем настройки окна
            if (_configService.Config.WindowSettings != null)
            {
                if (MainWindow != null)
                {
                    MainWindow.Width = _configService.Config.WindowSettings.Width;
                    MainWindow.Height = _configService.Config.WindowSettings.Height;
                }
            }

            // Применяем тему
            ApplyThemeFromConfig();

            var mainWindow = new MainWindow();
            var viewModel = new CalculatorViewModel(_configService);
            mainWindow.DataContext = viewModel;
            mainWindow.Show();
        }

        private void ApplyThemeFromConfig()
        {
            switch (_configService.Config.AccessibilityMode)
            {
                case "highContrast":
                    _configService.ApplyThemeToUI("highContrast");
                    break;
                case "gender":
                    _configService.ApplyGenderStyle("male");
                    break;
                case "age":
                    _configService.ApplyAgeStyle("adult");
                    break;
                default:
                    _configService.ApplyThemeToUI(_configService.Config.Theme);
                    break;
            }
        }

        private void OnConfigError(object? sender, string error)
        {
            // Ошибка уже показана в MessageBox, просто логируем
            System.Diagnostics.Debug.WriteLine($"Config Error: {error}");
        }

        protected override void OnExit(ExitEventArgs e)
        {
            CursorService.RestoreDefaultCursor();

            if (MainWindow?.DataContext is IDisposable disposable)
            {
                disposable.Dispose();
            }

            base.OnExit(e);
        }

        public ConfigurationService GetConfigService()
        {
            return _configService;
        }
    }
}