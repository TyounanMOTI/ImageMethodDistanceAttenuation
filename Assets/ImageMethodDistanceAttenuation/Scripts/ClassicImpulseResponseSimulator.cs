using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ImageMethodDistanceAttenuation
{
    public class ClassicImpulseResponseSimulator : IImpulseResponseSimulator
    {
        public RoomDefinition room { get; set; }
        public Vector3 sourcePosition { get; set; }
        public Vector3 listenerPosition { get; set; }
        public int maxReflection { get; set; }
        public int samplingFrequency { get; set; }

        private Vector3[] imageSourcePositionBuffer;

        public void Simulate(double[] result)
        {
            var imageSourceCount = ImageSourcePositionCalculator.GetImageSourceCount(maxReflection);
            if (imageSourcePositionBuffer == null || imageSourcePositionBuffer.Length < imageSourceCount)
            {
                imageSourcePositionBuffer = new Vector3[imageSourceCount];
            }

            ImageSourcePositionCalculator.GetImageSourcePositions(sourcePosition, room, maxReflection,
                imageSourcePositionBuffer);

            for (var i = 0; i < result.Length; i++)
            {
                result[i] = 0.0f;
            }

            for (var i = 0; i < imageSourceCount; i++)
            {
                var imageSourcePosition = imageSourcePositionBuffer[i];
                var distance = Vector3.Distance(imageSourcePosition, listenerPosition);
                var delay = distance / room.speedOfSound;
                var delaySamples = Mathf.RoundToInt(delay * samplingFrequency);

                if (delaySamples >= result.Length) continue;

                result[delaySamples] += 1.0 / (4.0f * Mathf.PI * distance);
            }
        }
    }
}
