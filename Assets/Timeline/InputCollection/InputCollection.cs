using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class InputCollection : MonoBehaviour
{

    [SerializeField] Image fmod;
    [SerializeField] RectTransform clickToPlay;


    private void Start() {
        clickToPlay.localScale = new Vector3(0, 0, 0);
    }

    public void ShowFmod() {
        DOTween.Sequence()
            .AppendCallback(() => fmod.DOColor(new Color(1, 1, 1, 1), 1.0f));
    }

    public void HideFmodAndShowClickToPlay() {
        scaleSequence = DOTween.Sequence()
            .AppendCallback(() => fmod.DOColor(new Color(1, 1, 1, 0), 1.0f))
            .AppendInterval(1.0f)
            .AppendCallback(() => {
                TweenScaleTo(new Vector3(1, 1, 1), 1.0f);
                canClickToPlay = true;
                })
            .AppendInterval(1.0f)
            .AppendCallback(() => StartClickToPlayBobbing());
    }

    bool canClickToPlay = false;
    Sequence scaleSequence;
    void StartClickToPlayBobbing() {
        scaleSequence = DOTween.Sequence()
            .AppendCallback(() => TweenScaleTo(new Vector3(1.1f, 1.1f, 1.1f), 1.0f))
            .AppendInterval(1.0f)
            .AppendCallback(() => TweenScaleTo(new Vector3(1.0f, 1.0f, 1.0f), 1.0f))
            .AppendInterval(1.0f)
            .SetLoops(-1)
            .Play();
    }

    Tween _scaleTween;
    void TweenScaleTo(Vector3 target, float time) {
        if (_scaleTween.IsActive()) { _scaleTween.Kill(); }

        _scaleTween = clickToPlay.DOScale(target, time);
    }

    public void OnShoot(InputAction.CallbackContext context) {
        if (!context.started) { return; }
        if (canClickToPlay) {
            scaleSequence.Kill();
            float scale = clickToPlay.localScale.x;
            scale = Mathf.Max(scale - 0.15f, 0.0f);
            TweenScaleTo(Vector3.one * scale, 0.2f);
            SceneTransitionManager.Instance.TransitionScene("C00Intro");
        }
    }
}


