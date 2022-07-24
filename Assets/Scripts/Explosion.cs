using System;
using System.Collections;
using UnityEngine;

public class Explosion : MonoBehaviour
{
    public AnimatedSpriteRenderer start;
    public AnimatedSpriteRenderer middle;
    public AnimatedSpriteRenderer end;
    
    public float explosionDuration = 1f;

    private void Awake()
    {
        SetActiveRenderer(start);
        DestroyAfter(explosionDuration);
    }

    private void Start()
    {
        StartCoroutine(StartDelayed());
    }

    private IEnumerator StartDelayed()
    {
        yield return new WaitForSeconds(3.0f);
        StartExplosionTimer();
    }

    public void SetActiveRenderer(AnimatedSpriteRenderer inputRenderer)
    {
        start.enabled = inputRenderer == start;
        middle.enabled = inputRenderer == middle;
        end.enabled = inputRenderer == end;
    }

    public void SetDirection(Vector2 direction)
    {
        float angle = Mathf.Atan2(direction.y, direction.x);
        transform.rotation = Quaternion.AngleAxis(angle * Mathf.Rad2Deg, Vector3.forward);
    }

    public void StartExplosionTimer()
    {
        DestroyAfter(explosionDuration);
    }

    private void DestroyAfter(float seconds)
    {
        Destroy(gameObject, seconds);
    }
}
