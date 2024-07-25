using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SubGameManager : MonoBehaviour
{
    public void LoadLevel(int level)
    {
        GameManager.instance.LoadLevel(level);
    }

    public void LoadNextLevel()
    {
        GameManager.instance.LoadLevel(GameManager.instance.CurrentLevel() + 1);
    }

    public void LoadCurrentLevel()
    {
        GameManager.instance.LoadLevel(GameManager.instance.CurrentLevel());
    }
}
