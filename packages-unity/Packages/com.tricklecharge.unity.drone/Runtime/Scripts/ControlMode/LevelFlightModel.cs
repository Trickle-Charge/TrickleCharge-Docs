using System;

using TrickleCharge.Math.PID;
using TrickleCharge.Drone.Input;

using UnityEngine;

namespace TrickleCharge.Drone.ControlMode
{
[Serializable]
public class LevelFlightModel : FlightModel
{
    [SerializeField]
    private PidController m_verticalVelocityPid;
    private PidController VerticalVelocityPid => m_verticalVelocityPid;

    public float TargetVerticalVelocity { get; set; }

    /// <inheritdoc />
    protected override void UpdateLogic(float deltaTime, Rigidbody rigidbody, DroneInputManager.FlightInputData input)
    {
        if (rigidbody == null) { return; }

        Quaternion targetRotation = Quaternion.Euler(m_targetEulerEulerAngles);
        Quaternion errorQuat = Quaternion.Inverse(rigidbody.rotation) * targetRotation;

        // Convert error into local-space axis angles
        // This gives the "shortest path" for each axis without Euler flipping
        errorQuat.ToAngleAxis(out float angle, out Vector3 axis);

        // Normalize angle to -180 to 180 range
        if (angle > 180) { angle -= 360; }

        Vector3 localError = axis * angle;

        AxisCorrection = new Vector3
        {
            x = PitchPid.Update(deltaTime, localError.x, 0),
            y = YawPid.Update(deltaTime, localError.y, 0),
            z = RollPid.Update(deltaTime, localError.z, 0)
        };

        // Target vertical velocity (Throttle)
        TargetVerticalVelocity = input.ThrottleInput * m_sensitivityThrottle;
        Throttle = VerticalVelocityPid.Update(deltaTime, rigidbody.linearVelocity.y, TargetVerticalVelocity);

        // Offset gravity
        GravityCompensation = Mathf.Abs(UnityEngine.Physics.gravity.y) * rigidbody.mass;
    }

    protected override void CalculateTargetEulerAngles(DroneInputManager.FlightInputData input, ref Vector3 targetEulerAngles)
    {
        // Yaw: Rotate target angle over time (Degrees per second)
        targetEulerAngles.y += input.YawInput * m_sensitivityYaw * Time.deltaTime;

        // Pitch/Roll: Direct tilt control
        // Raw stick input to set the "Desired Angle"
        targetEulerAngles.x = input.PitchInput * m_sensitivityPitch;
        targetEulerAngles.z = -input.RollInput * m_sensitivityRoll;
    }

    public override void Reset()
    {
        base.Reset();
        VerticalVelocityPid.Reset();
        TargetVerticalVelocity = 0;
    }
}
}
