﻿using Lisha.Blazor.Infrastructure.Preferences;
using Lisha.Blazor.Infrastructure.Theme;
using MudBlazor;

namespace Lisha.Blazor.Shared
{
    public partial class BaseLayout
    {
        private ClientPreference? _themePreference;
        private MudTheme _currentTheme = new LightTheme();
        private bool _themeDrawerOpen;
        private bool _rightToLeft;

        protected override async Task OnInitializedAsync()
        {
            _themePreference = await ClientPreferences.GetPreference() as ClientPreference;
            if (_themePreference == null) _themePreference = new ClientPreference();
            SetCurrentTheme(_themePreference);

            Snackbar.Add("Lisha - Version 1.0", Severity.Success);
        }

        private async Task ThemePreferenceChanged(ClientPreference themePreference)
        {
            SetCurrentTheme(themePreference);
            await ClientPreferences.SetPreference(themePreference);
        }

        private void SetCurrentTheme(ClientPreference themePreference)
        {
            _currentTheme = themePreference.IsDarkMode ? new DarkTheme() : new LightTheme();
            _currentTheme.Palette.Primary = themePreference.PrimaryColor;
            _currentTheme.Palette.Secondary = themePreference.SecondaryColor;
            _currentTheme.LayoutProperties.DefaultBorderRadius = $"{themePreference.BorderRadius}px";
            _currentTheme.LayoutProperties.DefaultBorderRadius = $"{themePreference.BorderRadius}px";
            _rightToLeft = themePreference.IsRTL;
        }
    }
}
