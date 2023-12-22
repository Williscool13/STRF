using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RecoilData
{
    public Vector2 RecoilKick { get; set; }
    public float duration = 0.1f;
    public float totalDuration { get; private set; }
    public RecoilData(Vector2 recoilKick, float duration) {
        this.RecoilKick = recoilKick;

        this.duration = duration;
        this.totalDuration = duration;
    }
}
