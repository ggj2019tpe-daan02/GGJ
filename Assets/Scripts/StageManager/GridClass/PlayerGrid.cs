using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class PlayerGrid : MonoBehaviour {

    public int x = 0, y = 0;
    public MainStageManager stageManager;
    Vector3 startPosition;

    int testCooldown = 0;

    int h;
    int v;
    // Use this for initialization
    void Start () {
        startPosition = GlobalPool.globalPool.startPosition;

    }
	
	// Update is called once per frame
	void Update () {

        PlayerInput();

        if (testCooldown > 10)
        {
            testCooldown = 0;
            Move();
        }
        else
        {
            testCooldown++;
        }

        PutBlock();
    }

    void PlayerInput()
    {
        if (Input.GetButtonDown("Horizontal"))
        {
            v = 0;
            h = (int)Input.GetAxisRaw("Horizontal");
        }
        if (Input.GetButtonDown("Vertical"))
        {
            h = 0;
            v = (int)Input.GetAxisRaw("Vertical");
        }
        if (Input.GetButtonUp("Horizontal"))
        {
            h = 0;
        }
        if (Input.GetButtonUp("Vertical"))
        {
            v = 0;
        }
    }

    void PutBlock()
    {
        if (Input.GetButtonDown("Jump"))
        {
            bool b = stageManager.IsBuildable(x, y);
        }
    }

    void Move()
    {

        if (h != 0)
        {
            if (stageManager.IsWalkable(x + h, y))
            {
                x += h;
            }
        }
        if (v != 0)
        {
            if (stageManager.IsWalkable(x, y + v))
            {
                y += v;
            }
        }

        transform.DOMove(startPosition + new Vector3(x, y, 0), 0.25f);
    }
}
