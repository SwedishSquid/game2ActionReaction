using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float thrust = 1;
    public float rotationSpeed = 1;
    Rigidbody2D rb;
    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    public void Move(Vector2 direction)
    {
        var forwardComponent = direction.y;
        var rotationComponent = direction.x;
        rb.AddForce((transform.up * forwardComponent));
        rb.AddTorque(-1 * rotationSpeed * rotationComponent);
    }
}
