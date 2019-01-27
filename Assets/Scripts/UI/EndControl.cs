using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndControl : MonoBehaviour {

    public EndImage endImage;
    public EndText endText;
    public EndBlack endBlack;

    [Header("SceneManager")]
    public _SceneManager sceneManager;

    bool canNextStage = false;
	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetButtonDown("Jump") && canNextStage)
        {
            canNextStage = false;
            sceneManager.LoadScene("Main");
        }
	}

    public void Play()
    {
        StartCoroutine(P());
    }

    IEnumerator P()
    {
        yield return new WaitForSecondsRealtime(0.1f);
        endImage.IPlay();
        endBlack.IPlay();
        yield return new WaitForSecondsRealtime(0.75f);
        endText.IPlay();
        yield return new WaitForSecondsRealtime(0.5f);
        canNextStage = true;
        yield return 0;
    }
}
