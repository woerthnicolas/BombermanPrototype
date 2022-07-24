using System;
using UnityEngine;

public class Destructible : MonoBehaviour
{
    public float destructionTime = 1f;
    private ItemSpawner _itemSpawner;
    private void Awake()
    {
        _itemSpawner = FindObjectOfType<ItemSpawner>();
    }

    private void Start()
    {
        Debug.Log("Destructible Start");
        Destroy(gameObject, destructionTime);
        SpawnItem();
    }

    private void SpawnItem()
    {
        Debug.Log("SpawnItem");
        if (_itemSpawner)
        {
            _itemSpawner.SpawnRandomItem(this.transform.position);
        }
    }
}
