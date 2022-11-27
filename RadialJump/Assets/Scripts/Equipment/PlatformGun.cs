using Unity.Mathematics;
using UnityEngine;
using Quaternion = UnityEngine.Quaternion;
using Vector3 = UnityEngine.Vector3;

public class PlatformGun : Weapon
{
    [SerializeField] private Transform _barrel;
    [SerializeField] private PlatformAmmo _ammo;
    [SerializeField] private Transform _ammosContainer;
    [SerializeField] private float _rotationAmount = 15f;
    
    private Camera _camera;
    
    private Quaternion _crosshairRotation = quaternion.identity;
    
    private Vector3 CENTER_SCREEN = new Vector3(0.5f, 0.5f, 0f);

    private void Awake() => _camera = Camera.main;

    protected override void Start()
    {
        base.Start();
    }

    void Update()
    {
        if (currentCadence < cadenceCD)
            currentCadence += Time.deltaTime;

        if (canBeUse && Input.GetMouseButtonDown(twoHanded ? 0 : hand.cote == 1 ? 1 : 0) && currentCadence > cadenceCD)
        {
            Shoot();
        }
        
        var scrollValue = Input.mouseScrollDelta.y;
        if (canBeUse && scrollValue != 0f)
        {
            ChangeRotation(scrollValue);
        }
    }
    
    public void Shoot()
    {
        recoilSystem.RecoilFire(this);
        hand.KnockbackFire();
        currentCadence -= cadenceCD;
        
        // We encounter a platform with platform orientation. In fact, Player can move around an object or change gravity,
        // so his axis are different to other objects. In short, we need to take in account the player orientation
        // with our platform creation.
        // In response, we use two points to create a direction and rotate a platform. Our first point
        // is the platform position and the second gives the player orientation.
        // We snap the platform direction with the second point, then we apply our desired rotation.
        var platformRay = _camera.ViewportPointToRay(CENTER_SCREEN);
        var markerRay = _camera.ViewportPointToRay(CENTER_SCREEN - new Vector3(0.02f, 0f));
        if (Physics.Raycast(platformRay, out RaycastHit platformHit, portee, canBeShot) && Physics.Raycast(markerRay, out var landmarkHit))
        {
            var clone = Instantiate(_ammo, _barrel.position, _barrel.rotation, _ammosContainer);
            
            // Inverse rotation with the player pespective
            var platformRotation = Quaternion.Inverse(_crosshairRotation);

            // Create player marker in world space
            var playerSnappingDirection = landmarkHit.point - platformHit.point;
            playerSnappingDirection.Normalize();

            clone.Inject(new Target(platformHit.point, platformRotation, platformHit.normal, playerSnappingDirection));
        }
    }

    private void ChangeRotation(float value)
    {
        _crosshairRotation *= Quaternion.Euler(0f, 0f, value * _rotationAmount);
        var crossHairTransform = hand.equipmentCrossHair.transform;
        crossHairTransform.rotation = _crosshairRotation;
    }
}
