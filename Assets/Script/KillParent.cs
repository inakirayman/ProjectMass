using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KillParent : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        List<Transform> list = new();

        for (int i = 0; i < transform.childCount; i++)
        {
            list.Add(transform.GetChild(i));
        }

        foreach (Transform transform in list)
            transform.parent = null;

        Destroy(gameObject);

    }

    // Update is called once per frame

}
