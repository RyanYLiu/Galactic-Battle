using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerShooting : MonoBehaviour
{
    Coroutine firingCoroutine;
    Coroutine trackingProjectileCoroutine;
    [SerializeField] AudioClip laserSound;
    [Range(0,1)] [SerializeField] float laserSoundVolume = 0.2f;
    [SerializeField] int attackLevel = 1;
    int maxLevel = 7;
    [SerializeField] List<AttackPatternConfig> attackLevelDetails;
    [SerializeField] float trackingFireDelay = 1f;
    float trackingLockoutTimer;
    Pauser pauser;
    Player player;

    void OnEnable()
    {
        pauser = FindObjectOfType<Pauser>();
        player = GetComponent<Player>();
    }

    // Update is called once per frame
    void Update()
    {
        if (player.GetRespawnStatus())
        {
            if (firingCoroutine != null)
            {
                StopCoroutine(firingCoroutine);
            }
            if (trackingProjectileCoroutine != null)
            {
                StopCoroutine(trackingProjectileCoroutine);
            }
            return;
        }

        Fire();
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        AttackPowerup attackPowerup = other.GetComponent<AttackPowerup>();
        if (attackPowerup)
        {
            LevelUpAttack(attackPowerup.GetAttackLevel());
        }
    }

    private void Fire()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            if (pauser.IsPaused()) { return; }

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
            AudioSource.PlayClipAtPoint(laserSound, Camera.main.transform.position, laserSoundVolume);
            yield return new WaitForSeconds(attackLevelDetails[attackLevel - 1].GetProjectileFiringSpeed());
        }
    }

    private void LevelUpAttack(int attackLevel)
    {
        this.attackLevel = Mathf.Clamp(this.attackLevel + attackLevel, 1, maxLevel);
    }

    public void LevelDown()
    {
        attackLevel = Mathf.Clamp(attackLevel - 1, 1, maxLevel);
    }
}
