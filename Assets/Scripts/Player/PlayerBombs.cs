using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBombs : MonoBehaviour
{
    [Header("Bomb")]
    [SerializeField] int bombCount = 3;
    [SerializeField] int maxBombCount = 3;
    Pauser pauser;
    BombDisplay bombDisplay;

    [Header("Sound Effects")]
    [SerializeField] AudioClip bombUseSound;
    [Range(0,1)] [SerializeField] float bombUseSoundVolume = 0.05f;
    Player player;

    private void OnEnable()
    {
        player = GetComponent<Player>();
        bombCount = maxBombCount;
        bombDisplay = FindObjectOfType<BombDisplay>();
        pauser = FindObjectOfType<Pauser>();
        for (int bombCounter = 0; bombCounter < bombCount; bombCounter++)
        {
            bombDisplay.AddBomb();
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (pauser.IsPaused() || player.GetRespawnStatus()) { return; }
        UseBomb();
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        BombPowerup bombPowerup = other.GetComponent<BombPowerup>();
        if (bombPowerup)
        {
            if (bombCount < maxBombCount)
            {
                bombDisplay.AddBomb();
            }
            bombCount = Mathf.Clamp(bombCount + 1, 0, maxBombCount);
        }
    }

    private void UseBomb()
    {
        if (Input.GetButtonDown("Fire3") && bombCount > 0)
        {
            // TODO: Bomb VFX
            AudioSource.PlayClipAtPoint(bombUseSound, Camera.main.transform.position, bombUseSoundVolume);
            ClearScreen();
            bombCount -= 1;
            bombDisplay.RemoveBomb();
        }
    }

    public void ClearScreen()
    {
        GameObject[] allEnemyProjectiles = GameObject.FindGameObjectsWithTag("Enemy Projectile");
        foreach (GameObject projectile in allEnemyProjectiles)
        {
            Destroy(projectile);
        }
    }
}
