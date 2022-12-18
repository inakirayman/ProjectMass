using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateAroundCenter : MonoBehaviour
{


    public Transform pivot; // the object to rotate around
    public float speed = 10f; // the speed at which to rotate
    private Rigidbody2D rb;

    void FixedUpdate()
    {
        // Calculate the new position for the object
        Vector2 direction = ((Vector2)pivot.position - (Vector2)transform.position).normalized;
        Vector2 newPosition = (Vector2)transform.position + direction * speed * Time.fixedDeltaTime;
        // Update the object's position
        GetComponent<Rigidbody2D>().MovePosition(newPosition);
        // Rotate the object to face the center point
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        GetComponent<Rigidbody2D>().MoveRotation(angle);
    }


}
