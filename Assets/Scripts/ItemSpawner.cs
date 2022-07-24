using System;
using UnityEngine;
using UnityEngine.Pool;
using Random = UnityEngine.Random;

public class ItemSpawner : MonoBehaviour
{
    [Range(0f, 1f)] public float itemSpawnChance = 0.2f;

    private ItemPickupObjectPool _itemPickupPool;

    private void Awake()
    {
        _itemPickupPool = gameObject.AddComponent<ItemPickupObjectPool>();
        _itemPickupPool.itemCreationAction = SpawnRandomItem;
    }

    public void SpawnRandomItem(Vector2 position)
    {
        if (GameManager.Instance.SoPickableItems.Count > 0 && Random.value < itemSpawnChance)
        {
            int randomIndex = Random.Range(0, GameManager.Instance.SoPickableItems.Count);
            SoPickableItem item = GameManager.Instance.SoPickableItems[randomIndex];
            GameObject spawnedItem =
                Instantiate(GameManager.Instance.itemPrefab, position, Quaternion.identity);
            ItemPickup itemPickup = spawnedItem.GetComponent<ItemPickup>();
            itemPickup.LoadSoData(item);
            ItemPickupReturnToPool returnToPool = spawnedItem.AddComponent<ItemPickupReturnToPool>();
            returnToPool._pool = _itemPickupPool.Pool;
        }
    }
}