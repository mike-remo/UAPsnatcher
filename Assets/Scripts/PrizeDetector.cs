using UnityEngine;

public class PrizeDetector : MonoBehaviour
{
    [SerializeField] private GameManager gameMan;
    private int destroyTimer = 5;

    void OnTriggerEnter(Collider collided)
    {
        if (collided.gameObject.TryGetComponent<Prize>(out Prize prize))
        {
            if (prize.won == false)
            {
                prize.won = true;
                gameMan.PrizeWon(prize.pointValue);
                Destroy(collided.gameObject, destroyTimer);
            }
        }
    }
} // END