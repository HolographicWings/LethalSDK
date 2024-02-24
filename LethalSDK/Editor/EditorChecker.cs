using LethalSDK.Component;
using LethalSDK.ScriptableObjects;
using LethalSDK.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unity.Netcode;
using UnityEditor;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

namespace LethalSDK.Editor
{
    public class EditorChecker : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();
        }
    }
    [CustomEditor(typeof(ModManifest))]
    public class ModManifestEditor : EditorChecker
    {
        public override void OnInspectorGUI()
        {
            ModManifest t = (ModManifest)target;

            if (t.serializedVersion == "0.0.0.0")
            {
                EditorGUILayout.HelpBox("Please define a version to your mod and don't forget to increment it at each update.", MessageType.Warning);
            }
            if (t.modName == null || t.modName.Length == 0)
            {
                EditorGUILayout.HelpBox("Your mod need a name.", MessageType.Error);
            }

            var duplicateScraps = t.scraps.Where(e => e != null).ToList().GroupBy(e => e.itemName).Where(g => g.Count() > 1).Select(g => g.Key);

            if (duplicateScraps.Any())
            {
                string list = string.Empty;
                foreach (var duplicateId in duplicateScraps)
                {
                    list += $"{duplicateId},";
                }
                list = list.Remove(list.Length - 1);
                EditorGUILayout.HelpBox($"You are trying to register two times or more the same Scraps. Duplicated Scraps are: {list}", MessageType.Warning);
            }
            var duplicateMoons = t.moons.Where(e => e != null).ToList().GroupBy(e => e.MoonName).Where(g => g.Count() > 1).Select(g => g.Key);

            if (duplicateMoons.Any())
            {
                string list = string.Empty;
                foreach (var duplicateId in duplicateMoons)
                {
                    list += $"{duplicateId},";
                }
                list = list.Remove(list.Length - 1);
                EditorGUILayout.HelpBox($"You are trying to register two times or more the same Moons. Duplicated Moons are: {list}", MessageType.Warning);
            }

            string outsideAssets = string.Empty;
            foreach (var s in t.scraps)
            {
                if (s != null && AssetModificationProcessor.ExtractBundleNameFromPath(AssetDatabase.GetAssetPath(s)) != AssetModificationProcessor.ExtractBundleNameFromPath(AssetDatabase.GetAssetPath(t)))
                {
                    outsideAssets += $"{s.name},";
                }
            }
            foreach (var m in t.moons)
            {
                if (m != null && AssetModificationProcessor.ExtractBundleNameFromPath(AssetDatabase.GetAssetPath(m)) != AssetModificationProcessor.ExtractBundleNameFromPath(AssetDatabase.GetAssetPath(t)))
                {
                    outsideAssets += $"{m.name},";
                }
            }
            if (outsideAssets != null && outsideAssets.Length > 0)
            {
                outsideAssets = outsideAssets.Remove(outsideAssets.Length - 1);
                EditorGUILayout.HelpBox($"You try to register a Scrap or a Moon from another mod folder. {outsideAssets}", MessageType.Warning);
            }
            if (t.assetBank != null && AssetModificationProcessor.ExtractBundleNameFromPath(AssetDatabase.GetAssetPath(t.assetBank)) != AssetModificationProcessor.ExtractBundleNameFromPath(AssetDatabase.GetAssetPath(t)))
            {
                EditorGUILayout.HelpBox($"You try to register an AssetBank from another mod folder.", MessageType.Warning);
            }

            base.OnInspectorGUI();
        }
    }
    [CustomEditor(typeof(AssetBank))]
    public class AssetBankEditor : EditorChecker
    {

        public override void OnInspectorGUI()
        {
            AssetBank t = (AssetBank)target;

            var duplicateAudioClips = t.AudioClips().Where(e => e.AudioClipName != null && e.AudioClipName.Length > 0).ToList().GroupBy(e => e.AudioClipName).Where(g => g.Count() > 1).Select(g => g.Key);

            if (duplicateAudioClips.Any())
            {
                string list = string.Empty;
                foreach (var duplicateId in duplicateAudioClips)
                {
                    list += $"{duplicateId},";
                }
                list = list.Remove(list.Length - 1);
                EditorGUILayout.HelpBox($"You are trying to register two times or more the same Audio Clip. Duplicated Clips are: {list}", MessageType.Warning);
            }
            var duplicatePlanetPrefabs = t.PlanetPrefabs().Where(e => e.PlanetPrefabName != null && e.PlanetPrefabName.Length > 0).ToList().GroupBy(e => e.PlanetPrefabName).Where(g => g.Count() > 1).Select(g => g.Key);

            if (duplicatePlanetPrefabs.Any())
            {
                string list = string.Empty;
                foreach (var duplicateId in duplicatePlanetPrefabs)
                {
                    list += $"{duplicateId},";
                }
                list = list.Remove(list.Length - 1);
                EditorGUILayout.HelpBox($"You are trying to register two times or more the same Planet Prefabs. Duplicated Planet Prefabs are: {list}", MessageType.Warning);
            }

            string outsideAssets = string.Empty;
            foreach (var a in t.AudioClips())
            {
                if (a.AudioClipName != null && a.AudioClipName.Length > 0 && AssetModificationProcessor.ExtractBundleNameFromPath(a.AudioClipPath) != AssetModificationProcessor.ExtractBundleNameFromPath(AssetDatabase.GetAssetPath(t)))
                {
                    outsideAssets += $"{a.AudioClipName},";
                }
            }
            foreach (var p in t.PlanetPrefabs())
            {
                if (p.PlanetPrefabName != null && p.PlanetPrefabName.Length > 0 && AssetModificationProcessor.ExtractBundleNameFromPath(p.PlanetPrefabPath) != AssetModificationProcessor.ExtractBundleNameFromPath(AssetDatabase.GetAssetPath(t)))
                {
                    outsideAssets += $"{p.PlanetPrefabName},";
                }
            }
            if (outsideAssets != null && outsideAssets.Length > 0)
            {
                outsideAssets = outsideAssets.Remove(outsideAssets.Length - 1);
                EditorGUILayout.HelpBox($"You try to register an Audio Clip or a Planet Prefab from another mod folder. {outsideAssets}", MessageType.Warning);
            }

            base.OnInspectorGUI();
        }
    }
    [CustomEditor(typeof(SI_DungeonGenerator))]
    public class SI_DungeonGeneratorEditor : EditorChecker
    {
        public override void OnInspectorGUI()
        {
            SI_DungeonGenerator t = (SI_DungeonGenerator)target;

            String path = AssetDatabase.GetAssetPath(t.DungeonRoot);
            if (path != null && path.Length > 0)
            {
                EditorGUILayout.HelpBox("Dungeon Root must be in the scene.", MessageType.Error);
            }

            base.OnInspectorGUI();
        }
    }
    [CustomEditor(typeof(SI_ScanNode))]
    public class SI_ScanNodeEditor : EditorChecker
    {
        public override void OnInspectorGUI()
        {
            SI_ScanNode t = (SI_ScanNode)target;

            if(t.MinRange > t.MaxRange)
            {
                EditorGUILayout.HelpBox("Min Range must be smaller than Max Ranger.", MessageType.Error);
            }
            if(t.CreatureScanID < -1)
            {
                EditorGUILayout.HelpBox("Creature Scan ID can't be less than -1.", MessageType.Warning);
            }

            base.OnInspectorGUI();
        }
    }
    [CustomEditor(typeof(SI_AnimatedSun))]
    public class SI_AnimatedSunEditor : EditorChecker
    {
        public override void OnInspectorGUI()
        {
            SI_AnimatedSun t = (SI_AnimatedSun)target;

            if(t.directLight == null || t.indirectLight == null)
            {
                EditorGUILayout.HelpBox("A direct and an indirect light must be defined.", MessageType.Warning);
            }
            if(t.directLight.transform.parent != t.transform || t.indirectLight.transform.parent != t.transform)
            {
                EditorGUILayout.HelpBox("Direct and an indirect light must be a child of the AnimatedSun in the hierarchy.", MessageType.Warning);
            }

            base.OnInspectorGUI();
        }
    }
    [CustomEditor(typeof(SI_EntranceTeleport))]
    public class SI_EntranceTeleportEditor : EditorChecker
    {
        public override void OnInspectorGUI()
        {
            SI_EntranceTeleport t = (SI_EntranceTeleport)target;

            var duplicates = GameObject.FindObjectsOfType<SI_EntranceTeleport>().ToList().GroupBy(e => e.EntranceID).Where(g => g.Count() > 1).Select(g => g.Key);

            if (duplicates.Any())
            {
                string list = string.Empty;
                foreach (var duplicateId in duplicates)
                {
                    list += $"{duplicateId},";
                }
                list = list.Remove(list.Length - 1);
                EditorGUILayout.HelpBox($"Two entrances or more have same Entrance ID. Duplicated entrances are: {list}", MessageType.Warning);
            }
            if (t.EntrancePoint == null)
            {
                EditorGUILayout.HelpBox("An entrance point must be defined.", MessageType.Error);
            }
            if(t.AudioReverbPreset < 0)
            {
                EditorGUILayout.HelpBox("Audio Reverb Preset can't be negative.", MessageType.Error);
            }

            base.OnInspectorGUI();
        }
    }
    [CustomEditor(typeof(Scrap))]
    public class ScrapEditor : EditorChecker
    {
        public override void OnInspectorGUI()
        {
            Scrap t = (Scrap)target;
            
            if(t.prefab == null)
            {
                EditorGUILayout.HelpBox("You must add a Prefab to your Scrap.", MessageType.Info);
            }
            else
            {
                if(t.prefab.GetComponent<NetworkObject>() == null)
                {
                    EditorGUILayout.HelpBox("The Prefab must have a NetworkObject.", MessageType.Error);
                }
                else
                {
                    NetworkObject no = t.prefab.GetComponent<NetworkObject>();
                    string errorMessage = string.Empty;
                    if (no.AlwaysReplicateAsRoot != false)
                        errorMessage += "\n- AlwaysReplicateAsRoot should be false.";
                    if (no.SynchronizeTransform != true)
                        errorMessage += "\n- SynchronizeTransform should be true.";
                    if (no.ActiveSceneSynchronization != false)
                        errorMessage += "\n- ActiveSceneSynchronization should be false.";
                    if (no.SceneMigrationSynchronization != true)
                        errorMessage += "\n- SceneMigrationSynchronization should be true.";
                    if (no.SpawnWithObservers != true)
                        errorMessage += "\n- SpawnWithObservers should be true.";
                    if (no.DontDestroyWithOwner != true)
                        errorMessage += "\n- DontDestroyWithOwner should be true.";
                    if (no.AutoObjectParentSync != false)
                        errorMessage += "\n- AutoObjectParentSync should be false.";

                    if (errorMessage.Length > 0)
                    {
                        EditorGUILayout.HelpBox($"The NetworkObject of the Prefab have incorrect settings: {errorMessage}", MessageType.Warning);
                    }
                }
                if (t.meshVariants != null && t.meshVariants.Length > 0 && t.prefab.GetComponent<MeshFilter>() == null)
                {
                    EditorGUILayout.HelpBox("A scrap with Mesh Variants need a MeshFilter in it's parent object.", MessageType.Error);
                }
                if (t.prefab.transform.Find("ScanNode") == null)
                {
                    EditorGUILayout.HelpBox("The Prefab don't have a ScanNode.", MessageType.Warning);
                }
                if (AssetModificationProcessor.ExtractBundleNameFromPath(AssetDatabase.GetAssetPath(t.prefab)) != AssetModificationProcessor.ExtractBundleNameFromPath(AssetDatabase.GetAssetPath(t)))
                {
                    EditorGUILayout.HelpBox("The Prefab must come from the same mod folder as your Scrap.", MessageType.Warning);
                }
            }
            if (t.itemName == null || t.itemName.Length == 0)
            {
                EditorGUILayout.HelpBox("Your scrap must have a Name.", MessageType.Error);
            }
            if (!t.useGlobalSpawnWeight && !t.perPlanetSpawnWeight().Any(w => w.SceneName != null && w.SceneName.Length > 0))
            {
                EditorGUILayout.HelpBox("Your scrap use Per Planet Spawn Weight but no planet are defined.", MessageType.Warning);
            }

            base.OnInspectorGUI();
        }
    }
    [CustomEditor(typeof(Moon))]
    public class MoonEditor : EditorChecker
    {
        public override void OnInspectorGUI()
        {
            Moon t = (Moon)target;
            
            if (t.MoonName == null || t.MoonName.Length == 0)
            {
                EditorGUILayout.HelpBox("Your moon must have a Name.", MessageType.Error);
            }
            if (t.PlanetName == null || t.PlanetName.Length == 0)
            {
                EditorGUILayout.HelpBox("Your moon must have a Planet Name.", MessageType.Error);
            }
            if (t.RouteWord == null || t.RouteWord.Length < 3)
            {
                EditorGUILayout.HelpBox("Your moon route word must be at least 3 characters long.", MessageType.Error);
            }
            if (t.MainPrefab == null)
            {
                EditorGUILayout.HelpBox("You must add a Main Prefab to your Scrap.", MessageType.Info);
            }
            else
            {
                if (AssetModificationProcessor.ExtractBundleNameFromPath(AssetDatabase.GetAssetPath(t.MainPrefab)) != AssetModificationProcessor.ExtractBundleNameFromPath(AssetDatabase.GetAssetPath(t)))
                {
                    EditorGUILayout.HelpBox("The Main Prefab must come from the same mod folder as your Moon.", MessageType.Warning);
                }
            }

            base.OnInspectorGUI();
        }
    }
    [CustomEditor(typeof(SI_DoorLock))]
    public class SI_DoorLockEditor : EditorChecker
    {
        public override void OnInspectorGUI()
        {
            SI_DoorLock t = (SI_DoorLock)target;

            EditorGUILayout.HelpBox("DoorLock is not implemented yet.", MessageType.Info);

            base.OnInspectorGUI();
        }
    }
    [CustomEditor(typeof(SI_Ladder))]
    public class SI_LadderEditor : EditorChecker
    {
        public override void OnInspectorGUI()
        {
            SI_Ladder t = (SI_Ladder)target;

            EditorGUILayout.HelpBox("Ladder is experimental.", MessageType.Info);

            base.OnInspectorGUI();
        }
    }
    [InitializeOnLoad]
    [CustomEditor(typeof(TerrainChecker))]
    public class TerrainCheckerEditor : EditorChecker
    {
        static TerrainCheckerEditor()
        {
            Selection.selectionChanged += OnSelectionChanged;
        }

        static void OnSelectionChanged()
        {
            if (Selection.activeGameObject && Selection.activeGameObject.GetComponent<Terrain>())
            {
                Terrain terrain = Selection.activeGameObject.GetComponent<Terrain>();
                if (!terrain.gameObject.GetComponent<TerrainChecker>())
                {
                    terrain.gameObject.AddComponent<TerrainChecker>();
                }
            }
        }
        public override void OnInspectorGUI()
        {
            TerrainChecker t = (TerrainChecker)target;

            if (t.terrain != null)
            {
                int decalLayerMask = 1 << 10;
                if ((t.terrain.renderingLayerMask & decalLayerMask) == 0)
                {
                    EditorGUILayout.HelpBox("Quicksand will not show on this terrain, to fix you must enable the '10: Decal Layer 2' in Terrain Settings > Rendering Layer Mask.", MessageType.Warning);
                }
                if (t.terrain.materialTemplate != null && t.terrain.materialTemplate.shader.name != "HDRP/UpdatedTerrainLit")
                {
                    EditorGUILayout.HelpBox("To use Terrain Holes, you must change the material of the terrain with another one that use the HDRP/UpdatedTerrainLit shader in Terrain Settings > Basic Terrain > Material.", MessageType.Info);
                }
                if (t.terrain.drawInstanced == false)
                {
                    EditorGUILayout.HelpBox("For performances with a lot of trees, grass and other details, it's recommended to enable Terrain Settings > Basic Terrain > Draw Instanced.", MessageType.Info);
                }
            }
        }
    }
}
