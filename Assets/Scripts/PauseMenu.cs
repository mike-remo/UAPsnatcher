using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    [SerializeField] private GameObject gameUI, pauseUI, selectThis;
    private GameObject playerInputObj;
    private PlayerInputHandler playerInput;
    private bool isPaused;
    public void OpenPauseMenu()
    {
        isPaused = true;
        playerInputObj.SetActive(false);
        gameUI.SetActive(false);
        pauseUI.SetActive(true);
        Time.timeScale = 0f;
    }

    public void ClosePauseMenu()
    {
        isPaused = false;
        pauseUI.SetActive(false);
        gameUI.SetActive(true);
        playerInputObj.SetActive(true);
        Time.timeScale = 1f;
    }
    public void BackMainMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(0);
    }

    void Start()
    {
        playerInputObj = GameObject.Find("PlayerInputHandler");
        playerInput = playerInputObj.GetComponent<PlayerInputHandler>();
        isPaused = false;
    } 

    void Update()
    {
        if(!isPaused)
            if (playerInput.button3Pressed)
                OpenPauseMenu();
        if (EventSystem.current.currentSelectedGameObject == null)
            EventSystem.current.SetSelectedGameObject(selectThis);
    }
} // END