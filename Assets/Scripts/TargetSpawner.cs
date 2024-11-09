using UnityEngine;
using TMPro;

public class TargetSpawner : MonoBehaviour
{
    public GameObject targetPrefab;
    public Transform wall1, wall2, wall3, wall4;
    public TextMeshProUGUI scoreText;
    private int score = 0;

    public bool TargetSpawnerActive { get; set; } = false;

    public void SpawnTargetsAtCenters()
    {
        if (TargetSpawnerActive)
        {
            SpawnTargetAtTop(wall1);
            SpawnTargetAtTop(wall2);
            SpawnTargetAtTop(wall3);
            SpawnTargetAtTop(wall4);
        }
    }

    void SpawnTargetAtTop(Transform wall)
    {
        Vector3 topPosition = wall.position + new Vector3(0, 2.5f, 0);
        GameObject newTarget = Instantiate(targetPrefab, topPosition, Quaternion.identity);
        newTarget.transform.rotation = Quaternion.LookRotation(wall.forward);
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
