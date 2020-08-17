using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Level : MonoBehaviour
{
    [SerializeField] float gameOverDelay = 3f;

    public void LoadGameOver()
    {
        StartCoroutine(DelayGameOverLoad());
    }

    private IEnumerator DelayGameOverLoad()
    {
        yield return new WaitForSeconds(gameOverDelay);
        SceneManager.LoadScene("Game Over");
    }

    public void LoadGame()
    {
        SceneManager.LoadScene("Game");
        GameSession gameSession = FindObjectOfType<GameSession>();
        if (gameSession)
        {
            gameSession.ResetScore();
        }
    }

    public void LoadCustomization()
    {
        SceneManager.LoadScene("Ship Customization");
        Player[] player = Resources.FindObjectsOfTypeAll<Player>();
        if (player.Length > 0)
        {
            Destroy(player[0].gameObject);
        }
    }

    public void LoadStartMenu()
    {
        Destroy(FindObjectOfType<MusicPlayer>().gameObject);
        SceneManager.LoadScene(0);
    }

    public void LoadVictory()
    {
        // TODO: load scene victory
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
