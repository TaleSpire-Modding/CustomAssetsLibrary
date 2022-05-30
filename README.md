## Custom Assets Library
The Custom Assets Library is the successor to EAL and transitions to use the TaleWeave Interface. This plugin enables r2modman routing for assets (Minis, Tiles and Props) but does not support audio in the r2modman profile directory. For audio support please use the AudioPlugin by PluginMasters as that's the superior alternative.

## Installing With R2ModMan
This package is designed specifically for R2ModMan and Talespire. 
You can install them via clicking on "Install with Mod Manager" or using the r2modman directly.

## Player Usage
This plugin used for players allows TaleSpire to search for TaleWeaver content in the R2Modman Directory.
This will allow direct download of TaleWeaver packs via R2Modman.

## Developer Usage
Inbuilt into this package is a Binary Index writer which will generate a TaleWeaver compliant index file mimicing the interface of TaleWeaver. This is incredibly experimental and is being worked on. With CAP, EAR compliant files can be converted to TW compliance. This still can pack audio files for the TaleWeaver route but does not support it in the profile directory of r2modman. This enables the packing of minis, props, and tiles with their portraits.

## Binary index writer usage
the index writer as a supplied interface and method to write the pack. PackContent data structure does not use blob references or ECS upon initial construction. 
```CSharp
var folderPath = $Path.Combine(Paths.PluginPath,"<ModPack>");
CustomAssetPlugin.Generate(folderPath);
```
After completion, the WritePack method will convert it into the ECS Blob structure and write the index file into the modpack directory.

## Changelog
- 1.0.0: Full release, Published on ThunderStore
- 0.10.0: Alpha release, Fixed binary index writer. Supports TW NGUID Folder name in r2modman directory.
- 0.9.0: Alpha release, Comes with binary index writer and TW patch

Shoutout to my Patreons on https://www.patreon.com/HolloFox recognising your
mighty contribution to my caffeine addiction:
- John Fuller
- [Tales Tavern](https://talestavern.com/) - MadWizard
