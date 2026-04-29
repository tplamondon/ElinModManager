using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElinModManager.Models
{
    public partial class SettingsObservable : ObservableObject
    {
        [ObservableProperty]
        private static string? gameExePath;
        [ObservableProperty]
        private static string? gameWorkshopPath;
        [ObservableProperty]
        private static string? language;
        
        public SettingsObservable()
        {
            gameExePath = Settings.GameExePath;
            gameWorkshopPath = Settings.GameWorkshopPath;
            language = Settings.Language;
        }
        /// <summary>
        /// Refresh settings to match the static class
        /// </summary>
        public void Refresh()
        {
            gameExePath = Settings.GameExePath;
            gameWorkshopPath = Settings.GameWorkshopPath;
            language = Settings.Language;
        }

        /// <summary>
        /// Save settings to JSON and the static class
        /// </summary>
        public void SaveSettings()
        {
            Settings.GameExePath = gameExePath;
            Settings.GameWorkshopPath = gameWorkshopPath;
            Settings.Language = language;
            Settings.SaveSettings();
        }
    }
}
