using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EndTrigger : MonoBehaviour
{
    [SerializeField]
    Animator transi;

    public int nextScene;

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
        if (other.CompareTag("Player"))
        {
            Debug.Log("Fin");
            StartCoroutine(endLevel(0.0f));
        }
    }

    public IEnumerator endLevel(float time)
    {
        yield return new WaitForSeconds(time);

        transi.SetTrigger("Close");
    }

    public void loadNextScene()
    {
        SceneManager.LoadScene(nextScene);
    }
}
