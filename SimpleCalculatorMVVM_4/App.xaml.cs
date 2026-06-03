using System;
using System.Diagnostics;
using System.Windows;

using Calculator.Services;
using SimpleCalculatorMVVM_4.ViewModels;
using SimpleCalculatorMVVM_4.Views;

namespace SimpleCalculatorMVVM_4
{
    /// <summary>
    /// App.xaml.cs для демонстрации динамической загрузки DLL (Задание 3)
    /// </summary>
    public partial class App : Application
    {
        // Укажите правильный путь к Calculator.Services.dll
        // Например: @"C:\Users\vlad\Desktop\Assignment_2\Calculator.Services.dll"
        private const string SERVICES_DLL_PATH = @"C:\Users\vlad\source\repos\Vetrikc\calculator-C-\SimpleCalculatorMVVM_4\bin\Release\net8.0-windows\Calculator.Services.dll";

        private ConfigurationService _configService;

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            // 1. Динамическая загрузка DLL
            try
            {
                Debug.WriteLine($"Загрузка DLL: {SERVICES_DLL_PATH}");
                DynamicAssemblyLoader.LoadDllDynamically(SERVICES_DLL_PATH);
                Debug.WriteLine("DLL успешно загружена!");
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    $"Ошибка загрузки Calculator.Services.dll:\n{ex.Message}\n\n" +
                    $"Проверьте путь: {SERVICES_DLL_PATH}",
                    "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                Shutdown();
                return;
            }

            // 2. Инициализация сервисов
            try
            {
                _configService = new ConfigurationService();
                _configService.ErrorOccurred += OnConfigError;

                if (_configService.Config.CursorEnabled)
                    CursorService.Initialize();

                // Применяем настройки окна
                if (MainWindow != null && _configService.Config.WindowSettings != null)
                {
                    MainWindow.Width = _configService.Config.WindowSettings.Width;
                    MainWindow.Height = _configService.Config.WindowSettings.Height;
                }

                ApplyThemeFromConfig();

                var mainWindow = new MainWindow();
                var viewModel = new CalculatorViewModel(_configService);
                mainWindow.DataContext = viewModel;
                mainWindow.Show();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка инициализации: {ex.Message}", "Ошибка",
                    MessageBoxButton.OK, MessageBoxImage.Error);
                Shutdown();
            }
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
                    break;   // добавлен break
                default:
                    _configService.ApplyThemeToUI(_configService.Config.Theme);
                    break;
            }
        }

        private void OnConfigError(object? sender, string error)
        {
            Debug.WriteLine($"Config Error: {error}");
            MessageBox.Show($"Ошибка конфигурации: {error}", "Предупреждение",
                MessageBoxButton.OK, MessageBoxImage.Warning);
        }

        protected override void OnExit(ExitEventArgs e)
        {
            CursorService.RestoreDefaultCursor();
            if (MainWindow?.DataContext is IDisposable disposable)
                disposable.Dispose();
            base.OnExit(e);
        }

        public ConfigurationService GetConfigService() => _configService;
    }
}