using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hand : MonoBehaviour
{
    [Header("Input")]
    [SerializeField]
    KeyCode key;

    [SerializeField]
    float dureeInputMenu;

    [SerializeField]
    Hand otherHand;

    float dureeInput;

    [Header("Menu Equipment")]
    [SerializeField]
    MenuEquip menuEquip;

    [SerializeField]
    Transform inventory;

    [Header("Mouse flow")]
    public float amount;
    public float maxAmountX;
    public float maxAmountY;
    public float smoothAmount;

    [Header("Movement flow")]
    [SerializeField]
    Rigidbody playerRB;
    public float xIntensityBreathing;
    public float yIntensityBreathing;
    public float xIntensityMovement;
    public float yIntensityMovement;
    public float speedB;
    public float speedM;
    public float speedLimit;

    float t = 0;

    float currentZ;
    float targetZ;

    [SerializeField]
    float initZ;

    [SerializeField]
    public Equipment currentEquipment;

    [SerializeField]
    Vector3 initialPos;

    Vector3 currentPos;

    [SerializeField]
    Vector3 rangerPos;

    [SerializeField]
    float dureeRangement;

    [SerializeField]
    float dureeSortie;

    public GameObject lastEquip;

    [SerializeField]
    GameObject canvasCrossHair;

    public GameObject equipmentCrossHair;

    bool enTransi = false;

    public int cote;

    Coroutine rangement;
    Coroutine sortir;

    // Start is called before the first frame update
    void Start()
    {
        currentPos = initialPos;
        initZ = currentPos.z;
        targetZ = initialPos.z;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(key))
        {
            dureeInput += Time.deltaTime;
        }

        if (dureeInput > dureeInputMenu)
        {
            //Debug.Log("MENU");
            if (!menuEquip.gameObject.activeSelf)
            {
                menuEquip.gameObject.SetActive(true);
            }
            if (menuEquip.hand != cote)
                menuEquip.setHand(cote);
        }

        if (Input.GetKeyUp(key))
        {
            if (dureeInput < dureeInputMenu)
            {
                if (enTransi)
                {
                    if (rangement != null)
                    {
                        stopRangement();
                        if (lastEquip != null)
                            selectEquipment(lastEquip);
                    }
                    else if (sortir != null)
                    {
                        stopSortir();
                        rangement = StartCoroutine(rangerEquipment());
                    }
                }
                else
                {
                    if (currentEquipment != null)
                    {
                        rangement = StartCoroutine(rangerEquipment());
                    }
                    else
                    {
                        if (lastEquip != null)
                            selectEquipment(lastEquip);
                    }
                }
            }
            else
            {
                CloseMenu();
            }

            dureeInput = 0;

        }

        if(!Input.GetKey(key) && menuEquip.gameObject.activeSelf && menuEquip.hand == cote)
        {
            CloseMenu();
        }

        float movementX = -Input.GetAxis("Mouse X") * amount;
        float movementY = -Input.GetAxis("Mouse Y") * amount;
        movementX = Mathf.Clamp(movementX, -maxAmountX, maxAmountX);
        movementY = Mathf.Clamp(movementY, -maxAmountY, maxAmountY);

        Vector3 finalPos = new Vector3(movementX, movementY, 0);

        transform.localPosition = Vector3.Lerp(transform.localPosition, finalPos + currentPos, Time.deltaTime * smoothAmount);

        if (playerRB.velocity.magnitude > speedLimit)
        {
            t += Time.deltaTime * speedM;
            transform.localPosition = transform.localPosition + new Vector3(Mathf.Cos(t) * xIntensityMovement, Mathf.Sin(t * 2) * yIntensityMovement, 0);

        }
        else
        {
            t += Time.deltaTime * speedB;
            transform.localPosition = transform.localPosition + new Vector3(Mathf.Cos(t) * xIntensityBreathing, Mathf.Sin(t * 2) * yIntensityBreathing, 0);

        }

        if (t > 99)
            t = 0;

        if (currentEquipment is Weapon)
        {
            Weapon currentWeapon = currentEquipment as Weapon;
            targetZ = Mathf.Lerp(targetZ, initZ, currentWeapon.knockbackSpeed * Time.deltaTime);
            currentZ = Mathf.Lerp(currentZ, targetZ, currentWeapon.knockbackSnappiness * Time.fixedDeltaTime);
        }

        transform.localPosition = new Vector3(transform.localPosition.x, transform.localPosition.y, currentZ);
    }

    public void KnockbackFire()
    {
        Weapon currentWeapon = currentEquipment as Weapon;
        targetZ -= currentWeapon.knockbackAmount;
    }

    public void CloseMenu()
    {
        dureeInput = 0;
        if (!menuEquip.canBeDisplayed)
            return;

        menuEquip.gameObject.SetActive(false);
        
        

        if (menuEquip.selection != -1)
        {
            if (currentEquipment != null)
            {
                leaveEquipment();
            }

            if (menuEquip.menuItemSc != null)
                selectEquipment(menuEquip.menuItemSc.equip.gameObject);
        }
    }

    public void newEquipment(GameObject newEquip)
    {
        leaveEquipment();
        selectEquipment(newEquip);
    }

    public void leaveEquipment()
    {
        if (currentEquipment != null)
        {
            currentEquipment.canBeUse = false;
            currentEquipment.notUsableEquipment();
            Destroy(equipmentCrossHair);
            currentEquipment.rangerEquipment();
            currentEquipment.gameObject.SetActive(false);
            currentEquipment.transform.parent = inventory;
            currentEquipment.hand = null;
            currentEquipment = null;

        }
    }

    IEnumerator rangerEquipment()
    {
        currentEquipment.canBeUse = false;
        currentEquipment.notUsableEquipment();
        Destroy(equipmentCrossHair);
        enTransi = true;

        float t = 0;
        while (t < 1)
        {
            t += Time.deltaTime / dureeRangement;
            currentPos = Vector3.Lerp(initialPos, rangerPos, t);
            transform.localPosition = currentPos;
            initZ = currentPos.z;
            yield return null;
        }

        currentPos = rangerPos;
        initZ = currentPos.z;
        currentEquipment.rangerEquipment();
        currentEquipment.gameObject.SetActive(false);
        currentEquipment.transform.parent = inventory;
        currentEquipment.hand = null;
        currentEquipment = null;

        enTransi = false;
        rangement = null;
    }

    IEnumerator sortirEquipment()
    {
        currentEquipment.sortirEquipment();

        float t = 0;
        while (t < 1)
        {
            t += Time.deltaTime / dureeSortie;
            currentPos = Vector3.Lerp(rangerPos, initialPos, t);
            transform.localPosition = currentPos;
            initZ = currentPos.z;
            yield return null;
        }
        currentPos = initialPos;
        initZ = currentPos.z;

        currentEquipment.usableEquipment();
        equipmentCrossHair = Instantiate(currentEquipment.crossHair, canvasCrossHair.transform);
        currentEquipment.canBeUse = true;

        enTransi = false;
        sortir = null;
    }

    public void selectEquipment(GameObject equipment)
    {
        Equipment equip = equipment.GetComponent<Equipment>();

        if (otherHand.currentEquipment == equip)
        {
            otherHand.stopRangement();
            otherHand.stopSortir();

            equip.canBeUse = false;
            equip.notUsableEquipment();
            Destroy(otherHand.equipmentCrossHair);
            equip.rangerEquipment();

            otherHand.currentEquipment = null;
        }

        enTransi = true;
        equipment.SetActive(true);
        equipment.transform.parent = this.transform;
        equipment.transform.localPosition = Vector3.zero;
        equipment.transform.rotation = new Quaternion();
        equip.transform.localScale = new Vector3(cote, 1, 1);
        lastEquip = equipment;
        currentEquipment = equip;
        currentEquipment.hand = this;

        if (currentEquipment.twoHanded || (otherHand.currentEquipment != null && otherHand.currentEquipment.twoHanded))
        {
            if (otherHand.sortir != null)
                otherHand.cancelSortir();
            else if (otherHand.currentEquipment != null)
            {
                otherHand.currentEquipment.canBeUse = false;
                otherHand.currentEquipment.notUsableEquipment();
                Destroy(otherHand.equipmentCrossHair);
                otherHand.currentEquipment.rangerEquipment();
                otherHand.stopRangement();

                if (otherHand.currentEquipment != null)
                {
                    otherHand.currentEquipment.transform.parent = inventory;
                    otherHand.currentEquipment = null;
                }
            }
        }

        sortir = StartCoroutine(sortirEquipment());
    }

    public void stopRangement()
    {
        if (rangement != null)
            StopCoroutine(rangement);
        rangement = null;

        currentPos = rangerPos;
        initZ = currentPos.z;
        currentEquipment.rangerEquipment();
        currentEquipment.gameObject.SetActive(false);
        currentEquipment.transform.parent = inventory;
        currentEquipment.hand = null;
        currentEquipment = null;
        enTransi = false;
    }

    public void stopSortir()
    {
        if (sortir == null)
            return;

        enTransi = false;

        StopCoroutine(sortir);
        sortir = null;
       
        currentPos = initialPos;
        initZ = currentPos.z;

        if (currentEquipment == null)
            return;
        currentEquipment.usableEquipment();
        currentEquipment.canBeUse = true;

        
    }

    public void cancelSortir()
    {
        if (sortir == null)
            return;

        StopCoroutine(sortir);
        sortir = null;

        currentEquipment.rangerEquipment();
        currentEquipment.gameObject.SetActive(false);
        currentEquipment.transform.parent = inventory;
        currentEquipment.hand = null;
        currentEquipment = null;
        enTransi = false;
    }
}
