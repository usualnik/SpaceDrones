using System;
using UnityEngine;
using Random = UnityEngine.Random;

public class ResourceSpawnManager : MonoBehaviour
{
    [SerializeField] private GameObject _resourcePrefab;
    public static ResourceSpawnManager Instance { get; private set; }
   
    public event EventHandler<GameObject> OnResourceSpawned;
    
    private readonly Vector3 _spawnBoundaries = new Vector3(7,4, 0 );

    private const float FirstResourceSpawnTimer = 2f;
    private const float ResourceSpawnCooldown = 2f;
  
    
    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }
    
    private void Start()
    {
        InvokeRepeating(nameof(SpawnResources),FirstResourceSpawnTimer,ResourceSpawnCooldown);
    }

    private void SpawnResources()
    {
        GameObject resourceSpawned = Instantiate(_resourcePrefab, RandomizeSpawnPos(), Quaternion.identity);
        OnResourceSpawned?.Invoke(this, resourceSpawned);
    }

    private Vector3 RandomizeSpawnPos()
    {
        var randomPosX = Random.Range(_spawnBoundaries.x, -_spawnBoundaries.x);
        var randomPosY = Random.Range(_spawnBoundaries.y, -_spawnBoundaries.y);

        return new Vector3(randomPosX,randomPosY,0);
    }
}
