using UnityEngine;
using TMPro;
using System.Collections;

public class TargetSpawner : MonoBehaviour
{
    public GameObject targetPrefab;
    public Transform wallFront, wallRight, wallRear, wallLeft;
    public TextMeshProUGUI scoreText;
    private int score = 0;

    private readonly float wallHeight = 5.0f;
    private readonly float minTileSize = 2.0f;
    private readonly float maxTileSize = 5.0f;
    private readonly int tileCount = 3;

    public bool TargetSpawnerActive { get; set; } = false;

    public Transform sensorWallFront, sensorWallRight, sensorWallRear, sensorWallLeft;

    private bool waitingForRespawn = false;

    public void StartSpawning()
    {
        if (TargetSpawnerActive && !waitingForRespawn)
        {
            Logger.Instance.LogEvent("Target spawning started.", "TargetSpawner");
            SpawnTargetsImmediately();
        }
    }

    void SpawnTargetsImmediately()
    {
        SpawnTarget(wallFront);
        SpawnTarget(wallRear);
        SpawnTarget(wallLeft);
        SpawnTarget(wallRight);
        StartCoroutine(SpawnTargetsAfterDelay());
    }

    IEnumerator SpawnTargetsAfterDelay()
    {
        waitingForRespawn = true;

        float waitTime = Random.Range(2f, 3f);
        yield return new WaitForSeconds(waitTime);

        if (TargetSpawnerActive)
        {
            if (IsWallEmpty(sensorWallFront)) SpawnTarget(wallFront);
            if (IsWallEmpty(sensorWallRear)) SpawnTarget(wallRear);
            if (IsWallEmpty(sensorWallLeft)) SpawnTarget(wallLeft);
            if (IsWallEmpty(sensorWallRight)) SpawnTarget(wallRight);
        }

        waitingForRespawn = false;
    }

    void SpawnTarget(Transform wall)
    {
        float totalLength = maxTileSize * tileCount;
        float currentPosition = -totalLength / 2;

        for (int i = 0; i < tileCount; i++)
        {
            float randomSpacing = Random.Range(minTileSize, maxTileSize);
            currentPosition += randomSpacing;

            Vector3 randomPosition = GetRandomPositionAlongWall(wall, currentPosition);
            Vector3 spawnPosition = randomPosition + new Vector3(0, wallHeight / 2, 0);
            GameObject newTarget = Instantiate(targetPrefab, spawnPosition, Quaternion.identity);
            newTarget.transform.rotation = Quaternion.LookRotation(wall.forward);
        }
    }

    Vector3 GetRandomPositionAlongWall(Transform wall, float offset)
    {
        if (wall == wallLeft || wall == wallRight)
        {
            float randomZ = wall.position.z + offset;
            float randomX = wall.position.x;
            return new Vector3(randomX, wall.position.y, randomZ);
        }
        else
        {
            float randomX = wall.position.x + offset;
            float randomZ = wall.position.z;
            return new Vector3(randomX, wall.position.y, randomZ);
        }
    }

    public void DestroyAllTargets()
    {
        foreach (var target in GameObject.FindGameObjectsWithTag("Target"))
        {
            Destroy(target);
        }
        Logger.Instance.LogEvent("All targets destroyed.", "TargetSpawner");
    }

    public void IncrementScore()
    {
        score++;
        UpdateScoreText();
        Logger.Instance.LogEvent("Score incremented to " + score, "TargetSpawner");
    }

    public void ResetScore()
    {
        score = 0;
        UpdateScoreText();
        Logger.Instance.LogEvent("Score reset to 0.", "TargetSpawner");
    }

    void UpdateScoreText()
    {
        scoreText.text = "Score: " + score;
    }

    public void StopSpawning()
    {
        TargetSpawnerActive = false;
    }

    void Update()
    {
        if (TargetSpawnerActive)
        {
            CheckAndRespawnTargets(sensorWallFront, wallFront);
            CheckAndRespawnTargets(sensorWallRear, wallRear);
            CheckAndRespawnTargets(sensorWallLeft, wallLeft);
            CheckAndRespawnTargets(sensorWallRight, wallRight);
        }
    }

    void CheckAndRespawnTargets(Transform sensor, Transform wall)
    {
        if (IsWallEmpty(sensor) && !waitingForRespawn)
        {
            StartCoroutine(SpawnTargetsAfterDelay());
        }
    }

    bool IsWallEmpty(Transform sensor)
    {
        BoxCollider sensorCollider = sensor.GetComponent<BoxCollider>();
        if (sensorCollider == null)
        {
            return false;
        }

        Collider[] hitColliders = Physics.OverlapBox(sensor.position, sensorCollider.size / 2, Quaternion.identity);

        foreach (var hitCollider in hitColliders)
        {
            if (hitCollider.CompareTag("Target"))
            {
                return false;
            }
        }

        return true;
    }
}
