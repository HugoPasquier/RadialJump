using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckPointGravity : MonoBehaviour
{
    public Vector3 gravity;

    [SerializeField]
    float gravityMagnitude;

    private void Awake()
    {
        gravity = transform.up * gravityMagnitude;
        gravity = new Vector3(Mathf.Abs(gravity.x) > 0.1f ? gravity.x : 0, Mathf.Abs(gravity.y) > 0.1f ? gravity.y : 0, Mathf.Abs(gravity.z) > 0.1f ? gravity.z : 0); 
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
