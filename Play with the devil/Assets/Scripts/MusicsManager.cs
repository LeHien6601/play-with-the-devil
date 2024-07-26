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
    public enum MusicType
    {
        MainMenu,
        EasyLevel,
        MediumLevel,
        HardLevel
    }
}
