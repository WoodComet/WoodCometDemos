using System.IO;
using System.Linq;
using UnityEngine;

namespace WoodCometDemos.BetterModSupport
{
    public class ModInfo
    {
        /// <summary>
        /// Full path to this mod's root folder.
        /// </summary>
        public string RootPath { get; private set; }
        
        /// <summary>
        /// Full path to the mod's json file.
        /// </summary>
        public string ModFile { get; private set; }
        
        /// <summary>
        /// Stores information of a mod using a path to the json file.
        /// From here, it browses the parent directories to identify the platform's name and mod's name.
        /// It is mandatory that the mod's strucute looks like the following: `[ModName]/[Platform]/[Mod].json`
        /// Otherwise it may cause issues or misinterpret paths. Be careful with this please.
        ///
        /// To provide a description, place a text file named `description.txt` in the mod's root folder `[ModName]/description.txt`
        /// </summary>
        /// <param name="jsonModFile">Path to the json file</param>
        public ModInfo(string jsonModFile)
        {
            if(!jsonModFile.EndsWith("json"))
                Debug.LogError("You must specify a json file!");

            ModFile = jsonModFile;
            RootPath = Directory.GetParent(ModFile).Parent.FullName;
        }
        
        public string GetModPlatform => Directory.GetParent(ModFile).Name;
        public string GetModName() => Directory.GetParent(ModFile).Parent.Name;

        public string GetModDescription()
        {
            string desc = "No description provided.";
            
            try
            {
                desc = File.ReadLines(Path.Combine(RootPath, "description.txt")).First();
            }
            catch (FileNotFoundException e)
            {
                Debug.LogError("There is no description file for this mod, `description.txt` was not found in " + RootPath);
            }

            return desc;
        }
    }
}