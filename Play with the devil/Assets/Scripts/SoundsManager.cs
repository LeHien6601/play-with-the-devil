using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundsManager : MonoBehaviour
{
    static public SoundsManager instance;
    public SoundClip[] sounds;
    [SerializeField] private AudioSource slimeAudioSource;
    [SerializeField] private AudioSource devilAudioSource;
    [SerializeField] private AudioSource oneShotAudioSource;

    [System.Serializable]
    public class SoundClip
    {
        public SoundType type;
        public AudioClip clip;
    }
    public enum SoundType
    {
        CardShuffle,
        CardMove,
        CardHover,
        CardClick,
        CardTurn,
        LevelOpen,
        LevelClose,
        ButtonHover,
        ButtonClick,
        Win,
        Lose,
        SlimeTalk,
        DevilSlime
    }
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
    private AudioClip GetAudioClip(SoundType type)
    {
        for (int i = 0; i < sounds.Length; i++)
        {
            if (sounds[i].type == type) return sounds[i].clip;
        }
        return null;
    }
    public void PlaySoundOneShot(SoundType type)
    {
        oneShotAudioSource.PlayOneShot(GetAudioClip(type));
    }
    public IEnumerator PlaySlimeTalkSound(float duration)
    {
        slimeAudioSource.PlayScheduled(duration);
        yield return null;
    }
}
