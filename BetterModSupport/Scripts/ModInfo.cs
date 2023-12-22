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
        public string GetModThumbnailPath() => Path.Combine(RootPath, "thumbnail.png");
        public Texture2D GetModThumbnainTexture()
        {
            if (!HasThumbnail())
                return null;
                
            byte[] bytes = File.ReadAllBytes(GetModThumbnailPath());
            Texture2D texture = new Texture2D(1, 1);
            texture.LoadImage(bytes);
            return texture;
        }
        
        public bool HasThumbnail() => File.Exists(GetModThumbnailPath());

        public string GetModDescription()
        {
            string descriptionFilePath = Path.Combine(RootPath, "description.txt");
            
            if(File.Exists(descriptionFilePath))
                return File.ReadLines(Path.Combine(RootPath, "description.txt")).First();
            
            return "No description provided.";
        }

        /// <summary>
        /// Replaces all insatnces of MOD_NAME found in the catalog file by the actual mod name so it finds the bundle files accordingly.
        /// </summary>
        public void FixBundlePathsInModFile()
        {
            // Read content
            StreamReader reader = new StreamReader(ModFile);
            string contentCatalog = reader.ReadToEnd();
            reader.Close();

            // Replace and cache data
            contentCatalog = contentCatalog.Replace("MOD_NAME", GetModName());
            
            // Write back to the mod file
            StreamWriter writer = new StreamWriter(ModFile);
            writer.Write(contentCatalog);
            writer.Close();
        }
    }
}