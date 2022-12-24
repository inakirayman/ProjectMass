using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Orbiters : MonoBehaviour
{
    public Transform Center; // The object to orbit around
    public float Distance = 10f; // The distance of the orbit
    private float _privouseDistance = 0;
    public float OrbitSpeed = 10f; // The speed of the orbit
    public float AlignSpeed = 15f;

    public bool DoOnce = false;
    private bool _absorb = false;


    private Rigidbody2D _rb; // The Rigidbody component of the object
    private float _angle; // The current angle of the orbit

    public Color GizmoColor = Color.white;



    public float lerpTime = 1f; // the time it should take to move to the target position
    private float _currentLerpTime; // a counter to track the time elapsed since 

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


        if(Distance != _privouseDistance)
        {
            _currentLerpTime = 0;
            _privouseDistance = Distance;
        }

        


    }

    
    void FixedUpdate()
    {
        _currentLerpTime += Time.deltaTime;
        if (Center != null && !_absorb)
        {
            OrbitLogic();
        }

        if ( _absorb)
        {
            float t = _currentLerpTime / 0.5f;
            _rb.MovePosition(Vector3.Lerp(transform.position, Center.position, t));

            if (_currentLerpTime > 0.5f)
                gameObject.SetActive(false);
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

        OrbitPositionLogic(orbitPosition);



        Debug.DrawLine(Center.position + orbitPosition, Center.position); 
    }

    void OrbitPositionLogic(Vector3 orbitPosition)
    {
        // Increment the counter by the elapsed time since the last frame
        
        if (_currentLerpTime > lerpTime)
        {
            // If the elapsed time exceeds the desired lerp time, set the object's position to the target position
            _rb.MovePosition(Center.position + orbitPosition);
        }
        else
        {
            // Otherwise, lerp the object's position towards the target position
            float t = _currentLerpTime / lerpTime;
            _rb.MovePosition(Vector3.Lerp(transform.position, Center.position + orbitPosition, t));
        }
    }


    public void AbsorbSatellite()
    {
        gameObject.layer = 6;
        _currentLerpTime = 0;
        _absorb = true;
    }


    void OnDrawGizmos()
    {
        // draw a wireframe sphere at the transform's position with the specified radius and color
        
        if (Center != null)
        {
            Gizmos.color = GizmoColor;
            Gizmos.DrawWireSphere(Center.position, Distance);
        }
    }
}

