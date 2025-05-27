using System;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Drone))][RequireComponent(typeof(DroneResourceCollector))]
public class DroneInventory : MonoBehaviour
{
    [SerializeField] private List<Resource> _resources;
    
    private DroneResourceCollector _collector;
    private Drone _drone;

    private BaseInventory _baseInventory;

    private const string BlueDrone = "BlueDrone";
    private const string BlueBase = "BlueBase";
    private const string RedDrone = "RedDrone";
    private const string RedBase = "RedBase";
    
    
    private void Awake()
    {
        _drone = GetComponent<Drone>();
    }

    private void Start()
    {
        _baseInventory = gameObject.CompareTag(RedDrone)
            ? GameObject.FindWithTag(RedBase).GetComponent<BaseInventory>()
            : GameObject.FindWithTag(BlueBase).GetComponent<BaseInventory>();

    }

    private void OnEnable()
    {
        _drone.OnUnloadResources += Drone_OnUnloadResources;
    }
    private void OnDisable()
    {
        _drone.OnUnloadResources -= Drone_OnUnloadResources;
    }


    private void Drone_OnUnloadResources(object sender, EventArgs e)
    {
      
        UnloadResourceToBase();
        _drone.ResourceUnloadedToBase_Callback();
    }

    private void UnloadResourceToBase()
    {
        for (int i = _resources.Count - 1; i >= 0; i--)
        {
            var res = _resources[i];
            _baseInventory.AddToBaseInventory(res);
            _resources.RemoveAt(i);
        }
    }

    public void AddResourceToInventory(Resource resource)
    {
        _resources.Add(resource);
    }
    
}
