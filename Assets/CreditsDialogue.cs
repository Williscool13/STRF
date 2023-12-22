using FMOD.Studio;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreditsDialogue : MonoBehaviour
{
    [SerializeField] string path;
    public void PlayDialogue() {

        EventInstance instance = FMODUnity.RuntimeManager.CreateInstance(path);
        instance.set3DAttributes(FMODUnity.RuntimeUtils.To3DAttributes(new Vector3(0, 0, 0)));
        instance.start();
        instance.setVolume(0.4f);
        instance.release();
    }
}
