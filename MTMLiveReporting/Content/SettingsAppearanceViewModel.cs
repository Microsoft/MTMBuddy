﻿//------------------------------------------------------------------------------------------------------- 
// Copyright (C) Microsoft. All rights reserved. 
// Licensed under the MIT license. See LICENSE.txt file in the project root for full license information. 
//------------------------------------------------------------------------------------------------------- 
using System.ComponentModel;
using System.Linq;
using System.Windows.Media;
using FirstFloor.ModernUI.Presentation;

namespace MTMLiveReporting.Content
{
    /// <summary>
    ///     A simple view model for configuring theme, font and accent colors.
    /// </summary>
    public class SettingsAppearanceViewModel
        : NotifyPropertyChanged
    {
        private const string FontSmall = "small";
        private const string FontLarge = "large";

   

        private Color _selectedAccentColor;
        private string _selectedFontSize;
        private Link _selectedTheme;

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public SettingsAppearanceViewModel()
        {
            // add the default themes
            Themes.Add(new Link {DisplayName = "dark", Source = AppearanceManager.DarkThemeSource});
            Themes.Add(new Link {DisplayName = "light", Source = AppearanceManager.LightThemeSource});

            SelectedFontSize = AppearanceManager.Current.FontSize == FontSize.Large ? FontLarge : FontSmall;
            SyncThemeAndColor();

            AppearanceManager.Current.PropertyChanged += OnAppearanceManagerPropertyChanged;
        }

        public LinkCollection Themes { get; } = new LinkCollection();

        public string[] FontSizes => new[] {FontSmall, FontLarge};

        public Color[] AccentColors { get; } = {
            Color.FromRgb(0xa4, 0xc4, 0x00), // lime
            Color.FromRgb(0x60, 0xa9, 0x17), // green
            Color.FromRgb(0x00, 0x8a, 0x00), // emerald
            Color.FromRgb(0x00, 0xab, 0xa9), // teal
            Color.FromRgb(0x1b, 0xa1, 0xe2), // cyan
            Color.FromRgb(0x00, 0x50, 0xef), // cobalt
            Color.FromRgb(0x6a, 0x00, 0xff), // indigo
            Color.FromRgb(0xaa, 0x00, 0xff), // violet
            Color.FromRgb(0xf4, 0x72, 0xd0), // pink
            Color.FromRgb(0xd8, 0x00, 0x73), // magenta
            Color.FromRgb(0xa2, 0x00, 0x25), // crimson
            Color.FromRgb(0xe5, 0x14, 0x00), // red
            Color.FromRgb(0xfa, 0x68, 0x00), // orange
            Color.FromRgb(0xf0, 0xa3, 0x0a), // amber
            Color.FromRgb(0xe3, 0xc8, 0x00), // yellow
            Color.FromRgb(0x82, 0x5a, 0x2c), // brown
            Color.FromRgb(0x6d, 0x87, 0x64), // olive
            Color.FromRgb(0x64, 0x76, 0x87), // steel
            Color.FromRgb(0x76, 0x60, 0x8a), // mauve
            Color.FromRgb(0x87, 0x79, 0x4e) // taupe
        };

        public Link SelectedTheme
        {
            get { return _selectedTheme; }
            set
            {
                if (_selectedTheme != value)
                {
                    _selectedTheme = value;
                    OnPropertyChanged("SelectedTheme");

                    // and update the actual theme
                    AppearanceManager.Current.ThemeSource = value.Source;
                    DataGetter.SaveConfig("Theme", value.DisplayName);
                }
            }
        }

        public string SelectedFontSize
        {
            get { return _selectedFontSize; }
            set
            {
                if (_selectedFontSize != value)
                {
                    _selectedFontSize = value;
                    OnPropertyChanged("SelectedFontSize");

                    AppearanceManager.Current.FontSize = value == FontLarge ? FontSize.Large : FontSize.Small;
                    DataGetter.SaveConfig("FontSize", value);
                }
            }
        }

        public Color SelectedAccentColor
        {
            get { return _selectedAccentColor; }
            set
            {
                if (_selectedAccentColor != value)
                {
                    _selectedAccentColor = value;
                    OnPropertyChanged("SelectedAccentColor");

                    AppearanceManager.Current.AccentColor = value;
                }
            }
        }

        private void SyncThemeAndColor()
        {
            // synchronizes the selected viewmodel theme with the actual theme used by the appearance manager.
            SelectedTheme = Themes.FirstOrDefault(l => l.Source.Equals(AppearanceManager.Current.ThemeSource));

            // and make sure accent color is up-to-date
            SelectedAccentColor = AppearanceManager.Current.AccentColor;
            DataGetter.SaveConfig("R", AppearanceManager.Current.AccentColor.R.ToString());
            DataGetter.SaveConfig("A", AppearanceManager.Current.AccentColor.A.ToString());
            DataGetter.SaveConfig("G", AppearanceManager.Current.AccentColor.G.ToString());
            DataGetter.SaveConfig("B", AppearanceManager.Current.AccentColor.B.ToString());
        }

        private void OnAppearanceManagerPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "ThemeSource" || e.PropertyName == "AccentColor")
            {
                SyncThemeAndColor();
            }
        }
    }
}