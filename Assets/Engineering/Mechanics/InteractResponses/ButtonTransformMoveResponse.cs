using DG.Tweening;
using FMOD.Studio;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonTransformMoveResponse : InteractableResponse
{

    [SerializeField] Transform target;

    EventInstance sound;
    [SerializeField] string soundPath;
    public override void OnInteracted(bool b) {
        sound = FMODUnity.RuntimeManager.CreateInstance(soundPath);
        sound.set3DAttributes(FMODUnity.RuntimeUtils.To3DAttributes(gameObject));
        sound.start();
        // play looping sound
        DOTween.Sequence()
            .Append(transform.DOMove(target.position, 5.0f))
            .AppendCallback(() => {
                sound.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
                sound.release();
                // disable sound
            });
    }

}
