using System;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.InteropServices;
using Sirenix.OdinInspector;
using FMOD.Studio;

public class DialogueManager : MonoBehaviour, IDialogueManager
{
    EVENT_CALLBACK dialogueCallback;

    public FMODUnity.EventReference EventName;

#if UNITY_EDITOR
    void Reset() {
        EventName = FMODUnity.EventReference.Find("event:/Character/Radio/Command");
    }
#endif
    [SerializeField] private DialogueManagerVariable mainDialogueManager;

    public static DialogueManager Instance { get; private set; }
    private void Awake() {
        if (Instance != null) {
            Destroy(this.gameObject);
            return;
        }
        
        Instance = this;
        mainDialogueManager.Value = this;
        DontDestroyOnLoad(this.gameObject);
    }

    void Start() {
        // Explicitly create the delegate object and assign it to a member so it doesn't get freed
        // by the garbage collected while it's being used
        dialogueCallback = new EVENT_CALLBACK(DialogueEventCallback);
    }

    EventInstance currentDialogue;
    public void PlayDialogue(string key, bool over) {
        Debug.Log("Play dialogue called " + key);
        if (currentDialogue.isValid()) {
            if (over) {
                currentDialogue.stop(STOP_MODE.ALLOWFADEOUT);
            } 
            currentDialogue.release();
        }

        currentDialogue = FMODUnity.RuntimeManager.CreateInstance(EventName);

        // Pin the key string in memory and pass a pointer through the user data
        GCHandle stringHandle = GCHandle.Alloc(key);
        currentDialogue.setUserData(GCHandle.ToIntPtr(stringHandle));

        currentDialogue.setCallback(dialogueCallback);
        currRef = GameObject.FindGameObjectWithTag("GlorbonSource");
        currentDialogue.set3DAttributes(FMODUnity.RuntimeUtils.To3DAttributes(currRef));
        currentDialogue.start();
    }
    GameObject currRef;
    private void Update() {
        if (currentDialogue.isValid() && currRef != null) {
            currentDialogue.set3DAttributes(FMODUnity.RuntimeUtils.To3DAttributes(currRef));
        }
    }


    [AOT.MonoPInvokeCallback(typeof(EVENT_CALLBACK))]
    static FMOD.RESULT DialogueEventCallback(EVENT_CALLBACK_TYPE type, IntPtr instancePtr, IntPtr parameterPtr) {
        EventInstance instance = new EventInstance(instancePtr);

        // Retrieve the user data
        instance.getUserData(out IntPtr stringPtr);

        // Get the string object
        GCHandle stringHandle = GCHandle.FromIntPtr(stringPtr);
        string key = stringHandle.Target as string;

        switch (type) {
            case EVENT_CALLBACK_TYPE.CREATE_PROGRAMMER_SOUND: {
                    FMOD.MODE soundMode = FMOD.MODE.LOOP_NORMAL | FMOD.MODE.CREATECOMPRESSEDSAMPLE; //| FMOD.MODE.NONBLOCKING;
                    var parameter = (PROGRAMMER_SOUND_PROPERTIES)Marshal.PtrToStructure(parameterPtr, typeof(PROGRAMMER_SOUND_PROPERTIES));

                    if (key.Contains(".")) {
                        FMOD.Sound dialogueSound;
                        var soundResult = FMODUnity.RuntimeManager.CoreSystem.createSound(Application.streamingAssetsPath + "/" + key, soundMode, out dialogueSound);
                        if (soundResult == FMOD.RESULT.OK) {
                            parameter.sound = dialogueSound.handle;
                            parameter.subsoundIndex = -1;
                            Marshal.StructureToPtr(parameter, parameterPtr, false);
                        }
                    }
                    else {
                        SOUND_INFO dialogueSoundInfo;
                        var keyResult = FMODUnity.RuntimeManager.StudioSystem.getSoundInfo(key, out dialogueSoundInfo);
                        if (keyResult != FMOD.RESULT.OK) {
                            break;
                        }
                        FMOD.Sound dialogueSound;
                        var soundResult = FMODUnity.RuntimeManager.CoreSystem.createSound(dialogueSoundInfo.name_or_data, soundMode | dialogueSoundInfo.mode, ref dialogueSoundInfo.exinfo, out dialogueSound);
                        if (soundResult == FMOD.RESULT.OK) {
                            parameter.sound = dialogueSound.handle;
                            parameter.subsoundIndex = dialogueSoundInfo.subsoundindex;
                            Marshal.StructureToPtr(parameter, parameterPtr, false);
                        }
                    }
                    break;
                }
            case EVENT_CALLBACK_TYPE.DESTROY_PROGRAMMER_SOUND: {
                    var parameter = (PROGRAMMER_SOUND_PROPERTIES)Marshal.PtrToStructure(parameterPtr, typeof(PROGRAMMER_SOUND_PROPERTIES));
                    var sound = new FMOD.Sound(parameter.sound);
                    sound.release();

                    break;
                }
            case EVENT_CALLBACK_TYPE.DESTROYED: {
                    // Now the event has been destroyed, unpin the string memory so it can be garbage collected
                    stringHandle.Free();

                    break;
                }
        }
        return FMOD.RESULT.OK;
    }
}

public interface IDialogueManager
{
    void PlayDialogue(string key, bool over);
}