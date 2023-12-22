using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

public class ShippingContainerController : MonoBehaviour
{
    [SerializeField] VideoPlayer videoPlayer;
    private void Start() {
        videoPlayer.url = System.IO.Path.Combine(Application.streamingAssetsPath, "ShippingContainerVideo.mp4");
        videoPlayer.Play();
    }
    public void StartGame() {
        SceneTransitionManager.Instance.TransitionScene("01FPSIntro");
    }
}
