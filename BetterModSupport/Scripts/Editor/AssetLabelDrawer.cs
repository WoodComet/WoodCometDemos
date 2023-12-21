using System;
using UnityEditor;
using UnityEditor.AddressableAssets;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace WoodCometDemos.BetterModSupport
{
    [CustomPropertyDrawer(typeof(AssetLabelReference))]
    public class AssetLabelDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            EditorGUI.BeginProperty(position, label, property);
            {
                // Get the current label (Property found in the class AssetLabelReference)
                var currentLabel = property.FindPropertyRelative("m_LabelString");
                var labelList = GetAvailableLabels(); 

                // Find the index of the current label
                var currIndex = Array.IndexOf(labelList, currentLabel.stringValue);

                // Display a dropdown for selecting labels
                var newIndex = EditorGUI.Popup(position, currIndex, labelList);
                if (newIndex != currIndex)
                {
                    currentLabel.stringValue = labelList[newIndex];
                }
            }
            EditorGUI.EndProperty();
        }
        
        // Helper method to get the available labels from Addressables settings
        private string[] GetAvailableLabels()
        {
            return AddressableAssetSettingsDefaultObject.Settings.GetLabels().ToArray();
        }
    }
}