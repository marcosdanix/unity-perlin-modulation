using System;
using System.Linq;
using UnityEngine;

//It makes sense to inherit from PlaneGenerator. 
//This should be done if there are more primitives


//Generates a quarter of a cylinder with center in the origin
//The cylinder is built rotating from +Z to +X, the cylinder is oriented in the Y direction
//The normal can be pointed towards the center or not
//Horizontal resolution is increased by a factor of pi/2 and rounded
//About UVs, they are still horizontally scaled to 1.0, not pi/2. Use a different texture.
namespace Marcosdanix.PerlinModulation
{
    class QuarterCylinderGenerator : IMeshGenerator
    {
        static float PI_2 = 0.5f * Mathf.PI;
        float radius, bottom;
        int horizontal, vertical;
        float normalMultiplier;

        float height;
        float length;
        float addTh;
        float addY;
        Vector2 addUv;

        public QuarterCylinderGenerator(float radius, float bottom, float top, int horizontal, int vertical, bool normalToOrigin = true)
        {
            this.radius = radius;
            this.bottom = bottom;
            this.horizontal = Mathf.RoundToInt(0.5f * Mathf.PI * horizontal);
            this.vertical = vertical;
            this.normalMultiplier = normalToOrigin ? -1.0f : 1.0f;

            this.height = top - bottom;
            //this.length = 0.5f * Mathf.PI * radius;
            //this.add = new Vector3(difference.x / horizontal, height / vertical, 0.0f);
            this.addTh = PI_2 / horizontal;
            this.addY = height / vertical;
            this.addUv = new Vector2(1.0f / horizontal, 1.0f / vertical);
        }


        public MeshAttributes Generate()
        {
            int numVerts = horizontal * vertical * 6;

            MeshAttributes mesh = new MeshAttributes
            {
                vertices = new Vector3[numVerts],
                uv = new Vector2[numVerts],
                triangles = Enumerable.Range(0, numVerts).ToArray<int>(),
                normals = new Vector3[horizontal + 1],
                normalIndex = new int[numVerts],
                tangent = new Vector3[] { Vector3.up },
                tangentIndex = Enumerable.Repeat<int>(0, numVerts).ToArray<int>()
            };

            //Generate Normals
            GenerateNormals(mesh);

            for (int y = 0; y < vertical; ++y)
            {
                for (int x = 0; x < horizontal; ++x)
                {
                    int index = (y * horizontal + x) * 6;
                    GenerateQuad(mesh, x, y, index);
                    GenerateUv(mesh, x, y, index);
                    GenerateNormalIndex(mesh, x, index);
                }
            }

            return mesh;
        }

        void GenerateNormals(MeshAttributes mesh)
        {
            for (int i = 0; i <= horizontal; ++i)
            {
                float t = ((float)i / (float)horizontal);
                mesh.normals[i] = normalMultiplier * new Vector3(Mathf.Sin(PI_2 * t), 0.0f, Mathf.Cos(PI_2 * t));
            }
        }

        private void GenerateQuad(MeshAttributes mesh, int x, int y, int index)
        {
            //Angle of the leftmost vertex
            float angle = x * addTh;

            //I could've used a vector3 and added to another y vector3
            //These two values are more easily memoizable than the positions.
            Vector2 positionXZ = GetXzPosition(angle);
            Vector2 nextPositionXZ = GetXzPosition(angle + addTh);
            float positionY = bottom + y * addY;
            float nextPositionY = bottom + (y + 1) * addY;

            Vector3 position = new Vector3(positionXZ.x, positionY, positionXZ.y);
            Vector3 nextX = new Vector3(nextPositionXZ.x, positionY, nextPositionXZ.y);
            Vector3 nextY = new Vector3(positionXZ.x, nextPositionY, positionXZ.y);
            Vector3 nextXY = new Vector3(nextPositionXZ.x, nextPositionY, nextPositionXZ.y);

            //Bottom triangle
            mesh.vertices[index] = position;
            mesh.vertices[index + 1] = nextY;
            mesh.vertices[index + 2] = nextX;

            //Top triangle
            mesh.vertices[index + 3] = nextXY;
            mesh.vertices[index + 4] = nextX;
            mesh.vertices[index + 5] = nextY;

        }

        private Vector2 GetXzPosition(float angle)
        {
            return radius * (new Vector2(Mathf.Sin(angle), Mathf.Cos(angle)));
        }

        //copied from PlaneGenerator
        private void GenerateUv(MeshAttributes mesh, int x, int y, int index)
        {
            Vector2 position = new Vector2(addUv.x * x, addUv.y * y);
            Vector2 nextU = position + addUv.x * Vector2.right;
            Vector2 nextV = position + addUv.y * Vector2.up;
            Vector2 nextUV = position + addUv;

            //Bottom triangle
            mesh.uv[index] = position;
            mesh.uv[index + 1] = nextV;
            mesh.uv[index + 2] = nextU;

            //Top triangle
            mesh.uv[index + 3] = nextUV;
            mesh.uv[index + 4] = nextU;
            mesh.uv[index + 5] = nextV;
        }

        private void GenerateNormalIndex(MeshAttributes mesh, int x, int index)
        {
            //Vertices 0,1 and 5 are to the left, and the rest are to the right
            for (int i = 0; i < 6; ++i)
            {
                //More to the right
                if (i >= 2 && i <= 4) mesh.normalIndex[index + i] = x + 1;
                //More to the left
                else mesh.normalIndex[index + i] = x;
            }
        }
    }
}

