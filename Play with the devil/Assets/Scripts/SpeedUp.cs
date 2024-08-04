using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class SpeedUp : MonoBehaviour
{
    [SerializeField] private int[] speedUpLevels;
    [SerializeField] private int currentSpeedUpLevel = 0;
    [SerializeField] private TextMeshProUGUI speedUpTMP;
    public void NextSpeedUpLevel()
    {
        currentSpeedUpLevel = (currentSpeedUpLevel + 1) % speedUpLevels.Length;
        speedUpTMP.text = "speed up = " + speedUpLevels[currentSpeedUpLevel];
        Time.timeScale = speedUpLevels[currentSpeedUpLevel];
    }
}
