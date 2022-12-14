using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    [SerializeField]
    Transform leftSide;

    [SerializeField]
    Transform rightSide;

    [SerializeField]
    float speed;

    public bool isOpen;

    Coroutine transition;

    // Start is called before the first frame update
    void Start()
    {
        if (isOpen)
        {
            Open();
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Open()
    {
        if (transition != null)
            StopCoroutine(transition);

        transition = StartCoroutine(openDoors());
    }

    public void Close()
    {
        if (transition != null)
            StopCoroutine(transition);

        transition = StartCoroutine(closeDoors());
    }

    IEnumerator openDoors()
    {
        while (leftSide.localPosition.x > -3.7f)
        {
            leftSide.localPosition = leftSide.localPosition - Vector3.right * speed * Time.deltaTime;
            rightSide.localPosition = rightSide.localPosition + Vector3.right * speed * Time.deltaTime;
            yield return null;
        }

        leftSide.localPosition = new Vector3(-3.7f, leftSide.localPosition.y, leftSide.localPosition.z);
        rightSide.localPosition = new Vector3(3.7f, rightSide.localPosition.y, rightSide.localPosition.z);

        isOpen = true;
    }

    IEnumerator closeDoors()
    {
        isOpen = false;

        while (leftSide.localPosition.x < -1.25f)
        {
            leftSide.localPosition = leftSide.localPosition + Vector3.right * speed * Time.deltaTime;
            rightSide.localPosition = rightSide.localPosition - Vector3.right * speed * Time.deltaTime;
            yield return null;
        }

        leftSide.localPosition = new Vector3(-1.25f, leftSide.localPosition.y, leftSide.localPosition.z);
        rightSide.localPosition = new Vector3(1.25f, rightSide.localPosition.y, rightSide.localPosition.z);
    }
}
