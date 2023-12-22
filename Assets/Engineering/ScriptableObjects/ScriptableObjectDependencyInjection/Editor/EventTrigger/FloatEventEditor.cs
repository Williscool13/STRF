using UnityEditor;
using UnityEngine;

namespace ScriptableObjectDependencyInjection
{
    [CustomEditor(typeof(FloatEvent))]
    public class FloatEventEditor : Editor
    {
        float variable;
        public override void OnInspectorGUI() {
            base.OnInspectorGUI();

            GUI.enabled = Application.isPlaying;

            FloatEvent e = target as FloatEvent;
            variable = EditorGUILayout.FloatField("Float Value", variable);
            if (GUILayout.Button("Raise"))
                e.Raise(variable);
        }
    }
}