using System;
using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Drone))]
public class DroneAnimationHandler : MonoBehaviour
{

    [SerializeField] private ParticleSystem _returnToBasePS;
    
    private Drone _drone;

    private void Awake()
    {
        _drone = GetComponent<Drone>();
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
        _returnToBasePS.Play();
        StartCoroutine(StopReturnToBasePS());

    }

    private IEnumerator StopReturnToBasePS()
    {
        yield return new WaitForSeconds(1f);
        _returnToBasePS.Stop();
    }
}
