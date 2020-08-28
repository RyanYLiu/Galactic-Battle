using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFocus : MonoBehaviour
{
    [SerializeField] float focusMoveSpeed = 3f;
    PlayerMovement movement;
    SpriteRenderer hitbox;
    Pauser pauser;
    Player player;

    void OnEnable()
    {
        movement = GetComponent<PlayerMovement>();
        player = GetComponent<Player>();
        hitbox = GetComponent<SpriteRenderer>();
        pauser = FindObjectOfType<Pauser>();
    }

    // Update is called once per frame
    void Update()
    {
        if (pauser.IsPaused() || player.GetRespawnStatus()) { return; }
        Focus();
    }

    private void Focus()
    {
        if (Input.GetButtonDown("Fire2"))
        {
            hitbox.enabled = true;
            movement.SetMoveSpeed(focusMoveSpeed);
        }
        else if (Input.GetButtonUp("Fire2"))
        {
            hitbox.enabled = false;
            movement.ResetMoveSpeed();
        }
    }
}
