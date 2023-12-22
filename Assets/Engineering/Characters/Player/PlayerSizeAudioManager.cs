using FMOD.Studio;
using ScriptableObjectDependencyInjection;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSizeAudioManager : MonoBehaviour
{
    //[SerializeField] FloatVariable playerScale;

    public FloatVariable playerScale;
    public string fmodParam = "PlayerSize";
    public FMODUnity.StudioEventEmitter fmodEventEmitter; 
    
    
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

