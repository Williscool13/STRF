using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class BotCountDisplay : MonoBehaviour
{
    [SerializeField] BotZoneChecker botZoneChecker;
    [SerializeField] TextMeshPro text;
    private void Start() {
        botZoneChecker.BotsInZoneChanged += (sender, count) => {
            text.text = count.ToString() + " Bots\nRemaining";
        };
    }
}
