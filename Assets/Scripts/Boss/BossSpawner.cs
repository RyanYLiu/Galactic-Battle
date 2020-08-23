using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossSpawner : MonoBehaviour
{
    [SerializeField] Transform spawnPosition;
    [SerializeField] GameObject boss;

    public void SpawnBoss()
    {
        Instantiate(boss, spawnPosition.position, boss.transform.rotation);
    }
}
