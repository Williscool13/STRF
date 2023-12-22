using UnityEditor;
using UnityEngine;

namespace ScriptableObjectDependencyInjection
{
    [CustomEditor(typeof(StringEvent))]
    public class StringEventEditor : Editor
    {
        string variable;
        public override void OnInspectorGUI() {
            base.OnInspectorGUI();

            GUI.enabled = Application.isPlaying;

            StringEvent e = target as StringEvent;
            variable = EditorGUILayout.TextField("String Value", variable);
            if (GUILayout.Button("Raise")) {
                e.Raise(variable);

            }
        }
    }
}