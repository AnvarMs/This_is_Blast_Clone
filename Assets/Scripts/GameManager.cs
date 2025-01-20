using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    static GameManager instance;
    [SerializeField]
    GameObject winPanel;
    [SerializeField]
    GameObject winParticle;
    [SerializeField]
    GameObject loosPanel;
    [SerializeField]
    GameObject gameFinish;
    [SerializeField]
    GameObject pouseButton;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(instance);
        }
    }
    public static GameManager Instance { get { return instance; } }
  
    public void GameWin()
    {
        winPanel.SetActive(true);
        winParticle.SetActive(true);
        pouseButton.SetActive(false);
        Handheld.Vibrate();
    }
    public void GameOver()
    {
        loosPanel.SetActive(true);
        pouseButton.SetActive(false);
        Handheld.Vibrate();

    }
    public void RestartGame()
    {
        NextLevel(SceneManager.GetActiveScene().buildIndex);
    }
    public void NextLevel(int level)
    {
        SceneManager.LoadScene(level);
    }

    public void GameFinish()
    {
        gameFinish.SetActive(true);
        pouseButton.SetActive(false);
        winPanel.SetActive(false);
        Handheld.Vibrate();
    }
    public void GameQuit() {
     Application.Quit();
    }
}
