using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Aspirateur : Weapon
{
    public float ObjDistance = 1;
    public float offset;
    PickableObject grabObj = null;
    Vector3 prevPos, currentPos;
    public float forceAmount = 1;
    public float epsilon = 0;
    public float forceDiminution = 0.5f;


    // Update is called once per frame
    void Update()
    {
        prevPos = currentPos;
        currentPos = GrabPosition();

        if (currentCadence < cadenceCD)
            currentCadence += Time.deltaTime;

        if (canBeUse && Input.GetMouseButtonDown(twoHanded ? 0 : hand.cote == 1 ? 1 : 0) && currentCadence > cadenceCD)
        {
            Tir();
        }

        if (grabObj && canBeUse && Input.GetMouseButtonUp(twoHanded ? 0 : hand.cote == 1 ? 1 : 0)) {
            Lacher();
        }

        if(grabObj != null) {
            Vector3 delta = currentPos - grabObj.transform.position;
            if(delta.magnitude < epsilon) {
                grabObj.Move(delta * forceAmount * forceDiminution);
            } else {
                grabObj.Move(delta * forceAmount);
            }
        }
    }

    Vector3 GrabPosition(){
        //Debug.DrawLine(transform.position, transform.position + (transform.forward) * 10, Color.blue, 30);
        
        return transform.position + (transform.forward) * ObjDistance + transform.right * offset * (hand.cote == 1 ? -1 : 1);
    }

    void Tir()
    {
        recoilSystem.RecoilFire(this);
        hand.KnockbackFire();
        currentCadence = 0;
        Debug.Log("Shooting");
        Ray ray = Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0f));
        RaycastHit hit = new RaycastHit();
        if (Physics.Raycast(ray, out hit, portee, canBeShot))
        {
            Debug.Log("Touching smth");
            Debug.DrawLine(transform.position, hit.point, Color.red, 60);
            if (hit.collider.CompareTag("Pickable"))
            {
                Debug.Log("Aspirated to : " + GrabPosition());
                //hit.collider.gameObject.transform.position = GrabPosition();
                grabObj = hit.collider.gameObject.GetComponent<PickableObject>();
                grabObj.Picked();
            }
        }
    }

    void Lacher() {
        grabObj.Unpicked();
        grabObj = null;
    }
}
