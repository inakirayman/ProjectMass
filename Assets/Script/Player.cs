using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public GameObject _player;


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
      

        //transform.position = transform.position + new Vector3(_movement.x * MovementSpeed * Time.deltaTime, _movement.y * MovementSpeed * Time.deltaTime, 0);

        _player.GetComponent<Rigidbody2D>().MovePosition(Vector2.Lerp(_player.transform.position, transform.position, (MovementSpeed/5) * Time.deltaTime));


        
        _rigidbody.AddForce(  new Vector2(_movement.x * MovementSpeed, _movement.y * MovementSpeed));
        //Debug.Log(_rigidbody.velocity);
    }
}
