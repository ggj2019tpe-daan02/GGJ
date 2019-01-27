using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TitlePageScript : MonoBehaviour {
    public _SceneManager manager;
    public AudioSource source;
    bool started = false;
	// Use this for initialization
	void Start () {
		
	}


    // Update is called once per frame
    void Update () {

        if (started)
        {
            source.volume -= 0.005f;
        }

        if (!started && Input.GetButton("Jump")) {
            started = true;
            manager.LoadScene("Main");
        }
	}
}
