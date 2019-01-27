using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

public class EndText : MonoBehaviour {

    RectTransform rectTransform;
    Text text;
    // Use this for initialization
    void Start()
    {
        rectTransform = GetComponent<RectTransform>();
        text = GetComponent<Text>();
        text.color = new Color(1, 1, 1, 0);
        rectTransform.localScale = new Vector3(3, 3, 3);
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void IPlay()
    {
        StartCoroutine(Play());
    }

    IEnumerator Play()
    {
        text.text = MainStageManager.score.ToString();
        rectTransform.DOScale(new Vector3(1, 1, 1), 0.5f);
        text.DOColor(new Color(0, 0, 0, 1), 0.5f);
        yield return 0;
    }
}
