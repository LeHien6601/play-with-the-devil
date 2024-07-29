using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    [SerializeField] private GameState currentGameState = GameState.RUN;
    public enum GameState { RUN, WIN, LOSE };
    [SerializeField] private int currentLevel = 0;
    [SerializeField] private int currentUnlockedLevel = 1;
    [SerializeField] private int maxLevel = 24;
    public void Awake()
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
    public int CurrentLevel() { return currentLevel; }
    public int CurrentUnlockedLevel() {  return currentUnlockedLevel; }
    public void UnlockNewLevel()
    {
        if (currentUnlockedLevel == maxLevel)
        {
            //Finish game
        }
        else
        {
            currentUnlockedLevel++;
        }
    }
    public void LoadLevel(int level)
    {
        StartCoroutine(LoadLevelWithDelay(level));
    }
    public IEnumerator LoadLevelWithDelay(int level)
    {
        currentLevel = level;
        yield return new WaitForSeconds(1.5f);
        SceneManager.LoadScene(level);
        SoundsManager.instance.PlaySoundOneShot(SoundsManager.SoundType.LevelOpen);
        if (level == 0)
        {
            MusicsManager.instance.PlayMusic(MusicsManager.MusicType.MainMenu);
        }
        else if (level <= 8)
        {
            MusicsManager.instance.PlayMusic(MusicsManager.MusicType.EasyLevel);
        }
        else if (level <= 16)
        {
            MusicsManager.instance.PlayMusic(MusicsManager.MusicType.MediumLevel);
        }
        else
        {
            MusicsManager.instance.PlayMusic(MusicsManager.MusicType.HardLevel);
        }
        DontDestroyOnLoad(gameObject);
    }
    public void QuitGame()
    {
        Application.Quit();
    }

    public IEnumerator SetWinState()
    {
        UnlockNewLevel();
        currentGameState = GameState.WIN;
        yield return new WaitForSeconds(3f);
        PlayerController.instance.WinGameAction();
        DevilController.instance.WinGameAction();
        StartCoroutine(SoundsManager.instance.PlayDevilTalkSound(3.5f));
        StartCoroutine(SoundsManager.instance.PlaySlimeTalkSound(3.5f));
        MusicsManager.instance.StopMusic();
        SoundsManager.instance.PlaySoundOneShot(SoundsManager.SoundType.Win);
        //Next level or main menu
    }

    public IEnumerator SetLoseState()
    {
        currentGameState = GameState.LOSE;
        yield return new WaitForSeconds(3f);
        PlayerController.instance.LoseGameAction();
        DevilController.instance.LoseGameAction();
        StartCoroutine(SoundsManager.instance.PlayDevilTalkSound(3.5f));
        StartCoroutine(SoundsManager.instance.PlaySlimeTalkSound(3.5f));
        MusicsManager.instance.StopMusic();
        SoundsManager.instance.PlaySoundOneShot(SoundsManager.SoundType.Lose);
        //Restart or main menu
    }

    public IEnumerator SetAskTFState()
    {
        yield return new WaitForSeconds(1f);
        DevilController.instance.GiveTrueFalseResultAction();
        StartCoroutine(SoundsManager.instance.PlayDevilTalkSound(3.5f));
        yield return new WaitForSeconds(1f);
        PlayerController.instance.NormalResponseAction();
        StartCoroutine(SoundsManager.instance.PlaySlimeTalkSound(3.5f));
    }

    public IEnumerator SetUseCardState(bool isTrue)
    {
        yield return new WaitForSeconds(1f);
        DevilController.instance.IncreaseNumberOfTF(isTrue);
        DevilController.instance.NormalResponseAction();
        StartCoroutine(SoundsManager.instance.PlayDevilTalkSound(3.5f));
    }

    public IEnumerator SetWrongAnswerState()
    {
        yield return new WaitForSeconds(1f);
        DevilController.instance.ResponeAnswerAction(false);
        StartCoroutine(SoundsManager.instance.PlayDevilTalkSound(3f));
        yield return new WaitForSeconds(1f);
        PlayerController.instance.AnswerWrongAction();
        StartCoroutine(SoundsManager.instance.PlaySlimeTalkSound(3.5f));
    }

    public IEnumerator SetRemindSwapCardsState(float delay)
    {
        yield return new WaitForSeconds(delay);
        DevilController.instance.RemindSwapCardsAction();
        yield return new WaitForSeconds(1f);
        PlayerController.instance.NormalResponseAction();
        StartCoroutine(SoundsManager.instance.PlaySlimeTalkSound(3.5f));
    }
}
