using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class TraditionalGunBulletHole : BaseBulletHole
{
    [SerializeField] DecalProjector decalProjector;
    [SerializeField] float fadeTime = 2.0f;


    private void Start() {
        this.transform.hideFlags = HideFlags.HideInHierarchy;   
    }

    public override void Initialize(Collider collidedObject) {
        decalProjector.fadeFactor = 1.0f;
        DOTween.Sequence()
            .Append(DOTween.To(() => decalProjector.fadeFactor, x => decalProjector.fadeFactor = x, 0f, fadeTime))
            .AppendCallback(() => Release());
    }
}
