using TMPro;
using UnityEngine;

public class PrizeDetector : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI scoreText;
    private int scoreValue, destroyTimer;

    void Awake()
    {
        scoreValue = 0;
        destroyTimer = 5;
    }

    void OnTriggerEnter(Collider collided)
    {
        //Debug.Log("Prize won!");
        if (collided.gameObject.TryGetComponent<Prize>(out Prize prize))
        {
            scoreValue += prize.pointValue;
            Destroy(collided.gameObject, destroyTimer);
            scoreText.SetText($"Score: {scoreValue}");
        }
    }
}
