using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace ImageMethodDistanceAttenuation
{
    public class NegativeReflectionCoefficientImpulseResponseSimulator : IImpulseResponseSimulator
    {
        public RoomDefinition room { get; set; }
        public Vector3 sourcePosition { get; set; }
        public Vector3 listenerPosition { get; set; }
        public int maxReflection { get; set; }
        public int samplingFrequency { get; set; }

        public void Simulate(double[] result)
        {
            for (var i = 0; i < result.Length; i++)
            {
                result[i] = 0.0f;
            }
            
            var imageSourcePosition = new Vector3();
            for (var x = -maxReflection; x <= maxReflection; x++)
            {
                imageSourcePosition.x = x * room.size.x + ((x + 1) & 1) * sourcePosition.x +
                                        (x & 1) * (room.size.x - sourcePosition.x);
                for (var y = -maxReflection; y <= maxReflection; y++)
                {
                    imageSourcePosition.y = y * room.size.y + ((y + 1) & 1) * sourcePosition.y +
                                            (y & 1) * (room.size.y - sourcePosition.y);
                    for (var z = -maxReflection; z <= maxReflection; z++)
                    {
                        imageSourcePosition.z = z * room.size.z + ((z + 1) & 1) * sourcePosition.z +
                                                (z & 1) * (room.size.z - sourcePosition.z);

                        var distance = Vector3.Distance(imageSourcePosition, listenerPosition);
                        var reflectionAttenuation = Math.Pow(- room.uniformReflectionCoefficient, Math.Abs(x)) *
                                                    Math.Pow(- room.uniformReflectionCoefficient, Math.Abs(y)) *
                                                    Math.Pow(- room.uniformReflectionCoefficient, Math.Abs(z));
                        var attenuationDistance = distance < 1.0 ? Math.Sqrt(distance) : distance;
                        var attenuation = reflectionAttenuation / (4.0f * Math.PI * attenuationDistance);
                        var delay = distance / room.speedOfSound;
                        var delaySamples = Mathf.RoundToInt(delay * samplingFrequency);
                        if (delaySamples >= result.Length) continue;

                        result[delaySamples] += 1.0 * attenuation;
                    }
                }
            }
        }
    }
}
