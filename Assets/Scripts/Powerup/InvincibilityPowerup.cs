using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InvincibilityPowerup : Powerup
{
    [SerializeField] float duration = 10f;
    [SerializeField] float fallSpeed = 5f;
    [SerializeField] AudioClip invincibilityPowerupSound;
    [Range(0,1)] [SerializeField] float invincibilityPowerupSoundVolume = 0.05f;

    void Start()
    {
        GetComponent<Rigidbody2D>().velocity = new Vector2(0f, -fallSpeed);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.GetComponent<Player>())
        {
            AudioSource.PlayClipAtPoint(invincibilityPowerupSound, Camera.main.transform.position, invincibilityPowerupSoundVolume);
        }
        Destroy(gameObject);
    }

    public float GetDuration()
    {
        return duration;
    }
}
