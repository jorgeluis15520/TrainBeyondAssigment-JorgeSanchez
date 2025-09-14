using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private GraspableNut graspableNut;
    [SerializeField] private GameObject testCap;
    [SerializeField] private Transform nutOriginTransform;

    private static int currentSequenceOrder = 1;
    private int goalSequence = 8;

    private GameState state = GameState.WaitingForGrabbing;

    private void OnEnable()
    {
        GameEvents.OnNutGrabbed += NutGrabbed;
        GameEvents.OnNutDropped += NutDropped;
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
        graspableNut.gameObject.SetActive(false);
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
        graspableNut.gameObject.SetActive(true);
        graspableNut.MoveToTransform(nutOriginTransform);        
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
    }

    public void NutPlaced()
    {
        UpdateGamestate(GameState.WaitingForTightening);
        MoveNutToOrigin();
        graspableNut.gameObject.SetActive(false);
    }

    public void NutTightened()
    {
        UpdateGamestate(GameState.Tightening);
    }

    public void NutDropped()
    {
        if (graspableNut.IsOnSocketRange()) return;

        AudioManager.Instance?.PlayAudioClip(SoundsName.DropNut);
        graspableNut.gameObject.SetActive(false);
        ResetLastSequence();
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
