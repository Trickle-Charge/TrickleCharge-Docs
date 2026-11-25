using UnityEngine;

namespace TrickleCharge.Attributes
{
/// <summary>
/// Marks a serialized field as read-only in the Unity Inspector.
/// Has zero runtime cost — only affects the Editor PropertyDrawer.
/// </summary>
public class ReadOnlyAttribute : PropertyAttribute { }
}
