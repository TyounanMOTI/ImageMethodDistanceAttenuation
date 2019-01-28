using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ImageMethodDistanceAttenuation
{
    [RequireComponent(typeof(AudioSource))]
    public class DistanceAttenuator : MonoBehaviour
    {
        public float gain = 1.0f;
        private AudioSource source;

        private void Start()
        {
            source = GetComponent<AudioSource>();
            source.spatialBlend = 0.0f;
        }

        public void Attenuate(ImpulseResponseProperty irProperty)
        {
            source.volume = (float)irProperty.strength * gain;
        }
    }
}
