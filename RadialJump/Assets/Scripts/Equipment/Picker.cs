using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class Picker : Weapon
{
    [SerializeField]
    protected LayerMask canBePick;

    public TileEffect pickedEffect;

    [SerializeField]
    TextMeshProUGUI pickedLabel;
    [SerializeField]
    GameObject canvasEcran;
    [SerializeField]
    Image pickedImage;
    [SerializeField]
    AudioClip soundRight;

    void Update()
    {
        if (currentCadence < cadenceCD)
            currentCadence += Time.deltaTime;

        if (canBeUse && Input.GetMouseButtonDown(1) && currentCadence > cadenceCD)
        {
            Pick();
        }
        else if(canBeUse && Input.GetMouseButtonDown(0) && currentCadence > cadenceCD)
        {
            Shoot();
        }
        else if (canBeUse && Input.GetMouseButtonDown(2) && currentCadence > cadenceCD)
        {
            ResetWhiteTile();
        }
    }

    void Pick()
    {
        recoilSystem.RecoilFire(this);
        hand.KnockbackFire();
        currentCadence = 0;
        audioSource.clip = soundRight;
        audioSource.Play();

        Ray ray = Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0f));
        RaycastHit hit = new RaycastHit();
        if (Physics.Raycast(ray, out hit, portee, canBePick))
        {
            if (hit.collider.CompareTag("CPGravity"))
            {
                pickedEffect = hit.collider.GetComponent<CheckPointGravity>();
            }
            else if (hit.collider.CompareTag("Bumper"))
            {
                pickedEffect = hit.collider.GetComponent<Bumper>();
            }

            pickedLabel.text = pickedEffect.label;
            pickedImage.color = pickedEffect.mat.color;
        }
    }

    void Shoot()
    {
        recoilSystem.RecoilFire(this);
        hand.KnockbackFire();
        currentCadence = 0;
        audioSource.clip = shootSound;
        audioSource.Play();

        Ray ray = Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0f));
        RaycastHit hit = new RaycastHit();
        if (Physics.Raycast(ray, out hit, portee, canBeShot))
        {
            hit.collider.GetComponent<WhiteTile>().setCurrentEffect(pickedEffect);
        }
    }

    void ResetWhiteTile()
    {
        recoilSystem.RecoilFire(this);
        hand.KnockbackFire();
        currentCadence = 0;

        Ray ray = Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0f));
        RaycastHit hit = new RaycastHit();
        if (Physics.Raycast(ray, out hit, portee, canBeShot))
        {
            hit.collider.GetComponent<WhiteTile>().resetWhiteTile();
        }
    }

    public override void sortirEquipment()
    {
        model.SetActive(true);
        canvasEcran.transform.localScale = new Vector3(1, hand.cote == 1 ? 1 : -1, 1);
    }
}
