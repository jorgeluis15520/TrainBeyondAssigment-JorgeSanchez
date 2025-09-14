using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.LowLevel;

public class GameManager : MonoBehaviour
{
    [SerializeField] private GameObject graspableNut;
    [SerializeField] private GameObject testCap;
    [SerializeField] private Transform nutOriginTransform;

    private static int currentSequenceOrder = 1;
    private int goalSequence = 8;

    private GameState state = GameState.WaitingForGrabbing;

    private void OnEnable()
    {
        GameEvents.OnNutPlacedSuccess += NutPlaced;
        GameEvents.OnNutPlacedFail += ResetLastSequence;
        GameEvents.OnNutTightenedStart += NutTightened;
        GameEvents.OnNutTightenedEnd += CheckTrainingComplete;
    }

    private void OnDisable()
    {
        GameEvents.OnNutPlacedSuccess -= NutPlaced;
        GameEvents.OnNutPlacedFail -= ResetLastSequence;
        GameEvents.OnNutTightenedStart -= NutTightened;
        GameEvents.OnNutTightenedEnd -= CheckTrainingComplete;
    }

    private void Start()
    {
        graspableNut.SetActive(false);
        testCap.SetActive(true);
        state = GameState.WaitingForGrabbing;
    }

    public void StartTraining()
    {
        testCap.SetActive(false);
        MoveNutToOrigin();
        GameEvents.InvokeTrainginStart();
    }

    public void MoveNutToOrigin()
    {
        graspableNut.SetActive(true);
        Rigidbody rgbd = graspableNut.GetComponent<Rigidbody>();
        rgbd.linearVelocity = Vector3.zero;
        rgbd.MovePosition(nutOriginTransform.position);
        rgbd.MoveRotation(transform.rotation);
    }

    public void ResetLastSequence()
    {
        UpdateGamestate(GameState.WaitingForGrabbing);
        MoveNutToOrigin();
    }

    public void NutGrabbed()
    {
        AudioManager.Instance?.PlayAudioClip(SoundsName.GrabNut);
        UpdateGamestate(GameState.WaitingForPlacing);
        GameEvents.InvokeNutGrabbed();
    }

    public void NutPlaced()
    {
        UpdateGamestate(GameState.WaitingForTightening);
        MoveNutToOrigin();
        graspableNut.SetActive(false);
    }

    public void NutTightened()
    {
        UpdateGamestate(GameState.Tightening);
    }

    public void DropNut()
    {
        AudioManager.Instance?.PlayAudioClip(SoundsName.DropNut);
        graspableNut.SetActive(false);
        ResetLastSequence();
        GameEvents.InvokeNutDropped();
    }

    private void CheckTrainingComplete()
    {
        currentSequenceOrder++;
        if (currentSequenceOrder > goalSequence)
        {
            EndTraining();
        }
        else
        {
            UpdateGamestate(GameState.WaitingForGrabbing);
            MoveNutToOrigin();
        }
    }

    private void EndTraining()
    {
        AudioManager.Instance?.PlayAudioClip(SoundsName.TrainingComplete);
        testCap.SetActive(true);
        GameEvents.InvokeTrainingComplete();
    }

    public GameState GetGameState() { return state; }

    public static int GetCurrentSequenceOrder() { return currentSequenceOrder; }

    public void UpdateGamestate(GameState newState)
    {
        state = newState;
        GameEvents.InvokeUpdateState();
    }

    public void Restart()
    {
        Start();
        GameEvents.InvokeTrainignRestart();
    }
}
