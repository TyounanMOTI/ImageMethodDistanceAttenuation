using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace ImageMethodDistanceAttenuation
{
    public struct ImpulseResponseProperty
    {
        public double strength;
    }
    
    public static class ImpulseResponseAnalyzer
    {
        public static ImpulseResponseProperty Analyze(
            double[] impulseResponse, float samplingFrequency, float windowDurationSeconds)
        {
            var squareSum = 0.0;
            var directSoundIndex = -1;
            var windowDurationSamples = Mathf.CeilToInt(windowDurationSeconds * samplingFrequency);
            for (var i = 0; i < impulseResponse.Length; i++)
            {
                var sample = impulseResponse[i];
                var absSample = Math.Abs(sample);
                if (directSoundIndex < 0 && absSample > 0.0f)
                {
                    directSoundIndex = i;
                }

                if (directSoundIndex < 0 && absSample <= 0.0f)
                {
                    continue;
                }

                if (i > directSoundIndex + windowDurationSamples)
                {
                    break;
                }

                squareSum += Math.Pow(sample, 2.0);
            }
            var meanSquare = squareSum / windowDurationSamples;
            var rms = Math.Sqrt(meanSquare);

            var originalRms = Math.Sqrt(1.0 / windowDurationSamples);

            return new ImpulseResponseProperty()
            {
                strength = rms / originalRms
            };
        }
    }
}
