using System;

using TrickleCharge.Physics.Thrust;

using UnityEditor;

using UnityEngine;

namespace Thrust.Debug
{
[RequireComponent(typeof(Thruster))]
public class ThrusterDebugDisplay : MonoBehaviour
{
    [SerializeField]
    private ForceDebug m_thrustForceDebug;

    private Thruster _thruster;
    private Thruster Thruster => _thruster = _thruster ? _thruster : gameObject.GetComponent<Thruster>();

    private void OnDrawGizmos() => m_thrustForceDebug.DrawThrustForce(Thruster);

    [Serializable]
    private class ForceDebug
    {
        [SerializeField]
        private bool m_enabled = true;

        [SerializeField]
        private bool m_invertDirection;

        [SerializeField]
        private Color m_debugWeakColor = Color.blue;

        [SerializeField]
        private Color m_debugStrongColor = Color.red;

        public void DrawThrustForce(Thruster thruster)
        {
            if(!m_enabled) { return; }
            if(thruster == null) { return; }
            if(thruster.ForceVector.sqrMagnitude == 0) { return; }

            Handles.color = Color.Lerp(m_debugWeakColor, m_debugStrongColor, thruster.ThrustStrength);

            Vector3 forceVector = m_invertDirection
                ? -thruster.ForceVector
                :  thruster.ForceVector;

            Handles.ArrowHandleCap(
                0,
                thruster.ForcePosition,
                Quaternion.LookRotation(forceVector),
                thruster.ForceVector.magnitude,
                EventType.Repaint
            );
        }
    }
}
}
