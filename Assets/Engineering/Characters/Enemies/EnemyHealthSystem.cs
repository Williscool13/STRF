using Combat;
using ScriptableObjectDependencyInjection;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealthSystem : MonoBehaviour, ITarget, IHealthSystem, ICallbackSoundEvent
{
    [SerializeField] NullEvent OnDeathEvent;

    [SerializeField] HealthData healthData;
    [SerializeField] HealthSoundManager healthSoundManager;
    [SerializeField] Collider col;
    [SerializeField] bool reflective;
    [SerializeField] bool invulnerable;
    [SerializeField] bool disableColliderOnDeath = true;
    public string Name => unitName;
    public int MaxHealth => maxHealth;
    public int CurrentHealth => currentHealth;
    public bool IsDead => isDead;
    public bool Reflective => reflective;

    public event EventHandler<HitDataContainer> OnEnemyHit;
    public event EventHandler<HitDataContainer> OnEnemyDeath;
    public event EventHandler OnSoundEvent;

    string unitName;
    int maxHealth;
    int currentHealth;
    bool isDead;

    public TargetSurface Surface => TargetSurface.Metal;

    private void Start() {
        InitializeHealth();

    }


    public void Damage(int value) {
        if (invulnerable) return;
        currentHealth -= value;
        if (currentHealth <= 0) {
            currentHealth = 0;
            Death();
            return;
        } 
        
    }
    public void Death() {
        if (isDead) return;

        if (disableColliderOnDeath) col.enabled = false;

        healthSoundManager.DeathSound();
        if (OnDeathEvent != null) OnDeathEvent.Raise(null);
        OnEnemyDeath?.Invoke(this, null);
        OnSoundEvent?.Invoke(this, null);

        if (OnEnemyHit != null)
            foreach (var d in OnEnemyHit.GetInvocationList())
                OnEnemyHit -= (d as EventHandler<HitDataContainer>);
        if (OnEnemyDeath != null)
            foreach (var d in OnEnemyDeath.GetInvocationList())
                OnEnemyDeath -= (d as EventHandler<HitDataContainer>);
        if (OnSoundEvent != null)
            foreach (var d in OnSoundEvent.GetInvocationList())
                OnSoundEvent -= (d as EventHandler);

        isDead = true;
    }

    public void Heal(int value) {
    }

    public void Hit(HitDataContainer damage) {
        healthSoundManager.HitSound();

        OnEnemyHit?.Invoke(this, damage);

        this.Damage(damage.GetTotalDamage());
    }

    public void SetInvulnerable(bool v) {
        this.invulnerable = v;
        this.reflective = v;
    }

    void InitializeHealth() {
        this.maxHealth = healthData.maxHealth;
        this.currentHealth = healthData.maxHealth;
        this.unitName = healthData.unitName;
        if (isDead) {
            isDead = false;
            if (disableColliderOnDeath) {
                col.enabled = true;
            }
        }
    }
}
