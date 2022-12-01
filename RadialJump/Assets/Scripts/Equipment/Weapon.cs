using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Weapon : Equipment
{
    [Header("General settings")]
    public float cadenceCD;
    protected float currentCadence;

    [SerializeField]
    protected float portee;

    [SerializeField]
    protected AudioSource audioSource;

    [SerializeField]
    protected AudioClip shootSound;

    [SerializeField]
    protected LayerMask canBeShot;

    [Header("Recoil settings")]
    [SerializeField]
    public float recoilX;

    [SerializeField]
    public float recoilY;

    [SerializeField]
    public float recoilZ;

    protected RecoilCamera recoilSystem;

    [Header("Knockback settings")]
    [SerializeField]
    public float knockbackSnappiness;

    [SerializeField]
    public float knockbackSpeed;

    [SerializeField]
    public float knockbackAmount;


    // Start is called before the first frame update
    protected virtual void Start()
    {
        recoilSystem = GameObject.Find("Player/CameraHolder").GetComponent<RecoilCamera>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public override void usableEquipment()
    {
    }
    
    public override void notUsableEquipment()
    {
    }
}
