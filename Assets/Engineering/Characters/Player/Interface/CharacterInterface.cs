using Febucci.UI;
using ScriptableObjectDependencyInjection;
using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CharacterInterface : MonoBehaviour
{
    [SerializeField] private PlayerLoadoutManager loadoutManager;
    [SerializeField] private Canvas canvas;
    [SerializeField] private TypewriterByCharacter ammoCount;
    [SerializeField] private TypewriterByWord ammoCountReserve;


    [Title("Stats")]
    [SerializeField] private TypewriterByCharacter robotKills;
    [SerializeField] private TypewriterByCharacter sentryKills;
    [SerializeField] private TypewriterByCharacter playerTeleports;
    [SerializeField] private TypewriterByCharacter ballTeleports;
    [SerializeField] private TypewriterByCharacter playerHits;
    [SerializeField] private TypewriterByCharacter gameTime;
    [SerializeField] private GameObject robotKillParent;
    [SerializeField] private GameObject sentryKillParent;
    [SerializeField] private GameObject playerTeleportParent;
    [SerializeField] private GameObject ballTeleportParent;
    [SerializeField] private GameObject playerHitParent;
    [SerializeField] private GameObject gameTimeParent;

    [SerializeField] IntegerReference robotKillsRef;
    [SerializeField] IntegerReference sentryKillsRef;
    [SerializeField] IntegerReference playerTeleportsRef;
    [SerializeField] IntegerReference ballTeleportsRef;
    [SerializeField] IntegerReference playerHitsRef;
    [SerializeField] FloatReference gameTimeRef;

    int prevRobotKills = 0;
    int prevSentryKills = 0;
    int prevPlayerTeleports = 0;
    int prevBallTeleports = 0;
    int prevPlayerHits = 0;
    int prevGameTime = 0;

    bool prevRobotKillParent = false;
    bool prevSentryKillParent = false;
    bool prevPlayerTeleportParent = false;
    bool prevBallTeleportParent = false;
    bool prevPlayerHitParent = false;
    bool prevGameTimeParent = false;
    WeaponBase currWeapon = null;
    private void Start() {
        loadoutManager.OnWeaponChange += LoadoutManager_OnWeaponChange;
        canvas.worldCamera = Camera.main;
        robotKillParent.SetActive(false);
        sentryKillParent.SetActive(false);
        playerTeleportParent.SetActive(false);
        ballTeleportParent.SetActive(false);
        playerHitParent.SetActive(false);
        gameTimeParent.SetActive(false);
    }

    int prevAmmoCount = 0;
    int prevReserveAmmoCount = 0;
    private void Update() {

        SetAmmo();
        StatsUI();
    }

    void SetAmmo() {
        if (currWeapon == null) return;

        if (currWeapon.GetAmmo != prevAmmoCount) {
            ammoCount.ShowText(currWeapon.GetAmmo.ToString());
            prevAmmoCount = currWeapon.GetAmmo;
        }
        if (currWeapon.IsInfinite) {
            if (prevReserveAmmoCount != -1) {
                ammoCountReserve.ShowText("--");
                prevReserveAmmoCount = -1;
            }
        }
        else {
            if (currWeapon.GetReserveAmmo != prevReserveAmmoCount) {
                ammoCountReserve.ShowText(currWeapon.GetReserveAmmo.ToString());
                prevReserveAmmoCount = currWeapon.GetReserveAmmo;
            }
        }
    }

    void StatsUI() {
        SetStats(robotKills, robotKillParent, robotKillsRef.Value, ref prevRobotKills, ref prevRobotKillParent);
        SetStats(sentryKills, sentryKillParent, sentryKillsRef.Value, ref prevSentryKills, ref prevSentryKillParent);
        SetStats(playerTeleports, playerTeleportParent, playerTeleportsRef.Value, ref prevPlayerTeleports, ref prevPlayerTeleportParent);
        SetStats(ballTeleports, ballTeleportParent, ballTeleportsRef.Value, ref prevBallTeleports, ref prevBallTeleportParent);
        SetStats(playerHits, playerHitParent, playerHitsRef.Value, ref prevPlayerHits, ref prevPlayerHitParent);
        SetTime(gameTime, gameTimeParent, (int)gameTimeRef.Value, ref prevGameTime, ref prevGameTimeParent);

    }

    void SetStats(TypewriterByCharacter tw, GameObject parent, int value, ref int prevValue, ref bool enab) {
        if (value > 0) {
            if (!enab) {
                enab = true;
                parent.SetActive(true);
            }
        }
        if (value != prevValue) {
            prevValue = value;
            tw.ShowText(Mathf.Clamp(value, -99, 999).ToString());
        }
    }

    void SetTime(TypewriterByCharacter tw, GameObject parent, int value, ref int prevValue, ref bool enab) {
        if (value > 0) {
            if (!enab) {
                enab = true;
                parent.SetActive(true);
            }
        }
        if (value != prevValue) {
            prevValue = value;
            if (value < 60) {
                tw.ShowText(value.ToString());
            } else {
                int minutes = value / 60;
                int seconds = value % 60;
                tw.ShowText(minutes.ToString() + ":" + seconds.ToString("00"));
            }
        }
    }

    private void LoadoutManager_OnWeaponChange(object sender, WeaponBase e) {
        currWeapon = e;
    }
}
