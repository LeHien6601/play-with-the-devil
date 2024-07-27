using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Clock : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI clockTMP;
    [SerializeField] private Animator animator;
    [SerializeField] private AudioSource audioSource;
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
        audioSource.PlayOneShot(SoundsManager.instance.GetAudioClip(SoundsManager.SoundType.CardMove));
        yield return new WaitForSeconds(0.2f);
        audioSource.clip = SoundsManager.instance.GetAudioClip(SoundsManager.SoundType.ClockTick);
        yield return new WaitForSeconds(1f);
        audioSource.Play();
        timer = time;
        yield return new WaitForSeconds(time);
        audioSource.Stop();
        yield return new WaitForSeconds(1f);
        animator.SetBool("Show", false);
        audioSource.PlayOneShot(SoundsManager.instance.GetAudioClip(SoundsManager.SoundType.CardMove));
    }
}
