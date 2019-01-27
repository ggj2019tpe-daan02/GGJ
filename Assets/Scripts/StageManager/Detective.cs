using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Detective : MonoBehaviour {
    public MainStageManager stagemanager;


    class GridIDState {
        public int x;
        public int y;
        public string hash;
        public int id;
        public GridIDState(int x, int y, int id) {
            this.x = x;
            this.y = y;
            this.hash = x + " " + y;
            this.id = id;
        }
    }

    public int[,] GetGridID() {
        Vector2 size = stagemanager.MapSize();
        int[,] result = new int[(int)size.x, (int)size.y];
        Stack<GridIDState> stack = new Stack<GridIDState>();
        for (int i = 0; i < size.x; i++) {
            for (int j = 0; j < size.y; j++) {
                stack.Push(new GridIDState(i, j, i + j * (int)size.x));
                result[i, j] = -1;
            }
        }
        while (stack.Count > 0) {
            var now = stack.Pop();
            if (result[now.x, now.y] != -1) continue;
            if (!stagemanager.IsWalkable(now.x, now.y)) continue;
            result[now.x, now.y] = now.id;
            for (int i = -1; i <= 1; i++) {
                for (int j = -1; j <= 1; j++) {
                    if (i != 0 && j != 0) continue;
                    if (i == 0 && j == 0) continue;
                    int x = now.x + i;
                    int y = now.y + j;
                    if (x < 0 || x >= size.x || y < 0 || y >= size.y) continue;
                    stack.Push(new GridIDState(x, y, now.id));
                }
            }
        }
        return result;
    }


    class PathState {
        public int x;
        public int y;
        public PathState prev;
    }

    public Vector2 FindDirection(int x, int y, int tx, int ty) {
        //Debug.Log(x + " " + y + " -> " + tx + " " + ty);
        Vector2 size = stagemanager.MapSize();
        Queue<PathState> queue = new Queue<PathState>();
        int[,] result = new int[(int)size.x, (int)size.y];
        queue.Enqueue(new PathState {
            x = x, y = y, prev = null
        });

        for (int i = 0; i < size.x; i++) {
            for (int j = 0; j < size.y; j++) {
                result[i, j] = -1;
            }
        }

        while (queue.Count > 0) {
            var now = queue.Dequeue();
            if (now.x == tx && now.y == ty) {

                int dx = 0;
                int dy = 0;
                PathState p = now;
                while (p.prev != null) {
                    dx = p.x - p.prev.x;
                    dy = p.y - p.prev.y;
                    p = p.prev;
                }
                return new Vector2(dx, dy);
            }

            if (result[now.x, now.y] != -1) continue;
            if (!stagemanager.IsWalkable(now.x, now.y)) continue;
            result[now.x, now.y] = 1;

            for (int i = -1; i <= 1; i++) {
                for (int j = -1; j <= 1; j++) {
                    if (i != 0 && j != 0) continue;
                    if (i == j) continue;


                    int nx = now.x + i;
                    int ny = now.y + j;
                    if (nx < 0 || nx >= size.x || ny < 0 || ny >= size.y) continue;

                    queue.Enqueue(new PathState() {
                        x = nx, y = ny, prev = now
                    });
                }
            }
        }
        //Debug.Log("failed");
        return new Vector2(0, 0);
    }




    public void CalculateStatus() {
        int[,] GridId = GetGridID();
        Vector2 playerXY = stagemanager.playerXY();
        Vector2 size = stagemanager.MapSize();
        List<int> solvedIds = new List<int>();

        int playerId = GridId[(int)playerXY.x, (int)playerXY.y];

        bool withGhost = false;


        // gether all point belongs to one id

        Dictionary<int, List<Vector2>> PointDict = new Dictionary<int, List<Vector2>>();

        for (int i = 0; i < size.x; i++) {
            for (int j = 0; j < size.y; j++) {
                int id = GridId[i, j];
                if (id == -1) continue;
                if (!PointDict.ContainsKey(id)) PointDict[id] = new List<Vector2>();
                PointDict[id].Add(new Vector2(i, j));
            }
        }

        // do per enemy check
        List<Vector2> PlayerSpots = new List<Vector2>();

        if (playerId != -1) {
            PlayerSpots = PointDict[playerId];

            foreach (var ghost in stagemanager.ghostList) {

                int ghostId = GridId[ghost.x, ghost.y];
                if (ghostId == playerId) withGhost = true;
                else {
                    if (ghost.Isdeath) continue;
                    solvedIds.Add(ghostId);
                    int blockCount = PointDict[ghostId].Count;
                    Debug.Log("ghost chawdu! with " + blockCount + " blocks captured");
                    int score = 100 + ((1 - blockCount) * 8);
                    if (score < 10) score = 10;
                    MainStageManager.score += score;
                    ghost.Death();
                }
            }

            // player win

            if (!withGhost) {
                int blockCount = PointDict[playerId].Count;
                // do player win action
                Debug.Log("player win! with " + blockCount + " blocks captured");
                MainStageManager.score += blockCount;
                stagemanager.PlayerWin();
            }

        }

        // do per block update
        foreach (var i in solvedIds) {
            if (PointDict[i] != null)
                foreach (var point in PointDict[i]) {
                    // block those points
                    stagemanager.SetBlocked((int)point.x, (int)point.y);
                    Debug.Log(point.x + " " + point.y);
                }
            PointDict.Remove(i);
        }


        //foreach (var pair in PointDict) {
        //    if (pair.Key == -1) continue;
        //    if (pair.Value != null) {
        //        Debug.Log("detected a empty place"+pair.Key);
        //    }
        //}
    }



}