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

    void QuitGame()
    {
        Application.Quit();
    }


}
