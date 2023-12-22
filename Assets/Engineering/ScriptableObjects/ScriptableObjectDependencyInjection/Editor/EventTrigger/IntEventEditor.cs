using UnityEditor;
using UnityEngine;

namespace ScriptableObjectDependencyInjection
{
    [CustomEditor(typeof(IntEvent))]
    public class IntEventEditor : Editor
    {
        int variable;
        public override void OnInspectorGUI() {
            base.OnInspectorGUI();

            GUI.enabled = Application.isPlaying;

            IntEvent e = target as IntEvent;
            variable = EditorGUILayout.IntField("Int Value", variable);
            if (GUILayout.Button("Raise")) {
                e.Raise(variable);
            }
        }
    }
}