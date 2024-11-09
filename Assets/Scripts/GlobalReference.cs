using UnityEngine;

public class GloablReference : MonoBehaviour
{
    public static GloablReference Instance { get; private set; }

    public GameObject bulletPrefab;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }
    }
}
