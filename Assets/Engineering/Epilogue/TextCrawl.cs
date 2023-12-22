using DG.Tweening;
using FMOD.Studio;
using ScriptableObjectDependencyInjection;
using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TextCrawl : MonoBehaviour
{
    [SerializeField] float crawlTime = 100f;
    [SerializeField] float timeBeforeReturn = 20.0f;
    [SerializeField] float timeBeforeFinalDialogue = 10.0f;
    [SerializeField] float waitTime = 1f;
    [SerializeField] RectTransform textRect;
    [SerializeField] RectTransform textRect2;
    [SerializeField] TextMeshProUGUI text;

    [SerializeField] string finalDialoguePath;

    [Title("Stats")]
    [SerializeField] FloatReference timeToCompletion;
    [SerializeField] IntegerReference hitsTaken;
    [SerializeField] IntegerReference deaths;


    [SerializeField] IntegerReference shotsFired;

    [SerializeField] IntegerReference robotsDestroyed;
    [SerializeField] IntegerReference sentriesDestroyed;

    [SerializeField] IntegerReference barrelsImploded;
    [SerializeField] IntegerReference ballsTeleported;

    void Start()
    {
        SetText();
        DOTween.Sequence()
            .AppendInterval(waitTime)
            .Append(textRect.DOMove(textRect2.position, crawlTime));
        //.OnComplete(() => Destroy(this.gameObject));


        DOTween.Sequence()
            .AppendInterval(timeBeforeReturn)
            .AppendCallback(() => SceneTransitionManager.Instance.TransitionScene("00LevelSelect"));

        if (finalDialoguePath != "") { 
            EventInstance finalDialogue = FMODUnity.RuntimeManager.CreateInstance(finalDialoguePath);
            finalDialogue.start();
            finalDialogue.release();
        }

    }


    void SetText() {
        text.text = "After what could only be described as a most lackluster encounter with the so called \"Big Bad Evil Guy\", our lone operative proceeded to obliterate the enemy base by imploding all the barrels on the base at the same time. " +
            "\n\nThis was a crucial victory for the \"rebel\" forces and marked the beginning of their rise to power." +
            "\n\nHowever, the story of our operative did not end there. As the dust settled and the echoes of the implosion faded, our operative received another communication from mission control." +
            "\n\nA familiar but chilling voice spoke, and a disturbing truth revealed. For our \"hero\" was deceived and had unwittingly paved the way for a greater evil to rise to power." +
            "\n\nAs our operative began to grasp the potential repercussions of what they had unintentionally set in motion, they began to question their loyalty to the \"rebel\" cause. " +
            "\n\n\nIt seems our hero's journey is far from over."
            
            
            +"\n\n\n\nYour Stats:" +
            "\nTime to Completion: " + (timeToCompletion.Value / 60).ToString("00") + ":" + (timeToCompletion.Value % 60).ToString("00") +
            "\nShots Fired: " + shotsFired.Value +
            "\nShots Taken: " + hitsTaken.Value +
            "\n\"Deaths\": " + deaths.Value +
            "\nRobots Destroyed: " + robotsDestroyed.Value +
            "\nSentries Destroyed: " + sentriesDestroyed.Value +
            "\nBarrels Imploded: " + barrelsImploded.Value +
            "\nBalls Teleported: " + ballsTeleported.Value +
            "";
    }

}
