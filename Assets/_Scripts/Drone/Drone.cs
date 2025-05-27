using System;
using UnityEngine;

public class Drone : MonoBehaviour
{
    public Resource CurrentTargetResource { get; private set; }
    
    public event EventHandler OnLookingForTheResources;
    public event EventHandler OnMovingTowardsResources;
    public event EventHandler OnCollectingResources;
    public event EventHandler OnMovingTowardsBase;
    public event EventHandler OnUnloadResources;

    
    private enum DroneState
    {
        LookingForTheResources,
        MovingTowardsResources,
        CollectingResources,
        MovingTowardsBase,
        UnloadResources
    }
    
    [SerializeField] private DroneState _state = DroneState.LookingForTheResources;

    private void Start()
    {
        UpdateState();
    }

    private void UpdateState()
    {
        switch (_state)
        {
            case DroneState.LookingForTheResources:
                OnLookingForTheResources?.Invoke(this,EventArgs.Empty);
                break;
            case DroneState.MovingTowardsResources:
                OnMovingTowardsResources?.Invoke(this, EventArgs.Empty);
                break;
            case DroneState.CollectingResources:
                OnCollectingResources?.Invoke(this, EventArgs.Empty);
                break;
            case DroneState.MovingTowardsBase:
                OnMovingTowardsBase?.Invoke(this,EventArgs.Empty);
                break;
            case DroneState.UnloadResources:
                OnUnloadResources?.Invoke(this,EventArgs.Empty);
                break;
            default:
                OnLookingForTheResources?.Invoke(this,EventArgs.Empty);
                break;
        }
    }

    public void ResourceFound_Callback(in Resource resource)
    {
        CurrentTargetResource = resource;
        _state = DroneState.MovingTowardsResources;
        UpdateState();
    }

    public void MovementTowardsResource_Callback()
    {
        _state = DroneState.CollectingResources;
        UpdateState();
    }
    public void ResourceCollected_Callback()
    {
        CurrentTargetResource = null;
        _state = DroneState.MovingTowardsBase;
        UpdateState();
    }
    
    public void MovementTowardsBase_Callback()
    {
        _state = DroneState.UnloadResources;
        UpdateState();
    }
    
    public void ResourceUnloadedToBase_Callback()
    {
        _state = DroneState.LookingForTheResources;
        UpdateState();
    }
    
    
}
