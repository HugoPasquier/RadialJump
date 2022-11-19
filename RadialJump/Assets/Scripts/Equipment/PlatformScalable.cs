using System;
using UnityEngine;
using System.Collections;
using Vector3 = UnityEngine.Vector3;

/// <summary>
/// Platform scale until it reaches a maximum or an obstacle
/// </summary>
public class PlatformScalable : MonoBehaviour
{
    [SerializeField] private float _maxScale = 1f;
    [SerializeField] private float _scaleSpeed = 0.1f;

    private Transform _thisTransform;

    private void Awake() => _thisTransform = transform;

    private void Start()
    {
        _thisTransform.localScale = Vector3.zero;
        StartCoroutine(Scale_Coroutine());
    }

    private bool _bCollide = false;
    
    private IEnumerator Scale_Coroutine()
    {
        int nbScale = (int)(_maxScale / _scaleSpeed);
        for (int i = 0; i < nbScale; i++)
        {
            yield return new WaitForEndOfFrame();

            if (_bCollide)
            {
                Debug.Log("stop");
                break;
            }
            
            _thisTransform.localScale += _scaleSpeed * Vector3.one;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        _bCollide = true;
    }
}
