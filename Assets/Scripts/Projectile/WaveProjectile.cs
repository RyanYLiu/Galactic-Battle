using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveProjectile : MonoBehaviour
{
    Vector3 pos;
    [SerializeField] float moveSpeed = 1f;
    [SerializeField] float frequency = 20f;
    [SerializeField] Vector3 axis;
    
    // Start is called before the first frame update
    void Start()
    {
        GetComponent<Rigidbody2D>().velocity = new Vector2(0, -2);
        pos = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        pos += axis * Time.deltaTime * moveSpeed;
        transform.position = pos + transform.right * Mathf.Sin(pos.y * frequency);
    }

    public void SetAxis(Vector3 axis)
    {
        this.axis = axis;
    }
}
