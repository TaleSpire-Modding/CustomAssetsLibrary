## Custom Assets Library
The Custom Assets Library is the successor to EAL and transitions to use the TaleWeave Interface. 

## Installing With R2ModMan
This package is designed specifically for R2ModMan and Talespire. 
You can install them via clicking on "Install with Mod Manager" or using the r2modman directly.

## Player Usage
This plugin used for players allows TaleSpire to search for TaleWeaver content in the R2Modman Directory.
This will allow direct download of TaleWeaver packs via R2Modman.

## Developer Usage
Inbuilt into this package is a Binary Index writer which will generate a TaleWeaver compliant index file mimicing the interface of TaleWeaver. This is incredibly experimental and is being worked on. With CAP, EAR compliant files can be converted to TW compliance. 

## Binary index writer usage
the index writer as a supplied interface and method to write the pack. PackContent data structure does not use blob references or ECS upon initial construction. 
```CSharp
var folderPath = $Path.Combine(Paths.PluginPath,"<ModPack>");
AssetPackContent content = new AssetPackContent();
// Populate content
CustomAssetPlugin.WritePack(folderPath,content);
```
After completion, the WritePack method will convert it into the ECS Blob structure and write the index file into the modpack directory.

## Changelog
- 0.9.0: Alpha release, Comes with binary index writer and TW patch

Shoutout to my Patreons on https://www.patreon.com/HolloFox recognising your
mighty contribution to my caffeine addiction:
- John Fuller
- [Tales Tavern](https://talestavern.com/) - MadWizard
