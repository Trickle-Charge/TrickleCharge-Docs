using UnityEngine;

namespace TrickleCharge.Math.Waves.Components
{
[RequireComponent(typeof(AudioSource))]
public class WaveAudioSynthesizer : MonoBehaviour
{
    [SerializeField]
    private WaveCollection m_wave;

    [SerializeField, Range(0f, 1f)]
    private float m_volume = 0.2f;

    private double _samplingFrequency;
    private double _preciseAudioTime;

    private void Awake()
    {
        // Usually 44100Hz or 48000Hz based on project audio settings
        _samplingFrequency = AudioSettings.outputSampleRate;
    }

    // WARNING: This method runs on Unity's background Audio Thread, NOT the Main Thread.
    private void OnAudioFilterRead(float[] data, int channels)
    {

        if (m_wave is not { Enabled: true }) { return; }

        // Determine exact time delta per individual sample block slice
        double deltaT = 1.0 / _samplingFrequency;

        for (int i = 0; i < data.Length; i += channels)
        {
            // 1. Sample the wave evaluation using sample-accurate high-precision time
            float waveSample = m_wave.Evaluate((float)_preciseAudioTime, 0f) * m_volume;

            // 2. Interleave the signal across all available speaker channels (Mono/Stereo/Surround)
            for (int channel = 0; channel < channels; channel++)
            {
                data[i + channel] = waveSample;
            }

            // 3. Increment internal time step precisely
            _preciseAudioTime += deltaT;
        }
    }
}
}
