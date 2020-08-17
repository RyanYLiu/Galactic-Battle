using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MusicPlayer : MonoBehaviour
{
    // Start is called before the first frame update
    void Awake()
    {
        int sceneIndex = SceneManager.GetActiveScene().buildIndex;
        if (sceneIndex == 0) { return; }
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
            AudioSource audioSource = GetComponent<AudioSource>();
            audioSource.Play();
            audioSource.ignoreListenerPause = true;
        }
    }
}
