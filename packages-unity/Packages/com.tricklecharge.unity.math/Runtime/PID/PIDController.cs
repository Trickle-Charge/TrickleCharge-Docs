using UnityEngine;

namespace TrickleCharge.Math.PID
{
[System.Serializable]
public class PidController
{
    [SerializeField]
    private float m_pGain, m_iGain, m_dGain;

    [SerializeField, Space(5)]
    private float m_iClamp = 10f;

    private float _integral;
    private float _lastError;

    public float Update(float dt, float current, float target)
    {
        float error = target - current;
        _integral += error * dt;
        _integral = Mathf.Clamp(_integral, -m_iClamp, m_iClamp);

        float derivative = (error - _lastError) / dt;
        _lastError = error;

        return (error * m_pGain) + (_integral * m_iGain) + (derivative * m_dGain);
    }

    public void Reset()
    {
        _integral = 0;
        _lastError = 0;
    }
}
}
