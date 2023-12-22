using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueEventTrigger : MonoBehaviour, ICallbackSoundEvent
{
    [SerializeField] bool multipleTrigger = false;
    public event EventHandler OnSoundEvent;
    bool triggered = false;
    private void OnTriggerEnter(Collider other) {
        if (triggered && !multipleTrigger) return;

        if (other.gameObject.CompareTag("Player")) {
            OnSoundEvent?.Invoke(this, EventArgs.Empty);
            triggered = true;
        }
    }
}