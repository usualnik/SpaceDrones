using System.Collections.Generic;
using UnityEngine;

public class BaseInventory : MonoBehaviour
{
    [SerializeField] private List<Resource> _baseInventory;

    public void AddToBaseInventory(Resource resource)
    {
        _baseInventory.Add(resource);
    }
}
