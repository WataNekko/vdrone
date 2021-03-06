using UnityEditor;
using UnityEngine;
using Util;

namespace EditorNS
{
    [CustomPropertyDrawer(typeof(RangedInt))]
    public class RangedIntDrawer : PropertyDrawer
    {
        private SerializedProperty _valueProp;
        private SerializedProperty _minProp;
        private SerializedProperty _maxProp;

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            float height = EditorGUIUtility.singleLineHeight;

            if (property.isExpanded)
            {
                height = height * 2 + EditorGUIUtility.standardVerticalSpacing;
            }

            return height;
        }

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            // Cache the serialized fields if haven't
            if (_valueProp == null)
            {
                _valueProp = property.FindPropertyRelative("_value");
                _minProp = property.FindPropertyRelative("_min");
                _maxProp = property.FindPropertyRelative("_max");
            }

            // Set the rect height as a single line if is expanded to layout each line properly
            if (property.isExpanded)
            {
                position.height = EditorGUIUtility.singleLineHeight;
            }

            // Store min and max for later use
            int min = _minProp.intValue;
            int max = _maxProp.intValue;

            // Value slider and foldout arrow
            EditorGUI.IntSlider(position, _valueProp, min, max, label);
            property.isExpanded = EditorGUI.Foldout(position, property.isExpanded, "");

            // Draw min and max fields if is expanded
            if (property.isExpanded)
            {
                // Set up layout
                EditorGUI.indentLevel++;
                EditorGUIUtility.labelWidth = 50;
                Rect minMaxPos = new Rect(
                    position.x,
                    position.y + position.height + EditorGUIUtility.standardVerticalSpacing,
                    102,
                    position.height);

                // Draw min max int fields
                min = EditorGUI.IntField(minMaxPos, _minProp.displayName, min);
                minMaxPos.x += position.width / 2;
                max = EditorGUI.IntField(minMaxPos, _maxProp.displayName, max);

                // Update if valid
                if (min <= max)
                {
                    _minProp.intValue = min;
                    _maxProp.intValue = max;
                    // Ensure value stays within new range
                    _valueProp.intValue = Mathf.Clamp(_valueProp.intValue, min, max);
                }

                EditorGUI.indentLevel--;
            }
        }
    }
}
