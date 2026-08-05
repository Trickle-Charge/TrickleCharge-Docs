using System;

using TrickleCharge.Math.Waves.Calculation;

using UnityEngine;

namespace TrickleCharge.Math.Waves
{
[Serializable]
public class Wave : IWave
{
    [Tooltip("Should the wave be evaluated?")]
    [SerializeField]
    private bool m_enabled = true;

    [Tooltip("The equilibrium point")]
    [SerializeField]
    private float m_bias;

    [Tooltip("Time offset")]
    [SerializeField]
    private float m_phase;

    [Tooltip("Maximum displacement from the Bias value")]
    [SerializeField]
    private float m_amplitude = 1f;

    [Tooltip("Wave frequency in Hz")]
    [SerializeField]
    private float m_frequency = 1f;

    [SerializeField]
    private float m_velocity = 1f;

    [SerializeField]
    private WaveFunctions.WaveFunctionType m_type = WaveFunctions.WaveFunctionType.Sin;

    public bool Enabled
    {
        get => m_enabled;
        set => m_enabled = value;
    }

    public float Bias
    {
        get => m_bias;
        set => m_bias = value;
    }

    public float Phase
    {
        get => m_phase;
        set => m_phase = value;
    }

    public float Amplitude
    {
        get => m_amplitude;
        set => m_amplitude = value;
    }

    public float Frequency
    {
        get => m_frequency;
        set => m_frequency = value;
    }

    public float Velocity
    {
        get => m_velocity;
        set => m_velocity = value;
    }

    public WaveFunctions.WaveFunctionType Type
    {
        get => m_type;
        set => m_type = value;
    }

    private WaveFunction Function => WaveFunctions.FunctionByType(m_type);

    public float Evaluate(float time, float position = 0) => Evaluate(Function, time, position);

    public float Evaluate(WaveFunction function, float time, float position = 0)
    {
        if(! Enabled) { return 0; }

        return function.Invoke(time, position, Phase, Amplitude, Frequency, Velocity, Bias);
    }
}
}
