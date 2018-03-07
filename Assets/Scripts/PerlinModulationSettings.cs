using UnityEngine;


[CreateAssetMenu(fileName = "Perlin Modulation Settings", menuName = "Perlin Modulation/Settings")]
public class PerlinModulationSettings : ScriptableObject {
    public float frequency = 1.0f;
    public float amplitude = 1.0f;
    public int octaves = 4;
    public Vector3 seed = Vector3.zero;
    public Vector3 seedX = Vector3.zero;
    public Vector3 seedY = Vector3.zero;

    public int resolutionPerUnitX = 1;
    public int resolutionPerUnitY = 1;
    public float horizontalAmplitudeMultiplier = 1.0f;
    public float verticalAmplitudeMultiplier = 1.0f;
}
