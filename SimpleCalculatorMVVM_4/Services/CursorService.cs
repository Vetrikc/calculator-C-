using System;
using System.IO;
using System.Windows.Input;

namespace SimpleCalculatorMVVM_4.Services
{
    public static class CursorService
    {
        private static bool _isInitialized = false;

        public static void Initialize()
        {
            if (_isInitialized) return;

            try
            {
                // Путь к файлу курсора в выходной директории
                string cursorPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory,
                    "Assets", "Cursors", "calculator.cur");

                System.Diagnostics.Debug.WriteLine($"Looking for cursor at: {cursorPath}");

                if (File.Exists(cursorPath))
                {
                    using (var stream = new FileStream(cursorPath, FileMode.Open, FileAccess.Read))
                    {
                        var cursor = new Cursor(stream);
                        Mouse.OverrideCursor = cursor;
                        _isInitialized = true;
                        System.Diagnostics.Debug.WriteLine("Cursor loaded successfully!");
                    }
                }
                else
                {
                    System.Diagnostics.Debug.WriteLine($"Cursor file NOT found at: {cursorPath}");
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Cursor error: {ex.Message}");
            }
        }

        public static void RestoreDefaultCursor()
        {
            Mouse.OverrideCursor = null;
        }
    }
}