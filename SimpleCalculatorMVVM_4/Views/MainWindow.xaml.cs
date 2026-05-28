using System;
using System.Windows;
using SimpleCalculatorMVVM_4.Services;

namespace SimpleCalculatorMVVM_4.Views
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Exit_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void About_Click(object sender, RoutedEventArgs e)
        {
            var aboutDialog = new AboutDialog();
            aboutDialog.Owner = this;
            aboutDialog.ShowDialog();
        }

        // Для слабовидящих (Высокий контраст)
        private void SetHighContrastTheme(object sender, RoutedEventArgs e)
        {
            var configService = (App.Current as App)?.GetConfigService();
            configService?.SetAccessibilityMode("highContrast");
        }

        // Светлый и темный стили
        private void SetDarkTheme(object sender, RoutedEventArgs e)
        {
            var configService = (App.Current as App)?.GetConfigService();
            configService?.ChangeTheme("dark");
        }

        private void SetLightTheme(object sender, RoutedEventArgs e)
        {
            var configService = (App.Current as App)?.GetConfigService();
            configService?.ChangeTheme("light");
        }

        // По гендерному признаку
        private void SetMaleStyle(object sender, RoutedEventArgs e)
        {
            var configService = (App.Current as App)?.GetConfigService();
            configService?.SetAccessibilityMode("gender", "male");
        }

        private void SetFemaleStyle(object sender, RoutedEventArgs e)
        {
            var configService = (App.Current as App)?.GetConfigService();
            configService?.SetAccessibilityMode("gender", "female");
        }

        // По возрастному признаку
        private void SetChildrenStyle(object sender, RoutedEventArgs e)
        {
            var configService = (App.Current as App)?.GetConfigService();
            configService?.SetAccessibilityMode("age", "children");
        }

        private void SetYouthStyle(object sender, RoutedEventArgs e)
        {
            var configService = (App.Current as App)?.GetConfigService();
            configService?.SetAccessibilityMode("age", "youth");
        }

        private void SetAdultStyle(object sender, RoutedEventArgs e)
        {
            var configService = (App.Current as App)?.GetConfigService();
            configService?.SetAccessibilityMode("age", "adult");
        }

        private void SetElderlyStyle(object sender, RoutedEventArgs e)
        {
            var configService = (App.Current as App)?.GetConfigService();
            configService?.SetAccessibilityMode("age", "elderly");
        }

        private void ToggleSound(object sender, RoutedEventArgs e)
        {
            var configService = (App.Current as App)?.GetConfigService();
            configService?.ToggleSound();
            ToggleSoundMenuItem.Header = configService?.Config.SoundEnabled == true ? "Выключить звук" : "Включить звук";
        }

    }
}