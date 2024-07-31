using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SubGameManager : MonoBehaviour
{
    public void LoadLevel(int level)
    {
        GameManager.instance.LoadLevel(level);
    }

    public void LoadNextLevel()
    {
        GameManager.instance.LoadLevel(GameManager.instance.CurrentLevel() + 1);
    }

    public void LoadCurrentLevel()
    {
        GameManager.instance.LoadLevel(GameManager.instance.CurrentLevel());
    }
    public void QuitGame()
    {
        GameManager.instance.QuitGame();
    }

    public void PlayButtonHoverSound()
    {
        SoundsManager.instance.PlaySoundOneShot(SoundsManager.SoundType.ButtonHover);
    }
    public void PlayButtonClickSound()
    {
        SoundsManager.instance.PlaySoundOneShot(SoundsManager.SoundType.ButtonClick);
    }
    public void PlayNoteHoverSound()
    {
        SoundsManager.instance.PlaySoundOneShot(SoundsManager.SoundType.CardHover);
    }
    public void PlayOpenLevelSound()
    {
        SoundsManager.instance.PlaySoundOneShot(SoundsManager.SoundType.LevelOpen);
    }
    public void PlayCloseLevelSound()
    {
        SoundsManager.instance.PlaySoundOneShot(SoundsManager.SoundType.LevelClose);
    }
}
