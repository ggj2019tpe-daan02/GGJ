using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlackMOOD_OUT : MonoBehaviour {
    public float FadeSpeed = 1;

    float startTime = 0;
    // Use this for initialization
    void Start () {
        startTime = Time.time;
	}
	
	// Update is called once per frame
	void Update () {
        var color = GetComponent<UnityEngine.UI.Image>().color;
        color.a = (1 - Time.time / FadeSpeed);
        if (color.a < 0) color.a = 0;

        GetComponent<UnityEngine.UI.Image>().color = color;

    }
}
