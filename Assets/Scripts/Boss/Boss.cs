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
    }

    // Update is called once per frame
    void Update()
    {
        Move();
    }

    private void Move()
    {
        if (transform.position == leftWaypoint.position)
        {
            StartCoroutine(MoveDelay(rightWaypoint));
        }
        else if (transform.position == rightWaypoint.position)
        {
            StartCoroutine(MoveDelay(leftWaypoint));
        }

        transform.position = Vector2.MoveTowards(transform.position, endpoint.position, moveSpeed);
    }

    private void Die()
    {

    }

    IEnumerator MoveDelay(Transform waypoint)
    {
        yield return new WaitForSeconds(moveDelayTime);
        endpoint = waypoint;
    }
}
