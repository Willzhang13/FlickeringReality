using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collector : MonoBehaviour
{
    [SerializeField] Collection collection;

    void OnTriggerEnter2D(Collider2D c)
    {
        if (c.gameObject.tag == "Collectable") {
            collection.collect(c.gameObject);
        }
    }
}
