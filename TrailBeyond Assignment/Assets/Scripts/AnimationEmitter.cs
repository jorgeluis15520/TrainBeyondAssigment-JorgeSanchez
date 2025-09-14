using System;
using UnityEngine;

public class AnimationEmitter : MonoBehaviour
{
    public event Action OnAnimationEnd;

    public void InvokeAnimationEnd()
    {
        OnAnimationEnd?.Invoke();
    }
}
