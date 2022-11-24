using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SphereManager : MonoBehaviour
{
    public List<PickableSphere> spheres = new List<PickableSphere>();

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void UpdateGravity(Vector3 newGravity)
    {
        foreach (PickableSphere s in spheres)
            s.setCustomGravity(newGravity);
    }
}