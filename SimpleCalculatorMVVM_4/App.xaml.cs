using System.Windows;
using SimpleCalculatorMVVM_4.Services;

namespace SimpleCalculatorMVVM_4
{
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            // Инициализируем кастомный курсор
            CursorService.Initialize();
        }

        protected override void OnExit(ExitEventArgs e)
        {
            CursorService.RestoreDefaultCursor();

            // Освобождаем ресурсы ViewModel
            if (MainWindow?.DataContext is IDisposable disposable)
            {
                disposable.Dispose();
            }

            base.OnExit(e);
        }
    }
}