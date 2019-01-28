using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace ImageMethodDistanceAttenuation
{
    [CustomEditor(typeof(ImageSourceVisualizer))]
    public class ImageSourceVisualizerEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            if (!EditorApplication.isPlaying)
            {
                return;
            }
            
            var visualizer = target as ImageSourceVisualizer;
            if (visualizer == null) return;

            if (GUILayout.Button("反射"))
            {
                visualizer.Reflect();
            }

            visualizer.showPath = GUILayout.Toggle(visualizer.showPath, visualizer.showPath ? "パス表示" : "パス非表示", "button");
        }
    }
}
