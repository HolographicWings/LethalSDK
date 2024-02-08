using UnityEngine;
using UnityEditor;
using System.IO;
using LethalSDK.Utils;

public class AssetModificationProcessor : AssetPostprocessor
{
    private static void OnPostprocessAllAssets(
        string[] importedAssets,
        string[] deletedAssets,
        string[] movedAssets,
        string[] movedFromAssetPaths)
    {
        foreach (string assetPath in importedAssets)
        {
            ProcessAsset(assetPath);
        }

        foreach (string assetPath in movedAssets)
        {
            ProcessAsset(assetPath);
        }
    }
    private static void ProcessAsset(string assetPath)
    {
        if (assetPath.Contains("NavMesh-Environment"))
        {
            AssetDatabase.RenameAsset(assetPath, SelectionLogger.name != string.Empty ? SelectionLogger.name + "NavMesh" : "New NavMesh");
        }
        if (assetPath.Contains("New Terrain"))
        {
            AssetDatabase.RenameAsset(assetPath, SelectionLogger.name != string.Empty ? SelectionLogger.name : "New Terrain");
        }
        if (assetPath.ToLower().StartsWith("assets/mods/") && assetPath.Split('/').Length > 3 && !assetPath.ToLower().EndsWith(".unity") && !assetPath.ToLower().Contains("/scenes"))
        {
            var asset = AssetImporter.GetAtPath(assetPath);
            if (asset != null)
            {
                string bundleName = ExtractBundleNameFromPath(assetPath);

                asset.assetBundleName = bundleName;
                asset.assetBundleVariant = "lem";

                Debug.Log($"{assetPath} asset moved to {bundleName} asset bundle.");
            }
            if(assetPath != "Assets/Mods/" + assetPath.Replace("Assets/Mods/", string.Empty).RemoveNonAlphanumeric(4))
            {
                AssetDatabase.MoveAsset(assetPath, "Assets/Mods/" + assetPath.Replace("Assets/Mods/", string.Empty).RemoveNonAlphanumeric(4));
            }
        }
        else
        {
            var asset = AssetImporter.GetAtPath(assetPath);
            if (asset != null)
            {
                asset.assetBundleName = null;

                Debug.Log($"{assetPath} asset removed from asset bundle.");
            }
        }
    }
    public static string ExtractBundleNameFromPath(string path)
    {
        var pathSegments = path.Split('/');
        if (pathSegments.Length > 3)
        {
            return pathSegments[2].ToLower();
        }
        return "";
    }
}
[InitializeOnLoad]
public class SelectionLogger
{
    static SelectionLogger()
    {
        Selection.selectionChanged += OnSelectionChanged;
    }
    public static string name;

    private static void OnSelectionChanged()
    {
        if (Selection.activeGameObject != null)
        {
            name = GetRootParent(Selection.activeGameObject).name;
        }
        else
        {
            name = string.Empty;
        }
    }
    public static GameObject GetRootParent(GameObject obj)
    {
        if (obj != null && obj.transform.parent != null)
        {
            return GetRootParent(obj.transform.parent.gameObject);
        }
        else
        {
            return obj;
        }
    }
}
[InitializeOnLoad]
public class AssetBundleVariantAssigner
{
    static AssetBundleVariantAssigner()
    {
        AssignVariantToAssetBundles();
    }

    [InitializeOnLoadMethod]
    static void AssignVariantToAssetBundles()
    {
        string[] allAssetBundleNames = AssetDatabase.GetAllAssetBundleNames();

        foreach (string name in allAssetBundleNames)
        {
            if (!name.Contains("."))
            {
                string[] assetPaths = AssetDatabase.GetAssetPathsFromAssetBundle(name);

                foreach (string assetPath in assetPaths)
                {
                    AssetImporter.GetAtPath(assetPath).SetAssetBundleNameAndVariant(name, "lem");
                }

                Debug.Log($"File extention added to AssetBundle: {name}");
            }
        }

        AssetDatabase.SaveAssets();

        string folderPath = "Assets/AssetBundles";

        if (!Directory.Exists(folderPath))
        {
            Debug.LogError("The folder doesn't exist : " + folderPath);
            return;
        }

        string[] files = Directory.GetFiles(folderPath);

        foreach (string file in files)
        {
            if (Path.GetExtension(file) == "" && Path.GetFileName(file) != "AssetBundles")
            {
                string metaFile = file + ".meta";
                string manifestFile = file + ".manifest";
                string manifestMetaFile = manifestFile + ".meta";
                File.Delete(file);
                if (File.Exists(metaFile))
                {
                    File.Delete(metaFile);
                }
                if (File.Exists(manifestFile))
                {
                    File.Delete(manifestFile);
                }
                if (File.Exists(manifestMetaFile))
                {
                    File.Delete(manifestMetaFile);
                }
                Debug.Log("File deleted : " + file);
            }
        }

        AssetDatabase.Refresh();
    }
}
