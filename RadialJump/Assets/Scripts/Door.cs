using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    [SerializeField]
    Transform leftSide;

    [SerializeField]
    Transform rightSide;

    [SerializeField] private float _maxPosition = 3.7f;
    [SerializeField] private float _minPosition = 1.25f;

    [SerializeField]
    float speed;

    public bool isOpen;

    protected Coroutine transition;

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

    public virtual void Open()
    {
        if (transition != null)
            StopCoroutine(transition);

        transition = StartCoroutine(openDoors());
    }

    public virtual void Close()
    {
        if (transition != null)
            StopCoroutine(transition);

        transition = StartCoroutine(closeDoors());
    }

    protected IEnumerator openDoors()
    {
        while (leftSide.localPosition.x > -_maxPosition)
        {
            leftSide.localPosition = leftSide.localPosition - Vector3.right * speed * Time.deltaTime;
            rightSide.localPosition = rightSide.localPosition + Vector3.right * speed * Time.deltaTime;
            yield return null;
        }

        leftSide.localPosition = new Vector3(-_maxPosition, leftSide.localPosition.y, leftSide.localPosition.z);
        rightSide.localPosition = new Vector3(_maxPosition, rightSide.localPosition.y, rightSide.localPosition.z);

        isOpen = true;
    }

    protected IEnumerator closeDoors()
    {
        isOpen = false;

        while (leftSide.localPosition.x < -_minPosition)
        {
            leftSide.localPosition = leftSide.localPosition + Vector3.right * speed * Time.deltaTime;
            rightSide.localPosition = rightSide.localPosition - Vector3.right * speed * Time.deltaTime;
            yield return null;
        }

        leftSide.localPosition = new Vector3(-_minPosition, leftSide.localPosition.y, leftSide.localPosition.z);
        rightSide.localPosition = new Vector3(_minPosition, rightSide.localPosition.y, rightSide.localPosition.z);
    }
}
