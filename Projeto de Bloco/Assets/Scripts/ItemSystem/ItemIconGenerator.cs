using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class ItemIconGenerator : MonoBehaviour
{
    [SerializeField] private string _iconName;
    
    [SerializeField] private Camera _camera;

    [ContextMenu("Take Screenshot")]
    private void TakeScreenshot()
    {
        RenderTexture rt = new(256, 256, 24);
        _camera.targetTexture = rt;
        Texture2D screenShot = new(256, 256, TextureFormat.RGBA32, false);
        _camera.Render();
        RenderTexture.active = rt;
        screenShot.ReadPixels(new Rect(0, 0, 256, 256 ), 0 , 0);
        _camera.targetTexture = null;
        RenderTexture.active = null;

        if (Application.isEditor)
        {
            DestroyImmediate(rt);
        }
        else
        {
            Destroy(rt);
        }
        
        byte[] bytes = screenShot.EncodeToPNG();

        string path = System.IO.Path.Combine(System.IO.Directory.GetCurrentDirectory(), $"Assets/Textures/Item Icons/{_iconName}.png");

        System.IO.File.WriteAllBytes(path, bytes);

#if UNITY_EDITOR
        
        AssetDatabase.Refresh();

        var importer = (TextureImporter) AssetImporter.GetAtPath($"Assets/Textures/Item Icons/{_iconName}.png");

        importer.textureType = TextureImporterType.Sprite;
        
        AssetDatabase.ImportAsset($"Assets/Textures/Item Icons/{_iconName}.png", ImportAssetOptions.ForceUpdate);

#endif
    }
}
