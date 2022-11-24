using System;
using Unity.Mathematics;
using UnityEngine;

public class PlatformGun : Weapon
{
    [SerializeField] private Transform _barrel;
    [SerializeField] private PlatformAmmo _ammo;
    [SerializeField] private Transform _ammosContainer;
    [SerializeField] private float _rotationAmount = 15f;
    
    private Camera _camera;

    private Quaternion _platformRotation = quaternion.identity;
    
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
            clone.Inject(new Target(hit.point, _platformRotation, hit.normal));
        }
    }

    private void ChangeRotation(float value)
    {
        _platformRotation *= Quaternion.Euler(0f, 0f, value * _rotationAmount);

        var crossHairTransform = hand.equipmentCrossHair.transform;
        crossHairTransform.rotation = _platformRotation;
    }
}
