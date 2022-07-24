using System;
using System.Collections.Generic;
using UnityEngine;

public class ItemPickup : MonoBehaviour, IReturnToPool
{
    public enum ItemType
    {
        ExtraBomb,
        BlastRadius,
        SpeedIncrease,
    }

    public SoPickableItem PickableItem { get; set; }

    private void OnItemPickup(GameObject player)
    {
        if (PickableItem == null)
            return;

        PlayerController playerController = player.GetComponent<PlayerController>();
        if (playerController == null)
            return;

        PickableItem.Activate(playerController);

        Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            OnItemPickup(other.gameObject);
            ReturnToPool();
        }
    }

    public void LoadSoData(SoPickableItem soPickableItem)
    {
        PickableItem = soPickableItem;
        
        GetComponent<SpriteRenderer>().sprite = PickableItem.icon;
    }
    
    public void ReturnToPool()
    {
        var returnToPool = GetComponent<ItemPickupReturnToPool>();
        if (returnToPool)
        {
            returnToPool.ReturnObjectToPool(this);
        }
    }
}
