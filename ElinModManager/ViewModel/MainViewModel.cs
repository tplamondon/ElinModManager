using CommunityToolkit.Mvvm.Input;
using ElinModManager.Commands;
using ElinModManager.Models;
using ElinModManager.Resources;
using ElinModManager.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Xml.Linq;

namespace ElinModManager.ViewModel
{
    public class MainViewModel
    {

        public ObservableCollection<Mod> ActiveMods { get; set; } = new ObservableCollection<Mod>();
        public ObservableCollection<Mod> InactiveMods { get; set; } = new ObservableCollection<Mod>();

        public ICommand ShowSettingsCommand { get; set; }
        public ICommand RefreshCommand { get; set; }
        public ICommand SwitchEnglishCommand { get; set; }
        public ICommand SwitchJapaneseCommand { get; set; }
        public ICommand ExportOrderCommand { get; set; }

        
        //context menu commands
        public ICommand OpenModFolderCommand { get; set; }

        public MainViewModel()
        {

            ShowSettingsCommand = new RelayCommand(OpenSettingsClicked);
            SwitchEnglishCommand = new RelayCommand(StaticCommands.ChangeToEnglish);
            SwitchJapaneseCommand = new RelayCommand(StaticCommands.ChangeToJapanese);
            ExportOrderCommand = new RelayCommand(ExportOrder);
            RefreshCommand = new RelayCommand(Refresh);

            OpenModFolderCommand = new RelayCommand<Mod?>(OpenModFolder);

            LoadMods();

            // Setup language from settings
            if(Settings.Language == Labels.ResourceManager.GetString("Label_English", new CultureInfo("en"))){
                LocalisationService.Instance.ChangeCulture("en");
            }
            else if (Settings.Language == Labels.ResourceManager.GetString("Label_Japanese", new CultureInfo("ja")))
            {
                LocalisationService.Instance.ChangeCulture("ja");
            }
            // default to english
            else
            {
                LocalisationService.Instance.ChangeCulture("en");
            }
        }



        private void OpenSettingsClicked()
        {
            SettingsView settingsView = new SettingsView();
            settingsView.Show();
        }
        private void Refresh()
        {
            LoadMods();
        }


        private void ExportOrder()
        {
            //loop through ActiveMods then InactiveMods
            if(Settings.LoadOrderFile != null)
            {
                using var file = new StreamWriter(Settings.LoadOrderFile,false);
                foreach(var active in ActiveMods)
                {
                    string text = $"{active.Directory},1";
                    file.WriteLine(text);
                }
                foreach(var inactive in InactiveMods)
                {
                    string text = $"{inactive.Directory},0";
                    file.WriteLine(text);
                }
                file.Close();
            }
            
        }

        private void AddMod(Mod mod, bool active)
        {
            if (active)
            {
                ActiveMods.Add(mod);
            }
            else
            {
                InactiveMods.Add(mod);
            }
        }

        /// <summary>
        /// Loads the mod info from a given directory and returns it. null if can't get mod info
        /// </summary>
        /// <param name="directory"></param>
        /// <returns></returns>
        private Mod? GetModInfo(string directory)
        {
            if(!File.Exists($"{directory}\\package.xml")) { return null; }

            XDocument modPackage = XDocument.Load($"{directory}\\package.xml");

            if (modPackage == null){ return null; }

            Mod? mod = null;
            foreach(var metaNode in modPackage.Elements())
            {
                if(metaNode.Name.LocalName.ToLower() == "meta")
                {
                    //found a package.xml with the first element being <meta> which matches expected format. We can parse the mod now
                    mod = new Mod();
                    mod.Directory = directory;
                    //go through each element and assign values to appropriate things in object
                    foreach(var node in metaNode.Elements())
                    {
                        switch (node.Name.LocalName.ToLower())
                        {
                            case "title":
                                mod.Title = node.Value;
                                break;
                            case "id":
                                mod.Id = node.Value;
                                break;
                            case "author":
                                mod.Author = node.Value;
                                break;
                            case "loadpriority":
                                mod.LoadPriority = int.Parse(node.Value);
                                break;
                            case "description":
                                mod.Description = node.Value;
                                break;
                            case "tags":
                                mod.Tags = node.Value.Split(',').ToList();
                                break;
                            case "version":
                                mod.Version = node.Value;
                                break;
                            default:
                                //dont' do anything. Unknown node
                                break;
                        }
                    }

                    //found meta node so we can exit the loop
                    break;
                }

            }
            return mod;
        }


        /// <summary>
        ///  Load all the mods
        /// </summary>
        private void LoadMods()
        {
            //helper local methods
            void LoadFromLoadOrderFile()
            {
                if (Settings.GameExePath != null)
                {
                    if (File.Exists(Settings.LoadOrderFile))
                    {
                        // load order exists and is found
                        var lines = File.ReadLines(Settings.LoadOrderFile);
                        foreach (var line in lines)
                        {
                            //splitLine[0] is path, splitLine[1] is if enabled
                            var splitLine = line.Split(',');
                            Mod? mod = GetModInfo(splitLine[0]);
                            bool isActive = splitLine[1] == "1";

                            if (mod == null) continue;

                            AddMod(mod, isActive);
                        }

                    }
                }
            }
            void LoadUnloadedWorkshopMods()
            {
                if (Settings.GameWorkshopPath != null)
                {
                    if (Directory.Exists(Settings.GameWorkshopPath))
                    {
                        var directories = Directory.GetDirectories(Settings.GameWorkshopPath);
                        foreach (var directory in directories)
                        {
                            //check if mod is already loaded from load order file
                            if (ActiveMods.Any(m => m.Directory == directory) || InactiveMods.Any(m => m.Directory == directory))
                            {
                                continue;
                            }
                            Mod? mod = GetModInfo(directory);
                            if (mod != null)
                            {
                                AddMod(mod, false);
                            }
                        }
                    }
                }
            }
            void LoadUnloadedLocalMods()
            {
                //default game mods that come with the game. Don't include these in load order
                List<string> GAME_MODS = new List<string>() { "_Elona", "_Lang_Chinese", "Mod_Slot" };
                if (Settings.GameLocalModsPath != null && Directory.Exists(Settings.GameLocalModsPath))
                {
                    var directories = Directory.EnumerateDirectories(Settings.GameLocalModsPath);
                    foreach (var directory in directories)
                    {
                        //get folder name of each mod for checking "mods" we don't wnat to include
                        var folderName = Path.GetFileName(directory);
                        if (GAME_MODS.Contains(folderName))
                        {
                            continue;
                        }

                        //check if mod is already loaded from load order file
                        if (ActiveMods.Any(m => m.Directory == directory) || InactiveMods.Any(m => m.Directory == directory))
                        {
                            continue;
                        }
                        Mod? mod = GetModInfo(directory);
                        if (mod != null)
                        {
                            AddMod(mod, false);
                        }
                    }
                }
            }
            
            //clear this out to reset
            ActiveMods.Clear();
            InactiveMods.Clear();

            //get load order file mods first
            LoadFromLoadOrderFile();
            //load workshop mods that haven't been put in load order file (usually newly subscribed mods and haven't rerun game since)
            LoadUnloadedWorkshopMods();
            //load local mods not in load order file (usually newly downloaded or created mods)
            LoadUnloadedLocalMods();
        }


        // Open Mod Commands
        private void OpenModFolder(Mod? mod)
        {
            Console.WriteLine(mod?.Author);
        }
    }
}
