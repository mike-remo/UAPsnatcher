using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI scoreText, gameOverText, timerText;
    [SerializeField] private GameObject timerUI, gameOverUI;
    [SerializeField] private int prizeCount, scoreValue, gameMode;
    private float timerTime;
    private bool isGameOver;
    private AudioSource audio;
    [SerializeField] private AudioClip prizeSound, winSound;
    private OptionsManager optMan;

    public void PrizeWon(int prizeValue)
    {
        if (isGameOver) return;
        prizeCount -= 1;
        scoreValue += prizeValue;
        scoreText.SetText($"Score: {scoreValue}");
        if (prizeCount < 1)
        {
            GameOver();
            return;
        }
        if (!audio.isPlaying) 
            audio.PlayOneShot(prizeSound);
    }

    public void GameOver()
    {
        isGameOver = true;
        gameOverUI.SetActive(true);
        switch (gameMode)
        {
            case 1:
            {
                audio.PlayOneShot(winSound);
                break;
            }   
            case 2:
            {
                gameOverText.SetText("Times up! Final score: " + scoreValue);
                break;
            }
            default:
            {
                Debug.Log("Game Over case not expected!");
                break;
            }
        }
    }

    void Start()
    {
        prizeCount = FindObjectsByType<Prize>().Length;
        scoreValue = 0;
        gameMode = 1;
        isGameOver = false;
        
        audio = GetComponent<AudioSource>();
        if (GameObject.Find("OptionsManager"))
        {
            if (GameObject.Find("OptionsManager").TryGetComponent<OptionsManager>(out optMan))
            {
                audio.volume = optMan.options.volume;
                gameMode = optMan.gameMode;
            }   
        }

        if (gameMode == 2)
        {
            timerUI.SetActive(true);
            timerTime = 30;
        }
    }

    void Update()
    {
        if (gameMode != 2) return;
        if (timerTime < 0)
        {
            if (!isGameOver) GameOver();
            return;
        }
        timerTime -= Time.deltaTime;
        timerText.SetText("Timer: " + Mathf.CeilToInt(timerTime));
    }
} // END