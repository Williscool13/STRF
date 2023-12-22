using UnityEditor;
using UnityEngine;

namespace ScriptableObjectDependencyInjection
{
    [CustomEditor(typeof(GameObjectEvent))]
    public class GameObjectEventEditor : Editor
    {
        GameObject variable;
        public override void OnInspectorGUI() {
            base.OnInspectorGUI();

            GUI.enabled = Application.isPlaying;

            GameObjectEvent e = target as GameObjectEvent;
            variable = EditorGUILayout.ObjectField("Damage Sender", variable, typeof(GameObject), true) as GameObject;
            if (GUILayout.Button("Raise")) {
                if (variable == null) {
                    Debug.LogError("No GameObject selected to raise event with.");
                    return;
                }
                if (EditorUtility.IsPersistent(variable)) {
                    Debug.LogError("Selected object is not in scene");
                    return;
                }
                e.Raise(variable);

            }
        }
    }
}