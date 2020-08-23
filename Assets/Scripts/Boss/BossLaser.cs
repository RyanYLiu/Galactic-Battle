using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossLaser : MonoBehaviour
{
    [SerializeField] float laserDuration = 1f;
    Animator animator;
    BoxCollider2D laserHitBox;
    [SerializeField] float laserTimer = 10f;
    [SerializeField] float timeBetweenShots = 10f;
    [SerializeField] float delayBeforeFiring = 1f;
    bool firing = false;
    [SerializeField] AudioClip laserSound;
    [Range(0,1)] [SerializeField] float laserSoundVolume = 0.2f;
    [SerializeField] AudioClip laserChargeSound;
    [Range(0,1)] [SerializeField] float laserChargeSoundVolume = 0.2f;
    SpriteRenderer spriteRenderer;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        laserHitBox = GetComponent<BoxCollider2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        laserTimer -= Time.deltaTime;
        if (laserTimer <= 0 && !firing)
        {
            StartCoroutine(FinalCharge());
            firing = true;
        }
    }

    IEnumerator FireLaser()
    {
        AudioSource.PlayClipAtPoint(laserSound, Camera.main.transform.position, laserSoundVolume);
        animator.SetBool("fireLaser", true);
        laserHitBox.enabled = true;
        yield return new WaitForSeconds(laserDuration);
        animator.SetBool("fireLaser", false);
        laserHitBox.enabled = false;
        firing = false;
        laserTimer = timeBetweenShots;
        animator.enabled = false;
        spriteRenderer.sprite = null;
    }

    IEnumerator FinalCharge()
    {
        animator.enabled = true;
        // animator.SetBool("fireLaser", false);
        AudioSource.PlayClipAtPoint(laserChargeSound, Camera.main.transform.position, laserChargeSoundVolume);
        yield return new WaitForSeconds(delayBeforeFiring);
        StartCoroutine(FireLaser());
    }
}
