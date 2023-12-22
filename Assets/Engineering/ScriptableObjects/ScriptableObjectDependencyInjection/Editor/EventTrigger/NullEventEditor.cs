using UnityEditor;
using UnityEngine;

namespace ScriptableObjectDependencyInjection
{
    [CustomEditor(typeof(NullEvent))]
    public class NullEventEditor : Editor
    {
        public override void OnInspectorGUI() {
            base.OnInspectorGUI();

            GUI.enabled = Application.isPlaying;

            NullEvent e = target as NullEvent;
            if (GUILayout.Button("Raise"))
                e.Raise(null);
        }
    }
}