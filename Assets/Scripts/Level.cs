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
        FindObjectOfType<MusicPlayer>().PlayDefeatMusic();
        SceneManager.LoadScene("Game Over");
    }

    public void LoadGame()
    {
        SceneManager.LoadScene("Game");
        Destroy(FindObjectOfType<MusicPlayer>().gameObject);
        GameSession gameSession = FindObjectOfType<GameSession>();
        if (gameSession)
        {
            gameSession.ResetScore();
        }
    }

    public void LoadCustomization()
    {
        SceneManager.LoadScene("Ship Customization");
        Player player = FindObjectOfType<Player>();
        if (player)
        {
            SceneManager.sceneLoaded -= player.WaitForGame;
            Destroy(player.gameObject);
        }
        Destroy(FindObjectOfType<MusicPlayer>().gameObject);
    }

    public void LoadStartMenu()
    {
        Destroy(FindObjectOfType<MusicPlayer>().gameObject);
        SceneManager.LoadScene(0);
    }

    public void LoadVictory()
    {
        StartCoroutine(DelayLoadVictory());
    }

    public IEnumerator DelayLoadVictory()
    {
        yield return new WaitForSeconds(gameOverDelay);
        FindObjectOfType<MusicPlayer>().PlayVictoryMusic();
        foreach (Transform child in FindObjectOfType<Player>().transform)
        {
            child.gameObject.SetActive(false);
        }
        SceneManager.LoadScene("Victory");
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
