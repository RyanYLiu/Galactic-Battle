using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] float moveSpeed = 10f;
    [SerializeField] float defaultMoveSpeed = 10f;
    [SerializeField] float padding = 1f;
    float xMin;
    float xMax;
    float yMin;
    float yMax;
    Player player;
    
    void OnEnable()
    {
        SetUpMoveBoundaries();
        player = GetComponent<Player>();
    }

    // Update is called once per frame
    void Update()
    {
        if (player.GetRespawnStatus()) { return; }
        Move();
    }

    private void Move()
    {
        var deltaX = Input.GetAxisRaw("Horizontal") * Time.deltaTime * moveSpeed;
        var deltaY = Input.GetAxisRaw("Vertical") * Time.deltaTime * moveSpeed;
        var newXPos = Mathf.Clamp(transform.position.x + deltaX, xMin, xMax);
        var newYPos = Mathf.Clamp(transform.position.y + deltaY, yMin, yMax);
        transform.position = new Vector2(newXPos, newYPos);
    }

    private void SetUpMoveBoundaries()
    {
        Camera gameCamera = Camera.main;
        xMin = gameCamera.ViewportToWorldPoint(new Vector3(0,0,0)).x + padding;
        xMax = gameCamera.ViewportToWorldPoint(new Vector3(1,0,0)).x - padding;
        yMin = gameCamera.ViewportToWorldPoint(new Vector3(0,0,0)).y + padding;
        yMax = gameCamera.ViewportToWorldPoint(new Vector3(0,1,0)).y - padding;
    }

    public void SetMoveSpeed(float speed)
    {
        moveSpeed = speed;
    }

    public void ResetMoveSpeed()
    {
        moveSpeed = defaultMoveSpeed;
    }
}
