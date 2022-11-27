using UnityEngine;

public struct Target
{
    public Vector3 Position;
    public Quaternion Rotation;
    public Vector3 ForwardNormal;
    public Vector3 SnappingDirection;

    public Target(Vector3 position, Quaternion rotation, Vector3 forwardNormal, Vector3 snappingDirection)
    {
        Position = position;
        Rotation = rotation;
        ForwardNormal = forwardNormal;
        SnappingDirection = snappingDirection;
    }
}

/// <summary>
/// Drop an platform when it's near his target
/// </summary>
public class PlatformAmmo : MonoBehaviour
{
    [SerializeField] private PlatformScalable _platformScalable;
    [SerializeField] private float _speed = 7.5f;
    [SerializeField] private float _targetSpawnRadius = 0.2f;

    private Target _target;
    private Transform _thisTransform;
    
    private void Awake()
    {
        _thisTransform = transform;
    }

    public void Inject(in Target target)
    {
        _target = target;
    }

    private void Update()
    {
        transform.position = Vector3.Lerp(transform.position, _target.Position, Time.deltaTime * _speed);
        if (Vector3.Distance(_thisTransform.position, _target.Position) < _targetSpawnRadius)
        {
            CreatePlatform(_target);
        }
    }

    private void CreatePlatform(in Target target)
    {
        var clone = Instantiate(_platformScalable, target.Position,  Quaternion.identity, transform.parent);
        
        // Change forward direction of plateform scalable based on surface hit normal
        clone.transform.forward = target.ForwardNormal;

        // Snap platform direction with player perspective
        var platformDirection = clone.SnappingPoint.position - target.Position;
        float snappingAngle = -Vector3.SignedAngle(target.SnappingDirection, platformDirection.normalized, target.ForwardNormal);
        
        // Apply player orientation and target rotation
        clone.transform.rotation *= Quaternion.Euler(0f, 0f, snappingAngle) * target.Rotation;
        
        Destroy(gameObject);
    }
}
