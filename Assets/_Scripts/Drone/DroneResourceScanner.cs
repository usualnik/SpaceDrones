using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[RequireComponent(typeof(Drone))]
public class DroneResourceScanner : MonoBehaviour
{
    [SerializeField] private float _scanDelay = 0.5f;
    
    private Drone _drone;
    private bool _isLooking;
   
    private void Awake()
    {
        _drone = gameObject.GetComponent<Drone>();
    }

    private void OnEnable()
    {
        _drone.OnLookingForTheResources += Drone_OnLookingForTheResources;
    }

    private void OnDisable()
    {
        _drone.OnLookingForTheResources -= Drone_OnLookingForTheResources;
    }

    private void Drone_OnLookingForTheResources(object sender, EventArgs e)
    {
        StartCoroutine(nameof(ScanRoutine));
    }

    private IEnumerator ScanRoutine()  
    {  
        while (true)  
        {  
            Resource resource = FindNearestResource();
            if (resource != null)
            {
                resource.IsOccupied = true;
                _drone.ResourceFound_Callback(resource);
                yield break;
            }
            yield return new WaitForSeconds(_scanDelay);  
        }  
    } 
    
    private Resource FindNearestResource()
    {
        List<Resource> resources = ResourceManager.Instance.GetResourcesOnScene();

        var availableResources = resources.Where(r => !r.IsOccupied).ToList();
        
        return availableResources.Count == 0  
            ? null  
            : availableResources.OrderBy(r => Vector2.Distance(transform.position, r.transform.position)).First();  
    }  
}  
