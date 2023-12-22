using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Combat
{

    public interface IHealthSystem
    {
        string Name { get; }
        int MaxHealth { get; }
        int CurrentHealth { get; }
        bool IsDead { get; }

        void Damage(int value);
        void Heal(int value);
        void Death();

    }
}