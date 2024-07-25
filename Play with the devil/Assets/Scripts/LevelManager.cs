using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LevelManager : MonoBehaviour
{
    [SerializeField] private Button[] levelButtons;
    static public LevelManager instance;
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
            levelButtons[i].onClick.AddListener(() => { GameManager.instance.LoadLevel(j + 1); Debug.Log("add " + j); });
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
            }
            else
            {
                levelButtons[i].interactable = false;
            }
        }
    }
}
