using UnityEngine;
using TMPro;

public class TargetSpawner : MonoBehaviour
{
    public GameObject targetPrefab;
    public Transform wall1, wall2, wall3, wall4;
    public TextMeshProUGUI scoreText;
    private int score = 0;

    private readonly float wallHeight = 5.0f;
    private readonly float minTileSize = 2.0f;
    private readonly float maxTileSize = 5.0f;
    private readonly int tileCount = 3;

    public bool TargetSpawnerActive { get; set; } = false;

    public void SpawnTargets()
    {
        if (TargetSpawnerActive)
        {
            SpawnTarget(wall1);
            SpawnTarget(wall2);
            SpawnTarget(wall3);
            SpawnTarget(wall4);
        }
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
        if (wall == wall3 || wall == wall4)
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
    }

    public void IncrementScore()
    {
        score++;
        UpdateScoreText();
    }

    public void ResetScore()
    {
        score = 0;
        UpdateScoreText();
    }

    void UpdateScoreText()
    {
        scoreText.text = "Score: " + score;
    }

    public void StopSpawning()
    {
        TargetSpawnerActive = false;
    }
}
