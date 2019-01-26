using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
}
