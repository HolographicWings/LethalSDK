using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace LethalSDK.Editor
{
    internal class OldAssetsRemover
    {
        private static readonly List<string> assetPaths = new List<string>
        {
            "Assets/LethalCompanyAssets",
            "Assets/Mods/LethalExpansion/Audio",
            "Assets/Mods/LethalExpansion/AudioMixerController",
            "Assets/Mods/LethalExpansion/Materials/Default.mat",
            "Assets/Mods/LethalExpansion/Prefabs/Settings",
            "Assets/Mods/LethalExpansion/Prefabs/EntranceTeleportA.prefab",
            "Assets/Mods/LethalExpansion/Prefabs/Prefabs.zip",
            "Assets/Mods/LethalExpansion/Scenes/ItemPlaceTest",
            "Assets/Mods/LethalExpansion/Sprites/HandIcon.png",
            "Assets/Mods/LethalExpansion/Sprites/HandIconPoint.png",
            "Assets/Mods/LethalExpansion/Sprites/HandLadderIcon.png",
            "Assets/Mods/TemplateMod/Moons/NavMesh-Environment.asset",
            "Assets/Mods/TemplateMod/Moons/OldSeaPort.asset",
            "Assets/Mods/TemplateMod/Moons/Sky and Fog Global Volume Profile.asset",
            "Assets/Mods/TemplateMod/Moons/Sky and Fog Global Volume Profile 1.asset",
            "Assets/Mods/TemplateMod/AssetBank.asset",
            "Assets/Mods/LethalExpansion/Animations/Sun/TimeOfDaySunCompanyLevel.anim",
            "Assets/Mods/LethalExpansion/Animations/Sun/TimeOfDaySunTypeB.anim",
            "Assets/Mods/LethalExpansion/Animations/Sun/TimeOfDaySunTypeBEclipse.anim",
            "Assets/Mods/LethalExpansion/Animations/Sun/TimeOfDaySunTypeBStormy.anim",
            "Assets/Mods/LethalExpansion/Animations/Sun/TimeOfDaySunTypeC.anim",
            "Assets/Mods/LethalExpansion/Animations/Sun/TimeOfDaySunTypeCEclipse.anim",
            "Assets/Mods/LethalExpansion/Animations/Sun/TimeOfDaySunTypeCStormy.anim",
            "Assets/Mods/LethalExpansion/Skybox",
            "Assets/Mods/LethalExpansion/Sprites/XButton.png",
            "Assets/Mods/LethalExpansion/Textures/sunTexture1.png",
            "Assets/Mods/OldSeaPort/Materials/Maple_bark_1.mat",
            "Assets/Mods/OldSeaPort/Materials/maple_leaves.mat",
            "Assets/Mods/TemplateMod/AssetBank.asset",
            "Assets/Mods/OldSeaPort/EffectExamples/Shared/Scripts",
            "Assets/Mods/OldSeaPort/scenes",
            "Assets/Mods/OldSeaPort/prefabs/Plane (12).prefab",
            "Assets/Mods/LethalExpansion/Meshes/labyrinth.fbx",
            "Assets/Mods/ChristmasVillage/christmas-assets-free/fbx/Materials",
            "Assets/Mods/OldSeaPort/effectexamples/profiles",
            "Assets/Mods/OldSeaPort/effectexamples/shared/animation",
            "Assets/Mods/OldSeaPort/effectexamples/shared/fonts",
            "Assets/Mods/OldSeaPort/effectexamples/shared/shaders",
            "Assets/Mods/OldSeaPort/effectexamples/shared/sprites"
        };

        [InitializeOnLoadMethod]
        public static void CheckOldAssets()
        {
            foreach (var path in assetPaths)
            {
                if (AssetDatabase.IsValidFolder(path))
                {
                    DeleteFolder(path);
                }
                else if (AssetDatabase.LoadAssetAtPath<GameObject>(path) != null)
                {
                    DeleteAsset(path);
                }
            }
        }
        private static void DeleteFolder(string path)
        {
            if (AssetDatabase.DeleteAsset(path))
            {
                Debug.Log("Deleted folder at: " + path);
            }
            else
            {
                Debug.LogError("Failed to delete folder at: " + path);
            }
        }

        private static void DeleteAsset(string path)
        {
            if (AssetDatabase.DeleteAsset(path))
            {
                Debug.Log("Deleted asset at: " + path);
            }
            else
            {
                Debug.LogError("Failed to delete asset at: " + path);
            }
        }
    }
}
