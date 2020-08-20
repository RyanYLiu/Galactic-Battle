using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyShotgun : Enemy
{
    [SerializeField] float spread = 3;

    void Update()
    {
        CountDownAndShoot();
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

    private void Fire()
    {
        GameObject laser = Instantiate(
            laserPrefab,
            transform.position,
            Quaternion.identity) as GameObject;
        GameObject laserRight = Instantiate(
            laserPrefab,
            transform.position,
            Quaternion.identity) as GameObject;
        GameObject laserLeft = Instantiate(
            laserPrefab,
            transform.position,
            Quaternion.identity) as GameObject;
        laser.GetComponent<Rigidbody2D>().velocity = new Vector2(0, -projectileSpeed);
        laserRight.GetComponent<Rigidbody2D>().velocity = new Vector2(-projectileSpeed/spread, -projectileSpeed);
        laserLeft.GetComponent<Rigidbody2D>().velocity = new Vector2(projectileSpeed/spread, -projectileSpeed);
        AudioSource.PlayClipAtPoint(laserSound, Camera.main.transform.position, laserSoundVolume);
    }
}
