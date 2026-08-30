using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class MainMenu : MonoBehaviour
{
    [SerializeField] private GameObject menuObject1, menuObject2;
    [SerializeField] private TextMeshProUGUI videoText;
    private bool inOptions = false;
    private float pollTimer = 0, pollNext = 2;
    private OptionsManager optMan;
    private bool wasChanged = false;
    private AudioSource audio;
    public Slider volumeSlider;
    [SerializeField] private AudioClip testSound;

    public void GameStart()
    {
        SceneManager.LoadScene(1);
    }

    public void GameOptions()
    {
        menuObject1.SetActive(false);
        menuObject2.SetActive(true);
        inOptions = true;
        volumeSlider.value = optMan.options.volume;
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
    }

    public void Update()
    {
        if (!inOptions) return;
        pollTimer += Time.deltaTime;
        if (pollTimer < pollNext) return;
        videoText.SetText($"Current Video Mode: {GetVideoMode()}");
        pollTimer = 0;
    }
} // END