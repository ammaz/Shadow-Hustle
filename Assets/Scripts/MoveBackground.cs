using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveBackground : MonoBehaviour {

    public float speed = 0;
    float offsetX = 0;
    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        offsetX = Time.deltaTime * speed;
        this.transform.localPosition = new Vector3(this.transform.localPosition.x - offsetX, this.transform.localPosition.y, this.transform.localPosition.z);
	}
}
