using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RigidTest : MonoBehaviour
{
    Rigidbody2D rb;
    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        rb.AddForce(transform.up * 1);
    }
}
