using System;
using UnityEngine;

public static class GameEvents
{
    public static event Action OnTrainingStart;
    public static event Action OnTrainingComplete;
    public static event Action OnNutGrabbed;
    public static event Action OnNutPlacedSuccess;
    public static event Action OnNutPlacedFail;
    public static event Action OnNutTightenedStart;
    public static event Action OnNutTightenedEnd;
    public static event Action OnNutDropped;
    public static event Action OnUpdateState;

    public static event Action OnTrainingRestart;

    public static void InvokeTrainginStart() => OnTrainingStart?.Invoke();
    public static void InvokeTrainingComplete() => OnTrainingComplete?.Invoke();
    public static void InvokeNutGrabbed() => OnNutGrabbed?.Invoke();
    public static void InvokeNutPlacedSuccess() => OnNutPlacedSuccess?.Invoke();
    public static void InvokeNutPlacedFail() => OnNutPlacedFail?.Invoke();
    public static void InvokeNutTightenedStart() => OnNutTightenedStart?.Invoke();
    public static void InvokeNutTightenedEnd() => OnNutTightenedEnd?.Invoke();
    public static void InvokeNutDropped() => OnNutDropped?.Invoke();
    public static void InvokeUpdateState() => OnUpdateState?.Invoke();
    public static void InvokeTrainignRestart() => OnTrainingRestart?.Invoke();
}
