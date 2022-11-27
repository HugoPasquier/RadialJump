using Unity.Mathematics;
using UnityEngine;
using Quaternion = UnityEngine.Quaternion;
using Vector3 = UnityEngine.Vector3;

public class PlatformGun : Weapon
{
    [SerializeField] private Transform _barrel;
    [SerializeField] private PlatformAmmo _ammo;
    [SerializeField] private Transform _ammosContainer;
    [SerializeField, Range(0, 90)] private float _rotationAmount = 15;
    [SerializeField, Range(0f, 0.5f)] private float _orientationRadius = 0.01f;

    public float DebugRadius = 0.2f;
    public Transform ShowRadius;
    
    private Camera _camera;
    private Quaternion _crosshairRotation = Quaternion.identity;
    
    private Vector3 CENTER_SCREEN = new Vector3(0.5f, 0.5f, 0f);

    private void Awake() => _camera = Camera.main;

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

        // TODO delete debug only
        if (Input.GetMouseButtonDown(2))
        {
            int nbStep = 360 / (int) _rotationAmount;
            for (int i = 0; i < nbStep; i++)
            {
                float angle = DifformAngle(i * _rotationAmount);
                var orientationOnScreen = GetPositionOnCircle(CENTER_SCREEN, DebugRadius, angle);
                var orientationRay = _camera.ViewportPointToRay(orientationOnScreen);
                Physics.Raycast(orientationRay, out var orientationHit, portee, canBeShot);

                Instantiate(ShowRadius, orientationHit.point, Quaternion.identity);
            }
        }
    }
    
    public void Shoot()
    {
        recoilSystem.RecoilFire(this);
        hand.KnockbackFire();
        currentCadence -= cadenceCD;
        
        // We encounter an issue with platform orientation. Player can move around an object or change gravity,
        // so his axis are different to other objects. In short, we need to take in account the player orientation
        // with our platform creation.
        // In response, we use two rays from player perspective to create a direction and rotate a platform. Our first point
        // is the platform position and the second gives the orientation.
        var platformRay = _camera.ViewportPointToRay(CENTER_SCREEN);

        float angle = DifformAngle(_crosshairRotation.eulerAngles.z);
        var orientationOnScreen = GetPositionOnCircle(CENTER_SCREEN, _orientationRadius, angle);
        var orientationRay = _camera.ViewportPointToRay(orientationOnScreen);
        
        if (Physics.Raycast(platformRay, out RaycastHit platformHit, portee, canBeShot) && Physics.Raycast(orientationRay, out var orientationHit, portee, canBeShot))
        {
            var clone = Instantiate(_ammo, _barrel.position, _barrel.rotation, _ammosContainer);
            
            // Create player marker in world space
            var playerSnappingDirection = orientationHit.point - platformHit.point;
            
            clone.Inject(new Target(platformHit.point, platformHit.normal, playerSnappingDirection));
        }
    }

    private void ChangeRotation(float value)
    {
        _crosshairRotation *= Quaternion.Euler(0f, 0f, value * _rotationAmount);
        var crossHairTransform = hand.equipmentCrossHair.transform;
        crossHairTransform.rotation = _crosshairRotation;
    }

    private float DifformAngle(float angleInDegrees)
    {
        // Difformation is symmetrical
        float halfDegree = angleInDegrees % 180f;
        
        if (10f < halfDegree && halfDegree < 80f)
            return angleInDegrees + 15.0f;

        if (100f < halfDegree && halfDegree < 170f)
            return angleInDegrees - 15.0f;
        
        return angleInDegrees;
    }
    
    private Vector3 GetPositionOnCircle(in Vector3 center, float radius, float angleInDegrees)
    {
        return center + new Vector3(radius * Mathf.Cos(angleInDegrees * Mathf.Deg2Rad),
            radius * Mathf.Sin(angleInDegrees * Mathf.Deg2Rad));
    }
}
