using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

[RequireComponent(typeof(SpriteRenderer))]
public class Grid_Basic : MonoBehaviour {
    public bool IsWalkable;
    public bool IsBuildable;
    public int obj_num;
    SpriteRenderer spriteRenderer;
	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void Set(GridInfo g)
    {
        IsWalkable = g.Walkable;
        IsBuildable = g.Buildable;
        spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.sprite = g.GridSprite;
    }

    public void Move(Vector3 v3, float t)
    {
        transform.DOMove(v3, t).SetEase(Ease.InCirc);
    }
}
