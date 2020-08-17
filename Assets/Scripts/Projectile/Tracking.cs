using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tracking : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] float trackSpeed = 10f;
    Enemy enemy;

    void Start()
    {
        // find any enemy
        enemy = FindObjectOfType<Enemy>();
    }

    // Update is called once per frame
    void Update()
    {
        if (enemy)
        {
            var movementThisFrame = trackSpeed * Time.deltaTime;
            transform.position = Vector2.MoveTowards
                (transform.position, enemy.transform.position, movementThisFrame);
        }
        else
        {
            enemy = FindObjectOfType<Enemy>();
            
        }
    }
}
