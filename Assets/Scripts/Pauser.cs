using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pauser : MonoBehaviour
{
    AudioListener audioListener;
    [SerializeField] Canvas pauseScreen;
    [SerializeField] bool paused = false;       // Debug

    // Start is called before the first frame update
    void Start()
    {
        audioListener = FindObjectOfType<AudioListener>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonUp("Cancel"))
        {
            if (Time.timeScale == 1.0f)
            {
                Time.timeScale = 0f;
                paused = true;
                AudioListener.pause = true;
                pauseScreen.gameObject.SetActive(true);
            }
            else
            {
                Time.timeScale = 1.0f;
                paused = false;
                AudioListener.pause = false;
                pauseScreen.gameObject.SetActive(false);
            }
        }
    }

    public bool IsPaused()
    {
        return paused;
    }
}
