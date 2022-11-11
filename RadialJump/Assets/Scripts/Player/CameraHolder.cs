using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraHolder : MonoBehaviour
{
    [SerializeField]
    Transform cameraPos;

    void Update()
    {
        transform.position = cameraPos.transform.position;
    }
}
