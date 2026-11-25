using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using TrickleCharge.Math.Waves;
using UnityEditor;
using UnityEngine;

namespace TrickleCharge.Math.Editor.Waves
{
    public static class WavePreviewRenderer
    {
        public const float GraphHeight = 75f;

        private const int _sampleCount = 100;
        private const float _sampleRangeX = 10f;

        private static readonly Dictionary<string, float> s_scrubTimes = new();

        // Reusable transient curve instance to stop runtime heap allocations on repaint loops
        private static readonly AnimationCurve s_transientCurve = new();
        private static readonly List<Keyframe> s_keyframeBuffer = new();

        public static void Draw(Rect position, SerializedProperty property)
        {
            IWave wave = GetWaveInstance(property);
            if (wave == null) { return; }

            string pathKey = property.propertyPath;
            float currentTime = s_scrubTimes.GetValueOrDefault(pathKey, 0f);

            float sliderHeight = EditorGUIUtility.singleLineHeight;
            Rect sliderRect = new(position.x, position.y, position.width, sliderHeight);
            Rect curveRect = new(position.x, position.y + sliderHeight + EditorGUIUtility.standardVerticalSpacing, position.width, 55f);

            Rect indentedSliderRect = EditorGUI.IndentedRect(sliderRect);
            s_scrubTimes[pathKey] = EditorGUI.Slider(indentedSliderRect, "Scrub Time", currentTime, 0f, 20f);

            // 1. Populate the local reusable buffer via our new public extraction layout
            s_keyframeBuffer.Clear();
            WaveUtility.GenerateWaveKeyframesNonAlloc(wave, s_scrubTimes[pathKey], _sampleCount, _sampleRangeX, s_keyframeBuffer);

            // 2. Assign the keys to our persistent transient curve representation
            s_transientCurve.keys = s_keyframeBuffer.ToArray();

            EditorGUI.BeginDisabledGroup(true);
            Rect indentedCurveRect = EditorGUI.IndentedRect(curveRect);
            EditorGUI.CurveField(indentedCurveRect, "Shape Preview", s_transientCurve);
            EditorGUI.EndDisabledGroup();
        }

        private static IWave GetWaveInstance(SerializedProperty property)
        {
            if (property.propertyType == SerializedPropertyType.ManagedReference)
            {
                return property.managedReferenceValue as IWave;
            }

            object targetObject = property.serializedObject.targetObject;
            if (targetObject == null) { return null; }

            return GetFieldValueFromPath(targetObject, property.propertyPath) as IWave;
        }

        private static object GetFieldValueFromPath(object obj, string path)
        {
            string[] fields = path.Replace(".Array.data[", "[").Split('.');
            object currentObj = obj;

            foreach (string field in fields)
            {
                if (currentObj == null) { return null; }

                if (field.Contains("["))
                {
                    string fieldName = field.Substring(0, field.IndexOf('['));
                    int index = int.Parse(field.Substring(field.IndexOf('[') + 1).Replace("]", ""));

                    currentObj = GetFieldValue(currentObj, fieldName);
                    currentObj = GetElementAt(currentObj, index);
                    continue;
                }

                currentObj = GetFieldValue(currentObj, field);
            }

            return currentObj;
        }

        private static object GetFieldValue(object obj, string fieldName)
        {
            Type type = obj.GetType();
            FieldInfo field = type.GetField(fieldName, BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance);

            while (field == null && type.BaseType != null)
            {
                type = type.BaseType;
                field = type.GetField(fieldName, BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance);
            }

            return field?.GetValue(obj);
        }

        private static object GetElementAt(object obj, int index)
        {
            if (obj is IList list && index >= 0 && index < list.Count)
            {
                return list[index];
            }
            return null;
        }
    }
}