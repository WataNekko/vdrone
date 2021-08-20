using UnityEngine;
using UnityEditor;

namespace Robotics.EditorNS
{
    [CustomEditor(typeof(Servo), editorForChildClasses: true)]
    public class ServoEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            Servo servo = (Servo)target;
            var range = servo.PulseWidthRange;

            int pulseWidth = EditorGUILayout.IntSlider("Pulse Width (uS)", servo.readMicroseconds(), range.min, range.max);
            servo.writeMicroseconds(pulseWidth);
        }
    }
}
