using System.Collections;
using UnityEngine;

// Tile-based player movement. InputManager queues a direction with SetInput();
// the car turns as soon as the queued direction is open and otherwise keeps going straight.
public class PacStudentController : MonoBehaviour
{
    private Animator mAnimator;
    private AudioSource mAudioSource;
    public AudioSource carCollide;
    public AudioSource pelletEaten;
    public AudioSource powerPelletEaten;
    public AudioSource carWreck;

    public GameObject BonusPellet;
    public ParticleSystem particleEffect;

    // Rotation (z) of the car sprite for each direction: Up, Down, Left, Right
    private static readonly float[] rotationZ = { 0f, 180f, 90f, 270f };
    private const float respawnDelay = 2f;
    private const float safeTimeAfterRespawn = 2f;

    private int row, col;
    private int dir = -1;       // current heading
    private int queued = -1;    // direction asked for by the player

    private bool moving;
    private bool wrapStep;      // current step goes through the side tunnel
    private int fromRow, fromCol, toRow, toCol;
    private Vector3 fromPos, toPos;
    private float progress;
    private bool bumped;

    private float z;
    private bool dead;
    private float safeUntil;

    // true while respawning or briefly after, ghosts can't hurt us
    public bool Invulnerable { get { return dead || Time.time < safeUntil; } }

    void Start()
    {
        init();
    }

    public void init()
    {
        mAudioSource = GetComponent<AudioSource>();
        mAudioSource.Stop();
        mAnimator = GetComponent<Animator>();
        mAnimator.enabled = false;

        carCollide = FindSound("Sound-Collide");
        pelletEaten = FindSound("Sound-Pellet-Eaten");
        powerPelletEaten = FindSound("Sound-Power-Pellet-Eaten");
        carWreck = FindSound("Sound-Wreck");

        GameObject particleObj = GameObject.FindGameObjectWithTag("Particle-Effect-Drive");
        particleEffect = particleObj != null ? particleObj.GetComponent<ParticleSystem>() : GetComponentInChildren<ParticleSystem>();
        if (particleEffect != null) particleEffect.Stop();

        z = transform.position.z;
        PlaceAt(Constants.levelMapStartPos[0], Constants.levelMapStartPos[1]);
        transform.rotation = Quaternion.Euler(0f, 0f, rotationZ[3]);

        // Spawn Bonus pellet until game over
        if (!Constants.isGameOver && BonusPellet != null)
        {
            InvokeRepeating("spawnBonusPellet", 10, 20);
        }
    }

    private static AudioSource FindSound(string tag)
    {
        GameObject obj = GameObject.FindGameObjectWithTag(tag);
        return obj != null ? obj.GetComponent<AudioSource>() : null;
    }

    void Update()
    {
        if (Constants.isGameOver || dead) return;

        if (moving)
        {
            progress += Time.deltaTime / Constants.carMovementDuration;
            if (progress >= 1f)
            {
                Arrive();
            }
            else
            {
                transform.position = Vector3.Lerp(fromPos, toPos, progress);
            }
        }

        if (!moving)
        {
            TryStartStep();
        }
    }

    // Player input: 0 = Up, 1 = Down, 2 = Left, 3 = Right
    public void SetInput(int direction)
    {
        queued = direction;

        // reversing is allowed at any time
        if (moving && dir >= 0 && direction == Constants.Opposite(dir) && !wrapStep)
        {
            Vector3 tmpPos = fromPos; fromPos = toPos; toPos = tmpPos;
            int tmpRow = fromRow; fromRow = toRow; toRow = tmpRow;
            int tmpCol = fromCol; fromCol = toCol; toCol = tmpCol;
            progress = 1f - progress;
            dir = direction;
            transform.rotation = Quaternion.Euler(0f, 0f, rotationZ[dir]);
            Constants.pacDir = dir;
        }
    }

    private void TryStartStep()
    {
        int next = -1;
        int r, c;
        bool w;

        if (queued >= 0 && Constants.Step(row, col, queued, false, out r, out c, out w))
        {
            next = queued;
        }
        else if (dir >= 0 && Constants.Step(row, col, dir, false, out r, out c, out w))
        {
            next = dir;
        }

        if (next < 0)
        {
            // blocked: stop and bump (once) if the player is pushing into a wall
            if (queued >= 0 && !bumped && carCollide != null)
            {
                carCollide.Play();
            }
            bumped = queued >= 0;
            StopMoving();
            return;
        }

        bumped = false;
        BeginStep(next);
    }

    private void BeginStep(int direction)
    {
        int nRow, nCol;
        bool wrapped;
        Constants.Step(row, col, direction, false, out nRow, out nCol, out wrapped);

        dir = direction;
        fromRow = row; fromCol = col;
        toRow = nRow; toCol = nCol;
        fromPos = Cell(row, col);
        // through the tunnel we glide to the mouth, then pop out on the other side
        toPos = wrapped ? Cell(row + Constants.dRow[dir], col + Constants.dCol[dir]) : Cell(nRow, nCol);
        wrapStep = wrapped;
        progress = 0f;
        moving = true;

        transform.rotation = Quaternion.Euler(0f, 0f, rotationZ[dir]);
        Constants.pacDir = dir;

        mAnimator.enabled = true;
        if (!mAudioSource.isPlaying) mAudioSource.Play();
        if (particleEffect != null) particleEffect.Play();
    }

    private void Arrive()
    {
        row = toRow;
        col = toCol;
        transform.position = Cell(row, col);
        moving = false;
        wrapStep = false;
        Constants.levelMaplastPos[0] = row;
        Constants.levelMaplastPos[1] = col;
    }

    private void StopMoving()
    {
        moving = false;
        mAudioSource.Stop();
        mAnimator.enabled = false;
        if (particleEffect != null) particleEffect.Stop();
    }

    private Vector3 Cell(int r, int c)
    {
        Vector3 p = Constants.CellToWorld(r, c);
        p.z = z;
        return p;
    }

    private void PlaceAt(int r, int c)
    {
        row = r;
        col = c;
        dir = -1;
        queued = -1;
        moving = false;
        wrapStep = false;
        transform.position = Cell(r, c);
        Constants.levelMaplastPos[0] = r;
        Constants.levelMaplastPos[1] = c;
        Constants.pacDir = -1;
    }

    // Spawn bonus pellet
    public void spawnBonusPellet()
    {
        if (Constants.isGameOver) return;
        Instantiate(BonusPellet, Vector3.zero, BonusPellet.transform.rotation);
    }

    // Wrecked by a ghost: play the dead animation, then respawn at the start
    public void respawn()
    {
        if (!dead) StartCoroutine("respawning");
    }

    IEnumerator respawning()
    {
        dead = true;
        StopMoving();
        mAnimator.enabled = true;
        mAnimator.SetBool("isDead", true);
        yield return new WaitForSeconds(respawnDelay);
        mAnimator.SetBool("isDead", false);
        mAnimator.enabled = false;

        PlaceAt(Constants.levelMapStartPos[0], Constants.levelMapStartPos[1]);
        transform.rotation = Quaternion.Euler(0f, 0f, rotationZ[3]);
        safeUntil = Time.time + safeTimeAfterRespawn;
        dead = false;
    }

    // Stop all movements
    public void stopMovement()
    {
        queued = -1;
        StopMoving();
    }
}
