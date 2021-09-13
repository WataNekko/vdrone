using UnityEngine;
using UnityEditor;

namespace Util
{
    [CustomPropertyDrawer(typeof(ReadOnlyInspectorAttribute))]
    public class ReadOnlyInspectorDrawer : PropertyDrawer
    {
        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            return EditorGUI.GetPropertyHeight(property, label, includeChildren: true);
        }

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            bool enabled = GUI.enabled;
            GUI.enabled = false;
            EditorGUI.PropertyField(position, property, label, includeChildren: true);
            GUI.enabled = enabled;
        }
    }
}
