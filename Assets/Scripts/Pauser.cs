using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pauser : MonoBehaviour
{
    AudioListener audioListener;
    [SerializeField] GameObject pauseScreen;

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
                AudioListener.pause = true;
                pauseScreen.SetActive(true);
            }
            else
            {
                Time.timeScale = 1.0f;
                AudioListener.pause = false;
                pauseScreen.SetActive(false);
            }
        }
    }
}
