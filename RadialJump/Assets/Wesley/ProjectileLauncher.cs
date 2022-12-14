using System.Collections;
using UnityEngine;

public class ProjectileLauncher : MonoBehaviour
{
    [SerializeField] private Door _door;
    [SerializeField] private float _waitingTime = 1.5f;
    [SerializeField] private DeadZone _deadZone;

    public bool Active = false;
    
    private void Start() => LaunchNextProjectile();

    private void OnEnable() => _deadZone.OnDead += LaunchNextProjectile;

    private void OnDisable() => _deadZone.OnDead -= LaunchNextProjectile;

    public void LaunchNextProjectile() => StartCoroutine(LaunchNextProjectile_Coroutine());

    public IEnumerator LaunchNextProjectile_Coroutine()
    {
        if (Active)
        {
            _door.Open();

            yield return new WaitForSeconds(_waitingTime);
    
            _door.Close();
        }
    }

    public void ChangeState(bool platformActive)
    {
        Active = platformActive;
        if (Active)
        {
            LaunchNextProjectile();
        }
    }
}
