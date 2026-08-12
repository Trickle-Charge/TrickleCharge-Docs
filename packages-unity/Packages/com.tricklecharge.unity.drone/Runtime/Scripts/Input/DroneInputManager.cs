using System;

using TrickleCharge.Attributes;

using UnityEngine;
using UnityEngine.InputSystem;

namespace TrickleCharge.Drone.Input
{
public class DroneInputManager : MonoBehaviour
{
    [Header("Input Actions")]
    [SerializeField]
    private InputActionProperty m_throttleAction;

    [SerializeField]
    private AnimationCurve m_throttleCurve = DefaultCurve;

    [SerializeField]
    private InputActionProperty m_yawAction;

    [SerializeField]
    private AnimationCurve m_yawCurve = DefaultCurve;

    [SerializeField]
    private InputActionProperty m_pitchAction;

    [SerializeField]
    private AnimationCurve m_pitchCurve = DefaultCurve;

    [SerializeField]
    private InputActionProperty m_rollAction;

    [SerializeField]
    private AnimationCurve m_rollCurve = DefaultCurve;

    [SerializeField]
    private InputActionProperty m_toggleThrustAction;

    [SerializeField]
    private InputActionProperty m_swapFlightModeAction;

    [SerializeField]
    private InputActionProperty m_toggleGravityCompensationAction;

    [SerializeField]
    private InputActionProperty m_resetAction;

    [SerializeField, ReadOnly]
    private FlightInputData m_flightInputData;

    public float PitchInput => ApplyCurve(m_pitchAction.action, m_pitchCurve);
    public float YawInput => ApplyCurve(m_yawAction.action, m_yawCurve);
    public float RollInput => ApplyCurve(m_rollAction.action, m_rollCurve);
    public float ThrottleInput => ApplyCurve(m_throttleAction.action, m_throttleCurve);

    private static float ApplyCurve(InputAction inputAction, AnimationCurve curve)
    {
        float inputValue = inputAction.ReadValue<float>();
        return inputValue * curve.Evaluate(Mathf.Abs(inputValue));
    }

    private static AnimationCurve DefaultCurve => AnimationCurve.Linear(0,0,1,1);

    public FlightInputData InputData => m_flightInputData = new(PitchInput, YawInput, RollInput, ThrottleInput);

    public event Action OnToggleThrustEvent;
    public event Action OnSwapFlightModeEvent;
    public event Action OnToggleGravityCompensationEvent;
    public event Action OnResetEvent;

    private void OnEnable()
    {
        m_throttleAction.action.Enable();
        m_yawAction.action.Enable();
        m_pitchAction.action.Enable();
        m_rollAction.action.Enable();
        m_toggleThrustAction.action.Enable();
        m_swapFlightModeAction.action.Enable();
        m_toggleGravityCompensationAction.action.Enable();
        m_resetAction.action.Enable();
    }

    private void OnDisable()
    {
        m_throttleAction.action.Disable();
        m_yawAction.action.Disable();
        m_pitchAction.action.Disable();
        m_rollAction.action.Disable();
        m_toggleThrustAction.action.Disable();
        m_swapFlightModeAction.action.Disable();
        m_toggleGravityCompensationAction.action.Disable();
        m_resetAction.action.Disable();
    }

    private void Update() => InvokeEvents();

    private void InvokeEvents()
    {
        if(m_toggleThrustAction.action.triggered) { OnToggleThrustEvent?.Invoke(); }
        if(m_swapFlightModeAction.action.triggered) { OnSwapFlightModeEvent?.Invoke(); }
        if(m_toggleGravityCompensationAction.action.triggered) { OnToggleGravityCompensationEvent?.Invoke(); }
        if(m_resetAction.action.triggered) { OnResetEvent?.Invoke(); }
    }

    [Serializable]
    public struct FlightInputData
    {
        [SerializeField]
        private float m_pitchInput;

        [SerializeField]
        private float m_yawInput;

        [SerializeField]
        private float m_rollInput;

        [SerializeField]
        private float m_throttleInput;

        public float PitchInput
        {
            readonly get => m_pitchInput;
            private set => m_pitchInput = value;
        }

        public float YawInput
        {
            readonly get => m_yawInput;
            private set => m_yawInput = value;
        }

        public float RollInput
        {
            readonly get => m_rollInput;
            private set => m_rollInput = value;
        }

        public float ThrottleInput
        {
            readonly get => m_throttleInput;
            private set => m_throttleInput = value;
        }

        public FlightInputData(float pitch, float yaw, float roll, float throttle)
        {
            m_pitchInput = pitch;
            m_yawInput = yaw;
            m_rollInput = roll;
            m_throttleInput = throttle;
        }
    }
}
}
