using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss : MonoBehaviour
{
    float totalHealth;
    [SerializeField] Transform path;
    [SerializeField] float moveDelayTime = 3f;
    [SerializeField] float moveSpeed = 3f;
    Transform leftWaypoint;
    Transform rightWaypoint;
    Transform endpoint;
    Pauser pauser;
    float moveDelayTimer = 0f;

    // Start is called before the first frame update
    void Start()
    {
        foreach (Transform child in transform)
        {
            totalHealth += child.gameObject.GetComponent<Enemy>().GetHealth();
        }
        leftWaypoint = path.GetChild(0);
        rightWaypoint = path.GetChild(1);
        endpoint = leftWaypoint;
        pauser = FindObjectOfType<Pauser>();
    }

    // Update is called once per frame
    void Update()
    {
        if (pauser.IsPaused()) { return; }
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

    private void ResetMoveDelayTimer()
    {
        moveDelayTimer = 0;
    }

    private void Die()
    {

    }
}
