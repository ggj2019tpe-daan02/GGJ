using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TitleTextBlink : MonoBehaviour {



    public float BlinkSpeed = 200;


    // Use this for initialization
    void Start() {

    }

    // Update is called once per frame
    void Update() {
        var color = GetComponent<UnityEngine.UI.Text>().color;
        if (Time.time / BlinkSpeed % 2 <= 1)
            color.a = 0;
        else color.a = 1;

        GetComponent<UnityEngine.UI.Text>().color = color;
    }
}
