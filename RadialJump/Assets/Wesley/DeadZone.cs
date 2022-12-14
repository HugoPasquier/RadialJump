using System;
using UnityEngine;

public class DeadZone : MonoBehaviour
{
    public Action OnDead;
    [SerializeField] private Transform _initialPosition;
    [SerializeField] private PickableObject _projectile;

    private void OnTriggerEnter(Collider col)
    {
        if (col.GetComponent<PickableObject>())
        {
            OnDead?.Invoke();
            ResetProjectile();
        }
    }

    private void ResetProjectile()
    {
        _projectile.transform.position = _initialPosition.position;
        var rb = _projectile.GetComponent<Rigidbody>();
        rb.velocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
    }
}
