using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PickableObject : MonoBehaviour
{
    [Header("Gravity Settings")]
    [SerializeField]
    protected Vector3 customGravity;
    [SerializeField]
    float gravityMultiplier;

    Rigidbody rb;

    [Header("Grabbed ?")]
    public bool isGrabbed;



    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
    }

    private void FixedUpdate()
    {
        if (!isGrabbed)
            ApplyCustomGravity();
    }

    void ApplyCustomGravity()
    {
        rb.AddForce(customGravity * gravityMultiplier);
    }

}