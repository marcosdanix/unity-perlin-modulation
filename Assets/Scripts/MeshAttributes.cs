using UnityEngine;

namespace Marcosdanix.PerlinModulation
{
    class MeshAttributes
    {
        public Vector3[] vertices { get; set; }
        public Vector2[] uv { get; set; }
        public int[] triangles { get; set; }
        public Vector3[] normals { get; set; }
        public int[] normalIndex { get; set; }
        //REMEMBER: Unity uses left-hand coordinates. The Tangent should point UP!
        public Vector3[] tangent { get; set; }
        public int[] tangentIndex { get; set; }
    }
}
