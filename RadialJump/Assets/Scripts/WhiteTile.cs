using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class WhiteTile : MonoBehaviour
{
    TileEffect currentEffect;
    MeshRenderer MR;

    private void Awake()
    {
        MR = GetComponent<MeshRenderer>();
    }


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void setCurrentEffect(TileEffect effect)
    {
        Destroy(currentEffect);
        currentEffect = null;

        if (effect == null)
            return;

        currentEffect = gameObject.AddComponent(Type.GetType(effect.label)) as TileEffect;
        gameObject.tag = effect.tagEffect;
        MR.material.color = effect.mat.color;
    }

    public void resetWhiteTile()
    {
        Destroy(currentEffect);
        currentEffect = null;
        gameObject.tag = "WhiteTile";
        MR.material.color = Color.white;

    }
}
