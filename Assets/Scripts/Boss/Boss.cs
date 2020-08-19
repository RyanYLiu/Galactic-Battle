using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss : MonoBehaviour
{
    float totalHealth;

    // Start is called before the first frame update
    void Start()
    {
        foreach (Transform child in transform)
        {
            totalHealth += child.gameObject.GetComponent<Enemy>().GetHealth();
        }
    }

    // Update is called once per frame
    void Update()
    {
        Move();
    }

    private void Move()
    {
        
    }
}
