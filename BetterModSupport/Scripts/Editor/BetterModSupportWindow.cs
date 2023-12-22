#if UNITY_EDITOR
using System;
using System.IO;
using UnityEditor;
using UnityEngine;
using UnityEngine.Serialization;

namespace WoodCometDemos.BetterModSupport
{
    
    public class BetterModSupportWindow : EditorWindow
    {
        static int _thumbnailWidth = 1024;
        static int _thumbnailHeight = 1024;
        
        static string SavePath()
        {
            ModManager manager = ModManager.Instance;
            if (!ModManager.Instance) manager = FindAnyObjectByType<ModManager>();

            if (!manager)
                throw new Exception("Unable to find the mods path, please add a ModManager component somewhere");
            
            return Path.Combine(manager.ModsRootDirectory, "thumbnail.png");
        }
        
        [MenuItem("BetterModSupport/Configure")]
        static void ShowWindow()
        {
            GetWindow(typeof(BetterModSupportWindow));
        }
        
        static void CaptureThumbnail()
        {
            Camera _camera = Camera.main;
            
            RenderTexture activeRenderTexture = RenderTexture.active;
            RenderTexture.active = _camera.targetTexture;
 
            _camera.Render();
 
            _camera.targetTexture = new RenderTexture(_thumbnailWidth, _thumbnailHeight, 24);

            Texture2D image = new Texture2D(_camera.targetTexture.width, _camera.targetTexture.height);
            image.ReadPixels(new Rect(0, 0, _camera.targetTexture.width, _camera.targetTexture.height), 0, 0);
            image.Apply();
            RenderTexture.active = activeRenderTexture;
 
            byte[] bytes = image.EncodeToPNG();
            DestroyImmediate(image);
 
            File.WriteAllBytes(SavePath(), bytes);
            
            EditorUtility.RevealInFinder(SavePath());
        }

        private void OnGUI()
        {
            titleContent.text = "BetterModSupport";
            
            ThumbnailSection();
        }

        private void ThumbnailSection()
        {
            EditorGUILayout.LabelField("Thumbnail creation", EditorStyles.boldLabel);
            
            GUILayout.Label("Image size (in pixels):");
            EditorGUILayout.BeginHorizontal();
            {
                GUILayout.Label("Width:");
                _thumbnailWidth = EditorGUILayout.IntField(_thumbnailWidth);
                GUILayout.Label("Height: ");
                _thumbnailHeight = EditorGUILayout.IntField(_thumbnailHeight);
                
            }
            EditorGUILayout.EndHorizontal();
            
            if(GUILayout.Button("Capture Thumbnail", GUILayout.Height(50)))
                CaptureThumbnail();
        }
    }
}
#endif