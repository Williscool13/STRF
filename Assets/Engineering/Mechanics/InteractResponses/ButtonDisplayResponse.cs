using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ButtonDisplayResponse : InteractableResponse
{
    [SerializeField] TextMeshPro display;
    public override void OnInteracted(bool b) { display.text = b ? "ON" : "OFF"; }

}
