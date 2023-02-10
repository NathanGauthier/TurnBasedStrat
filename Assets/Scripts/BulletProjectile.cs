using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletProjectile : MonoBehaviour
{
    [SerializeField] private TrailRenderer trailRenderer;
    [SerializeField] private Transform bulletHitVfxPrefab;

    private Vector3 targetPosition;
    public void Setup(Vector3 targetPosition)
    {
        this.targetPosition = targetPosition;
    }

    private void Update()
    {
        Vector3 movedDir = (targetPosition - transform.position).normalized;

        float distanceBeforeMoving = Vector3.Distance(targetPosition, transform.position);

        float moveSpeed = 200.0f;
        transform.position += movedDir * moveSpeed * Time.deltaTime;

        float distanceAfterMoving = Vector3.Distance(targetPosition, transform.position);

        if(distanceBeforeMoving < distanceAfterMoving)
        {
            transform.position = targetPosition;
            trailRenderer.transform.parent = null;
            Destroy(gameObject);

            Instantiate(bulletHitVfxPrefab, targetPosition, Quaternion.identity);
        }

    }
}
