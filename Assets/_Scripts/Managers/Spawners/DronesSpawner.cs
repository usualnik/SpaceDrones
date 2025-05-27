using System;
using UnityEngine;

public class DronesSpawner : MonoBehaviour
{
    [SerializeField] private GameObject _dronePrefab;

    private const float FirstDroneSpawnTimer = 2f;
    private const float DronesSpawnCooldown = 5f;

    private void Start()
    {
        //InvokeRepeating(nameof(SpawnDrones), FirstDroneSpawnTimer,DronesSpawnCooldown);
    }

    private void SpawnDrones()
    {
        //Instantiate(_dronePrefab, gameObject.transform.position, Quaternion.identity);
        
    }
}
