using System;
using UnityEngine;
using UnityEngine.Pool;

public class ItemPickupObjectPool : MonoBehaviour
{
    IObjectPool<ItemPickup> m_Pool;

    public Action<Vector2> itemCreationAction = null;

    // Collection checks will throw errors if we try to release an item that is already in the pool.
    [SerializeField]
    private bool collectionChecks = true;

    [SerializeField]
    private int maxPoolSize = 20;
    
    public IObjectPool<ItemPickup> Pool
    {
        get
        {
            if (m_Pool == null)
            {
                m_Pool = new LinkedPool<ItemPickup>(
                    CreatePooledItem,
                    OnTakeFromPool,
                    OnReturnedToPool,
                    OnDestroyPoolObject,
                    collectionChecks,
                    maxPoolSize);
            }

            return m_Pool;
        }
    }

    private void OnDestroyPoolObject(ItemPickup itemPickup)
    {
        Debug.Log("OnDestroyPoolObject");
        Debug.Log($"Pool available : {Pool.CountInactive}");
        Destroy(itemPickup.gameObject);
    }

    private void OnReturnedToPool(ItemPickup itemPickup)
    {
        Debug.Log("OnReturnedToPool");
        Debug.Log($"Pool available : {Pool.CountInactive}");
        itemPickup.gameObject.SetActive(false);
    }

    private void OnTakeFromPool(ItemPickup itemPickup)
    {
        Debug.Log("OnTakeFromPool");
        Debug.Log($"Pool available : {Pool.CountInactive}");
        itemPickup.gameObject.SetActive(true);
    }

    private ItemPickup CreatePooledItem()
    {
        Debug.Log("CreatePooledItem");
        var go = new GameObject($"ObjectPool for {typeof(ItemPickup)}");
        var goComponent = go.AddComponent<ItemPickup>();

        itemCreationAction?.Invoke(go.transform.position);
        
        var returnToPool = go.AddComponent<ItemPickupReturnToPool>();
        returnToPool._pool = Pool;
        Debug.Log($"Pool available : {Pool.CountInactive}");

        return goComponent;
    }
}