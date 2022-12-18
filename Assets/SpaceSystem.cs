using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpaceSystem : MonoBehaviour
{
    public readonly float G = 100f;
    GameObject[] SolarObjects;

    void Start()
    {
        SolarObjects = GameObject.FindGameObjectsWithTag("Celestials");
    }

    // Update is called once per frame
    void Update()
    {
        Gravity();
    }


    void Gravity()
    {
        foreach(GameObject a in SolarObjects)
        {
            foreach(GameObject b in SolarObjects)
            {
                if (!a.Equals(b))
                {
                    float m1 = a.GetComponent<Rigidbody2D>().mass;
                    float m2 = b.GetComponent<Rigidbody2D>().mass;
                    float r = Vector2.Distance(a.transform.position, b.transform.position);

                    a.GetComponent<Rigidbody2D>().AddForce((b.transform.position -a.transform.position).normalized * (G * (m1 *m2) / (r * r)));
                }
            }
        }
    }


 


}
