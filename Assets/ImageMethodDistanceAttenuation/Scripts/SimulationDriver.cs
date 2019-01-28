using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EditorPlot;

namespace ImageMethodDistanceAttenuation
{
    public class SimulationDriver : MonoBehaviour
    {
        public enum Simulators
        {
            Classic,
            NegativeReflectionCoefficients,
        }

        public Simulators selectedSimulator;

        public int maxReflection = 10;
        public AudioSource source;
        public Transform listener;
        public RoomDefinition room;
        public int samplingFrequency = 48000;
        public float maxResponseSecond = 4.0f;
        public float windowDurationSeconds = 0.05f;

        [SerializeField] private DoubleArray impulseResponse = new DoubleArray();

        private IImpulseResponseSimulator simulator;
        private DistanceAttenuator attenuator;
        private AttenuationCurveDrawer attenuationCurveDrawer;

        private void Start()
        {
            switch (selectedSimulator)
            {
                case Simulators.Classic:
                    simulator = new ClassicImpulseResponseSimulator();
                    break;
                case Simulators.NegativeReflectionCoefficients:
                    simulator = new NegativeReflectionCoefficientImpulseResponseSimulator();
                    break;
                default:
                    simulator = null;
                    break;
            }

            impulseResponse.samples = new double[Mathf.CeilToInt(samplingFrequency * maxResponseSecond)];
            impulseResponse.xStep = 1.0 / samplingFrequency;
            attenuator = GetComponent<DistanceAttenuator>();
            attenuationCurveDrawer = GetComponent<AttenuationCurveDrawer>();
        }

        private void Update()
        {
            if (simulator == null) return;

            var sourcePosition = source.transform.position;
            var listenerPosition = listener.position;
            simulator.room = room;
            simulator.sourcePosition = sourcePosition;
            simulator.listenerPosition = listenerPosition;
            simulator.maxReflection = maxReflection;
            simulator.samplingFrequency = samplingFrequency;
            simulator.Simulate(impulseResponse.samples);

            var irProperty = ImpulseResponseAnalyzer.Analyze(impulseResponse.samples,
                samplingFrequency,
                windowDurationSeconds);
            attenuator.Attenuate(irProperty);

            attenuationCurveDrawer.UpdateCurve(source.volume);
        }
    }
}
