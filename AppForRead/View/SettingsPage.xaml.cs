using AppForRead.Model;
using System.Collections.ObjectModel;

namespace AppForRead.View;

public partial class SettingsPage : ContentPage
{
    public ObservableCollection<ColorOption> AvailableColors { get; } = new()
        {
            new ColorOption("Синий", "#512BD4"),
            new ColorOption("Красный", "#FF0000"),
            new ColorOption("Зеленый", "#00FF00")
        };

    /// <summary>
    /// Collection of font names.
    /// </summary>
    public ObservableCollection<string> AvailableFonts { get; } = new()
        {
            "OpenSans",
            "Roboto",
            "Arial"
        };

    /// <summary>
    /// Size of font.
    /// </summary>
    private double _fontSize = 14;
    public double FontSize
    {
        get => _fontSize;
        set
        {
            _fontSize = value;
            OnPropertyChanged();
            UpdateFontSize();
        }
    }

    public SettingsPage()
    {
        InitializeComponent();
        BindingContext = this;

        if (Preferences.ContainsKey("PrimaryColor"))
        {
            var colorName = Preferences.Get("PrimaryColor", "Синий");
            ColorPicker.SelectedItem = AvailableColors.FirstOrDefault(c => c.Name == colorName);
        }

        if (Preferences.ContainsKey("FontFamily"))
            FontPicker.SelectedItem = Preferences.Get("FontFamily", "OpenSans");

        if (Preferences.ContainsKey("FontSize"))
            FontSizeSlider.Value = Preferences.Get("FontSize", 14.0);
    }

    private void UpdateStyles()
    {
        if (ColorPicker.SelectedItem is ColorOption selectedColor)
        {
            Application.Current.Resources["PrimaryColor"] = Color.FromArgb(selectedColor.Value);
        }

        if (FontPicker.SelectedItem is string selectedFont)
        {
            Application.Current.Resources["FontFamily"] = selectedFont;
        }

        UpdateFontSize();
    }

    private void UpdateFontSize()
    {
        Application.Current.Resources["FontSize"] = FontSize;
    }


    /// <summary>
    /// Action when Save settings button clicked.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void OnSaveSettingsClicked(object sender, EventArgs e)
    {
        if (ColorPicker.SelectedItem is ColorOption color)
            Preferences.Set("PrimaryColor", color.Name);

        if (FontPicker.SelectedItem is string font)
            Preferences.Set("FontFamily", font);

        Preferences.Set("FontSize", FontSize);

        UpdateStyles();

        DisplayAlert("Сохранено", "Настройки успешно сохранены", "OK");
    }

    /// <summary>
    /// Action when Reset settings button clicked. Reset default settings.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void OnResetSettingsClicked(object sender, EventArgs e)
    {
        Preferences.Clear();
        ColorPicker.SelectedItem = AvailableColors[0];
        FontPicker.SelectedItem = "OpenSans";
        FontSizeSlider.Value = 14;
        UpdateStyles();
    }
}