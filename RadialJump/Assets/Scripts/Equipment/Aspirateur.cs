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

    void Tir()
    {
        recoilSystem.RecoilFire(this);
        hand.KnockbackFire();
        currentCadence = 0;
    }
}
