using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorMultiple : Door
{
    [SerializeField]
    List<PlaquePression> plates;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public override void Open()
    {
        foreach (PlaquePression p in plates)
            if (!p.isActive)
                return;

        if (transition != null)
            StopCoroutine(transition);

        transition = StartCoroutine(openDoors());
    }

}
