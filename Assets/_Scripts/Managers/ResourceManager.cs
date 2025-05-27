using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ResourceManager : MonoBehaviour
{
   public static ResourceManager Instance { get; private set; }
   
   [SerializeField] private List<Resource> _resourcesOnScene;


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
      _resourcesOnScene = FindObjectsByType<Resource>(FindObjectsSortMode.None)
         .Where(r => r.gameObject.activeInHierarchy)
         .ToList();
      
      foreach (var resource in _resourcesOnScene)
      {
         resource.OnResourceCollected += Resource_OnResourceCollected;
      }
      
      ResourceSpawnManager.Instance.OnResourceSpawned += ResourceSpawnManager_OnResourceSpawned;
   }

   private void OnDestroy()
   {
      ResourceSpawnManager.Instance.OnResourceSpawned -= ResourceSpawnManager_OnResourceSpawned;
   }

   private void ResourceSpawnManager_OnResourceSpawned(object sender, GameObject e)
   {
      _resourcesOnScene.Add(e.GetComponent<Resource>());
   }

   private void Resource_OnResourceCollected(object sender, EventArgs e)
   {
      Resource resource = sender as Resource;
      if (resource != null)
      {
         resource.OnResourceCollected -= Resource_OnResourceCollected;
         _resourcesOnScene.Remove(resource);
      }
   }

   public List<Resource> GetResourcesOnScene()
   {
      return _resourcesOnScene
         .Where(r => r != null && r.gameObject.activeInHierarchy)
         .ToList();
   }
   
}
