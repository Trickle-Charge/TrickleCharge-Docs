using UnityEditor;

using UnityEngine;

namespace TrickleCharge.Attributes.Editor
{
/// <summary>
/// Draws fields tagged with <see cref="ReadOnlyAttribute"/> as disabled (greyed-out) in the Inspector.
/// Editor-only — completely stripped from runtime builds.
/// </summary>
[CustomPropertyDrawer(typeof(ReadOnlyAttribute))]
public class ReadOnlyDrawer : PropertyDrawer
{
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        EditorGUI.BeginDisabledGroup(true);
        EditorGUI.PropertyField(position, property, label, true);
        EditorGUI.EndDisabledGroup();
    }

    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        return EditorGUI.GetPropertyHeight(property, label, true);
    }
}
}