using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicsManager : MonoBehaviour
{
    public MusicClip[] musics;
    [System.Serializable]
    public class MusicClip
    {
        public MusicType type;
        public AudioClip clip;
    }
    static public MusicsManager instance;
    [SerializeField] private AudioSource source;
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
    public void PlayMusic(MusicType type)
    {
        source.clip = GetAudioClip(type);
        source.Play();
    }
    public void StopMusic()
    {
        source.Stop();
    }
    private AudioClip GetAudioClip(MusicType type)
    {
        return Array.Find(musics, m => m.type == type).clip;
    }
    public enum MusicType
    {
        MainMenu,
        EasyLevel,
        MediumLevel,
        HardLevel
    }
}
