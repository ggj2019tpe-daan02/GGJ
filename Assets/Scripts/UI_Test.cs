using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class UI_Test : MonoBehaviour {

    public Text Lego;
    public Text scoreText;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        Lego.text = "<size=25>x</size>" + PlayerGrid.LegoNum.ToString();
        scoreText.text = MainStageManager.score.ToString();
	}
}
