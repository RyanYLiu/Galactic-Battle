using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour
{
    // config params
    [Header("Player")]
    [SerializeField] GameObject deathVFX;
    [SerializeField] float durationOfExplosion = 1f;
    [SerializeField] GameObject invincibilityVFX;
    [SerializeField] float invincibilityDuration = 2f;

    [Header("Sound Effects")]
    [SerializeField] AudioClip deathSound;
    [Range(0,1)] [SerializeField] float deathSoundVolume = 0.2f;
    Pauser pauser;
    bool respawning = false;
    [SerializeField] bool invincible = false;
    bool processingHit = false;


    Respawner respawner;
    SpriteRenderer hitbox;
    PlayerBombs bombs;
    PlayerMovement movement;
    PlayerShooting attack;
    PlayerLife life;
    PlayerShield shield;
    PlayerFocus focus;
    CircleCollider2D myCollider;

    // Start is called before the first frame update
    void Start()
    {
        SetupSingleton();
    }

    private void SetUpGame()
    {
        bombs = GetComponent<PlayerBombs>();
        movement = GetComponent<PlayerMovement>();
        attack = GetComponent<PlayerShooting>();
        life = GetComponent<PlayerLife>();
        shield = GetComponent<PlayerShield>();
        focus = GetComponent<PlayerFocus>();
        myCollider = GetComponent<CircleCollider2D>();
        hitbox = GetComponent<SpriteRenderer>();
        respawner = FindObjectOfType<Respawner>();
        pauser = FindObjectOfType<Pauser>();
    }

    private void SetupSingleton()
    {
        int numPlayers = FindObjectsOfType(GetType()).Length;
        if (numPlayers > 1)
        {
            gameObject.SetActive(false);
            Destroy(gameObject);
        }
        else
        {
            DontDestroyOnLoad(gameObject);
            SceneManager.sceneLoaded += WaitForGame;
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        DamageDealer damageDealer = other.GetComponent<DamageDealer>();
        InvincibilityPowerup invincibilityPowerup = other.GetComponent<InvincibilityPowerup>();
        if (!invincible && damageDealer)
        {
            if (!processingHit) {
                processingHit = true;
                ProcessHit(damageDealer);
            }
            
            if (other.GetComponent<BossLaser>())
            {
                return;
            }
            
            damageDealer.Hit();
        }
        else if (invincibilityPowerup)
        {
            float duration = invincibilityPowerup.GetDuration();
            StartInvincibility(duration);
        }
    }

    public void StartInvincibility(float duration)
    {
        StartCoroutine(Invincibility(duration));
    }

    IEnumerator Invincibility(float duration)
    {
        GameObject invincibilitySparkles = Instantiate(invincibilityVFX, transform.position, Quaternion.identity);
        invincibilitySparkles.transform.SetParent(transform);
        invincible = true;
        yield return new WaitForSeconds(duration);
        invincible = false;
        Destroy(invincibilitySparkles.gameObject);
    }

    public void WaitForGame(Scene scene, LoadSceneMode mode)
    {
        if (scene.name == "Game")
        {
            ResetPlayer();
        }
        else
        {
            EnableComponents(false);
        }
    }

    private void ProcessHit(DamageDealer damageDealer)
    {
        int damage = damageDealer.GetDamage();
        if (shield.IsShieldUp())
        {
            shield.ShieldsDown();
            processingHit = false;
            return;
        }
        else
        {
            life.Subtract(damage);
        }

        if (life.GetLifeCount() <= 0)
        {
            Die();
        }
        else
        {
            PlayExplosionVFX();
            bombs.ClearScreen();
            attack.LevelDown();
            hitbox.enabled = false;
            myCollider.enabled = false;
            movement.ResetMoveSpeed();
            respawner.StartRespawn();
            SetRespawnStatus(true);
            foreach (Transform child in transform)
            {
                child.gameObject.SetActive(false);
            }
        }
        processingHit = false;
    }

    public void ResetPlayer()
    {
        SetUpGame();
        EnableComponents(true);
        SetRespawnStatus(false);
        foreach (Transform child in transform)
        {
            child.gameObject.SetActive(true);
        }
        transform.position = respawner.transform.position;
    }

    private void Die()
    {
        PlayExplosionVFX();
        SetRespawnStatus(true);
        myCollider.enabled = false;
        foreach (Transform child in transform)
        {
            child.gameObject.SetActive(false);
        }
        FindObjectOfType<Level>().LoadGameOver();
    }

    public void Win()
    {
        SetRespawnStatus(true);
    }

    private void EnableComponents(bool val)
    {
        bombs.enabled = val;
        focus.enabled = val;
        movement.enabled = val;
        attack.enabled = val;
        life.enabled = val;
        shield.enabled = val;
        myCollider.enabled = val;
    }

    private void PlayExplosionVFX()
    {
        AudioSource.PlayClipAtPoint(deathSound, Camera.main.transform.position, deathSoundVolume);
        GameObject explosion = Instantiate(deathVFX, transform.position, Quaternion.identity);
        Destroy(explosion, durationOfExplosion);
    }

    public float GetInvincibilityDuration()
    {
        return invincibilityDuration;
    }

    public bool GetRespawnStatus()
    {
        return respawning;
    }

    public void SetRespawnStatus(bool val)
    {
        respawning = val;
    }
}
