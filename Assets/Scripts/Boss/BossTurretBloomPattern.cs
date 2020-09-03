using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossTurretBloomPattern : Enemy
{
    [SerializeField] int minNumPulses = 5;
    [SerializeField] int maxNumPulses = 5;
    [SerializeField] float timeBetweenPulses = 0.3f;
    [SerializeField] GameObject firingDirections;
    [SerializeField] float angleToRotate = 10f;
    bool firingAttack = false;

    private void Start() 
    {
        ResetShotCounter();
        health = CalculateHealth();
    }
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
            StartCoroutine(FireAttackPattern());
        }
    }

    IEnumerator FireAttackPattern()
    {
        for (int pulse = 0; pulse < Random.Range(minNumPulses, maxNumPulses); pulse++)
        {
            foreach (Transform child in firingDirections.transform)
            {
                GameObject laser = Instantiate(laserPrefab, transform.position, Quaternion.identity) as GameObject;
                child.transform.RotateAround(transform.position, Vector3.forward, angleToRotate);
                Vector3 direction = child.transform.position - laser.transform.position;
                Vector3 normalizedDirection = direction / direction.magnitude;
                laser.GetComponent<Rigidbody2D>().velocity = normalizedDirection * projectileSpeed;
            }
            yield return new WaitForSeconds(timeBetweenPulses);
        }
        ResetShotCounter();
    }
}
