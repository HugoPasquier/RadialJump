using System;
using UnityEngine;

public class DeadZone : MonoBehaviour
{
    public Action OnDead;
    [SerializeField] private Checkpoint _checkpoint;

    private void OnTriggerEnter(Collider col)
    {
        if (col.TryGetComponent<PickableObject>(out var pickable))
        {
            OnDead?.Invoke();
            pickable.Respawn();
        }

        if (col.CompareTag("Player"))
        {
            var player = FindObjectOfType<PlayerMovement>();
            player.transform.position = _checkpoint.transform.position;
        }
    }
}
