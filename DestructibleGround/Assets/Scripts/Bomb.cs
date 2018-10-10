using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Bomb : MonoBehaviour {


    private GameObject slider;
    private new CircleCollider2D collider;

    private void Awake()
    {
        slider = GameObject.Find("Slider");
        Slider slide = slider.GetComponent<Slider>();
        collider = gameObject.transform.GetChild(0).GetComponent<CircleCollider2D>();
        collider.radius = slide.value;
    }
    private void Update()
    {
        if(transform.position.y < -10)
            Destroy(gameObject);
    }
}
