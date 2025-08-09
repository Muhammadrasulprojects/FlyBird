using System.Collections.Generic;
using UnityEngine;

public class PipeSpawnScript : MonoBehaviour
{
    public static PipeSpawnScript instance; // singleton — to call from PipeMiddle

    public GameObject pipePrefab; // The pipe prefab to spawn 
    public float spawnInterval = 2f; // Time interval between spawns (for spacing calculation)
    public float timer = 0f; // (currently unused, kept for now)
    public float heightOffset = 10f; // Offset to adjust the height of the spawned pipes

    [Header("Initial spawn settings")]
    public int initialPipesCount = 3;      // number of pipes that should be visible at the start
    public float initialDelaySeconds = 2f; // how far the first pipe should be from the bird (in seconds)

    // internal: list of currently active pipes in the scene
    private List<GameObject> activePipes = new List<GameObject>();

    void Awake()
    {
        // singleton
        if (instance == null) instance = this;
        else if (instance != this) Destroy(gameObject);
    }

    void Start()
    {
        // spacing calculation (moveSpeed * spawnInterval) — fallback if moveSpeed is missing
        float pipeMoveSpeed = GetPipeMoveSpeed();
        float spacing = pipeMoveSpeed * spawnInterval;
        if (spacing <= 0.01f) spacing = spawnInterval * 1.5f; // fallback spacing

        float initialOffsetDistance = pipeMoveSpeed * initialDelaySeconds;
        if (initialOffsetDistance <= 0.01f) initialOffsetDistance = spacing * 0.5f;

        // place initial N pipes (starting from the end with spacing)
        for (int i = 0; i < initialPipesCount; i++)
        {
            float spawnX = transform.position.x + initialOffsetDistance + (spacing * i);
            SpawnPipeAtX(spawnX);
        }

        timer = 0f;
    }

    // (automatic spawning based on timer is intentionally not used in the main flow,
    void Update()
    {
        /*
        if (timer < spawnInterval)
        {
            timer += Time.deltaTime;
        }
        else
        {
            SpawnPipe();
            timer = 0f;
        }
        */
    }

    // original spawn method (spawns at spawner's transform.x)
    void SpawnPipe()
    {
        float lowestPoint = transform.position.y - heightOffset;
        float highestPoint = transform.position.y + heightOffset;

        GameObject go = Instantiate(pipePrefab, new Vector3(transform.position.x, Random.Range(lowestPoint, highestPoint), 0), transform.rotation);
        activePipes.Add(go);
    }

    // helper: spawn a pipe at a given x position and add it to the list
    GameObject SpawnPipeAtX(float x)
    {
        float lowestPoint = transform.position.y - heightOffset;
        float highestPoint = transform.position.y + heightOffset;

        GameObject go = Instantiate(pipePrefab, new Vector3(x, Random.Range(lowestPoint, highestPoint), 0), transform.rotation);
        activePipes.Add(go);
        return go;
    }

    // called when the bird passes 1 pipe — add a new pipe to the end
    public void AddPipeToEnd()
    {
        // clean up null (destroyed) objects
        CleanNulls();

        // spacing calculation
        float pipeMoveSpeed = GetPipeMoveSpeed();
        float spacing = pipeMoveSpeed * spawnInterval;
        if (spacing <= 0.01f) spacing = spawnInterval * 1.5f;

        float spawnX;

        if (activePipes.Count == 0)
        {
            spawnX = transform.position.x + spacing;
        }
        else
        {
            // find the farthest pipe's x position
            float maxX = float.MinValue;
            foreach (GameObject p in activePipes)
            {
                if (p == null) continue;
                if (p.transform.position.x > maxX) maxX = p.transform.position.x;
            }

            spawnX = maxX + spacing;
        }

        SpawnPipeAtX(spawnX);
    }

    // if the number of pipes in the scene is less than initialPipesCount,
    // this can be used to fill in the missing ones (for example, after scene load)
    public void EnsureMinimumPipes()
    {
        CleanNulls();
        while (activePipes.Count < initialPipesCount)
        {
            AddPipeToEnd();
        }
    }

    // remove null elements from the list
    void CleanNulls()
    {
        activePipes.RemoveAll(item => item == null);
    }

    // old instant spawn method, kept just in case
    public void SpawnPipeImmediate()
    {
        SpawnPipe();
        timer = 0f;
    }

    float GetPipeMoveSpeed()
    {
        if (pipePrefab == null) return 0f;
        PipeMoveScript pm = pipePrefab.GetComponent<PipeMoveScript>();
        if (pm == null) return 0f;
        return pm.moveSpeed;
    }
}
