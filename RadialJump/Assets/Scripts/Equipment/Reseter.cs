using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Reseter : Equipment
{
    [SerializeField]
    GameObject canvasEcran;

    [SerializeField]
    Image loadingImage;

    public float loadingTime;
    public float loadingSpeed;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (canBeUse && Input.GetMouseButton(twoHanded ? 0 : hand.cote == 1 ? 1 : 0))
            loadingTime += Time.deltaTime * loadingSpeed; 
       else if(loadingTime > 0)
           loadingTime -= Time.deltaTime * 2 * loadingSpeed;

        loadingImage.fillAmount = loadingTime;
        if (loadingTime >= 1)
        {
            Debug.Log("RESET LEVEL");
            SceneManager.LoadScene(0); // Temporaire, il faudra creer un etat initial a chaque salle et le charger
        }
           
    }

    public override void sortirEquipment()
    {
        model.SetActive(true);
        canvasEcran.transform.localScale = new Vector3(hand.cote == 1 ? 1 : -1, 1, 1);
    }

    public override void notUsableEquipment()
    {
        
    }

    public override void usableEquipment()
    {
        
    }
}
