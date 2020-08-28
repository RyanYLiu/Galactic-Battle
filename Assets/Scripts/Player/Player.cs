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
        Debug.Log("PO");
        SetupSingleton();
        bombs = GetComponent<PlayerBombs>();
        Debug.Log(bombs);
        movement = GetComponent<PlayerMovement>();
        attack = GetComponent<PlayerShooting>();
        life = GetComponent<PlayerLife>();
        shield = GetComponent<PlayerShield>();
        focus = GetComponent<PlayerFocus>();
        myCollider = GetComponent<CircleCollider2D>();
        SceneManager.sceneLoaded += WaitForGame;
        hitbox = GetComponent<SpriteRenderer>();
    }

    public void SetUpGame()
    {
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
        }
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        DamageDealer damageDealer = other.GetComponent<DamageDealer>();
        InvincibilityPowerup invincibilityPowerup = other.GetComponent<InvincibilityPowerup>();
        if (damageDealer)
        {
            ProcessHit(damageDealer);
            
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
        myCollider.enabled = false;
        yield return new WaitForSeconds(duration);
        myCollider.enabled = true;
        Destroy(invincibilitySparkles.gameObject);
    }

    private void WaitForGame(Scene scene, LoadSceneMode mode)
    {
        if (scene.name == "Game")
        {
            Debug.Log("game scene");
            ResetPlayer();
            SceneManager.sceneLoaded -= WaitForGame;
        }
    }

    private void ProcessHit(DamageDealer damageDealer)
    {
        int damage = damageDealer.GetDamage();
        if (shield.IsShieldUp())
        {
            shield.ShieldsDown();
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
    }

    public void ResetPlayer()
    {
        SetUpGame();
        EnableComponents(true);
        transform.position = respawner.transform.position;
    }

    private void Die()
    {
        PlayExplosionVFX();
        EnableComponents(false);
        SetRespawnStatus(true);
        foreach (Transform child in transform)
        {
            child.gameObject.SetActive(false);
        }
        SceneManager.sceneLoaded += WaitForGame;
        FindObjectOfType<Level>().LoadGameOver();
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
