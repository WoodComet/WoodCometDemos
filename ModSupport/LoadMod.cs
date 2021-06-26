using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement;
using UnityEngine.ResourceManagement.ResourceLocations;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.AddressableAssets.ResourceLocators;
using UnityEngine.AddressableAssets;
using UnityEngine.UI;

public class LoadMod : MonoBehaviour
{
    public string modDir;
    // Start is called before the first frame update
    public void openMod()
    {
        string dir = modDir;
        AsyncOperationHandle<IResourceLocator> loadContentCatalogAsync = Addressables.LoadContentCatalogAsync(@"" + dir);

        loadContentCatalogAsync.Completed += OnCompleted;
    }

    private void OnCompleted(AsyncOperationHandle<IResourceLocator> obj)
    {

        IResourceLocator resourceLocator = obj.Result;
        resourceLocator.Locate("Scene", typeof(GameObject), out IList<IResourceLocation> locations);
        if (locations != null)
        {
            foreach (IResourceLocation resourceLocation in locations)
            {
                GameObject resourceLocationData = (GameObject)resourceLocation.Data;
                Addressables.InstantiateAsync(resourceLocation);
            }
            
        }



    }
}