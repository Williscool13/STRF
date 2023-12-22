using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicEventTrigger : MonoBehaviour, ICallbackMusicEvent
{
    [SerializeField] bool multipleTrigger = false;
    public event EventHandler OnMusicEvent;
    bool triggered = false;
    private void OnTriggerEnter(Collider other) {
        if (triggered && !multipleTrigger) return;

        if (other.gameObject.CompareTag("Player")) {
            OnMusicEvent?.Invoke(this, EventArgs.Empty);
            triggered = true;
        }
    }
}
