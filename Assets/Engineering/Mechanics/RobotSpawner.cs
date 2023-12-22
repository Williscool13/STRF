using Combat;
using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RobotSpawner : MonoBehaviour, ICallbackSoundEvent
{
    [SerializeField] int maxBots = 10;
    [SerializeField] float spawnRatePerMinute = 10f;
    [SerializeField] GameObject botPrefab;

    [SerializeField] Transform spawnPosition;
    List<GameObject> bots;

    Sequence pollSequence;

    public event EventHandler OnBotSpawned;
    public event EventHandler OnBotDestroyed;
    public event EventHandler OnSoundEvent;

    void Start() {
        bots = new List<GameObject>();
        pollSequence = DOTween.Sequence()
            .AppendInterval(60.0f / spawnRatePerMinute)
            .AppendCallback(() => PollCurrentBots())
            .SetLoops(-1)
            .Play();
    }

    void PollCurrentBots() {
        for (int i = bots.Count - 1; i >= 0; i--) if (bots[i] == null) bots.RemoveAt(i);
        if (bots.Count < maxBots) SpawnBot();
    }

    void SpawnBot() { 
        if (!this.gameObject.activeSelf) return;
        GameObject bot = Instantiate(botPrefab, spawnPosition.position, spawnPosition.rotation);
        bots.Add(bot); 
        OnBotSpawned?.Invoke(this, null);

        if(bot.TryGetComponent(out EnemyHealthSystem healthSystem)) {
            healthSystem.OnEnemyDeath += (sender, e) => {
                OnBotDestroyed?.Invoke(this, null);
                OnSoundEvent?.Invoke(this, null);
            };
        }
    }
}
