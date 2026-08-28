#if UNITY_EDITOR
using UnityEditor;
#endif

using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private GameObject menuObject1, menuObject2;
    private OptionsManager optionsManager;
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

        volumeSlider.value = optionsManager.options.volume;
    }

    public void SetVolume()
    {
        optionsManager.options.volume = volumeSlider.value;
        audio.volume = optionsManager.options.volume;

        if (audio && !audio.isPlaying)
            audio.PlayOneShot(testSound);
    }

    public void GameOptionsBack()
    {
        menuObject2.SetActive(false);
        menuObject1.SetActive(true);
        optionsManager.SaveData();
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
        GameObject.Find("OptionsManager").TryGetComponent<OptionsManager>(out optionsManager);
        GameObject.Find("MenuAudio").TryGetComponent<AudioSource>(out audio);
        if (audio && optionsManager)
            audio.volume = optionsManager.options.volume;
    }
}
