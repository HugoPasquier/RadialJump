using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasketHoop : MonoBehaviour {
    
    [SerializeField]
    ParticleSystem ps;
    private Vector3 old_pos;
    
    private void OnTriggerEnter(Collider other) {
        old_pos = other.transform.position;

    }

    private void OnTriggerExit(Collider other) {

        if (other.transform.position.y < old_pos.y && other.gameObject.tag == "ball") {
            ps.Play();
        }
    }
}
