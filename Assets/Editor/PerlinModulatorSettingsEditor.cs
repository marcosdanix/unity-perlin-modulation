using UnityEngine;
using UnityEditor;


class PerlinModulatorSettingsEditor
{
    
    [MenuItem("Assets/Create/Perlin Modulation/Settings")]
    public static void CreateAsset()
    {
        AssetDatabase.CreateAsset(new PerlinModulationSettings(), "Assets/Perlin Modulation Settings.asset");
    }

}
