using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Respawner : MonoBehaviour
{
    [SerializeField] float respawnTime = 2f;
    Player player;
    Pauser pauser;

    void Start()
    {
        pauser = FindObjectOfType<Pauser>();
        player = FindObjectOfType<Player>();
    }

    IEnumerator RespawnTimer()
    {
        yield return new WaitForSeconds(respawnTime);
        player.transform.position = transform.position;
        player.GetComponent<CircleCollider2D>().enabled = true;
        player.SetRespawnStatus(false);
        foreach (Transform child in player.transform)
        {
            child.gameObject.SetActive(true);
        }
        player.StartInvincibility(player.GetInvincibilityDuration());
    }

    public void StartRespawn()
    {
        StartCoroutine(RespawnTimer());
    }
}
