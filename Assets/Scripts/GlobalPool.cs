using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlobalPool : MonoBehaviour {

    [SerializeField] public GridInfoPool groundInfoPool;
    [SerializeField] public GridInfoPool objInfoPool;
    [SerializeField] public Vector3 startPosition;

    public static GlobalPool globalPool;
	// Use this for initialization
	void Start () {
        globalPool = this;
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
