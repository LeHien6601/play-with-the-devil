using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UIDescriptionHandler : MonoBehaviour
{
    [SerializeField] private GameObject descriptionBox;
    private bool isActive = true;
    public void TurnDescriptionBox(bool on)
    {
        if (!isActive) return;
        descriptionBox.SetActive(on);
    }
    public void SetActive(bool active)
    {
        isActive = active;
    }
}
