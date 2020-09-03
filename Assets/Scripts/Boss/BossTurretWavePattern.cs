using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossTurretWavePattern : Enemy
{
    [SerializeField] int numPulses = 5;
    [SerializeField] float timeBetweenPulses = 0.3f;
    [SerializeField] GameObject firingDirections;
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
        for (int pulse = 0; pulse < numPulses; pulse++)
        {
            foreach (Transform child in firingDirections.transform)
            {
                GameObject laser = Instantiate(laserPrefab, transform.position, Quaternion.identity) as GameObject;
                Vector3 direction = child.transform.position - laser.transform.position;
                Vector3 normalizedDirection = direction / direction.magnitude;
                laser.GetComponent<WaveProjectile>().SetAxis(normalizedDirection);
                yield return new WaitForSeconds(timeBetweenPulses);
            }
        }
        ResetShotCounter();
    }
}
