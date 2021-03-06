﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Grid_Ghost : Grid_Basic {

    public int x = 0, y = 0;
    int way = 0;
    public MainStageManager stageManager;
    Vector3 startPosition;

    public Sprite DeathSprite;
    public bool Isdeath = false;

    [Header("AnimationSprite")]
    [SerializeField] Sprite[] IdleSprites;
    int testCooldown = 0;

    AudioSource audioSource;

    SpriteRenderer s;
    int h;
    int v;

    // Variables for AI
    public bool Sausage = false;
    float AI_multipler;
    float AI_predictor;




    // Use this for initialization
    void Start ()
    {
        AI_multipler = Random.Range(0, 1.5f);
        AI_predictor = Random.Range(0, 4f);
        startPosition = GlobalPool.globalPool.startPosition;
        s = GetComponent<SpriteRenderer>();
        audioSource = GetComponent<AudioSource>();

        if (Sausage)
        {
            AI_multipler = 0;
        }
    }
	
	// Update is called once per frame
	void Update () {

        if (Isdeath)
        {
            if(transform.localScale.x < 0.01f)
            {
                Destroy(this.gameObject);
            }
            return;
        }
        if (testCooldown > 30)
        {



            testCooldown = 0;
            Vector2 playerXY = stageManager.playerXY();
            Vector2 Target = new Vector2(playerXY.x, playerXY.y);

            Target += (playerXY - new Vector2(x, y))*AI_multipler;

            Vector2 hv = stageManager.detective.FindDirection(x, y, (int)Target.x, (int)Target.y);
            if(hv.x==0 && hv.y==0) stageManager.detective.FindDirection(x, y, (int)playerXY.x, (int)playerXY.y);
            Move((int)hv.x, (int)hv.y);



        }
        else
        {
            testCooldown++;
        }
        AnimationUpdate();
    }

    public void Move(int h, int v)
    {
        if (Isdeath) return;
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
        if (v > 0)
        {
            way = 2;
        }
        else if (v < 0)
        {
            way = 0;
        }
        if (h > 0)
        {
            way = 1;
        }
        else if (h < 0)
        {
            way = 3;
        }

        transform.DOMove(startPosition + new Vector3(x, y, -2), 0.25f);
    }

    public void Death()
    {
        Isdeath = true;
        s.sprite = DeathSprite;
        audioSource.Play();
        transform.DOScale(Vector3.zero, 1.5f).SetEase(Ease.InOutCubic);
    }

    private void OnDestroy()
    {
        Destroy(this.gameObject);
    }
    void AnimationUpdate()
    {
        s.sprite = IdleSprites[(way * 2 + (int)(GlobalPool.globalPool.audio.time * 147 / 60) % 2)];
    }
}
