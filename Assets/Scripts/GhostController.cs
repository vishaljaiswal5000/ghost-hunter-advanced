using System.Collections.Generic;
using UnityEngine;

public enum GhostState
{
    InPen,   // waiting in / leaving the ghost pen
    Normal,  // roaming the maze (scared or not, see IsScared)
    Dead     // eaten, eyes heading back to the pen
}

// Tile-based ghost with a personality per colour:
//   Red    - chases PacStudent directly
//   Blue   - aims 4 tiles ahead of PacStudent
//   Yellow - chases when far away, retreats to a corner when close
//   Green  - wanders randomly (hunts directly on level 2)
// Scared ghosts wander randomly and slowly, eaten ghosts take the shortest path back to the pen.
public class GhostController : MonoBehaviour
{
    private enum Personality { Red, Blue, Yellow, Green }

    public GhostState state = GhostState.InPen;

    private Personality personality;
    private int startRow, startCol;
    private float releaseDelay;

    private int row, col;
    private int dir = -1;
    private bool moving;
    private bool wrapStep;
    private int fromRow, fromCol, toRow, toCol;
    private Vector3 fromPos, toPos;
    private float progress;
    private float z;

    private float waitUntil = -1f;
    private int eatenScare = -1;    // scare this ghost was already eaten in
    private int seenScare;

    private readonly List<int> options = new List<int>(4);

    // Scared (and not yet eaten in this scare)
    public bool IsScared
    {
        get { return state == GhostState.Normal && Constants.isGhostScared && eatenScare != Constants.scareCounter; }
    }

    void Start()
    {
        init();
    }

    public void init()
    {
        string n = gameObject.name;
        if (n.Contains("Blue")) { personality = Personality.Blue; startRow = 13; startCol = 16; releaseDelay = 1.5f; }
        else if (n.Contains("Green")) { personality = Personality.Green; startRow = 16; startCol = 11; releaseDelay = 4.5f; }
        else if (n.Contains("Yellow")) { personality = Personality.Yellow; startRow = 13; startCol = 11; releaseDelay = 3f; }
        else { personality = Personality.Red; startRow = 16; startCol = 16; releaseDelay = 0f; }

        z = transform.position.z;
        ResetToPen();
    }

    private void ResetToPen()
    {
        row = startRow;
        col = startCol;
        dir = -1;
        moving = false;
        wrapStep = false;
        state = GhostState.InPen;
        waitUntil = -1f;
        seenScare = Constants.scareCounter;
        transform.position = Cell(row, col);
    }

    void Update()
    {
        // ghosts stay put during the intro and after game over
        if (!LoadAssets.isIntroCompleted || Constants.isGameOver) return;

        if (waitUntil < 0f) waitUntil = Time.time + releaseDelay;
        if (Time.time < waitUntil) return;

        // a new power pellet makes everyone turn around
        if (seenScare != Constants.scareCounter)
        {
            seenScare = Constants.scareCounter;
            if (state == GhostState.Normal) Reverse();
        }

        if (moving)
        {
            progress += Time.deltaTime / StepDuration();
            if (progress >= 1f) Arrive();
            else transform.position = Vector3.Lerp(fromPos, toPos, progress);
        }

        if (!moving) Decide();
    }

    private float StepDuration()
    {
        if (state == GhostState.Dead) return Constants.deadGhostMovementDuration;
        if (IsScared) return Constants.scaredGhostMovementDuration;
        return Constants.ghostMovementDuration;
    }

    // ---- AI -----------------------------------------------------------------------------

    private void Decide()
    {
        int next = -1;

        switch (state)
        {
            case GhostState.InPen:
                // shortest way out of the pen, then roam
                next = Constants.BfsDir(row, col, (r, c) => !Constants.IsPen(r, c), true);
                if (next < 0 && !Constants.IsPen(row, col)) state = GhostState.Normal;
                break;

            case GhostState.Dead:
                next = Constants.BfsDir(row, col, (r, c) => r >= 14 && r <= 15 && c >= 13 && c <= 14, true);
                if (next < 0)
                {
                    // home: take a short rest, then leave the pen again
                    state = GhostState.InPen;
                    dir = -1;
                    waitUntil = Time.time + 1f;
                }
                break;
        }

        if (state == GhostState.Normal)
        {
            next = ChooseRoamingDir();
        }

        if (next >= 0) BeginStep(next);
    }

