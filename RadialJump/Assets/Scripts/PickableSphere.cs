using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickableSphere : PickableObject
{
    public void setCustomGravity(Vector3 newGravity)
    {
        if (Vector3.Distance(customGravity, newGravity) < 0.1f)
            return;

        customGravity = newGravity;
    }
}