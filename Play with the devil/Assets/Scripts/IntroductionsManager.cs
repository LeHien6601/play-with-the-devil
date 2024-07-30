using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class IntroductionsManager : MonoBehaviour
{
    [SerializeField] private Button nextBTN;
    [SerializeField] private Button prevBTN;
    [SerializeField] private int currentPage = 0;
    [SerializeField] private GameObject pagesObject;
    private Page []pages;
    private void Start()
    {
        pages = pagesObject.GetComponentsInChildren<Page>();
        pages[0].ShowPage(false);
        CheckButtonCondition();
    }
    public void NextPage()
    {
        currentPage++;
        pages[currentPage - 1].HidePage(false);
        pages[currentPage].ShowPage(false);
        CheckButtonCondition();
    }
    public void PreviousPage()
    {
        currentPage--;
        pages[currentPage + 1].HidePage(true);
        pages[currentPage].ShowPage(true);
        CheckButtonCondition();
    }
    private void CheckButtonCondition()
    {
        prevBTN.interactable = (currentPage != 0);
        nextBTN.interactable = (currentPage != pages.Length - 1);
    }
}
