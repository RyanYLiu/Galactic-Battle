using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MusicPlayer : MonoBehaviour
{
    [SerializeField] AudioClip bossMusic;
    [SerializeField] AudioClip defeatMusic;
    [SerializeField] AudioClip victoryMusic;
    AudioSource audioSource;

    // Start is called before the first frame update
    void Awake()
    {
        int sceneIndex = SceneManager.GetActiveScene().buildIndex;
        if (sceneIndex == 0) { return; }
        audioSource = GetComponent<AudioSource>();
        SetUpSingleton();
    }

    private void SetUpSingleton()
    {
        int numMusicPlayers = FindObjectsOfType(GetType()).Length;
        if (numMusicPlayers > 1)
        {
            gameObject.SetActive(false);
            Destroy(gameObject);
        }
        else
        {
            DontDestroyOnLoad(gameObject);
            audioSource.Play();
            audioSource.ignoreListenerPause = true;
        }
    }

    public void PlayVictoryMusic()
    {
        audioSource.clip = victoryMusic;
        audioSource.Play();
    }

    public void PlayBossMusic()
    {
        audioSource.clip = bossMusic;
        audioSource.Play();
    }

    public void PlayDefeatMusic()
    {
        audioSource.clip = defeatMusic;
        audioSource.Play();
    }

    public void PauseMusic()
    {
        audioSource.Pause();
    }
}
