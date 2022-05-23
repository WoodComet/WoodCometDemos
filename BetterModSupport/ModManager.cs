using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.ResourceLocations;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.AddressableAssets.ResourceLocators;
using System.IO;
using System.Linq;
using System;
using TMPro;

public class ModManager : MonoBehaviour
{
    //static refrence for all ModPrefab objects
    public static ModManager modManager;

    //the directory of the currently selected mod
    public string dir;
    
    //the name of the currently selected mod
    public TextMeshProUGUI DisplayMod;

    //the verticle layout group where all ui prefabs will go
    public GameObject PrefabParent;

    //the ui prefab that selects mods
    public GameObject ModButtonPrefab;

    // Start is called before the first frame update
    void Start()
    {
        modManager = this;

        //go through all subfolders and grab their mods
        GetSubDirectories();
    }

    //change the selected mod
    public void SelectMod(string name, string location)
    {
        dir = location;
        DisplayMod.text = name;
    }

    //spawn in the selected mod
    public void SpawnMod()
    {
        //if were not spawning anything dont spawn anything
        if (dir == "") return;

        //load from the directory were spawning from
        AsyncOperationHandle<IResourceLocator> loadContentCatalogAsync = Addressables.LoadContentCatalogAsync(@"" + dir);

        //call this when were done loading in the content
        loadContentCatalogAsync.Completed += OnCompleted;
    }

    //go through all subfolders and grab their mods
    public void GetSubDirectories()
    {
        //get where the maps are
        string root = Path.Combine(Application.dataPath, "maps");

        //see if there is data in each folder, if so add it to the maps dictonary and spawn it in
        foreach (var folder in Directory.GetDirectories(root).Select(d => Path.GetFileName(d)))
        {
            //try to get description data
            var desc = "";
            try
            {
                desc = File.ReadLines(Path.Combine(root + "/"+ folder, "text.txt")).First();
            }
            catch (Exception ex)
            {
                Debug.LogError(ex);
            }

            //spawn in the UI prefab to select this object
            GameObject NewMod = GameObject.Instantiate(ModButtonPrefab, transform.position, transform.rotation);
            NewMod.transform.parent = PrefabParent.transform;

            //intialize the UI prefab
            NewMod.GetComponent<ModPrefab>().SetupModPrefab(folder, desc, root);
        }

    }

    //what to do when we complete loading the adressable
    private void OnCompleted(AsyncOperationHandle<IResourceLocator> obj)
    {
        //find prefabs in the addressable with the tag specified in the first parameter
        IResourceLocator resourceLocator = obj.Result;
        resourceLocator.Locate("Prefab", typeof(GameObject), out IList<IResourceLocation> locations);

        //if there are loactions in the adressable spawn them
        if (locations != null)
        {
            foreach (IResourceLocation resourceLocation in locations)
            {
                GameObject resourceLocationData = (GameObject)resourceLocation.Data;               
                AsyncOperationHandle<GameObject> prefab = Addressables.InstantiateAsync(resourceLocation);

                //do this when the object is spawned
                prefab.Completed += OnMapInstantiated;
            }
        }
    }

    //what to do after the adressable is spawned in the scene
    private void OnMapInstantiated(AsyncOperationHandle<GameObject> obj)
    {
        Debug.Log("Prefab Spawned");
    }

}
