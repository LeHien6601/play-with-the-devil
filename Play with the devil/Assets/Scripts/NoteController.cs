using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class NoteController : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI noteTMP;
    public void SetNoteContent(string content)
    {
        noteTMP.text = content;
    }
}
