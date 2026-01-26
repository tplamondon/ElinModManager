using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using ElinModManager.Commands;
using ElinModManager.Models;
using ElinModManager.Resources;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Resources;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace ElinModManager.ViewModel
{
    public class SettingsViewModel : INotifyPropertyChanged
    {
        public static SettingsObservable SettingsObject { get; set; } = new();

        public ICommand SaveSettingsCommand {  get; set; }
        public ICommand SelectExeCommand { get; set; }

        public static ObservableCollection<string?> AvailableLanguages { get; set; } = new();
        private string? _selectedLanguage;

        public string? SelectedLanguage
        {
            get => _selectedLanguage;
            set
            {
                if (_selectedLanguage != value)
                {
                    _selectedLanguage = value;
                    OnPropertyChanged();
                }
            }
        }

        public SettingsViewModel()
        {
            SettingsObject = new();
            SaveSettingsCommand = new RelayCommand(SaveSettings);
            SelectExeCommand = new RelayCommand(SelectExe);
            SelectedLanguage = Settings.Language;
            AvailableLanguages = new()
            {
                "",
                Labels.ResourceManager.GetString("Label_English", new CultureInfo("en")),
                Labels.ResourceManager.GetString("Label_Japanese", new CultureInfo("ja"))
            };
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string name = null)
        => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));

        private void SaveSettings()
        {
            if (SelectedLanguage == "")
            {
                SettingsObject.Language = null;
            }
            else
            {
                SettingsObject.Language = SelectedLanguage;
            }
            SettingsObject.SaveSettings();
            StaticCommands.ResetLanguage();
        }
        private void SelectExe()
        {
            var dialog = new Microsoft.Win32.OpenFileDialog();
            dialog.FileName = "Elin"; // Default file name
            dialog.DefaultExt = ".exe"; // Default file extension
            dialog.Filter = "Game Executable (*.exe)|*.exe"; // Filter files by extension
            // show open file dialog box
            bool? result = dialog.ShowDialog();

            //process open file dialog box results
            if (result == true)
            {
                //can do stuff now
                string fileName = dialog.FileName;
                SettingsObject.GameExePath = fileName;
            }
        }
    }
}
