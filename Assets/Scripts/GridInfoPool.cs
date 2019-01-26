using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[CreateAssetMenu(menuName = "CreateGridInfoData")]
public class GridInfoPool : ScriptableObject {
    public GridInfo[] gridInfos;
}

[Serializable]
public class GridInfo
{
    public string Gridname;
    public int Gridnum;
    public bool Walkable;
    public bool Buildable;
    public Sprite GridSprite;
}
