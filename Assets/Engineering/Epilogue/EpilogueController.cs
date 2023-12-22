using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class EpilogueController : MonoBehaviour
{
    [SerializeField] PlayableDirector epilogueIntroDirector;
    [SerializeField] EnemyHealthSystem bossHealthSystem;

    [SerializeField] PlayableDirector bossKillDirector;

    bool outroTriggered = false;
    private void Start() {
        bossHealthSystem.OnEnemyDeath += OnBossDeath;
    }

    private void OnBossDeath(object sender, HitDataContainer e) {
        epilogueIntroDirector.Stop();
        bossKillDirector.Play();
    }

    private void OnTriggerEnter(Collider other) {
        if (outroTriggered) { return; }
        if (other.CompareTag("Player")) {
            epilogueIntroDirector.Play();
            outroTriggered = true;
        }
    }

    public void TransitionToCredits() {
        SceneTransitionManager.Instance.TransitionScene("C02Credits");
    }
}
