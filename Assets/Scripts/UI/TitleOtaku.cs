using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TitleOtaku : MonoBehaviour {

    Vector3 originalSize;


	// Use this for initialization
	void Start () {
        originalSize = transform.localScale;
		
	}
	
	// Update is called once per frame
	void Update () {
        float t = Time.time * 1.8f;
        float d = Mathf.Sin(t) * .01f + Mathf.Sin(t*1.3f) * .005f;
        transform.localScale = originalSize +new Vector3(-d, d, 0);

    }
}
