using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Orbiters : MonoBehaviour
{

   

    public Transform Center; // The object to orbit around
    public float Distance = 10f; // The distance of the orbit
    public float OrbitSpeed = 10f; // The speed of the orbit
    public float AlignSpeed = 15f;

    public bool DoOnce = false;
    


    private bool IsAligned = false;
    private Rigidbody2D _rb; // The Rigidbody component of the object
    private float _angle; // The current angle of the orbit

   

    void Start()
    {
        _rb = GetComponent<Rigidbody2D>();

        if (Center != null)
            SetAngleRelativeToCenter();
    }

    private void SetAngleRelativeToCenter()
    {
        // Calculate the initial angle based on the object's position relative to the center
        Vector3 relativePosition = transform.position - Center.position;
        _angle = Mathf.Atan2(relativePosition.x, relativePosition.y);
    }

    private void Update()
    {
        if (Center != null && !DoOnce)
        {
            SetAngleRelativeToCenter();
            DoOnce = true;
        }
        if (Center == null && DoOnce)
        {
            DoOnce = false;
        }





    }

    
    void FixedUpdate()
    {

        if (Center != null)
        {
            OrbitLogic();
        }

    }

    private void OrbitLogic()
    {
        // Increment the angle by the orbit speed
        _angle += OrbitSpeed * Time.deltaTime;

        // Calculate the orbit position
        float x = Mathf.Sin(_angle) * Distance;
        float y = Mathf.Cos(_angle) * Distance;
        Vector3 orbitPosition = new Vector3(x, y, 0);

        _rb.MovePosition(Vector2.MoveTowards(transform.position, Center.position + orbitPosition, (OrbitSpeed * AlignSpeed) * Time.deltaTime));
        
        //_rb.MovePosition(Center.position + orbitPosition);


        Debug.DrawLine(Center.position + orbitPosition, Center.position); 
    }

    

}

