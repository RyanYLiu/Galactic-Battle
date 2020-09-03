using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBurst : Enemy
{
    bool firingAttack = false;
    [SerializeField] int numShots = 5;
    [SerializeField] float timeBetweenShots = 0.1f;

    // Start is called before the first frame update
    void Start()
    {
        ResetShotCounter();
        health = CalculateHealth();
    }
    
    /// <summary>
    /// Update is called every frame, if the MonoBehaviour is enabled.
    /// </summary>
    void Update()
    {
        CountDownAndShoot();
    }

    private new void ResetShotCounter()
    {
        shotCounter = maxTimeBetweenShots;
        firingAttack = false;
    }

    private void CountDownAndShoot()
    {
        shotCounter -= Time.deltaTime;
        if (shotCounter <= 0 && !firingAttack)
        {
            firingAttack = true;
            StartCoroutine(Fire());
        }
    }

    IEnumerator Fire()
    {
        AudioSource.PlayClipAtPoint(laserSound, Camera.main.transform.position, laserSoundVolume);
        for (int shot = 0; shot < numShots; shot++)
        {
            GameObject laser = Instantiate(
                laserPrefab,
                transform.position,
                Quaternion.identity) as GameObject;
            laser.GetComponent<Rigidbody2D>().velocity = new Vector2(0, -projectileSpeed);
            yield return new WaitForSeconds(timeBetweenShots);
        }
        ResetShotCounter();
    }
}
