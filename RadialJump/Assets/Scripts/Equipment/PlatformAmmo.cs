using UnityEngine;

/// <summary>
/// Drop an platform when it's near his target
/// </summary>
public class PlatformAmmo : MonoBehaviour
{
    [SerializeField] private PlatformScalable _platformScalable;
    [SerializeField] private float _targetSpawnRadius = 0.2f;
    
    public Rigidbody Rigidbody;

    private RaycastHit _targetHit;
    private Transform _thisTransform;
    
    private void Awake()
    {
        _thisTransform = transform;

        Ray ray = new Ray(_thisTransform.position, _thisTransform.forward);
        if (!Physics.Raycast(ray, out _targetHit))
        {
            Debug.LogWarning("Can't create a platform in space.");
            Destroy(gameObject);
        }
    }

    private void Update()
    {
        // Use distance to spawn a platform and not a collision event, because the ammos can't move too fast to detect a collision
        // It pass through the other object
        if (Vector3.Distance(_thisTransform.position, _targetHit.point) < _targetSpawnRadius)
        {
            CreatePlatform(_targetHit.point, _targetHit.normal);
        }
    }

    private void CreatePlatform(Vector3 origin, Vector3 normal)
    {
        Instantiate(_platformScalable, origin, _platformScalable.transform.rotation);
        Destroy(gameObject);
    }
}
