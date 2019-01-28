using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

namespace ImageMethodDistanceAttenuation
{
    public class AttenuationCurveDrawer : MonoBehaviour
    {
        public RoomDefinition room;
        private AudioSource source;
        private AudioListener listener;

        private void Start()
        {
            source = GetComponent<AudioSource>();
            listener = FindObjectOfType<AudioListener>();

            //Clear Curve
            source.SetCustomCurve(AudioSourceCurveType.CustomRolloff, new AnimationCurve(new Keyframe(0, 1)));
            source.rolloffMode = AudioRolloffMode.Custom;
            source.maxDistance = Vector3.Magnitude(room.size);
        }

        public void UpdateCurve(float volume)
        {
            var curve = source.GetCustomCurve(AudioSourceCurveType.CustomRolloff);
            var distance = Vector3.Distance(listener.transform.position, transform.position);
            curve.AddKey(new Keyframe(distance / source.maxDistance, Mathf.Clamp01(volume)));

            source.SetCustomCurve(AudioSourceCurveType.CustomRolloff, curve);
        }
    }
}
