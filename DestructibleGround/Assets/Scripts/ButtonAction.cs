using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonAction : MonoBehaviour {

    public GameObject ground;
    private GroundScript groundScript;

    public void ResetMap()
    {
        ground = GameObject.Find("ground");
        groundScript = ground.GetComponent<GroundScript>();
        groundScript.GenerateSprite();
        groundScript.UpdateCollider();
    }



}
