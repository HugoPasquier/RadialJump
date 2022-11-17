using UnityEngine;

public class PlatformGun : Weapon
{
    [SerializeField] private Transform _barrel;
    [SerializeField] private PlatformAmmo _ammo;
    [SerializeField] private Transform _ammosContainer;
    [SerializeField] private float propulsionForce = 1.0f;
    
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
    }
    
    public void Shoot()
    {
        recoilSystem.RecoilFire(this);
        hand.KnockbackFire();
        currentCadence = 0;

        var clone = Instantiate(_ammo, _barrel.position, _barrel.rotation, _ammosContainer);
        clone.Rigidbody.AddForce(transform.forward * propulsionForce, ForceMode.Impulse);
        currentCadence -= cadenceCD;
    }
}
