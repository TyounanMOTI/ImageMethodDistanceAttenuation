using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEditor;
using UnityEngine;
using System.Linq;

namespace EditorPlot
{
    [CustomPropertyDrawer(typeof(DoubleArray))]
    public class DoubleArrayDrawer : PropertyDrawer
    {
        private const float WidthPerHeight = 2.5f;
        private const float PlotPadding = 0.05f;
        private readonly Vector2 LabelInterval = new Vector2(0.1f, 0.05f);
        private readonly Vector2 GridInterval = new Vector2(0.1f, 0.025f);
        private FloatRange xRange = new FloatRange(-0.05f, 0.5f);
        private FloatRange yRange = new FloatRange(-0.1f, 0.1f);

        private Vector3[] dataPoints;

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            var rect = new Rect(position.x, position.y, Screen.width, Screen.width / WidthPerHeight);
            Handles.DrawSolidRectangleWithOutline(rect, Color.white, Color.white);

            var plotRect = new Rect(
                rect.x + rect.width * PlotPadding,
                rect.y + rect.height * PlotPadding * WidthPerHeight,
                rect.width * (1.0f - PlotPadding * 2.0f),
                rect.height * (1.0f - PlotPadding * 2.0f * WidthPerHeight));
            Handles.DrawSolidRectangleWithOutline(plotRect, new Color(0, 0, 0, 0), Color.black);

            // X Axis Label
            Handles.color = Color.black;
            var xLabelIntervalPixel = plotRect.width / (xRange.Distance() / LabelInterval.x);
            for (var x = plotRect.x; x < plotRect.xMax; x += xLabelIntervalPixel)
            {
                if (x < plotRect.x) continue;
                Handles.Label(new Vector3(x, plotRect.yMax + 3.0f),
                    $"{(x - plotRect.x) / plotRect.width * xRange.Distance() + xRange.min:0.00}");
            }

            // X Axis Grid
            Handles.color = Color.black;
            var xGridIntervalPixel = plotRect.width / (xRange.Distance() / GridInterval.x);
            for (var x = plotRect.x; x < plotRect.xMax; x += xGridIntervalPixel)
            {
                Handles.DrawLine(new Vector3(x, plotRect.yMax), new Vector3(x, plotRect.yMax - 10.0f));
            }

            // Y Axis Label
            Handles.color = Color.black;
            var yLabelIntervalPixel = plotRect.height / (yRange.Distance() / LabelInterval.y);
            for (var y = plotRect.yMax; y > plotRect.yMin; y -= yLabelIntervalPixel)
            {
                Handles.Label(new Vector3(plotRect.xMin - 35.0f, y - 10.0f), 
                    $"{(plotRect.height - (y - plotRect.y)) / plotRect.height * yRange.Distance() + yRange.min:0.00}");
            }

            // Y Axis Grid
            Handles.color = Color.black;
            var yGridIntervalPixel = plotRect.height / (yRange.Distance() / GridInterval.y);
            for (var y = plotRect.yMax; y > plotRect.yMin; y -= yGridIntervalPixel)
            {
                Handles.DrawLine(new Vector3(plotRect.xMin, y), new Vector3(plotRect.xMin + 10.0f, y));
            }

            var originPosition = new Vector3(plotRect.x + (0.0f - xRange.min) / xRange.Distance() * plotRect.width,
                plotRect.yMax - (0.0f - yRange.min) / yRange.Distance() * plotRect.height);

            // X Axis Zero Line
            Handles.color = new Color(0, 0, 0, 0.5f);
            Handles.DrawLine(new Vector3(originPosition.x, plotRect.y), new Vector3(originPosition.x, plotRect.yMax));

            // Y Axis Zero Line
            Handles.color = new Color(0, 0, 0, 0.5f);
            Handles.DrawLine(new Vector3(plotRect.x, originPosition.y), new Vector3(plotRect.xMax, originPosition.y));

            if (!(GetValue(property.FindPropertyRelative("samples")) is double[] samples)) return;
            
            var dataLength = samples.Length;
            if (dataPoints == null || dataPoints.Length < dataLength)
            {
                dataPoints = new Vector3[dataLength];
            }

            Handles.color = Color.red;

            var xStep = property.FindPropertyRelative("xStep").doubleValue;
            for (var i = 0; i < dataLength; i++)
            {
                var xValue = i * xStep;
                var x = plotRect.x + (xValue - xRange.min) / xRange.Distance() * plotRect.width;
                if (x > plotRect.xMax)
                {
                    dataPoints[i] = dataPoints[i - 1];
                }
                else
                {
                    dataPoints[i] = new Vector3((float)x,
                        (float)(plotRect.yMax - (samples[i] - yRange.min) / yRange.Distance() * plotRect.height));
                }
            }
            Handles.DrawPolyLine(dataPoints);
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            return Screen.width / WidthPerHeight;
        }

        public override bool CanCacheInspectorGUI(SerializedProperty property)
        {
            return false;
        }

        private static object GetValue(SerializedProperty property)
        {
            var target = property.serializedObject.targetObject as object;
            var path = property.propertyPath;
            var elements = path.Split('.');
            foreach (var element in elements)
            {
                var fields = target.GetType()
                    .GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
                var field = fields.First(x => x.Name == element);
                target = field.GetValue(target);
            }

            return target;
        }
    }
}
