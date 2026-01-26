# ElinModManager

ì˙ñ{åÍî≈ÇÕ[Ç±ÇøÇÁÇ≈Ç∑](README.jp.md)ÅB

A mod manager for the game [Elin](https://store.steampowered.com/app/2135150/Elin/).

UI and Colours based on [LaughingLeaders BG3ModManager](https://github.com/LaughingLeader/BG3ModManager)


# How to use

## Setup

1. Run the game at least once.
2. Make sure you have [.NET 8.0](https://dotnet.microsoft.com/en-us/download/dotnet/thank-you/runtime-desktop-8.0.15-windows-x64-installer) and [the latest C++ redistributable](https://aka.ms/vs/17/release/vc_redist.x64.exe) installed.
3. Extract the latest release somewhere on your computer. Don't extract it to a protect folder please (such as program files).

## Settings

First you should adjust your settings.

![settings screenshot](Images/settings.png)

- **Game Path**: The path to your Elin game exuctuable. Click on the box to open a file dialog to select your executable file.
- **Default Language**: The default language the mod manager uses when opening.

## Main Window

The game path must be set before you can use the mod manager.

**To see newly added mods, you must load the game once to have it add the new mods to loadorder.txt**.

![main window](Images/mainWindow.png)

Pressing refresh will reorder the mods based on your current loadorder.txt file.

To order your mods, just drag and drop them how you want them to be ordered. Mods on the left side will be active, 
and mods on the right side will be inactive.

Once you have ordered them, pressing the save icon will export your load order.

# Contributing.

If you wish to contribute, you can make a pull request or create an issue on GitHub.

Regarding English labels, only Canadian or UK spelling will be accepted.