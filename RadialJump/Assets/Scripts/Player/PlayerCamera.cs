using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCamera : MonoBehaviour
{
    [Header("Mouse Sensibility")]
    [SerializeField]
    public float sensX;
    [SerializeField]
    public float sensY;

    [SerializeField]
    PlayerSettings ps;

    [Header("Other Settings")]
    [SerializeField]
    Transform orientation;

    float xRotation;
    float yRotation;

    public Vector3 recoil;
    public bool inMenu = false;

    // Start is called before the first frame update
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (inMenu)
            return;

        float mouseX = Input.GetAxisRaw("Mouse X") * Time.deltaTime * (600f * (ps.camSen / 100.0f)); //sensX;
        float mouseY = Input.GetAxisRaw("Mouse Y") * Time.deltaTime * (600f * (ps.camSen / 100.0f)); //sensY;

        yRotation += mouseX;

        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90);

        transform.localRotation = Quaternion.Euler(new Vector3(xRotation, yRotation, 0) + recoil);
        recoil = Vector3.zero;
        orientation.localRotation = Quaternion.Euler(0, yRotation, 0);
    }
}
