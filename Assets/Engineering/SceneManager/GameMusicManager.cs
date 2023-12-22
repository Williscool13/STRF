using DG.Tweening;
using FMOD.Studio;
using FMODUnity;
using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameMusicManager : MonoBehaviour, IMusicManager
{

    [SerializeField][ReadOnly] float currentMusicVolume = 1f;
    [SerializeField][ReadOnly] int currentSceneId = 0;
    [SerializeField][ReadOnly] float currentLevelProgress = 0f;
    EventInstance musicInstance;
    [SerializeField] string musicEventPath = "event:/Music/Play_2D_Music_InGame";
    [SerializeField] MusicManagerVariable mainMusicManager;

    [SerializeField] StudioBankLoader bankLoader;

    public static GameMusicManager Instance { get; private set; }
    private void Awake() {
        if (Instance != null) {
            Destroy(this.gameObject);
            return;
        }
        
        Instance = this;
        mainMusicManager.Value = this;
        DontDestroyOnLoad(this.gameObject);
    }
    void Start()
    {
        musicInstance = RuntimeManager.CreateInstance(musicEventPath);
        currentSceneId = GetSceneId();
        musicInstance.setParameterByName("CurrentLevel", currentSceneId);
        musicInstance.setParameterByName("LevelProgress", 0);
        musicInstance.start();
        musicInstance.setVolume(currentMusicVolume);

        bankLoader.Load();
    }

    public void OnSceneExitStart() {
        DOTween.To(() => currentMusicVolume, x => currentMusicVolume = x, 0f, 1f).OnUpdate(() => musicInstance.setVolume(currentMusicVolume));
    }

    public void OnSceneEnterStart() {
        int id = GetSceneId();
        currentSceneId = id;
        musicInstance.setParameterByName("CurrentLevel", currentSceneId);
        musicInstance.setParameterByName("LevelProgress", 0);
        DOTween.To(() => currentMusicVolume, x => currentMusicVolume = x, 1f, 1f).OnUpdate(() => musicInstance.setVolume(currentMusicVolume));
        Debug.Log("scene started, setting music level to " + id + " and level progress to 0");
        musicInstance.start();
    }

    public void SetCurrentLevelProgress(float progress) {
        currentLevelProgress = progress;
        musicInstance.setParameterByName("LevelProgress", currentLevelProgress);
    }

    int GetSceneId() {
        return SceneManager.GetActiveScene().name switch {
            "00LevelSelect" => 0,
            "01FPSintro" => 1,
            "02Barrels" => 2,
            "03Grow" => 3,
            "04Shrink" => 4,
            "C00Intro" => 5,
            "C01Epilogue" => 6,
            "C02Credits" => 7,
            "C03InputCollectionScene" => 8,
            _ => -1,
        };
    }

}

public interface IMusicManager
{
    void SetCurrentLevelProgress(float progress);
    void OnSceneExitStart();
    void OnSceneEnterStart();
}