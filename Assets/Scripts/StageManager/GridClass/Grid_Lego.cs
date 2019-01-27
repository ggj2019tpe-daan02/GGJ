using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grid_Lego : Grid_obj {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public override void OnPlayerTouch()
    {
        base.OnPlayerTouch();
        if(!IsBuildable)
        PlayerGrid.LegoNum += 3;
        IsWalkable = true;
        IsBuildable = true;
        SpriteRenderer s = GetComponent<SpriteRenderer>();
        s.sprite = null;
        
        SFXController.Play(SFXController.SoundType.PickUp, 0.5f);
    }
}
