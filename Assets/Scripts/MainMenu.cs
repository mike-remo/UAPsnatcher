#if UNITY_EDITOR
using UnityEditor;
#endif

using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private GameObject menuObject1, menuObject2;

    public void GameStart()
    {
         SceneManager.LoadScene(1);
    }

    public void GameOptions()
    {
        menuObject1.SetActive(false);
        menuObject2.SetActive(true);
    }

    public void GameOptionsBack()
    {
        menuObject2.SetActive(false);
        menuObject1.SetActive(true);
    }

    public void GameQuit()
    {
        #if UNITY_EDITOR
            EditorApplication.ExitPlaymode();
        #else
            Application.Quit();
        #endif
    }
}
