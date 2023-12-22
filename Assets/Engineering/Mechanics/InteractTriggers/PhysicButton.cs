using DG.Tweening;
using FMOD.Studio;
using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhysicButton : MonoBehaviour, IInteractable, ICallbackMusicEvent
{


    bool pressed = false;
    [SerializeField] float pressDistance = 1f;
    [SerializeField] Vector3 pressDirection = Vector3.down;
    public event EventHandler<bool> OnInteracted;
    public event EventHandler OnMusicEvent;

    [SerializeField] string soundPath;
    [SerializeField] int buttonSFXSize;

    private void OnCollisionEnter(Collision collision) {
        if (pressed) return;
        if (!collision.gameObject.CompareTag("PhysicsInteractableTrigger")) return;

        PressButton();
    }

    [Button("Press Button")]
    void PressButton() {
        pressed = true;
        transform.DOMove(transform.position + pressDirection * pressDistance, 1f)
            .OnComplete(() => Interact());
        //transform.DOMoveY(transform.position.y - pressDistance, 1f);
        Debug.Log("Button activated");

        OnMusicEvent?.Invoke(this, EventArgs.Empty);

        EventInstance sound = FMODUnity.RuntimeManager.CreateInstance(soundPath);
        sound.setParameterByName("ObjectSize", buttonSFXSize);
        sound.set3DAttributes(FMODUnity.RuntimeUtils.To3DAttributes(gameObject));
        sound.start();
        sound.release();
    }

    void Interact() {
        Debug.Log("Button event triggered");
        OnInteracted?.Invoke(this, pressed);
    }


}


public interface  IInteractable
{
    public event EventHandler<bool> OnInteracted;
}