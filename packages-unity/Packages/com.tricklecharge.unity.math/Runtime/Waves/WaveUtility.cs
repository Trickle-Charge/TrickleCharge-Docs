using System;
using System.Collections.Generic;

using UnityEngine;

namespace TrickleCharge.Math.Waves
{
public static class WaveUtility
{
    /// <summary>
    /// Generates an evaluation curve layout snapshot of an IWave instance.
    /// </summary>
    public static IEnumerable<Keyframe> GenerateWaveKeyframes(IWave wave, float time, int sampleCount, float sampleRangeX)
    {
        float stepSize = sampleRangeX / sampleCount;
        for (int i = 0; i <= sampleCount; i++)
        {
            float x = i * stepSize;
            float y = wave.Evaluate(time, x);
            yield return new Keyframe(x, y);
        }
    }

    /// <summary>
    /// Performance-focused implementation that streams spatial evaluation steps into an existing list buffer.
    /// Useful for high-frequency inspector repaints or structural runtime tracking.
    /// </summary>
    public static void GenerateWaveKeyframesNonAlloc(IWave wave, float time, int sampleCount, float sampleRangeX, ICollection<Keyframe> resultsBuffer)
    {
        if (resultsBuffer == null) { throw new ArgumentNullException(nameof(resultsBuffer)); }

        float stepSize = sampleRangeX / sampleCount;
        for (int i = 0; i <= sampleCount; i++)
        {
            float x = i * stepSize;
            float y = wave.Evaluate(time, x);
            resultsBuffer.Add(new Keyframe(x, y));
        }
    }
}
}
