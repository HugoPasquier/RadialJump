using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasketHoop : MonoBehaviour {
    
    [SerializeField]
    ParticleSystem ps;
    private Vector3 old_pos;

    [SerializeField]
    BasketHoopCollider upCollider;

    [SerializeField]
    BasketHoopCollider downCollider;

    BasketHoopCollider current = null;

    [SerializeField]
    Door door;


    [SerializeField]
    bool MultipleHoopDoor = false;
    [SerializeField]
    List<BasketHoop> hoops;

    public bool isActive = false;

    public void processBall(BasketHoopCollider col) {
        if (current == upCollider && col == downCollider) {
            if (MultipleHoopDoor){
                bool openDoor = true;
                foreach (BasketHoop hoop in hoops){
                    openDoor = openDoor && hoop.isActive;
                }

                if (openDoor){
                    door.Open();
                }
            }
            else {
                door.Open();
            }
            isActive = true;
            ps.Play();

        }
            

        current = col;
    }
    
}
