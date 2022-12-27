using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CelestialBodyLogic : MonoBehaviour
{
    public bool Collided = false;



    [Header("Mass")]
    public int Mass = 0;

    [Header("Orbit Settings")]
    public float MinOrbitDistance = 2;
    public float MinOrbitSpeed = 1;
    public float MaxOrbitSpeed = 2;

    public float OrbitSpacing = 1;
    public float OrbitOffsetRange = 0.20f;
    public float Cooldown = 2;
    private float _currentTime;


    private float _nextOrbitDistance;

    public int MaxOrbitingObjects = 0;




    public CelestialBodyType Type = CelestialBodyType.Astroid;
    public GravityWell _gravityWell;

    public List<GameObject> Satellites = new();

    public bool IsOrbiting = false;
    public bool IsPlayer = false;
    public bool IsPlayerControlled = false;
    // Start is called before the first frame update

    [Header("ParticleSystems")]
    [SerializeField]
    private ParticleSystem _particleSystem;

    void Start()
    {
        _currentTime = Cooldown;


        
        
        _gravityWell = gameObject.GetComponent<GravityWell>();
        
        _nextOrbitDistance = MinOrbitDistance;

    }

    // Update is called once per frame
    void Update()
    {
        _currentTime += Time.deltaTime;


        if (Type != CelestialBodyType.Astroid &&  Satellites.Count < MaxOrbitingObjects)
        {
            if (Type == CelestialBodyType.Planet)
                AddCelestialBodyToSattllites(CelestialBodyType.Astroid);

            else if (Type == CelestialBodyType.Star)
                AddCelestialBodyToSattllites(CelestialBodyType.Planet);
            else if(Type == CelestialBodyType.Blackhole)
            {
                AddCelestialBodyToSattllites(CelestialBodyType.Astroid);
                AddCelestialBodyToSattllites(CelestialBodyType.Planet);
                AddCelestialBodyToSattllites(CelestialBodyType.Star);
            }

        }



        if (Satellites.Count != 0)
        {
            SetSatelliteOrbitDistance();


            if (IsPlayerControlled && Type != CelestialBodyType.Blackhole)
            {
                if (Input.GetKeyDown(KeyCode.Space) && DoesSatellitesHaveSatellites() && Type == CelestialBodyType.Star)
                {
                    RequestSatelliteToAbsorbe();
                }
                else if (Input.GetKeyDown(KeyCode.Space))
                    AbsorbLightestSatellite();
            }
            else if(Type == CelestialBodyType.Blackhole)
            {
                if (DoesSatellitesHaveSatellites())
                {
                    RequestSatelliteToAbsorbe();
                }
                else
                    AbsorbLightestSatellite();

            }

            
            CheckSatellites();
        }
        else
        {
            _nextOrbitDistance = MinOrbitDistance;
        }

        CelestialBodyStateCheck();
    }

    private void CelestialBodyStateCheck()
    {

        if (Type == CelestialBodyType.Astroid && Mass >= EvolveHelper.PlanetMass)
        {
            Type = CelestialBodyType.Planet;
            EvolveHelper.UpdateStats(gameObject, CelestialBodyType.Planet);
        }
        else if (Type == CelestialBodyType.Planet && Mass >= EvolveHelper.StarMass)
        {
            Type = CelestialBodyType.Star;
            EvolveHelper.UpdateStats(gameObject, CelestialBodyType.Star);
        }
        else if (Type == CelestialBodyType.Star && Mass >= EvolveHelper.BlackHoleMass)
        {
            Type = CelestialBodyType.Blackhole;
            EvolveHelper.UpdateStats(gameObject, CelestialBodyType.Blackhole);
        }
        else if (Type == CelestialBodyType.Blackhole && Mass < EvolveHelper.StarMass)
        {
            Type = CelestialBodyType.Star;
            EvolveHelper.UpdateStats(gameObject, CelestialBodyType.Star);
        }
        else if (Type == CelestialBodyType.Star && Mass < EvolveHelper.PlanetMass)
        {
            Type = CelestialBodyType.Planet;
            EvolveHelper.UpdateStats(gameObject, CelestialBodyType.Planet);
        }
        else if (Type == CelestialBodyType.Planet && Mass < EvolveHelper.PlanetMass)
        {
            Type = CelestialBodyType.Astroid;
            EvolveHelper.UpdateStats(gameObject, CelestialBodyType.Astroid);
        }

    }

    [ContextMenu("Become Astroid")]
    public void Test()
    {
        EvolveHelper.UpdateStats(gameObject ,Type);// change
        Type = CelestialBodyType.Astroid;

    }



    private void RequestSatelliteToAbsorbe()
    {
        foreach (GameObject gameObject in Satellites)
        {
            CelestialBodyLogic logic = gameObject.GetComponent<CelestialBodyLogic>();
            if (logic.Satellites.Count != 0)
            {
                logic.AbsorbLightestSatellite();
                return;
            }
        }

    }

    private void AbsorbLightestSatellite()
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

    private void CheckSatellites()
    {

        foreach (GameObject Satellite in Satellites)
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
            if (IsSatelliteInRange(Satellite) && logic.Type == sattlliteType && !logic.IsPlayer)
            {
                logic.IsOrbiting = true;

                if (IsPlayer)
                    logic.IsPlayer = true;



                Satellites.Add(Satellite);
                Orbiters orbiter = Satellite.GetComponent<Orbiters>();
                orbiter.Center = transform;
                orbiter.OrbitSpeed = Random.Range(MinOrbitSpeed, MaxOrbitSpeed);

                if (logic.Type != CelestialBodyType.Astroid)
                    Satellite.GetComponent<GravityWell>().SolarObjects.Clear();



                _gravityWell.SolarObjects.Remove(Satellite);


                _currentTime = 0;
            }


        }
    }

    private bool IsSatelliteInRange(GameObject Satellite)
    {
        return Vector2.Distance(transform.position, Satellite.transform.position) < (float)_nextOrbitDistance + OrbitOffsetRange && Vector2.Distance(transform.position, Satellite.transform.position) > (float)_nextOrbitDistance - OrbitOffsetRange;
    }

    private bool DoesSatellitesHaveSatellites()
    {
        foreach (GameObject gameObject in Satellites)
        {
            if (gameObject.GetComponent<CelestialBodyLogic>().Satellites.Count != 0)
                return true;
        }
        return false;
    }

    private void SetSatelliteOrbitDistance()
    {
        float distance = MinOrbitDistance;
        for (int i = 0; i < Satellites.Count; i++)
        {
            Orbiters orbiter = Satellites[i].GetComponent<Orbiters>();
            orbiter.Distance = distance;



            distance += OrbitSpacing;
            _nextOrbitDistance = distance;
        }
    }






    private void OnCollisionEnter2D(Collision2D collision)
    {
        GameObject gameObject = collision.gameObject;
        if (gameObject.GetComponent<CelestialBodyLogic>().Type == CelestialBodyType.Astroid && Type == CelestialBodyType.Astroid && !gameObject.GetComponent<CelestialBodyLogic>().IsPlayer && !Collided)
        {
            gameObject.GetComponent<CelestialBodyLogic>().Collided = true;
            Mass += gameObject.GetComponent<CelestialBodyLogic>().Mass;

            _particleSystem.Play();
            gameObject.SetActive(false);
            Destroy(gameObject);
        }






    }
    void OnDrawGizmos()
    {


        if (Type != CelestialBodyType.Astroid)
        {
            Gizmos.color = Color.yellow;

            Gizmos.DrawWireSphere(transform.position, _nextOrbitDistance);

            //Gizmos.DrawWireSphere(transform.position, (float)_nextOrbitDistance + OrbitOffsetRange);
            //Gizmos.DrawWireSphere(transform.position, (float)_nextOrbitDistance - OrbitOffsetRange);
        }





    }
}
