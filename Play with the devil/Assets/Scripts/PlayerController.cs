using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    static public PlayerController instance;
    public string[] loseSentences;
    public string[] winSentences;
    public string[] selectFakeCardsSentences;
    public string[] responseSentences;
    public string[] wrongAnswerSentences;
    [SerializeField] private TextMeshProUGUI talkTMP;
    [SerializeField] private GameObject slimeBox;
    [SerializeField] private float delayTime = 3.5f;

    private float timer = 0f;

    [SerializeField] private Animator animator;
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
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            //Talk
            AnswerFakeCardsResultAction();
        }
        else if (Input.GetKeyDown(KeyCode.E))
        {
            //Win
            WinGameAction();
        }
        else if (Input.GetKeyDown(KeyCode.R))
        {
            //Lose
            LoseGameAction();
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
    public void WinGameAction()
    {
        //Win
        animator.SetInteger("Win", 1);
        slimeBox.SetActive(true);
        talkTMP.text = GetRandomSentence(winSentences);
        timer = delayTime * 5;
    }
    public void LoseGameAction()
    {
        //Lose
        animator.SetInteger("Win", 0);
        slimeBox.SetActive(true);
        talkTMP.text = GetRandomSentence(loseSentences);
        timer = delayTime * 5;
    }
    public void AnswerFakeCardsResultAction()
    {
        //Talk
        animator.SetBool("Talk", true);
        animator.SetInteger("Win", -1);
        slimeBox.SetActive(true);
        talkTMP.text = GetRandomSentence(selectFakeCardsSentences);
        timer = delayTime;
    }
    public void AnswerWrongAction()
    {
        animator.SetBool("Talk", true);
        animator.SetInteger("Win", -1);
        slimeBox.SetActive(true);
        talkTMP.text = GetRandomSentence(wrongAnswerSentences);
        timer = delayTime;
    }
    public void NormalResponseAction()
    {
        animator.SetBool("Talk", true);
        animator.SetInteger("Win", -1);
        slimeBox.SetActive(true);
        talkTMP.text = GetRandomSentence(responseSentences);
        timer = delayTime;
    }
}
