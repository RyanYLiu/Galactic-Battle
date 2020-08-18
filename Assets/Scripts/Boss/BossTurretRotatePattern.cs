using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossTurretRotatePattern : Enemy
{
    [SerializeField] int numPulses = 5;
    [SerializeField] float timeBetweenPulses = 0.3f;
    [SerializeField] GameObject rotationEndpoints;
    [SerializeField] float rotationSpeed = 1f;
    bool firingAttack = false;
    Transform aimPosition;
    Transform targetPosition;
    int targetPositionIndex = 0;

    private void Start() 
    {
        ResetShotCounter();
        targetPosition = rotationEndpoints.transform.GetChild(targetPositionIndex);
        aimPosition = transform.GetChild(0);
    }
    void Update()
    {
        Aim();
        CountDownAndShoot();
    }

    private void ResetShotCounter()
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

    private void Aim()
    {
        if (aimPosition.position == targetPosition.position)
        {
            if (targetPositionIndex == 0)
            {
                targetPositionIndex = 1;
            }
            else
            {
                targetPositionIndex = 0;
            }
            targetPosition = rotationEndpoints.transform.GetChild(targetPositionIndex);
        }
        aimPosition.position = Vector2.MoveTowards
            (aimPosition.position, targetPosition.position, rotationSpeed * Time.deltaTime);
    }

    IEnumerator FireAttackPattern()
    {
        for (int pulse = 0; pulse < numPulses; pulse++)
        {
            GameObject laser = Instantiate(laserPrefab, transform.position, Quaternion.identity) as GameObject;
            Vector3 direction = aimPosition.position - laser.transform.position;
            Vector3 normalizedDirection = direction / direction.magnitude;
            laser.GetComponent<Rigidbody2D>().velocity = normalizedDirection * projectileSpeed;
            yield return new WaitForSeconds(timeBetweenPulses);
        }
        ResetShotCounter();
    }
}
