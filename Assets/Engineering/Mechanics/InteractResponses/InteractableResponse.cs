using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class InteractableResponse : MonoBehaviour
{
    [SceneObjectsOnly][SerializeField] GameObject targetIntarctable;

    IInteractable interactable;
    void Start()
    {
        interactable = targetIntarctable.GetComponent<IInteractable>();
        Debug.Assert(interactable != null);

        interactable.OnInteracted += OnInteractableInteracted;
    }


    void OnInteractableInteracted(object o, bool b) { OnInteracted(b); }

    public abstract void OnInteracted(bool b);
}
