using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShieldPowerup : Powerup
{
    [SerializeField] float fallSpeed = 5f;
    [SerializeField] AudioClip shieldsUpSound;
    [Range(0,1)] [SerializeField] float shieldsUpSoundVolume = 0.05f;

    // Start is called before the first frame update
    void Start()
    {
        GetComponent<Rigidbody2D>().velocity = new Vector2(0f, -fallSpeed);
    }

    private void OnTriggerEnter2D(Collider2D other) {
        Player player = other.GetComponent<Player>();
        if (player)
        {
            AudioSource.PlayClipAtPoint(shieldsUpSound, Camera.main.transform.position, shieldsUpSoundVolume);
        }
        Destroy(gameObject);
    }
}
