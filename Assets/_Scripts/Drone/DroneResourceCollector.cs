using System;
using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Drone))][RequireComponent(typeof(DroneInventory))]
public class DroneResourceCollector : MonoBehaviour
{
    private Drone _drone;
    private DroneInventory _droneInventory;
    private float _collectResourceTimer = 2f;
    
    

    private void Awake()
    {
        _drone = GetComponent<Drone>();
        _droneInventory = GetComponent<DroneInventory>();
    }

    private void OnEnable()
    {
        
        _drone.OnCollectingResources += Drone_OnCollectingResources;
    }

    private void OnDisable()
    {
        _drone.OnCollectingResources -= Drone_OnCollectingResources;
    }

    private void Drone_OnCollectingResources(object sender, EventArgs e)
    {
        StartCoroutine(CollectResourceRoutine());
    }

    private IEnumerator CollectResourceRoutine()
    {
        _drone.CurrentTargetResource.Collect();
        _droneInventory.AddResourceToInventory(_drone.CurrentTargetResource);
        yield return new  WaitForSeconds(_collectResourceTimer);
        _drone.ResourceCollected_Callback();
    }
    
}
