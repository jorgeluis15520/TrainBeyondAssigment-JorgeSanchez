using UnityEngine;

public enum SoundsName
{
    GrabNut,
    PlaceNut,
    TightenNut,
    CompleteTighten,
    PlaceFail,
    DropNut,
    TrainingComplete,
    ButtonPress,
}

[System.Serializable]
public class CustomAudioClip
{
    public SoundsName soundName;
    public AudioClip audioClip;
}
