using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class PlayerGrid : MonoBehaviour {

    public int x = 0, y = 0;
    int way = 0;
    public MainStageManager stageManager;
    Vector3 startPosition;

    public static int LegoNum = 3;
    int testCooldown = 0;

    [Header("AnimationSprite")]
    [SerializeField] Sprite[] IdleSprites;
    [SerializeField] Sprite[] WalkSprites;
    SpriteRenderer s;

    [Header("Death")]
    SpriteRenderer[] DeathSprites;
    bool IsDeath = false;

    int h;
    int v;
    // Use this for initialization
    void Start () {
        startPosition = GlobalPool.globalPool.startPosition;
        s = GetComponent<SpriteRenderer>();
        LegoNum = 3;
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

        AnimationUpdate();
        PutBlock();
    }

    public void Death()
    {

    }

    void PlayerInput()
    {
        if (Input.GetButton("Horizontal"))
        {
            v = 0;
            h = (int)Input.GetAxisRaw("Horizontal");
        }
        if (Input.GetButton("Vertical"))
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

        if (v > 0)
        {
            way = 0;
        }
        else if (v < 0)
        {
            way = 2;
        }
        if (h > 0)
        {
            way = 1;
        }else if(h < 0)
        {
            way = 3;
        }
    }

    void PutBlock()
    {
        if (LegoNum <= 0) return;
        if (Input.GetButton("Jump"))
        {
            Debug.Log("?");
            bool b = stageManager.IsBuildable(x, y);
            if (b)
            {
                LegoNum--;
                stageManager.Build(x, y);
            }
        }
    }

    void Move()
    {

        if (h != 0)
        {
            if (stageManager.IsWalkable(x + h, y))
            {
                x += h;
                stageManager.CheckPlayerPosition();
            }
        }
        if (v != 0)
        {
            if (stageManager.IsWalkable(x, y + v))
            {
                y += v;
                stageManager.CheckPlayerPosition();
            }
        }

        transform.DOMove(startPosition + new Vector3(x, y, -2), 0.25f);
    }

    void AnimationUpdate()
    {
        s.sprite = WalkSprites[(way * 2 + (int)(GlobalPool.globalPool.audio.time * 147 / 60 )% 2)];
    }
}
