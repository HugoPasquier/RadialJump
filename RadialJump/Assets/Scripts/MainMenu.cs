using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenu : MonoBehaviour
{
    [SerializeField]
    PlaquePression PP;

    [SerializeField]
    GameObject Canvas;

    [SerializeField]
    EndTrigger ET;


    [SerializeField]
    Transform camT;

    [SerializeField]
    float camSpeed;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void startButtonClick()
    {
        PP.activePlate();
        Canvas.SetActive(false);
        StartCoroutine(moveCamera());
        StartCoroutine(ET.endLevel(3));
    }

    public void quitButton()
    {
        Application.Quit();
    }

    IEnumerator moveCamera()
    {
        while (true)
        {
            camT.position += Vector3.right * camSpeed * Time.deltaTime;
            yield return null;
        }
    }
}
