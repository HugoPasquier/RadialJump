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
    bool isClose = false;
    public float closeDistance = 1;
    public float grabForceAmount = 2;
    public float repulseForceAmount = 0.3f;
    public AudioClip repulseSound;


    // Update is called once per frame
    void Update()
    {
        prevPos = currentPos;
        currentPos = GrabPosition();

        // Check if cooldown is up
        if (currentCadence < cadenceCD)
            currentCadence += Time.deltaTime;

        // Drag the object to the player
        if (canBeUse && Input.GetMouseButtonDown(twoHanded ? 0 : hand.cote == 1 ? 1 : 0) && currentCadence > cadenceCD)
        {
            Tir();
        }

        // Release the object
        if (grabObj && canBeUse && Input.GetMouseButtonUp(0)) {
            Lacher();
        }

        // Repulse the object
        if (grabObj && canBeUse && Input.GetMouseButton(1)) {
            Repulse();
        }

        
    }

    private void FixedUpdate() {

        // Maintain the object in front of the player (if grabbed)
        if (grabObj != null) {
            Vector3 delta = currentPos - grabObj.transform.position;
            if (!isClose) {
                grabObj.Move(delta * grabForceAmount);
                if (delta.magnitude < closeDistance)
                    isClose = true;
            } else {
                if (delta.magnitude < epsilon) {
                    grabObj.Move(delta * forceAmount * forceDiminution);
                } else {
                    grabObj.Move(delta * forceAmount);
                }
            }

        }
    }

    // Determine the position in front of the player where the grabbed object will be
    Vector3 GrabPosition(){
        return transform.position + (transform.forward) * ObjDistance + transform.right * offset * (hand.cote == 1 ? -1 : 1);
    }

    void Tir()
    {
        recoilSystem.RecoilFire(this);
        hand.KnockbackFire();
        audioSource.PlayOneShot(shootSound);
        currentCadence = 0;

        Ray ray = Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0f));
        RaycastHit hit = new RaycastHit();
        if (Physics.Raycast(ray, out hit, portee, canBeShot))
        {
            if (hit.collider.CompareTag("Pickable") || hit.collider.CompareTag("ball"))
            {
                grabObj = hit.collider.gameObject.GetComponent<PickableObject>();
                grabObj.Picked();
            }
        }
    }

    void Lacher() {
        grabObj.Unpicked();
        grabObj = null;
        isClose = false;
    }

    void Repulse() {
        Vector3 delta = Camera.main.transform.forward;
        delta.Normalize();
        grabObj.Repulse(delta * repulseForceAmount);
        audioSource.PlayOneShot(repulseSound);
        Lacher();
    }
        
}
