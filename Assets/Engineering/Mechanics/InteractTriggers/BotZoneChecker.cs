using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

public class BotZoneChecker : MonoBehaviour, IInteractable
{
    [SerializeField] Transform cubeCenter;
    [SerializeField] float cubeLength = 1.0f;
    [SerializeField] LayerMask botLayer;

    [SerializeField] float pollRate = 1.0f;
    [ReadOnly]
    [SerializeField]
    int botsInZone = 0;

    public int BotsInZone => botsInZone;

    public event EventHandler<int> BotsInZoneChanged;
    public event EventHandler<bool> OnInteracted;
    Sequence pollSequence;
    void Start()
    {
        ZoneCount();

        pollSequence = DOTween.Sequence()
            .AppendInterval(pollRate)
            .AppendCallback(() => ZoneCount())
            .AppendCallback(() => ZoneCheck())
            .SetLoops(-1)
            .Play();
    }
    
    void ZoneCount() {
        float halfLength = cubeLength / 2f;
        Collider[] cols = Physics.OverlapBox(cubeCenter.position, Vector3.one * halfLength, Quaternion.identity, botLayer);
        botsInZone = cols.Length;
        BotsInZoneChanged?.Invoke(this, botsInZone);
    }

    void ZoneCheck() {
        if (botsInZone == 0) {
            Interact();
            pollSequence.Kill();
        }
    }

    [Button(ButtonSizes.Medium)]
    public void InteractDebug() {
        Interact();
        pollSequence.Kill();
    }

    public void Interact() {
        OnInteracted?.Invoke(this, true);
    }

    private void OnDrawGizmosSelected() {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireCube(cubeCenter.position, new Vector3(cubeLength, cubeLength, cubeLength));
    }
}
