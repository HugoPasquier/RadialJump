using UnityEngine;

public class PlatformAmmo : MonoBehaviour
{
    public Rigidbody Rigidbody;
    [SerializeField] PlatformScalable _platformScalable;
    
    
    private void OnCollisionEnter(Collision other)
    {
        Instantiate(_platformScalable, other.contacts[0].point, Quaternion.identity);
        Destroy(gameObject);
    }
}
