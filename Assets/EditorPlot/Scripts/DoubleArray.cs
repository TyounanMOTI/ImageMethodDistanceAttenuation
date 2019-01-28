using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace EditorPlot
{
    [Serializable]
    public class DoubleArray
    {
        public double[] samples;
        public double xStep = 1.0;
    }
}
