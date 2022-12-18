using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [Header("Movement")]
    public float MovementSpeed = 12f;
    public CelestialBodyType Type = CelestialBodyType.Planet;

    


    private Vector2 _movement;
    private Rigidbody2D _rigidbody;

    // Start is called before the first frame update
    void Start()
    {
        _rigidbody = gameObject.GetComponent<Rigidbody2D>();
        
    }

    // Update is called once per frame
    void Update()
    {
        _movement = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical")).normalized;
        
    }

    private void FixedUpdate()
    {
        _rigidbody.velocity = new Vector2(_movement.x * MovementSpeed, _movement.y * MovementSpeed);
        Debug.Log(_rigidbody.velocity);
    }
}
