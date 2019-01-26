using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainStageManager : MonoBehaviour {

    [SerializeField] int Map_Xsize;
    [SerializeField] int Map_Ysize;

    Vector3 startPosition;

    [Header("Pool")]
    public GridPrefabPool GroundPrefabPool;
    public GridPrefabPool ObjPrefabPool;
    public GridPrefabPool CharacterPrefabPool;
    [Header("")]

    public GameObject prefabs;
    public PlayerGrid playerGrid;

    Camera camera;
    Grid_Basic[,] GroundGrids;
    Grid_Basic[,] ObjGrids;
    Grid_Basic[,] CharactorGrids;
    
	// Use this for initialization
	void Start () {
        /*camera = Camera.main;
        float height = 2f * camera.orthographicSize;
        float width = height * camera.aspect;
        Debug.Log(height);
        Debug.Log(width);*/

        startPosition = GlobalPool.globalPool.startPosition;

        GroundGrids = new Grid_Basic[Map_Xsize, Map_Ysize];
        ObjGrids = new Grid_Basic[Map_Xsize, Map_Ysize];

        for(int x = 0; x < Map_Xsize; x++)
        {
            for(int y = 0; y < Map_Ysize; y++)
            {
                GameObject g = Instantiate(GroundPrefabPool.obj[0], startPosition + new Vector3(x, y, 0), Quaternion.identity);
                GroundGrids[x, y] = g.GetComponent<Grid_Basic>();
            }
        }
        for (int x = 0; x < Map_Xsize; x++)
        {
            for (int y = 0; y < Map_Ysize; y++)
            {
                GameObject g = Instantiate(ObjPrefabPool.obj[0], startPosition + new Vector3(x, y, -1), Quaternion.identity);
                ObjGrids[x, y] = g.GetComponent<Grid_Basic>();
            }
        }
        int r = (int)Random.Range(5, 20);
        SetRandomBlock(10);

        GameObject Player = Instantiate(CharacterPrefabPool.obj[0], startPosition + new Vector3(0, 0, -2), Quaternion.identity);
        playerGrid = Player.GetComponent<PlayerGrid>();
        playerGrid.stageManager = this;

    }
	
	// Update is called once per frame
	void Update () {
		
	}

    void SetRandomBlock(int blockNum)
    {
        GridInfo g = GlobalPool.globalPool.groundInfoPool.gridInfos[2];

        for(int i = 0; i < blockNum; i++)
        {
            int x = 0, y = 0;
            x = (int)Random.Range(0, Map_Xsize);
            y = (int)Random.Range(0, Map_Ysize);
            GroundGrids[x, y].Set(g);
        }
    }

    // Only check ground layer now
    public bool IsWalkable(int x, int y)
    {
        bool isWalkable;
        if (GroundGrids[x, y] == null) return false;
        if(GroundGrids[x, y].IsWalkable && ObjGrids[x, y].IsWalkable)
        {
            isWalkable = true;
        }
        else
        {
            isWalkable = false;
        }

        return isWalkable;
    }
    public bool IsBuildable(int x, int y)
    {
        if (GroundGrids[x, y] == null) return false;
        bool IsBuildable;
        if (GroundGrids[x, y].IsBuildable && ObjGrids[x, y].IsBuildable)
        {
            IsBuildable = true;
        }
        else
        {
            IsBuildable = false;
        }

        if (IsBuildable)
        {
            Debug.Log("Put");
            ObjGrids[x, y].Set(GlobalPool.globalPool.objInfoPool.gridInfos[0]);
        }
        // need check objGrid too!!!!

        return IsBuildable;
    }

    public Vector2 MapSize()
    {
        Vector2 xy = new Vector2(Map_Xsize, Map_Ysize);
        return xy;
    }

    public Vector2 playerXY()
    {
        Vector2 xy = new Vector2(playerGrid.x, playerGrid.y);
        return xy;
    }
}
