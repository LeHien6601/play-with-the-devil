using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Clock : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI clockTMP;
    [SerializeField] private Animator animator;
    private float timer = 0;

    private void Update()
    {
        if (timer > 0f)
        {
            timer -= Time.deltaTime;
            if (timer <= 0f)
            {
                timer = 0f;
            }
            clockTMP.text = Mathf.RoundToInt(timer).ToString();
        }
    }
    public IEnumerator TriggerClock(float time)
    {
        clockTMP.text = "";
        animator.enabled = true;
        animator.SetBool("Show", true);
        yield return new WaitForSeconds(1.2f);
        timer = time;
        yield return new WaitForSeconds(time + 1);
        animator.SetBool("Show", false);
    }
}
