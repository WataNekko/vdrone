using UnityEngine;
using UnityEditor;
using Robotics.Servo;

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
