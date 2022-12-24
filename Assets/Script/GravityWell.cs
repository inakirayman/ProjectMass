using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GravityWell : MonoBehaviour
{
    public readonly float G = 100f;

    public List<GameObject> SolarObjects = new List<GameObject>();

    private CircleCollider2D _circleCollider2D;

    private void Start()
    {
        CircleCollider2D[] circleCollider2D = GetComponents<CircleCollider2D>();
        foreach (CircleCollider2D collider2D in circleCollider2D)
            if (collider2D.isTrigger)
            {
                _circleCollider2D = collider2D;
                break;
            }
    }

    private void FixedUpdate()
    {
       
        Gravity();
    }


    void Gravity()
    {


        foreach (GameObject b in SolarObjects)
        {
            if (b.GetComponent<CelestialBodyLogic>().IsOrbiting)
                return;


            float m1 = gameObject.GetComponent<Rigidbody2D>().mass;
            float m2 = b.GetComponent<Rigidbody2D>().mass;
            float r = Vector2.Distance(gameObject.transform.position, b.transform.position);

            b.GetComponent<Rigidbody2D>().AddForce((gameObject.transform.position - b.transform.position).normalized * (G * (m1 * m2) / (r * r)));

        }

    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<CelestialBodyLogic>().Type == CelestialBodyType.Astroid && !collision.GetComponent<CelestialBodyLogic>().IsOrbiting)
            SolarObjects.Add(collision.gameObject);
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        SolarObjects.Remove(collision.gameObject);
    }

    void OnDrawGizmos()
    {

        if (_circleCollider2D != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, _circleCollider2D.radius);
        }
       


    }





}
