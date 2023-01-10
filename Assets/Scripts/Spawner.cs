using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    public BoxCollider2D SpawnArea;

    public int MaxAstroids = 10;
    public int MaxPlanet = 10;
    public int MaxStar = 10;

    public Camera PlayerCamera;
    public float Offset = 2;

    private List<GameObject> _listAstroids = new List<GameObject>();
    private List<GameObject> _listPlanet = new List<GameObject>();
    private List<GameObject> _listStar = new List<GameObject>();




    [SerializeField] private GameObject _astroid;
    [SerializeField] private GameObject _planet;
    [SerializeField] private GameObject _star;
    [SerializeField] private GameObject _blackhole;

    void Start()
    {
        SpawnObjectsOutsideOfView(_listAstroids, MaxAstroids, _astroid);
        SpawnObjectsOutsideOfView(_listPlanet, MaxPlanet, _planet);
        SpawnObjectsOutsideOfView(_listStar, MaxStar, _star);
    }

    private void Update()
    {
        CheckForDestroyedObjects(_listAstroids);
        CheckForDestroyedObjects(_listPlanet);
        CheckForDestroyedObjects(_listStar);

        SpawnObjectsOutsideOfView(_listAstroids, MaxAstroids, _astroid);
        SpawnObjectsOutsideOfView(_listPlanet, MaxPlanet, _planet);
        SpawnObjectsOutsideOfView(_listStar, MaxStar, _star);

        CheckIfGameObjecetIsInsideBounds(_listAstroids);
        CheckIfGameObjecetIsInsideBounds(_listPlanet);
        CheckIfGameObjecetIsInsideBounds(_listStar);
    }

    private void CheckForDestroyedObjects(List<GameObject> list)
    {
        var tmp = list.ToArray();

        foreach (GameObject gameObject in tmp)
        {
            if (gameObject == null)
            {
                list.Remove(gameObject);
            }
        }
    }

    private void CheckIfGameObjecetIsInsideBounds(List<GameObject> list)
    {
        foreach(GameObject gameObject in list)
        {
            Transform transform = gameObject.transform;

            Vector3 screenPoint = PlayerCamera.WorldToViewportPoint(new Vector3(transform.position.x, transform.position.y, 0));
            bool onScreen = screenPoint.x > 0 && screenPoint.x < 1 && screenPoint.y > 0 && screenPoint.y < 1;

            if (onScreen)
                continue;
            if (gameObject.GetComponent<CelestialBodyLogic>().IsOrbiting)
                continue;




            float y = transform.position.y;
            float x = transform.position.x;

            if (transform.position.y > SpawnArea.bounds.max.y)
            {
                y = transform.position.y * -1;
            }
            else if (transform.position.y < SpawnArea.bounds.min.y)
            {
                y = transform.position.y * -1;
            }

            if (transform.position.x > SpawnArea.bounds.max.x)
            {
                x = transform.position.x * -1;
            }
            else if (transform.position.x < SpawnArea.bounds.min.x)
            {
                x = transform.position.x * -1;
            }

            gameObject.GetComponent<Rigidbody2D>().position = new Vector2(x, y);






        }
    }

    private void SpawnObjectsOutsideOfView(List<GameObject> list, int amount, GameObject preFab)
    {
        while (list.Count < amount)
        {
            float x = GiveRandomPositionXInsideBounds();
            float y = GiveRandomPositionYInsideBounds();
            Vector3 screenPoint = PlayerCamera.WorldToViewportPoint(new Vector3(x, y, 0));
            bool onScreen = screenPoint.x > 0 && screenPoint.x < 1 && screenPoint.y > 0 && screenPoint.y < 1;
            bool close = CheckIfCloseToOtherObject(list, x, y);


            while (onScreen && close)
            {
                x = GiveRandomPositionXInsideBounds();
                y = GiveRandomPositionYInsideBounds();
                screenPoint = PlayerCamera.WorldToViewportPoint(new Vector3(x, y, 0));

                onScreen = screenPoint.x > 0 && screenPoint.x < 1 && screenPoint.y > 0 && screenPoint.y < 1;
                close = CheckIfCloseToOtherObject(list, x, y);
            }

            GameObject obj = Instantiate(preFab, new Vector3(x, y, 0), Quaternion.identity);
            obj.GetComponent<Rigidbody2D>().velocity = new Vector2(Random.Range(-1f, 1f), Random.Range(-1f, 1f));
            list.Add(obj);
            
        }
    }
    private bool CheckIfCloseToOtherObject(List<GameObject> list, float x , float y)
    {
        foreach(GameObject gameObject in list)
        {
            if(Vector2.Distance(new Vector2(x,y), gameObject.transform.position) < 1)
            {
                return true;
            }
        }






        return false;
    }


    private float GiveRandomPositionYInsideBounds()
    {
        return Random.Range(SpawnArea.bounds.min.y, SpawnArea.bounds.max.y);
    }

    private float GiveRandomPositionXInsideBounds()
    {
        return Random.Range(SpawnArea.bounds.min.x, SpawnArea.bounds.max.x);
    }
}
