using ScriptableObjectDependencyInjection;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "DialogueManagerVariable", menuName = "ScriptableObjects/Variables/DialogueManagerVariable")]
public class DialogueManagerVariable : ScriptableVariable<IDialogueManager> { }

[System.Serializable]
public class DialogueManagerVariableReference : ScriptableReference<DialogueManagerVariable, IDialogueManager> { }

