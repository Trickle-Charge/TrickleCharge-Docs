using System;

using TrickleCharge.Attributes;

using Thrust;

using UnityEngine;

namespace TrickleCharge.Physics.Thrust
{
public class Thruster : MonoBehaviour, IThruster
{
    [SerializeField]
    private float m_maxForce;

    [SerializeField, ReadOnly]
    private float m_currentForce;

    private Transform ThrustTransform => transform;

    public Vector3 ForceVector => ThrustTransform.up * CurrentForce;
    public Vector3 ForcePosition => ThrustTransform.position;

    public float ThrustStrength => ForceVector.magnitude / MaxForce;

    /// <inheritdoc />
    public float MaxForce => m_maxForce;

    /// <inheritdoc />
    public float CurrentForce => m_currentForce;

    /// <inheritdoc />
    public void SetForce(float force) => m_currentForce = Mathf.Clamp(force, -m_maxForce, m_maxForce);

    public void ApplyForce(Rigidbody rigidbody)
    {
        if(! isActiveAndEnabled) { return; }

        rigidbody.AddForceAtPosition(ForceVector, ForcePosition);
    }

    [Serializable]
    public struct ThrusterMap
    {
        [SerializeField]
        public Thruster m_thruster;

        [SerializeField, Tooltip("1 for front, -1 for rear")]
        public float m_pitchWeight;

        [SerializeField, Tooltip("1 for left, -1 for right")]
        public float m_rollWeight;

        [SerializeField, Tooltip("1 for CW, -1 for CCW")]
        public float m_yawWeight;

        public Thruster Thruster => m_thruster;
        public float PitchWeight => m_pitchWeight;
        public float RollWeight => m_rollWeight;
        public float YawWeight => m_yawWeight;
    }
}
}
