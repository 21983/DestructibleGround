using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InstantiateBomb : MonoBehaviour {

    [SerializeField]
    private GameObject Bomb;

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector3 pos = new Vector3(Camera.main.ScreenToWorldPoint(Input.mousePosition).x, Camera.main.ScreenToWorldPoint(Input.mousePosition).y, 0);
            Instantiate(Bomb, pos, Quaternion.identity);
        }
    }
}
