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

    [Header("Detective")]
    public Detective detective;
    [Header("")]

    public GameObject prefabs;
    public PlayerGrid playerGrid;

    Camera camera;
    Grid_Basic[,] GroundGrids;
    Grid_obj[,] ObjGrids;
    Grid_Basic[,] CharactorGrids;

    int testCooldown = 0;

    public List<Grid_Ghost> ghostList = new List<Grid_Ghost>();
    
	// Use this for initialization
	void Start () {
        /*camera = Camera.main;
        float height = 2f * camera.orthographicSize;
        float width = height * camera.aspect;
        Debug.Log(height);
        Debug.Log(width);*/

        startPosition = GlobalPool.globalPool.startPosition;

        GroundGrids = new Grid_Basic[Map_Xsize, Map_Ysize];
        ObjGrids = new Grid_obj[Map_Xsize, Map_Ysize];

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
                ObjGrids[x, y] = g.GetComponent<Grid_obj>();
            }
        }
        int r = (int)Random.Range(5, 20);
        SetRandomBlock(10);

        // Create Player
        GameObject Player = Instantiate(CharacterPrefabPool.obj[0], startPosition + new Vector3(0, 0, -2), Quaternion.identity);
        playerGrid = Player.GetComponent<PlayerGrid>();
        playerGrid.stageManager = this;

        SpawnGhost();SpawnGhost(); SpawnItem();

        /*GameObject Lego = Instantiate(ObjPrefabPool.obj[2], startPosition + new Vector3(5, 5, -2), Quaternion.identity);
        Grid_obj grid = Lego.GetComponent<Grid_obj>();
        ObjGrids[5, 5] = grid;*/
    }
	
	// Update is called once per frame
	void Update () {
        int[,] result = detective.GetGridID();
        // Debug.Log(result[playerGrid.x, playerGrid.y]);
        if(testCooldown > 120)
        {
            testCooldown = 0;
            SpawnItem();
        }
        else
        {
            testCooldown++;
        }

        detective.CalculateStatus();
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

    void SpawnGhost()
    {
        bool work = false;
        int x = 0, y = 0;
        while (!work)
        {
            x = (int)Random.Range(0, Map_Xsize);
            y = (int)Random.Range(0, Map_Ysize);
            work = IsWalkable(x, y);
        }
        GameObject Ghost = Instantiate(CharacterPrefabPool.obj[1], startPosition + new Vector3(x, y, -2), Quaternion.identity);
        Grid_Ghost grid_Ghost = Ghost.GetComponent<Grid_Ghost>();
        grid_Ghost.x = x; grid_Ghost.y = y;
        grid_Ghost.stageManager = this;

        ghostList.Add(grid_Ghost);
    }

    void SpawnItem()
    {
        bool work = false;
        int x = 0, y = 0;
        while (!work)
        {
            x = (int)Random.Range(0, Map_Xsize);
            y = (int)Random.Range(0, Map_Ysize);
            if(IsWalkable(x,y) && IsBuildable(x,y) && ObjGrids[x,y])
            {
                work = IsWalkable(x, y);
            }
        }
        GameObject Lego = Instantiate(ObjPrefabPool.obj[2], startPosition + new Vector3(x, y, -1), Quaternion.identity);
        Grid_obj grid = Lego.GetComponent<Grid_obj>();
        ObjGrids[x, y] = grid;
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
            Debug.Log("Put");
            IsBuildable = true;
        }
        else
        {
            IsBuildable = false;
        }
        // need check objGrid too!!!!

        return IsBuildable;
    }
    public void Build(int x, int y)
    {
      ObjGrids[x, y].Set(GlobalPool.globalPool.objInfoPool.gridInfos[0]);
    }

    public void CheckPlayerPosition()
    {
        Vector2 xy = playerXY();
        int x = (int)xy.x; int y = (int)xy.y;
        if(ObjGrids[x,y] != null)
        {
            ObjGrids[x, y].OnPlayerTouch();
        }
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
