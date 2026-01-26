using ElinModManager.Models;
using ElinModManager.Resources;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElinModManager.Commands
{
    public static class StaticCommands
    {
        public static void ChangeToEnglish()
        {
            LocalisationService.Instance.ChangeCulture("en");
        }
        public static void ChangeToJapanese()
        {
            LocalisationService.Instance.ChangeCulture("ja");
        }

        public static void ResetLanguage()
        {
            if(Settings.Language == Labels.ResourceManager.GetString("Label_English", new CultureInfo("en")))
            {
                ChangeToEnglish();
            }
            else if(Settings.Language == Labels.ResourceManager.GetString("Label_Japanese", new CultureInfo("ja")))
            {
                ChangeToJapanese();
            }
        }
    }
}
