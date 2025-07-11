using System.Collections.Generic;
using UnityEngine;

public class PrefabsPool
{
    private GameObject prefab;
    private Queue<GameObject> poolQueue = new Queue<GameObject>();
    private Transform parent;

    public PrefabsPool(GameObject prefab, int initialSize, Transform parent = null)
    {
        this.prefab = prefab;
        this.parent = parent;

        for (int i = 0; i < initialSize; i++)
        {
            GameObject obj = CreateNew();
            obj.SetActive(false);
            poolQueue.Enqueue(obj);
        }
    }

    private GameObject CreateNew()
    {
        GameObject obj = GameObject.Instantiate(prefab, parent);
        obj.name = prefab.name;
        return obj;
    }

    public GameObject Spawn(Vector3 position, Quaternion rotation)
    {
        GameObject obj = poolQueue.Count > 0 ? poolQueue.Dequeue() : CreateNew();

        obj.transform.SetPositionAndRotation(position, rotation);
        obj.SetActive(true);

        // 触发回调
        IPoolable poolable = obj.GetComponent<IPoolable>();
        poolable?.OnSpawn();

        return obj;
    }

    public void Recycle(GameObject obj)
    {
        obj.SetActive(false);
        poolQueue.Enqueue(obj);

        // 触发回调
        IPoolable poolable = obj.GetComponent<IPoolable>();
        poolable?.OnRecycle();
    }
}
