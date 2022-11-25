using System;
using System.Numerics;
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
    [SerializeField] private Transform Orientation;
    
    private Camera _camera;

    private Quaternion _crosshairRotation = quaternion.identity;
    
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

        Ray ray = _camera.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0f));
        if (Physics.Raycast(ray, out RaycastHit hit, portee, canBeShot))
        {
            var clone = Instantiate(_ammo, _barrel.position, _barrel.rotation, _ammosContainer);
            
            var platformRotation = Quaternion.Inverse(_crosshairRotation);
            
            // Take the player rotation to orient an object on floor or ceil surface,
            // because he can rotate around an object and have a different perspective
            var dotProduct = Vector3.Dot(hit.normal, Orientation.transform.up);
            bool bHitGround = Math.Abs(dotProduct - 1f) < float.Epsilon;
            bool bHitCeil = Math.Abs(dotProduct + 1f) < float.Epsilon;
            if (bHitGround || bHitCeil)
            {
                platformRotation *= Quaternion.Euler(0f, 0f, Orientation.eulerAngles.y);
            }

            clone.Inject(new Target(hit.point, platformRotation, hit.normal));
        }
    }

    private void ChangeRotation(float value)
    {
        _crosshairRotation *= Quaternion.Euler(0f, 0f, value * _rotationAmount);
        var crossHairTransform = hand.equipmentCrossHair.transform;
        crossHairTransform.rotation = _crosshairRotation;
    }
}
