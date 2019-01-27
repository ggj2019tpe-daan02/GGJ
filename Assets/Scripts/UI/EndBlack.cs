using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class EndBlack : MonoBehaviour {

    Image image;
	// Use this for initialization
	void Start () {
        image = GetComponent<Image>();
	}
	
	// Update is called once per frame
	void Update () {

    }
    public void IPlay()
    {
        StartCoroutine(Play());
    }

    IEnumerator Play()
    {

        image.DOColor(new Color(0, 0, 0, 0.45f), 0.6f);
        yield return 0;
    }
}
