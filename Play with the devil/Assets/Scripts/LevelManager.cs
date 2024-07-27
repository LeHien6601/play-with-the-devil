using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class LevelManager : MonoBehaviour
{
    [SerializeField] private Button[] levelButtons;
    [SerializeField] private SubGameManager subGameManager;
    static public LevelManager instance;
    private int currentUpdateEventTrigger = 0;
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(this.gameObject);
        }
        levelButtons = GetComponentsInChildren<Button>();
    }
    private void Start()
    {
        int i = 0;
        foreach (Button btn in levelButtons)
        {
            int j = i;
            levelButtons[i].GetComponentInChildren<TextMeshProUGUI>().text = (j + 1).ToString();
            levelButtons[i].onClick.AddListener(() => { GameManager.instance.LoadLevel(j + 1);});
            i++;
        }
        UpdateLevelUnlockedState();
    }
    public void UpdateLevelUnlockedState()
    {
        for (int i = 0; i < levelButtons.Length; i++)
        {
            if (i < GameManager.instance.CurrentUnlockedLevel())
            {
                levelButtons[i].interactable = true;
                if (currentUpdateEventTrigger < i)
                {
                    currentUpdateEventTrigger++;
                    EventTrigger evenTrigger = levelButtons[i].AddComponent<EventTrigger>();
                    evenTrigger.enabled = true;
                    EventTrigger.Entry hover = new EventTrigger.Entry();
                    hover.eventID = EventTriggerType.PointerEnter;
                    hover.callback.AddListener((data) => { subGameManager.PlayButtonHoverSound(); });
                    EventTrigger.Entry click = new EventTrigger.Entry();
                    click.eventID = EventTriggerType.PointerClick;
                    click.callback.AddListener((data) => { subGameManager.PlayButtonClickSound(); });
                    evenTrigger.triggers.Add(hover);
                    evenTrigger.triggers.Add(click);
                }
            }
            else
            {
                levelButtons[i].interactable = false;
            }
        }
    }
}
