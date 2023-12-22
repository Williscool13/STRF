using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneTransitionCollider : MonoBehaviour
{
    [SerializeField] string sceneName;
    private void OnTriggerEnter(Collider other) {
        if (other.CompareTag("Player"))
        {
            if (!SceneTransitionManager.Instance.IsTransitioning()) {
                SceneTransitionManager.Instance.TransitionScene(sceneName);
            }
        }
    }
}
