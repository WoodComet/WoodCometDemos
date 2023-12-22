using System.IO;
using UnityEngine;
using TMPro;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace WoodCometDemos.BetterModSupport
{
    public class ModPrefab : MonoBehaviour
    {
        [FormerlySerializedAs("Name")] [Tooltip("UI Text item that will display the mod's name.")] [SerializeField]
        private TextMeshProUGUI modNameTextField;

        [FormerlySerializedAs("Description")]
        [Tooltip("UI Text item that will display the mod's description.")]
        [SerializeField]
        private TextMeshProUGUI modNameDescriptionField;

        [Tooltip("Raw Image component that will display the mod's thumbnail.")] 
        [SerializeField] private RawImage modThumbnail;

    private ModInfo _selectedModInformation;

        /// <summary>
        /// Configures the prefab to load the provided mod, then display the mod's name and description.
        /// </summary>
        /// <param name="modInfo">The mod to load</param>
        public void SetupModPrefab(ModInfo modInfo)
        {
            _selectedModInformation = modInfo;
            modNameTextField.text = modInfo.GetModName();
            modNameDescriptionField.text = modInfo.GetModDescription();
            if(modInfo.HasThumbnail()) modThumbnail.texture = modInfo.GetModThumbnainTexture();
        }

        /// <summary>
        /// Tells the mod manager to select this mod.
        /// </summary>
        public void SelectThisMod()
        {
            ModManager.SelectMod(_selectedModInformation);
        }
    }
}