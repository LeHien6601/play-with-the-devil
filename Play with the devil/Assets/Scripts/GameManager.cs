using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public GameState currentGameState = GameState.RUN;
    public enum GameState { RUN, WIN, LOSE };

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
    }

    public IEnumerator SetLoseState()
    {
        currentGameState = GameState.LOSE;
        yield return new WaitForSeconds(3f);
        PlayerController.instance.LoseGameAction();
        DevilController.instance.LoseGameAction();
    }

    public IEnumerator SetAskTFState()
    {
        yield return new WaitForSeconds(1f);
        DevilController.instance.GiveTrueFalseResultAction();
        yield return new WaitForSeconds(1f);
        PlayerController.instance.NormalResponseAction();
    }

    public IEnumerator SetUseCardState()
    {
        yield break;
    }
}
