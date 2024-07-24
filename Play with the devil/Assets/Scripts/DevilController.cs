using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DevilController : MonoBehaviour
{
    static public DevilController instance;
    public string[] loseSentences;
    public string[] winSentences;
    public string[] selectCorrectSentences;
    public string[] selectIncorrectSentences;
    public string[] responseSentences;
    public string[] remindSentences;
    public int numberOfTrue = 0;
    public int numberOfFalse = 0;
    [SerializeField] private TextMeshProUGUI talkTMP;
    [SerializeField] private TextMeshProUGUI trueTMP;
    [SerializeField] private TextMeshProUGUI falseTMP;
    [SerializeField] private GameObject answer;
    [SerializeField] private GameObject devilBox;
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
            //Right answer
            ResponeAnswerAction(true);
        }
        else if (Input.GetKeyDown(KeyCode.W))
        {
            //Wrong answer
            ResponeAnswerAction(false);
        }
        else if (Input.GetKeyDown(KeyCode.E))
        {
            //Win game
            WinGameAction();
        }
        else if (Input.GetKeyDown(KeyCode.R))
        {
            //Lose game
            LoseGameAction();
        }
        else if (Input.GetKeyDown(KeyCode.T))
        {
            //Give answer
            GiveTrueFalseResultAction();
        }
        if (timer > 0)
        {
            timer -= Time.deltaTime;
            if (timer <= 0)
            {
                timer = 0;
                devilBox.SetActive(false);
                animator.SetBool("Talk", false);
            }
        }
    }
    public void IncreaseNumberOfTF(bool isTrue)
    {
        if (isTrue)
        {
            numberOfTrue++;
        }
        else
        {
            numberOfFalse++;
        }
    }
    private string GetRandomSentence(string[] strings)
    {
        if (string.IsNullOrEmpty(strings[0])) { return null; }
        return strings[Random.Range(0, strings.Length)];
    }
    public void GiveTrueFalseResultAction()
    {
        //Give answer
        answer.SetActive(true);
        devilBox.SetActive(true);
        trueTMP.text = numberOfTrue.ToString();
        falseTMP.text = numberOfFalse.ToString();
        talkTMP.text = "";
        timer = delayTime;
        animator.SetInteger("Win", -1);
        animator.SetBool("Talk", true);
    }
    public void LoseGameAction()
    {
        //Lose game
        answer.SetActive(false);
        devilBox.SetActive(true);
        talkTMP.text = GetRandomSentence(loseSentences);
        timer = delayTime * 5;
        animator.SetInteger("Win", 0);
    }
    public void WinGameAction()
    {
        //Win game
        answer.SetActive(false);
        devilBox.SetActive(true);
        talkTMP.text = GetRandomSentence(winSentences);
        timer = delayTime * 5;
        animator.SetInteger("Win", 1);
    }
    public void ResponeAnswerAction(bool correct)
    {
        //Right answer
        answer.SetActive(false);
        devilBox.SetActive(true);
        talkTMP.text = GetRandomSentence((correct) ? selectCorrectSentences : selectIncorrectSentences);
        timer = delayTime;
        animator.SetInteger("Win", -1);
        animator.SetBool("Talk", true);
    }
    public void NormalResponseAction()
    {
        answer.SetActive(false);
        devilBox.SetActive(true);
        talkTMP.text = GetRandomSentence(responseSentences);
        timer = delayTime;
        animator.SetInteger("Win", -1);
        animator.SetBool("Talk", true);
    }
    public void RemindSwapCardsAction()
    {
        answer.SetActive(false);
        devilBox.SetActive(true);
        talkTMP.text = GetRandomSentence(remindSentences);
        timer = delayTime;
        animator.SetInteger("Win", -1);
        animator.SetBool("Talk", true);
    }
}
