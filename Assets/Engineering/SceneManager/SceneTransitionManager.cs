using DG.Tweening;
using FMODUnity;
using ScriptableObjectDependencyInjection;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class SceneTransitionManager : MonoBehaviour
{
    public static SceneTransitionManager Instance;
    [SerializeField] float fadeTime = 1f;

    [SerializeField] Material fadeMaterial;

    [SerializeField] NullEvent onSceneExitStart;
    [SerializeField] NullEvent onSceneExitEnd;
    [SerializeField] NullEvent onSceneEnterStart;
    [SerializeField] NullEvent onSceneEnterEnd;

    [SerializeField] StudioBankLoader bankLoader;
    private void Awake() {
        if (Instance == null) {
            Instance = this;
            DontDestroyOnLoad(this.gameObject);


            fadeMaterial.SetFloat("_Blackness", 1f);
            currentSequence = DOTween.Sequence()
                .AppendInterval(0.5f)
                .Append(DOTween.To(() => fadeMaterial.GetFloat("_Blackness"), x => fadeMaterial.SetFloat("_Blackness", x), 0f, fadeTime))
                .AppendCallback(() => onSceneEnterEnd.Raise(null))
                .AppendCallback(() => currentSequence = null);
            bankLoader.Load();
        }
        else {
            Destroy(this.gameObject);
        }
    }


    Sequence currentSequence;

    void SceneTransition(string sceneName) {
        onSceneExitStart.Raise(null);
        currentSequence = DOTween.Sequence()
            .Append(DOTween.To(() => fadeMaterial.GetFloat("_Blackness"), x => fadeMaterial.SetFloat("_Blackness", x), 1f, fadeTime))
            .AppendCallback(() => onSceneExitEnd.Raise(null))
            .AppendInterval(0.75f)
            .AppendCallback(() => SceneManager.LoadScene(sceneName))
            .AppendInterval(0.75f)
            .AppendCallback(() => onSceneEnterStart.Raise(null))
            .Append(DOTween.To(() => fadeMaterial.GetFloat("_Blackness"), x => fadeMaterial.SetFloat("_Blackness", x), 0f, fadeTime))
            .AppendCallback(() => onSceneEnterEnd.Raise(null))
            .AppendCallback(() => currentSequence = null);
    }
    public bool IsTransitioning() { return currentSequence != null && currentSequence.IsPlaying(); }

    public void TransitionScene(string sceneName) {
        if (IsTransitioning()) { Debug.LogError("Scene is midtrantision, you need to check this before you call this function"); return; }
        SceneTransition(sceneName);
    }

    public void OnLevelSelect(InputAction.CallbackContext context) {
        if (context.started) {
            TransitionScene("00LevelSelect");
        }
    }
}   
