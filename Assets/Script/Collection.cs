using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class Collection : MonoBehaviour
{
    [SerializeField] GameObject[] collectables;
    [SerializeField] GameObject light;
    [SerializeField] GameObject exit;
    private int found = 0;

    void Start() {
        exit.SetActive(false);
    }
    
    public void collect(GameObject g) {
        foreach(GameObject G in collectables) {
            if (G.Equals(g)) {
                found++;
                g.SetActive(false);
                break;
            }
        }

        if (found >= collectables.Length) {
            exit.SetActive(true);
            light.GetComponent<Light2D>().intensity = 0.1f;
        }
    }

}
