using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Combat
{
    public interface ITarget
    {
        public TargetSurface Surface { get; }
        public bool Reflective { get; }
        void Hit(HitDataContainer damage);

        event EventHandler<HitDataContainer> OnEnemyHit;
    }

    public enum TargetSurface {
        Metal,
    }


}