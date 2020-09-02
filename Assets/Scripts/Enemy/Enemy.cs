using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [Header("Enemy")]
    [SerializeField] float baseHealth = 1;
    float health;
    [SerializeField] GameObject deathVFX;
    [SerializeField] float durationOfExplosion = 1f;
    [SerializeField] int pointValue = 100;
    [SerializeField] float powerupDropRate = 0.3f;
    [SerializeField] List<Powerup> powerupList;

    [Header("Projectile")]
    [SerializeField] protected float projectileSpeed = 10f;
    [SerializeField] float minTimeBetweenShots = 0.2f;
    [SerializeField] protected float maxTimeBetweenShots = 3f;
    protected float shotCounter = 0f;
    [SerializeField] protected GameObject laserPrefab;

    [Header("Sound Effects")]
    [SerializeField] protected AudioClip laserSound;
    [Range(0,1)] [SerializeField] protected float laserSoundVolume = 0.2f;
    [SerializeField] AudioClip deathSound;
    [Range(0,1)] [SerializeField] float deathSoundVolume = 0.2f;
    PlayerShooting playerShooting;

    // Start is called before the first frame update
    void Start()
    {
        ResetShotCounter();
        playerShooting = FindObjectOfType<PlayerShooting>();
        health = baseHealth * playerShooting.GetAttackLevel();
    }

    // Update is called once per frame
    void Update()
    {
        CountDownAndShoot();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        DamageDealer damageDealer = other.GetComponent<DamageDealer>();
        if (other.GetComponent<Player>())
        {
            Die();
            return;
        }
        else if (!damageDealer)
        {
            return;
        }
        ProcessHit(damageDealer);
    }

    private void ProcessHit(DamageDealer damageDealer)
    {
        health -= damageDealer.GetDamage();
        damageDealer.Hit();
        if (health <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        FindObjectOfType<GameSession>().AddToScore(pointValue);
        if (Random.Range(0f, 1f) < powerupDropRate)
        {
            Instantiate(powerupList[Random.Range(0, powerupList.Count)], transform.position, Quaternion.identity);
        }
        Destroy(gameObject);
        GameObject explosion = Instantiate(deathVFX, transform.position, Quaternion.identity);
        Destroy(explosion, durationOfExplosion);
        AudioSource.PlayClipAtPoint(deathSound, Camera.main.transform.position, deathSoundVolume);
    }

    private void CountDownAndShoot()
    {
        shotCounter -= Time.deltaTime;
        if (shotCounter <= 0)
        {
            Fire();
            ResetShotCounter();
        }
    }

    protected void ResetShotCounter()
    {
        shotCounter = Random.Range(minTimeBetweenShots, maxTimeBetweenShots);
    }

    private void Fire()
    {
        GameObject laser = Instantiate(
            laserPrefab,
            transform.position,
            Quaternion.identity) as GameObject;
        laser.GetComponent<Rigidbody2D>().velocity = new Vector2(0, -projectileSpeed);
        AudioSource.PlayClipAtPoint(laserSound, Camera.main.transform.position, laserSoundVolume);
    }

    public float GetHealth()
    {
        return health;
    }
}
