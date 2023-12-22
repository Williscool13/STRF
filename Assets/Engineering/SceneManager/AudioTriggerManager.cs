using DG.Tweening;
using FMOD.Studio;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using Unity.VisualScripting;

public class AudioTriggerManager : MonoBehaviour
{
    [SerializeField] List<SoundEvent> soundEvents;
    [SerializeField] List<MusicEvent> musicEvents;

    [SerializeField] MusicManagerVariableReference musicManagerVariableReference;
    [SerializeField] DialogueManagerVariableReference dialogueManagerVariableReference;
    void Start()
    {
        if (musicManagerVariableReference.Value == null) {
            Debug.LogError("Music manager variable reference is null! BIG ERROR");
            return;
        }

        for (int i = 0; i < soundEvents.Count; i++) {
            SoundEvent soundEvent = soundEvents[i];
            int index = i;
            if (soundEvent.callBackSoundEventMonoBehaviour.TryGetComponent(out ICallbackSoundEvent callbackSoundEvent)) {
                callbackSoundEvent.OnSoundEvent += (sender, e) => { PlaySound(sender, e, ref index); };

            }
        }

        foreach (MusicEvent musicEvent in musicEvents) {
            if (musicEvent.callBackMusicEventMonoBehaviour.TryGetComponent(out ICallbackMusicEvent callbackMusicEvent)) {
                callbackMusicEvent.OnMusicEvent += (sender, e) => { musicManagerVariableReference.Value.SetCurrentLevelProgress(musicEvent.LevelProgressValue); };
            }
        }
    }

    void PlaySound(object o, EventArgs e, ref int index) {
        if (soundEvents[index].playCount >= soundEvents[index].maxPlayCount) return;
        SoundEvent soundEvent = soundEvents[index];

        dialogueManagerVariableReference.Value.PlayDialogue(soundEvent.soundEventPath, soundEvent.overrideCurrentDialogue);
        soundEvent.playCount++;

        soundEvents[index] = soundEvent;
    }

    void PlaySoundClip(string path, bool overrideCurrentDialogue) {
        /*if (targetMusicManager.MusicDialogueActive && !overrideCurrentDialogue) {
            Debug.Log("A dialogue is already active");
            return;
        }*/

        EventInstance instance = FMODUnity.RuntimeManager.CreateInstance(path);
        instance.set3DAttributes(FMODUnity.RuntimeUtils.To3DAttributes(new Vector3(0, 0, 0)));
        instance.start();
        instance.getDescription(out EventDescription desc);
        desc.getLength(out int _t);
        instance.release();
        float t = _t / 1000.0f;
        //targetMusicManager.StartMusicDialogue(t);
        Debug.Log("Playing sound clip " + path + " for duration " + t);

    }

    public void PlayClipAtIndex(int index) {
        if (index >= soundEvents.Count) {
            Debug.LogError("Index out of range");
            return;
        }
        PlaySoundClip(soundEvents[index].soundEventPath, soundEvents[index].overrideCurrentDialogue);
    
    }
}

[Serializable]
public struct SoundEvent
{
    public MonoBehaviour callBackSoundEventMonoBehaviour;
    public string soundEventPath;
    public bool overrideCurrentDialogue;
    public int playCount;
    public int maxPlayCount;
}

[Serializable]
public struct MusicEvent
{
    public MonoBehaviour callBackMusicEventMonoBehaviour;
    public float LevelProgressValue;
}

public interface ICallbackSoundEvent
{
    event EventHandler OnSoundEvent;
}

public interface ICallbackMusicEvent
{
    event EventHandler OnMusicEvent;
}