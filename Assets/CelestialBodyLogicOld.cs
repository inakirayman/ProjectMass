using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CelestialBodyLogicOld : MonoBehaviour
{
    public bool IsPlayer = false;
    public CelestialBodyType Type = CelestialBodyType.Astroid;
    public int MaxSatellitesInOrbit = 0;
    public List<GameObject> Satellites = new List<GameObject>();

    public float Orbitspeed = 10f;
    public float OrbitRadius = 2f;

    public float orbitangle;
    private Force _force;



    public bool IsOrbiting = false;

    private void Start()
    {
        _force = gameObject.GetComponent<Force>();
    }

    void Update()
    {


        if (Input.GetKeyDown(KeyCode.Q)&& Type == CelestialBodyType.Planet )
        {
            foreach(GameObject Satellite in _force.SolarObjects)
            {
                CelestialBodyLogicOld logic = Satellite.GetComponent<CelestialBodyLogicOld>();

                logic.IsOrbiting = true;
                logic.IsPlayer = true;



                float angle = Mathf.Atan2(Satellite.transform.position.y - transform.position.y, Satellite.transform.position.x - transform.position.y);
                angle *= Mathf.Rad2Deg;
               

                if (Satellite.transform.position.y < transform.position.y)
                {
                    angle -= 90;
                }
                else
                {
                    angle += 90;
                }

                if (Satellite.transform.position.x < transform.position.x)
                {
                    angle -= 180;
                }

                logic.orbitangle = angle;

                Satellites.Add(Satellite);
                _force.SolarObjects.Remove(Satellite);

            }
        }



    }


    void FixedUpdate()
    {

        if (Type == CelestialBodyType.Planet)
            foreach (GameObject Satellite in Satellites)
            {
                float angle = Satellite.GetComponent<CelestialBodyLogicOld>().orbitangle;
                angle += Orbitspeed * Time.deltaTime;

                angle %= 360;

                float x = transform.position.x + OrbitRadius * Mathf.Sin(Orbitspeed * angle);
                float y = transform.position.y + OrbitRadius * Mathf.Cos(Orbitspeed * angle);

                Vector3 orbitPosition = new Vector3(x, y, transform.position.z);

                Satellite.GetComponent<Rigidbody2D>().MovePosition(Vector2.MoveTowards(Satellite.transform.position, orbitPosition, (Orbitspeed *10) * Time.deltaTime));
                //Satellite.GetComponent<Rigidbody2D>().MovePosition(orbitPosition);
                Satellite.GetComponent<CelestialBodyLogicOld>().orbitangle = angle;

            }



    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        GameObject gameObject = collision.gameObject;
        if(gameObject.GetComponent<CelestialBodyLogicOld>().Type == CelestialBodyType.Astroid && Type == CelestialBodyType.Astroid && !gameObject.GetComponent<CelestialBodyLogicOld>().IsPlayer)
        {
            transform.localScale = transform.localScale * 1.1f;
            gameObject.SetActive(false);
        }
    }

    

}
