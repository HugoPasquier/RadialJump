using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GravityChanger : Weapon
{
    PlayerMovement PM;

    void Start()
    {
        recoilSystem = GameObject.Find("Player/CameraHolder").GetComponent<RecoilCamera>();
        PM = GameObject.Find("Player").GetComponent<PlayerMovement>();
    }


    void Update()
    {
        if (currentCadence < cadenceCD)
            currentCadence += Time.deltaTime;

        if (canBeUse && Input.GetMouseButtonDown(twoHanded ? 0 : hand.cote == 1 ? 1 : 0) && currentCadence > cadenceCD)
        {
            Tir();
        }
    }

    void Tir()
    {
        recoilSystem.RecoilFire(this);
        hand.KnockbackFire();
        currentCadence = 0;

        Ray ray = Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0f));
        RaycastHit hit = new RaycastHit();
        if (Physics.Raycast(ray, out hit, portee, canBeShot))
        {
            if (hit.collider.CompareTag("CPGravity"))
            {
                PM.setCustomGravity(hit.collider.gameObject.GetComponent<CheckPointGravity>().gravity);
            }
        }
    }
}
