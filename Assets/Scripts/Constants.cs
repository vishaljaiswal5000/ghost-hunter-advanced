using System;
using System.Collections.Generic;
using UnityEngine;

public class Constants
{
    public const int Rows = 30;
    public const int Cols = 28;

    // Directions, indexed 0..3 = Up(W), Down(S), Left(A), Right(D)
    public static readonly int[] dRow = { -1, 1, 0, 0 };
    public static readonly int[] dCol = { 0, 0, -1, 1 };
    public static int Opposite(int dir) { return dir ^ 1; }

    // Level Input array for 1st quadrant
    public static int[,] levelMap ={
                     {1,2,2,2,2,2,2,2,2,2,2,2,2,7},
                     {2,5,5,5,5,5,5,5,5,5,5,5,5,4},
                     {2,5,3,4,4,3,5,3,4,4,4,3,5,4},
                     {2,6,4,0,0,4,5,4,0,0,0,4,5,4},
                     {2,5,3,4,4,3,5,3,4,4,4,3,5,3},
                     {2,5,5,5,5,5,5,5,5,5,5,5,5,5},
                     {2,5,3,4,4,3,5,3,3,5,3,4,4,4},
                     {2,5,3,4,4,3,5,4,4,5,3,4,4,3},
                     {2,5,5,5,5,5,5,4,4,5,5,5,5,4},
                     {1,2,2,2,2,1,5,4,3,4,4,3,0,4},
                     {0,0,0,0,0,2,5,4,3,4,4,3,0,3},
                     {0,0,0,0,0,2,5,4,4,0,0,0,0,0},
                     {0,0,0,0,0,2,5,4,4,0,3,4,4,0},
                     {2,2,2,2,2,1,5,3,3,0,4,0,0,0},
                     {0,0,0,0,0,0,5,0,0,0,4,0,0,0},
                     };

