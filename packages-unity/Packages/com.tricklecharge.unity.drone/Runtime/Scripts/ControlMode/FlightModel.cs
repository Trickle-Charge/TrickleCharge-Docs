using System;

using TrickleCharge.Math.PID;
using TrickleCharge.Drone.Input;

using UnityEngine;

namespace TrickleCharge.Drone.ControlMode
{
[Serializable]
public abstract class FlightModel
{
    [SerializeField]
    protected PidController m_pitchPid;

    [SerializeField]
    protected PidController m_rollPid;

    [SerializeField]
    protected PidController m_yawPid;

    [Header("Sensitivity Settings")]
    [SerializeField, Tooltip("Max tilt angle")] // TODO fix tooltips
    protected float m_sensitivityPitch = 30f;

    [SerializeField, Tooltip("Max tilt angle")]
    protected float m_sensitivityRoll = 30f;

    [SerializeField, Tooltip("Degrees per second")]
    protected float m_sensitivityYaw = 100f;

    [SerializeField, Tooltip("Meters per second")]
    protected float m_sensitivityThrottle = 5f;

    [SerializeField]
    protected Vector3 m_targetEulerEulerAngles;
    public ref Vector3 TargetEulerAngles => ref m_targetEulerEulerAngles;

    protected PidController PitchPid => m_pitchPid;
    protected PidController RollPid => m_rollPid;
    protected PidController YawPid => m_yawPid;

    public float Throttle { get; protected set; }
    public float GravityCompensation { get; protected set; }
    public Vector3 AxisCorrection { get; protected set; }

    public void Update(float deltaTime, Rigidbody rigidbody, DroneInputManager.FlightInputData input)
    {
        CalculateTargetEulerAngles(input, ref TargetEulerAngles);

        UpdateLogic(deltaTime, rigidbody, input);
    }

    protected abstract void UpdateLogic(float deltaTime, Rigidbody rigidbody, DroneInputManager.FlightInputData input);

    protected abstract void CalculateTargetEulerAngles(DroneInputManager.FlightInputData input, ref Vector3 targetEulerAngles);

    public virtual void Reset()
    {
        PitchPid.Reset();
        RollPid.Reset();
        YawPid.Reset();
    }
}
}
