using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TitleGhost : MonoBehaviour {

    Vector3 Origin;

	// Use this for initialization
	void Start () {
        Origin = transform.position;

    }
	
	// Update is called once per frame
	void Update () {
        transform.position = Origin + new Vector3(
            Mathf.Sin(Time.time/5) * 40f, 
            Mathf.Sin(Time.time) * 30f, 0);

    }
}
