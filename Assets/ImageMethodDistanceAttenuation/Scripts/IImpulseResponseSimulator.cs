using UnityEngine;

namespace ImageMethodDistanceAttenuation
{
    public interface IImpulseResponseSimulator
    {
        RoomDefinition room { get; set; }
        Vector3 sourcePosition { get; set; }
        Vector3 listenerPosition { get; set; }
        int maxReflection { get; set; }
        int samplingFrequency { get; set; }
        void Simulate(double[] result);
    }
}
