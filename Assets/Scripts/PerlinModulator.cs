﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Marcosdanix.PerlinModulation
{
    class PerlinModulator : IModulator
    {
        public float frequency { get; set; }
        public float amplitude { get; set; }
        public int octaves { get; set; }
        public Matrix4x4 world { get; set; }
        public Vector3 seed { get; set; }
        public Vector3 seedX { get; set; }
        public Vector3 seedY { get; set; }

        public Vector2 uvDistPos { get; set; } //The distance between vertices in the U (left-right) and V (up-down) direction
        public Vector2 uvDistTex { get; set; } //The distance between vertices in the U (left-right) and V (up-down) direction
        public Vector2 uvDisplacement { get; set; } //Displacement multiplier in the UV plane
        public float vMin; //smallest v position
        public float vMax; //largest v position

        private Vector3 uvwAmplitudePos;
        private Vector2 uvAmplitudeTex;


        public Mesh Modulate(MeshAttributes mesh)
        {
            Mesh result = new Mesh();
            int length = mesh.vertices.Length;

            uvwAmplitudePos = new Vector3(
                uvDisplacement.x * uvDistPos.x,
                uvDisplacement.y * uvDistPos.y,
                amplitude
            );

            uvAmplitudeTex = Vector2.Scale(uvDistTex, uvDisplacement);

            for (int i = 0; i < length; ++i)
            {
                Vector3 normal = mesh.normals[mesh.normalIndex[i]];
                //AGAIN, TANGENT IS UP BECAUSE THIS ARE LEFT HANDED COORDINATES!!
                Vector3 tangent = mesh.tangent[mesh.tangentIndex[i]];
                Vector3 bitangent = Vector3.Cross(normal, tangent);

                Vector3 noise = CalculateNoise(mesh.vertices[i]);

                float t = (mesh.vertices[i].y - vMin) / (vMax - vMin);
                float vFactor = Mathf.Sin(Mathf.PI * t);
                mesh.vertices[i] = ModulateVertex(mesh.vertices[i], noise, normal, bitangent, tangent, vFactor);
                mesh.uv[i] = ModulateUv(mesh.uv[i], noise, vFactor);
            }

            result.vertices = mesh.vertices;
            result.uv = mesh.uv;
            result.triangles = mesh.triangles;

            result.RecalculateNormals();
            result.RecalculateTangents();
            result.RecalculateBounds();

            return result;
        }

        private Vector3 CalculateNoise(Vector3 position)
        {
            Vector3 noisePosition = (frequency * world.MultiplyPoint(position)) + seed;
            return new Vector3(
                    Perlin.Fbm(noisePosition + seedX, octaves),
                    Perlin.Fbm(noisePosition + seedY, octaves),
                    Perlin.Fbm(noisePosition, octaves)
            );
        }

        private Vector3 ModulateVertex(
            Vector3 position, Vector3 noise, Vector3 normal, Vector3 bitangent, Vector3 tangent, float vFactor)
        {
            Vector3 modulation = Vector3.Scale(noise, uvwAmplitudePos);
            modulation[1] *= vFactor;
            return position + modulation.x * bitangent + modulation.y * tangent + modulation.z * normal;
        }

        private Vector2 ModulateUv(Vector2 texcoord, Vector3 noise, float vFactor)
        {
            Vector2 modulation = Vector2.Scale(noise, uvAmplitudeTex);
            modulation[1] *= vFactor;
            return texcoord + modulation;
        }
    }
}