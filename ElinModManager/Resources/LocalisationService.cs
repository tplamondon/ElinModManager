using System.ComponentModel;
using System.Globalization;

namespace ElinModManager.Resources
{
    public class LocalisationService : INotifyPropertyChanged
    {
        public static LocalisationService Instance { get; } = new();

        public event PropertyChangedEventHandler? PropertyChanged;

        public string this[string key] =>
            Labels.ResourceManager.GetString(key, CultureInfo.CurrentUICulture)
            ?? $"!{key}!";

        public void ChangeCulture(string cultureName)
        {
            var culture = new CultureInfo(cultureName);

            Thread.CurrentThread.CurrentCulture = culture;
            Thread.CurrentThread.CurrentUICulture = culture;

            // Force all bindings to refresh
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(null));
        }
    }
}
