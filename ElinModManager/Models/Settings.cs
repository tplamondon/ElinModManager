using CommunityToolkit.Mvvm.ComponentModel;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElinModManager.Models
{
    /// <summary>
    /// Exists so we have 1 master settings available everywhere at load. Observable settings must interact with this class
    /// </summary>
    public static class Settings 
    {

        public static string JSONPATH { get; } = $"{Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData)}\\ElinModManager";
        public static string JSONFILE { get; } = $"{JSONPATH}\\settings.json";

        /// <summary>
        /// File path to Executable. loadorder.txt is with the executable.
        /// </summary>
        public static string? GameExePath;
        public static string? Language;
        /// <summary>
        /// Path to steam workshop
        /// </summary>
        public static string? GameWorkshopPath;
        /// <summary>
        /// Path to load order file
        /// </summary>
        public static string? LoadOrderFile { 
            get
            {
                if(GameExePath != null)
                {
                    string? fullPath = new FileInfo(GameExePath)?.Directory?.FullName;
                    if (fullPath != null)
                    {
                        return $"{fullPath}\\loadorder.txt";
                    }
                }
                return null;
            }  
        }
        public static string? GameLocalModsPath
        {
            get
            {
                if(GameExePath != null)
                {
                    string? fullPath = new FileInfo(GameExePath)?.Directory?.FullName;
                    if (fullPath != null)
                    {
                        return $"{fullPath}\\Package";
                    }
                }
                return null;
            }
        }


        static Settings()
        {
            LoadSettings();
        }

        /// <summary>
        /// Load all the settings
        /// </summary>
        public static void LoadSettings()
        {
            //if file exists, read and return that
            if (File.Exists(JSONFILE))
            {
                JObject jsonObject = JObject.Parse(File.ReadAllText($"{JSONFILE}"));
                SettingsJson? deserialised = jsonObject.ToObject<SettingsJson>();
                if (deserialised != null)
                {
                    GameExePath = deserialised.GameExePath;
                    GameWorkshopPath = deserialised.GameWorkshopPath;
                    Language = deserialised.Language;
                }
            }
            else
            {
                Directory.CreateDirectory(JSONPATH);
            }
        }

        /// <summary>
        /// Saves the settings
        /// </summary>
        public static void SaveSettings()
        {
            if (!Directory.Exists(JSONPATH))
            {
                Directory.CreateDirectory(JSONPATH);
            }
            SettingsJson settingsJson = new()
            {
                GameExePath = GameExePath,
                GameWorkshopPath = GameWorkshopPath,
                Language = Language,
            };
            JObject jsonObject = (JObject)JToken.FromObject(settingsJson);
            var jsonString = jsonObject.ToString();
            File.WriteAllText($"{JSONFILE}", jsonObject.ToString());
        }

        /// <summary>
        /// Helper class for JSON
        /// </summary>
        private class SettingsJson
        {
            public string? GameExePath;
            public string? GameWorkshopPath;
            public string? Language;
        }
    }
}
