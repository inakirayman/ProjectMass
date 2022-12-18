using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CelestialBodyLogic : MonoBehaviour
{
    [Header("Mass")]
    public int Mass = 0;

    public CelestialBodyType Type = CelestialBodyType.Astroid;
    public GravityWell _gravityWell;

    public List<GameObject> Satellites = new List<GameObject>();

    public bool IsOrbiting = false;
    public bool IsPlayer = false;

    // Start is called before the first frame update



    void Start()
    {
        if(Type != CelestialBodyType.Astroid)
        {
            _gravityWell = gameObject.GetComponent<GravityWell>();
        }
        
    }

    // Update is called once per frame
    void Update()
    {

        if (Input.GetKeyDown(KeyCode.Q) && Type == CelestialBodyType.Planet)
        {
            foreach (GameObject Satellite in _gravityWell.SolarObjects)
            {
                CelestialBodyLogic logic = Satellite.GetComponent<CelestialBodyLogic>();

                logic.IsOrbiting = true;
                logic.IsPlayer = true;

               

                Satellites.Add(Satellite);
                Orbiters orbiter = Satellite.GetComponent<Orbiters>();
                orbiter.Center = transform;
                orbiter.OrbitSpeed = Random.Range(1, 5);



                _gravityWell.SolarObjects.Remove(Satellite);

            }
        }


        int distance = 2;
        for(int i = 0; i < Satellites.Count; i++) 
        {
            Orbiters orbiter = Satellites[i].GetComponent<Orbiters>();
            orbiter.Distance = distance;
           


            distance += 2;
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
}
