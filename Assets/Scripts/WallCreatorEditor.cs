using UnityEngine;
using UnityEditor;

namespace Marcosdanix.PerlinModulation
{
    [CustomEditor(typeof(WallCreator))]
    class WallCreatorEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();

            WallCreator wg = (WallCreator)target;
            if (GUILayout.Button("Generate"))
            {
                wg.Create();
            }
            /*if (GUILayout.Button("Reset"))
            {
                wg.Reset();
            }*/

        }
    }
}
