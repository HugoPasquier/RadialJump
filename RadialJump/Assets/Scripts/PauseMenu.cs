using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PauseMenu : MonoBehaviour
{
    [SerializeField]
    PlayerCamera PC;

    [SerializeField]
    GameObject pauseObject;

    [SerializeField]
    Slider slide;

    [SerializeField]
    PlayerSettings ps;

    public bool isPaused= false;

    int sceneIndex;

    // Start is called before the first frame update
    void Start()
    {
        sceneIndex = SceneManager.GetActiveScene().buildIndex;
        slide.value = ps.camSen;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && sceneIndex != 0 && sceneIndex != 4)
        {
            if (!isPaused)
                pauseGame();
            else
                resumeGame();
        }
    }

    void pauseGame()
    {
        isPaused = true;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        Time.timeScale = 0f;
        pauseObject.SetActive(true);
    }

    public void resumeGame()
    {
        isPaused = false;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        Time.timeScale = 1.0f;
        pauseObject.SetActive(false);
    }

    public void quitToMenu()
    {
        Time.timeScale = 1.0f;
        SceneManager.LoadScene(0);
    }

    public void updateSens()
    {
        ps.camSen = slide.value;
    }
}
