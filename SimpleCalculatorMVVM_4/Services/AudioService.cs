using System;
using System.IO;
using System.Media;

namespace SimpleCalculatorMVVM_4.Services
{
    public class AudioService : IDisposable
    {
        private SoundPlayer _buttonClickPlayer;
        private SoundPlayer _operationPlayer;
        private SoundPlayer _errorPlayer;
        private bool _disposed = false;

        public AudioService()
        {
            InitializePlayers();
        }

        private void InitializePlayers()
        {
            try
            {
                string basePath = AppDomain.CurrentDomain.BaseDirectory;

                // Пути к звуковым файлам
                string buttonPath = Path.Combine(basePath, "Assets", "Sounds", "button_click.wav");
                string operationPath = Path.Combine(basePath, "Assets", "Sounds", "operation.wav");
                string errorPath = Path.Combine(basePath, "Assets", "Sounds", "error.wav");

                System.Diagnostics.Debug.WriteLine($"Button sound path: {buttonPath}");
                System.Diagnostics.Debug.WriteLine($"Operation sound path: {operationPath}");
                System.Diagnostics.Debug.WriteLine($"Error sound path: {errorPath}");

                // Загружаем звуки
                if (File.Exists(buttonPath))
                {
                    _buttonClickPlayer = new SoundPlayer(buttonPath);
                    _buttonClickPlayer.Load();
                    System.Diagnostics.Debug.WriteLine("Button sound loaded!");
                }

                if (File.Exists(operationPath))
                {
                    _operationPlayer = new SoundPlayer(operationPath);
                    _operationPlayer.Load();
                    System.Diagnostics.Debug.WriteLine("Operation sound loaded!");
                }

                if (File.Exists(errorPath))
                {
                    _errorPlayer = new SoundPlayer(errorPath);
                    _errorPlayer.Load();
                    System.Diagnostics.Debug.WriteLine("Error sound loaded!");
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Audio init error: {ex.Message}");
            }
        }

        public void PlayButtonClick()
        {
            try
            {
                _buttonClickPlayer?.Play();
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"PlayButtonClick error: {ex.Message}");
            }
        }

        public void PlayOperation()
        {
            try
            {
                _operationPlayer?.Play();
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"PlayOperation error: {ex.Message}");
            }
        }

        public void PlayError()
        {
            try
            {
                _errorPlayer?.Play();
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"PlayError error: {ex.Message}");
            }
        }
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    _buttonClickPlayer?.Dispose();
                    _operationPlayer?.Dispose();
                    _errorPlayer?.Dispose();
                }
                _disposed = true;
            }
        }
    }
}