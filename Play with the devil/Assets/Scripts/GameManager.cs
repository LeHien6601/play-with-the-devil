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
        DontDestroyOnLoad(gameObject);
        if (currentLevel == 0)
        {
            LevelManager.instance.UpdateLevelUnlockedState();
        }
    }
    public void QuitGame()
    {
        Application.Quit();
    }

    public IEnumerator SetWinState()
    {
        currentGameState = GameState.WIN;
        yield return new WaitForSeconds(3f);
        PlayerController.instance.WinGameAction();
        DevilController.instance.WinGameAction();
        //Next level or main menu
    }

    public IEnumerator SetLoseState()
    {
        currentGameState = GameState.LOSE;
        yield return new WaitForSeconds(3f);
        PlayerController.instance.LoseGameAction();
        DevilController.instance.LoseGameAction();
        //Restart or main menu
    }

    public IEnumerator SetAskTFState()
    {
        yield return new WaitForSeconds(1f);
        DevilController.instance.GiveTrueFalseResultAction();
        yield return new WaitForSeconds(1f);
        PlayerController.instance.NormalResponseAction();
    }

    public IEnumerator SetUseCardState(bool isTrue)
    {
        yield return new WaitForSeconds(1f);
        DevilController.instance.IncreaseNumberOfTF(isTrue);
        DevilController.instance.NormalResponseAction();
    }

    public IEnumerator SetWrongAnswerState()
    {
        yield return new WaitForSeconds(1f);
        DevilController.instance.ResponeAnswerAction(false);
        yield return new WaitForSeconds(1f);
        PlayerController.instance.AnswerWrongAction();
    }

    public IEnumerator SetRemindSwapCardsState(float delay)
    {
        yield return new WaitForSeconds(delay);
        DevilController.instance.RemindSwapCardsAction();
        yield return new WaitForSeconds(1f);
        PlayerController.instance.NormalResponseAction();
    }
}
