using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class MenuEquip : MonoBehaviour
{
    [SerializeField]
    GameObject canvas;

    [SerializeField]
    GameObject itemPrefab;

    [SerializeField]
    List<ItemMenuEquip> items = new List<ItemMenuEquip>();

    [SerializeField]
    Inventory inventory;

    [SerializeField]
    PlayerCamera playerCam;

    [SerializeField]
    TextMeshProUGUI handLabel;

    public Vector2 normalisedMousePosition;
    public float currentAngle;
    public int selection;
    int previousSelection;
    public float minDist;

    public bool inCircle;

    public ItemMenuEquip menuItemSc;
    public ItemMenuEquip previousMenuItem;

    public int hand;

    public bool canBeDisplayed = false;

    private void Awake()
    {
        handLabel = GetComponent<TextMeshProUGUI>();
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    private void OnEnable()
    {
        playerCam.inMenu = true;
        updateMenuItems();
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        canBeDisplayed = true;
    }

    private void OnDisable()
    {
        playerCam.inMenu = false;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        canBeDisplayed = false;
    }

    public void setHand(int h)
    {
        hand = h;
        if (hand == 1)
            handLabel.text = "Right\nHand";
        else
            handLabel.text = "Left\nHand";
    }

    // Update is called once per frame
    void Update()
    {
        normalisedMousePosition = new Vector2(Input.mousePosition.x - this.transform.position.x, Input.mousePosition.y - this.transform.position.y);
        inCircle = normalisedMousePosition.magnitude > minDist;

        if (inCircle)
        {
            currentAngle = Mathf.Atan2(normalisedMousePosition.y, normalisedMousePosition.x) * Mathf.Rad2Deg;
            currentAngle = (currentAngle + 360) % 360;

            selection = (int)currentAngle / ((int)360.0f / items.Count);

            if (selection != previousSelection)
            {
                if (previousSelection != -1)
                {
                    previousMenuItem = items[previousSelection];
                    previousMenuItem.Deselect();
                }
                previousSelection = selection;
                menuItemSc = items[selection];
                menuItemSc.Selected();
            }
        }
        else
        {
            if (selection != -1)
            {
                selection = -1;
                previousMenuItem = items[previousSelection];
                previousMenuItem.Deselect();
                previousSelection = selection;
            }
        }
    }

    void updateMenuItems()
    {

        foreach (Transform child in transform)
        {
            GameObject.Destroy(child.gameObject);
        }

        items.Clear();

        int i = 0;
        foreach (Equipment e in inventory.equipements)
        {
            GameObject go = Instantiate(itemPrefab, transform);
            items.Add(go.GetComponent<ItemMenuEquip>());
            items[i].init(i, inventory.equipements.Count, e);
            i++;
        }
    }
}
