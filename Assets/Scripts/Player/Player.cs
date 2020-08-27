using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour
{
    // config params
    [Header("Player")]
    [SerializeField] float moveSpeed = 10f;
    [SerializeField] float focusMoveSpeed = 3f;
    [SerializeField] float normalMoveSpeed = 10f;
    [SerializeField] float padding = 1f;
    [SerializeField] int lifeCount = 3;
    [SerializeField] int maxLifeCount = 3;
    [SerializeField] GameObject deathVFX;
    [SerializeField] float durationOfExplosion = 1f;
    [SerializeField] bool shield = false;
    [SerializeField] int bombCount = 3;
    [SerializeField] int maxBombCount = 3;
    [SerializeField] GameObject iframeVFX;
    [SerializeField] float iframeDuration = 2f;
    bool invincible = false;
    [SerializeField] GameObject shieldSprite;
    [SerializeField] float shieldOffset = 10f;
    [SerializeField] int attackLevel = 1;
    int maxLevel = 7;
    [SerializeField] List<AttackPatternConfig> attackLevelDetails;
    [SerializeField] float trackingFireDelay = 1f;
    float trackingLockoutTimer;

    [Header("Sound Effects")]
    [SerializeField] AudioClip deathSound;
    [Range(0,1)] [SerializeField] float deathSoundVolume = 0.2f;
    [SerializeField] AudioClip laserSound;
    [Range(0,1)] [SerializeField] float laserSoundVolume = 0.2f;
    [SerializeField] AudioClip shieldsDownSound;
    [Range(0,1)] [SerializeField] float shieldsDownSoundVolume = 0.05f;
    [SerializeField] AudioClip bombUseSound;
    [Range(0,1)] [SerializeField] float bombUseSoundVolume = 0.05f;
    [SerializeField] AudioClip iframePowerupSound;
    [Range(0,1)] [SerializeField] float iframePowerupSoundVolume = 0.05f;
    Pauser pauser;


    GameObject shieldVFX;
    Coroutine firingCoroutine;
    Coroutine trackingProjectileCoroutine;
    LifeDisplay lifeDisplay;
    ShieldDisplay shieldDisplay;
    Respawner respawner;
    Collider2D myCollider;
    SpriteRenderer hitBox;
    PlayerBombs bombs;

    float xMin;
    float xMax;
    float yMin;
    float yMax;

    // Start is called before the first frame update
    void Start()
    {
        SetupSingleton();
        SetUpMoveBoundaries();
        bombs = GetComponent<PlayerBombs>();
        // SceneManager.sceneLoaded += WaitForGame;
        myCollider = GetComponent<Collider2D>();
        hitBox = GetComponent<SpriteRenderer>();
    }

    public void SetupPlayerUI()
    {
        lifeDisplay = FindObjectOfType<LifeDisplay>();
        shieldDisplay = FindObjectOfType<ShieldDisplay>();
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
        if (IsGameScene())
        {
            Focus();
            Move();
            Fire();
        }
    }

    private bool IsGameScene()
    {
        return SceneManager.GetActiveScene().name == "Game";
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        DamageDealer damageDealer = other.GetComponent<DamageDealer>();
        ShieldPowerup shieldPowerup = other.GetComponent<ShieldPowerup>();
        BombPowerup bombPowerup = other.GetComponent<BombPowerup>();
        AttackPowerup attackPowerup = other.GetComponent<AttackPowerup>();
        if (damageDealer)
        {
            if (!invincible)
            {
                ProcessHit(damageDealer);
            }
            
            if (other.GetComponent<BossLaser>())
            {
                return;
            }
            
            damageDealer.Hit();
        }
        else if (shieldPowerup)
        {
            shield = true;
            shieldVFX = Instantiate(shieldSprite, transform.position + new Vector3(0, shieldOffset, 0), Quaternion.identity);
            shieldVFX.transform.SetParent(transform);
            shieldVFX.transform.localScale = new Vector3(1,1,1);
            shieldDisplay.AddShield();
        }
        else if (bombPowerup)
        {
            bombDisplay.AddBomb();
            bombCount = Mathf.Clamp(bombCount + 1, 0, maxBombCount);
        }
        else if (attackPowerup)
        {
            LevelUpAttack(attackPowerup);
        }
    }

    public void StartIframes(float duration)
    {
        StartCoroutine(Invincibility(duration));
    }

    IEnumerator Invincibility(float duration)
    {
        GameObject iframeSparkles = Instantiate(iframeVFX, transform.position, Quaternion.identity);
        iframeSparkles.transform.SetParent(transform);
        invincible = true;
        yield return new WaitForSeconds(duration);
        invincible = false;
        Destroy(iframeSparkles.gameObject);
    }

    // private void WaitForGame(Scene scene, LoadSceneMode mode)
    // {
    //     if (IsGameScene())
    //     {
    //         Debug.Log("delegate waitforgame");
    //         ResetPlayer();
    //         SceneManager.sceneLoaded -= WaitForGame;
    //     }
    // }

    private void ProcessHit(DamageDealer damageDealer)
    {
        int damage = damageDealer.GetDamage();
        if (shield)
        {
            shield = false;
            shieldDisplay.RemoveShield();
            Destroy(shieldVFX.gameObject);
            PlaySFX(shieldsDownSound, shieldsDownSoundVolume);
        }
        else
        {
            lifeCount -= damageDealer.GetDamage();
            lifeDisplay.SubtractLife();
            if (lifeCount <= 0)
            {
                Die();
            }
            else
            {
                PlayExplosionVFX();
                bombs.ClearScreen();
                attackLevel = Mathf.Clamp(attackLevel - 1, 0, maxLevel);
                respawner.StartRespawn();
                hitBox.enabled = false;
                moveSpeed = normalMoveSpeed;
                gameObject.SetActive(false);
            }
        }
    }

    public void ResetPlayer()
    {
        bombCount = maxBombCount;
        lifeCount = maxLifeCount;
        shield = false;
        invincible = false;
        attackLevel = 1;
        SetupPlayerUI();
        for (int lifeCounter = 0; lifeCounter < lifeCount; lifeCounter++)
        {
            lifeDisplay.AddLife();
        }
        transform.position = respawner.transform.position;
    }

    private void LevelUpAttack(AttackPowerup attackPowerup)
    {
        attackLevel = Mathf.Clamp(attackLevel + attackPowerup.GetAttackLevel(), 0 , maxLevel);
    }

    private void Die()
    {
        PlayExplosionVFX();
        // SceneManager.sceneLoaded += WaitForGame;
        gameObject.SetActive(false);
        FindObjectOfType<Level>().LoadGameOver();
    }

    private void PlayExplosionVFX()
    {
        PlaySFX(deathSound, deathSoundVolume);
        GameObject explosion = Instantiate(deathVFX, transform.position, Quaternion.identity);
        Destroy(explosion, durationOfExplosion);
    }

    private void Fire()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            firingCoroutine = StartCoroutine(FireContinously());
            if (attackLevel == maxLevel)
            {
                trackingProjectileCoroutine = StartCoroutine(FireTrackingProjectileContinuously());
            }
        }
        if (Input.GetButtonUp("Fire1"))
        {
            StopCoroutine(firingCoroutine);
            if (trackingProjectileCoroutine != null)
            {
                StopCoroutine(trackingProjectileCoroutine);
            }
        }
    }

    private void PlaySFX(AudioClip audioClip, float volume)
    {
        AudioSource.PlayClipAtPoint(audioClip, Camera.main.transform.position, volume);
    }

    IEnumerator FireTrackingProjectileContinuously()
    {
        AttackPatternConfig trackingProjectileConfig = attackLevelDetails[attackLevelDetails.Count - 1];
        GameObject laserPrefab = trackingProjectileConfig.GetLaser();
        while (true)
        {
            GameObject laser = Instantiate(
                laserPrefab,
                transform.position + trackingProjectileConfig.GetLaserOffset(),
                trackingProjectileConfig.GetFiringAngle());
            laser.GetComponent<Rigidbody2D>().velocity = trackingProjectileConfig.GetVelocity();
            yield return new WaitForSeconds(trackingFireDelay);
        }
    }

    IEnumerator FireContinously()
    {
        while (true)
        {
            for (int projectileCounter = 0; projectileCounter <= attackLevel - 1; projectileCounter++)
            {
                AttackPatternConfig attackPatternConfig =  attackLevelDetails[projectileCounter];
                GameObject laserPrefab = attackPatternConfig.GetLaser();
                if (!laserPrefab) continue;
                GameObject laser = Instantiate(
                    laserPrefab,
                    transform.position + attackPatternConfig.GetLaserOffset(),
                    attackPatternConfig.GetFiringAngle());
                laser.GetComponent<Rigidbody2D>().velocity = attackPatternConfig.GetVelocity();
            }
            PlaySFX(laserSound, laserSoundVolume);
            yield return new WaitForSeconds(attackLevelDetails[attackLevel - 1].GetProjectileFiringSpeed());
        }
    }

    private void Move()
    {
        var deltaX = Input.GetAxisRaw("Horizontal") * Time.deltaTime * moveSpeed;
        var deltaY = Input.GetAxisRaw("Vertical") * Time.deltaTime * moveSpeed;
        var newXPos = Mathf.Clamp(transform.position.x + deltaX, xMin, xMax);
        var newYPos = Mathf.Clamp(transform.position.y + deltaY, yMin, yMax);
        transform.position = new Vector2(newXPos, newYPos);
    }

    private void SetUpMoveBoundaries()
    {
        Camera gameCamera = Camera.main;
        xMin = gameCamera.ViewportToWorldPoint(new Vector3(0,0,0)).x + padding;
        xMax = gameCamera.ViewportToWorldPoint(new Vector3(1,0,0)).x - padding;
        yMin = gameCamera.ViewportToWorldPoint(new Vector3(0,0,0)).y + padding;
        yMax = gameCamera.ViewportToWorldPoint(new Vector3(0,1,0)).y - padding;
    }

    private void Focus()
    {
        if (Input.GetButtonDown("Fire2"))
        {
            hitBox.enabled = true;
            moveSpeed = focusMoveSpeed;
        }
        else if (Input.GetButtonUp("Fire2"))
        {
            hitBox.enabled = false;
            moveSpeed = normalMoveSpeed;
        }
    }

    public int GetLife()
    {
        return lifeCount;
    }

    public int GetBombs()
    {
        return bombCount;
    }

    public float GetIframeDuration()
    {
        return iframeDuration;
    }
}
