using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackPowerup : Powerup
{
    [SerializeField] int attackLevel = 1;
    [SerializeField] float fallSpeed = 5f;
    [SerializeField] AudioClip attackPowerupSound;
    [Range(0,1)] [SerializeField] float attackPowerupSoundVolume = 0.05f;

    // Start is called before the first frame update
    void Start()
    {
        GetComponent<Rigidbody2D>().velocity = new Vector2(0f, -fallSpeed);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public int GetAttackLevel()
    {
        return attackLevel;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.GetComponent<Player>())
        {
            AudioSource.PlayClipAtPoint(attackPowerupSound, Camera.main.transform.position, attackPowerupSoundVolume);
        }
        Destroy(gameObject);
    }
}
