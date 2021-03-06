﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Warning : MonoBehaviour
{
    [SerializeField] float opacityIncrement = 0.01f;
    [SerializeField] float maxOpacity = 0.6f;
    BossSpawner bossSpawner;
    int numFlashes = 3;
    bool increaseOpacity = true;
    Image backgroundColor;
    int flashCounter = 0;
    Pauser pauser;
    MusicPlayer music;
    [SerializeField] AudioClip warningSound;
    [SerializeField] float warningSoundVolume = 0.05f;

    // Start is called before the first frame update
    void Start()
    {
        backgroundColor = GetComponent<Image>();
        bossSpawner = FindObjectOfType<BossSpawner>();
        pauser = FindObjectOfType<Pauser>();
        music = FindObjectOfType<MusicPlayer>();
        music.PauseMusic();
    }

    // Update is called once per frame
    void Update()
    {
        if (pauser.IsPaused()) { return; }

        FlashScreen();
        if (flashCounter >= numFlashes)
        {
            transform.GetChild(0).gameObject.SetActive(false);
            bossSpawner.GetComponent<BossSpawner>().SpawnBoss();
            GetComponent<Warning>().enabled = false;
        }
    }

    private void FlashScreen()
    {
        var color = backgroundColor.color;
        if (increaseOpacity)
        {
            color.a += opacityIncrement;
            if (color.a >= maxOpacity)
            {
                increaseOpacity = false;
                AudioSource.PlayClipAtPoint(warningSound, Camera.main.transform.position, warningSoundVolume);
            }
        }
        else
        {
            color.a -= opacityIncrement;
            if (color.a <= 0)
            {
                increaseOpacity = true;
                flashCounter += 1;
            }
        }
        backgroundColor.color = color;
    }
}
