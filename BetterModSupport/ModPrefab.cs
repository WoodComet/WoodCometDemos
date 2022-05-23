using UnityEngine;
using TMPro;

public class ModPrefab : MonoBehaviour
{
    public TextMeshProUGUI Name;
    public TextMeshProUGUI Description;
    string Location;

    //basic setup method
    public void SetupModPrefab(string name, string desc, string loacation)
    {
        Name.text = name;
        Description.text = desc;
        Location = loacation;
    }
    //called to set the mod that will be loaded next
    public void SetMe()
    {
        ModManager.modManager.SelectMod(Name.text, Location + "/" + Name.text + "/mod.json");
    }
}
