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
    
    public Transform SnappingPoint;

    private Transform _thisTransform;
    
    private void Awake() => _thisTransform = transform;

    private void Start()
    {
        _thisTransform.localScale = Vector3.zero;
        
        // // Define max scale on obstacle
        // if (Physics.BoxCast(transform.position - new Vector3(0f,0f, 1f), new Vector3(_maxScale, 0.2f, _maxScale) / 2f, transform.forward,  out var hit, Quaternion.identity, _maxScale))
        // {
        //     _maxScale = Vector3.Distance(hit.point, transform.position);
        // }
        
        StartCoroutine(Scale_Coroutine());
    }

    private IEnumerator Scale_Coroutine()
    {
        int nbScale = (int)(_maxScale / _scaleSpeed);
        for (int i = 0; i < nbScale; i++)
        {
            yield return new WaitForEndOfFrame();

            _thisTransform.localScale += _scaleSpeed * Vector3.one;
        }
    }

    // private void OnDrawGizmos()
    // {
    //     Gizmos.color = Color.yellow;
    //     Gizmos.DrawWireCube(transform.position + new Vector3(0f, 0f, 1f), new Vector3(_maxScale, 0.2f, _maxScale));
    // }
}
