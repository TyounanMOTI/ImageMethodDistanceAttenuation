using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EditorPlot
{
    public struct FloatRange
    {
        public float min { get; }
        public float max { get; }

        public FloatRange(float min, float max)
        {
            this.min = min;
            this.max = max;
        }

        public float Distance()
        {
            return max - min;
        }
    }
}
