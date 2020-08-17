using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombPowerup : MonoBehaviour
{
    [SerializeField] float fallSpeed = 5f;
    [SerializeField] AudioClip bombSound;
    [Range(0,1)] [SerializeField] float bombSoundVolume = 0.05f;

    // Start is called before the first frame update
    void Start()
    {
        GetComponent<Rigidbody2D>().velocity = new Vector2(0f, -fallSpeed);
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if (other.GetComponent<Player>())
        {
            AudioSource.PlayClipAtPoint(bombSound, Camera.main.transform.position, bombSoundVolume);
        }
        Destroy(gameObject);
    }
}

