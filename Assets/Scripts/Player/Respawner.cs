using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Respawner : MonoBehaviour
{
    [SerializeField] float respawnTime = 2f;

    IEnumerator RespawnTimer()
    {
        Player player = FindObjectOfType<Player>();
        yield return new WaitForSeconds(respawnTime);
        player.transform.position = transform.position;
        player.gameObject.SetActive(true);
        player.StartIframes(player.GetIframeDuration());
    }

    public void StartRespawn()
    {
        StartCoroutine(RespawnTimer());
    }
}
