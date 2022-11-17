using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PlayerMovement : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField]
    Transform orientation;
    public float moveSpeed;
    public float groundDrag;
    public float jumpForce;
    public float jumCooldown;
    public float gravityMultiplier;
    public float airMultiplier;
    bool readyToJump = true;
    public float rotationGrav;

    [Header("Keybinds")]
    public KeyCode jumpKey = KeyCode.Space;

    [Header("Ground Check")]
    public float playerHeight;
    public LayerMask whatIsGround;
    bool grounded;

    [Header("Current Level Settings")]
    public SphereManager sphereManager;

    

    float horizontalInput;
    float verticalInput;
    Vector3 moveDirection;
    Rigidbody rb;
    Vector3 customGravity;

    

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;
        customGravity = Physics.gravity;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //Basic Ground Check (pas optimal mais fonctionnel pour un debut, voir a remplacer le raycast par un spherecast)
        Debug.DrawLine(transform.position, transform.position - (transform.up * (playerHeight * 0.5f + 0.2f)), Color.red, 1);

        grounded = Physics.Raycast(transform.position, -transform.up, playerHeight * 0.5f + 0.2f, whatIsGround);

        CheckInputs();
        SpeedControl();

        if (grounded)
            rb.drag = groundDrag;
        else
            rb.drag = 0;
    }

    private void FixedUpdate()
    {
        MovePlayer();
    }

    void CheckInputs()
    {
        horizontalInput = Input.GetAxisRaw("Horizontal");
        verticalInput = Input.GetAxisRaw("Vertical");
        if (Input.GetKeyDown(jumpKey) && readyToJump && grounded)
        {
            readyToJump = false;
            Jump();
            Invoke(nameof(ResetJump), jumCooldown);
        }
    }

    void SpeedControl()
    {
        Vector3 flatVel = (new Vector3(1, 1, 1) - new Vector3(Mathf.Abs(orientation.up.x), Mathf.Abs(orientation.up.y), Mathf.Abs(orientation.up.z)) );
        flatVel.x *= rb.velocity.x;
        flatVel.y *= rb.velocity.y;
        flatVel.z *= rb.velocity.z;

        if (flatVel.magnitude > moveSpeed)
        {
            Vector3 limitedVel = flatVel.normalized * moveSpeed;
            rb.velocity = new Vector3(Mathf.Abs(limitedVel.x) > 0.01f ? limitedVel.x : rb.velocity.x, Mathf.Abs(limitedVel.y) > 0.01f ? limitedVel.y : rb.velocity.y, Mathf.Abs(limitedVel.z) > 0.01f ? limitedVel.z : rb.velocity.z);
        }
    }

    void Jump()
    {
        rb.velocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);
        rb.AddForce(transform.up * jumpForce, ForceMode.Impulse);
    }

    private void ResetJump()
    {
        readyToJump = true;
    }

    void MovePlayer()
    {
        moveDirection = orientation.forward * verticalInput + orientation.right * horizontalInput;
        if (grounded)
            rb.AddForce(moveDirection.normalized * moveSpeed * 10f, ForceMode.Force);
        else
        {
            rb.AddForce(moveDirection.normalized * moveSpeed * 10f * airMultiplier, ForceMode.Force);
            rb.AddForce(customGravity * gravityMultiplier);
        }
    }

    public void setCustomGravity(Vector3 newGravity)
    {
        if (Vector3.Distance(customGravity, newGravity) < 0.1f)
            return;

        customGravity = newGravity;
        StartCoroutine(ChangeGravityOrientation());

        if (sphereManager != null)
            sphereManager.UpdateGravity(newGravity);
    }

    IEnumerator ChangeGravityOrientation()
    {
        Vector3 gravityUp = -customGravity.normalized;
        Quaternion targetRotation = Quaternion.FromToRotation(transform.up, gravityUp) * transform.rotation;
        orientation.up = gravityUp;
        float t = 0;
        while(t < 1)
        {
            t += rotationGrav * Time.deltaTime;
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, t);
            yield return null;
        }
        transform.rotation = targetRotation;
    }
}
