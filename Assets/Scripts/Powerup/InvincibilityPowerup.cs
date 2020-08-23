using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InvincibilityPowerup : Powerup
{
    [SerializeField] float iframeDuration = 10f;
    [SerializeField] float fallSpeed = 5f;

    void Start()
    {
        GetComponent<Rigidbody2D>().velocity = new Vector2(0f, -fallSpeed);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        Player player = other.GetComponent<Player>();
        if (player)
        {
            player.StartIframes(iframeDuration);
        }
        Destroy(gameObject);
    }
}
