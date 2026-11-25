using System;
using System.Collections.Generic;
using System.Linq;

using TrickleCharge.Physics.Thrust;

using UnityEditor;

using UnityEngine;

namespace TrickleCharge.Drone.Debug
{
public class DroneDebug : MonoBehaviour
{
    [SerializeField]
    private QuadcopterController m_controller;

    [Space(10), Header("Velocity Display")]
    [SerializeField]
    private VelocityDebug m_velocityDebug;

    [Space(10), Header("Total Force Display")]

    [SerializeField]
    private ForceDebug m_forceDebug;

    private void OnDrawGizmos()
    {
        m_velocityDebug.DrawVelocity(m_controller.Rigidbody);
        m_forceDebug.DrawTotalForce(m_controller);
    }

    [Serializable]
    private class VelocityDebug
    {
        [SerializeField]
        private bool m_enabled = true;

        [SerializeField]
        private Color m_arrowColor = Color.green;

        public void DrawVelocity(Rigidbody rigidbody)
        {
            if(!m_enabled) { return; }
            if(rigidbody == null) { return; }
            if(rigidbody.linearVelocity.sqrMagnitude == 0) { return; }

            Handles.color = m_arrowColor;

            Handles.ArrowHandleCap(
                0,
                rigidbody.position,
                Quaternion.LookRotation(rigidbody.linearVelocity),
                rigidbody.linearVelocity.magnitude,
                EventType.Repaint
            );
        }
    }

    [Serializable]
    private class ForceDebug
    {
        [SerializeField]
        private bool m_enabled = true;

        [SerializeField]
        private Color m_arrowColor = Color.magenta;

        [SerializeField]
        private bool m_includeGravity = true;

        public void DrawTotalForce(QuadcopterController controller)
        {
            if(!m_enabled) { return; }
            if(controller == null) { return; }

            Vector3 totalForce = CalculateTotalThrusterForce(controller.Thrusters);
            totalForce = m_includeGravity ? totalForce + UnityEngine.Physics.gravity : totalForce;

            if(totalForce.sqrMagnitude == 0) { return; }

            Handles.color = m_arrowColor;

            Handles.ArrowHandleCap(
                0,
                controller.Rigidbody.position,
                Quaternion.LookRotation(totalForce),
                totalForce.magnitude,
                EventType.Repaint
            );
        }

        private static Vector3 CalculateTotalThrusterForce(IEnumerable<Thruster> thrusters)
            => thrusters.Aggregate(Vector3.zero, static (total, thruster) => total + thruster.ForceVector);
    }
}
}
