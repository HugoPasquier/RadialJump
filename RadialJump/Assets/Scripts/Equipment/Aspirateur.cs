using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Aspirateur : Weapon
{
    // Update is called once per frame
    void Update()
    {
        if (currentCadence < cadenceCD)
            currentCadence += Time.deltaTime;

        if (canBeUse && Input.GetMouseButtonDown(twoHanded ? 0 : hand.cote == 1 ? 1 : 0) && currentCadence > cadenceCD)
        {
            Tir();
        }
    }

    Vector3 GrabPosition(){
        return new Vector3(0, 0, 1) + Camera.main.ViewportToWorldPoint(new Vector3(0.5f, 0.5f, 0f));
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
            if (hit.collider.CompareTag("Aspirated"))
            {
                Debug.Log("Aspirated to : " + GrabPosition());
                hit.collider.gameObject.transform.position = GrabPosition();
            }
        }
    }
}
