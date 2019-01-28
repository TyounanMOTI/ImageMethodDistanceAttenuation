using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace ImageMethodDistanceAttenuation
{
    [CustomEditor(typeof(SingleAxisImageSourceVisualizer))]
    public class SingleAxisImageSourceVisualizerEditor : Editor
    {
        private CubeFace selectedPlane;

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            if (!EditorApplication.isPlaying)
            {
                return;
            }

            var visualizer = target as SingleAxisImageSourceVisualizer;
            if (visualizer == null) return;
            
            selectedPlane = (CubeFace)EditorGUILayout.EnumPopup("反射面", selectedPlane);
            if (GUILayout.Button("反射"))
            {
                visualizer.Reflect(selectedPlane);
            }
        }
    }
}
