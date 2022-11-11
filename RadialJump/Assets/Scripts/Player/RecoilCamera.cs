using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RecoilCamera : MonoBehaviour
{
    Vector3 currentRotation;
    Vector3 targetRotation;

    //Weapon currentWeapon;

    [SerializeField]
    PlayerCamera PC;

    [SerializeField]
    float returnSpeed;

    [SerializeField]
    float snappiness;


    void Update()
    {
        targetRotation = Vector3.Lerp(targetRotation, Vector3.zero, returnSpeed * Time.deltaTime);
        currentRotation = Vector3.Slerp(currentRotation, targetRotation, snappiness * Time.fixedDeltaTime);

        PC.recoil += currentRotation;
    }

    public void RecoilFire(Weapon w)
    {
        targetRotation += new Vector3(w.recoilX, Random.Range(-w.recoilY, w.recoilY), Random.Range(-w.recoilZ, w.recoilZ));
    }

}
