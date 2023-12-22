using ScriptableObjectDependencyInjection;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameStatisticsManager : MonoBehaviour
{
    [SerializeField] IntegerVariable robotsKilled;
    [SerializeField] IntegerVariable sentriesKilled;

    [SerializeField] IntegerVariable playerTeleported;
    [SerializeField] IntegerVariable ballsTeleported;

    [SerializeField] IntegerVariable playerHit;
    [SerializeField] FloatVariable gameTime;

    [SerializeField] IntegerVariable shotsFired;
    [SerializeField] IntegerVariable barrelsImploded;

    public static GameStatisticsManager Instance { get; private set; }

    private void Awake() {
        if (Instance != null) {
            Destroy(this.gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(this.gameObject);
    }

    private void Start() {
        
        robotsKilled.Value = 0;
        sentriesKilled.Value = 0;
        playerTeleported.Value = 0;
        ballsTeleported.Value = 0;

        playerHit.Value = 0;
        gameTime.Value = 0f;

        shotsFired.Value = 0;
        barrelsImploded.Value = 0;

        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode) {
        if (scene.name == "00LevelSelect" || scene.name == "01FPSintro") {
            robotsKilled.Value = 0;
            sentriesKilled.Value = 0;
            playerTeleported.Value = 0;
            ballsTeleported.Value = 0;

            playerHit.Value = 0;
            gameTime.Value = 0f;
            shotsFired.Value = 0;
            barrelsImploded.Value = 0;
        }
    }
    private void Update() {
        gameTime.Value += Time.deltaTime;
    }

    public void OnPlayerHit(object nullObj) {
        playerHit.Value++;
    }

    public void OnRobotDeathEvent(object nullObj) {
        robotsKilled.Value++;
    }

    public void OnSentryDeathEvent(object nullObj) {
        sentriesKilled.Value++;
    }

    public void OnShotFiredEvent(object nullObj) {
        shotsFired.Value++;
    }
    public void OnBarrelImplodeEvent(object nullObj) {
        barrelsImploded.Value++;
    }

    public void OnTeleportEvent(GameObject gob) {
        if (gob == null) return;
        if (gob.CompareTag("Player")) {
            playerTeleported.Value++;
        } else {
            ballsTeleported.Value++;
        }
    }
}
