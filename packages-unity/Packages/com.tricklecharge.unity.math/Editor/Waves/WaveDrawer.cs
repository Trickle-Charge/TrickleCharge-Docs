using System;
using System.Collections.Generic;
using System.Linq;

using TrickleCharge.Math.Waves;

using UnityEditor;

using UnityEngine;

namespace TrickleCharge.Math.Editor.Waves
{
    [CustomPropertyDrawer(typeof(IWave), true)]
    public class WaveDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            Rect headerRect = new(position.x, position.y, position.width, EditorGUIUtility.singleLineHeight);

            // 1. Setup layout header based on field type
            DrawHeader(headerRect, property, label);

            // 2. Guard against unassigned managed object references
            if (property.propertyType == SerializedPropertyType.ManagedReference && string.IsNullOrEmpty(property.managedReferenceFullTypename)) { return; }

            // 3. Foldout visibility guard
            if (!property.isExpanded) { return; }

            // 4. Render concrete internal properties
            DrawChildProperties(position, property);

            // 5. Compute layout space offset to place the decoupled visualizer field
            float childrenHeight = GetChildrenTotalHeight(property);
            float previewStartY = position.y + EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing + childrenHeight;

            Rect previewRect = new(position.x, previewStartY, position.width, WavePreviewRenderer.GraphHeight);
            WavePreviewRenderer.Draw(previewRect, property);
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            float baseHeight = EditorGUIUtility.singleLineHeight;

            if (property.propertyType == SerializedPropertyType.ManagedReference && string.IsNullOrEmpty(property.managedReferenceFullTypename)) { return baseHeight; }
            if (!property.isExpanded) { return baseHeight; }

            return baseHeight + GetChildrenTotalHeight(property) + WavePreviewRenderer.GraphHeight + EditorGUIUtility.standardVerticalSpacing;
        }

        private static void DrawHeader(Rect rect, SerializedProperty property, GUIContent label)
        {
            if (property.propertyType == SerializedPropertyType.ManagedReference)
            {
                DrawTypeDropdown(rect, property, label);
                property.isExpanded = EditorGUI.Foldout(rect, property.isExpanded, GUIContent.none, true);
                return;
            }

            // Concrete fields (like root WaveCollection) get a standard structural foldout header toggle
            property.isExpanded = EditorGUI.Foldout(rect, property.isExpanded, label, true);
        }

        private static void DrawTypeDropdown(Rect rect, SerializedProperty property, GUIContent label)
        {
            Rect dropdownRect = EditorGUI.PrefixLabel(rect, label);
            string typeName = GetDisplayTypeName(property.managedReferenceFullTypename);

            if (!EditorGUI.DropdownButton(dropdownRect, new GUIContent(typeName), FocusType.Passive)) { return; }

            GenericMenu menu = new();
            menu.AddItem(new GUIContent("Null"), string.IsNullOrEmpty(property.managedReferenceFullTypename), () => AssignNewType(property, null));

            IEnumerable<Type> concreteTypes = TypeCache.GetTypesDerivedFrom<IWave>().Where(static t => !t.IsAbstract && !t.IsInterface);
            foreach (Type type in concreteTypes)
            {
                bool isCurrent = property.managedReferenceFullTypename.EndsWith(type.FullName);
                menu.AddItem(new GUIContent(type.Name), isCurrent, () => AssignNewType(property, type));
            }

            menu.ShowAsContext();
        }

        private static void DrawChildProperties(Rect position, SerializedProperty property)
        {
            SerializedProperty child = property.Copy();
            int rootDepth = child.depth;

            float yOffset = EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;
            bool enterChildren = true;

            EditorGUI.indentLevel++;

            while (child.NextVisible(enterChildren))
            {
                if (child.depth <= rootDepth) { break; }
                enterChildren = false;

                float height = EditorGUI.GetPropertyHeight(child, true);
                Rect childRect = new(position.x, position.y + yOffset, position.width, height);

                EditorGUI.PropertyField(childRect, child, true);
                yOffset += height + EditorGUIUtility.standardVerticalSpacing;
            }

            EditorGUI.indentLevel--;
        }

        private static float GetChildrenTotalHeight(SerializedProperty property)
        {
            SerializedProperty child = property.Copy();
            int rootDepth = child.depth;

            float totalHeight = 0f;
            bool enterChildren = true;

            while (child.NextVisible(enterChildren))
            {
                if (child.depth <= rootDepth) { break; }
                enterChildren = false;

                totalHeight += EditorGUI.GetPropertyHeight(child, true) + EditorGUIUtility.standardVerticalSpacing;
            }

            return totalHeight;
        }

        private static string GetDisplayTypeName(string fullTypeName) => string.IsNullOrEmpty(fullTypeName)
            ? "Select Wave type"
            : fullTypeName.Split(' ').Last().Split('.').Last();

        private static void AssignNewType(SerializedProperty property, Type type)
        {
            property.managedReferenceValue = type != null ? Activator.CreateInstance(type) : null;
            property.serializedObject.ApplyModifiedProperties();
        }
    }
}