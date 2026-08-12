using System;
using System.Collections.Generic;

using UnityEngine;

namespace TrickleCharge.Math.Waves.Components
{
public class LineRendererWave : MonoBehaviour
{
    [SerializeField]
    private LineRenderer m_lineRenderer;

    [SerializeField]
    private WaveCollection m_waves;

    [SerializeField]
    private int m_length = 10;

    [SerializeField]
    private int m_resolution = 10;

    // Allocate the collection shell once at the class footprint level to prevent garbage generation
    private readonly List<Keyframe> _keyframeBuffer = new();
    private Vector3[] _positionBuffer = Array.Empty<Vector3>();

    private void Update()
    {
        if (m_lineRenderer == null || m_waves == null) { return; }
        if (m_length <= 0 || m_resolution <= 0) { return; }

        int totalPoints = m_length * m_resolution;

        // 1. Clear the persistent data buffer shell for the next tick frame stream
        _keyframeBuffer.Clear();

        // 2. Stream spatial snap points non-allocatingly out of your core math assembly
        WaveUtility.GenerateWaveKeyframesNonAlloc(
            m_waves,
            Time.timeSinceLevelLoad,
            totalPoints,
            m_length,
            _keyframeBuffer
        );

        // 3. Ensure your local array buffer matches the size of points collected
        if (_positionBuffer.Length != totalPoints)
        {
            _positionBuffer = new Vector3[totalPoints];
        }

        // 4. Translate mathematical Keyframes into 3D Vector coordinates
        for (int i = 0; i < totalPoints; i++)
        {
            Keyframe kf = _keyframeBuffer[i];
            _positionBuffer[i] = new Vector3(kf.time, kf.value, 0f);
        }

        // 5. Update LineRenderer boundaries completely in a single flat non-alloc block
        m_lineRenderer.positionCount = totalPoints;
        m_lineRenderer.SetPositions(_positionBuffer);
    }
}
}
