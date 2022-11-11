using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

[RequireComponent(typeof(Image))]
public class ItemMenuEquip : MonoBehaviour
{
    public Equipment equip;
    Image im;
    [SerializeField]
    TextMeshProUGUI label;
    [SerializeField]
    float distLabel;

    float hColor;
    Color itemColor;

    public void init(int index, int maxIndex, Equipment equipment)
    {
        im = GetComponent<Image>();
        float a, b;
        Color.RGBToHSV(equipment.colorItemMenu, out hColor, out a, out b);
        itemColor = Color.HSVToRGB(hColor, 0.5f, 1f);
        itemColor.a = 0.7f;
        im.color = itemColor;
        im.fillAmount = 1.0f / ((float)maxIndex);
        transform.rotation = Quaternion.Euler(0, 0, index * (360f / ((float)maxIndex)));
        equip = equipment;
        string hand = "";
        if (equip.hand != null)
            hand = "\n" + (equip.hand.cote == 1 ? "(R)" : "(L)");

        label.text = equip.label + hand;
        label.transform.localPosition = new Vector3(distLabel * Mathf.Cos(Mathf.Deg2Rad * ((360f / ((float)maxIndex)) * -0.5f)), distLabel * Mathf.Sin(Mathf.Deg2Rad * ((360f / ((float)maxIndex)) * -0.5f)), 0);
        label.transform.rotation = Quaternion.Euler(0, 0, -transform.rotation.z);
    }

    public void Selected()
    {
        itemColor = Color.HSVToRGB(hColor, 0.9f, 1f);
        itemColor.a = 0.7f;
        im.color = itemColor;
    }

    public void Deselect()
    {
        itemColor = Color.HSVToRGB(hColor, 0.5f, 1f);
        itemColor.a = 0.7f;
        im.color = itemColor;
    }
}
