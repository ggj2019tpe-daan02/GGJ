using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Grid_Ghost : Grid_Basic {

    public int x = 0, y = 0;
    public MainStageManager stageManager;
    Vector3 startPosition;

    int testCooldown = 0;

    int h;
    int v;
    // Use this for initialization
    void Start ()
    {
        startPosition = GlobalPool.globalPool.startPosition;
    }
	
	// Update is called once per frame
	void Update () {

        if (testCooldown > 30)
        {
            testCooldown = 0;
            Vector2 playerXY = stageManager.playerXY();
            Vector2 hv = stageManager.detective.FindDirection(x, y, (int)playerXY.x, (int)playerXY.y);
            Move((int)hv.x, (int)hv.y);
        }
        else
        {
            testCooldown++;
        }

    }

    public void Move(int h, int v)
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

        transform.DOMove(startPosition + new Vector3(x, y, -2), 0.25f);
    }
}
