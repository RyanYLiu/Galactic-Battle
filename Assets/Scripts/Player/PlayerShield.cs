using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerShield : MonoBehaviour
{
    [SerializeField] bool shield = false;
    [SerializeField] GameObject shieldSprite;
    [SerializeField] float shieldOffset = 10f;
    GameObject shieldVFX;
    ShieldDisplay shieldDisplay;
    [SerializeField] AudioClip shieldsDownSound;
    [Range(0,1)] [SerializeField] float shieldsDownSoundVolume = 0.05f;

    void OnEnable()
    {
        shieldDisplay = FindObjectOfType<ShieldDisplay>();
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        ShieldPowerup shieldPowerup = other.GetComponent<ShieldPowerup>();
        if (shieldPowerup)
        {
            shield = true;
            shieldVFX = Instantiate(shieldSprite, transform.position + new Vector3(0, shieldOffset, 0), Quaternion.identity);
            shieldVFX.transform.SetParent(transform);
            shieldVFX.transform.localScale = new Vector3(1,1,1);
            shieldDisplay.AddShield();
        }
    }

    public void ShieldsDown()
    {
        shield = false;
        shieldDisplay.RemoveShield();
        Destroy(shieldVFX.gameObject);
        AudioSource.PlayClipAtPoint(shieldsDownSound, Camera.main.transform.position, shieldsDownSoundVolume);
    }

    public bool IsShieldUp()
    {
        return shield;
    }
}
