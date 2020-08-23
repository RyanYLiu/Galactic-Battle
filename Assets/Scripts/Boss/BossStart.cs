using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossStart : MonoBehaviour
{
    [SerializeField] Transform endPosition;
    [SerializeField] float moveSpeed = 10f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (transform.position == endPosition.position)
        {
            foreach (Transform child in transform)
            {
                child.gameObject.SetActive(true);
            }
            GetComponent<Boss>().enabled = true;
            GetComponent<BossStart>().enabled = false;
        }
        transform.position = Vector2.MoveTowards(transform.position, endPosition.position, moveSpeed);
    }
}
