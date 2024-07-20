using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DevilController : MonoBehaviour
{
    public string[] loseSentences;
    public string[] winSentences;
    public string[] selectCorrectSentences;
    public string[] selectIncorrectSentences;
    public int numberOfTrue = 0;
    public int numberOfFalse = 0;
    [SerializeField] private TextMeshProUGUI talkTMP;
    [SerializeField] private TextMeshProUGUI trueTMP;
    [SerializeField] private TextMeshProUGUI falseTMP;
    [SerializeField] private GameObject answer;
    [SerializeField] private GameObject devilBox;
    [SerializeField] private float delayTime = 5f;
    private float timer = 0f;

    [SerializeField] private Animator animator;

    private void Update()
    {

        if (Input.GetKeyDown(KeyCode.Q))
        {
            //Right answer
            answer.SetActive(false);
            devilBox.SetActive(true);
            talkTMP.text = GetRandomSentence(selectCorrectSentences);
            timer = delayTime;

            animator.SetInteger("Win", -1);
            animator.SetBool("Talk", true);
        }
        else if (Input.GetKeyDown(KeyCode.W))
        {
            //Wrong answer
            answer.SetActive(false);
            devilBox.SetActive(true);
            talkTMP.text = GetRandomSentence(selectIncorrectSentences);
            timer = delayTime;

            animator.SetInteger("Win", -1);
            animator.SetBool("Talk", true);
        }
        else if (Input.GetKeyDown(KeyCode.E))
        {
            //Win game
            answer.SetActive(false);
            devilBox.SetActive(true);
            talkTMP.text = GetRandomSentence(winSentences);
            timer = delayTime;

            animator.SetInteger("Win", 1);
        }
        else if (Input.GetKeyDown(KeyCode.R))
        {
            //Lose game
            answer.SetActive(false);
            devilBox.SetActive(true);
            talkTMP.text = GetRandomSentence(loseSentences);
            timer = delayTime;
            animator.SetInteger("Win", 0);
        }
        else if (Input.GetKeyDown(KeyCode.T))
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
    private string GetRandomSentence(string[] strings)
    {
        if (string.IsNullOrEmpty(strings[0])) { return null; }
        return strings[Random.Range(0, strings.Length)];
    }
}
