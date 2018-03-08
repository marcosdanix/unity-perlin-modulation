using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Marcosdanix.PerlinModulation
{
    public class WallCreator : MonoBehaviour
    {

        public Vector2Int LowerLeft;
        public Vector2Int UpperRight;
        public Vector2 TexCoordOrigin;
        public Vector2 TexCoordSize;
        public PerlinModulationSettings settings;


        // Use this for initialization
        void Start()
        {
            int resX = settings.resolutionPerUnitX * (UpperRight.x - LowerLeft.x);
            int resY = settings.resolutionPerUnitY * (UpperRight.y - LowerLeft.y);
            MeshAttributes plane = new PlaneGenerator(LowerLeft, UpperRight, TexCoordOrigin, TexCoordSize, resX, resY).Generate();

            Vector2 invResolution = new Vector2(1.0f / resX, 1.0f / resY);


            Vector2 size = UpperRight - LowerLeft;
            Vector2 distPos = Vector2.Scale(size, invResolution);

            Vector2 invTexSize = new Vector2(1.0f / TexCoordSize.x, 1.0f / TexCoordSize.y);
            Vector2 texFullSize = Vector2.Scale(size, invTexSize);
            Vector2 distTex = Vector2.Scale(texFullSize, invResolution);


            PerlinModulator perlinModulator = new PerlinModulator()
            {
                frequency = settings.frequency,
                amplitude = settings.amplitude,
                octaves = settings.octaves,
                world = transform.localToWorldMatrix,
                seed = settings.seed,
                seedX = settings.seedX,
                seedY = settings.seedY,
                uvDistPos = distPos,
                uvDistTex = distTex,
                uvDisplacement = new Vector2(settings.horizontalAmplitudeMultiplier, settings.verticalAmplitudeMultiplier),
                vMin = LowerLeft.y,
                vMax = UpperRight.y
            };

            Mesh mesh = perlinModulator.Modulate(plane);
            GetComponent<MeshFilter>().mesh = mesh;
        }

        // Update is called once per frame
        void Update()
        {

        }
    }
}
