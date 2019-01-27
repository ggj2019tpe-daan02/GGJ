using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlobalPool : MonoBehaviour {

    [SerializeField] public GridInfoPool groundInfoPool;
    [SerializeField] public GridInfoPool objInfoPool;
    [SerializeField] public Vector3 startPosition;
    public int BeatTime = 0;
    public AudioSource audio;

    public static GlobalPool globalPool;
	// Use this for initialization
	void Start () {
        globalPool = this;
	}
	
	// Update is called once per frame
	void Update () {
		/*if(audio.time * 147 / 60 % 2 == 1)
        {
            BeatTime = 1;
        }
        else
        {
            BeatTime = 0;
        }*/
	}
}
