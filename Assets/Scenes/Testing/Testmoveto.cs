using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Testmoveto : MonoBehaviour
{
    public Rigidbody2D rb;
    public Vector2 targetPosition;
    public float speed = 5f;

    private void FixedUpdate()
    {
        rb.MovePosition(Vector2.MoveTowards(rb.position, targetPosition, speed * Time.deltaTime));
    }
}
