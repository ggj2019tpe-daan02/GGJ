using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class EndImage : MonoBehaviour {

    RectTransform rectTransform;
	// Use this for initialization
	void Start () {
        rectTransform = GetComponent<RectTransform>();
        StartCoroutine(Play());
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    IEnumerator Play()
    {
        rectTransform.localScale = Vector3.zero;
        rectTransform.DOScale(new Vector3(1, 1, 1), 0.5f);
        yield return 0;
    }
}
