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
    [SerializeField]
    float normalDrag;
    [SerializeField]
    float normalRotDrag;
    [SerializeField]
    float pickedDrag;
    [SerializeField]
    float pickedRotDrag;


    Rigidbody rb;

    [Header("Grabbed ?")]
    public bool isGrabbed;


    public void Picked() {
        isGrabbed = true;
        rb.drag = pickedDrag;
        rb.angularDrag = pickedRotDrag;
        rb.constraints = RigidbodyConstraints.FreezeRotation;
    }

    public void Unpicked() {
        isGrabbed = false;
        rb.drag = normalDrag;
        rb.angularDrag = normalRotDrag;
        rb.constraints = RigidbodyConstraints.None;
    }

    public void Move(Vector3 delta) {
        rb.AddForce(delta, ForceMode.VelocityChange);
    }
    
    public void Repulse(Vector3 delta){
        rb.AddForce(delta, ForceMode.Impulse);
    }


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