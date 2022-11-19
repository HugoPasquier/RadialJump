using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PlayerMovement : MonoBehaviour
{
    [Header("Movement")]
    public float moveSpeed;
    public float groundDrag;
    public float jumpForce;
    public float jumCooldown;
    public float gravityMultiplier;
    public float airMultiplier;
    bool readyToJump = true;

    [Header("Keybinds")]
    public KeyCode jumpKey = KeyCode.Space;

    [Header("Ground Check")]
    public float playerHeight;
    public LayerMask whatIsGround;
    public float groundRadius = 0.05f;
    bool grounded;

    [SerializeField]
    Transform orientation;

    float horizontalInput;
    float verticalInput;
    Vector3 moveDirection;
    Rigidbody rb;

    Vector3 customGravity;

    public float rotationGrav;

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
        var origin = transform.position - transform.up * playerHeight / 2.0f;
        grounded = Physics.CheckSphere(origin, groundRadius, whatIsGround);
        
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
            rb.AddForce(moveDirection.normalized * (moveSpeed * 10f), ForceMode.Force);
        else
        {
            rb.AddForce(moveDirection.normalized * (moveSpeed * 10f * airMultiplier), ForceMode.Force);
            rb.AddForce(customGravity * gravityMultiplier);
        }
    }

    public void setCustomGravity(Vector3 newGravity)
    {
        if (Vector3.Distance(customGravity, newGravity) < 0.1f)
            return;

        customGravity = newGravity;
        StartCoroutine(ChangeGravityOrientation());
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

    private void OnDrawGizmos()
    {
        var origin = transform.position - transform.up * playerHeight / 2.0f;
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(origin, groundRadius);
    }
}