    // Full level map (walls 1-4 and 7, pellets 5 and 6, empty 0)
    public static readonly int[,] FulllevelMap = {
                     {1, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 7, 7, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 1},
                     {2, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 4, 4, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 2},
                     {2, 5, 3, 4, 4, 3, 5, 3, 4, 4, 4, 3, 5, 4, 4, 5, 3, 4, 4, 4, 3, 5, 3, 4, 4, 3, 5, 2},
                     {2, 6, 4, 0, 0, 4, 5, 4, 0, 0, 0, 4, 5, 4, 4, 5, 4, 0, 0, 0, 4, 5, 4, 0, 0, 4, 6, 2},
                     {2, 5, 3, 4, 4, 3, 5, 3, 4, 4, 4, 3, 5, 3, 3, 5, 3, 4, 4, 4, 3, 5, 3, 4, 4, 3, 5, 2},
                     {2, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 2},
                     {2, 5, 3, 4, 4, 3, 5, 3, 3, 5, 3, 4, 4, 4, 4, 4, 4, 3, 5, 3, 3, 5, 3, 4, 4, 3, 5, 2},
                     {2, 5, 3, 4, 4, 3, 5, 4, 4, 5, 3, 4, 4, 3, 3, 4, 4, 3, 5, 4, 4, 5, 3, 4, 4, 3, 5, 2},
                     {2, 5, 5, 5, 5, 5, 5, 4, 4, 5, 5, 5, 5, 4, 4, 5, 5, 5, 5, 4, 4, 5, 5, 5, 5, 5, 5, 2},
                     {1, 2, 2, 2, 2, 1, 5, 4, 3, 4, 4, 3, 0, 4, 4, 0, 3, 4, 4, 3, 4, 5, 1, 2, 2, 2, 2, 1},
                     {0, 0, 0, 0, 0, 2, 5, 4, 3, 4, 4, 3, 0, 3, 3, 0, 3, 4, 4, 3, 4, 5, 2, 0, 0, 0, 0, 0},
                     {0, 0, 0, 0, 0, 2, 5, 4, 4, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 4, 4, 5, 2, 0, 0, 0, 0, 0},
                     {0, 0, 0, 0, 0, 2, 5, 4, 4, 0, 3, 4, 4, 0, 0, 4, 4, 3, 0, 4, 4, 5, 2, 0, 0, 0, 0, 0},
                     {2, 2, 2, 2, 2, 1, 5, 3, 3, 0, 4, 0, 0, 0, 0, 0, 0, 4, 0, 3, 3, 5, 1, 2, 2, 2, 2, 2},
                     {0, 0, 0, 0, 0, 0, 5, 0, 0, 0, 4, 0, 0, 0, 0, 0, 0, 4, 0, 0, 0, 5, 0, 0, 0, 0, 0, 0},
                     {0, 0, 0, 0, 0, 0, 5, 0, 0, 0, 4, 0, 0, 0, 0, 0, 0, 4, 0, 0, 0, 5, 0, 0, 0, 0, 0, 0},
                     {2, 2, 2, 2, 2, 1, 5, 3, 3, 0, 4, 0, 0, 0, 0, 0, 0, 4, 0, 3, 3, 5, 1, 2, 2, 2, 2, 2},
                     {0, 0, 0, 0, 0, 2, 5, 4, 4, 0, 3, 4, 4, 0, 0, 4, 4, 3, 0, 4, 4, 5, 2, 0, 0, 0, 0, 0},
                     {0, 0, 0, 0, 0, 2, 5, 4, 4, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 4, 4, 5, 2, 0, 0, 0, 0, 0},
                     {0, 0, 0, 0, 0, 2, 5, 4, 3, 4, 4, 3, 0, 3, 3, 0, 3, 4, 4, 3, 4, 5, 2, 0, 0, 0, 0, 0},
                     {1, 2, 2, 2, 2, 1, 5, 4, 3, 4, 4, 3, 0, 4, 4, 0, 3, 4, 4, 3, 4, 5, 1, 2, 2, 2, 2, 1},
                     {2, 5, 5, 5, 5, 5, 5, 4, 4, 5, 5, 5, 5, 4, 4, 5, 5, 5, 5, 4, 4, 5, 5, 5, 5, 5, 5, 2},
                     {2, 5, 3, 4, 4, 3, 5, 4, 4, 5, 3, 4, 4, 3, 3, 4, 4, 3, 5, 4, 4, 5, 3, 4, 4, 3, 5, 2},
                     {2, 5, 3, 4, 4, 3, 5, 3, 3, 5, 3, 4, 4, 4, 4, 4, 4, 3, 5, 3, 3, 5, 3, 4, 4, 3, 5, 2},
                     {2, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 2},
                     {2, 5, 3, 4, 4, 3, 5, 3, 4, 4, 4, 3, 5, 3, 3, 5, 3, 4, 4, 4, 3, 5, 3, 4, 4, 3, 5, 2},
                     {2, 6, 4, 0, 0, 4, 5, 4, 0, 0, 0, 4, 5, 4, 4, 5, 4, 0, 0, 0, 4, 5, 4, 0, 0, 4, 6, 2},
                     {2, 5, 3, 4, 4, 3, 5, 3, 4, 4, 4, 3, 5, 4, 4, 5, 3, 4, 4, 4, 3, 5, 3, 4, 4, 3, 5, 2},
                     {2, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 4, 4, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 2},
                     {1, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 7, 7, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 1},
                     };

    public static int[] levelMapStartPos = { 1, 1 };
    // Pac-student's current cell {row, col} and heading (-1 = standing still)
    public static int[] levelMaplastPos = { 1, 1 };
    public static int pacDir = -1;

    public static float minXPos = -4.899062f;
    public static float minYPos = 4.873574f;
    public static float maxXPos = 4.718785f;
    public static float maxYPos = -4.844084f;

    // Seconds per tile (lesser is faster)
    public static float carMovementDuration = 0.25f;
    public static float deadGhostMovementDuration = 0.12f;

    // Level 1 or 2 (picked on the start screen); level 2 has faster, more aggressive ghosts
    public static int level = 1;
    public static bool IsLevelTwo { get { return level == 2; } }
    public static float ghostMovementDuration { get { return IsLevelTwo ? 0.22f : 0.28f; } }
    public static float scaredGhostMovementDuration { get { return IsLevelTwo ? 0.34f : 0.4f; } }
    public static float scaredTotalTime { get { return IsLevelTwo ? 6f : 10f; } }
    public static float scatterTime { get { return IsLevelTwo ? 4f : 7f; } }
    public static float chaseTime { get { return IsLevelTwo ? 30f : 20f; } }

    public const int totalLives = 3;
    public const float scaredRecoveringTime = 3f;

    // Game state
    public static int currentScore = 0;
    public static int lives = totalLives;
    public static bool isGhostScared = false;
    public static bool isGhostRecovering = false;
    public static float scaredTimeLeft = 0f;
    public static int scareCounter = 0;     // increments on every power pellet
    public static bool isGameOver = false;
    public static float playerGameTime = 0f;
    public static int pelletLeft = 0;

