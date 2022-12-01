using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EquipmentGift : MonoBehaviour
{
    [SerializeField]
    float speedRotation;

    [SerializeField]
    GameObject prefabEquipment;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(transform.up, speedRotation * Time.deltaTime);
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log(other.gameObject);
        if (other.CompareTag("Player"))
        {
            GameObject inventory = other.gameObject.transform.parent.Find("Inventory").gameObject;
            Inventory inventoryScript = inventory.GetComponent<Inventory>();

            GameObject newEquipment = Instantiate(prefabEquipment);
            newEquipment.transform.SetParent(inventory.transform);
            newEquipment.transform.localPosition = Vector3.zero;
            newEquipment.transform.localRotation = Quaternion.identity;
            inventoryScript.equipements.Add(newEquipment.GetComponent<Equipment>());

            other.gameObject.transform.parent.Find("CameraHolder/PlayerCamera/RightHand").GetComponent<Hand>().newEquipment(newEquipment);

            Destroy(this.gameObject);
        }
    }
}
