using System;
using UnityEngine;
using UnityEngine.Pool;

public class ItemPickupReturnToPool : MonoBehaviour
{
    public IObjectPool<ItemPickup> _pool;
    
    public void ReturnObjectToPool(ItemPickup itemPickup)
    {
        // Return to the pool
        _pool.Release(itemPickup);
    }
}