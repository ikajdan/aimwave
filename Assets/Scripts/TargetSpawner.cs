using UnityEngine;
using TMPro;

public class TargetSpawner : MonoBehaviour
{
    public GameObject targetPrefab;
    public Transform wall1, wall2, wall3, wall4;
    public TextMeshProUGUI scoreText;
    private int score = 0;

    private readonly float wallHeight = 5.0f;
    private readonly float tileSize = 5.0f;

    public bool TargetSpawnerActive { get; set; } = false;

    public void SpawnTargetsAtCenters()
    {
        if (TargetSpawnerActive)
        {
            SpawnTargetAtWall(wall1);
            SpawnTargetAtWall(wall2);
            SpawnTargetAtWall(wall3);
            SpawnTargetAtWall(wall4);
        }
    }

    void SpawnTargetAtWall(Transform wall)
    {
        Vector3 randomPosition = GetRandomPositionAlongWall(wall);
        Vector3 spawnPosition = randomPosition + new Vector3(0, wallHeight / 2, 0);
        GameObject newTarget = Instantiate(targetPrefab, spawnPosition, Quaternion.identity);
        newTarget.transform.rotation = Quaternion.LookRotation(wall.forward);
    }

    Vector3 GetRandomPositionAlongWall(Transform wall)
    {
        if (wall == wall3 || wall == wall4)
        {
            float randomZ = Random.Range(wall.position.z - tileSize, wall.position.z + tileSize);
            float randomX = wall.position.x;
            return new Vector3(randomX, wall.position.y, randomZ);
        }
        else
        {
            float randomX = Random.Range(wall.position.x - tileSize, wall.position.x + tileSize);
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
