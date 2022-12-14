using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HoopMovement : MonoBehaviour
{
    [SerializeField]
    bool vertical = false;
    public float speed = 1;
    public float distance = 5;

    private Vector3 origin;

    void Movel(){
        if (Vector3.Distance(origin, transform.position) > distance){
            speed = -speed;
        }
        if(vertical) transform.position += transform.up * speed * Time.deltaTime;
        else transform.position += transform.right * speed * Time.deltaTime;
    }


    // Start is called before the first frame update
    void Start()
    {
        origin = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        Movel();
    }
}
