using Combat;
using ScriptableObjectDependencyInjection;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTarget : MonoBehaviour, ITarget, IHealthSystem, ICallbackSoundEvent
{
    [SerializeField] bool reflective;
    [SerializeField] HealthSoundManager healthSoundManager;

    [SerializeField] private NullEvent OnPlayerHit;
    public string Name => "Player";

    public int MaxHealth => throw new NotImplementedException();

    public int CurrentHealth => throw new NotImplementedException();

    public bool IsDead => throw new NotImplementedException();

    
    
    public event EventHandler<HitDataContainer> OnEnemyHit;
    public event EventHandler OnSoundEvent;

    public bool Reflective => reflective;
    public TargetSurface Surface => TargetSurface.Metal;

    public void Damage(int value) {
    }

    public void Death() {
    }


    public void Heal(int value) {
    }

    public void Hit(HitDataContainer damage) {
        healthSoundManager.HitSound();

        OnSoundEvent?.Invoke(this, null);
        OnPlayerHit.Raise(null);
    }

}
