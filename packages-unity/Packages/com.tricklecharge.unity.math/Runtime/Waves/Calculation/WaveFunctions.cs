using System.Collections.Generic;

using UnityEngine;

namespace TrickleCharge.Math.Waves.Calculation
{
public static class WaveFunctions
{
    public static WaveFunction Sin => SinFunction;
    public static WaveFunction Cos => CosFunction;
    public static WaveFunction Tan => TanFunction;
    public static WaveFunction Square => SquareFunction;

    public static float SinFunction(float time, float position, float phase, float amplitude, float frequency, float velocity, float bias)
    {
        return Mathf.Sin((position + time * velocity + phase) * frequency) * amplitude + bias;
    }

    public static float SinFunction(float time, float position, Wave wave)
    {
        return SinFunction(time, position, wave.Phase, wave.Amplitude, wave.Frequency, wave.Velocity, wave.Bias);
    }

    public static float CosFunction(float time, float position, float phase, float amplitude, float frequency, float velocity, float bias)
    {
        return Mathf.Cos((position + time * velocity + phase) * frequency) * amplitude + bias;
    }

    public static float CosFunction(float time, float position, Wave wave)
    {
        return CosFunction(time, position, wave.Phase, wave.Amplitude, wave.Frequency, wave.Velocity, wave.Bias);
    }

    public static float TanFunction(float time, float position, float phase, float amplitude, float frequency, float velocity, float bias)
    {
        return Mathf.Tan((position + time * velocity + phase) * frequency) * amplitude + bias;
    }

    public static float TanFunction(float time, float position, Wave wave)
    {
        return TanFunction(time, position, wave.Phase, wave.Amplitude, wave.Frequency, wave.Velocity, wave.Bias);
    }

    public static float SquareFunction(float time, float position, float phase, float amplitude, float frequency, float velocity, float bias)
    {
        return Mathf.Sign(Mathf.Sin((position + time * velocity + phase) * frequency)) * amplitude + bias;
    }

    public static float PerlinFunction(float time, float position, float phase, float amplitude, float frequency, float velocity, float bias)
    {
        //return Mathf.PerlinNoise1D(Mathf.Sin((position + time * velocity + phase) * frequency) * amplitude + bias);
        return Mathf.PerlinNoise1D((position + time * velocity + phase) * frequency) * amplitude + bias;
    }

    public enum WaveFunctionType
    {
        Sin,
        Cos,
        Tan,
        Square,
        Perlin
    }

    private static readonly Dictionary<WaveFunctionType, WaveFunction> s_waveFunctionsByType = new()
    {
        { WaveFunctionType.Sin, SinFunction },
        { WaveFunctionType.Cos, CosFunction },
        { WaveFunctionType.Tan, TanFunction },
        { WaveFunctionType.Square, SquareFunction },
        { WaveFunctionType.Perlin, PerlinFunction },
    };

    public static WaveFunction FunctionByType(WaveFunctionType type) => s_waveFunctionsByType[type];
}
}