    private int ChooseRoamingDir()
    {
        options.Clear();
        for (int d = 0; d < 4; d++)
        {
            int r, c;
            bool w;
            if (dir >= 0 && d == Constants.Opposite(dir)) continue;   // no u-turns
            if (Constants.Step(row, col, d, false, out r, out c, out w)) options.Add(d);
        }

        if (options.Count == 0)
        {
            // dead end: turning back is the only way
            if (dir >= 0 && Constants.Step(row, col, Constants.Opposite(dir), false, out int br, out int bc, out bool bw))
                return Constants.Opposite(dir);
            return -1;
        }
        if (options.Count == 1) return options[0];

        if (IsScared || (personality == Personality.Green && !Constants.IsLevelTwo))
        {
            return options[Random.Range(0, options.Count)];
        }

        Vector2Int target = TargetCell();
        int best = options[0];
        float bestDist = float.MaxValue;
        foreach (int d in options)
        {
            float dr = row + Constants.dRow[d] - target.x;
            float dc = col + Constants.dCol[d] - target.y;
            float dist = dr * dr + dc * dc;
            if (dist < bestDist)
            {
                bestDist = dist;
                best = d;
            }
        }
        return best;
    }

    // Ghosts alternate between heading for their home corner and hunting (durations depend on the level)
    private static bool IsScatterPhase()
    {
        return (Time.timeSinceLevelLoad - 4f) % (Constants.scatterTime + Constants.chaseTime) < Constants.scatterTime;
    }

    private Vector2Int TargetCell()
    {
        int pr = Constants.levelMaplastPos[0];
        int pc = Constants.levelMaplastPos[1];

        if (IsScatterPhase())
        {
            switch (personality)
            {
                case Personality.Red: return new Vector2Int(0, Constants.Cols - 3);
                case Personality.Blue: return new Vector2Int(Constants.Rows - 1, Constants.Cols - 2);
                default: return new Vector2Int(Constants.Rows - 1, 1);
            }
        }

        switch (personality)
        {
            case Personality.Blue:
                if (Constants.pacDir >= 0)
                {
                    pr += 4 * Constants.dRow[Constants.pacDir];
                    pc += 4 * Constants.dCol[Constants.pacDir];
                }
                return new Vector2Int(pr, pc);

            case Personality.Yellow:
                {
                    float dr = row - pr;
                    float dc = col - pc;
                    if (dr * dr + dc * dc < 64f) return new Vector2Int(Constants.Rows - 2, 1);  // too close, back off to a corner
                    return new Vector2Int(pr, pc);
                }

            default:
                return new Vector2Int(pr, pc);
        }
    }

    // ---- Movement -----------------------------------------------------------------------

    private void BeginStep(int direction)
    {
        int nRow, nCol;
        bool wrapped;
        if (!Constants.Step(row, col, direction, true, out nRow, out nCol, out wrapped)) return;

        dir = direction;
        fromRow = row; fromCol = col;
        toRow = nRow; toCol = nCol;
        fromPos = Cell(row, col);
        toPos = wrapped ? Cell(row + Constants.dRow[dir], col + Constants.dCol[dir]) : Cell(nRow, nCol);
        wrapStep = wrapped;
        progress = 0f;
        moving = true;
    }

    private void Arrive()
    {
        row = toRow;
        col = toCol;
        transform.position = Cell(row, col);
        moving = false;
        wrapStep = false;

        // once out of the pen we are a regular roaming ghost
        if (state == GhostState.InPen && !Constants.IsPen(row, col)) state = GhostState.Normal;
    }

    private void Reverse()
    {
        if (dir < 0) return;
        if (!moving)
        {
            BeginStep(Constants.Opposite(dir));
            return;
        }
        if (wrapStep) return;

        Vector3 tmpPos = fromPos; fromPos = toPos; toPos = tmpPos;
        int tmpRow = fromRow; fromRow = toRow; toRow = tmpRow;
        int tmpCol = fromCol; fromCol = toCol; toCol = tmpCol;
        progress = 1f - progress;
        dir = Constants.Opposite(dir);
    }

    private Vector3 Cell(int r, int c)
    {
        Vector3 p = Constants.CellToWorld(r, c);
        p.z = z;
        return p;
    }

    // PacStudent ate this ghost while it was scared
    public void Eaten()
    {
        if (state != GhostState.Normal) return;
        state = GhostState.Dead;
        eatenScare = Constants.scareCounter;
    }
}
