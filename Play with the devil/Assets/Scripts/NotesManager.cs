using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NotesManager : MonoBehaviour
{
    [SerializeField] private GameObject note;
    public void SetNotes(float showTime, bool isSwitch, bool isSwap)
    {
        Instantiate(note, transform).GetComponent<NoteController>().SetNoteContent("You have " + showTime + " seconds to remember all the cards after pressing start button");
        Instantiate(note, transform).GetComponent<NoteController>().SetNoteContent("Use your cards then check T/F Ratio with the devil to find fake cards");
        Instantiate(note, transform).GetComponent<NoteController>().SetNoteContent("You can look up card's information by clicking the icon in the top right corner");
        Instantiate(note, transform).GetComponent<NoteController>().SetNoteContent("You will lose 2 souls if give wrong answer");
        if (isSwap)
        {
            Instantiate(note, transform).GetComponent<NoteController>().SetNoteContent("After asking the devil about T/F ratio, some cards on the table will move");
        }
        if (isSwitch)
        {
            Instantiate(note, transform).GetComponent<NoteController>().SetNoteContent("After asking the devil about T/F ratio, fake cards change from true to false and false to true");
        }
    }
}
