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
    private int _tolerance =5;
    public int Tolerance => _tolerance;


    



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


        if (Type != CelestialBodyType.Astroid && Satellites.Count < MaxOrbitingObjects)
        {
            if (Type == CelestialBodyType.Planet)
                AddCelestialBodyToSattllites(CelestialBodyType.Astroid);

            else if (Type == CelestialBodyType.Star)
                AddCelestialBodyToSattllites(CelestialBodyType.Planet);
            else if (Type == CelestialBodyType.Blackhole)
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
            else if (Type == CelestialBodyType.Blackhole)
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
            ClearAllSatillites();

            Type = CelestialBodyType.Star;
            EvolveHelper.UpdateStats(gameObject, CelestialBodyType.Star);
        }
        else if (Type == CelestialBodyType.Star && Mass >= EvolveHelper.BlackHoleMass)
        {

            Type = CelestialBodyType.Blackhole;
            EvolveHelper.UpdateStats(gameObject, CelestialBodyType.Blackhole);
        }
        else if (Type == CelestialBodyType.Blackhole && Mass < EvolveHelper.BlackHoleMass - _tolerance)
        {
            Type = CelestialBodyType.Star;
            EvolveHelper.UpdateStats(gameObject, CelestialBodyType.Star);
        }
        else if (Type == CelestialBodyType.Star && Mass < EvolveHelper.StarMass - _tolerance)
        {
            ClearAllSatillites();
            Type = CelestialBodyType.Planet;
            EvolveHelper.UpdateStats(gameObject, CelestialBodyType.Planet);
        }
        else if (Type == CelestialBodyType.Planet && Mass < EvolveHelper.PlanetMass - _tolerance)
        {
            ClearAllSatillites();
            Type = CelestialBodyType.Astroid;
            EvolveHelper.UpdateStats(gameObject, CelestialBodyType.Astroid);
        }

    }

    private void ClearAllSatillites()
    {
        foreach (GameObject satellite in Satellites)
        {
            satellite.GetComponent<Orbiters>().Center = null;
            satellite.GetComponent<CelestialBodyLogic>().IsOrbiting = false;
            satellite.GetComponent<CelestialBodyLogic>().IsPlayer = false;

        }
        Satellites.Clear();
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

        foreach (GameObject satellite in Satellites)
        {
            if (satellite == null)
            {
                Satellites.Remove(satellite);
            }
            else if (!satellite.activeSelf)
            {
                Satellites.Remove(satellite);
                Destroy(satellite);

            }
            else if (satellite.GetComponent<CelestialBodyLogic>().Type == Type && Type != CelestialBodyType.Blackhole)
            {
                satellite.GetComponent<Orbiters>().Center = null;
                satellite.GetComponent<CelestialBodyLogic>().IsOrbiting = false;
                satellite.GetComponent<CelestialBodyLogic>().IsPlayer = false;
                Satellites.Remove(satellite);
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

            if (Satellites[i] != null)
            {
                Orbiters orbiter = Satellites[i].GetComponent<Orbiters>();
                orbiter.Distance = distance;



                distance += OrbitSpacing;
                _nextOrbitDistance = distance;
            }


        }
    }






    private void OnCollisionEnter2D(Collision2D collision)
    {
        GameObject gameObject = collision.gameObject;
        if (Satellites.Contains(gameObject))
            return;



        CelestialBodyLogic logic = collision.gameObject.GetComponent<CelestialBodyLogic>();



        if (logic.Type == CelestialBodyType.Astroid) 
            AstroidCollisionLogic(gameObject, logic);
        else if (logic.Type == CelestialBodyType.Planet)
            PlanetCollisionLogic(gameObject, logic);
        else if (logic.Type == CelestialBodyType.Star)
            StarCollisionLogic(gameObject, logic);
        else if (logic.Type == CelestialBodyType.Blackhole)
        {
            if (logic.Mass < Mass)
            {

                Mass += logic.Mass;

                gameObject.SetActive(false);
                Destroy(gameObject);
            }

        }








    }

    private void StarCollisionLogic(GameObject gameObject, CelestialBodyLogic logic)
    {
        if (Type == CelestialBodyType.Star)
        {
            _particleSystem.Play();
            this.gameObject.GetComponent<AudioSource>().Play();
            gameObject.SetActive(false);
            Destroy(gameObject);
        }
        else if (Type == CelestialBodyType.Blackhole)
        {
            Mass += logic.Mass;

            gameObject.SetActive(false);
            Destroy(gameObject);
        }
    }

    private void PlanetCollisionLogic(GameObject gameObject, CelestialBodyLogic logic)
    {
        if (Type == CelestialBodyType.Planet)
        {
            _particleSystem.Play();
            this.gameObject.GetComponent<AudioSource>().Play();
            gameObject.SetActive(false);
            Destroy(gameObject);
        }
        else if (Type == CelestialBodyType.Star)
        {
            logic.Collided = true;
            Mass -= logic.Mass / 2;

            _particleSystem.Play();
            this.gameObject.GetComponent<AudioSource>().Play();

            gameObject.SetActive(false);
            Destroy(gameObject);
        }
        else if (Type == CelestialBodyType.Blackhole)
        {
            Mass += logic.Mass;

            gameObject.SetActive(false);
            Destroy(gameObject);
        }


    }

    private void AstroidCollisionLogic(GameObject gameObject, CelestialBodyLogic logic)
    {
        if (Type == CelestialBodyType.Astroid && !logic.IsPlayer && !Collided)
        {
            logic.Collided = true;
            Mass += logic.Mass;
            
            _particleSystem.Play();
            this.gameObject.GetComponent<AudioSource>().Play();
            gameObject.SetActive(false);
            Destroy(gameObject);
        }
        else if (Type == CelestialBodyType.Planet || Type == CelestialBodyType.Star)
        {
            logic.Collided = true;
            Mass -= logic.Mass;

            _particleSystem.Play();
            this.gameObject.GetComponent<AudioSource>().Play();
            gameObject.SetActive(false);
            Destroy(gameObject);
        }
        else if (Type == CelestialBodyType.Blackhole)
        {
            Mass += logic.Mass;

            gameObject.SetActive(false);
            Destroy(gameObject);
        }




    }

    void OnDrawGizmos()
    {


        if (Type != CelestialBodyType.Astroid && Satellites.Count != MaxOrbitingObjects)
        {
            Gizmos.color = Color.yellow;

            Gizmos.DrawWireSphere(transform.position, _nextOrbitDistance);

            //Gizmos.DrawWireSphere(transform.position, (float)_nextOrbitDistance + OrbitOffsetRange);
            //Gizmos.DrawWireSphere(transform.position, (float)_nextOrbitDistance - OrbitOffsetRange);
        }





    }

 
    

}
