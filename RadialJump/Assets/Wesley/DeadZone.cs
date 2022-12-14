using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DeadZone : MonoBehaviour
{
    public Action OnDead;
    [SerializeField] private Transform _initialPosition;
    [SerializeField] private PickableObject _projectile;
    [SerializeField] private Transform _checkpoint;

    private void OnTriggerEnter(Collider col)
    {
        if (col.TryGetComponent<PickableObject>(out var pickable))
        {
            OnDead?.Invoke();
            pickable.Respawn();
        }

        if (col.CompareTag("Player"))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }
}
