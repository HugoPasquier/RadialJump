using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Equipment : MonoBehaviour
{
    [Header("Equipment settings")]
    [SerializeField]
    protected GameObject model;

    public bool canBeUse;

    public Hand hand;

    public bool twoHanded;

    public Color colorItemMenu;

    [SerializeField]
    public GameObject crossHair;

    //protected Crosshair crossHairScript;

    [TextArea]
    public string label;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public virtual void sortirEquipment()
    {
        model.SetActive(true);
    }

    public virtual void rangerEquipment()
    {
        model.SetActive(false);
    }

    public abstract void usableEquipment();

    public abstract void notUsableEquipment();
}
