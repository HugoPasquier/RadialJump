using Unity.Mathematics;
using UnityEngine;

public struct Target
{
    public Vector3 Position;
    public Quaternion Rotation;
    public Vector3 Normal;

    public Target(Vector3 position, Quaternion rotation, Vector3 normal)
    {
        Position = position;
        Rotation = rotation;
        Normal = normal;
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
        var clone = Instantiate(_platformScalable, target.Position, _platformScalable.transform.rotation);
        // Change forward direction of plateform scalable based on surface hit normal
        clone.transform.forward = target.Normal;
        clone.transform.rotation *= target.Rotation;
        Destroy(gameObject);
    }
}
