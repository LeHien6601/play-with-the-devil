using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Page : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI pageNumberTMP;
    [SerializeField] private int pageNumber;
    [SerializeField] private Animator animator;
    private void Start()
    {
        pageNumberTMP.text = pageNumber.ToString();
    }
    public void ShowPage(bool isBackward)
    {
        animator.SetBool("Show", true);
        animator.SetBool("Backward", isBackward);
    }

    public void HidePage(bool isBackward)
    {
        animator.SetBool("Show", false);
        animator.SetBool("Backward", isBackward);
    }
}
