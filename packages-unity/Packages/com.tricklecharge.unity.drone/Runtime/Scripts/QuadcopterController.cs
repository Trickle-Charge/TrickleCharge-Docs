using System.Collections.Generic;
using System.Linq;

using TrickleCharge.Physics.Thrust;
using TrickleCharge.Drone.ControlMode;
using TrickleCharge.Drone.Input;

using UnityEngine;

namespace TrickleCharge.Drone
{
public class QuadcopterController : MonoBehaviour
{
    [Header("References")]
    [SerializeField]
    private DroneInputManager m_droneInputManager;

    [SerializeField]
    private float m_throttleModifier = 1f;

    [SerializeField]
    private List<Thruster.ThrusterMap> m_thrusterMap;

    [Header("PID Settings")]
    [SerializeField]
    private DronePid m_pid;

    [Header("Physics Settings")]
    [Tooltip("How much rotation torque is generated per unit of force")]
    public float m_torqueCoefficient = 0.01f;

    public DronePid Pid => m_pid;
    public Rigidbody Rigidbody => Pid.Rigidbody;

    public float ThrottleModifier
    {
        get => m_throttleModifier;
        set => m_throttleModifier = value;
    }

    public IEnumerable<Thruster.ThrusterMap> ThrusterMap => m_thrusterMap;
    public IEnumerable<Thruster> Thrusters => ThrusterMap.Select(static map => map.Thruster);

    private void Awake()
    {
        if(m_droneInputManager == null) { return; }

        m_droneInputManager.OnToggleThrustEvent += ToggleThrottle;
        m_droneInputManager.OnSwapFlightModeEvent += SwapFlightMode;
        m_droneInputManager.OnToggleGravityCompensationEvent += ToggleGravityCompensation;
        m_droneInputManager.OnResetEvent += ResetDrone;
    }

    private void FixedUpdate()
    {
        if(m_droneInputManager == null) { return; }

        Pid.Update(Time.fixedDeltaTime, m_droneInputManager.InputData);

        ApplyThrusterForces(Pid, m_thrusterMap);
    }

    private void ApplyThrusterForces(DronePid pid, ICollection<Thruster.ThrusterMap> thrusterMap)
    {
        if (pid.Rigidbody == null) { return; }

        // Mix and Apply Forces
        foreach (Thruster.ThrusterMap map in thrusterMap)
        {
            if (map.Thruster == null) { continue; }

            float throttleComponent = pid.Throttle + (pid.GravityCompensation / thrusterMap.Count);
            float correctionComponent = (pid.PitchCorrection * map.PitchWeight) +
                                        (pid.RollCorrection * map.RollWeight) +
                                        (pid.YawCorrection * map.YawWeight);

            float finalForce = throttleComponent + correctionComponent;

            // If we are over the limit, reduce the throttle part to keep the rotation logic intact
            if (finalForce > map.Thruster.MaxForce)
            {
                float overflow = finalForce - map.Thruster.MaxForce;
                finalForce -= overflow; // Reduces total force but preserves the 'ratio' of the correction
            }

            // Set the force (your class handles the MaxForce clamping)
            map.Thruster.SetForce(finalForce * ThrottleModifier);

            // Apply to the Rigidbody
            map.Thruster.ApplyForce(pid.Rigidbody);

            // Apply simulated Yaw
            Vector3 torqueVector = map.Thruster.transform.up * (map.Thruster.CurrentForce * map.YawWeight * -m_torqueCoefficient);
            pid.Rigidbody.AddTorque(torqueVector, ForceMode.Force);
        }
    }

    public void ResetDrone()
    {
        // Reset Rigidbody velocities
        Pid.Rigidbody.linearVelocity = Vector3.zero;
        Pid.Rigidbody.angularVelocity = Vector3.zero;

        // Reset PID controllers
        Pid.Reset();

        // Reset rotation and target rotation
        Vector3 newRotation = Vector3.zero;
        newRotation.y = Rigidbody.rotation.eulerAngles.y;
        Rigidbody.MoveRotation(Quaternion.Euler(newRotation));
        Pid.FlightModel.TargetEulerAngles = newRotation;

        ThrottleModifier = 1f;
    }

    private void ToggleThrottle()
    {
        ThrottleModifier = ThrottleModifier == 0f ? 1f : 0f;
        Pid.Reset();
    }

    private void SwapFlightMode()
    {
        Pid.FlightMode = Pid.FlightMode == FlightMode.Level
            ? FlightMode.Acro
            : FlightMode.Level;

        Pid.Reset();
    }

    private void ToggleGravityCompensation() => Pid.GravityCompensationEnabled = !Pid.GravityCompensationEnabled;
}
}
