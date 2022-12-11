using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaquePression : MonoBehaviour
{
    [SerializeField]
    Transform plaque;

    [SerializeField]
    Transform cables;

    [SerializeField]
    Color enabledColor;

    [SerializeField]
    Color disabledColor;

    [SerializeField]
    float speedTransition;

    [SerializeField]
    Door door;

    Material plaqueMat;

    public List<GameObject> onPlate;

    List<Material> matCables = new List<Material>();

    public bool isActive;

    Coroutine transition;

    private void Awake()
    {
        plaqueMat = plaque.GetComponent<Renderer>().material;
        foreach (Transform t in cables)
            matCables.Add(t.GetComponent<Renderer>().material);
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Pickable") || other.CompareTag("Player"))
        {
            onPlate.Add(other.gameObject);
            if(!isActive && transition == null)
            {
                if (transition != null)
                    StopCoroutine(transition);
                transition = StartCoroutine(enablePressure());
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Pickable") || other.CompareTag("Player"))
        {
            onPlate.Remove(other.gameObject);
            if(onPlate.Count <= 0)
            {
                if (transition != null)
                    StopCoroutine(transition);
                transition = StartCoroutine(disablePressure());
            }
        }
    }

    IEnumerator enablePressure()
    {
        plaqueMat.color = enabledColor;
        plaqueMat.EnableKeyword("_EMISSION");
        plaqueMat.SetColor("_EmissionColor", enabledColor);
        DynamicGI.UpdateEnvironment();

        while(plaque.localPosition.y > 0.05f)
        {
            plaque.localPosition = plaque.localPosition - Vector3.up * speedTransition * Time.deltaTime; 
            yield return null;
        }

        foreach(Material m in matCables)
        {
            m.color = enabledColor;
            m.EnableKeyword("_EMISSION");
            m.SetColor("_EmissionColor", enabledColor);
        }
        DynamicGI.UpdateEnvironment();

        isActive = true;
        if (door is DoorMultiple)
            (door as DoorMultiple).Open();
        else
            door.Open();


        transition = null;
    }

    IEnumerator disablePressure()
    {
        plaqueMat.color = disabledColor;
        plaqueMat.DisableKeyword("_EMISSION");
        DynamicGI.UpdateEnvironment();
        
        if (isActive)
        {
            isActive = false;
            door.Close();
            foreach (Material m in matCables)
            {
                m.color = disabledColor;
                m.DisableKeyword("_EMISSION");
            }
            DynamicGI.UpdateEnvironment();
        }
        

        while (plaque.localPosition.y < 0.15f)
        {
            plaque.localPosition = plaque.localPosition + Vector3.up * speedTransition * Time.deltaTime;
            yield return null;
        }

        transition = null;
    }
}