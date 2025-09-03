using UnityEngine;
using UnityEditor;

public class CursorTexture : Editor
{
    [MenuItem("Tools/Convert To Cursor Texture")]
    private static void ConvertToCursor()
    {
        // Loop through all selected textures in Project
        foreach (Object obj in Selection.objects)
        {
            string path = AssetDatabase.GetAssetPath(obj);
            TextureImporter importer = AssetImporter.GetAtPath(path) as TextureImporter;

            if (importer != null)
            {
                importer.textureType = TextureImporterType.Cursor; // Make it usable for SetCursor
                importer.filterMode = FilterMode.Point;            // Keep pixel look
                importer.mipmapEnabled = false;                    // No mipmaps for cursors
                importer.textureCompression = TextureImporterCompression.Uncompressed;

                AssetDatabase.ImportAsset(path, ImportAssetOptions.ForceUpdate);
                Debug.Log($"Converted {obj.name} to Cursor texture!");
            }
        }
    }
}
