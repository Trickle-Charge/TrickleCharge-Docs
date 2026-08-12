using System;

using TrickleCharge.Drone.Input;

using UnityEngine;

namespace TrickleCharge.Drone.ControlMode
{
[Serializable]
public class AcroFlightModel : FlightModel
{
    /// <inheritdoc />
    protected override void UpdateLogic(float deltaTime, Rigidbody rigidbody, DroneInputManager.FlightInputData input)
    {
        // Get Local Angular Velocity in Degrees
        Vector3 localAngularVelocityDeg = rigidbody.transform.InverseTransformDirection(rigidbody.angularVelocity) * Mathf.Rad2Deg;

        // Apply Corrections
        // Negate the 'current' values to align with the ThrusterMap weights
        // and provide negative feedback (stabilization).
        AxisCorrection = new Vector3
        {
            x = PitchPid.Update(deltaTime, -localAngularVelocityDeg.x, TargetEulerAngles.x),
            y = YawPid.Update(deltaTime, -localAngularVelocityDeg.y, TargetEulerAngles.y),
            z = RollPid.Update(deltaTime, -localAngularVelocityDeg.z, TargetEulerAngles.z)
        };

        // Manual Throttle (Acro doesn't use Altitude PID)
        // Map stick (0 to 1) directly to a force value (0 to MaxForce)
        // We use a simple multiplier here; 0.5 input ≈ 5 units of force.
        Throttle = input.ThrottleInput * m_sensitivityThrottle;

        // Gravity Compensation
        // In Acro hover at 50% stick
        GravityCompensation = Mathf.Abs(UnityEngine.Physics.gravity.y) * rigidbody.mass; // Should this be relative to drone rotation?
    }

    /// <inheritdoc />
    protected override void CalculateTargetEulerAngles(DroneInputManager.FlightInputData input, ref Vector3 targetEulerAngles)
    {
        // In Acro, all sticks represent DESIRED ROTATION SPEED (Deg/s)
        // Sensitivity should be ~200+
        targetEulerAngles.x = -input.PitchInput * m_sensitivityPitch;
        targetEulerAngles.y = -input.YawInput * m_sensitivityYaw;
        targetEulerAngles.z = input.RollInput * m_sensitivityRoll;
    }
}
}
