using TMPro;
using UnityEngine;

public class PrizeDetector : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI scoreText, winText;
    [SerializeField] private int scoreValue, destroyTimer, prizeCount;
    private AudioSource audio;
    [SerializeField] private AudioClip prizeSound, winSound;
    private OptionsManager optionsManager;

    void Awake()
    {
        audio = GetComponent<AudioSource>();
        scoreValue = 0;
        destroyTimer = 5;
    }

    void Start()
    {
        if (GameObject.Find("OptionsManager"))
            if (GameObject.Find("OptionsManager").TryGetComponent<OptionsManager>(out optionsManager))
                audio.volume = optionsManager.options.volume;
        
        prizeCount = FindObjectsByType<Prize>().Length;
    }

    void OnTriggerEnter(Collider collided)
    {
        if (collided.gameObject.TryGetComponent<Prize>(out Prize prize))
        {
            if (prize.won == false)
            {
                prize.won = true;
                prizeCount -= 1;
                scoreValue += prize.pointValue;
                Destroy(collided.gameObject, destroyTimer);
            }
            if (prizeCount < 1)
            {
                winText.gameObject.SetActive(true);
                audio.PlayOneShot(winSound);
            }
            scoreText.SetText($"Score: {scoreValue}");
            if (!audio.isPlaying) 
                audio.PlayOneShot(prizeSound);
        }
    }
}
// END