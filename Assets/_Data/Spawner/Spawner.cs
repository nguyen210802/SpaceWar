using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Spawner : NguyenMonoBehaviour
{
    [SerializeField] protected Transform holder;

    [SerializeField] protected int spawnedCount = 0;
    public int GetSpawnedCount => spawnedCount;

    [SerializeField] protected int totalSpawnedCount = 0;
    public int GetTotalSpawnedCount => totalSpawnedCount;

    [SerializeField] protected List<Transform> prefabs;

    [SerializeField] protected List<Transform> poolObjs;

    protected override void LoadComponents()
    {
        base.LoadComponents();
        this.LoadPrefabs();
        this.LoadHolder();
    }

    protected virtual void LoadPrefabs()
    {
        if (prefabs.Count > 0)  return;

        Transform prefabObj = transform.Find("Prefabs");
        foreach(Transform prefab in prefabObj)
        {
            this.prefabs.Add(prefab);
        }
        this.HidePrefabs();

        Debug.Log(transform.name + ": LoadPrefabs", gameObject);
    }

    protected virtual void HidePrefabs()
    {
        foreach (Transform prefab in this.prefabs)
        {
            prefab.gameObject.SetActive(false);
        }
    }

    protected virtual void LoadHolder()
    {
        if (this.holder != null) return;
        this.holder = transform.Find("Holder");
        Debug.Log(transform.name + ": LoadHolder", gameObject);
    }

    public virtual Transform SpawnByName(string prefabName, Vector3 spawnPos, Quaternion rotation)
    {
        Transform prefab = this.getPrefabByName(prefabName);
        if(prefab == null)
        {
            Debug.LogWarning("Prefab not found: " + prefabName);
            return null;
        }

        return this.SpawnByTransform(prefab, spawnPos, rotation);
    }
    
    public virtual Transform SpawnByTransform(Transform prefab, Vector3 spawnPos, Quaternion rotation)
    {
        Transform newPrefab = this.GetObjectFromPool(prefab);
        newPrefab.SetPositionAndRotation(spawnPos, rotation);

        newPrefab.parent = this.holder;
        this.spawnedCount++;
        this.totalSpawnedCount++;
        return newPrefab;
    }

    public virtual Transform getPrefabByName(string prefabName)
    {
        foreach (Transform prefab in this.prefabs)
        {
            if (prefab.name == prefabName)
                return prefab;
        }
        return null;
    }

    protected virtual Transform GetObjectFromPool(Transform prefab)
    {
        foreach(Transform poolObj in this.poolObjs)
        {
            if(poolObj.name == prefab.name)
            {
                this.poolObjs.Remove(poolObj);
                return poolObj;
            }
        }

        Transform newPrefab = Instantiate(prefab);
        newPrefab.name = prefab.name;
        return newPrefab;
    }

    public virtual void Despawn(Transform obj)
    {
        this.poolObjs.Add(obj);
        obj.transform.gameObject.SetActive(false);
        this.spawnedCount--;
        if (this.spawnedCount <= 0) this.spawnedCount = 0;
    }

    public virtual Transform RandomPrefab()
    {
        int rand = Random.Range(0, this.prefabs.Count);
        return this.prefabs[rand];
    }

    public virtual void Hold(Transform obj)
    {
        obj.parent = this.holder;
    }
}
