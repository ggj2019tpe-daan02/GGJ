using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class _SceneManager : MonoBehaviour {

    private AsyncOperation async = null;
    public Image black;
    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
    private IEnumerator LoadALevel(string levelName)
    {
        while (black.color.a < 1f)
        {
            Color c = black.color;
            Debug.Log(c.a);
            c.a += (1 - c.a) * 0.05f + 0.01f;
            black.color = c;

            yield return 0;
        }
        async = SceneManager.LoadSceneAsync(levelName);

        yield return new WaitForSecondsRealtime(1f);

        while (!async.isDone)
        {
            Debug.Log(async.progress);
            yield return null;
        }
        yield return async;
    }

    public void LoadScene(string level)
    {
        StartCoroutine(LoadALevel(level));
    }

    public void Exit()
    {
        StartCoroutine(CloseGame());
    }

    IEnumerator CloseGame()
    {
        while (black.color.a < 1f)
        {
            Color c = black.color;
            Debug.Log(c.a);
            c.a += (1 - c.a) * 0.05f + 0.01f;
            black.color = c;

            yield return 0;
        }
        yield return new WaitForSecondsRealtime(0.5f);
        Application.Quit();
        yield return 0;
    }
}
