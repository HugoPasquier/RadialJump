using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasketHoopCollider : MonoBehaviour
{
    [SerializeField]
    BasketHoop basketHoop;
    private void OnTriggerEnter(Collider other) {
        if (other.gameObject.tag == "ball") {
            basketHoop.processBall(this);
        }
    }
}
