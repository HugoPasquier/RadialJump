using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoWhiteTile : MonoBehaviour
{
    WhiteTile wt;

    public TileEffect tileEffect;
         
    // Start is called before the first frame update
    void Start()
    {
        wt = GetComponent<WhiteTile>();
        wt.setCurrentEffect(tileEffect);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
