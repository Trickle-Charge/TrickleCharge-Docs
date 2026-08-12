using System;

using TrickleCharge.Drone.ControlMode;
using TrickleCharge.Drone.Input;

using UnityEngine;

namespace TrickleCharge.Drone
{
[Serializable]
public class DronePid
{
    [SerializeField]
    private Rigidbody m_rigidbody;

    [Space(10), Header("Flight Mode")]
    [SerializeField]
    private FlightMode m_flightMode;

    [SerializeField]
    public bool m_gravityCompensationEnabled = true;

    [SerializeField]
    private LevelFlightModel m_levelFlightModel;

    [SerializeField]
    private AcroFlightModel m_acroFlightModel;

    public Rigidbody Rigidbody => m_rigidbody;

    public FlightModel FlightModel => m_flightMode switch
    {
        FlightMode.Level => m_levelFlightModel,
        FlightMode.Acro => m_acroFlightModel,
        _ => throw new ArgumentOutOfRangeException()
    };

    public float PitchCorrection => FlightModel.AxisCorrection.x;
    public float YawCorrection   => FlightModel.AxisCorrection.y;
    public float RollCorrection  => FlightModel.AxisCorrection.z;
    public float Throttle => FlightModel.Throttle;
    public float GravityCompensation => m_gravityCompensationEnabled ? FlightModel.GravityCompensation : 0f;

    public FlightMode FlightMode
    {
        get => m_flightMode;
        set => m_flightMode = value;
    }

    public bool GravityCompensationEnabled
    {
        get => m_gravityCompensationEnabled;
        set => m_gravityCompensationEnabled = value;
    }

    private FlightMode _previousFlightMode;

    private DronePid() => _previousFlightMode = FlightMode;

    public void Update(float deltaTime, DroneInputManager.FlightInputData input)
    {
        if (Rigidbody == null) { return; }

        if (FlightMode != _previousFlightMode) { FlightModel.Reset(); }

        FlightModel.Update(deltaTime, Rigidbody, input);
    }

    public void Reset()
    {
        FlightModel.Reset();

        FlightModel.TargetEulerAngles = Rigidbody.rotation.eulerAngles;
    }
}
}
