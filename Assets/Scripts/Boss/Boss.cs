using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss : MonoBehaviour
{
    [SerializeField] Transform path;
    [SerializeField] float moveDelayTime = 3f;
    [SerializeField] float moveSpeed = 3f;
    [SerializeField] GameObject deathExplosion;
    Transform leftWaypoint;
    Transform rightWaypoint;
    Transform endpoint;
    Pauser pauser;
    float moveDelayTimer = 0f;
    bool dead = false;

    // Start is called before the first frame update
    void Start()
    {
        leftWaypoint = path.GetChild(0);
        rightWaypoint = path.GetChild(1);
        endpoint = leftWaypoint;
        pauser = FindObjectOfType<Pauser>();
        StartCoroutine(WaitForDeath());
    }

    // Update is called once per frame
    void Update()
    {
        if (pauser.IsPaused() || dead) { return; }
        Move();
    }

    private void Move()
    {
        if (transform.position == leftWaypoint.position)
        {
            MoveDelayCheck(rightWaypoint);
        }
        else if (transform.position == rightWaypoint.position)
        {
            MoveDelayCheck(leftWaypoint);
        }

        transform.position = Vector2.MoveTowards(transform.position, endpoint.position, moveSpeed);
    }

    private void MoveDelayCheck(Transform endPos)
    {
        moveDelayTimer += Time.deltaTime;
        if (moveDelayTimer >= moveDelayTime)
        {
            endpoint = endPos;
            ResetMoveDelayTimer();
        }
    }

    private void Die()
    {
        GameObject[] allEnemyProjectiles = GameObject.FindGameObjectsWithTag("Enemy Projectile");
        foreach (GameObject projectile in allEnemyProjectiles)
        {
            Destroy(projectile);
        }
        Instantiate(deathExplosion, transform.position, Quaternion.identity);
        FindObjectOfType<Level>().LoadVictory();
        FindObjectOfType<Player>().Win();
    }

    private void ResetMoveDelayTimer()
    {
        moveDelayTimer = 0;
    }

    private IEnumerator WaitForDeath()
    {
        yield return new WaitUntil(() => transform.childCount == 0);
        dead = true;
        Die();
    }
}
