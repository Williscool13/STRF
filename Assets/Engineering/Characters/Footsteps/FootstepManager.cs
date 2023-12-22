using FMOD.Studio;
using ScriptableObjectDependencyInjection;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FootstepManager : MonoBehaviour
{
    [SerializeField] FootstepSwapper footstepSwapper;

    string currSurfaceType = "Metal";

    [SerializeField] private string playerWalkFootstepEventPath = "event:/SFX/Player/Footsteps/Play_Player_Footstep_Walk";
    [SerializeField] private string playerRunFootstepEventPath = "event:/SFX/Player/Footsteps/Play_Player_Footstep_Run";
    [SerializeField] private string playerJumpFootstepEventPath = "event:/SFX/Player/Footsteps/Play_Player_Footstep_Jump";
    [SerializeField] private string playerLandFootstepEventPath = "event:/SFX/Player/Footsteps/Play_Player_Footstep_Land";
    [SerializeField] private FloatReference playerScale;
    public void SetFootstepId(string surfaceType) {
        this.currSurfaceType = surfaceType;
    }

    public void PlayWalkSound() {
        footstepSwapper.CheckLayers();
        PlaySound(playerWalkFootstepEventPath);        
    }

    public void PlayRunSound() {
        footstepSwapper.CheckLayers();
        PlaySound(playerRunFootstepEventPath);
    }

    public void PlayJumpSound() {
        footstepSwapper.CheckLayers();
        PlaySound(playerJumpFootstepEventPath);
    }

    public void PlayLandSound() {
        footstepSwapper.CheckLayers();
        PlaySound(playerLandFootstepEventPath);
    }

    public void PlaySound(string path) {
        EventInstance soundEvent = FMODUnity.RuntimeManager.CreateInstance(path);
        soundEvent.setParameterByNameWithLabel("SurfaceType", currSurfaceType);
        soundEvent.set3DAttributes(FMODUnity.RuntimeUtils.To3DAttributes(gameObject));
        soundEvent.setVolume(playerScale.Value);
        soundEvent.start();
        soundEvent.release();
    }
}
