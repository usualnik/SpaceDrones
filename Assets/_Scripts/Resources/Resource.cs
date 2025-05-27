using System;
using System.Collections;
using UnityEngine;

public class Resource : MonoBehaviour, ICollectable
{
    public bool IsOccupied;
    public event EventHandler OnResourceCollected;

    private float destroyTimer = 2f;
    
    public ICollectable Collect()
    {
        OnResourceCollected?.Invoke(this,EventArgs.Empty);
        StartCoroutine(DeactivateSelf());
        return this;
    }

    private IEnumerator DeactivateSelf()
    {
        yield return new WaitForSeconds(destroyTimer);
        gameObject.SetActive(false);
    }
}
