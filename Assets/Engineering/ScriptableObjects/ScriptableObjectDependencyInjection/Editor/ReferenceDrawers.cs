using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;


namespace ScriptableObjectDependencyInjection
{
    [CustomPropertyDrawer(typeof(FloatReference), true)]
    public class FloatReferenceDrawer : ScriptableReferenceDrawer { }

    [CustomPropertyDrawer(typeof(StringReference), true)]
    public class StringReferenceDrawer : ScriptableReferenceDrawer { }

    [CustomPropertyDrawer(typeof(IntegerReference), true)]
    public class IntegerReferenceDrawer : ScriptableReferenceDrawer { }

    [CustomPropertyDrawer(typeof(GameObjectReference), true)]
    public class GameObjectReferenceDrawer : ScriptableReferenceDrawer { }
    [CustomPropertyDrawer(typeof(MusicManagerVariableReference), true)]
    public class MusicManagerVariableReferenceDrawer : ScriptableReferenceDrawer { }
    [CustomPropertyDrawer(typeof(DialogueManagerVariableReference), true)]
    public class DialogueManagerVariableReferenceDrawer : ScriptableReferenceDrawer { }
}