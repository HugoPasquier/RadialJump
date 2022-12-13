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


    public void processBall(BasketHoopCollider col) {
        if (current == upCollider && col == downCollider) {
            ps.Play();
            door.Open();
        }
            

        current = col;
    }
    
}
