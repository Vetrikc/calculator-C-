using System.Text.Json.Serialization;

namespace SimpleCalculatorMVVM_4.Models
{
    public class AppConfig
    {
        public WindowSettings WindowSettings { get; set; } = new();
        public string Theme { get; set; } = "dark";
        public FontSettings FontSettings { get; set; } = new();
        public string AccessibilityMode { get; set; } = "none";
        public bool SoundEnabled { get; set; } = true;
        public bool CursorEnabled { get; set; } = true;
        public DatabaseSettings DatabaseSettings { get; set; } = new();

        public Dictionary<string, ThemeColors> Themes { get; set; } = new();
        public Dictionary<string, GenderStyle> GenderStyles { get; set; } = new();
        public Dictionary<string, AgeStyle> AgeStyles { get; set; } = new();
    }

    public class WindowSettings
    {
        public int Width { get; set; } = 300;
        public int Height { get; set; } = 640;
        public string BackgroundColor { get; set; } = "#0F0F0F";
        public string ResizeMode { get; set; } = "NoResize";
    }

    public class FontSettings
    {
        public string Family { get; set; } = "Segoe UI";
        public int Size { get; set; } = 14;
        public bool IsCustomFont { get; set; } = false;
        public string CustomFontPath { get; set; } = "";
    }

    public class DatabaseSettings
    {
        public string ConnectionString { get; set; } = "";
        public string ProviderName { get; set; } = "";
        public bool EnableLogging { get; set; } = false;
    }

    public class ThemeColors
    {
        public string BackgroundColor { get; set; } = "";
        public string ButtonColor { get; set; } = "";
        public string ButtonSpecialColor { get; set; } = "";
        public string ButtonEqualColor { get; set; } = "";
        public string TextColor { get; set; } = "";
        public string DisplayBackground { get; set; } = "";
    }

    public class GenderStyle
    {
        public string BackgroundColor { get; set; } = "";
        public string ButtonColor { get; set; } = "";
        public string ButtonSpecialColor { get; set; } = "";
        public string TextColor { get; set; } = "";
        public string DisplayBackground { get; set; } = "";
    }

    public class AgeStyle
    {
        public string BackgroundColor { get; set; } = "";
        public string ButtonColor { get; set; } = "";
        public string ButtonSpecialColor { get; set; } = "";
        public string ButtonEqualColor { get; set; } = "";
        public string TextColor { get; set; } = "";
        public string DisplayBackground { get; set; } = "";
        public int FontSize { get; set; } = 14;
    }
}