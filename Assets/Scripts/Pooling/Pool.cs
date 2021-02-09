using System.Collections.Generic;
using UnityEngine;

public class Pool : MonoBehaviour
{
    static Dictionary<PooledMonoBehavior, Pool> pools = new Dictionary<PooledMonoBehavior, Pool>();

    Queue<PooledMonoBehavior> objects = new Queue<PooledMonoBehavior>();

    private PooledMonoBehavior prefab;

    public static Pool GetPool(PooledMonoBehavior prefab)
    {
        if (pools.ContainsKey(prefab))
            return pools[prefab];

        var pool = new GameObject("Pool-" + prefab.name).AddComponent<Pool>();
        pool.prefab = prefab;

        pools.Add(prefab, pool);
        return pool;
    }

    public T Get<T>() where T : PooledMonoBehavior
    {
        if (objects.Count == 0)
            GrowPool();

        var pooledObject = objects.Dequeue();
        return pooledObject as T;
    }

    private void GrowPool()
    {
        for (int i = 0; i < prefab.InitialPoolSize; i++)
        {
            var pooledObject = Instantiate(prefab, this.transform) as PooledMonoBehavior;
            pooledObject.gameObject.name += " " + i;

            pooledObject.gameObject.SetActive(false);
            pooledObject.OnReturnToPool += AddObjectToAvailableQueue;
            objects.Enqueue(pooledObject);
        }
    }

    private void AddObjectToAvailableQueue(PooledMonoBehavior pooledObject)
    {
        pooledObject.transform.SetParent(this.transform);
        objects.Enqueue(pooledObject);
    }

    private void OnDestroy()
    {
        pools.Clear();
    }
}
