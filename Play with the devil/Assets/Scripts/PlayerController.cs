using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public string[] loseSentences;
    public string[] winSentences;
    public string[] selectFakeCardsSentences;
    [SerializeField] private TextMeshProUGUI talkTMP;
    [SerializeField] private GameObject slimeBox;
    [SerializeField] private float delayTime = 5f;

    private float timer = 0f;

    [SerializeField] private Animator animator;
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            //Talk
            animator.SetBool("Talk", true);
            animator.SetInteger("Win", -1);
            slimeBox.SetActive(true);
            talkTMP.text = GetRandomSentence(selectFakeCardsSentences);
            timer = delayTime;
        }
        else if (Input.GetKeyDown(KeyCode.E))
        {
            //Win
            animator.SetInteger("Win", 1);
            slimeBox.SetActive(true);
            talkTMP.text = GetRandomSentence(winSentences);
            timer = delayTime;
        }
        else if (Input.GetKeyDown(KeyCode.R))
        {
            //Lose
            animator.SetInteger("Win", 0);
            slimeBox.SetActive(true);
            talkTMP.text = GetRandomSentence(loseSentences);
            timer = delayTime;
        }
        if (timer > 0f)
        {
            timer -= Time.deltaTime;
            if (timer <= 0f)
            {
                timer = 0f;
                slimeBox.SetActive(false);
                animator.SetBool("Talk", false);
            }
        }
    }
    private string GetRandomSentence(string[] strings)
    {
        if (string.IsNullOrEmpty(strings[0])) { return null; }
        return strings[Random.Range(0, strings.Length)];
    }

}
