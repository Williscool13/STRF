using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameObjectActivationZones : MonoBehaviour
{
    [SerializeField] GameObject[] objectsToActivate;
    [SerializeField] bool inverted = false;
    void Start()
    {
        for (int i = 0; i < objectsToActivate.Length; i++) {
            GameObject obj = objectsToActivate[i];
            obj.SetActive(inverted);
        }
        
    }

    private void OnTriggerEnter(Collider other) {
        if (other.gameObject.tag == "Player") {
            for (int i = 0; i < objectsToActivate.Length; i++) {
                GameObject obj = objectsToActivate[i];
                if (obj != null) {
                    obj.SetActive(!inverted);
                }
            }
        }
    }
}
