using System;
using System.Collections;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Tilemaps;

public class BombController : MonoBehaviour
{
    [Header("Bomb")]
   
    public int bombAmount = 1;
    private int _bombsRemaining;
    public int explosionRadius = 1;
    public GameObject bombPrefab;
    
    [Header("Destructible")]
    public Tilemap destructibleTiles;
    public Destructible destructiblePrefab;

    private bool _isPlacingBomb = false;

    public int ExplosionRadius
    {
        get => explosionRadius;
        private set => explosionRadius = value;
    }

    private void OnEnable()
    {
        _bombsRemaining = bombAmount;
    }
    
    public void TryPlaceBomb()
    {
        if (_bombsRemaining > 0 && !_isPlacingBomb)
        {
            _isPlacingBomb = true;
            PlaceBomb();
        }

        _isPlacingBomb = false;
    }

    private void PlaceBomb()
    {
        Vector2 position = transform.position;
        position.x = Mathf.Round(position.x);
        position.y = Mathf.Round(position.y);

        GameObject bomb = Instantiate(bombPrefab, position, Quaternion.identity);
        bomb.GetComponent<Bomb>().Initialize(this);
        ReduceAvailableBombs();
    }


    Tuple<Vector3Int, TileBase> GetTileByPosition(Vector2 position)
    {
        Vector3Int cell = destructibleTiles.WorldToCell(position);
        TileBase tile = destructibleTiles.GetTile(cell);

        return new Tuple<Vector3Int, TileBase>(cell, tile);
    }

    public void ClearItem(GameObject other)
    {
        Destroy(other);
    }

    public void ClearDestructible(Vector2 position)
    {
        var tileData = GetTileByPosition(position);

        if (tileData.Item2 == null) return;
        
        Instantiate(destructiblePrefab, position, Quaternion.identity);
        destructibleTiles.SetTile(tileData.Item1, null);
    }

    public void AddBomb()
    {
        bombAmount++;
        _bombsRemaining++;
    }

    public void ReduceAvailableBombs()
    {
        _bombsRemaining--;
    }

    public void RetablishAvailableBombs()
    {
        _bombsRemaining++;
    }

    public void AddExplosionRadius()
    {
        ExplosionRadius++;
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Bomb")) {
            other.isTrigger = false;
        }
    }
}
