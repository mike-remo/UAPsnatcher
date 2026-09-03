using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using TMPro;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class MainMenu : MonoBehaviour
{
    [SerializeField] private GameObject menuObject1, menuObject2, menuObject3,
                                        initSelect1, initSelect2, initSelect3;
    private GameObject selectThis;
    [SerializeField] private TextMeshProUGUI videoModeText, gamepadStatusText, ModeInfoText;
    private bool inOptions = false;
    private float pollTimer = 0, pollNext = 2;
    private OptionsManager optMan;
    private bool wasChanged = false;
    private AudioSource audio;
    public Slider volumeSlider;
    [SerializeField] private AudioClip testSound;

    public void GameStart()
    {
        menuObject1.SetActive(false);
        menuObject3.SetActive(true);
        selectThis = initSelect3;
        EventSystem.current.SetSelectedGameObject(selectThis);
    }

    public void GameStartBack()
    {
        menuObject3.SetActive(false);
        menuObject1.SetActive(true);
        selectThis = initSelect1;
        EventSystem.current.SetSelectedGameObject(selectThis);
    }

    public void GameStartMode1()
    {
        optMan.gameMode = 1;
        SceneManager.LoadScene(1);
    }

    public void GameStartMode2()
    {
        optMan.gameMode = 2;
        SceneManager.LoadScene(1);
    }

    public void GameOptions()
    {
        menuObject1.SetActive(false);
        menuObject2.SetActive(true);
        inOptions = true;
        volumeSlider.value = optMan.options.volume;
        selectThis = initSelect2;
        EventSystem.current.SetSelectedGameObject(selectThis);
    }

    public string GetVideoMode() // Check and return enumerated video mode
    {
        switch (Screen.fullScreenMode)
        {
            case FullScreenMode.ExclusiveFullScreen:
                return "Fullscreen (Exclusive)";
            case FullScreenMode.FullScreenWindow:
                return "Fullscreen (Borderless Windowed)";
            case FullScreenMode.MaximizedWindow:
                return "Windowed (Maximized)";
            case FullScreenMode.Windowed:
                return "Windowed (Custom Size)";
            default:
                return "Unidentified Mode?!";
        }
    }

    public void PollDetectConfig()
    {
        pollTimer += Time.deltaTime;
        if (pollTimer < pollNext) return;
        videoModeText.SetText($"Current Video Mode: {GetVideoMode()}");
        if (Gamepad.current != null)
            gamepadStatusText.SetText("Gamepad Detected: YES");
        else
            gamepadStatusText.SetText("Gamepad Detected: NO");
        pollTimer = 0;
    }

    public void SetVolume() // Attach to slider's (OnValueChanged)
    {
        optMan.options.volume = volumeSlider.value;
        audio.volume = optMan.options.volume;
        wasChanged = true;

        if (audio && !audio.isPlaying) // Test volume changes
            audio.PlayOneShot(testSound);
    }

    public void GameOptionsBack()
    {
        menuObject2.SetActive(false);
        menuObject1.SetActive(true);
        inOptions = false;
        if (wasChanged)
        {
            optMan.SaveData();
            wasChanged = false;
        }
        selectThis = initSelect1;
        EventSystem.current.SetSelectedGameObject(selectThis);
    }

    public void GameQuit()
    {
        #if UNITY_EDITOR
            EditorApplication.ExitPlaymode();
        #else
            Application.Quit();
        #endif
    }

    public void Start()
    {
        GameObject.Find("OptionsManager").TryGetComponent<OptionsManager>(out optMan);
        GameObject.Find("MenuAudio").TryGetComponent<AudioSource>(out audio);
        if (audio && optMan)
            audio.volume = optMan.options.volume;
        selectThis = initSelect1;
    }

    public void Update()
    {
        if (inOptions) PollDetectConfig();
        if (EventSystem.current.currentSelectedGameObject == null)
            EventSystem.current.SetSelectedGameObject(selectThis);
    }
} // END