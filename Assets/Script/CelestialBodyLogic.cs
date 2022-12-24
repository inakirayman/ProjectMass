using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CelestialBodyLogic : MonoBehaviour
{
    [Header("Mass")]
    public int Mass = 0;
    public int MinOrbitDistance = 2;
    public float OrbitOffsetRange = 0.20f;
    public float Cooldown = 2;
    private float _currentTime;


    private int _nextOrbitDistance;

    public int MaxOrbitingObjects = 0;




    public CelestialBodyType Type = CelestialBodyType.Astroid;
    public GravityWell _gravityWell;

    public List<GameObject> Satellites = new();

    public bool IsOrbiting = false;
    public bool IsPlayer = false;

    // Start is called before the first frame update



    void Start()
    {
        if(Type != CelestialBodyType.Astroid)
        {
            _gravityWell = gameObject.GetComponent<GravityWell>();
        }
        _nextOrbitDistance = MinOrbitDistance;
        
    }

    // Update is called once per frame
    void Update()
    {
        _currentTime += Time.deltaTime;


        if (Type != CelestialBodyType.Astroid)
        {
            if (Type == CelestialBodyType.Planet)
                AddCelestialBodyToSattllites(CelestialBodyType.Astroid);
            else if (Type == CelestialBodyType.Star)
                AddCelestialBodyToSattllites(CelestialBodyType.Planet);
            else if (Type == CelestialBodyType.Blackhole)
                AddCelestialBodyToSattllites(CelestialBodyType.Star);



        }

       

        if (Satellites.Count != 0)
        {
            SetSatelliteOrbitDistance();
            AbsorbLightestSatellite();
            CheckSatellites();
        }
        else
        {
            _nextOrbitDistance = MinOrbitDistance;
        }


    }

    private void AbsorbLightestSatellite()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            GameObject selected = Satellites[0];

            foreach (GameObject Satellite in Satellites)
            {
                CelestialBodyLogic logic = Satellite.GetComponent<CelestialBodyLogic>();
                if (selected.GetComponent<CelestialBodyLogic>().Mass > logic.Mass)
                {
                    selected = Satellite;
                }
            }
            Mass += selected.GetComponent<CelestialBodyLogic>().Mass;
            selected.GetComponent<Orbiters>().AbsorbSatellite();
            Satellites.Remove(selected);

        }
    }

    private void CheckSatellites()
    {

       foreach(GameObject Satellite in Satellites)
       {
            if (!Satellite.activeSelf)
            {
                Satellites.Remove(Satellite);
                Destroy(Satellite);

            }
       }


    }

    private void AddCelestialBodyToSattllites(CelestialBodyType sattlliteType)
    {
        if (!(_currentTime >= Cooldown))
        {
            return;
        }
           

        GameObject[] tempArray = _gravityWell.SolarObjects.ToArray();

        foreach (GameObject Satellite in tempArray)
        {
            CelestialBodyLogic logic = Satellite.GetComponent<CelestialBodyLogic>();
            if (IsSatellitInRange(Satellite) && logic.Type == sattlliteType)
            {
                logic.IsOrbiting = true;
                logic.IsPlayer = true;



                Satellites.Add(Satellite);
                Orbiters orbiter = Satellite.GetComponent<Orbiters>();
                orbiter.Center = transform;
                orbiter.OrbitSpeed = Random.Range(1, 5);



                _gravityWell.SolarObjects.Remove(Satellite);


                _currentTime = 0;
            }


        }
    }

    private bool IsSatellitInRange(GameObject Satellite)
    {
        return Vector2.Distance(transform.position, Satellite.transform.position) < (float)_nextOrbitDistance + OrbitOffsetRange && Vector2.Distance(transform.position, Satellite.transform.position) > (float)_nextOrbitDistance - OrbitOffsetRange;
    }

    private void SetSatelliteOrbitDistance()
    {
        int distance = MinOrbitDistance;
        for (int i = 0; i < Satellites.Count; i++)
        {
            Orbiters orbiter = Satellites[i].GetComponent<Orbiters>();
            orbiter.Distance = distance;



            distance += 1;
            _nextOrbitDistance = distance;
        }
    }






    private void OnCollisionEnter2D(Collision2D collision)
    {
        GameObject gameObject = collision.gameObject;
        if (gameObject.GetComponent<CelestialBodyLogic>().Type == CelestialBodyType.Astroid && Type == CelestialBodyType.Astroid && !gameObject.GetComponent<CelestialBodyLogic>().IsPlayer)
        {
            transform.localScale = transform.localScale * 1.1f;
            gameObject.SetActive(false);
        }


    }
    void OnDrawGizmos()
    {


        //if(Type != CelestialBodyType.Astroid)
        //{
        //    Gizmos.color = Color.yellow;
        //    Gizmos.DrawWireSphere(transform.position, (float)_nextOrbitDistance + OrbitOffsetRange);
        //    Gizmos.DrawWireSphere(transform.position, (float)_nextOrbitDistance - OrbitOffsetRange);
        //}
 




    }
}
