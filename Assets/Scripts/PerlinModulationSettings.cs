using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public class PerlinModulationSettings : ScriptableObject {
    public float Frequency = 1.0f;
    public float Amplitude = 1.0f;
    public int Octaves = 4;
    public Vector3 Seed = Vector3.zero;
    public Vector3 SeedX = Vector3.zero;
    public Vector3 SeedY = Vector3.zero;

    public int ResolutionPerUnitX = 1;
    public int ResolutionPerUnitY = 1;
    public float horizontalAmplitudeMultiplier = 1.0f;
    public float verticalAmplitudeMultiplier = 1.0f;
}