    // pref keys
    public static string highscoreKey = "highscoreState";
    public static string timeKey = "timeState";

    // Reset all per-game state (also on every play session, in case domain reload is off)
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
    public static void init()
    {
        currentScore = 0;
        lives = totalLives;
        playerGameTime = 0f;
        isGameOver = false;
        isGhostScared = false;
        isGhostRecovering = false;
        scaredTimeLeft = 0f;
        scareCounter = 0;
        pacDir = -1;
        levelMaplastPos[0] = levelMapStartPos[0];
        levelMaplastPos[1] = levelMapStartPos[1];
        LoadAssets.isIntroCompleted = false;

        pelletLeft = 0;
        foreach (int code in FulllevelMap)
        {
            if (code == 5) pelletLeft++;
        }
    }

    public static void StartScare()
    {
        isGhostScared = true;
        isGhostRecovering = false;
        scaredTimeLeft = scaredTotalTime;
        scareCounter++;
    }

    // ---- Grid <-> world ---------------------------------------------------------------
    // Mirrors LevelGenerator: quadrant 1 is built at local (-10 + 0.6 * col, 4.5 - 0.639 * row),
    // scaled by 0.5 and offset by (0.94, 2.34); the other quadrants are mirrors of it.
    // col may be -1 or Cols to get the (off-map) tunnel mouth positions.
    public static Vector3 CellToWorld(int row, int col)
    {
        int r = row < Rows / 2 ? row : Rows - 1 - row;
        bool mirroredX = col >= Cols / 2;
        int c = mirroredX ? Cols - 1 - col : col;

        float lx = -10.0f + 0.6000002f * c;
        float ly = 4.5f - 0.639f * r;

        float x = mirroredX ? -0.94f - 0.5f * lx : 0.94f + 0.5f * lx;
        float y = 2.34f + 0.5f * ly;
        if (row >= Rows / 2) y = -y;
        return new Vector3(x, y, 0f);
    }

    // Ghost pen interior and its four doors
    public static bool IsPen(int row, int col)
    {
        if (row >= 13 && row <= 16 && col >= 11 && col <= 16) return true;
        return (row == 12 || row == 17) && (col == 13 || col == 14);
    }

    public static bool IsWalkable(int row, int col, bool allowPen)
    {
        if (row < 0 || row >= Rows || col < 0 || col >= Cols) return false;
        int code = FulllevelMap[row, col];
        if (code >= 1 && code <= 4 || code == 7) return false;
        return allowPen || !IsPen(row, col);
    }

    // Try to step from (row, col) in direction dir. Handles the side tunnel (rows 14/15) wrap.
    public static bool Step(int row, int col, int dir, bool allowPen, out int nRow, out int nCol, out bool wrapped)
    {
        nRow = row + dRow[dir];
        nCol = col + dCol[dir];
        wrapped = false;

        if (nCol < 0 || nCol >= Cols)
        {
            if (row != 14 && row != 15) return false;
            nCol = nCol < 0 ? Cols - 1 : 0;
            wrapped = true;
        }
        return IsWalkable(nRow, nCol, allowPen);
    }

    // Shortest path (BFS): first direction to take from (sRow, sCol) to the nearest goal cell, -1 if none/already there
    public static int BfsDir(int sRow, int sCol, Func<int, int, bool> isGoal, bool allowPen)
    {
        if (isGoal(sRow, sCol)) return -1;

        const int Unvisited = -2;
        int[,] first = new int[Rows, Cols];
        for (int r = 0; r < Rows; r++)
            for (int c = 0; c < Cols; c++)
                first[r, c] = Unvisited;

        Queue<Vector2Int> queue = new Queue<Vector2Int>();
        first[sRow, sCol] = -1;
        queue.Enqueue(new Vector2Int(sRow, sCol));

        while (queue.Count > 0)
        {
            Vector2Int cur = queue.Dequeue();
            for (int d = 0; d < 4; d++)
            {
                int nr, nc;
                bool wrapped;
                if (!Step(cur.x, cur.y, d, allowPen, out nr, out nc, out wrapped)) continue;
                if (first[nr, nc] != Unvisited) continue;

                first[nr, nc] = (cur.x == sRow && cur.y == sCol) ? d : first[cur.x, cur.y];
                if (isGoal(nr, nc)) return first[nr, nc];
                queue.Enqueue(new Vector2Int(nr, nc));
            }
        }
        return -1;
    }
}
