using System;
using UnityEngine;

public class PooledMonoBehavior : MonoBehaviour
{
    [SerializeField] int initialPoolSize = 50;

    public event Action<PooledMonoBehavior> OnReturnToPool;
    public event Action OnExitPool;

    public T Get<T>(bool enable = true) where T : PooledMonoBehavior
    {
        var pool = Pool.GetPool(this);
        var pooledObject = pool.Get<T>();

        if (enable)
            pooledObject.gameObject.SetActive(true);

        return pooledObject;
    }

    public T Get<T>(Vector3 position, Quaternion rotation, Vector2? velocity = null) where T : PooledMonoBehavior
    {
        var pooledObject = Get<T>();

        pooledObject.transform.position = position;
        pooledObject.transform.rotation = rotation;

        if (velocity != null)
            pooledObject.GetComponent<Rigidbody2D>().velocity = (Vector2)velocity;

        if (pooledObject.OnExitPool != null)
            pooledObject.OnExitPool();

        return pooledObject;
    }

    public int InitialPoolSize { get { return initialPoolSize; } }

    void OnDisable()
    {
        if (OnReturnToPool != null)
            OnReturnToPool(this);
    }
}