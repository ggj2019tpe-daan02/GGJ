using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

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


    [Header("Audio")]
    public AudioSource audio;

    [Header("PostFX")]
    public PostFXScript postFX;

    [Header("WinScene")]
    public EndControl endControl;

    [SerializeField] _SceneManager _SceneManager;

    public GameObject prefabs;
    public PlayerGrid playerGrid = new PlayerGrid();

    int GhostCount = 0;

    public static int score = 0;

    Camera camera;
    Grid_Basic[,] GroundGrids;
    Grid_obj[,] ObjGrids;
    Grid_Basic[,] CharactorGrids;
    bool[,] BlockedArray;

    int testCooldown = 0;

    public List<Grid_Ghost> ghostList = new List<Grid_Ghost>();
    
	// Use this for initialization
	void Start () {
        /*camera = Camera.main;
        float height = 2f * camera.orthographicSize;
        float width = height * camera.aspect;
        Debug.Log(height);
        Debug.Log(width);*/
        score = 0;
        StartCoroutine(StageStart());
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

        EnemyCheck();
        detective.CalculateStatus();
        ghostList.RemoveAll(item => item == null);
    }

    IEnumerator StageStart()
    {
        startPosition = GlobalPool.globalPool.startPosition;

        GroundGrids = new Grid_Basic[Map_Xsize, Map_Ysize];
        ObjGrids = new Grid_obj[Map_Xsize, Map_Ysize];
        BlockedArray = new bool[Map_Xsize, Map_Ysize];

        float t = 0;
        
        for (int x = 0; x < Map_Xsize; x++)
        {
            for (int y = 0; y < Map_Ysize; y++)
            {
                float rx = Random.Range(6, -6);
                float ry = Random.Range(6, -6);
                GameObject g = Instantiate(GroundPrefabPool.obj[0], startPosition + new Vector3(x, y - 0.2f, y * 0.01f) + new Vector3(0, -30, 0), Quaternion.identity);

                GroundGrids[x, y] = g.GetComponent<Grid_Basic>();
                GroundGrids[x, y].Move(startPosition + new Vector3(x, y - 0.2f, y * 0.01f), 2f - t);
                t += 0.01f;
            }
        }
        for (int x = 0; x < Map_Xsize; x++)
        {
            for (int y = 0; y < Map_Ysize; y++)
            {
                GameObject g = Instantiate(ObjPrefabPool.obj[0], startPosition + new Vector3(x, y, -1), Quaternion.identity);
                ObjGrids[x, y] = g.GetComponent<Grid_obj>();
                BlockedArray[x, y] = false;
            }
        }
        int r = (int)Random.Range(5, 20);
        SetRandomBlock(10);

        yield return new WaitForSecondsRealtime(1.5f);
        // Create Player
        GameObject Player = Instantiate(CharacterPrefabPool.obj[0], startPosition + new Vector3(8, 5, -2) + new Vector3(0,20,0), Quaternion.identity);
        playerGrid = Player.GetComponent<PlayerGrid>();
        playerGrid.x = 8; playerGrid.y = 5;
        playerGrid.stageManager = this;

        SpawnGhost(); SpawnGhost(); SpawnGhost(); SpawnItem();
        yield return 0;
    }

    void EnemyCheck()
    {
        if (ghostList.Count <= 0) return;
        int x = playerGrid.x;
        int y = playerGrid.y;
        foreach(Grid_Ghost ghost in ghostList)
        {
            if(ghost.x == x&&ghost.y == y)
            {
                PlayerLose();
            }
        }
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
        GhostCount++;
        if(GhostCount >= 3)
        {
            grid_Ghost.Sausage = true ;
        }

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

    public void SetBlocked(int x, int y) {
        BlockedArray[x, y] = true;
        GroundGrids[x, y].Set(GlobalPool.globalPool.groundInfoPool.gridInfos[3]);
    }

    // Only check ground layer now
    public bool IsWalkable(int x, int y)
    {
        bool isWalkable;
        if (GroundGrids[x, y] == null) return false;
        if (BlockedArray[x, y]) return false;
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
        // need check objGrid too!!!!

        return IsBuildable;
    }
    public void Build(int x, int y)
    {
        int r = (int)Random.Range(0, 4);
        if (r == 1) r = 4;
      ObjGrids[x, y].Set(GlobalPool.globalPool.objInfoPool.gridInfos[r]);
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


    public void PlayerWin()
    {
        StartCoroutine(Win());
    }

    IEnumerator Win()
    {
        yield return new WaitForSecondsRealtime(1);
        endControl.Play();
        // _SceneManager.LoadScene("Title");
        yield return 0;
    }

    public void PlayerLose()
    {
        if (playerGrid != null)
        {
            Destroy(playerGrid);
            StartCoroutine(Lose());
        }
    }

    IEnumerator Lose()
    {
        int i = 0;
        postFX.enabled = true;
        postFX.mat.SetFloat("EffectAmount", 0);
        while(i < 140)
        {
            audio.pitch -= 0.023f;
            i++;
            postFX.mat.SetFloat("EffectAmount", i / 5f);
            if (audio.pitch < -2)
            {
                _SceneManager.LoadScene("Main");
                break;
            }

            yield return 0;
        }
        while (true)
        {
            audio.pitch -= 0.023f;
            i++;
            postFX.mat.SetFloat("EffectAmount", i / 5f);
            yield return 0;
        }
    }
}
