using ScriptableObjectDependencyInjection;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "MusicManagerVariable", menuName = "ScriptableObjects/Variables/MusicManagerVariable")]
public class MusicManagerVariable : ScriptableVariable<IMusicManager> { }

[Serializable]
public class MusicManagerVariableReference : ScriptableReference<MusicManagerVariable, IMusicManager> { }
