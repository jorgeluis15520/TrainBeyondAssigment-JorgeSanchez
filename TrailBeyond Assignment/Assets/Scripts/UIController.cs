using System.Collections.Generic;
using TMPro;
using UnityEngine;

[System.Serializable]
public class Tip
{
    public GameState state;
    public string text;
}

public class UIController : MonoBehaviour
{
    [SerializeField] private GameObject startPanel;
    [SerializeField] private GameObject missionPanel;
    [SerializeField] private GameObject infoPanel;
    [SerializeField] private GameObject finalPanel;
    [SerializeField] private TMP_Text tipUIText;
    [SerializeField] private List<Tip> tips;
    [SerializeField] private GameManager gameManager;
    private List<GameObject> panels;

    private void OnEnable()
    {
        GameEvents.OnTrainingStart += StartTrainingUI;
        GameEvents.OnUpdateState += UpdateTip;
        GameEvents.OnTrainingComplete += DisplayResetPanel;
        GameEvents.OnTrainingRestart += Start;
    }

    private void OnDisable()
    {
        GameEvents.OnTrainingStart -= StartTrainingUI;
        GameEvents.OnUpdateState -= UpdateTip;
        GameEvents.OnTrainingComplete += DisplayResetPanel;
        GameEvents.OnTrainingRestart -= Start;
    }

    private void Start()
    {
        panels = new List<GameObject> { startPanel, missionPanel, infoPanel, finalPanel };
        DisplayPanel(startPanel);
    }

    public void DisplayPanel(GameObject panelObject)
    {
        panels.ForEach(panel => panel.SetActive(false));
        panelObject.SetActive(true);
    }

    private void DisplayResetPanel()
    {
        DisplayPanel(finalPanel);
    }

    private void StartTrainingUI()
    {
        DisplayPanel(infoPanel);
        UpdateTip();
    }

    private void UpdateTip()
    {
        SetTipText(gameManager.GetGameState());
    }

    private void SetTipText(GameState state)
    {
        tips.ForEach(tip =>
        {
            if (state == tip.state)
            {
                tipUIText.text = tip.text;
                return;
            }
        });
    }

    public void PlayPressButtonSound()
    { 
        AudioManager.Instance?.PlayAudioClip(SoundsName.ButtonPress);
    }
}
