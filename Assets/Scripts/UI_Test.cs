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
        Lego.text = "剩餘樂高數:" + PlayerGrid.LegoNum;
        scoreText.text = MainStageManager.score.ToString();
	}
}
