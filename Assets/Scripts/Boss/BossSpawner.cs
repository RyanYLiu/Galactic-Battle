using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossSpawner : MonoBehaviour
{
    [SerializeField] Transform spawnPosition;
    [SerializeField] GameObject boss;

    // Start is called before the first frame update
    void Start()
    {
        Instantiate(boss, spawnPosition.position, boss.transform.rotation);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
