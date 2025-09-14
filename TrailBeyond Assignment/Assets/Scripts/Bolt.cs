using System.Security.Cryptography;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit.Interactables;
using UnityEngine.XR.Interaction.Toolkit.Interactors;

public class Bolt : MonoBehaviour
{
    [SerializeField] private int SequenceOrderId;
    [SerializeField] private GameObject AnimatedNut;
    [SerializeField] private XRSocketInteractor NutSocket;
    [SerializeField] private ParticleSystem successParticle;
    private Animator nutAnimator;
    private AnimationEmitter nutAnimEmitter;

    private void OnEnable()
    {
        GameEvents.OnTrainingRestart += ResetBolt;
    }

    private void OnDisable()
    {
        GameEvents.OnTrainingRestart -= ResetBolt;
    }

    private void Start()
    {
        nutAnimator = AnimatedNut.GetComponent<Animator>();
        nutAnimEmitter = AnimatedNut.GetComponent<AnimationEmitter>();
        AnimatedNut.SetActive(false);
        NutSocket.gameObject.SetActive(true);
    }

    public void PlaceNut()
    {
        if (GameManager.GetCurrentSequenceOrder() == SequenceOrderId)
        {
            NutSocket.interactionManager.SelectExit(NutSocket, NutSocket.firstInteractableSelected);
            AudioManager.Instance?.PlayAudioClip(SoundsName.PlaceNut);
            NutSocket.gameObject.SetActive(false);
            AnimatedNut.SetActive(true);
            GameEvents.InvokeNutPlacedSuccess();
        }
        else
        {
            NutSocket.interactionManager.SelectExit(NutSocket, NutSocket.firstInteractableSelected);
            AudioManager.Instance?.PlayAudioClip(SoundsName.PlaceFail);
            GameEvents.InvokeNutPlacedFail();
        }

    }

    public void TightenNut()
    {
        AudioManager.Instance?.PlayAudioClip(SoundsName.TightenNut);
        AnimatedNut.GetComponent<XRSimpleInteractable>().enabled = false;
        nutAnimEmitter.OnAnimationEnd += FinishTightenNut;
        nutAnimator.Play("Turn_And_Down");
        GameEvents.InvokeNutTightenedStart();
    }

    public void FinishTightenNut()
    {
        AudioManager.Instance?.Stop();
        AudioManager.Instance?.PlayAudioClip(SoundsName.CompleteTighten);
        successParticle.Play();
        nutAnimEmitter.OnAnimationEnd -= FinishTightenNut;
        GameEvents.InvokeNutTightenedEnd();
    }

    private void ResetBolt()
    {
        AnimatedNut.GetComponent<XRSimpleInteractable>().enabled = true;
        nutAnimator.Play("Idle");
        AnimatedNut.SetActive(false);
        NutSocket.gameObject.SetActive(true);
    }
}