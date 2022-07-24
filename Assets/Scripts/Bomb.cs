using System;
using UnityEngine;

public class Bomb : MonoBehaviour
{
    [Header("Bomb")]
  
    public float bombFuseTime = 3f;
    
    [Header("Explosion")] public Explosion explosionPrefab;
    public LayerMask explosionLayerMask;
    public LayerMask itemLayerMask;

    private BombController _bombControllerOwner;

    private void Start()
    {
        if (_bombControllerOwner == null)
        {
            Destroy(gameObject);
            return;
        }

        Destroy(gameObject, bombFuseTime);
    }

    public void Initialize(BombController bombControllerOwner)
    {
        _bombControllerOwner = bombControllerOwner;
    }

    private void OnDestroy()
    {
        var position = transform.position;
        position.x = Mathf.Round(position.x);
        position.y = Mathf.Round(position.y);
        
        Instantiate(explosionPrefab, position, Quaternion.identity);

        int explosionRadius = _bombControllerOwner.ExplosionRadius;

        _bombControllerOwner.RetablishAvailableBombs();
        
        Explode(position, Vector2.up, explosionRadius);
        Explode(position, Vector2.down, explosionRadius);
        Explode(position, Vector2.left, explosionRadius);
        Explode(position, Vector2.right, explosionRadius);
    }
    
    private void Explode(Vector2 position, Vector2 direction, int length)
    {
        if (length <= 0) {
            return;
        }

        position += direction;

        if (Physics2D.OverlapBox(position, Vector2.one / 2f, 0f, explosionLayerMask))
        {
            _bombControllerOwner.ClearDestructible(position);
            return;
        }

        Debug.Log($"Ray : {position + direction * length}");
        RaycastHit2D ray = Physics2D.Raycast(position, direction, length, itemLayerMask);
        if (ray.collider != null)
        {
            _bombControllerOwner.ClearItem(ray.collider.gameObject); 
        }

        Explosion explosion = Instantiate(explosionPrefab, position, Quaternion.identity);
        explosion.SetActiveRenderer(length > 1 ? explosion.middle : explosion.end);
        explosion.SetDirection(direction);

        Explode(position, direction, length - 1);
    }
}